using ExileCore.Shared.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHotkeyChain.SettingsModels
{
    public class Hotkey
    {
        public HotkeyNode Key { get; set; } = new HotkeyNode();
        public TextNode WaitAfterInMs { get; set; } = new TextNode("0");
        public ToggleNode ControlModifier { get; set; } = new ToggleNode(false);
    }
}
