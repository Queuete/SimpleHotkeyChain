using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHotkeyChain.SettingsModels
{
    public class HotkeyChain : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);
        public Hotkey Trigger { get; set; } = new Hotkey();
        public List<Hotkey> Chain { get; set; } = new List<Hotkey>();
    }
}
