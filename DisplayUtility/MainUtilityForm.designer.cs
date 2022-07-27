namespace RejTech
{
    partial class MainUtilityForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUtilityForm));
            this.comboBoxDisplaysList = new System.Windows.Forms.ComboBox();
            this.labelDisplay = new System.Windows.Forms.Label();
            this.labelField2 = new System.Windows.Forms.Label();
            this.labelField3 = new System.Windows.Forms.Label();
            this.sliderField3 = new System.Windows.Forms.TrackBar();
            this.labelField4 = new System.Windows.Forms.Label();
            this.sliderField4 = new System.Windows.Forms.TrackBar();
            this.labelField3Value = new System.Windows.Forms.Label();
            this.labelField4Value = new System.Windows.Forms.Label();
            this.labelMessageLabel = new System.Windows.Forms.Label();
            this.labelMessageText = new System.Windows.Forms.Label();
            this.labelField5 = new System.Windows.Forms.Label();
            this.sliderField5 = new System.Windows.Forms.TrackBar();
            this.labelField5Value = new System.Windows.Forms.Label();
            this.labelField7 = new System.Windows.Forms.Label();
            this.labelField7Value = new System.Windows.Forms.Label();
            this.labelField6Value = new System.Windows.Forms.Label();
            this.labelField6 = new System.Windows.Forms.Label();
            this.comboBoxFIeld2 = new System.Windows.Forms.ComboBox();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuTrayFull = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuSection = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extremeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitStrobeUtilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sliderField3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderField4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderField5)).BeginInit();
            this.contextMenuTrayFull.SuspendLayout();
            this.contextMenuTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxDisplaysList
            // 
            this.comboBoxDisplaysList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplaysList.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxDisplaysList.FormattingEnabled = true;
            this.comboBoxDisplaysList.Location = new System.Drawing.Point(255, 21);
            this.comboBoxDisplaysList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxDisplaysList.Name = "comboBoxDisplaysList";
            this.comboBoxDisplaysList.Size = new System.Drawing.Size(210, 26);
            this.comboBoxDisplaysList.TabIndex = 2;
            this.comboBoxDisplaysList.SelectedIndexChanged += new System.EventHandler(this.comboBoxMonitors_SelectedIndexChanged);
            this.comboBoxDisplaysList.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxMonitors_SelectionChangeCommitted);
            this.comboBoxDisplaysList.Click += new System.EventHandler(this.ComboBoxMonitors_Click);
            // 
            // labelDisplay
            // 
            this.labelDisplay.AccessibleDescription = "Select monitor to adjust.";
            this.labelDisplay.BackColor = System.Drawing.Color.Transparent;
            this.labelDisplay.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDisplay.ForeColor = System.Drawing.Color.White;
            this.labelDisplay.Location = new System.Drawing.Point(10, 22);
            this.labelDisplay.Name = "labelDisplay";
            this.labelDisplay.Size = new System.Drawing.Size(238, 28);
            this.labelDisplay.TabIndex = 1;
            this.labelDisplay.Text = "Display:";
            this.labelDisplay.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelField2
            // 
            this.labelField2.AccessibleDescription = "Enable motion blur reduction";
            this.labelField2.BackColor = System.Drawing.Color.Transparent;
            this.labelField2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelField2.ForeColor = System.Drawing.Color.White;
            this.labelField2.Location = new System.Drawing.Point(10, 65);
            this.labelField2.Name = "labelField2";
            this.labelField2.Size = new System.Drawing.Size(236, 29);
            this.labelField2.TabIndex = 5;
            this.labelField2.Text = "Field2";
            this.labelField2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelField3
            // 
            this.labelField3.AccessibleDescription = "Bigger Strobe Pulse Width values are brighter but produce more motion blur.  Adju" +
    "st to user preference.";
            this.labelField3.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelField3.ForeColor = System.Drawing.Color.White;
            this.labelField3.Location = new System.Drawing.Point(12, 109);
            this.labelField3.Name = "labelField3";
            this.labelField3.Size = new System.Drawing.Size(236, 28);
            this.labelField3.TabIndex = 7;
            this.labelField3.Text = "Field3";
            this.labelField3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderField3
            // 
            this.sliderField3.Location = new System.Drawing.Point(297, 111);
            this.sliderField3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sliderField3.Maximum = 30;
            this.sliderField3.Minimum = 1;
            this.sliderField3.Name = "sliderField3";
            this.sliderField3.Size = new System.Drawing.Size(798, 45);
            this.sliderField3.TabIndex = 9;
            this.sliderField3.TickFrequency = 3;
            this.sliderField3.Value = 1;
            this.sliderField3.ValueChanged += new System.EventHandler(this.SliderStrobeLength_ValueChanged);
            // 
            // labelField4
            // 
            this.labelField4.AccessibleDescription = "";
            this.labelField4.BackColor = System.Drawing.Color.Transparent;
            this.labelField4.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelField4.ForeColor = System.Drawing.Color.White;
            this.labelField4.Location = new System.Drawing.Point(12, 154);
            this.labelField4.Name = "labelField4";
            this.labelField4.Size = new System.Drawing.Size(236, 28);
            this.labelField4.TabIndex = 10;
            this.labelField4.Text = "Field4";
            this.labelField4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderField4
            // 
            this.sliderField4.Location = new System.Drawing.Point(297, 157);
            this.sliderField4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sliderField4.Maximum = 47;
            this.sliderField4.Name = "sliderField4";
            this.sliderField4.Size = new System.Drawing.Size(798, 45);
            this.sliderField4.TabIndex = 12;
            this.sliderField4.TickFrequency = 4;
            this.sliderField4.ValueChanged += new System.EventHandler(this.SliderStrobePhase_ValueChanged);
            // 
            // labelField3Value
            // 
            this.labelField3Value.AutoSize = true;
            this.labelField3Value.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelField3Value.ForeColor = System.Drawing.Color.White;
            this.labelField3Value.Location = new System.Drawing.Point(249, 109);
            this.labelField3Value.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelField3Value.Name = "labelField3Value";
            this.labelField3Value.Size = new System.Drawing.Size(26, 29);
            this.labelField3Value.TabIndex = 8;
            this.labelField3Value.Text = "0";
            // 
            // labelField4Value
            // 
            this.labelField4Value.AutoSize = true;
            this.labelField4Value.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelField4Value.ForeColor = System.Drawing.Color.White;
            this.labelField4Value.Location = new System.Drawing.Point(249, 154);
            this.labelField4Value.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelField4Value.Name = "labelField4Value";
            this.labelField4Value.Size = new System.Drawing.Size(26, 29);
            this.labelField4Value.TabIndex = 11;
            this.labelField4Value.Text = "0";
            // 
            // labelMessageLabel
            // 
            this.labelMessageLabel.AccessibleDescription = "";
            this.labelMessageLabel.BackColor = System.Drawing.Color.Red;
            this.labelMessageLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessageLabel.ForeColor = System.Drawing.Color.Yellow;
            this.labelMessageLabel.Location = new System.Drawing.Point(0, 275);
            this.labelMessageLabel.Name = "labelMessageLabel";
            this.labelMessageLabel.Size = new System.Drawing.Size(251, 56);
            this.labelMessageLabel.TabIndex = 20;
            this.labelMessageLabel.Text = "ERROR";
            this.labelMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelMessageLabel.Click += new System.EventHandler(this.Popup_Click);
            // 
            // labelMessageText
            // 
            this.labelMessageText.BackColor = System.Drawing.Color.Red;
            this.labelMessageText.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessageText.ForeColor = System.Drawing.Color.Yellow;
            this.labelMessageText.Location = new System.Drawing.Point(248, 275);
            this.labelMessageText.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMessageText.Name = "labelMessageText";
            this.labelMessageText.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMessageText.Size = new System.Drawing.Size(858, 56);
            this.labelMessageText.TabIndex = 21;
            this.labelMessageText.Text = "Message Here";
            this.labelMessageText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMessageText.Click += new System.EventHandler(this.Popup_Click);
            // 
            // labelField5
            // 
            this.labelField5.AccessibleDescription = "Adjusts the amount of overdrive during motion blur reduction, to minimize strobe " +
    "crosstalk double images.";
            this.labelField5.BackColor = System.Drawing.Color.Transparent;
            this.labelField5.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelField5.ForeColor = System.Drawing.Color.White;
            this.labelField5.Location = new System.Drawing.Point(10, 198);
            this.labelField5.Name = "labelField5";
            this.labelField5.Size = new System.Drawing.Size(238, 31);
            this.labelField5.TabIndex = 13;
            this.labelField5.Text = "Field5";
            this.labelField5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderField5
            // 
            this.sliderField5.Location = new System.Drawing.Point(297, 202);
            this.sliderField5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sliderField5.Maximum = 100;
            this.sliderField5.Name = "sliderField5";
            this.sliderField5.Size = new System.Drawing.Size(798, 45);
            this.sliderField5.TabIndex = 15;
            this.sliderField5.TickFrequency = 3;
            this.sliderField5.ValueChanged += new System.EventHandler(this.SliderOverdriveGain_ValueChanged);
            // 
            // labelField5Value
            // 
            this.labelField5Value.AutoSize = true;
            this.labelField5Value.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelField5Value.ForeColor = System.Drawing.Color.White;
            this.labelField5Value.Location = new System.Drawing.Point(249, 198);
            this.labelField5Value.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelField5Value.Name = "labelField5Value";
            this.labelField5Value.Size = new System.Drawing.Size(26, 29);
            this.labelField5Value.TabIndex = 14;
            this.labelField5Value.Text = "0";
            // 
            // labelField7
            // 
            this.labelField7.AccessibleDescription = resources.GetString("labelField7.AccessibleDescription");
            this.labelField7.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelField7.ForeColor = System.Drawing.Color.White;
            this.labelField7.Location = new System.Drawing.Point(44, 282);
            this.labelField7.Name = "labelField7";
            this.labelField7.Size = new System.Drawing.Size(203, 31);
            this.labelField7.TabIndex = 18;
            this.labelField7.Text = "Field7";
            this.labelField7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelField7Value
            // 
            this.labelField7Value.AutoSize = true;
            this.labelField7Value.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelField7Value.ForeColor = System.Drawing.Color.White;
            this.labelField7Value.Location = new System.Drawing.Point(249, 282);
            this.labelField7Value.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelField7Value.Name = "labelField7Value";
            this.labelField7Value.Size = new System.Drawing.Size(26, 29);
            this.labelField7Value.TabIndex = 19;
            this.labelField7Value.Text = "0";
            // 
            // labelField6Value
            // 
            this.labelField6Value.AutoSize = true;
            this.labelField6Value.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.labelField6Value.ForeColor = System.Drawing.Color.White;
            this.labelField6Value.Location = new System.Drawing.Point(249, 240);
            this.labelField6Value.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelField6Value.Name = "labelField6Value";
            this.labelField6Value.Size = new System.Drawing.Size(26, 29);
            this.labelField6Value.TabIndex = 17;
            this.labelField6Value.Text = "0";
            // 
            // labelField6
            // 
            this.labelField6.AccessibleDescription = "The current refresh rate of the display.";
            this.labelField6.BackColor = System.Drawing.Color.Transparent;
            this.labelField6.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelField6.ForeColor = System.Drawing.Color.White;
            this.labelField6.Location = new System.Drawing.Point(9, 240);
            this.labelField6.Name = "labelField6";
            this.labelField6.Size = new System.Drawing.Size(238, 31);
            this.labelField6.TabIndex = 16;
            this.labelField6.Text = "Field6";
            this.labelField6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxFIeld2
            // 
            this.comboBoxFIeld2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFIeld2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxFIeld2.FormattingEnabled = true;
            this.comboBoxFIeld2.Location = new System.Drawing.Point(255, 65);
            this.comboBoxFIeld2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxFIeld2.Name = "comboBoxFIeld2";
            this.comboBoxFIeld2.Size = new System.Drawing.Size(145, 26);
            this.comboBoxFIeld2.TabIndex = 6;
            this.comboBoxFIeld2.SelectedIndexChanged += new System.EventHandler(this.comboBoxStrobe_SelectedIndexChanged);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.contextMenuTrayFull;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Display Utility";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_Click);
            // 
            // contextMenuTrayFull
            // 
            this.contextMenuTrayFull.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuTrayFull.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolStripMenuSection,
            this.offToolStripMenuItem,
            this.lightToolStripMenuItem,
            this.normalToolStripMenuItem,
            this.extremeToolStripMenuItem,
            this.ultraToolStripMenuItem,
            this.customToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitStrobeUtilityToolStripMenuItem});
            this.contextMenuTrayFull.Name = "contextMenuTray";
            this.contextMenuTrayFull.Size = new System.Drawing.Size(202, 192);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // toolStripMenuSection
            // 
            this.toolStripMenuSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.toolStripMenuSection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuSection.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuSection.Name = "toolStripMenuSection";
            this.toolStripMenuSection.Size = new System.Drawing.Size(201, 22);
            this.toolStripMenuSection.Text = "Blur Reduction Setting";
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.offToolStripMenuItem.Text = "    Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // lightToolStripMenuItem
            // 
            this.lightToolStripMenuItem.Name = "lightToolStripMenuItem";
            this.lightToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.lightToolStripMenuItem.Text = "    Light";
            this.lightToolStripMenuItem.Click += new System.EventHandler(this.lightToolStripMenuItem_Click);
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.normalToolStripMenuItem.Text = "    Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // extremeToolStripMenuItem
            // 
            this.extremeToolStripMenuItem.Name = "extremeToolStripMenuItem";
            this.extremeToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.extremeToolStripMenuItem.Text = "    Extreme";
            this.extremeToolStripMenuItem.Click += new System.EventHandler(this.extremeToolStripMenuItem_Click);
            // 
            // ultraToolStripMenuItem
            // 
            this.ultraToolStripMenuItem.Name = "ultraToolStripMenuItem";
            this.ultraToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ultraToolStripMenuItem.Text = "    Ultra";
            this.ultraToolStripMenuItem.Click += new System.EventHandler(this.ultraToolStripMenuItem_Click);
            // 
            // customToolStripMenuItem
            // 
            this.customToolStripMenuItem.Name = "customToolStripMenuItem";
            this.customToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.customToolStripMenuItem.Text = "    Custom";
            this.customToolStripMenuItem.Click += new System.EventHandler(this.customToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // quitStrobeUtilityToolStripMenuItem
            // 
            this.quitStrobeUtilityToolStripMenuItem.Name = "quitStrobeUtilityToolStripMenuItem";
            this.quitStrobeUtilityToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.quitStrobeUtilityToolStripMenuItem.Text = "Exit";
            this.quitStrobeUtilityToolStripMenuItem.Click += new System.EventHandler(this.quitStrobeUtilityToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem7.Text = "Exit";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.quitStrobeUtilityToolStripMenuItem_Click);
            // 
            // contextMenuTray
            // 
            this.contextMenuTray.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem7});
            this.contextMenuTray.Name = "contextMenuTray";
            this.contextMenuTray.Size = new System.Drawing.Size(94, 48);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(93, 22);
            // 
            // buttonTest
            // 
            this.buttonTest.AccessibleDescription = "Toggle the motion test";
            this.buttonTest.BackColor = System.Drawing.Color.White;
            this.buttonTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTest.ForeColor = System.Drawing.Color.Transparent;
            this.buttonTest.Location = new System.Drawing.Point(534, 21);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(83, 71);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = false;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // MainUtilityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1105, 331);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.comboBoxFIeld2);
            this.Controls.Add(this.labelMessageText);
            this.Controls.Add(this.labelMessageLabel);
            this.Controls.Add(this.labelField6Value);
            this.Controls.Add(this.labelField6);
            this.Controls.Add(this.labelField5Value);
            this.Controls.Add(this.sliderField5);
            this.Controls.Add(this.labelField5);
            this.Controls.Add(this.sliderField4);
            this.Controls.Add(this.sliderField3);
            this.Controls.Add(this.labelField4Value);
            this.Controls.Add(this.labelField3Value);
            this.Controls.Add(this.labelField4);
            this.Controls.Add(this.labelField3);
            this.Controls.Add(this.labelField2);
            this.Controls.Add(this.labelDisplay);
            this.Controls.Add(this.comboBoxDisplaysList);
            this.Controls.Add(this.labelField7Value);
            this.Controls.Add(this.labelField7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(980, 40);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainUtilityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Utility";
            this.Activated += new System.EventHandler(this.StrobeUtilityForm_Activated);
            this.Deactivate += new System.EventHandler(this.StrobeUtilityForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StrobeUtilityForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StrobeUtilityForm_FormClosed);
            this.Load += new System.EventHandler(this.StilityForm_Load);
            this.VisibleChanged += new System.EventHandler(this.StrobeUtilityForm_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StrobeUtilityForm_KeyDown);
            this.Resize += new System.EventHandler(this.StrobeUtilityForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.sliderField3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderField4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderField5)).EndInit();
            this.contextMenuTrayFull.ResumeLayout(false);
            this.contextMenuTray.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxDisplaysList;
        private System.Windows.Forms.Label labelDisplay;
        private System.Windows.Forms.Label labelField2;
        private System.Windows.Forms.Label labelField3;
        private System.Windows.Forms.TrackBar sliderField3;
        private System.Windows.Forms.Label labelField4;
        private System.Windows.Forms.TrackBar sliderField4;
        private System.Windows.Forms.Label labelField3Value;
        private System.Windows.Forms.Label labelField4Value;
        private System.Windows.Forms.Label labelMessageLabel;
        private System.Windows.Forms.Label labelMessageText;
        private System.Windows.Forms.Label labelField5;
        private System.Windows.Forms.TrackBar sliderField5;
        private System.Windows.Forms.Label labelField5Value;
        private System.Windows.Forms.Label labelField7;
        private System.Windows.Forms.Label labelField7Value;
        private System.Windows.Forms.Label labelField6Value;
        private System.Windows.Forms.Label labelField6;
        private System.Windows.Forms.ComboBox comboBoxFIeld2;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuTrayFull;
        private System.Windows.Forms.ToolStripMenuItem offToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extremeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ultraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitStrobeUtilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ContextMenuStrip contextMenuTray;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuSection;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button buttonTest;
    }
}
