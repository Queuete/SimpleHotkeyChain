using ExileCore;
using ExileCore.Shared;
using SimpleHotkeyChain.SettingsModels;
using System.Collections;
using System.Windows.Forms;

namespace SimpleHotkeyChain
{
    public class SimpleHotkeyChain : BaseSettingsPlugin<SimpleHotkeyChainSettings>
    {
        public override void Render()
        {
            if (Settings.DisableWhenChatOpen && GameController.IngameState.IngameUi.ChatBoxRoot.IsVisible) return;

            var coroutineWorker = new Coroutine(RunHotkeyChains(), this, "SimpleHotkeyChain.RunHotkeyChains");
            Core.MainRunner.Run(coroutineWorker);
        }

        private IEnumerator RunHotkeyChains()
        {
            foreach(var hotkeyChain in Settings.HotkeyChains)
            {
                if (hotkeyChain.Enable && Input.GetKeyState(hotkeyChain.Trigger?.Key))
                {
                    Input.KeyUp(hotkeyChain.Trigger?.Key);
                    yield return RunHotkeyChain(hotkeyChain);
                }
            }
        }

        private IEnumerator RunHotkeyChain(HotkeyChain hotkeyChain)
        {
            yield return new WaitTime(int.Parse(hotkeyChain.Trigger.WaitAfterInMs.Value));
            foreach (var hotkey in hotkeyChain.Chain)
            {
                yield return KeyPress(hotkey.Key, hotkey.ControlModifier.Value);
                var waitTime = int.Parse(hotkey.WaitAfterInMs.Value);
                if (waitTime > 0) yield return new WaitTime(waitTime);
            }
        }

        private IEnumerator KeyPress(Keys key, bool ControlModifier)
        {
            if (ControlModifier)
            {
                Input.KeyDown(Keys.LControlKey);
                yield return KeyPress(key);
                Input.KeyUp(Keys.LControlKey);
            }
            else
            {
                yield return KeyPress(key);
            }
        }

        private IEnumerator KeyPress(Keys key)
        {
            Input.KeyDown(key);
            yield return new WaitTime(0);
            Input.KeyUp(key);
        }

        public override void DrawSettings()
        {
            Settings.Draw();
        }
    }
}
