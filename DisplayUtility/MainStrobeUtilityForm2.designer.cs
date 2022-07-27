namespace BlurBusters
{
    partial class StrobeUtilityForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrobeUtilityForm));
            this.labelBlurBusters = new System.Windows.Forms.Label();
            this.comboBoxMonitors = new System.Windows.Forms.ComboBox();
            this.labelMonitor = new System.Windows.Forms.Label();
            this.labelEnableStrobe = new System.Windows.Forms.Label();
            this.labelPersistence = new System.Windows.Forms.Label();
            this.sliderStrobeLength = new System.Windows.Forms.TrackBar();
            this.labelCrosstalk = new System.Windows.Forms.Label();
            this.sliderStrobePhase = new System.Windows.Forms.TrackBar();
            this.labelPersistenceValue = new System.Windows.Forms.Label();
            this.labelCrosstalkValue = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelMessageLabel = new System.Windows.Forms.Label();
            this.labelMessageText = new System.Windows.Forms.Label();
            this.comboBoxConfig = new System.Windows.Forms.ComboBox();
            this.pictureChecked = new System.Windows.Forms.PictureBox();
            this.pictureCrossed = new System.Windows.Forms.PictureBox();
            this.backgroundBanner = new System.Windows.Forms.PictureBox();
            this.backgroundLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelOverdrive = new System.Windows.Forms.Label();
            this.sliderOverdriveGain = new System.Windows.Forms.TrackBar();
            this.labelOverdriveGainValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelVerticalTotal = new System.Windows.Forms.Label();
            this.labelVerticalTotalValue = new System.Windows.Forms.Label();
            this.labelRefreshRateValue = new System.Windows.Forms.Label();
            this.labelRefreshRate = new System.Windows.Forms.Label();
            this.comboBoxStrobe = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.sliderStrobeLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderStrobePhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChecked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCrossed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundLogo)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderOverdriveGain)).BeginInit();
            this.SuspendLayout();
            // 
            // labelBlurBusters
            // 
            this.labelBlurBusters.AutoSize = true;
            this.labelBlurBusters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBlurBusters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.labelBlurBusters.Location = new System.Drawing.Point(857, 610);
            this.labelBlurBusters.Name = "labelBlurBusters";
            this.labelBlurBusters.Size = new System.Drawing.Size(0, 13);
            this.labelBlurBusters.TabIndex = 5;
            this.labelBlurBusters.Click += new System.EventHandler(this.LabelBlurBusters_Click);
            // 
            // comboBoxMonitors
            // 
            this.comboBoxMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonitors.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMonitors.FormattingEnabled = true;
            this.comboBoxMonitors.Location = new System.Drawing.Point(513, 21);
            this.comboBoxMonitors.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMonitors.Name = "comboBoxMonitors";
            this.comboBoxMonitors.Size = new System.Drawing.Size(210, 26);
            this.comboBoxMonitors.TabIndex = 1;
            this.comboBoxMonitors.SelectedIndexChanged += new System.EventHandler(this.comboBoxMonitors_SelectedIndexChanged);
            this.comboBoxMonitors.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxMonitors_SelectionChangeCommitted);
            this.comboBoxMonitors.Click += new System.EventHandler(this.ComboBoxMonitors_Click);
            // 
            // labelMonitor
            // 
            this.labelMonitor.BackColor = System.Drawing.Color.Transparent;
            this.labelMonitor.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMonitor.ForeColor = System.Drawing.Color.White;
            this.labelMonitor.Location = new System.Drawing.Point(303, 22);
            this.labelMonitor.Name = "labelMonitor";
            this.labelMonitor.Size = new System.Drawing.Size(203, 28);
            this.labelMonitor.TabIndex = 0;
            this.labelMonitor.Text = "Monitor";
            this.labelMonitor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelEnableStrobe
            // 
            this.labelEnableStrobe.AccessibleDescription = "Enable motion blur reduction";
            this.labelEnableStrobe.BackColor = System.Drawing.Color.Transparent;
            this.labelEnableStrobe.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnableStrobe.ForeColor = System.Drawing.Color.White;
            this.labelEnableStrobe.Location = new System.Drawing.Point(305, 64);
            this.labelEnableStrobe.Name = "labelEnableStrobe";
            this.labelEnableStrobe.Size = new System.Drawing.Size(201, 29);
            this.labelEnableStrobe.TabIndex = 4;
            this.labelEnableStrobe.Text = "PureXP+";
            this.labelEnableStrobe.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelPersistence
            // 
            this.labelPersistence.AccessibleDescription = "Length of strobe flash.";
            this.labelPersistence.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPersistence.ForeColor = System.Drawing.Color.White;
            this.labelPersistence.Location = new System.Drawing.Point(305, 109);
            this.labelPersistence.Name = "labelPersistence";
            this.labelPersistence.Size = new System.Drawing.Size(201, 28);
            this.labelPersistence.TabIndex = 7;
            this.labelPersistence.Text = "Strobe Pulse Width";
            this.labelPersistence.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderStrobeLength
            // 
            this.sliderStrobeLength.Location = new System.Drawing.Point(555, 111);
            this.sliderStrobeLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sliderStrobeLength.Maximum = 30;
            this.sliderStrobeLength.Minimum = 3;
            this.sliderStrobeLength.Name = "sliderStrobeLength";
            this.sliderStrobeLength.Size = new System.Drawing.Size(527, 45);
            this.sliderStrobeLength.TabIndex = 12;
            this.sliderStrobeLength.TickFrequency = 3;
            this.sliderStrobeLength.Value = 12;
            this.sliderStrobeLength.ValueChanged += new System.EventHandler(this.SliderStrobeLength_ValueChanged);
            // 
            // labelCrosstalk
            // 
            this.labelCrosstalk.AccessibleDescription = "Phase of strobe flash relative to VSYNC. ";
            this.labelCrosstalk.BackColor = System.Drawing.Color.Transparent;
            this.labelCrosstalk.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCrosstalk.ForeColor = System.Drawing.Color.White;
            this.labelCrosstalk.Location = new System.Drawing.Point(305, 154);
            this.labelCrosstalk.Name = "labelCrosstalk";
            this.labelCrosstalk.Size = new System.Drawing.Size(201, 28);
            this.labelCrosstalk.TabIndex = 14;
            this.labelCrosstalk.Text = "Strobe Pulse Phase";
            this.labelCrosstalk.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderStrobePhase
            // 
            this.sliderStrobePhase.Location = new System.Drawing.Point(555, 158);
            this.sliderStrobePhase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sliderStrobePhase.Maximum = 47;
            this.sliderStrobePhase.Name = "sliderStrobePhase";
            this.sliderStrobePhase.Size = new System.Drawing.Size(527, 45);
            this.sliderStrobePhase.TabIndex = 19;
            this.sliderStrobePhase.TickFrequency = 4;
            this.sliderStrobePhase.ValueChanged += new System.EventHandler(this.SliderStrobePhase_ValueChanged);
            // 
            // labelPersistenceValue
            // 
            this.labelPersistenceValue.AutoSize = true;
            this.labelPersistenceValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelPersistenceValue.ForeColor = System.Drawing.Color.White;
            this.labelPersistenceValue.Location = new System.Drawing.Point(507, 109);
            this.labelPersistenceValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPersistenceValue.Name = "labelPersistenceValue";
            this.labelPersistenceValue.Size = new System.Drawing.Size(26, 29);
            this.labelPersistenceValue.TabIndex = 13;
            this.labelPersistenceValue.Text = "0";
            // 
            // labelCrosstalkValue
            // 
            this.labelCrosstalkValue.AutoSize = true;
            this.labelCrosstalkValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelCrosstalkValue.ForeColor = System.Drawing.Color.White;
            this.labelCrosstalkValue.Location = new System.Drawing.Point(507, 154);
            this.labelCrosstalkValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCrosstalkValue.Name = "labelCrosstalkValue";
            this.labelCrosstalkValue.Size = new System.Drawing.Size(26, 29);
            this.labelCrosstalkValue.TabIndex = 20;
            this.labelCrosstalkValue.Text = "0";
            // 
            // labelVersion
            // 
            this.labelVersion.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.Blue;
            this.labelVersion.Location = new System.Drawing.Point(28, 269);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(245, 22);
            this.labelVersion.TabIndex = 41;
            this.labelVersion.Text = "Strobe Utility by";
            this.labelVersion.Click += new System.EventHandler(this.LabelBlurBusters_Click);
            // 
            // labelMessageLabel
            // 
            this.labelMessageLabel.AccessibleDescription = "";
            this.labelMessageLabel.BackColor = System.Drawing.Color.Red;
            this.labelMessageLabel.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessageLabel.ForeColor = System.Drawing.Color.Yellow;
            this.labelMessageLabel.Location = new System.Drawing.Point(298, 275);
            this.labelMessageLabel.Name = "labelMessageLabel";
            this.labelMessageLabel.Size = new System.Drawing.Size(174, 56);
            this.labelMessageLabel.TabIndex = 3;
            this.labelMessageLabel.Text = "ERROR";
            this.labelMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelMessageLabel.Click += new System.EventHandler(this.Popup_Click);
            // 
            // labelMessageText
            // 
            this.labelMessageText.BackColor = System.Drawing.Color.Red;
            this.labelMessageText.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessageText.ForeColor = System.Drawing.Color.Yellow;
            this.labelMessageText.Location = new System.Drawing.Point(470, 275);
            this.labelMessageText.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMessageText.Name = "labelMessageText";
            this.labelMessageText.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMessageText.Size = new System.Drawing.Size(636, 56);
            this.labelMessageText.TabIndex = 3;
            this.labelMessageText.Text = "Message Here";
            this.labelMessageText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMessageText.Click += new System.EventHandler(this.Popup_Click);
            // 
            // comboBoxConfig
            // 
            this.comboBoxConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConfig.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxConfig.FormattingEnabled = true;
            this.comboBoxConfig.Location = new System.Drawing.Point(799, 21);
            this.comboBoxConfig.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxConfig.Name = "comboBoxConfig";
            this.comboBoxConfig.Size = new System.Drawing.Size(128, 26);
            this.comboBoxConfig.TabIndex = 2;
            this.comboBoxConfig.Visible = false;
            this.comboBoxConfig.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxConfig_SelectionChangeCommitted);
            // 
            // pictureChecked
            // 
            this.pictureChecked.BackColor = System.Drawing.Color.Transparent;
            this.pictureChecked.ErrorImage = null;
            this.pictureChecked.Image = ((System.Drawing.Image)(resources.GetObject("pictureChecked.Image")));
            this.pictureChecked.InitialImage = null;
            this.pictureChecked.Location = new System.Drawing.Point(728, 12);
            this.pictureChecked.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureChecked.Name = "pictureChecked";
            this.pictureChecked.Size = new System.Drawing.Size(43, 43);
            this.pictureChecked.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureChecked.TabIndex = 17;
            this.pictureChecked.TabStop = false;
            this.pictureChecked.Click += new System.EventHandler(this.Popup_Click);
            this.pictureChecked.MouseEnter += new System.EventHandler(this.PictureChecked_MouseEnter);
            this.pictureChecked.MouseLeave += new System.EventHandler(this.PictureChecked_MouseLeave);
            // 
            // pictureCrossed
            // 
            this.pictureCrossed.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureCrossed.ErrorImage")));
            this.pictureCrossed.Image = ((System.Drawing.Image)(resources.GetObject("pictureCrossed.Image")));
            this.pictureCrossed.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureCrossed.InitialImage")));
            this.pictureCrossed.Location = new System.Drawing.Point(730, 12);
            this.pictureCrossed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureCrossed.Name = "pictureCrossed";
            this.pictureCrossed.Size = new System.Drawing.Size(41, 43);
            this.pictureCrossed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureCrossed.TabIndex = 10;
            this.pictureCrossed.TabStop = false;
            this.pictureCrossed.Click += new System.EventHandler(this.Popup_Click);
            this.pictureCrossed.MouseEnter += new System.EventHandler(this.PictureCrossed_MouseEnter);
            this.pictureCrossed.MouseLeave += new System.EventHandler(this.PictureCrossed_MouseLeave);
            // 
            // backgroundBanner
            // 
            this.backgroundBanner.BackColor = System.Drawing.Color.Transparent;
            this.backgroundBanner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backgroundBanner.Image = global::BlurBusters.Properties.Resources.blurbusterstext;
            this.backgroundBanner.Location = new System.Drawing.Point(144, 268);
            this.backgroundBanner.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundBanner.Name = "backgroundBanner";
            this.backgroundBanner.Size = new System.Drawing.Size(129, 22);
            this.backgroundBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backgroundBanner.TabIndex = 40;
            this.backgroundBanner.TabStop = false;
            this.backgroundBanner.Click += new System.EventHandler(this.LabelBlurBusters_Click);
            // 
            // backgroundLogo
            // 
            this.backgroundLogo.Image = global::BlurBusters.Properties.Resources.Viewsonic;
            this.backgroundLogo.Location = new System.Drawing.Point(12, 12);
            this.backgroundLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.backgroundLogo.Name = "backgroundLogo";
            this.backgroundLogo.Size = new System.Drawing.Size(271, 207);
            this.backgroundLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backgroundLogo.TabIndex = 0;
            this.backgroundLogo.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.backgroundLogo);
            this.panel1.Controls.Add(this.backgroundBanner);
            this.panel1.Controls.Add(this.labelVersion);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 331);
            this.panel1.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(28, 296);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 22);
            this.label1.TabIndex = 42;
            this.label1.Text = "v2.1.0";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelOverdrive
            // 
            this.labelOverdrive.AccessibleDescription = "";
            this.labelOverdrive.BackColor = System.Drawing.Color.Transparent;
            this.labelOverdrive.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOverdrive.ForeColor = System.Drawing.Color.White;
            this.labelOverdrive.Location = new System.Drawing.Point(303, 198);
            this.labelOverdrive.Name = "labelOverdrive";
            this.labelOverdrive.Size = new System.Drawing.Size(203, 31);
            this.labelOverdrive.TabIndex = 43;
            this.labelOverdrive.Text = "Overdrive Gain";
            this.labelOverdrive.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderOverdriveGain
            // 
            this.sliderOverdriveGain.Location = new System.Drawing.Point(555, 202);
            this.sliderOverdriveGain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sliderOverdriveGain.Maximum = 30;
            this.sliderOverdriveGain.Minimum = 3;
            this.sliderOverdriveGain.Name = "sliderOverdriveGain";
            this.sliderOverdriveGain.Size = new System.Drawing.Size(527, 45);
            this.sliderOverdriveGain.TabIndex = 44;
            this.sliderOverdriveGain.TickFrequency = 3;
            this.sliderOverdriveGain.Value = 12;
            this.sliderOverdriveGain.ValueChanged += new System.EventHandler(this.sliderOverdriveGain_ValueChanged);
            // 
            // labelOverdriveGainValue
            // 
            this.labelOverdriveGainValue.AutoSize = true;
            this.labelOverdriveGainValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelOverdriveGainValue.ForeColor = System.Drawing.Color.White;
            this.labelOverdriveGainValue.Location = new System.Drawing.Point(507, 198);
            this.labelOverdriveGainValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOverdriveGainValue.Name = "labelOverdriveGainValue";
            this.labelOverdriveGainValue.Size = new System.Drawing.Size(26, 29);
            this.labelOverdriveGainValue.TabIndex = 45;
            this.labelOverdriveGainValue.Text = "0";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(346, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select Monitor";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelVerticalTotal
            // 
            this.labelVerticalTotal.AccessibleDescription = "";
            this.labelVerticalTotal.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVerticalTotal.ForeColor = System.Drawing.Color.White;
            this.labelVerticalTotal.Location = new System.Drawing.Point(302, 282);
            this.labelVerticalTotal.Name = "labelVerticalTotal";
            this.labelVerticalTotal.Size = new System.Drawing.Size(203, 31);
            this.labelVerticalTotal.TabIndex = 46;
            this.labelVerticalTotal.Text = "Vertical Total";
            this.labelVerticalTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelVerticalTotalValue
            // 
            this.labelVerticalTotalValue.AutoSize = true;
            this.labelVerticalTotalValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelVerticalTotalValue.ForeColor = System.Drawing.Color.White;
            this.labelVerticalTotalValue.Location = new System.Drawing.Point(507, 282);
            this.labelVerticalTotalValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVerticalTotalValue.Name = "labelVerticalTotalValue";
            this.labelVerticalTotalValue.Size = new System.Drawing.Size(26, 29);
            this.labelVerticalTotalValue.TabIndex = 47;
            this.labelVerticalTotalValue.Text = "0";
            // 
            // labelRefreshRateValue
            // 
            this.labelRefreshRateValue.AutoSize = true;
            this.labelRefreshRateValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelRefreshRateValue.ForeColor = System.Drawing.Color.White;
            this.labelRefreshRateValue.Location = new System.Drawing.Point(507, 240);
            this.labelRefreshRateValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRefreshRateValue.Name = "labelRefreshRateValue";
            this.labelRefreshRateValue.Size = new System.Drawing.Size(26, 29);
            this.labelRefreshRateValue.TabIndex = 49;
            this.labelRefreshRateValue.Text = "0";
            // 
            // labelRefreshRate
            // 
            this.labelRefreshRate.AccessibleDescription = "";
            this.labelRefreshRate.BackColor = System.Drawing.Color.Transparent;
            this.labelRefreshRate.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRefreshRate.ForeColor = System.Drawing.Color.White;
            this.labelRefreshRate.Location = new System.Drawing.Point(302, 240);
            this.labelRefreshRate.Name = "labelRefreshRate";
            this.labelRefreshRate.Size = new System.Drawing.Size(203, 31);
            this.labelRefreshRate.TabIndex = 48;
            this.labelRefreshRate.Text = "Refresh Rate";
            this.labelRefreshRate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxStrobe
            // 
            this.comboBoxStrobe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStrobe.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxStrobe.FormattingEnabled = true;
            this.comboBoxStrobe.Location = new System.Drawing.Point(513, 65);
            this.comboBoxStrobe.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxStrobe.Name = "comboBoxStrobe";
            this.comboBoxStrobe.Size = new System.Drawing.Size(145, 26);
            this.comboBoxStrobe.TabIndex = 50;
            this.comboBoxStrobe.SelectedIndexChanged += new System.EventHandler(this.comboBoxStrobe_SelectedIndexChanged);
            // 
            // StrobeUtilityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1105, 331);
            this.Controls.Add(this.comboBoxStrobe);
            this.Controls.Add(this.labelMessageText);
            this.Controls.Add(this.labelMessageLabel);
            this.Controls.Add(this.labelRefreshRateValue);
            this.Controls.Add(this.labelRefreshRate);
            this.Controls.Add(this.labelOverdriveGainValue);
            this.Controls.Add(this.sliderOverdriveGain);
            this.Controls.Add(this.labelOverdrive);
            this.Controls.Add(this.sliderStrobePhase);
            this.Controls.Add(this.sliderStrobeLength);
            this.Controls.Add(this.labelCrosstalkValue);
            this.Controls.Add(this.labelPersistenceValue);
            this.Controls.Add(this.labelCrosstalk);
            this.Controls.Add(this.labelPersistence);
            this.Controls.Add(this.labelEnableStrobe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelMonitor);
            this.Controls.Add(this.comboBoxMonitors);
            this.Controls.Add(this.pictureChecked);
            this.Controls.Add(this.pictureCrossed);
            this.Controls.Add(this.comboBoxConfig);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelBlurBusters);
            this.Controls.Add(this.labelVerticalTotalValue);
            this.Controls.Add(this.labelVerticalTotal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(980, 40);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "StrobeUtilityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewSonic Strobe Utility by BlurBusters";
            this.Activated += new System.EventHandler(this.StrobeUtilityForm_Activated);
            this.Deactivate += new System.EventHandler(this.StrobeUtilityForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StrobeUtilityForm_FormClosed);
            this.Load += new System.EventHandler(this.StrobeUtilityForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StrobeUtilityForm_KeyDown);
            this.MouseEnter += new System.EventHandler(this.StrobeUtilityForm_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.StrobeUtilityForm_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.sliderStrobeLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderStrobePhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChecked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCrossed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sliderOverdriveGain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelBlurBusters;
        private System.Windows.Forms.ComboBox comboBoxMonitors;
        private System.Windows.Forms.Label labelMonitor;
        private System.Windows.Forms.Label labelEnableStrobe;
        private System.Windows.Forms.Label labelPersistence;
        private System.Windows.Forms.TrackBar sliderStrobeLength;
        private System.Windows.Forms.Label labelCrosstalk;
        private System.Windows.Forms.TrackBar sliderStrobePhase;
        private System.Windows.Forms.PictureBox pictureChecked;
        private System.Windows.Forms.PictureBox pictureCrossed;
        private System.Windows.Forms.Label labelPersistenceValue;
        private System.Windows.Forms.Label labelCrosstalkValue;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelMessageLabel;
        private System.Windows.Forms.Label labelMessageText;
        private System.Windows.Forms.ComboBox comboBoxConfig;
        private System.Windows.Forms.PictureBox backgroundBanner;
        private System.Windows.Forms.PictureBox backgroundLogo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelOverdrive;
        private System.Windows.Forms.TrackBar sliderOverdriveGain;
        private System.Windows.Forms.Label labelOverdriveGainValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelVerticalTotal;
        private System.Windows.Forms.Label labelVerticalTotalValue;
        private System.Windows.Forms.Label labelRefreshRateValue;
        private System.Windows.Forms.Label labelRefreshRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxStrobe;
    }
}

