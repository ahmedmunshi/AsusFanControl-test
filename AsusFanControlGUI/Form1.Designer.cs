namespace AsusFanControlGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            // Menu
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTurnOffControlOnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemForbidUnsafeSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMinimizeToTrayOnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRunDiagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpenLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();

            // Tab control
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFanControl = new System.Windows.Forms.TabPage();
            this.tabProfiles = new System.Windows.Forms.TabPage();

            // Fan Control tab - Mode bar
            this.panelModeBar = new System.Windows.Forms.Panel();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            this.radioButtonCurve = new System.Windows.Forms.RadioButton();
            this.radioButtonFixed = new System.Windows.Forms.RadioButton();

            // Fan Control tab - Curve editor
            this.fanCurveEditor = new AsusFanControlGUI.Controls.FanCurveEditor();

            // Fan Control tab - Fixed speed
            this.panelFixedSpeed = new System.Windows.Forms.Panel();
            this.trackBarFanSpeed = new System.Windows.Forms.TrackBar();
            this.labelFixedSpeedValue = new System.Windows.Forms.Label();

            // Fan Control tab - Per-fan
            this.panelPerFan = new System.Windows.Forms.Panel();
            this.checkBoxPerFan = new System.Windows.Forms.CheckBox();
            this.labelFanSelect = new System.Windows.Forms.Label();
            this.comboBoxFanSelect = new System.Windows.Forms.ComboBox();

            // Fan Control tab - Stats
            this.panelStats = new System.Windows.Forms.Panel();
            this.labelCpuTempTitle = new System.Windows.Forms.Label();
            this.labelCpuTemp = new System.Windows.Forms.Label();
            this.labelRpmTitle = new System.Windows.Forms.Label();
            this.labelRPM = new System.Windows.Forms.Label();
            this.labelAppliedSpeedTitle = new System.Windows.Forms.Label();
            this.labelAppliedSpeed = new System.Windows.Forms.Label();

            // Profiles tab
            this.panelProfileBar = new System.Windows.Forms.Panel();
            this.labelProfile = new System.Windows.Forms.Label();
            this.comboBoxProfile = new System.Windows.Forms.ComboBox();
            this.buttonAddProfile = new System.Windows.Forms.Button();
            this.buttonRenameProfile = new System.Windows.Forms.Button();
            this.buttonDeleteProfile = new System.Windows.Forms.Button();
            this.panelProfileDetails = new System.Windows.Forms.Panel();
            this.labelProfileMode = new System.Windows.Forms.Label();
            this.labelProfileModeValue = new System.Windows.Forms.Label();
            this.labelProfileCurvePoints = new System.Windows.Forms.Label();
            this.labelProfileCurvePointsValue = new System.Windows.Forms.Label();
            this.labelProfileStorage = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();

            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.toolStripMenuItem1,
                this.toolStripMenuItemCheckForUpdates
            });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(600, 24);
            this.menuStrip1.TabIndex = 0;

            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.toolStripMenuItemTurnOffControlOnExit,
                this.toolStripMenuItemForbidUnsafeSettings,
                this.toolStripMenuItemMinimizeToTrayOnClose,
                this.toolStripSeparator1,
                this.toolStripMenuItemRunDiagnostics,
                this.toolStripMenuItemOpenLog
            });
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(72, 20);
            this.toolStripMenuItem1.Text = "Advanced";

            this.toolStripMenuItemTurnOffControlOnExit.CheckOnClick = true;
            this.toolStripMenuItemTurnOffControlOnExit.Name = "toolStripMenuItemTurnOffControlOnExit";
            this.toolStripMenuItemTurnOffControlOnExit.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemTurnOffControlOnExit.Text = "Turn off control on exit";
            this.toolStripMenuItemTurnOffControlOnExit.CheckedChanged += new System.EventHandler(this.toolStripMenuItemTurnOffControlOnExit_CheckedChanged);

            this.toolStripMenuItemForbidUnsafeSettings.CheckOnClick = true;
            this.toolStripMenuItemForbidUnsafeSettings.Name = "toolStripMenuItemForbidUnsafeSettings";
            this.toolStripMenuItemForbidUnsafeSettings.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemForbidUnsafeSettings.Text = "Forbid unsafe settings";
            this.toolStripMenuItemForbidUnsafeSettings.CheckedChanged += new System.EventHandler(this.toolStripMenuItemForbidUnsafeSettings_CheckedChanged);

            this.toolStripMenuItemMinimizeToTrayOnClose.CheckOnClick = true;
            this.toolStripMenuItemMinimizeToTrayOnClose.Name = "toolStripMenuItemMinimizeToTrayOnClose";
            this.toolStripMenuItemMinimizeToTrayOnClose.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemMinimizeToTrayOnClose.Text = "Minimize to tray on close";
            this.toolStripMenuItemMinimizeToTrayOnClose.Click += new System.EventHandler(this.toolStripMenuItemMinimizeToTrayOnClose_Click);

            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);

            this.toolStripMenuItemRunDiagnostics.Name = "toolStripMenuItemRunDiagnostics";
            this.toolStripMenuItemRunDiagnostics.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemRunDiagnostics.Text = "Run Diagnostics";
            this.toolStripMenuItemRunDiagnostics.Click += new System.EventHandler(this.toolStripMenuItemRunDiagnostics_Click);

            this.toolStripMenuItemOpenLog.Name = "toolStripMenuItemOpenLog";
            this.toolStripMenuItemOpenLog.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemOpenLog.Text = "Open Error Log";
            this.toolStripMenuItemOpenLog.Click += new System.EventHandler(this.toolStripMenuItemOpenLog_Click);

            this.toolStripMenuItemCheckForUpdates.Name = "toolStripMenuItemCheckForUpdates";
            this.toolStripMenuItemCheckForUpdates.Size = new System.Drawing.Size(115, 20);
            this.toolStripMenuItemCheckForUpdates.Text = "Check for updates";
            this.toolStripMenuItemCheckForUpdates.Click += new System.EventHandler(this.toolStripMenuItemCheckForUpdates_Click);

            // 
            // tabControl
            // 
            this.tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.tabControl.Controls.Add(this.tabFanControl);
            this.tabControl.Controls.Add(this.tabProfiles);
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(600, 466);
            this.tabControl.TabIndex = 1;

            // 
            // tabFanControl
            // 
            this.tabFanControl.Text = "Fan Control";
            this.tabFanControl.Padding = new System.Windows.Forms.Padding(0);

            // 
            // tabProfiles
            // 
            this.tabProfiles.Text = "Profiles";
            this.tabProfiles.Padding = new System.Windows.Forms.Padding(0);

            // 
            // panelModeBar
            // 
            this.panelModeBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModeBar.Height = 32;
            this.panelModeBar.Padding = new System.Windows.Forms.Padding(8, 4, 8, 0);

            this.checkBoxEnable.Text = "Enable";
            this.checkBoxEnable.Location = new System.Drawing.Point(8, 6);
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.CheckedChanged += new System.EventHandler(this.checkBoxEnable_CheckedChanged);

            this.radioButtonCurve.Text = "Fan Curve";
            this.radioButtonCurve.Location = new System.Drawing.Point(90, 6);
            this.radioButtonCurve.AutoSize = true;
            this.radioButtonCurve.Checked = true;
            this.radioButtonCurve.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);

            this.radioButtonFixed.Text = "Fixed Speed";
            this.radioButtonFixed.Location = new System.Drawing.Point(190, 6);
            this.radioButtonFixed.AutoSize = true;
            this.radioButtonFixed.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);

            this.panelModeBar.Controls.Add(this.checkBoxEnable);
            this.panelModeBar.Controls.Add(this.radioButtonCurve);
            this.panelModeBar.Controls.Add(this.radioButtonFixed);

            // 
            // fanCurveEditor
            // 
            this.fanCurveEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.fanCurveEditor.Location = new System.Drawing.Point(8, 36);
            this.fanCurveEditor.Name = "fanCurveEditor";
            this.fanCurveEditor.Size = new System.Drawing.Size(578, 320);
            this.fanCurveEditor.CurveChanged += new System.EventHandler(this.fanCurveEditor_CurveChanged);

            // 
            // panelFixedSpeed
            // 
            this.panelFixedSpeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.panelFixedSpeed.Location = new System.Drawing.Point(8, 36);
            this.panelFixedSpeed.Size = new System.Drawing.Size(578, 320);
            this.panelFixedSpeed.Visible = false;

            this.trackBarFanSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.trackBarFanSpeed.Location = new System.Drawing.Point(10, 50);
            this.trackBarFanSpeed.Size = new System.Drawing.Size(558, 45);
            this.trackBarFanSpeed.Maximum = 100;
            this.trackBarFanSpeed.Value = 80;
            this.trackBarFanSpeed.TickFrequency = 5;
            this.trackBarFanSpeed.MouseCaptureChanged += new System.EventHandler(this.trackBarFanSpeed_MouseCaptureChanged);
            this.trackBarFanSpeed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarFanSpeed_KeyUp);

            this.labelFixedSpeedValue.Text = "80%";
            this.labelFixedSpeedValue.Location = new System.Drawing.Point(10, 15);
            this.labelFixedSpeedValue.AutoSize = true;
            this.labelFixedSpeedValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);

            this.panelFixedSpeed.Controls.Add(this.trackBarFanSpeed);
            this.panelFixedSpeed.Controls.Add(this.labelFixedSpeedValue);

            // 
            // panelPerFan
            // 
            this.panelPerFan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPerFan.Height = 30;
            this.panelPerFan.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);

            this.checkBoxPerFan.Text = "Per-fan curves";
            this.checkBoxPerFan.Location = new System.Drawing.Point(8, 4);
            this.checkBoxPerFan.AutoSize = true;
            this.checkBoxPerFan.CheckedChanged += new System.EventHandler(this.checkBoxPerFan_CheckedChanged);

            this.labelFanSelect.Text = "Fan:";
            this.labelFanSelect.Location = new System.Drawing.Point(140, 6);
            this.labelFanSelect.AutoSize = true;
            this.labelFanSelect.Visible = false;

            this.comboBoxFanSelect.Location = new System.Drawing.Point(170, 2);
            this.comboBoxFanSelect.Size = new System.Drawing.Size(120, 24);
            this.comboBoxFanSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFanSelect.Visible = false;
            this.comboBoxFanSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxFanSelect_SelectedIndexChanged);

            this.panelPerFan.Controls.Add(this.checkBoxPerFan);
            this.panelPerFan.Controls.Add(this.labelFanSelect);
            this.panelPerFan.Controls.Add(this.comboBoxFanSelect);

            // 
            // panelStats
            // 
            this.panelStats.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStats.Height = 40;
            this.panelStats.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);

            this.labelCpuTempTitle.Text = "CPU:";
            this.labelCpuTempTitle.Location = new System.Drawing.Point(8, 10);
            this.labelCpuTempTitle.AutoSize = true;
            this.labelCpuTempTitle.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.labelCpuTemp.Text = "--";
            this.labelCpuTemp.Location = new System.Drawing.Point(44, 6);
            this.labelCpuTemp.AutoSize = true;
            this.labelCpuTemp.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);

            this.labelRpmTitle.Text = "Fans:";
            this.labelRpmTitle.Location = new System.Drawing.Point(170, 10);
            this.labelRpmTitle.AutoSize = true;
            this.labelRpmTitle.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.labelRPM.Text = "--";
            this.labelRPM.Location = new System.Drawing.Point(210, 8);
            this.labelRPM.AutoSize = true;
            this.labelRPM.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelRPM.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this.labelAppliedSpeedTitle.Text = "Applied:";
            this.labelAppliedSpeedTitle.Location = new System.Drawing.Point(8, 27);
            this.labelAppliedSpeedTitle.AutoSize = true;
            this.labelAppliedSpeedTitle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelAppliedSpeedTitle.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);

            this.labelAppliedSpeed.Text = "--";
            this.labelAppliedSpeed.Location = new System.Drawing.Point(60, 25);
            this.labelAppliedSpeed.AutoSize = true;
            this.labelAppliedSpeed.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.panelStats.Controls.Add(this.labelCpuTempTitle);
            this.panelStats.Controls.Add(this.labelCpuTemp);
            this.panelStats.Controls.Add(this.labelRpmTitle);
            this.panelStats.Controls.Add(this.labelRPM);
            this.panelStats.Controls.Add(this.labelAppliedSpeedTitle);
            this.panelStats.Controls.Add(this.labelAppliedSpeed);

            // Assemble Fan Control tab
            this.tabFanControl.Controls.Add(this.fanCurveEditor);
            this.tabFanControl.Controls.Add(this.panelFixedSpeed);
            this.tabFanControl.Controls.Add(this.panelPerFan);
            this.tabFanControl.Controls.Add(this.panelStats);
            this.tabFanControl.Controls.Add(this.panelModeBar);

            // 
            // panelProfileBar
            // 
            this.panelProfileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProfileBar.Height = 40;
            this.panelProfileBar.Padding = new System.Windows.Forms.Padding(8, 6, 8, 4);

            this.labelProfile.Text = "Profile:";
            this.labelProfile.Location = new System.Drawing.Point(8, 10);
            this.labelProfile.AutoSize = true;

            this.comboBoxProfile.Location = new System.Drawing.Point(56, 6);
            this.comboBoxProfile.Size = new System.Drawing.Size(200, 24);
            this.comboBoxProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProfile.SelectedIndexChanged += new System.EventHandler(this.comboBoxProfile_SelectedIndexChanged);

            this.buttonAddProfile.Text = "+";
            this.buttonAddProfile.Location = new System.Drawing.Point(264, 5);
            this.buttonAddProfile.Size = new System.Drawing.Size(30, 25);
            this.buttonAddProfile.Click += new System.EventHandler(this.buttonAddProfile_Click);

            this.buttonRenameProfile.Text = "Rename";
            this.buttonRenameProfile.Location = new System.Drawing.Point(299, 5);
            this.buttonRenameProfile.Size = new System.Drawing.Size(60, 25);
            this.buttonRenameProfile.Click += new System.EventHandler(this.buttonRenameProfile_Click);

            this.buttonDeleteProfile.Text = "Delete";
            this.buttonDeleteProfile.Location = new System.Drawing.Point(364, 5);
            this.buttonDeleteProfile.Size = new System.Drawing.Size(55, 25);
            this.buttonDeleteProfile.Click += new System.EventHandler(this.buttonDeleteProfile_Click);

            this.panelProfileBar.Controls.Add(this.labelProfile);
            this.panelProfileBar.Controls.Add(this.comboBoxProfile);
            this.panelProfileBar.Controls.Add(this.buttonAddProfile);
            this.panelProfileBar.Controls.Add(this.buttonRenameProfile);
            this.panelProfileBar.Controls.Add(this.buttonDeleteProfile);

            // 
            // panelProfileDetails
            // 
            this.panelProfileDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProfileDetails.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);

            this.labelProfileMode.Text = "Mode:";
            this.labelProfileMode.Location = new System.Drawing.Point(12, 12);
            this.labelProfileMode.AutoSize = true;
            this.labelProfileMode.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.labelProfileModeValue.Text = "Fan Curve";
            this.labelProfileModeValue.Location = new System.Drawing.Point(60, 12);
            this.labelProfileModeValue.AutoSize = true;
            this.labelProfileModeValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.labelProfileCurvePoints.Text = "Curve points:";
            this.labelProfileCurvePoints.Location = new System.Drawing.Point(12, 34);
            this.labelProfileCurvePoints.AutoSize = true;
            this.labelProfileCurvePoints.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.labelProfileCurvePointsValue.Text = "4";
            this.labelProfileCurvePointsValue.Location = new System.Drawing.Point(100, 34);
            this.labelProfileCurvePointsValue.AutoSize = true;
            this.labelProfileCurvePointsValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.labelProfileStorage.Text = "Profiles stored in %AppData%/AsusFanControl/profiles/";
            this.labelProfileStorage.Location = new System.Drawing.Point(12, 60);
            this.labelProfileStorage.AutoSize = true;
            this.labelProfileStorage.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelProfileStorage.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);

            this.panelProfileDetails.Controls.Add(this.labelProfileMode);
            this.panelProfileDetails.Controls.Add(this.labelProfileModeValue);
            this.panelProfileDetails.Controls.Add(this.labelProfileCurvePoints);
            this.panelProfileDetails.Controls.Add(this.labelProfileCurvePointsValue);
            this.panelProfileDetails.Controls.Add(this.labelProfileStorage);

            // Assemble Profiles tab
            this.tabProfiles.Controls.Add(this.panelProfileDetails);
            this.tabProfiles.Controls.Add(this.panelProfileBar);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 490);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(500, 420);
            this.Name = "Form1";
            this.Text = "Asus Fan Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // Menu
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTurnOffControlOnExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemForbidUnsafeSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCheckForUpdates;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMinimizeToTrayOnClose;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRunDiagnostics;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

        // Tabs
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabFanControl;
        private System.Windows.Forms.TabPage tabProfiles;

        // Fan Control tab - Mode bar
        private System.Windows.Forms.Panel panelModeBar;
        private System.Windows.Forms.RadioButton radioButtonCurve;
        private System.Windows.Forms.RadioButton radioButtonFixed;
        private System.Windows.Forms.CheckBox checkBoxEnable;

        // Fan Control tab - Curve editor
        private AsusFanControlGUI.Controls.FanCurveEditor fanCurveEditor;

        // Fan Control tab - Fixed speed
        private System.Windows.Forms.Panel panelFixedSpeed;
        private System.Windows.Forms.TrackBar trackBarFanSpeed;
        private System.Windows.Forms.Label labelFixedSpeedValue;

        // Fan Control tab - Per-fan
        private System.Windows.Forms.Panel panelPerFan;
        private System.Windows.Forms.CheckBox checkBoxPerFan;
        private System.Windows.Forms.Label labelFanSelect;
        private System.Windows.Forms.ComboBox comboBoxFanSelect;

        // Fan Control tab - Stats
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Label labelCpuTempTitle;
        private System.Windows.Forms.Label labelCpuTemp;
        private System.Windows.Forms.Label labelRpmTitle;
        private System.Windows.Forms.Label labelRPM;
        private System.Windows.Forms.Label labelAppliedSpeedTitle;
        private System.Windows.Forms.Label labelAppliedSpeed;

        // Profiles tab
        private System.Windows.Forms.Panel panelProfileBar;
        private System.Windows.Forms.Label labelProfile;
        private System.Windows.Forms.ComboBox comboBoxProfile;
        private System.Windows.Forms.Button buttonAddProfile;
        private System.Windows.Forms.Button buttonRenameProfile;
        private System.Windows.Forms.Button buttonDeleteProfile;
        private System.Windows.Forms.Panel panelProfileDetails;
        private System.Windows.Forms.Label labelProfileMode;
        private System.Windows.Forms.Label labelProfileModeValue;
        private System.Windows.Forms.Label labelProfileCurvePoints;
        private System.Windows.Forms.Label labelProfileCurvePointsValue;
        private System.Windows.Forms.Label labelProfileStorage;
    }
}