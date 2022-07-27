using System;
using System.Text;
using System.Runtime.InteropServices;

namespace RejTech.Drawing
{
    /// <summary>
    /// Module to oerate the display's Virtual Control Panel (VCP) interface via VESA Monitor Control Command Set (MCCS).
    /// This allows software operation of a display's on-screen menu controls via DDC/CI commands.
    /// </summary>
    public static class DisplayCommander
    {
        [DllImport("dxva2.dll", EntryPoint = "SetVCPFeature", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetVCPFeature([In] IntPtr hMonitor, uint dwVCPCode, uint dwNewValue);

        [DllImport("dxva2.dll", EntryPoint = "GetVCPFeatureAndVCPFeatureReply", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetVCPFeatureAndVCPFeatureReply([In] IntPtr hMonitor, uint dwVCPCode, ref IntPtr pvct, ref uint pdwCurrentValue, ref uint pdwMaximumValue);

        [DllImport("dxva2.dll", EntryPoint = "GetCapabilitiesStringLength", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCapabilitiesStringLength([In] IntPtr hMonitor, ref uint pdwCapsStringLenInChars);

        [DllImport("dxva2.dll", EntryPoint = "CapabilitiesRequestAndCapabilitiesReply", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CapabilitiesRequestAndCapabilitiesReply([In] IntPtr hMonitor, StringBuilder pszASCIICapsString, [In] uint dwCapsStringLenInChars);

        /// <summary>Check support for a specific VCP code via DDC/CI (VESA MCCS)</summary>
        /// <returns>value on success, -1 on fail</returns>
        /// <remarks>Takes several milliseconds and may block entire graphics driver until command completion</remarks>
        public static bool VCPSupported(DisplayInfo display, uint vcp)
        {
            if (display.legacyMonitorHandle == null) return false;
            return (VCPRead(display, vcp) >= 0);
        }

        /// <summary>Reads a VCP code from monitor via DDC/CI (VESA MCCS)</summary>
        /// <returns>value on success, -1 on fail</returns>
        /// <remarks>Takes several milliseconds and may block entire graphics driver until command completion</remarks>
        public static int VCPRead(DisplayInfo display, uint vcp)
        {
            uint vcpMaxValue;
            return VCPRead(display, vcp, out vcpMaxValue);
        }

        /// <summary>Reads a VCP code from monitor via DDC/CI (VESA MCCS)</summary>
        /// <returns>value on success, -1 on fail</returns>
        /// <remarks>Takes several milliseconds and may block entire graphics driver until command completion</remarks>
        public static int VCPRead(DisplayInfo display, uint vcp, out uint vcpMaxValue)
        {
            vcpMaxValue = 0;
            if (display.legacyMonitorHandle == null) return -1;
            uint vcpValue = 0;
            IntPtr dummy = IntPtr.Zero;
            if (GetVCPFeatureAndVCPFeatureReply(display.legacyMonitorHandle, vcp, ref dummy, ref vcpValue, ref vcpMaxValue))
            {
                return (int)vcpValue;
            }
            return -1;
        }

        /// <summary>Writes a VCP code to monitor via DDC/CI (VESA MCCS)</summary>
        /// <returns>true on success</returns>
        /// <remarks>Takes several milliseconds and may block entire graphics driver until command completion</remarks>
        public static bool VCPWrite(DisplayInfo display, uint vcp, uint value)
        {
            if (display.legacyMonitorHandle == null) return false;
            return SetVCPFeature(display.legacyMonitorHandle, vcp, value);
        }

        /// <summary>Gets monitor capabilities string (VESA MCCS)</summary>
        /// <returns>Monitor capabilities string</returns>
        /// <remarks>Takes approximately 1 second, and may block entire graphics driver until command completion</remarks>
        public static string VCPQueryCaps(DisplayInfo display)
        {
            if (display.legacyMonitorHandle == null) return "";
            StringBuilder monitorCaps = new StringBuilder("");
            uint capsLen = 0;
            if (GetCapabilitiesStringLength(display.legacyMonitorHandle, ref capsLen))
            {
                monitorCaps.EnsureCapacity((int)capsLen);
                CapabilitiesRequestAndCapabilitiesReply(display.legacyMonitorHandle, monitorCaps, capsLen);
            }
            return monitorCaps.ToString();
        }
    }
}
