using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RejTech.Drawing
{
    /// <summary>
    /// Easy display enumerator that gives information about all displays that are connected.
    /// </summary>
    /// 
    /// <remarks>For future cross-platform portability, the public methods are named in a generally platform independent way 
    /// and also applicable to mobile displays (laptops, smartphones) and code-improvements-compatible for variable refresh rate display tech</remarks>
    public class DisplayInfo : object
    {
        /// <summary>Accessing this object as a string, accesses the friendly name</summary>
        public override String ToString()
        {
            return (FriendlyName != "") ? FriendlyName : DeviceProductID;
        }

        /// <summary>User-friendly display name, e.g. "DELL 2005FPW"</summary>
        public String FriendlyName { get; internal set; } = "";

        /// <summary>Unique ID for display connection.. Useful for keeping track of a specific still-plugged-in monitor while re-enumerating (e.g. hot unplug/plug events)</summary>
        public uint ID { get; internal set; } = 0;

        /// <summary>Monitor number in multimonitor, indexed beginning at 1</summary>
        public uint Index { get; internal set; } = 0;

        /// <summary>Horizontal resolution of output</summary>
        public uint Width { get; internal set; } = 0;

        /// <summary>Vertical resolution of output</summary>
        public uint Height { get; internal set; } = 0;

        /// <summary>Bits per pixel</summary>
        public uint BitsPerPixel { get; internal set; } = 0;

        /// <summary>Video signal timings: Refresh Rate of output. For VRR, this will be the current max-Hz</summary>
        public double RefreshRate { get; internal set; } = 0;

        /// <summary>Video signal timings: Horizontal scan rate of output.</summary>
        public double HorizScanRate { get; internal set; } = 0;

        /// <summary>X position of upper-left corner in multimonitor setup</summary>
        public int PositionX { get; internal set; } = 0;

        /// <summary>Y position of upper-left corner in multimonitor setup</summary>
        public int PositionY { get; internal set; } = 0;

        /// <summary>Video signal timings: Horizontal active</summary>
        public uint HorizActive { get; internal set; } = 0;

        /// <summary>Video signal timings: Vertical active</summary>
        public uint VertActive { get; internal set; } = 0;

        /// <summary>Video signal timings: Horizontal total</summary>
        public uint HorizTotal { get; internal set; } = 0;

        /// <summary>Video signal timings: Vertical total</summary>
        public uint VertTotal { get; internal set; } = 0;

        /// <summary>Pixel clock of video signal.  This is a 64-bit value, but many video standards have a dotclock limit (e.g. HDMI 2.0 has a 600 MHz dotclock limit</summary>
        public ulong PixelClock { get; internal set; } = 0;

        /// <summary>Is GPU Scaling occuring? It is possible for Width/Height to be different from actual display HorizActive/VertActive</summary>
        public bool IsGPUScaled { get; internal set; } = false;

        /// <summary>Returns true of display is rotated</summary>
        public bool IsRotated { get; internal set; } = false;

        /// <summary>Is this monitor primary?</summary>
        public bool IsPrimary { get; internal set; } = false;

        /// <summary>Is the output interlaced?</summary>
        public bool IsInterlaced { get; internal set; } = false;

        /// <summary>Name of video port technology in use, such as "HDMI", "DisplayPort", "VGA", "DVI", "Miracast", "Internal" (laptop), etc.</summary>
        public string VideoPort { get; internal set; } = "";

        /// <summary>Device name of display such as "\.\\DISPLAY1"</summary>
        public String DeviceName { get; internal set; } = "";

        /// <summary>Device path usually platform specific. Such as "\\?\DISPLAY#DEL85A3#5&b71639a9&1&UID6478#{4F0B6237-C007-4F8A-B9EC-0A5520066BB2}" (on Windows)</summary>
        public string DevicePath { get; internal set; } = "";

        /// <summary>Plug-n-Play Product ID built into the display, such as "DEL85A3" or "ACI27F8". 
        /// First three characters defines monitor chipset brand (or monitor manufacturer) via http://www.uefi.org/pnp_id_list </summary>
        public string DeviceProductID { get; internal set; } = "";

        // INTERNAL USE ONLY (platform-specific)
        internal WindowsAPI.DISPLAYCONFIG_PATH_INFO displayPath;
        internal WindowsAPI.DISPLAYCONFIG_MODE_INFO displayModeSource;
        internal WindowsAPI.DISPLAYCONFIG_MODE_INFO displayMode;
        internal WindowsAPI.DISPLAYCONFIG_TARGET_DEVICE_NAME targetDeviceNameInfo;
        internal WindowsAPI.MONITORINFOEX legacyMonitorInfo;
        internal IntPtr legacyMonitorHandle = IntPtr.Zero;
    }

    /// <summary>Easy enumeratable list of displays attached to this system</summary>
    public class Displays : IEnumerable<DisplayInfo>
    {
        // INTERNAL USE ONLY (platform-specific)
        private WindowsAPI.DISPLAYCONFIG_PATH_INFO[] displayPaths;
        private WindowsAPI.DISPLAYCONFIG_MODE_INFO[] displayModes;
        private List<WindowsAPI.MONITORINFOEX> legacyLogicalMonitors = new List<WindowsAPI.MONITORINFOEX>();

        /// <summary>List of monitors found</summary>
        private List<DisplayInfo> displays = new List<DisplayInfo>();

#if DEBUG
        private bool LogEnabled { get; set; } = true;
#else
        private bool LogEnabled { get; set; } = false;
#endif
        private string LogFile { get; set; } = "";

        /// <summary>Constructor</summary>
        public Displays()
        {
            Enumerate();
        }

        /// <summary>Enumerator</summary>
        public IEnumerator<DisplayInfo> GetEnumerator()
        {
            return displays.GetEnumerator();
        }

        /// <summary>Enumerator</summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return displays.GetEnumerator();
        }

        /// <summary>Count of displays enumerated</summary>
        public int Count
        {
            get { return displays.Count; }
        }

        /// <summary>Return display at specified index</summary>
        public DisplayInfo this[int i]
        {
            get { return displays[i]; }
        }

        /// <summary>The primary monitor of a multi-monitor setup. This is not necessarily the first item in the displays List</summary>
        public DisplayInfo PrimaryDisplay { get; internal set; } = null;

        /// <summary>Is multimonitor in surround mode?</summary>
        /// <remarks>Triple gaming monitor setups will often have this enabled.  Multi monitor setups are not seamless surround mode by default.</remarks>
        public bool IsSurroundMode
        {
            get
            {
                return (displays.Count > 0) && (displays.Count != legacyLogicalMonitors.Count);
            }
        }

        /// <summary>Enumerates all monitors on the system</summary>
        /// <remarks>Should call this during resolution-change events & hotplug events, to refresh the list of displays</remarks>
        public int Enumerate()
        {
            displays.Clear();

            uint pathCount, modeCount;
            int status = WindowsAPI.GetDisplayConfigBufferSizes(WindowsAPI.QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, out pathCount, out modeCount);
            if (status != WindowsAPI.ERROR_SUCCESS) return -1;

            displayPaths = new WindowsAPI.DISPLAYCONFIG_PATH_INFO[pathCount];
            displayModes = new WindowsAPI.DISPLAYCONFIG_MODE_INFO[modeCount];
            status = WindowsAPI.QueryDisplayConfig(WindowsAPI.QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, ref pathCount, displayPaths, ref modeCount, displayModes, IntPtr.Zero);
            if (status != WindowsAPI.ERROR_SUCCESS) return -1;

            for (uint i = 0; i < pathCount; i++)
            {
                DisplayInfo display = new DisplayInfo();

                display.Index = (i + 1);
                display.legacyMonitorHandle = (IntPtr)i;
                display.displayPath = displayPaths[i];
                display.displayModeSource = displayModes[display.displayPath.sourceInfo.modeInfoIdx];
                display.displayMode = displayModes[display.displayPath.targetInfo.modeInfoIdx];

                WindowsAPI.DISPLAYCONFIG_VIDEO_SIGNAL_INFO timings = display.displayMode.modeInfo.targetMode.targetVideoSignalInfo;
                display.ID = display.displayPath.targetInfo.id;
                display.Width = display.displayModeSource.modeInfo.sourceMode.width;
                display.Height = display.displayModeSource.modeInfo.sourceMode.height;
                display.PositionX = display.displayModeSource.modeInfo.sourceMode.position.x;
                display.PositionY = display.displayModeSource.modeInfo.sourceMode.position.y;

                display.HorizActive = timings.activeSize.cx;
                display.VertActive = timings.activeSize.cy;
                display.HorizTotal = timings.totalSize.cx;
                display.VertTotal = timings.totalSize.cy;
                display.PixelClock = timings.pixelRate;
                display.RefreshRate = (double)timings.vSyncFreq.Numerator / (double)timings.vSyncFreq.Denominator;
                display.HorizScanRate = (double)timings.hSyncFreq.Numerator / (double)timings.hSyncFreq.Denominator;

                display.VideoPort = PlatformGetFriendlyOutputName(display.displayPath.targetInfo.outputTechnology);
                display.BitsPerPixel = PlatformGetBitsPerPixels(display.displayModeSource.modeInfo.sourceMode.pixelFormat);

                display.IsRotated = (display.displayPath.targetInfo.rotation != WindowsAPI.DISPLAYCONFIG_ROTATION.DISPLAYCONFIG_ROTATION_IDENTITY);

                display.IsInterlaced = (display.displayPath.targetInfo.scanLineOrdering == WindowsAPI.DISPLAYCONFIG_SCANLINE_ORDERING.DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED) ||
                                       (display.displayPath.targetInfo.scanLineOrdering == WindowsAPI.DISPLAYCONFIG_SCANLINE_ORDERING.DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_LOWERFIELDFIRST) ||
                                       (display.displayPath.targetInfo.scanLineOrdering == WindowsAPI.DISPLAYCONFIG_SCANLINE_ORDERING.DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_UPPERFIELDFIRST);

                display.IsGPUScaled = (display.displayPath.targetInfo.scaling != WindowsAPI.DISPLAYCONFIG_SCALING.DISPLAYCONFIG_SCALING_IDENTITY) ||
                                      (display.HorizActive != display.Width) ||
                                      (display.VertActive != display.Height);

                PlatformGetDeviceName(display);
                PlatformGetFriendlyName(display);

                LogDisplay(display);
                displays.Add(display);
            }

            // Enumerate the legacy logical monitors
            PrimaryDisplay = null;
            WindowsAPI.MonitorEnumProc enumProc = LegacyMonitorEnumCallback;
            WindowsAPI.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, enumProc, IntPtr.Zero);
            if ((PrimaryDisplay == null) && (displays.Count > 0)) PrimaryDisplay = displays[0];
            return (int)pathCount;
        }

        /// <summary>Number of bits per pixel</summary>
        private uint PlatformGetBitsPerPixels(WindowsAPI.DISPLAYCONFIG_PIXELFORMAT pixelFormat)
        {
            switch (pixelFormat)
            {
                case WindowsAPI.DISPLAYCONFIG_PIXELFORMAT.DISPLAYCONFIG_PIXELFORMAT_32BPP: return 32;
                case WindowsAPI.DISPLAYCONFIG_PIXELFORMAT.DISPLAYCONFIG_PIXELFORMAT_24BPP: return 24;
                case WindowsAPI.DISPLAYCONFIG_PIXELFORMAT.DISPLAYCONFIG_PIXELFORMAT_16BPP: return 16;
                case WindowsAPI.DISPLAYCONFIG_PIXELFORMAT.DISPLAYCONFIG_PIXELFORMAT_8BPP: return 8;
            }
            return 0;
        }

        /// <summary>Friendly video output names such as "VGA", "HDMI", "DisplayPort"</summary>
        private string PlatformGetFriendlyOutputName(WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology)
        {
            switch (outputTechnology)
            {
                // Digital connections
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EXTERNAL: return "DisplayPort";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HDMI: return "HDMI";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DVI: return "DVI";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDI: return "SDI";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_MIRACAST: return "Miracast";

                // Analog connections
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HD15: return "VGA";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPONENT_VIDEO: return "Component";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPOSITE_VIDEO: return "Composite";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SVIDEO: return "S-Video";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDTVDONGLE: return "SDTV Dongle";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_D_JPN: return "D-Terminal";

                // Internal connections (laptop, tablet, AIO)
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_INTERNAL: return "Internal";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EMBEDDED: return "Internal DP";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EMBEDDED: return "Internal UDI";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_LVDS: return "Internal LVDS";
                case WindowsAPI.DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY.DISPLAYCONFIG_OUTPUT_TECHNOLOGY_OTHER: return "Other";
            }
            return String.Format("Unknown ({0})", outputTechnology);
        }

        /// <summary>Get GDI device name</summary>
        /// <remarks>Based from https://social.msdn.microsoft.com/Forums/en-US/70193623-e506-4723-999d-27bdfbb43584/ </remarks>
        private string PlatformGetDeviceName(DisplayInfo display)
        {
            WindowsAPI.DISPLAYCONFIG_SOURCE_DEVICE_NAME sourceDevice = new WindowsAPI.DISPLAYCONFIG_SOURCE_DEVICE_NAME();
            sourceDevice.header.size = (uint)Marshal.SizeOf(typeof(WindowsAPI.DISPLAYCONFIG_SOURCE_DEVICE_NAME));
            sourceDevice.header.adapterId = display.displayModeSource.adapterId;
            sourceDevice.header.id = display.displayModeSource.id;
            sourceDevice.header.type = WindowsAPI.DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME;
            int status = WindowsAPI.DisplayConfigGetDeviceInfo_Source(ref sourceDevice);
            display.DeviceName = (status == WindowsAPI.ERROR_SUCCESS) ? sourceDevice.gdiDeviceName : "";
            return display.DeviceName;
        }

        /// <summary>Get monitor friendly name</summary>
        /// <remarks>Based from https://stackoverflow.com/questions/4958683/how-do-i-get-the-actual-monitor-name-as-seen-in-the-resolution-dialog </remarks>
        private string PlatformGetFriendlyName(DisplayInfo display)
        {
            display.FriendlyName = "";
            WindowsAPI.DISPLAYCONFIG_TARGET_DEVICE_NAME targetDevice = new WindowsAPI.DISPLAYCONFIG_TARGET_DEVICE_NAME();
            targetDevice.header.size = (uint)Marshal.SizeOf(typeof(WindowsAPI.DISPLAYCONFIG_TARGET_DEVICE_NAME));
            targetDevice.header.adapterId = display.displayMode.adapterId;
            targetDevice.header.id = display.displayMode.id;
            targetDevice.header.type = WindowsAPI.DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME;
            int status = WindowsAPI.DisplayConfigGetDeviceInfo_Target(ref targetDevice);
            if (status == WindowsAPI.ERROR_SUCCESS)
            {
                // Get Monitor friendly name & PNP Device Path
                display.targetDeviceNameInfo = targetDevice;
                display.FriendlyName = targetDevice.monitorFriendlyDeviceName;
                display.DevicePath = display.targetDeviceNameInfo.monitorDevicePath;

                // Get monitor Product ID
                int hash = display.DevicePath.IndexOf('#');
                if (hash > 0)
                {
                    display.DeviceProductID = display.DevicePath.Substring(hash + 1);
                    hash = display.DeviceProductID.IndexOf("#");
                    if (hash > 0) display.DeviceProductID = display.DeviceProductID.Substring(0, hash);
                }
            }
            return display.FriendlyName;
       }

        /// <summary>Display debug log output for display</summary>
        private void LogDisplay(DisplayInfo display)
        {
            string logText = "PHYSICAL MONITOR (" + display.Index + "):\n" +
                                "  Name = " + display.FriendlyName + "\n" +
                                "  Product ID = " + display.DeviceProductID + "\n" +
                                "  Device Name = " + display.DeviceName + "\n" +
                                "  Device Path = " + display.DevicePath + "\n" +
                                "  Video Output = " + display.VideoPort + "\n" +
                                "  Width = " + display.Width + "\n" +
                                "  Height = " + display.Height + "\n" +
                                "  Color Depth = " + display.BitsPerPixel + "\n" +
                                "  Horiz Active = " + display.HorizActive + "\n" +
                                "  Vert Active = " + display.VertActive + "\n" +
                                "  Horiz Total = " + display.HorizTotal + "\n" +
                                "  Vert Total = " + display.VertTotal + "\n" +
                                "  Refresh Rate = " + display.RefreshRate + "\n" +
                                "  Horiz Scan Rate = " + display.HorizScanRate + "\n" +
                                "  Pixel Clock = " + display.PixelClock + "\n" +
                                "  Is GPU Scaled = " + display.IsGPUScaled + "\n" +
                                "  Is Interlaced = " + display.IsInterlaced + "\n" +
                                "  Is Rotated = " + display.IsRotated + "\n" +
                                "  Is Primary = " + display.IsPrimary + "\n";
            Console.Write(logText);
            if (LogEnabled)
            {
                if (LogFile == "") LogFile = AppDomain.CurrentDomain.BaseDirectory + "monitors.log";
                logText = logText.Replace("\n", "\r\n");
                System.IO.StreamWriter stream = System.IO.File.AppendText(LogFile);
                stream.WriteLine(logText);
                stream.Close();
            }
        }

        /// <summary>Callback for enumerating logical monitors</summary>
        private bool LegacyMonitorEnumCallback(IntPtr hMonitor, IntPtr hdcMonitor, ref WindowsAPI.Rect rectMonitor, IntPtr data)
        {
            legacyLogicalMonitors.Clear();
            WindowsAPI.MONITORINFOEX mi = new WindowsAPI.MONITORINFOEX();
            mi.Size = (uint)Marshal.SizeOf(mi);
            bool success = WindowsAPI.GetMonitorInfo(hMonitor, ref mi);

            // TODO: May need to handle multiple physical monitors attached to logical. Historically this worked to get individual monitors in surround monitors.
            //   success = WindowsAPI.GetNumberOfPhysicalMonitorsFromHMONITOR(hLogicalMonitor, ref numMonitors);
            //   WindowsAPI.PHYSICAL_MONITOR[] physicalMonitorsArray = new WindowsAPI.PHYSICAL_MONITOR[numMonitors];
            //   success = WindowsAPI.GetPhysicalMonitorsFromHMONITOR(hLogicalMonitor, numMonitors, physicalMonitorsArray);
            uint numMonitors = 0;
            success = WindowsAPI.GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, ref numMonitors);
            WindowsAPI.PHYSICAL_MONITOR[] physicalMonitorsArray = new WindowsAPI.PHYSICAL_MONITOR[numMonitors];
            success = WindowsAPI.GetPhysicalMonitorsFromHMONITOR(hMonitor, numMonitors, physicalMonitorsArray);

            if (success)
            {
                for (int i = 0; i < displays.Count; i++)
                {
                    if (displays[i].DeviceName == mi.DeviceName)
                    {
                        displays[i].legacyMonitorHandle = physicalMonitorsArray[0].hPhysicalMonitor;
                        displays[i].legacyMonitorInfo = mi;
                        if (0 != (mi.Flags & WindowsAPI.MONITORINFOF_PRIMARY))
                        {
                            displays[i].IsPrimary = true;
                            if (PrimaryDisplay == null) PrimaryDisplay = displays[i];
                        }
                    }
                }
                legacyLogicalMonitors.Add(mi);
            }
            return true;
        }
    }

    /// <summary>Internal class for Windows-specific calls</summary>
    /// <remarks>
    /// System DLLs: user32.dll and dxva2.dll
    /// NOTE: Inspiration from StackOverflow threads:
    /// http://stackoverflow.com/questions/846518/getphysicalmonitorsfromhmonitor-returned-handle-is-always-null
    /// TODO: NVIDIA Timngs & Resolution API can grab further (non-generically-obtainable) information such as Horizontal Porch, Sync Pixels, Vertical Porch, etc.
    /// https://github.com/falahati/NvAPIWrapper/blob/d58faf6d981e91c7f96fa0ad08edb16982a6ad4b/NvAPIWrapper/Native/Display/Structures/Timing.cs
    /// </remarks>
    internal static class WindowsAPI
    {
        internal const int ERROR_SUCCESS = 0;

        internal enum QUERY_DEVICE_CONFIG_FLAGS : uint
        {
            QDC_ALL_PATHS = 0x00000001,
            QDC_ONLY_ACTIVE_PATHS = 0x00000002,
            QDC_DATABASE_CURRENT = 0x00000004
        }

        internal enum DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY : uint
        {
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_OTHER = 0xFFFFFFFF,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HD15 = 0,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SVIDEO = 1,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPOSITE_VIDEO = 2,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPONENT_VIDEO = 3,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DVI = 4,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HDMI = 5,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_LVDS = 6,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_D_JPN = 8,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDI = 9,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EXTERNAL = 10,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EMBEDDED = 11,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EXTERNAL = 12,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EMBEDDED = 13,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDTVDONGLE = 14,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_MIRACAST = 15,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_INTERNAL = 0x80000000,
            DISPLAYCONFIG_OUTPUT_TECHNOLOGY_FORCE_UINT32 = 0xFFFFFFFF
        }

        internal enum DISPLAYCONFIG_SCANLINE_ORDERING : uint
        {
            DISPLAYCONFIG_SCANLINE_ORDERING_UNSPECIFIED = 0,
            DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE = 1,
            DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED = 2,
            DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_UPPERFIELDFIRST = DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED,
            DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_LOWERFIELDFIRST = 3,
            DISPLAYCONFIG_SCANLINE_ORDERING_FORCE_UINT32 = 0xFFFFFFFF
        }

        internal enum DISPLAYCONFIG_ROTATION : uint
        {
            DISPLAYCONFIG_ROTATION_IDENTITY = 1,
            DISPLAYCONFIG_ROTATION_ROTATE90 = 2,
            DISPLAYCONFIG_ROTATION_ROTATE180 = 3,
            DISPLAYCONFIG_ROTATION_ROTATE270 = 4,
            DISPLAYCONFIG_ROTATION_FORCE_UINT32 = 0xFFFFFFFF
        }

        internal enum DISPLAYCONFIG_SCALING : uint
        {
            DISPLAYCONFIG_SCALING_IDENTITY = 1,
            DISPLAYCONFIG_SCALING_CENTERED = 2,
            DISPLAYCONFIG_SCALING_STRETCHED = 3,
            DISPLAYCONFIG_SCALING_ASPECTRATIOCENTEREDMAX = 4,
            DISPLAYCONFIG_SCALING_CUSTOM = 5,
            DISPLAYCONFIG_SCALING_PREFERRED = 128,
            DISPLAYCONFIG_SCALING_FORCE_UINT32 = 0xFFFFFFFF
        }

        internal enum DISPLAYCONFIG_PIXELFORMAT : uint
        {
            DISPLAYCONFIG_PIXELFORMAT_8BPP = 1,
            DISPLAYCONFIG_PIXELFORMAT_16BPP = 2,
            DISPLAYCONFIG_PIXELFORMAT_24BPP = 3,
            DISPLAYCONFIG_PIXELFORMAT_32BPP = 4,
            DISPLAYCONFIG_PIXELFORMAT_NONGDI = 5,
            DISPLAYCONFIG_PIXELFORMAT_FORCE_UINT32 = 0xffffffff
        }

        internal enum DISPLAYCONFIG_MODE_INFO_TYPE : uint
        {
            DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE = 1,
            DISPLAYCONFIG_MODE_INFO_TYPE_TARGET = 2,
            DISPLAYCONFIG_MODE_INFO_TYPE_FORCE_UINT32 = 0xFFFFFFFF
        }

        internal enum DISPLAYCONFIG_DEVICE_INFO_TYPE : uint
        {
            DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME = 1,
            DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME = 2,
            DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_PREFERRED_MODE = 3,
            DISPLAYCONFIG_DEVICE_INFO_GET_ADAPTER_NAME = 4,
            DISPLAYCONFIG_DEVICE_INFO_SET_TARGET_PERSISTENCE = 5,
            DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_BASE_TYPE = 6,
            DISPLAYCONFIG_DEVICE_INFO_FORCE_UINT32 = 0xFFFFFFFF
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_PATH_SOURCE_INFO
        {
            public LUID adapterId;
            public uint id;
            public uint modeInfoIdx;
            public uint statusFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_PATH_TARGET_INFO
        {
            public LUID adapterId;
            public uint id;
            public uint modeInfoIdx;
            public DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
            public DISPLAYCONFIG_ROTATION rotation;
            public DISPLAYCONFIG_SCALING scaling;
            public DISPLAYCONFIG_RATIONAL refreshRate;
            public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
            public bool targetAvailable;
            public uint statusFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_RATIONAL
        {
            public uint Numerator;
            public uint Denominator;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_PATH_INFO
        {
            public DISPLAYCONFIG_PATH_SOURCE_INFO sourceInfo;
            public DISPLAYCONFIG_PATH_TARGET_INFO targetInfo;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_2DREGION
        {
            public uint cx;
            public uint cy;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_VIDEO_SIGNAL_INFO
        {
            public ulong pixelRate;
            public DISPLAYCONFIG_RATIONAL hSyncFreq;
            public DISPLAYCONFIG_RATIONAL vSyncFreq;
            public DISPLAYCONFIG_2DREGION activeSize;
            public DISPLAYCONFIG_2DREGION totalSize;
            public uint videoStandard;
            public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_TARGET_MODE
        {
            public DISPLAYCONFIG_VIDEO_SIGNAL_INFO targetVideoSignalInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTL
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_SOURCE_MODE
        {
            public uint width;
            public uint height;
            public DISPLAYCONFIG_PIXELFORMAT pixelFormat;
            public POINTL position;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct DISPLAYCONFIG_MODE_INFO_UNION
        {
            [FieldOffset(0)]
            public DISPLAYCONFIG_TARGET_MODE targetMode;
            [FieldOffset(0)]
            public DISPLAYCONFIG_SOURCE_MODE sourceMode;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_MODE_INFO
        {
            public DISPLAYCONFIG_MODE_INFO_TYPE infoType;
            public uint id;
            public LUID adapterId;
            public DISPLAYCONFIG_MODE_INFO_UNION modeInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS
        {
            public uint value;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DISPLAYCONFIG_DEVICE_INFO_HEADER
        {
            public DISPLAYCONFIG_DEVICE_INFO_TYPE type;
            public uint size;
            public LUID adapterId;
            public uint id;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DISPLAYCONFIG_TARGET_DEVICE_NAME
        {
            public DISPLAYCONFIG_DEVICE_INFO_HEADER header;
            public DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS flags;
            public DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
            public ushort edidManufactureId;
            public ushort edidProductCodeId;
            public uint connectorInstance;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string monitorFriendlyDeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string monitorDevicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DISPLAYCONFIG_SOURCE_DEVICE_NAME
        {
            public DISPLAYCONFIG_DEVICE_INFO_HEADER header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string gdiDeviceName;
        }

        [DllImport("user32.dll")]
        internal static extern int GetDisplayConfigBufferSizes(
            QUERY_DEVICE_CONFIG_FLAGS Flags,
            out uint NumPathArrayElements,
            out uint NumModeInfoArrayElements
        );

        [DllImport("user32.dll")]
        internal static extern int QueryDisplayConfig(
            QUERY_DEVICE_CONFIG_FLAGS Flags,
            ref uint NumPathArrayElements,
            [Out] DISPLAYCONFIG_PATH_INFO[] PathInfoArray,
            ref uint NumModeInfoArrayElements,
            [Out] DISPLAYCONFIG_MODE_INFO[] ModeInfoArray,
            IntPtr CurrentTopologyId
        );

        [DllImport("user32.dll")]
        internal static extern int DisplayConfigGetDeviceInfo(
            ref IntPtr deviceName
        );

        [DllImport("user32.dll", EntryPoint = "DisplayConfigGetDeviceInfo")]
        internal static extern int DisplayConfigGetDeviceInfo_Target(
            ref DISPLAYCONFIG_TARGET_DEVICE_NAME deviceName
        );

        [DllImport("user32.dll", EntryPoint = "DisplayConfigGetDeviceInfo")]
        internal static extern int DisplayConfigGetDeviceInfo_Source(
            ref DISPLAYCONFIG_SOURCE_DEVICE_NAME deviceName
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        [DllImport("user32.dll")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        internal const uint MONITORINFOF_PRIMARY = 1;
        private const int CCHDEVICENAME = 32;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public uint Size;
            /// <summary>Display monitor rectangle, expressed in virtual-screen coordinates. May be negative values on non-primary.</summary>
            public Rect MonitorArea;
            /// <summary>Work area rectangle, excluding taskbar/sidebars. May be negative values on non-primary.</summary>
            public Rect WorkArea;
            /// <summary>Attributes of monitor. May have MONITORINFOF_PRIMARY value</summary>
            public uint Flags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MONITORINFOEX
        {
            public uint Size;
            /// <summary>Display monitor rectangle, expressed in virtual-screen coordinates. May be negative values on non-primary.</summary>
            public Rect MonitorArea;
            /// <summary>Work area rectangle, excluding taskbar/sidebars. May be negative values on non-primary.</summary>
            public Rect WorkArea;
            /// <summary>Attributes of monitor. May have MONITORINFOF_PRIMARY value</summary>
            public uint Flags;
            /// <summary>Device name of the monitor being used.</summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string DeviceName;

            public void Init()
            {
                this.Size = 40 + 2 * CCHDEVICENAME;
                this.DeviceName = string.Empty;
            }
        }

        internal enum MC_DISPLAY_TECHNOLOGY_TYPE
        {
            MC_SHADOW_MASK_CATHODE_RAY_TUBE,
            MC_APERTURE_GRILL_CATHODE_RAY_TUBE,
            MC_THIN_FILM_TRANSISTOR,
            MC_LIQUID_CRYSTAL_ON_SILICON,
            MC_PLASMA,
            MC_ORGANIC_LIGHT_EMITTING_DIODE,
            MC_ELECTROLUMINESCENT,
            MC_MICROELECTROMECHANICAL,
            MC_FIELD_EMISSION_DEVICE,
        }

        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, IntPtr dwData);
        internal delegate bool MonitorEnumProc(IntPtr hDesktop, IntPtr hdc, ref Rect pRect, IntPtr dwData);

        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        /// <summary>For the DISPLAY_DEVICE structure, used by EnumDisplayDevices()</summary>
        [Flags()]
        internal enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        /// <summary>Structure used by EnumDisplayDevices()</summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("user32.dll", EntryPoint = "MonitorFromWindow")]
        internal static extern IntPtr MonitorFromWindow([In] IntPtr hwnd, uint dwFlags);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorTechnologyType")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorTechnologyType(IntPtr hMonitor, ref MC_DISPLAY_TECHNOLOGY_TYPE pdtyDisplayTechnologyType);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorCapabilities")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorCapabilities(IntPtr hMonitor, ref uint pdwMonitorCapabilities, ref uint pdwSupportedColorTemperatures);

        [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitors")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize, ref PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct PHYSICAL_MONITOR
        {
            public IntPtr hPhysicalMonitor;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPhysicalMonitorDescription;
        }

        [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);
    }
    /// End of platform-specific calls
}
