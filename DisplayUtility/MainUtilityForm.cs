using Microsoft.Win32;
using System;
using System.Drawing;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using RejTech.Drawing;

namespace RejTech
{
    public partial class MainUtilityForm : Form
    {
        const String DEFAULT_APP_TITLE = "Utility";
        const String DEFAULT_MESSAGEBOX_TITLE = "Utility";
        const String URL_UTILITY = "https://www.google.com";
        const String DEVICE_PREFIX = "";

        // Color Theme 
        readonly Color COLOR_BACKGROUND_LOGO = Color.Black;
        readonly Color COLOR_BACKGROUND_MAIN = Color.Black;
        readonly Color COLOR_VALUES = Color.White;
        readonly Color COLOR_LABEL_ENABLED = Color.Green;
        readonly Color COLOR_LABEL_DISABLED = Color.White;

        /* Not used in non-strobe-utility contexts
         
        const string ERROR_DETAILS_UNSUPPORTED = "This monitor is not supported by this Strobe Utility app.";

        const string ERROR_DETAILS_DDCSTROBE = "Detected a potentially supported monitor.\n\n" +
            "Unfortunately, the monitor is not signalling support for " +
            "application-controlled strobe adjustments via DDC commands. " +
            "Please check your monitor firmware version";

        const string ERROR_DETAILS_DDCDVI = "Detected a potentially supported monitor.\n\n" +
            "However, you appear to be using a DVI cable that might have a " +
            "missing DDC serial wire. Try a different DVI cable, or test a different " +
            "video output such as DisplayPort or HDMI.";

        const string ERROR_DETAILS_DDCFAIL = "Detected a potentially supported monitor.\n\n" +
            "However, Strobe Utility is unable to communicate to your monitor " +
            "right now. It is currently not responding to DDC commands.\n\n" +
            "- See monitor 'Setup Menu' -> scroll down -> 'DDC/CI' -> ON\n" +
            "- Try a different video port on your graphics card\n" +
            "- Try a different video port on your monitor\n" +
            "- Try a different video cable\n" +
            "- Try adjusting from a different computer\n" +
            "- Try running as Administrator";

        const string ERROR_DETAILS_DDCFAIL_FORCED = "Strobe Utility is unable to communicate to " +
            "your monitor right now. It is currently not responding to DDC commands.\n\n" +
            "If you are absolutely sure that your monitor is indeed supported, then try the following:\n\n" +
            "- See monitor 'Setup Menu' -> scroll down -> 'DDC/CI' -> ON\n" +
            "- Exit your monitor's Factory Menu or Service Menu\n" +
            "- Test monitor OSD main menu. Make sure it works.\n" +
            "- Try a different video port on your graphics card\n" +
            "- Try a different video port on your monitor\n" +
            "- Try a different video cable (DisplayPort vs HDMI)\n" +
            "- Try adjusting from a different computer\n" +
            "- Try running as Administrator";

        const string ERROR_DETAILS_DDCWRITE = "Detected a potentially supported monitor.\n\n" +
            "However, your monitor is rejecting attempts to change " +
            "strobe settings. Adjustments are not working as a result.\n\n" +
            "- Try running as Administrator\n" +
            "- Restart your computer. Upgrade graphics drivers.\n" +
            "- Try a different video cable (DisplayPort vs HDMI)\n" +
            "- Try a different video port on your graphics card\n" +
            "- Try a different video port on your monitor\n" +
            "- Try adjusting from a different computer\n";

        const string SUCCESS_DETAILS = "Your monitor is supported.\n\n" +
            "However, before adjusting the monitor, you may need to first " +
            "enable the blur reduction feature of your display before adjustments work.\n\n" +
            "If adjusting 'Strobe Length' does not change brightness, then " +
            "you need to enable blur reduction first.";
*/
        const string ERROR_DETAILS_ADMIN = "Insufficient priveleges to operate this app.\n\n" +
            "Please restart this software as Administrator and then try again.";

        const bool AUTO_REFRESH_DDC = false;

        /// <summary>Transparency of forn during test</summary>
        const double OPACITY_DURING_TEST = 0.25;

        static IntPtr windowHandle;
        static TestPattern motionTest;
        static Thread thread;
        StartupForm startupForm = new StartupForm();
        BrowserLauncher launcher = new BrowserLauncher();
        DisplayCommander ddc = new DisplayCommander();
        DisplayConfig configFile = new DisplayConfig();
        DisplayConfigItem selectedConfigForced = null;
        DisplayConfigItem lastDetectedConfig = null;
        String lastDetectedDeviceID = "";
        Displays displays;
        DisplayInfo selectedDisplay;
        Version versionInfo = Assembly.GetEntryAssembly().GetName().Version;
        string versionText = "";
        string status = "uninitialized";
        string popupMessage = "";
        uint initializing = 0;
        bool strobeAdjustable = false;
        bool formHover = true;
        bool exitNow = false;
        bool launchTray = false;
        bool launchTest = false;
        bool testWasVisible = false;
        bool firstScan = true;
        bool trayFirstClick = false;

        public MainUtilityForm(string options)
        {
            launchTray = (options == "-autostart");
            launchTest = (options != "-notest") && !launchTray;
            InitializeComponent();

            this.Text = DEFAULT_APP_TITLE;
            this.trayIcon.Text = DEFAULT_APP_TITLE;
            this.BackColor = COLOR_BACKGROUND_MAIN;

            buttonTest.ForeColor = COLOR_BACKGROUND_MAIN;
            labelField3Value.ForeColor = COLOR_VALUES;
            labelField4Value.ForeColor = COLOR_VALUES;
            labelField5Value.ForeColor = COLOR_VALUES;
            labelField6Value.ForeColor = COLOR_VALUES;
        }

        delegate void ShowScreenOnDisplayDelegate(DisplayInfo display);

        public void TestLaunch(DisplayInfo display)
        {
            if ((thread == null) || !thread.IsAlive)
            {
                windowHandle = this.Handle;
                thread = new Thread(TestThread);
                thread.Start(selectedDisplay);
            }
            else if (motionTest != null)
            {
                motionTest.SetDisplay = display;
            }
        }

        public bool TestVisible()
        {
            if (motionTest != null)
            {
                return motionTest.Visible;
            }
            return false;
        }

        public void TestShow(bool show)
        {
            if (show) TestLaunch(selectedDisplay);
            if (motionTest != null)
            {
                motionTest.Visible = show;
            }
        }

        public void TestStop()
        {
            if (motionTest != null)
            {
                motionTest.Stop();
            }
        }

        public void TestToggle()
        {
            if (!TestVisible())
            {
                TestShow(true);
                TopMost = true;
                this.Opacity = formHover ? 1.00 : OPACITY_DURING_TEST;
                Window.SetForeground(Handle);
            }
            else
            {
                TestShow(false);
                this.Opacity = 1.00;
                TopMost = false;
            }
        }

        /// <summary>Sync test visibile window state with dialog state</summary>
        private void TestSyncVisibleState(bool newVisibleState)
        {
            if (!newVisibleState)
            {
                if (TestVisible())
                {
                    testWasVisible = true;
                    TestShow(false);
                }
                TestStop();
            }
            else if (testWasVisible)
            {
                TestShow(true);
                testWasVisible = false;
            }
        }

        public static void TestThread(object display)
        {
            try
            {
                if (motionTest == null)
                {
                    motionTest = new TestPattern(windowHandle);
                    motionTest.Start((DisplayInfo)display);
                    motionTest = null;
                }
            }
            catch (Exception)
            {
                motionTest = null;
            }
        }

        /*protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore((launchTray && firstSetVisible) ? false : value);
            firstSetVisible = false;
        }*/

        /// <summary>Form startup</summary>
        private void StilityForm_Load(object sender, EventArgs e)
        {
            versionText = versionInfo.Major + "." + versionInfo.Minor + "." + versionInfo.Build;
            if (versionInfo.Revision > 0) versionText += "." + versionInfo.Revision;

            ClearMessage();
            EnableAdjustments(false);
            RefreshMonitorPulldown();
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
            if (launchTray)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                TestLaunch(selectedDisplay);
            }
        }

        /// <summary>Returns true if administrator</summary>
        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void UnauthorizedAccessOccured()
        {
            ErrorMessage("ERROR", "UnauthorizedAccess Exception error.\nPlease restart this app as Administrator.", ERROR_DETAILS_ADMIN);
            MessageBox.Show("UnauthorizedAccess error!\n\n" +
                "Try Running as Administrator. Right click on this executable, and select 'Run as administrator'",
                "Unauthorized Access Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Application.Exit();
        }

        private DisplayConfigItem CurrentStrobeConfig()
        {
            if (selectedConfigForced != null) return selectedConfigForced;
            return GetConfigForDisplay(selectedDisplay);
        }

        /// <summary>Automatically detect strobe configuration for monitor</summary>
        private DisplayConfigItem GetMonitorConfigByDeviceID(string displayProductID)
        {
            // Attempt automatic detect of monitor via Device ID
            foreach (DisplayConfigItem item in configFile.Items)
            {
                if (!String.IsNullOrEmpty(item.Device))
                {
                    Regex regex = new Regex(item.Device, RegexOptions.IgnoreCase);
                    Match m = regex.Match(displayProductID);
                    if (m.Success) return item;
                }
            }
            return null;
        }

        /// <summary>Automatically detect strobe configuration for monitor</summary>
        private DisplayConfigItem GetConfigForDisplay(DisplayInfo display)
        {
            if (display == null) return null;

            // Quick check of last detected monitor
            if (lastDetectedConfig != null)
            {
                if (lastDetectedDeviceID == display.DeviceProductID) return lastDetectedConfig;
            }

            // Attempt automatic detect of monitor via Device ID
            DisplayConfigItem detectedConfig = GetMonitorConfigByDeviceID(display.DeviceProductID);
            if ((detectedConfig != null) &&
                (display.DeviceProductID.Substring(0, DEVICE_PREFIX.Length) == DEVICE_PREFIX))
            {
                lastDetectedConfig = detectedConfig;
                lastDetectedDeviceID = display.DeviceProductID;
                return lastDetectedConfig;
            }

            // Attempt automatic detect of monitor via Product ID
            foreach (DisplayConfigItem item in configFile.Items)
            {
                if (display.FriendlyName.Contains(item.Name))
                {
                    lastDetectedConfig = item;
                    lastDetectedDeviceID = display.DeviceProductID;
                    return lastDetectedConfig;
                }
            }
            return null;
        }

        /// <summary>Are the strobe commands activated?</summary>
        private bool MonitorSettingsQuery(DisplayInfo display, DisplayConfigItem config = null)
        {
            if (config == null) config = GetConfigForDisplay(display);
            if (config == null) return false;
            // Configure the DDC command appropriate to the currently selected display
            ddc.VCPcodeStrobeEnable = config.CommandEnable;
            ddc.VCPcodeStrobeLen = config.CommandStrobeLen;
            ddc.VCPcodeStrobePhase = config.CommandStrobePhase;
            ddc.VCPcodeOverdriveGain = config.CommandOverdriveGain;
            return ddc.QuerySettings(display);
        }

        /// <summary>Clear the message display</summary>
        private void ClearMessage()
        {
            SuccessMessage("", "");
        }

        /// <summary>Message display for successes</summary>
        private void SuccessMessage(string label, string text, string popup = "")
        {
            bool hasMessage = ((label != "") || (text != ""));
            labelMessageLabel.Visible = hasMessage;
            labelMessageLabel.BackColor = Color.Black;
            labelMessageLabel.ForeColor = Color.Lime;
            labelMessageLabel.Text = label;
            labelMessageText.Visible = hasMessage;
            labelMessageText.BackColor = Color.Black;
            labelMessageText.ForeColor = Color.Lime;
            labelMessageText.Text = text;
            popupMessage = popup;
        }

        /// <summary>Message display for warnings</summary>
        private void WarningMessage(string label, string text, string popup = "")
        {
            labelMessageLabel.BackColor = Color.Black;
            labelMessageLabel.ForeColor = Color.Yellow;
            labelMessageLabel.Text = label;
            labelMessageLabel.Visible = true;
            labelMessageText.BackColor = Color.Black;
            labelMessageText.ForeColor = Color.Yellow;
            labelMessageText.Text = text;
            labelMessageText.Visible = true;
            popupMessage = popup;
        }

        /// <summary>Message display for errors</summary>
        private void ErrorMessage(string label, string text, string popup = "")
        {
            labelMessageLabel.BackColor = Color.Red;
            labelMessageLabel.ForeColor = Color.Yellow;
            labelMessageLabel.Text = label;
            labelMessageLabel.Visible = true;
            labelMessageText.BackColor = Color.Red;
            labelMessageText.ForeColor = Color.Yellow;
            labelMessageText.Text = text;
            labelMessageText.Visible = true;
            popupMessage = popup;
        }

        /// <summary>Enable/disable the adjustments (appearance, sliders active, etc)</summary>
        private void EnableAdjustments(bool enable = true)
        {
            strobeAdjustable = enable;
            sliderField4.Enabled = enable;
            sliderField3.Enabled = enable;
            sliderField5.Enabled = enable;
            comboBoxFIeld2.Enabled = enable;
            comboBoxFIeld2.Items.Clear();
            if (!enable)
            {
                sliderField4.Value = sliderField4.Minimum;
                sliderField3.Value = sliderField3.Minimum;
                sliderField5.Value = sliderField5.Minimum;
            }

            labelDisplay.ForeColor = COLOR_LABEL_ENABLED;
            labelField2.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;

            // Strobe list
            DisplayConfigItem config = CurrentStrobeConfig();
            if (enable)
            {
                if ((config != null) &&
                    (config.StrobeEnableList != null))
                {
                    comboBoxFIeld2.Items.AddRange(config.StrobeEnableList.ToArray());
                }
            }

            // Strobe pulse width slider
            labelField3.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            if (!enable) labelField3Value.Text = "";
            labelField3Value.Visible = enable;

            // Strobe pulse phase slider
            labelField4.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            if (!enable) labelField4Value.Text = "";
            labelField4Value.Visible = enable;

            // Overdrive gain slider
            labelField5.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            if (!enable) labelField5Value.Text = "";
            labelField5Value.Visible = enable;

            // Informational area
            labelField7.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            labelField7Value.ForeColor = enable ? COLOR_VALUES : COLOR_LABEL_DISABLED;
            if (!enable) labelField7Value.Text = "";
            labelField6.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            labelField6Value.ForeColor = enable ? COLOR_VALUES : COLOR_LABEL_DISABLED;
            if (!enable) labelField6Value.Text = "";

            if (enable) ClearMessage();
            this.Refresh();

            labelField2.Visible = (ddc.VCPcodeStrobeEnable > 0);
            comboBoxFIeld2.Visible = (ddc.VCPcodeStrobeEnable > 0);
            trayIcon.ContextMenuStrip = (enable && (ddc.VCPcodeStrobeEnable > 0)) ? contextMenuTrayFull : contextMenuTray;
        }

        /// <summary>Refresh success/fail messages for currently selected monitor</summary>
        private void RefreshMonitorMessage()
        {
            initializing++;
            status = "";
            try
            {
                WarningMessage("", "");
                EnableAdjustments(false);
                this.Refresh();
                if (selectedDisplay == null)
                {
                    ErrorMessage("ERROR", "No monitor selected");
                    status = "error-unselected";
                    return;
                }

                /* Not use for non-Strobe-Utility cases
                  
                DisplayConfigItem currentConfig = CurrentStrobeConfig();
                if (currentConfig == null)
                {
                    bool vcp10supported = Drawing.DisplayCommander.VCPSupported(selectedDisplay, 0x10);
                    if (!vcp10supported)
                    {
                        ErrorMessage("ERROR", "Unable to initialize DDC/CI for this monitor.\nClick for More Info", ERROR_DETAILS_DDCFAIL);
                        status = "error-autofailddc";
                    }
                    else
                    {
                        ErrorMessage("ERROR", "This monitor is not supported.\nClick for More Info", ERROR_DETAILS_UNSUPPORTED);
                        status = "error-notdetected";
                        return;
                    }
                }
                else if (!MonitorSettingsQuery(selectedDisplay, currentConfig))
                {
                    bool vcp10supported = Drawing.DisplayCommander.VCPSupported(selectedDisplay, 0x10);
                    if ((selectedConfigForced != null) && vcp10supported)
                    {
                        ErrorMessage("ERROR", "Manual strobe configuration does not seem to be supported. Select a different configuration.", "");
                        status = "error-forcefail";
                    }
                    else if (selectedConfigForced != null)
                    {
                        ErrorMessage("ERROR", "Unable to initialize DDC/CI for this monitor.\nClick for More Info", ERROR_DETAILS_DDCFAIL_FORCED);
                        status = "error-forcefailddc";
                    }
                    else if (vcp10supported)
                    {
                        ErrorMessage("ERROR", "Monitor does not support the strobe command codes.\nClick for More Info", ERROR_DETAILS_DDCSTROBE);
                        status = "error-ddcstrobe";
                    }
                    else if (selectedDisplay.VideoPort == "DVI")
                    {
                        ErrorMessage("ERROR", "Unable to initialize DDC/CI.\nPossible DVI cable issue. Click for More Info", ERROR_DETAILS_DDCDVI);
                        status = "error-ddcdvi";
                    }
                    else
                    {
                        ErrorMessage("ERROR", "Unable to initialize DDC/CI for this monitor.\nClick for More Info", ERROR_DETAILS_DDCFAIL);
                        status = "error-ddc";
                    }
                    return;
                }

                if (status == "")
                {
                    if (selectedConfigForced != null)
                    {
                        SuccessMessage("", "Manual Mode: If working, send screenshot to support. Device " + selectedDisplay.DeviceProductID + " via " + selectedDisplay.VideoPort + " using " + selectedConfigForced.Name + " settings.");
                    }
                    else
                    {
                        SuccessMessage("", "", SUCCESS_DETAILS);
                    }
                    status = "detected";
                }

                */
                if (status.StartsWith("detected"))
                {
                    EnableAdjustments(true);
                    RefreshMonitorSliders();
                }
                
                this.BringToFront();
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Exception error");
            }
            finally
            {
                initializing--;
            }
        }

        /// <summary>Refresh the monitor pulldown menu with currently attached monitors</summary>
        private void RefreshMonitorPulldown()
        {
            initializing++;
            try
            {
                uint oldID = 0;
                if (selectedDisplay != null) oldID = selectedDisplay.ID;
                selectedDisplay = null;

                if (firstScan && !launchTray)
                {
                    startupForm.Show();
                    startupForm.Message("Scanning Monitors...");
                }

                // Scan monitors 
                displays = new Displays();
                if (displays.Count > 0) selectedDisplay = displays[0];

                if (firstScan && !launchTray)
                {
                    startupForm.Message("Complete...");
                    startupForm.Hide();
                    firstScan = false;
                }

                // Update Strobe Utility UI
                comboBoxDisplaysList.Items.Clear();
                foreach (DisplayInfo item in displays)
                {
                    comboBoxDisplaysList.Items.Add(item);
                    if (item.ID == oldID)
                    {
                        // Maintain same selected monitor (during times when other monitors in multimonitor are hot-unplugged on the fly)
                        comboBoxDisplaysList.SelectedItem = item;
                    }
                }

                // Auto-select first strobe-commandable monitor
                if (comboBoxDisplaysList.SelectedIndex < 0)
                {
                    foreach (DisplayInfo item in comboBoxDisplaysList.Items)
                    {
                        if (MonitorSettingsQuery(item))
                        {
                            comboBoxDisplaysList.SelectedItem = item;
                            break;
                        }
                    }
                }

                // If fail, auto-select first supposely compatible monitor
                if (comboBoxDisplaysList.SelectedIndex < 0)
                {
                    foreach (DisplayInfo item in comboBoxDisplaysList.Items)
                    {
                        if (null != GetConfigForDisplay(item))
                        {
                            comboBoxDisplaysList.SelectedItem = item;
                            break;
                        }
                    }
                }

                // If all fail, auto-select first monitor in list
                if ((comboBoxDisplaysList.SelectedIndex < 0) && (comboBoxDisplaysList.Items.Count > 0))
                {
                    comboBoxDisplaysList.SelectedIndex = 0;
                }

                if (comboBoxDisplaysList.SelectedItem != null)
                {
                    selectedDisplay = (DisplayInfo)comboBoxDisplaysList.SelectedItem;
                    DisplayConfigItem currentConfig = GetConfigForDisplay(selectedDisplay);
                }
                RefreshMonitorMessage();
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Exception error");
            }
            finally
            {
                initializing--;
            }
        }

        /// <summary>Refresh monitor sliders with the current settings from monitor</summary>
        private void RefreshMonitorSliders()
        {
            if ((selectedDisplay == null) || !strobeAdjustable || !MonitorSettingsQuery(selectedDisplay, CurrentStrobeConfig()))
            {
                EnableAdjustments(false);
                return;
            }
            if ((configFile == null) || (configFile.Items == null) || (configFile.Items.Count == 0))
            {
                MessageBox.Show("Fatal error: Monitor supported but config is missing. Please screenshot this error message and send to support");
                EnableAdjustments(false);
                return;
            }

            // Set adjustment ranges specific to this monitor.
            DisplayConfigItem selectedConfig = CurrentStrobeConfig();
            sliderField3.RightToLeft = (selectedConfig.StrobeLenReversed ? RightToLeft.Yes : RightToLeft.No);
            sliderField3.Minimum = (int)selectedConfig.StrobeLenMin;
            sliderField3.Maximum = (int)selectedConfig.StrobeLenMax;
            sliderField4.RightToLeft = (selectedConfig.StrobePhaseReversed ? RightToLeft.Yes : RightToLeft.No);
            sliderField4.Minimum = (int)selectedConfig.StrobePhaseMin;
            sliderField4.Maximum = (int)selectedConfig.StrobePhaseMax;
            sliderField5.RightToLeft = (selectedConfig.OverdriveGainReversed ? RightToLeft.Yes : RightToLeft.No);
            sliderField5.Minimum = (int)selectedConfig.OverdriveGainMin;
            sliderField5.Maximum = (int)selectedConfig.OverdriveGainMax;
            RefreshValuesFromMonitor();
        }

        /// <summary>Refresh settings from monitor</summary>
        /// <param name="whichValue">Specify one of ddc.VCP numbers to refresh only 1 value</param>
        private void RefreshValuesFromMonitor(uint whichValue = uint.MaxValue, bool force = false)
        {
            int strobeLen = 0;
            int strobePhase = 0;
            int overdriveGain = 0;

            //---------------------------
            // Strobe Enable DDC VCP
            if ((whichValue == uint.MaxValue) || (whichValue == ddc.VCPcodeStrobeEnable))
            {
                try
                {
                    initializing++;
                    ddc.ForceRefresh = AUTO_REFRESH_DDC || force;
                    comboBoxFIeld2.SelectedIndex = ddc.Enable;
                }
                catch (Exception) { }
                finally
                {
                    initializing--;
                }
            }

            //---------------------------
            // Strobe Length DDC VCP
            if ((whichValue == uint.MaxValue) || (whichValue == ddc.VCPcodeStrobeLen))
            {
                try
                {
                    ddc.ForceRefresh = AUTO_REFRESH_DDC || force;
                    strobeLen = (int)ddc.StrobeLen;
                    // Re-query if it's a 0, sometimes it's a glitch
                    //if (strobeLen == 0) strobeLen = (int)ddc.StrobeLen;
                }
                catch (Exception) { }
                try
                {
                    initializing++;
                    sliderField3.Value = strobeLen;
                }
                catch (Exception)
                {
                    if (sliderField3.Minimum > strobeLen)
                    {
                        sliderField3.Value = sliderField3.Minimum;
                    }
                    if (sliderField3.Maximum < strobeLen)
                    {
                        sliderField3.Value = sliderField3.Maximum;
                    }
                }
                finally
                {
                    initializing--;
                }
                labelField3Value.Text = strobeLen.ToString();
            }

            //---------------------------
            // Strobe Phase DDC VCP
            if ((whichValue == uint.MaxValue) || (whichValue == ddc.VCPcodeStrobePhase))
            {
                try
                {
                    ddc.ForceRefresh = AUTO_REFRESH_DDC || force;
                    strobePhase = ddc.StrobePhase;
                }
                catch (Exception) { }
                try
                {
                    initializing++;
                    sliderField4.Value = strobePhase;
                }
                catch (Exception)
                {
                    if (sliderField4.Minimum > strobePhase)
                    {
                        sliderField4.Value = sliderField4.Minimum;
                    }
                    if (sliderField4.Maximum < strobePhase)
                    {
                        sliderField4.Value = sliderField4.Maximum;
                    }
                }
                finally
                {
                    initializing--;
                }
                labelField4Value.Text = strobePhase.ToString();
            }

            //---------------------------
            // Overdrive Gain DDC VCP
            if ((whichValue == uint.MaxValue) || (whichValue == ddc.VCPcodeStrobePhase))
            {
                try
                {
                    initializing++;
                    ddc.ForceRefresh = AUTO_REFRESH_DDC || force;
                    overdriveGain = ddc.OverdriveGain;
                }
                catch (Exception) { }
                try
                {
                    sliderField5.Value = overdriveGain;
                }
                catch (Exception)
                {
                    if (sliderField5.Minimum > overdriveGain)
                    {
                        sliderField5.Value = sliderField5.Minimum;
                    }
                    if (sliderField5.Maximum < overdriveGain)
                    {
                        sliderField5.Value = sliderField5.Maximum;
                    }
                }
                finally
                {
                    initializing--;
                }
                labelField5Value.Text = overdriveGain.ToString();
            }

            labelField6Value.Text = selectedDisplay.RefreshRate.ToString("F1") + " Hz";
            labelField7Value.Text = selectedDisplay.VertTotal.ToString();
        }

        private void VerifyCustomStrobe()
        {
            int customIndex = (comboBoxFIeld2.Items.Count - 1); // TODO: Make this customizable in INI
            if (comboBoxFIeld2.SelectedIndex != customIndex)
            {
                comboBoxFIeld2.SelectedIndex = customIndex;
            }
        }

        private void ForceMonitorRescan()
        {
            EnableAdjustments(false);
            RefreshMonitorPulldown();
            forceRefresh = true;
            ComboBoxMonitors_SelectionChangeCommitted(null, EventArgs.Empty);
        }

        /// <summary>Polls the monitor to see if strobe state changed in menus, and refresh GUI accordingly</summary>
        private void PollMonitorForStrobeState()
        {
            if (initializing > 0) return;
            if (CurrentStrobeConfig() == null) return;

            ddc.ForceRefresh = true;
            int oldIndex = comboBoxFIeld2.SelectedIndex;
            int newIndex = ddc.Enable;
            if (ddc.Failed)
            {
                ddc.QuerySettings(selectedDisplay, true);
                newIndex = ddc.Enable;
            }
            if (ddc.Failed)
            {
                ForceMonitorRescan();
                newIndex = ddc.Enable;
            }
            if (!ddc.Failed)
            {
                if (newIndex >= comboBoxFIeld2.Items.Count) newIndex = 0;
                if (!strobeAdjustable) EnableAdjustments(true);
                if (oldIndex != newIndex)
                {
                    initializing++;
                    comboBoxFIeld2.SelectedIndex = newIndex;
                    initializing--;
                    RefreshValuesFromMonitor(ddc.VCPcodeStrobeLen, true);
                    RefreshValuesFromMonitor(ddc.VCPcodeStrobePhase, true);
                    RefreshValuesFromMonitor(ddc.VCPcodeOverdriveGain, true);
                }
            }
        }

        private String GetParams()
        {
            return "strobeutil=" + versionText + "&strobeutilstatus=" + status + "&device=" + ((selectedDisplay != null) ? selectedDisplay.DeviceProductID : "none");
        }

        private bool forceRefresh = false;
        private void ComboBoxMonitors_SelectionChangeCommitted(object sender, EventArgs e)
        {
            selectedConfigForced = null;
            if (comboBoxDisplaysList.SelectedItem is DisplayInfo)
            {
                selectedDisplay = (DisplayInfo)comboBoxDisplaysList.SelectedItem;
            }
            if (forceRefresh)
            {
                ddc.CurrentDisplay = null;
                ddc.QuerySettings(selectedDisplay, true);
                forceRefresh = false;
            }
            DisplayConfigItem currentConfig = GetConfigForDisplay(selectedDisplay);
            RefreshMonitorMessage();
            if ((motionTest != null) && motionTest.Visible)
            {
                TestLaunch(selectedDisplay);
            }
        }

        private void ComboBoxMonitors_Click(object sender, EventArgs e)
        {
            forceRefresh = true;
        }

        private void ComboBoxConfig_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (forceRefresh) ddc.CurrentDisplay = null;
            RefreshMonitorMessage();
        }

        private void comboBoxStrobe_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disable strobe if not adjustable
            if (initializing > 0) return;
            bool strobeQuery = MonitorSettingsQuery(selectedDisplay, CurrentStrobeConfig());
            bool disabled = (!strobeAdjustable || !strobeQuery);
            if (disabled) comboBoxFIeld2.SelectedIndex = 0;

            try
            {
                // Feedback flash
                // TODO: comboBoxStrobe.BackColor = (comboBoxStrobe.SelectedIndex > 0) ? Color.Lime : Color.DarkRed;
                initializing++;
                if (!disabled)
                {
                    ddc.Enable = comboBoxFIeld2.SelectedIndex;
                    if (ddc.Failed)
                    {
                        // Wake from sleep/power: DDC can go nonfunctional.
                        ddc.QuerySettings(selectedDisplay, true);
                        ddc.Enable = comboBoxFIeld2.SelectedIndex;
                        if (ddc.Failed)
                        {
                            ForceMonitorRescan();
                            ddc.Enable = comboBoxFIeld2.SelectedIndex;
                        }
                    }
                }
                // TODO: comboBoxStrobe.BackColor = Color.White;
                this.RefreshValuesFromMonitor();
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Initialization Exception Error 0xBB01");
            }
            finally
            {
                initializing--;
            }
        }

        private void SliderStrobePhase_ValueChanged(object sender, EventArgs e)
        {
            if (!strobeAdjustable || !sliderField4.Enabled) return;
            labelField4Value.Text = sliderField4.Value.ToString();
            labelField4Value.Refresh();
            if (initializing > 0) return;
            VerifyCustomStrobe();

            try
            {
                initializing++;
                ddc.StrobePhase = sliderField4.Value;
                if (ddc.Failed)
                {
                    // Wake from sleep/power: DDC can go nonfunctional.
                    ddc.QuerySettings(selectedDisplay, true);
                    ddc.StrobePhase = sliderField4.Value;
                    if (ddc.Failed)
                    {
                        ForceMonitorRescan();
                        ddc.StrobePhase = sliderField4.Value;
                    }
                }
                this.RefreshValuesFromMonitor(ddc.VCPcodeStrobePhase);
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Initialization Exception Error 0xBB02");
            }
            finally
            {
                initializing--;
            }
        }

        private void SliderStrobeLength_ValueChanged(object sender, EventArgs e)
        {
            if (!strobeAdjustable || !sliderField3.Enabled) return;
            labelField3Value.Text = sliderField3.Value.ToString();
            labelField3Value.Refresh();
            if (initializing > 0) return;
            VerifyCustomStrobe();

            try
            {
                initializing++;
                ddc.StrobeLen = sliderField3.Value;
                if (ddc.Failed)
                {
                    // Wake from sleep/power: DDC can go nonfunctional.
                    ddc.QuerySettings(selectedDisplay, true);
                    ddc.StrobeLen = sliderField3.Value;
                    if (ddc.Failed)
                    {
                        ForceMonitorRescan();
                        ddc.StrobeLen = sliderField3.Value;
                    }
                }
                this.RefreshValuesFromMonitor(ddc.VCPcodeStrobeLen);
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Initialization Exception Error 0xBB03");
            }
            finally
            {
                initializing--;
            }
        }

        private void SliderOverdriveGain_ValueChanged(object sender, EventArgs e)
        {
            if (!strobeAdjustable || !sliderField5.Enabled) return;
            labelField5Value.Text = sliderField5.Value.ToString();
            labelField5Value.Refresh();
            if (initializing > 0) return;
            VerifyCustomStrobe();

            try
            {
                initializing++;
                ddc.OverdriveGain = sliderField5.Value;
                if (ddc.Failed)
                {
                    // Wake from sleep/power: DDC can go nonfunctional.
                    ddc.QuerySettings(selectedDisplay, true);
                    ddc.OverdriveGain = sliderField5.Value;
                    if (ddc.Failed)
                    {
                        ForceMonitorRescan();
                        ddc.OverdriveGain = sliderField5.Value;
                    }
                }
                this.RefreshValuesFromMonitor(ddc.VCPcodeOverdriveGain);
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Initialization Exception Error 0xBB04");
            }
            finally
            {
                initializing--;
            }
        }

        private void SliderStrobeLength_MouseDown(object sender, MouseEventArgs e)
        {
            // TODO:
            // if ((initializing == 0) && !checkboxEnableStrobe.Checked) checkboxEnableStrobe.Checked = true;
        }

        private void SliderStrobePhase_MouseDown(object sender, MouseEventArgs e)
        {
            // TODO:
            // if ((initializing == 0) && !checkboxEnableStrobe.Checked) checkboxEnableStrobe.Checked = true;
        }

        private void LabelPersistenceMotionTest_Click(object sender, EventArgs e)
        {
            TestToggle();
        }

        private void LabelCrosstalkMotionTest_Click(object sender, EventArgs e)
        {
            TestToggle();
        }

        private void LabelUnsupportedHelp_Click(object sender, EventArgs e)
        {
        }

        private void StrobeUtilityForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        bool displayChanging = false;
        /// <summary>Display settings changed, e.g. new monitor got plugged/unplugged/enabled/disabled/powered/EDID change</summary>
        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (displayChanging) return; // TODO: Handle this better
            displayChanging = true;
            ForceMonitorRescan();
            displayChanging = false;
        }

        private void Popup_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(popupMessage))
            {
                DisplayConfigItem currentConfig = GetConfigForDisplay(selectedDisplay);
                if (currentConfig != null)
                {
                    // In case user made changes to menu
                    PollMonitorForStrobeState();
                    if (strobeAdjustable) return;
                }
                MessageBox.Show(popupMessage,
                    DEFAULT_MESSAGEBOX_TITLE,
                    MessageBoxButtons.OK,
                    (labelMessageText.BackColor == Color.Red) ? MessageBoxIcon.Error : MessageBoxIcon.Information);
            }
        }

        private void StrobeUtilityForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.Visible) return;
            if (WindowState == FormWindowState.Minimized) return;

            bool emergencyClose = (e.Alt && (e.KeyCode == System.Windows.Forms.Keys.F4));
            if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Enter) || emergencyClose)
            {
                /* TODO: Rewrite this / make this optional as a setting
                if (null != GetStrobeConfigForDisplay(ddc.CurrentDisplay))
                {
                    if (ddc.Changed)
                    {
                        WarningMessage("", "Safety key was pressed.\nYour monitor has been reset to last saved settings.", SUCCESS_DETAILS);
                        ddc.RollbackSettings();
                        RefreshMonitorSliders();
                        ddc.Changed = false;
                        if (emergencyClose)
                        {
                            MessageBox.Show("Emergency rollback mode.\n\n" +
                                "You pressed Alt+F4 without saving your adjustments first on the selected monitor.\n\n" +
                                "Your monitor has been reset to last saved settings, just in case you hit Alt+F4 because the monitor blacked out.\n\n" +
                                "To save settings permanently, click the close button instead.", "Exit Strobe Utility");
                            e.Handled = true;
                        }
                    }
                }
                */
            }
            else if (e.KeyCode == Keys.Space)
            {
                // Toggle test
                TestToggle();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                // Force refresh values onscreen
                this.RefreshValuesFromMonitor(uint.MaxValue, true);
            }
        }

        private void StrobeUtilityForm_Activated(object sender, EventArgs e)
        {
            // Re-force motion test back to foreground if visible.
            if ((motionTest != null) && motionTest.Visible)
            {
                if ((!this.Visible) || (WindowState == FormWindowState.Minimized))
                {
                    TestStop();
                }
                else
                {
                    TopMost = true;
                    motionTest.Visible = true;
                }
            }
        }

        private void StrobeUtilityForm_Deactivate(object sender, EventArgs e)
        {
            TopMost = false;
        }

        private void comboBoxMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxFIeld2.SelectedIndex = 0;
            }
            catch { } 
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxFIeld2.SelectedIndex = 1;
            }
            catch { }
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxFIeld2.SelectedIndex = 2;
            }
            catch { }
        }

        private void extremeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxFIeld2.SelectedIndex = 3;
            }
            catch { }
        }

        private void ultraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxFIeld2.SelectedIndex = 4;
            }
            catch { }
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                comboBoxFIeld2.SelectedIndex = 5;
            }
            catch { }
        }

        private void quitStrobeUtilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exitNow = true;
            Application.Exit();
        }

        private void trayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                trayFirstClick = true;
                if (!this.Visible || (this.WindowState != FormWindowState.Normal))
                {
                    Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
                else
                {
                    Hide();
                }
            }
        }

        private void StrobeUtilityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exitNow)
            {
                if (TestVisible()) testWasVisible = true;
                this.Visible = false;
                e.Cancel = true;
            }
        }

        // Hide test on close-to-tray
        private void StrobeUtilityForm_VisibleChanged(object sender, EventArgs e)
        {
            if (launchTray && !trayFirstClick)
            {
                Hide();
            }
            else
            {
                this.Refresh();
                TestSyncVisibleState(this.Visible);
                if (this.Visible) PollMonitorForStrobeState();
            }
        }

        // Hide test on minimize
        private void StrobeUtilityForm_Resize(object sender, EventArgs e)
        {
            this.Refresh();
            TestSyncVisibleState(WindowState != FormWindowState.Minimized);
            PollMonitorForStrobeState();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                TestToggle();
                buttonTest.Visible = false;
                Thread.Sleep(50);
                buttonTest.Visible = true;
            }
        }

        /// <summary>Single-instance monitor for wake-up. Program.cs sends a signal if we're already running</summary>
        /// <param name="message">Message to process</param>
	    protected override void WndProc(ref Message message)
	    {
		    if (message.Msg == Program.WM_SHOWEXISTINGAPP)
		    {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                TestSyncVisibleState(true);
                PollMonitorForStrobeState();
            }
            else if (message.Msg == Program.WM_EXITEXISTINGAPP)
            {
                exitNow = true;
                Application.Exit();
            }
            base.WndProc(ref message);
	    }
    }
}
