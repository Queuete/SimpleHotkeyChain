using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using SimpleHotkeyChain.SettingsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHotkeyChain
{
    public class SimpleHotkeyChainSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public ToggleNode DisableWhenChatOpen { get; set; } = new ToggleNode(true);
        public List<HotkeyChain> HotkeyChains { get; set; } = new List<HotkeyChain>();

        public void Draw()
        {
            DisableWhenChatOpen.Value = ImGuiExtension.Checkbox("Disable when Chat open", DisableWhenChatOpen.Value);
            ImGuiExtension.ToolTipWithText("(new League?)", "You may want to disable that after new league start, when Offsets are not fully updated yet.");
            ImGui.NewLine();

            int chainCounter = 1;
            foreach (var hotkeyChain in HotkeyChains)
            {
                if (ImGui.TreeNodeEx($"Hotkey Chain {chainCounter} Options", ImGuiTreeNodeFlags.CollapsingHeader))
                {
                    DrawHotkeyChain(hotkeyChain, chainCounter);
                }
                chainCounter++;
            }

            ImGui.NewLine();
            ImGui.Separator();

            var chainCount = HotkeyChains.Count;
            if (ImGui.Button("HotkeyChain +") && chainCount < 10)
            {
                HotkeyChains.Add(new HotkeyChain());
            }
            ImGui.SameLine();
            if (ImGui.Button("HotkeyChain -") && chainCount > 1)
            {
                HotkeyChains.RemoveAt(chainCount - 1);
            }
            ImGui.Separator();
        }

        private void DrawHotkeyChain(HotkeyChain hotkeyChain, int chainCounter)
        {
            int hotkeyCounter = 0;
            hotkeyChain.Enable.Value = ImGuiExtension.Checkbox("Enable", hotkeyChain.Enable);
            ImGui.Separator();
            DrawHotkey(hotkeyChain.Trigger, "Trigger Key", chainCounter.ToString() + hotkeyCounter.ToString(), false);
            ImGui.Separator();

            hotkeyCounter++;
            foreach (var hotkey in hotkeyChain.Chain)
            {
                DrawHotkey(hotkey, "Key", chainCounter.ToString() + hotkeyCounter.ToString(), true);
                ImGui.Separator();
                hotkeyCounter++;
            }

            var keyCount = hotkeyChain.Chain.Count;
            ImGui.PushID(chainCounter.ToString() + "buttons");
            if (ImGui.Button("Key +") && keyCount < 20)
            {
                hotkeyChain.Chain.Add(new Hotkey());
            }
            ImGui.SameLine();
            if (ImGui.Button("Key -") && keyCount > 1)
            {
                hotkeyChain.Chain.RemoveAt(keyCount - 1);
            }
            ImGui.PopID();

            ImGui.Separator();
        }

        private void DrawHotkey(Hotkey hotkey, string keyText, string id, bool withControlModifier)
        {
            ImGui.PushID(id);
            hotkey.Key.Value = ImGuiExtension.HotkeySelector($"{keyText} {hotkey.Key.Value}", hotkey.Key.Value);
            ImGui.SameLine();
            ImGui.PushItemWidth(50);
            hotkey.WaitAfterInMs.Value = ImGuiExtension.InputText("Wait (ms)", hotkey.WaitAfterInMs.Value, 6, ImGuiInputTextFlags.CharsDecimal);
            ImGui.PopItemWidth();
            ImGui.SameLine();
            hotkey.ControlModifier.Value = ImGuiExtension.Checkbox("Crtl", hotkey.ControlModifier.Value);
            ImGui.PopID();
        }
    }
}
