using System;
using System.Collections.Generic;

namespace RejTech
{
    /// <summary>One configuration item for a strobed gaming monitor</summary>
    public class DisplayConfigItem : object
    {
        public string Name { get; set; } = "";
        public string Device { get; set; } = "";
        public bool Choose { get; set; } = false;
        public uint CommandEnable { get; set; } = 0;
        public uint CommandStrobeLen { get; set; } = 0;
        public uint CommandStrobePhase { get; set; } = 0;
        public uint CommandOverdriveGain { get; set; } = 0;
        public bool HasSingleStrobe60Hz { get; set; } = false;
        public List<string> StrobeEnableList { get; set; } = new List<string>();
        public uint StrobeLenMin { get; set; } = 0;
        public uint StrobeLenMax { get; set; } = 0;
        public bool StrobeLenReversed { get; set; } = false;
        public uint StrobePhaseMin { get; set; } = 0;
        public uint StrobePhaseMax { get; set; } = 0;
        public bool StrobePhaseReversed { get; set; } = false;
        public uint StrobePhaseWarnAbove { get; set; } = 0;
        public uint OverdriveGainMin { get; set; } = 0;
        public uint OverdriveGainMax { get; set; } = 0;
        public bool OverdriveGainReversed { get; set; } = false;
        public string Warning { get; set; } = "";
        public override string ToString() { return Name; }
    }

    /// <summary>Configuration file loader for supported strobed gaming monitors</summary>
    public class DisplayConfig
    {
        private const string SECTION_PREFIX = "Display";
        private IniFile configFile = new IniFile();

        /// <summary>Supported monitors loaded from config file</summary>
        public List<DisplayConfigItem> Items = new List<DisplayConfigItem>();

        /// <summary>Contructor</summary>
        public DisplayConfig()
        {
            Load();
        }

        /// <summary>Parse string number from configuration file. Supports hex via "0x" prefix, as well as common boolean words</summary>
        private uint Parse(string value)
        {
            try
            {
                value = value.Trim().ToLower();
                if (value == "")
                {
                    return 0;
                }
                else if ((value == "true") || (value == "yes") || (value == "on"))
                {
                    return 1;
                }
                else if ((value == "false") || (value == "no") || (value == "off"))
                {
                    return 0;
                }
                else if (value.StartsWith("0x"))
                {
                    return Convert.ToUInt32(value.Substring(2), 16);
                }
                return Convert.ToUInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Load configuration file of supported monitors</summary>
        public void Load()
        {
            Items.Clear();
            int section = 1;
            int strobeItem = 0;
            while (true)
            {
                string sectionName = SECTION_PREFIX + section;
                DisplayConfigItem item = new DisplayConfigItem
                {
                    Name = configFile.Read("Name", sectionName)
                };
                if (String.IsNullOrEmpty(item.Name)) break;
                item.Device = configFile.Read("Device", sectionName);
                item.Choose = 0 < Parse(configFile.Read("Choose", sectionName));
                item.CommandEnable = Parse(configFile.Read("CommandEnable", sectionName));
                item.CommandStrobeLen = Parse(configFile.Read("CommandStrobeLen", sectionName));
                item.CommandStrobePhase = Parse(configFile.Read("CommandStrobePhase", sectionName));
                item.CommandOverdriveGain = Parse(configFile.Read("CommandOverdriveGain", sectionName));
                // Strobe settings as strings
                strobeItem = 0;
                while (true)
                {
                    string strobeName = configFile.Read("StrobeEnable" + strobeItem.ToString(), sectionName);
                    if (String.IsNullOrEmpty(strobeName)) break;
                    item.StrobeEnableList.Add(strobeName);
                    strobeItem++;
                }
                item.HasSingleStrobe60Hz = 0 < Parse(configFile.Read("HasSingleStrobe60Hz", sectionName));
                item.StrobeLenMin = Parse(configFile.Read("StrobeLenMin", sectionName));
                item.StrobeLenMax = Parse(configFile.Read("StrobeLenMax", sectionName));
                item.StrobeLenReversed = 0 < Parse(configFile.Read("StrobeLenReversed", sectionName));
                item.StrobePhaseMin = Parse(configFile.Read("StrobePhaseMin", sectionName));
                item.StrobePhaseMax = Parse(configFile.Read("StrobePhaseMax", sectionName));
                item.StrobePhaseReversed = 0 < Parse(configFile.Read("StrobePhaseReversed", sectionName));
                item.StrobePhaseWarnAbove = Parse(configFile.Read("StrobePhaseWarnAbove", sectionName));
                item.OverdriveGainMin = Parse(configFile.Read("OverdriveGainMin", sectionName));
                item.OverdriveGainMax = Parse(configFile.Read("OverdriveGainMax", sectionName));
                item.OverdriveGainReversed = 0 < Parse(configFile.Read("OverdriveGainReversed", sectionName));
                item.Warning = configFile.Read("Warning", sectionName);
                Items.Add(item);
                section++;
            }
        }
    }
}
