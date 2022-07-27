using System.Threading;
using RejTech.Drawing;

namespace RejTech
{
    /// <summary>
    /// General class to modify the strobe features of a monitor with strobe adjustments (typically BenQ/ZOWIE gaming monitors).
    /// </summary>
    class DisplayCommander
    {
        // Default values for XG350R-C firmware
        private const uint DEFAULT_VCP_STROBE_ENABLE = 0x40;
        private const uint DEFAULT_VCP_STROBE_LEN = 0x41;
        private const uint DEFAULT_VCP_STROBE_PHASE = 0x42;
        private const uint DEFAULT_VCP_OVERDRIVE = 0x43;

        // Cached current values
        private int valEnable = -1;
        private int valStrobeLen = -1;
        private int valStrobePhase = -1;
        private int valOverdriveGain = -1;

        // Backed up values for rollback reset
        private const int BACKUP_ROLLBACK_RETRIES = 5;
        private bool backupValid = false;
        private int backupRollbackCounter = 0;
        private int backupOverride = -1;
        private int backupStrobeLen = -1;
        private int backupStrobePhase = -1;
        private int backupOverdriveGain = -1;
        
        /// <summary>Set TRUE to force direct re-read of DDC</summary>
        public bool ForceRefresh { get; set; } = false;

        /// <summary>Currently selected display</summary>
        public DisplayInfo CurrentDisplay { get; set; } = null;

        public bool Changed { get; set; } = false;

        /// <summary>VCP command code for override</summary>
        public uint VCPcodeStrobeEnable { get; set; } = DEFAULT_VCP_STROBE_ENABLE;

        /// <summary>VCP command code for strobe length</summary>
        public uint VCPcodeStrobeLen { get; set; } = DEFAULT_VCP_STROBE_LEN;

        /// <summary>VCP command code for strobe phase</summary>
        public uint VCPcodeStrobePhase { get; set; } = DEFAULT_VCP_STROBE_PHASE;

        /// <summary>VCP command code for overdrive gain
        public uint VCPcodeOverdriveGain { get; set; } = DEFAULT_VCP_OVERDRIVE;

        /// <summary>Failed flag if last read/write operation failed</summary>
        public bool Failed { get; set; } = false;

        /// <summary>Gets/sets override setting on monitor</summary>
        public int Enable
        {
            get
            {
                if ((CurrentDisplay != null) && (valEnable == -1) || ForceRefresh)
                {
                    valEnable = Drawing.DisplayCommander.VCPRead(CurrentDisplay, VCPcodeStrobeEnable);
                    if (valEnable == -1) Failed = true;
                }
                ForceRefresh = false;
                return valEnable;
            }
            set
            {
                if (CurrentDisplay != null)
                {
                    Failed = !Drawing.DisplayCommander.VCPWrite(CurrentDisplay, VCPcodeStrobeEnable, (uint)value);
                    Changed = true;
                    if (!Failed) valEnable = value;
                }
            }
        }
                                                                       
        /// <summary>Gets/sets strobe length on monitor</summary>
        public int StrobeLen
        {
            get
            {
                if ((CurrentDisplay != null) && (valStrobeLen == -1) || ForceRefresh)
                {
                    valStrobeLen = Drawing.DisplayCommander.VCPRead(CurrentDisplay, VCPcodeStrobeLen);
                    if (valEnable == -1) Failed = true;
                }
                ForceRefresh = false;
                return valStrobeLen;
            }
            set
            {
                if (CurrentDisplay != null)
                {
                    Failed = !Drawing.DisplayCommander.VCPWrite(CurrentDisplay, VCPcodeStrobeLen, (uint)value);
                    Changed = true;
                    if (!Failed) valStrobeLen = value;
                }
            }
        }

        /// <summary>Gets/sets strobe phase on monitor</summary>
        public int StrobePhase
        {
            get
            {
                if ((CurrentDisplay != null) && (valStrobePhase == -1) || ForceRefresh)
                {
                    valStrobePhase = Drawing.DisplayCommander.VCPRead(CurrentDisplay, VCPcodeStrobePhase);
                    if (valEnable == -1) Failed = true;
                }
                ForceRefresh = false;
                return valStrobePhase;
            }
            set
            {
                if (CurrentDisplay != null)
                {
                    Failed = !Drawing.DisplayCommander.VCPWrite(CurrentDisplay, VCPcodeStrobePhase, (uint)value);
                    Changed = true;
                    if (!Failed) valStrobePhase = value;
                }
            }
        }

        /// <summary>Gets/sets overdrive on monitor</summary>
        public int OverdriveGain
        {
            get
            {
                if ((CurrentDisplay != null) && (valOverdriveGain == -1) || ForceRefresh)
                {
                    valOverdriveGain = Drawing.DisplayCommander.VCPRead(CurrentDisplay, VCPcodeOverdriveGain);
                    if (valEnable == -1) Failed = true;
                }
                ForceRefresh = false;
                return valOverdriveGain;
            }
            set
            {
                if (CurrentDisplay != null)
                {
                    Failed = !Drawing.DisplayCommander.VCPWrite(CurrentDisplay, VCPcodeOverdriveGain, (uint)value);
                    Changed = true;
                    if (!Failed) valOverdriveGain = value;
                }
            }
        }

        /// <summary>Query display for current strobe settings. Cached for speed.</summary>
        /// <param name="display">Display to query</param>
        /// <returns>true if successfully queried</returns>
        public bool QuerySettings(DisplayInfo display, bool force = false)
        {
            if (display == null) return false;
            if ((CurrentDisplay == null) || (CurrentDisplay.ID != display.ID) || ForceRefresh || force)
            {
                // Performance: Cached is faster, as first read is slow
                Failed = false;
                CurrentDisplay = display;
                valEnable =  (VCPcodeStrobeEnable > 0) ? Drawing.DisplayCommander.VCPRead(display, VCPcodeStrobeEnable) : 0;
                valStrobeLen = (VCPcodeStrobeLen > 0) ? Drawing.DisplayCommander.VCPRead(display, VCPcodeStrobeLen) : 0;
                valStrobePhase = (VCPcodeStrobePhase > 0) ? Drawing.DisplayCommander.VCPRead(display, VCPcodeStrobePhase) : 0;
                valOverdriveGain = (VCPcodeOverdriveGain > 0) ? Drawing.DisplayCommander.VCPRead(display, VCPcodeOverdriveGain) : 0;
                Changed = false;
                backupOverride = valEnable;
                backupStrobeLen = valStrobeLen;
                backupStrobePhase = valStrobePhase;
                backupOverdriveGain = valOverdriveGain;
                backupRollbackCounter = 0;
                backupValid = !Failed;
                ForceRefresh = false;
            }
            return ((valEnable >= 0) && (valStrobeLen >= 0) && (valStrobePhase >= 0) && (valOverdriveGain >= 0));
        }

        /// <summary>Clear cached settings</summary>
        public bool ClearCache()
        {
            CurrentDisplay = null;
            Failed = false;
            Changed = false;
            return true;
        }

        /// <summary>Reset settings to last known safe settings</summary>
        /// <remarks>When the XL2720Z Version 2 is running with Large Vertical Totals, it can really bug out (black screen) with some large 
        /// strobe phase settings. This Rollback feature is designed to accomodate recovery from such a stessful situation as a monitor blacking out.</remarks>
        public bool RollbackSettings()
        {
            if (!backupValid || (backupRollbackCounter > 0) || (CurrentDisplay == null)) return false;
            backupRollbackCounter = BACKUP_ROLLBACK_RETRIES;
            while (backupRollbackCounter > 0)
            {
                Failed = false;
                Enable = backupOverride;
                StrobeLen = backupStrobeLen;
                StrobePhase = backupStrobePhase;
                OverdriveGain = backupOverdriveGain;
                if (!Failed) break;
                Thread.Sleep(1000); // Hopefully never happens but we must try hard to recover from nasty situations.
                backupRollbackCounter--;
            }
            backupRollbackCounter = 0;
            return !Failed;
        }

        /// <summary>Commit currently adjusted settings as being safe</summary>
        public bool CommitSettings()
        {
            if (CurrentDisplay == null) return false;
            if ((valEnable >= 0) && (valStrobeLen >= 0) && (valStrobePhase >= 0))
            {
                backupOverride = valEnable;
                backupStrobeLen = valStrobeLen;
                backupStrobePhase = valStrobePhase;
                backupOverdriveGain = valOverdriveGain;
                backupValid = true;
                Changed = false;
                return true;
            }
            return false;
        }
    }
}
