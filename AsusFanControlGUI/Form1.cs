using AsusFanControl;
using AsusFanControl.Models;
using AsusFanControl.Services;
using AsusFanControlGUI.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AsusFanControlGUI
{
    public partial class Form1 : Form
    {
        private AsusControl asusControl;
        private ProfileManager profileManager;
        private FanCurveEngine curveEngine;
        private List<FanProfile> profiles;
        private FanProfile activeProfile;
        private NotifyIcon trayIcon;

        private bool _suppressProfileEvents;
        private int _selectedFanIndex = -1; // -1 = All (default curve)

        public Form1()
        {
            InitializeComponent();

            asusControl = new AsusControl();
            profileManager = new ProfileManager();
            curveEngine = new FanCurveEngine(asusControl);
            curveEngine.OnTick += CurveEngine_OnTick;

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Apply dark theme
            DarkTheme.Apply(this);

            // Load settings
            toolStripMenuItemTurnOffControlOnExit.Checked = Properties.Settings.Default.turnOffControlOnExit;
            toolStripMenuItemForbidUnsafeSettings.Checked = Properties.Settings.Default.forbidUnsafeSettings;
            toolStripMenuItemMinimizeToTrayOnClose.Checked = Properties.Settings.Default.minimizeToTrayOnClose;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProfiles();
            UpdateUnsafeThreshold();
        }

        #region Profile Management

        private void LoadProfiles()
        {
            _suppressProfileEvents = true;

            profiles = profileManager.LoadAll();
            comboBoxProfile.Items.Clear();
            foreach (var profile in profiles)
                comboBoxProfile.Items.Add(profile.Name);

            // Restore last active profile
            var lastProfile = Properties.Settings.Default.activeProfileName;
            var idx = profiles.FindIndex(p => p.Name == lastProfile);
            if (idx < 0) idx = 0;

            comboBoxProfile.SelectedIndex = idx;
            _suppressProfileEvents = false;

            SelectProfile(idx);
        }

        private void SelectProfile(int index)
        {
            if (index < 0 || index >= profiles.Count)
                return;

            activeProfile = profiles[index];

            Properties.Settings.Default.activeProfileName = activeProfile.Name;
            Properties.Settings.Default.Save();

            // Update mode radio buttons
            _suppressProfileEvents = true;
            radioButtonCurve.Checked = (activeProfile.Mode == "curve");
            radioButtonFixed.Checked = (activeProfile.Mode == "fixed");

            // Update fixed speed slider
            trackBarFanSpeed.Value = Math.Max(0, Math.Min(100, activeProfile.FixedSpeedPercent));
            labelFixedSpeedValue.Text = $"{activeProfile.FixedSpeedPercent}%";

            // Update per-fan checkbox
            checkBoxPerFan.Checked = activeProfile.UsePerFanCurves;

            // Update fan selector
            PopulateFanSelector();

            _suppressProfileEvents = false;

            // Update curve editor
            UpdateCurveEditor();
            UpdateModeVisibility();
            RestartEngineIfEnabled();
        }

        private void PopulateFanSelector()
        {
            comboBoxFanSelect.Items.Clear();
            comboBoxFanSelect.Items.Add("All (Default)");

            try
            {
                var fanCount = asusControl.HealthyTable_FanCounts();
                for (int i = 0; i < fanCount; i++)
                    comboBoxFanSelect.Items.Add($"Fan {i}");
            }
            catch { }

            comboBoxFanSelect.SelectedIndex = 0;
            _selectedFanIndex = -1;
        }

        private void comboBoxProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressProfileEvents) return;
            SelectProfile(comboBoxProfile.SelectedIndex);
        }

        private void buttonAddProfile_Click(object sender, EventArgs e)
        {
            var name = ShowInputDialog("New Profile", "Enter profile name:");
            if (string.IsNullOrWhiteSpace(name)) return;

            if (profiles.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("A profile with that name already exists.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newProfile = FanProfile.CreateDefault();
            newProfile.Name = name;
            profileManager.Save(newProfile);

            LoadProfiles();

            // Select the new profile
            var idx = profiles.FindIndex(p => p.Name == name);
            if (idx >= 0)
            {
                comboBoxProfile.SelectedIndex = idx;
                SelectProfile(idx);
            }
        }

        private void buttonRenameProfile_Click(object sender, EventArgs e)
        {
            if (activeProfile == null) return;

            var newName = ShowInputDialog("Rename Profile", "Enter new name:", activeProfile.Name);
            if (string.IsNullOrWhiteSpace(newName) || newName == activeProfile.Name) return;

            if (profiles.Any(p => p.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("A profile with that name already exists.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            profileManager.Rename(activeProfile, newName);
            LoadProfiles();

            var idx = profiles.FindIndex(p => p.Name == newName);
            if (idx >= 0)
            {
                comboBoxProfile.SelectedIndex = idx;
                SelectProfile(idx);
            }
        }

        private void buttonDeleteProfile_Click(object sender, EventArgs e)
        {
            if (activeProfile == null || profiles.Count <= 1)
            {
                MessageBox.Show("Cannot delete the last remaining profile.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Delete profile \"{activeProfile.Name}\"?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                profileManager.Delete(activeProfile.Name);
                LoadProfiles();
            }
        }

        #endregion

        #region Mode Switching

        private void radioButtonMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressProfileEvents) return;

            if (activeProfile != null)
            {
                activeProfile.Mode = radioButtonCurve.Checked ? "curve" : "fixed";
                SaveActiveProfile();
            }

            UpdateModeVisibility();
            RestartEngineIfEnabled();
        }

        private void UpdateModeVisibility()
        {
            bool isCurveMode = radioButtonCurve.Checked;
            fanCurveEditor.Visible = isCurveMode;
            panelFixedSpeed.Visible = !isCurveMode;
            panelPerFan.Visible = isCurveMode;
        }

        #endregion

        #region Enable/Disable

        private void checkBoxEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnable.Checked)
                RestartEngineIfEnabled();
            else
                StopEngine(true);
        }

        private void RestartEngineIfEnabled()
        {
            if (!checkBoxEnable.Checked || activeProfile == null)
                return;

            curveEngine.Stop(false);
            curveEngine.Start(activeProfile);
        }

        private void StopEngine(bool resetFans)
        {
            curveEngine.Stop(resetFans);
        }

        #endregion

        #region Curve Editor

        private void UpdateCurveEditor()
        {
            if (activeProfile == null) return;

            FanCurve curve;
            if (activeProfile.UsePerFanCurves && _selectedFanIndex >= 0)
            {
                // Get or create per-fan curve
                if (!activeProfile.PerFanCurves.ContainsKey(_selectedFanIndex))
                    activeProfile.PerFanCurves[_selectedFanIndex] = activeProfile.DefaultCurve.Clone();

                curve = activeProfile.PerFanCurves[_selectedFanIndex];
            }
            else
            {
                curve = activeProfile.DefaultCurve;
            }

            fanCurveEditor.Curve = curve;
        }

        private void fanCurveEditor_CurveChanged(object sender, EventArgs e)
        {
            SaveActiveProfile();
            curveEngine.UpdateProfile(activeProfile);
        }

        private void checkBoxPerFan_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressProfileEvents) return;

            if (activeProfile != null)
            {
                activeProfile.UsePerFanCurves = checkBoxPerFan.Checked;
                SaveActiveProfile();
            }

            labelFanSelect.Visible = checkBoxPerFan.Checked;
            comboBoxFanSelect.Visible = checkBoxPerFan.Checked;

            if (!checkBoxPerFan.Checked)
            {
                _selectedFanIndex = -1;
                if (comboBoxFanSelect.Items.Count > 0)
                    comboBoxFanSelect.SelectedIndex = 0;
            }

            UpdateCurveEditor();
            RestartEngineIfEnabled();
        }

        private void comboBoxFanSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressProfileEvents) return;

            _selectedFanIndex = comboBoxFanSelect.SelectedIndex - 1; // 0 = "All", 1 = Fan 0, etc.
            UpdateCurveEditor();
        }

        #endregion

        #region Fixed Speed

        private void trackBarFanSpeed_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.forbidUnsafeSettings)
            {
                if (trackBarFanSpeed.Value < 40)
                    trackBarFanSpeed.Value = 40;
                else if (trackBarFanSpeed.Value > 99)
                    trackBarFanSpeed.Value = 99;
            }

            if (activeProfile != null)
            {
                activeProfile.FixedSpeedPercent = trackBarFanSpeed.Value;
                labelFixedSpeedValue.Text = $"{trackBarFanSpeed.Value}%";
                SaveActiveProfile();
                curveEngine.UpdateProfile(activeProfile);
            }
        }

        private void trackBarFanSpeed_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
                return;

            trackBarFanSpeed_MouseCaptureChanged(sender, e);
        }

        #endregion

        #region Engine Tick (GUI Update)

        private void CurveEngine_OnTick(object sender, FanCurveTickEventArgs e)
        {
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(() => CurveEngine_OnTick(sender, e)));
                }
                catch { }
                return;
            }

            // Update stats labels
            labelCpuTemp.Text = $"{e.Temperature}°C";
            labelRPM.Text = string.Join("  ", e.FanRpms.Select(r => $"{r}"));

            if (e.AppliedSpeeds != null && e.AppliedSpeeds.Count > 0)
            {
                var speeds = e.AppliedSpeeds.Values.Distinct().ToList();
                if (speeds.Count == 1)
                    labelAppliedSpeed.Text = $"{speeds[0]}%";
                else
                    labelAppliedSpeed.Text = string.Join("  ", e.AppliedSpeeds.Select(kv => $"F{kv.Key}:{kv.Value}%"));
            }

            // Update temperature marker on curve editor
            fanCurveEditor.CurrentTemperature = e.Temperature;
        }

        #endregion

        #region Settings

        private void toolStripMenuItemTurnOffControlOnExit_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.turnOffControlOnExit = toolStripMenuItemTurnOffControlOnExit.Checked;
            Properties.Settings.Default.Save();
        }

        private void toolStripMenuItemForbidUnsafeSettings_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.forbidUnsafeSettings = toolStripMenuItemForbidUnsafeSettings.Checked;
            Properties.Settings.Default.Save();
            UpdateUnsafeThreshold();
        }

        private void toolStripMenuItemMinimizeToTrayOnClose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.minimizeToTrayOnClose = toolStripMenuItemMinimizeToTrayOnClose.Checked;
            Properties.Settings.Default.Save();
        }

        private void toolStripMenuItemCheckForUpdates_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Karmel0x/AsusFanControl/releases");
        }

        private void UpdateUnsafeThreshold()
        {
            fanCurveEditor.UnsafeThreshold = Properties.Settings.Default.forbidUnsafeSettings ? 40 : (int?)null;
        }

        #endregion

        #region Lifecycle

        private void OnProcessExit(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.turnOffControlOnExit)
            {
                curveEngine.Stop(true);
            }
            else
            {
                curveEngine.Stop(false);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.minimizeToTrayOnClose && Visible)
            {
                if (trayIcon == null)
                {
                    trayIcon = new NotifyIcon()
                    {
                        Icon = Icon,
                        ContextMenu = new ContextMenu(new MenuItem[]
                        {
                            new MenuItem("Show", (s1, e1) =>
                            {
                                trayIcon.Visible = false;
                                Show();
                            }),
                            new MenuItem("Exit", (s1, e1) =>
                            {
                                Close();
                                trayIcon.Visible = false;
                                Application.Exit();
                            }),
                        }),
                    };

                    trayIcon.MouseClick += (s1, e1) =>
                    {
                        if (e1.Button != MouseButtons.Left)
                            return;

                        trayIcon.Visible = false;
                        Show();
                    };
                }

                trayIcon.Visible = true;
                e.Cancel = true;
                Hide();
            }
        }

        #endregion

        #region Helpers

        private void SaveActiveProfile()
        {
            if (activeProfile != null)
                profileManager.Save(activeProfile);
        }

        private string ShowInputDialog(string title, string prompt, string defaultValue = "")
        {
            using (var form = new Form())
            {
                form.Text = title;
                form.ClientSize = new System.Drawing.Size(300, 110);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.BackColor = DarkTheme.Background;
                form.ForeColor = DarkTheme.TextPrimary;

                var label = new Label
                {
                    Text = prompt,
                    Location = new System.Drawing.Point(10, 10),
                    AutoSize = true,
                    ForeColor = DarkTheme.TextPrimary
                };

                var textBox = new TextBox
                {
                    Location = new System.Drawing.Point(10, 35),
                    Size = new System.Drawing.Size(275, 22),
                    Text = defaultValue,
                    BackColor = DarkTheme.ComboBackground,
                    ForeColor = DarkTheme.TextPrimary,
                    BorderStyle = BorderStyle.FixedSingle
                };

                var okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new System.Drawing.Point(120, 70),
                    Size = new System.Drawing.Size(75, 28),
                    BackColor = DarkTheme.Accent,
                    ForeColor = DarkTheme.TextPrimary,
                    FlatStyle = FlatStyle.Flat
                };
                okButton.FlatAppearance.BorderSize = 0;

                var cancelButton = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new System.Drawing.Point(205, 70),
                    Size = new System.Drawing.Size(75, 28),
                    BackColor = DarkTheme.ButtonBackground,
                    ForeColor = DarkTheme.TextPrimary,
                    FlatStyle = FlatStyle.Flat
                };
                cancelButton.FlatAppearance.BorderColor = DarkTheme.Border;

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(okButton);
                form.Controls.Add(cancelButton);
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                return form.ShowDialog(this) == DialogResult.OK ? textBox.Text.Trim() : null;
            }
        }

        #endregion
    }
}
