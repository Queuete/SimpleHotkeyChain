using ExileCore;
using ImGuiNET;
using SimpleHotkeyChain.SettingsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleHotkeyChain
{
    public class SimpleHotkeyChain : BaseSettingsPlugin<SimpleHotkeyChainSettings>
    {
        public override void Render()
        {
            if (Settings.DisableWhenChatOpen && GameController.IngameState.IngameUi.ChatBox.IsVisible) return;

            foreach (var hotkeyChain in Settings.HotkeyChains)
            {
                if (hotkeyChain.Enable && Input.GetKeyState(hotkeyChain.Trigger?.Key))
                {
                    Input.KeyUp(hotkeyChain.Trigger?.Key);
                    Task.Run(() => RunHotkeyChain(hotkeyChain));
                }
            }
        }
        public void RunHotkeyChain(HotkeyChain hotkeyChain)
        {
            Thread.Sleep(int.Parse(hotkeyChain.Trigger.WaitAfterInMs.Value));
            foreach (var hotkey in hotkeyChain.Chain)
            {
                KeyPress(hotkey.Key, hotkey.ControlModifier.Value);
                Thread.Sleep(int.Parse(hotkey.WaitAfterInMs.Value));
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
            Thread.Sleep(3);
            Input.KeyUp(key);
        }

        public override void DrawSettings()
        {
            Settings.Draw();
        }
    }
}
