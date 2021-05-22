using ExileCore;
using ExileCore.Shared;
using SimpleHotkeyChain.SettingsModels;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace SimpleHotkeyChain
{
    public class SimpleHotkeyChain : BaseSettingsPlugin<SimpleHotkeyChainSettings>
    {
        public override Job Tick()
        {
            // ToDo consider calculating wait time with delays in settings in mind
            return new Job("SimpleHotkeyChain", RunHotkeyChains, 5000);
        }

        private void RunHotkeyChains()
        {
            if (Settings.DisableWhenChatOpen && GameController.IngameState.IngameUi.ChatBoxRoot.IsVisible) return;

            foreach (var hotkeyChain in Settings.HotkeyChains)
            {
                if (hotkeyChain.Enable && Input.GetKeyState(hotkeyChain.Trigger?.Key))
                {
                    Input.KeyUp(hotkeyChain.Trigger?.Key);
                    RunHotkeyChain(hotkeyChain);
                }
            }
        }

        private void RunHotkeyChain(HotkeyChain hotkeyChain)
        {
            Thread.Sleep(int.Parse(hotkeyChain.Trigger.WaitAfterInMs.Value));
            foreach (var hotkey in hotkeyChain.Chain)
            {
                KeyPress(hotkey.Key, hotkey.ControlModifier.Value);
                var waitTime = int.Parse(hotkey.WaitAfterInMs.Value);
                if (waitTime > 0) Thread.Sleep(waitTime);
            }
        }

        private void KeyPress(Keys key, bool ControlModifier)
        {
            if (ControlModifier)
            {
                Input.KeyDown(Keys.LControlKey);
                KeyPress(key);
                Input.KeyUp(Keys.LControlKey);
            }
            else
            {
                KeyPress(key);
            }
        }

        private void KeyPress(Keys key)
        {
            Input.KeyDown(key);
            Thread.Sleep(2);
            Input.KeyUp(key);
        }

        public override void DrawSettings()
        {
            Settings.Draw();
        }
    }
}
