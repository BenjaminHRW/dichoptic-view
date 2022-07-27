using Microsoft.Win32;
using System;
using System.Drawing;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using BlurBusters.UFO;

namespace BlurBusters
{
    public partial class StrobeUtilityForm : Form
    {
        const String DEFAULT_MESSAGEBOX_TITLE = "Blur Busters Strobe Utility";
        const String URL_MAIN = "https://www.viewsonic.com";
        const String URL_STROBEUTILITY = "https://www.blurbusters.com/";

        const string ERROR_DETAILS_UNSUPPORTED = "This monitor is not supported by this Strobe Utility app.";

        const string ERROR_DETAILS_DDCSTROBE = "Detected a potentially supported monitor.\n\n" +
            "Unfortunately, the monitor is not signalling support for " +
            "application-controlled strobe adjustments via DDC commands. " +
            "Please check your monitor firmware version and inform " +
            "Blur Busters at squad@blurbusters.com";

        const string ERROR_DETAILS_DDCDVI = "Detected a potentially supported monitor.\n\n" +
            "However, you appear to be using a DVI cable that might have a " +
            "missing DDC serial wire. Try a different DVI cable, or test a different " +
            "video output such as DisplayPort or HDMI.";

        const string ERROR_DETAILS_DDCFAIL = "Detected a potentially supported monitor.\n\n" +
            "However, Strobe Utility is unable to communicate to your monitor " +
            "right now. It is currently not responding to DDC commands.\n\n" +
            "- Exit your Factory Menu or Service Menu\n" +
            "- Make sure monitor burn-in mode is turned off\n" +
            "- Test monitor OSD main menu. Make sure it works.\n" +
            "- Turn on DDC/CI via Menu -> System -> DDC/CI -> ON\n" +
            "- Try a different DVI or DisplayPort video cable\n" +
            "- Try a different video port on your graphics card\n" +
            "- Try a different video port on your monitor\n" +
            "- Try adjusting from a different computer\n" +
            "- Try running as Administrator";

        const string ERROR_DETAILS_DDCFAIL_FORCED = "Strobe Utility is unable to communicate to " +
            "your monitor right now. It is currently not responding to DDC commands.\n\n" +
            "If you are absolutely sure that your monitor is indeed supported, then try the following:\n\n" +
            "- Exit your Factory Menu or Service Menu\n" +
            "- Make sure monitor burn-in mode is turned off\n" +
            "- Test monitor OSD main menu. Make sure it works.\n" +
            "- Turn on DDC/CI via Menu -> System -> DDC/CI -> ON\n" +
            "- Try a different DVI or DisplayPort video cable\n" +
            "- Try a different video port on your graphics card\n" +
            "- Try a different video port on your monitor\n" +
            "- Try adjusting from a different computer\n" +
            "- Try running as Administrator";

        const string ERROR_DETAILS_DDCWRITE = "Detected a potentially supported monitor.\n\n" +
            "However, your monitor is rejecting attempts to change " +
            "strobe settings. Adjustments are not working as a result.\n\n" +
            "- Try running as Administrator\n" +
            "- Restart your computer. Upgrade graphics drivers.\n" +
            "- Try a different DVI or DisplayPort video cable\n" +
            "- Try a different video port on your graphics card\n" +
            "- Try a different video port on your monitor\n" +
            "- Try adjusting from a different computer\n";

        const string ERROR_DETAILS_ADMIN = "Insufficient priveleges to operate this app.\n\n" +
            "Please restart this software as Administrator and then try again.";

        const string SUCCESS_DETAILS = "Your monitor is supported.\n\n" +
            "However, before adjusting the monitor, you may need to first " +
            "enable Blur Reduction or DyAc on your monitor before adjustments work.\n\n" +
            "If adjusting 'Strobe Length' does not change brightness, then " +
            "you need to enable Blur Reduction first via your monitor menus.\n\n" +
            "If your monitor has issues or blanks out while adjusting, " +
            "hit the ESC key to reset back to startup settings.";

        Color COLOR_LABEL_ENABLED = Color.White;
        Color COLOR_LABEL_DISABLED = Color.DimGray;

        /// <summary>Transparency of forn during TestUFO test</summary>
        const double OPACITY_DURING_TESTUFO = 0.25;

        static IntPtr windowHandle;
        static TestUFO motionTest;
        static bool motionTestFailed = false;
        static Thread thread;
        StartupForm startupForm = new StartupForm();
        BrowserLauncher launcher = new BrowserLauncher();
        StrobeCommander ddc = new StrobeCommander();
        StrobeConfig configFile = new StrobeConfig();
        StrobeConfigItem selectedConfigForced = null;
        StrobeConfigItem lastDetectedConfig = null;
        String lastDetectedDeviceID = "";
        Displays displays;
        DisplayInfo selectedDisplay;
        Version versionInfo = Assembly.GetEntryAssembly().GetName().Version;
        string versionText = "";
        string status = "uninitialized";
        string popupMessage = "";
        uint initializing = 0;
        bool adjustable = false;
        bool formHover = true;

        public StrobeUtilityForm()
        {
            InitializeComponent();
        }

        delegate void ShowScreenOnDisplayDelegate(DisplayInfo display);

        public void UFOLaunch(DisplayInfo display)
        {
            if ((thread == null) || !thread.IsAlive) 
            {
                windowHandle = this.Handle;
                thread = new Thread(UFOThread);
                thread.Start(selectedDisplay);
            }
            else if (motionTest != null)
            {
                motionTest.SetDisplay = display;
            }
        }

        public bool UFOVisible()
        {
            if (motionTest != null)
            {
                return motionTest.Visible;
            }
            return false;
        }

        public void UFOShow(bool show)
        {
            if (motionTest != null)
            {
                motionTest.Visible = show;
            }
        }

        public void UFOToggle()
        {
            if (!UFOVisible())
            {
                UFOLaunch(selectedDisplay);
                UFOShow(true);
                TopMost = true;
                this.Opacity = formHover ? 1.00 : OPACITY_DURING_TESTUFO;
                Window.SetForeground(Handle);
           }
            else
            {
                UFOShow(false);
                this.Opacity = 1.00;
                TopMost = false;
            }
        }

        public static void UFOThread(object display)
        {
            try
            {
                if (motionTest == null)
                {
                    motionTest = new TestUFO(windowHandle);
                    motionTest.Start((DisplayInfo)display);
                    motionTest = null;
                }
            }
            catch (Exception ex)
            {
                motionTestFailed = true;
                motionTest = null;
            }
        }

        /// <summary>Form startup</summary>
        private void StrobeUtilityForm_Load(object sender, EventArgs e)
        {
            versionText = versionInfo.Major + "." + versionInfo.Minor;
            if ((versionInfo.Build > 0) || (versionInfo.Revision > 0)) versionText += "." + versionInfo.Build;
            if (versionInfo.Revision > 0) versionText += "." + versionInfo.Revision;

            //labelVersion.Text = "Strobe Calibration Utility v" + versionText;

            ClearMessage();
            EnableAdjustments(false);
            if (!IsWin7orNewer())
            {
                ErrorMessage("ERROR", "You need Windows 7 or newer.");
                status = "oldwindows";
                return;
            }
            RefreshMonitorPulldown();
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        /// <summary>Begin a high-precision execution section</summary>
        public void BeginCriticalPriority()
        {
            Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            Thread.BeginCriticalRegion();
            Thread.BeginThreadAffinity();
        }

        /// <summary>Ends a high-precision execution section</summary>
        public void EndCriticalPriority()
        {
            Thread.EndThreadAffinity();
            Thread.EndCriticalRegion();
            Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        }

        /// <summary>Returns true if Windows 7 or newer</summary>
        private bool IsWin7orNewer()
        {
            OperatingSystem OS = Environment.OSVersion;
            if (OS.Platform != PlatformID.Win32NT) return false; // Not Windows
            if (OS.Version.Major < 6) return false; // Pre-Windows Vista unsupported
            if ((OS.Version.Major == 6) && (OS.Version.Minor == 0)) return false; // Windows Vista unsupported
            return true;
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
                "Try Running as Administrator. Right click on BlurBustersStrobeUtility, and select 'Run as administrator'",
                "Unauthorized Access Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Application.Exit();
        }

        private StrobeConfigItem CurrentStrobeConfig()
        {
            if (selectedConfigForced != null) return selectedConfigForced;
            return GetStrobeConfigForDisplay(selectedDisplay);
        }

        /// <summary>Automatically detect strobe configuration for monitor</summary>
        private StrobeConfigItem GetMonitorStrobeConfigByDeviceID(string displayProductID)
        {
            // Attempt automatic detect of monitor via Device ID
            foreach (StrobeConfigItem item in configFile.Items)
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
        private StrobeConfigItem GetStrobeConfigForDisplay(DisplayInfo display)
        {
            if (display == null) return null;

            // Quick check of last detected monitor
            if (lastDetectedConfig != null)
            {
                if (lastDetectedDeviceID == display.DeviceProductID) return lastDetectedConfig;
            }

            // Attempt automatic detect of monitor via Device ID
            StrobeConfigItem detectedConfig = GetMonitorStrobeConfigByDeviceID(display.DeviceProductID);
            if (detectedConfig != null)
            {
                lastDetectedConfig = detectedConfig;
                lastDetectedDeviceID = display.DeviceProductID;
                return lastDetectedConfig;
            }

            // Attempt automatic detect of monitor via Product ID
            foreach (StrobeConfigItem item in configFile.Items)
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
        private bool MonitorStrobeQuery(DisplayInfo display, StrobeConfigItem config = null)
        {
            if (config == null) config = GetStrobeConfigForDisplay(display);
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
            pictureChecked.Visible = false;
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

        /// <summary>Refreshes the secondary monitor selector</summary>
        private void RefreshForcedChooser(bool enableChooser)
        {
            if (comboBoxConfig.Items.Count == 0)
            {
                comboBoxConfig.Items.Add("Please Select...");
                foreach (StrobeConfigItem item in configFile.Items)
                {
                    comboBoxConfig.Items.Add(item);
                }
            }

            // Show/hide the pulldown if chooser is enabled
            comboBoxConfig.Visible = enableChooser;
            pictureChecked.Left = 5 + (comboBoxConfig.Visible ? comboBoxConfig.Right : comboBoxMonitors.Right);
            pictureCrossed.Left = pictureChecked.Left;

            // Autoselect the current strobe config in the pulldown list
            if (comboBoxConfig.Visible)
            {
                comboBoxConfig.SelectedIndex = 0;
                StrobeConfigItem currentConfig = CurrentStrobeConfig();
                if (currentConfig != null)
                {
                    foreach (object item in comboBoxConfig.Items)
                    {
                        if (item is StrobeConfigItem)
                        {
                            if (currentConfig.Name == item.ToString())
                            {
                                comboBoxConfig.SelectedItem = item;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Enable/disable the adjustments (appearance, sliders active, etc)</summary>
        private void EnableAdjustments(bool enable = true)
        {
            adjustable = enable;
            sliderStrobePhase.Enabled = enable;
            sliderStrobeLength.Enabled = enable;
            sliderOverdriveGain.Enabled = enable;
            comboBoxStrobe.Enabled = enable;
            if (!enable)
            {
                sliderStrobePhase.Value = sliderStrobePhase.Minimum;
                sliderStrobeLength.Value = sliderStrobeLength.Minimum;
                sliderOverdriveGain.Value = sliderOverdriveGain.Minimum;
            }

            labelEnableStrobe.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            //comboBoxStrobe.BackColor = enable ? COLOR_LABEL_ENABLED : Color.DarkGray;
            StrobeConfigItem config = CurrentStrobeConfig();

            labelPersistence.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            //labelPersistence1.ForeColor = enable ? Color.Lime : Color.DimGray;
            //labelPersistence2.ForeColor = enable ? Color.Lime : Color.DimGray;
            if (!enable) labelPersistenceValue.Text = "";
            labelPersistenceValue.Visible = enable;

            labelCrosstalk.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            //labelCrosstalk1.ForeColor = enable ? Color.Lime : Color.DimGray;
            //labelCrosstalk2.ForeColor = enable ? Color.Lime : Color.DimGray;
            if (!enable) labelCrosstalkValue.Text = "";
            labelCrosstalkValue.Visible = enable;

            labelOverdrive.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            if (!enable) labelOverdriveGainValue.Text = "";
            labelOverdriveGainValue.Visible = enable;

            labelVerticalTotal.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            if (!enable) labelVerticalTotalValue.Text = "";

            labelRefreshRate.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;
            labelRefreshRateValue.ForeColor = enable ? COLOR_LABEL_ENABLED : COLOR_LABEL_DISABLED;

            pictureChecked.Visible = enable && (selectedConfigForced == null); 
            pictureCrossed.Visible = !enable;
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
                else if ((comboBoxConfig.Visible) && !(comboBoxConfig.SelectedItem is StrobeConfigItem))
                {
                    WarningMessage("", "Please select monitor configuration above.", "");
                    status = "error-forcenotselected";
                    return;
                }

                StrobeConfigItem currentConfig = CurrentStrobeConfig();
                if (currentConfig == null)
                {
                    bool vcp10supported = DisplayCommander.VCPSupported(selectedDisplay, 0x10);
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
                else if (!MonitorStrobeQuery(selectedDisplay, currentConfig))
                {
                    bool vcp10supported = DisplayCommander.VCPSupported(selectedDisplay, 0x10);
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
                        SuccessMessage("", "Manual Mode: If working, send screenshot to BlurBusters. Device " + selectedDisplay.DeviceProductID + " via " + selectedDisplay.VideoPort + " using " + selectedConfigForced.Name + " settings.");
                    }
                    else
                    {
                        SuccessMessage("", "", SUCCESS_DETAILS);
                    }
                    status = "detected";
                }

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
                // FIXME: Doesn't launch properly upon startup
                /*
                if (status.StartsWith("detected"))
                {
                    UFOLaunch(selectedDisplay);
                }
                else
                {
                    UFOShow(false);
                }
                */
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

                // Scan monitors 
                startupForm.Show();
                startupForm.Message("Scanning Monitors...");
                displays = new Displays();
                if (displays.Count > 0) selectedDisplay = displays[0];
                startupForm.Message("Complete...");
                startupForm.Hide();

                // Update Strobe Utility UI
                comboBoxMonitors.Items.Clear();
                foreach (DisplayInfo item in displays)
                {
                    comboBoxMonitors.Items.Add(item);
                    if (item.ID == oldID)
                    {
                        // Maintain same selected monitor (during times when other monitors in multimonitor are hot-unplugged on the fly)
                        comboBoxMonitors.SelectedItem = item;
                    }
                }

                // Auto-select first strobe-commandable monitor
                if (comboBoxMonitors.SelectedIndex < 0)
                {
                    foreach (DisplayInfo item in comboBoxMonitors.Items)
                    {
                        if (MonitorStrobeQuery(item))
                        {
                            comboBoxMonitors.SelectedItem = item;
                            break;
                        }
                    }
                }

                // If fail, auto-select first supposely compatible monitor
                if (comboBoxMonitors.SelectedIndex < 0)
                {
                    foreach (DisplayInfo item in comboBoxMonitors.Items)
                    {
                        if (null != GetStrobeConfigForDisplay(item))
                        {
                            comboBoxMonitors.SelectedItem = item;
                            break;
                        }
                    }
                }

                // If all fail, auto-select first monitor in list
                if ((comboBoxMonitors.SelectedIndex < 0) && (comboBoxMonitors.Items.Count > 0))
                {
                    comboBoxMonitors.SelectedIndex = 0;
                }

                if (comboBoxMonitors.SelectedItem != null)
                {
                    selectedDisplay = (DisplayInfo)comboBoxMonitors.SelectedItem;
                    StrobeConfigItem currentConfig = GetStrobeConfigForDisplay(selectedDisplay);
                    if (currentConfig != null) RefreshForcedChooser(currentConfig.Choose);
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
            int strobeLen = 0;
            int strobePhase = 0;
            int overdriveGain = 0;

            if ((selectedDisplay == null) || !adjustable || !MonitorStrobeQuery(selectedDisplay, CurrentStrobeConfig()))
            {
                EnableAdjustments(false);
                return;
            }
            if ((configFile == null) || (configFile.Items == null) || (configFile.Items.Count == 0))
            {
                MessageBox.Show("Fatal error: Monitor supported but config is missing. Please screenshot this error message and send to squad@blurbusters.com");
                EnableAdjustments(false);
                return;
            }

            // Set adjustment ranges specific to this monitor.
            StrobeConfigItem selectedConfig = CurrentStrobeConfig();
            comboBoxStrobe.Items.Clear();
            comboBoxStrobe.Items.AddRange(selectedConfig.StrobeEnableList.ToArray());
            sliderStrobeLength.RightToLeft = (selectedConfig.StrobeLenReversed ? RightToLeft.Yes : RightToLeft.No);
            sliderStrobeLength.Minimum = (int)selectedConfig.StrobeLenMin;
            sliderStrobeLength.Maximum = (int)selectedConfig.StrobeLenMax;
            sliderStrobePhase.RightToLeft = (selectedConfig.StrobePhaseReversed ? RightToLeft.Yes : RightToLeft.No);
            sliderStrobePhase.Minimum = (int)selectedConfig.StrobePhaseMin;
            sliderStrobePhase.Maximum = (int)selectedConfig.StrobePhaseMax;
            sliderOverdriveGain.RightToLeft = (selectedConfig.OverdriveGainReversed ? RightToLeft.Yes : RightToLeft.No);
            sliderOverdriveGain.Minimum = (int)selectedConfig.OverdriveGainMin;
            sliderOverdriveGain.Maximum = (int)selectedConfig.OverdriveGainMax;

            // Retrieve default settings directly from first monitor
            try
            {
                comboBoxStrobe.SelectedIndex = ddc.Enable;
            }
            catch (Exception) { }
            //--------------
            try
            {
                strobeLen = (int)ddc.StrobeLen;
                if (strobeLen == 0)
                {
                    // Re-query if it's a 0, sometimes it's a glitch
                    strobeLen = ddc.StrobeLen;
                }
            }
            catch (Exception) { }
            try
            {
                sliderStrobeLength.Value = strobeLen;
            }
            catch (Exception)
            {
                if (sliderStrobeLength.Minimum > strobeLen)
                {
                    sliderStrobeLength.Value = sliderStrobeLength.Minimum;
                }
                if (sliderStrobeLength.Maximum < strobeLen)
                {
                    sliderStrobeLength.Value = sliderStrobeLength.Maximum;
                }
            }
            //--------------
            try
            {
                strobePhase = ddc.StrobePhase;
            }
            catch (Exception) { }
            try
            {
                sliderStrobePhase.Value = strobePhase;
            }
            catch (Exception)
            {
                if (sliderStrobePhase.Minimum > strobePhase)
                {
                    sliderStrobePhase.Value = sliderStrobePhase.Minimum;
                }
                if (sliderStrobePhase.Maximum < strobePhase)
                {
                    sliderStrobePhase.Value = sliderStrobePhase.Maximum;
                }
            }
            //--------------
            try
            {
                overdriveGain = ddc.OverdriveGain;
            }
            catch (Exception) { }
            try
            {
                sliderOverdriveGain.Value = overdriveGain;
            }
            catch (Exception)
            {
                if (sliderOverdriveGain.Minimum > overdriveGain)
                {
                    sliderOverdriveGain.Value = sliderOverdriveGain.Minimum;
                }
                if (sliderOverdriveGain.Maximum < overdriveGain)
                {
                    sliderOverdriveGain.Value = sliderOverdriveGain.Maximum;
                }
            }

            labelPersistenceValue.Text = strobeLen.ToString();
            labelCrosstalkValue.Text = strobePhase.ToString();                              
            labelOverdriveGainValue.Text = overdriveGain.ToString();
            labelRefreshRateValue.Text = selectedDisplay.RefreshRate.ToString("F1") + " Hz";
            labelVerticalTotalValue.Text = selectedDisplay.VertTotal.ToString();
            this.Refresh();
        }

        private void comboBoxStrobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SliderStrobePhase_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!adjustable || !sliderStrobePhase.Enabled) return;
                labelCrosstalkValue.Text = sliderStrobePhase.Value.ToString();
                labelCrosstalkValue.Refresh();
                if (initializing > 0) return;

                // TODO: if (!checkboxEnableStrobe.Checked) checkboxEnableStrobe.Checked = true;
                ddc.StrobePhase = sliderStrobePhase.Value;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Exception error");
            }
        }

        private void SliderStrobeLength_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!adjustable || !sliderStrobeLength.Enabled) return;
                labelPersistenceValue.Text = sliderStrobeLength.Value.ToString();
                labelPersistenceValue.Refresh();
                if (initializing > 0) return;

                // TODO: if (!checkboxEnableStrobe.Checked) checkboxEnableStrobe.Checked = true;
                ddc.StrobeLen = sliderStrobeLength.Value;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Exception error");
            }
        }

        private void sliderOverdriveGain_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!adjustable || !sliderOverdriveGain.Enabled) return;
                labelOverdriveGainValue.Text = sliderOverdriveGain.Value.ToString();
                labelOverdriveGainValue.Refresh();
                if (initializing > 0) return;

                // TODO: if (!checkboxEnableStrobe.Checked) checkboxEnableStrobe.Checked = true;
                ddc.OverdriveGain = sliderOverdriveGain.Value;
            }
            catch (UnauthorizedAccessException)
            {
                UnauthorizedAccessOccured();
            }
            catch
            {
                MessageBox.Show("Exception error");
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

        private String GetParams()
        {
            return "strobeutil=" + versionText + "&strobeutilstatus=" + status + "&device=" + ((selectedDisplay != null) ? selectedDisplay.DeviceProductID : "none");
        }

        private void LabelPersistenceMotionTest_Click(object sender, EventArgs e)
        {
            UFOToggle();
        }

        private void LabelCrosstalkMotionTest_Click(object sender, EventArgs e)
        {
            UFOToggle();
        }

        private void LabelUnsupportedHelp_Click(object sender, EventArgs e)
        {
        }

        private void LabelBlurBusters_Click(object sender, EventArgs e)
        {
            launcher.Launch(URL_STROBEUTILITY + GetParams());
        }

        private void StrobeUtilityForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        bool displayChanging = false;
        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (displayChanging) return; // TODO: Handle this better
            displayChanging = true;
            // Rez, Refresh changed or a monitor got added/removed.
            startupForm.Show();
            startupForm.Refresh();
            EnableAdjustments(false);
            RefreshMonitorPulldown();
            startupForm.Hide();
            displayChanging = false;
        }

        private void Popup_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(popupMessage))
            {
                MessageBox.Show(popupMessage,
                    DEFAULT_MESSAGEBOX_TITLE,
                    MessageBoxButtons.OK,
                    (labelMessageText.BackColor == Color.Red) ? MessageBoxIcon.Error : MessageBoxIcon.Information);
            }
        }

        private void PictureChecked_MouseEnter(object sender, EventArgs e)
        {
            pictureChecked.BackColor = Color.White;
        }

        private void PictureChecked_MouseLeave(object sender, EventArgs e)
        {
            pictureChecked.BackColor = Color.Transparent;
        }

        private void PictureCrossed_MouseEnter(object sender, EventArgs e)
        {
            pictureCrossed.BackColor = Color.White;
        }

        private void PictureCrossed_MouseLeave(object sender, EventArgs e)
        {
            pictureCrossed.BackColor = Color.Transparent;
        }

        private bool forceRefresh = false;
        private void ComboBoxMonitors_SelectionChangeCommitted(object sender, EventArgs e)
        {
            selectedConfigForced = null;
            if (forceRefresh) ddc.CurrentDisplay = null;
            if (comboBoxMonitors.SelectedItem is DisplayInfo)
            { 
                selectedDisplay = (DisplayInfo)comboBoxMonitors.SelectedItem;
            }
            StrobeConfigItem currentConfig = GetStrobeConfigForDisplay(selectedDisplay);
            RefreshForcedChooser((currentConfig != null) ? currentConfig.Choose : false);
            RefreshMonitorMessage();
        }

        private void ComboBoxConfig_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (forceRefresh) ddc.CurrentDisplay = null;
            if (comboBoxConfig.SelectedItem is StrobeConfigItem)
            {
                selectedConfigForced = (StrobeConfigItem)comboBoxConfig.SelectedItem;
            }
            else
            {
                selectedConfigForced = null;
            }
            RefreshMonitorMessage();
        }

        private void ComboBoxMonitors_Click(object sender, EventArgs e)
        {
            forceRefresh = true;
        }

        private void StrobeUtilityForm_KeyDown(object sender, KeyEventArgs e)
        {
            bool emergencyClose = (e.Alt && (e.KeyCode == System.Windows.Forms.Keys.F4));
            if ((e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Enter) || emergencyClose)
            {
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
            }
            else if (e.KeyCode == Keys.F)
            {
                RefreshForcedChooser(true);
                WarningMessage("", "Manual strobe configuration chooser enabled.\nPlease select overriding configuration.");
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Space)
            {
                UFOToggle();
                e.Handled = true;
            }
            
        }

        private void StrobeUtilityForm_Activated(object sender, EventArgs e)
        {
            // Re-force motion test back to foreground if visible.
            if ((motionTest != null) && motionTest.Visible)
            {
                TopMost = true;
                motionTest.Visible = true;
            }
        }

        private void StrobeUtilityForm_Deactivate(object sender, EventArgs e)
        {
            TopMost = false;
        }

        private void comboBoxMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StrobeUtilityForm_MouseEnter(object sender, EventArgs e)
        {
            //formHover = true;
            //if (UFOVisible()) this.Opacity = 1.00;
        }

        private void StrobeUtilityForm_MouseLeave(object sender, EventArgs e)
        {
            //formHover = false;
            //if (UFOVisible()) this.Opacity = OPACITY_DURING_TESTUFO;
        }
    }
}
