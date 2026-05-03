namespace AsusFanControlGUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTurnOffControlOnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemForbidUnsafeSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMinimizeToTrayOnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();

            // Profile bar
            this.panelProfileBar = new System.Windows.Forms.Panel();
            this.labelProfile = new System.Windows.Forms.Label();
            this.comboBoxProfile = new System.Windows.Forms.ComboBox();
            this.buttonAddProfile = new System.Windows.Forms.Button();
            this.buttonRenameProfile = new System.Windows.Forms.Button();
            this.buttonDeleteProfile = new System.Windows.Forms.Button();

            // Mode selection
            this.panelModeBar = new System.Windows.Forms.Panel();
            this.radioButtonCurve = new System.Windows.Forms.RadioButton();
            this.radioButtonFixed = new System.Windows.Forms.RadioButton();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();

            // Curve editor
            this.fanCurveEditor = new AsusFanControlGUI.Controls.FanCurveEditor();

            // Fixed speed panel
            this.panelFixedSpeed = new System.Windows.Forms.Panel();
            this.trackBarFanSpeed = new System.Windows.Forms.TrackBar();
            this.labelFixedSpeedValue = new System.Windows.Forms.Label();

            // Per-fan panel
            this.panelPerFan = new System.Windows.Forms.Panel();
            this.checkBoxPerFan = new System.Windows.Forms.CheckBox();
            this.labelFanSelect = new System.Windows.Forms.Label();
            this.comboBoxFanSelect = new System.Windows.Forms.ComboBox();

            // Stats bar
            this.panelStats = new System.Windows.Forms.Panel();
            this.labelCpuTempTitle = new System.Windows.Forms.Label();
            this.labelCpuTemp = new System.Windows.Forms.Label();
            this.labelRpmTitle = new System.Windows.Forms.Label();
            this.labelRPM = new System.Windows.Forms.Label();
            this.labelAppliedSpeedTitle = new System.Windows.Forms.Label();
            this.labelAppliedSpeed = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).BeginInit();
            this.menuStrip1.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(500, 24);
            this.menuStrip1.TabIndex = 0;

            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItemRunDiagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpenLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();

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

            // 
            // toolStripMenuItemTurnOffControlOnExit
            // 
            this.toolStripMenuItemTurnOffControlOnExit.CheckOnClick = true;
            this.toolStripMenuItemTurnOffControlOnExit.Name = "toolStripMenuItemTurnOffControlOnExit";
            this.toolStripMenuItemTurnOffControlOnExit.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemTurnOffControlOnExit.Text = "Turn off control on exit";
            this.toolStripMenuItemTurnOffControlOnExit.CheckedChanged += new System.EventHandler(this.toolStripMenuItemTurnOffControlOnExit_CheckedChanged);

            // 
            // toolStripMenuItemForbidUnsafeSettings
            // 
            this.toolStripMenuItemForbidUnsafeSettings.CheckOnClick = true;
            this.toolStripMenuItemForbidUnsafeSettings.Name = "toolStripMenuItemForbidUnsafeSettings";
            this.toolStripMenuItemForbidUnsafeSettings.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemForbidUnsafeSettings.Text = "Forbid unsafe settings";
            this.toolStripMenuItemForbidUnsafeSettings.CheckedChanged += new System.EventHandler(this.toolStripMenuItemForbidUnsafeSettings_CheckedChanged);

            // 
            // toolStripMenuItemMinimizeToTrayOnClose
            // 
            this.toolStripMenuItemMinimizeToTrayOnClose.CheckOnClick = true;
            this.toolStripMenuItemMinimizeToTrayOnClose.Name = "toolStripMenuItemMinimizeToTrayOnClose";
            this.toolStripMenuItemMinimizeToTrayOnClose.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemMinimizeToTrayOnClose.Text = "Minimize to tray on close";
            this.toolStripMenuItemMinimizeToTrayOnClose.Click += new System.EventHandler(this.toolStripMenuItemMinimizeToTrayOnClose_Click);

            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);

            // 
            // toolStripMenuItemRunDiagnostics
            // 
            this.toolStripMenuItemRunDiagnostics.Name = "toolStripMenuItemRunDiagnostics";
            this.toolStripMenuItemRunDiagnostics.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemRunDiagnostics.Text = "Run Diagnostics";
            this.toolStripMenuItemRunDiagnostics.Click += new System.EventHandler(this.toolStripMenuItemRunDiagnostics_Click);

            // 
            // toolStripMenuItemOpenLog
            // 
            this.toolStripMenuItemOpenLog.Name = "toolStripMenuItemOpenLog";
            this.toolStripMenuItemOpenLog.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemOpenLog.Text = "Open Error Log";
            this.toolStripMenuItemOpenLog.Click += new System.EventHandler(this.toolStripMenuItemOpenLog_Click);

            // 
            // toolStripMenuItemCheckForUpdates
            // 
            this.toolStripMenuItemCheckForUpdates.Name = "toolStripMenuItemCheckForUpdates";
            this.toolStripMenuItemCheckForUpdates.Size = new System.Drawing.Size(115, 20);
            this.toolStripMenuItemCheckForUpdates.Text = "Check for updates";
            this.toolStripMenuItemCheckForUpdates.Click += new System.EventHandler(this.toolStripMenuItemCheckForUpdates_Click);

            // 
            // panelProfileBar
            // 
            this.panelProfileBar.Location = new System.Drawing.Point(0, 24);
            this.panelProfileBar.Size = new System.Drawing.Size(500, 36);
            this.panelProfileBar.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);

            this.labelProfile.Text = "Profile:";
            this.labelProfile.Location = new System.Drawing.Point(10, 10);
            this.labelProfile.AutoSize = true;

            this.comboBoxProfile.Location = new System.Drawing.Point(62, 6);
            this.comboBoxProfile.Size = new System.Drawing.Size(200, 24);
            this.comboBoxProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProfile.SelectedIndexChanged += new System.EventHandler(this.comboBoxProfile_SelectedIndexChanged);

            this.buttonAddProfile.Text = "+";
            this.buttonAddProfile.Location = new System.Drawing.Point(270, 5);
            this.buttonAddProfile.Size = new System.Drawing.Size(30, 25);
            this.buttonAddProfile.Click += new System.EventHandler(this.buttonAddProfile_Click);

            this.buttonRenameProfile.Text = "Rename";
            this.buttonRenameProfile.Location = new System.Drawing.Point(305, 5);
            this.buttonRenameProfile.Size = new System.Drawing.Size(65, 25);
            this.buttonRenameProfile.Click += new System.EventHandler(this.buttonRenameProfile_Click);

            this.buttonDeleteProfile.Text = "Delete";
            this.buttonDeleteProfile.Location = new System.Drawing.Point(375, 5);
            this.buttonDeleteProfile.Size = new System.Drawing.Size(55, 25);
            this.buttonDeleteProfile.Click += new System.EventHandler(this.buttonDeleteProfile_Click);

            this.panelProfileBar.Controls.Add(this.labelProfile);
            this.panelProfileBar.Controls.Add(this.comboBoxProfile);
            this.panelProfileBar.Controls.Add(this.buttonAddProfile);
            this.panelProfileBar.Controls.Add(this.buttonRenameProfile);
            this.panelProfileBar.Controls.Add(this.buttonDeleteProfile);

            // 
            // panelModeBar
            // 
            this.panelModeBar.Location = new System.Drawing.Point(0, 60);
            this.panelModeBar.Size = new System.Drawing.Size(500, 32);

            this.checkBoxEnable.Text = "Enable";
            this.checkBoxEnable.Location = new System.Drawing.Point(10, 6);
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.CheckedChanged += new System.EventHandler(this.checkBoxEnable_CheckedChanged);

            this.radioButtonCurve.Text = "Fan Curve";
            this.radioButtonCurve.Location = new System.Drawing.Point(100, 6);
            this.radioButtonCurve.AutoSize = true;
            this.radioButtonCurve.Checked = true;
            this.radioButtonCurve.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);

            this.radioButtonFixed.Text = "Fixed Speed";
            this.radioButtonFixed.Location = new System.Drawing.Point(200, 6);
            this.radioButtonFixed.AutoSize = true;
            this.radioButtonFixed.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);

            this.panelModeBar.Controls.Add(this.checkBoxEnable);
            this.panelModeBar.Controls.Add(this.radioButtonCurve);
            this.panelModeBar.Controls.Add(this.radioButtonFixed);

            // 
            // fanCurveEditor
            // 
            this.fanCurveEditor.Location = new System.Drawing.Point(8, 96);
            this.fanCurveEditor.Size = new System.Drawing.Size(484, 280);
            this.fanCurveEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.fanCurveEditor.CurveChanged += new System.EventHandler(this.fanCurveEditor_CurveChanged);

            // 
            // panelFixedSpeed
            // 
            this.panelFixedSpeed.Location = new System.Drawing.Point(8, 96);
            this.panelFixedSpeed.Size = new System.Drawing.Size(484, 280);
            this.panelFixedSpeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.panelFixedSpeed.Visible = false;

            this.trackBarFanSpeed.Location = new System.Drawing.Point(10, 40);
            this.trackBarFanSpeed.Size = new System.Drawing.Size(460, 45);
            this.trackBarFanSpeed.Maximum = 100;
            this.trackBarFanSpeed.Value = 80;
            this.trackBarFanSpeed.TickFrequency = 5;
            this.trackBarFanSpeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.trackBarFanSpeed.MouseCaptureChanged += new System.EventHandler(this.trackBarFanSpeed_MouseCaptureChanged);
            this.trackBarFanSpeed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarFanSpeed_KeyUp);

            this.labelFixedSpeedValue.Text = "80%";
            this.labelFixedSpeedValue.Location = new System.Drawing.Point(10, 15);
            this.labelFixedSpeedValue.AutoSize = true;
            this.labelFixedSpeedValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);

            this.panelFixedSpeed.Controls.Add(this.trackBarFanSpeed);
            this.panelFixedSpeed.Controls.Add(this.labelFixedSpeedValue);

            // 
            // panelPerFan
            // 
            this.panelPerFan.Location = new System.Drawing.Point(8, 380);
            this.panelPerFan.Size = new System.Drawing.Size(484, 30);
            this.panelPerFan.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this.checkBoxPerFan.Text = "Per-fan curves";
            this.checkBoxPerFan.Location = new System.Drawing.Point(10, 5);
            this.checkBoxPerFan.AutoSize = true;
            this.checkBoxPerFan.CheckedChanged += new System.EventHandler(this.checkBoxPerFan_CheckedChanged);

            this.labelFanSelect.Text = "Fan:";
            this.labelFanSelect.Location = new System.Drawing.Point(140, 7);
            this.labelFanSelect.AutoSize = true;
            this.labelFanSelect.Visible = false;

            this.comboBoxFanSelect.Location = new System.Drawing.Point(170, 3);
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
            this.panelStats.Location = new System.Drawing.Point(8, 414);
            this.panelStats.Size = new System.Drawing.Size(484, 46);
            this.panelStats.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.panelStats.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);

            this.labelCpuTempTitle.Text = "CPU Temp:";
            this.labelCpuTempTitle.Location = new System.Drawing.Point(10, 6);
            this.labelCpuTempTitle.AutoSize = true;

            this.labelCpuTemp.Text = "--";
            this.labelCpuTemp.Location = new System.Drawing.Point(80, 6);
            this.labelCpuTemp.AutoSize = true;
            this.labelCpuTemp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.labelRpmTitle.Text = "Fan RPM:";
            this.labelRpmTitle.Location = new System.Drawing.Point(150, 6);
            this.labelRpmTitle.AutoSize = true;

            this.labelRPM.Text = "--";
            this.labelRPM.Location = new System.Drawing.Point(215, 6);
            this.labelRPM.AutoSize = true;
            this.labelRPM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.labelAppliedSpeedTitle.Text = "Applied:";
            this.labelAppliedSpeedTitle.Location = new System.Drawing.Point(10, 26);
            this.labelAppliedSpeedTitle.AutoSize = true;

            this.labelAppliedSpeed.Text = "--";
            this.labelAppliedSpeed.Location = new System.Drawing.Point(80, 26);
            this.labelAppliedSpeed.AutoSize = true;
            this.labelAppliedSpeed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            this.panelStats.Controls.Add(this.labelCpuTempTitle);
            this.panelStats.Controls.Add(this.labelCpuTemp);
            this.panelStats.Controls.Add(this.labelRpmTitle);
            this.panelStats.Controls.Add(this.labelRPM);
            this.panelStats.Controls.Add(this.labelAppliedSpeedTitle);
            this.panelStats.Controls.Add(this.labelAppliedSpeed);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 470);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelPerFan);
            this.Controls.Add(this.panelFixedSpeed);
            this.Controls.Add(this.fanCurveEditor);
            this.Controls.Add(this.panelModeBar);
            this.Controls.Add(this.panelProfileBar);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(450, 400);
            this.Name = "Form1";
            this.Text = "Asus Fan Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTurnOffControlOnExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemForbidUnsafeSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCheckForUpdates;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMinimizeToTrayOnClose;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRunDiagnostics;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

        // Profile bar
        private System.Windows.Forms.Panel panelProfileBar;
        private System.Windows.Forms.Label labelProfile;
        private System.Windows.Forms.ComboBox comboBoxProfile;
        private System.Windows.Forms.Button buttonAddProfile;
        private System.Windows.Forms.Button buttonRenameProfile;
        private System.Windows.Forms.Button buttonDeleteProfile;

        // Mode bar
        private System.Windows.Forms.Panel panelModeBar;
        private System.Windows.Forms.RadioButton radioButtonCurve;
        private System.Windows.Forms.RadioButton radioButtonFixed;
        private System.Windows.Forms.CheckBox checkBoxEnable;

        // Curve editor
        private AsusFanControlGUI.Controls.FanCurveEditor fanCurveEditor;

        // Fixed speed panel
        private System.Windows.Forms.Panel panelFixedSpeed;
        private System.Windows.Forms.TrackBar trackBarFanSpeed;
        private System.Windows.Forms.Label labelFixedSpeedValue;

        // Per-fan panel
        private System.Windows.Forms.Panel panelPerFan;
        private System.Windows.Forms.CheckBox checkBoxPerFan;
        private System.Windows.Forms.Label labelFanSelect;
        private System.Windows.Forms.ComboBox comboBoxFanSelect;

        // Stats panel
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Label labelCpuTempTitle;
        private System.Windows.Forms.Label labelCpuTemp;
        private System.Windows.Forms.Label labelRpmTitle;
        private System.Windows.Forms.Label labelRPM;
        private System.Windows.Forms.Label labelAppliedSpeedTitle;
        private System.Windows.Forms.Label labelAppliedSpeed;
    }
}
