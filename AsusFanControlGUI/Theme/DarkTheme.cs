using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AsusFanControlGUI.Theme
{
    public static class DarkTheme
    {
        // Core colors
        public static readonly Color Background = Color.FromArgb(30, 30, 30);
        public static readonly Color Surface = Color.FromArgb(45, 45, 45);
        public static readonly Color SurfaceLight = Color.FromArgb(55, 55, 55);
        public static readonly Color Border = Color.FromArgb(70, 70, 70);

        // Text
        public static readonly Color TextPrimary = Color.FromArgb(224, 224, 224);
        public static readonly Color TextSecondary = Color.FromArgb(160, 160, 160);
        public static readonly Color TextDisabled = Color.FromArgb(100, 100, 100);

        // Accent
        public static readonly Color Accent = Color.FromArgb(0, 120, 212);
        public static readonly Color AccentHover = Color.FromArgb(30, 144, 230);
        public static readonly Color AccentLight = Color.FromArgb(0, 120, 212, 80);

        // Curve editor
        public static readonly Color GridLines = Color.FromArgb(58, 58, 58);
        public static readonly Color CurveLine = Color.FromArgb(0, 180, 216);
        public static readonly Color CurveFill = Color.FromArgb(40, 0, 180, 216);
        public static readonly Color PointFill = Color.White;
        public static readonly Color PointBorder = Color.FromArgb(0, 180, 216);
        public static readonly Color PointHover = Color.FromArgb(255, 215, 0);
        public static readonly Color DangerZone = Color.FromArgb(30, 255, 68, 68);
        public static readonly Color TempMarker = Color.FromArgb(255, 215, 0);
        public static readonly Color TempMarkerLabel = Color.FromArgb(255, 215, 0);

        // Controls
        public static readonly Color ButtonBackground = Color.FromArgb(55, 55, 55);
        public static readonly Color ButtonHover = Color.FromArgb(70, 70, 70);
        public static readonly Color ComboBackground = Color.FromArgb(50, 50, 50);
        public static readonly Color CheckBoxCheck = Color.FromArgb(0, 180, 216);

        /// <summary>
        /// Applies dark theme to a control and all its children recursively.
        /// </summary>
        public static void Apply(Control control)
        {
            control.BackColor = Background;
            control.ForeColor = TextPrimary;

            foreach (Control child in control.Controls)
            {
                ApplyToControl(child);
            }

            // Handle MenuStrip specially
            if (control is Form form && form.MainMenuStrip != null)
            {
                ApplyToMenuStrip(form.MainMenuStrip);
            }
        }

        private static void ApplyToControl(Control control)
        {
            control.ForeColor = TextPrimary;

            if (control is Button button)
            {
                button.BackColor = ButtonBackground;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Border;
                button.FlatAppearance.MouseOverBackColor = ButtonHover;
                button.FlatAppearance.BorderSize = 1;
            }
            else if (control is ComboBox combo)
            {
                combo.BackColor = ComboBackground;
                combo.FlatStyle = FlatStyle.Flat;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = ComboBackground;
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.BackColor = Color.Transparent;
            }
            else if (control is RadioButton radio)
            {
                radio.BackColor = Color.Transparent;
            }
            else if (control is TrackBar)
            {
                control.BackColor = Surface;
            }
            else if (control is Label label)
            {
                label.BackColor = Color.Transparent;
            }
            else if (control is Panel panel)
            {
                panel.BackColor = Surface;
            }
            else if (control is GroupBox groupBox)
            {
                groupBox.BackColor = Surface;
            }
            else
            {
                control.BackColor = Background;
            }

            // Recurse into children
            foreach (Control child in control.Controls)
            {
                ApplyToControl(child);
            }
        }

        public static void ApplyToMenuStrip(MenuStrip menuStrip)
        {
            menuStrip.BackColor = Surface;
            menuStrip.ForeColor = TextPrimary;
            menuStrip.Renderer = new DarkMenuRenderer();

            foreach (ToolStripItem item in menuStrip.Items)
            {
                item.BackColor = Surface;
                item.ForeColor = TextPrimary;

                if (item is ToolStripMenuItem menuItem)
                    ApplyToMenuItem(menuItem);
            }
        }

        private static void ApplyToMenuItem(ToolStripMenuItem menuItem)
        {
            menuItem.BackColor = Surface;
            menuItem.ForeColor = TextPrimary;

            foreach (ToolStripItem sub in menuItem.DropDownItems)
            {
                sub.BackColor = Surface;
                sub.ForeColor = TextPrimary;

                if (sub is ToolStripMenuItem subMenu)
                    ApplyToMenuItem(subMenu);
            }
        }
    }

    /// <summary>
    /// Custom renderer for dark-themed menus.
    /// </summary>
    public class DarkMenuRenderer : ToolStripProfessionalRenderer
    {
        public DarkMenuRenderer() : base(new DarkMenuColorTable()) { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var rect = new Rectangle(Point.Empty, e.Item.Size);

            if (e.Item.Selected || e.Item.Pressed)
            {
                using (var brush = new SolidBrush(DarkTheme.ButtonHover))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
            else
            {
                using (var brush = new SolidBrush(DarkTheme.Surface))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            var rect = new Rectangle(e.ImageRectangle.X + 2, e.ImageRectangle.Y + 2,
                e.ImageRectangle.Width - 4, e.ImageRectangle.Height - 4);

            using (var pen = new Pen(DarkTheme.Accent, 2f))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Draw a checkmark
                e.Graphics.DrawLine(pen,
                    rect.X + 2, rect.Y + rect.Height / 2,
                    rect.X + rect.Width / 3, rect.Y + rect.Height - 3);
                e.Graphics.DrawLine(pen,
                    rect.X + rect.Width / 3, rect.Y + rect.Height - 3,
                    rect.X + rect.Width - 2, rect.Y + 2);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            using (var pen = new Pen(DarkTheme.Border))
            {
                int y = e.Item.Height / 2;
                e.Graphics.DrawLine(pen, 30, y, e.Item.Width - 4, y);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDownMenu)
            {
                using (var pen = new Pen(DarkTheme.Border))
                {
                    var rect = new Rectangle(0, 0, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            using (var brush = new SolidBrush(DarkTheme.Surface))
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (var brush = new SolidBrush(DarkTheme.Surface))
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }
    }

    public class DarkMenuColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => DarkTheme.ButtonHover;
        public override Color MenuItemSelectedGradientBegin => DarkTheme.ButtonHover;
        public override Color MenuItemSelectedGradientEnd => DarkTheme.ButtonHover;
        public override Color MenuBorder => DarkTheme.Border;
        public override Color MenuItemBorder => DarkTheme.Border;
        public override Color MenuItemPressedGradientBegin => DarkTheme.SurfaceLight;
        public override Color MenuItemPressedGradientEnd => DarkTheme.SurfaceLight;
        public override Color MenuStripGradientBegin => DarkTheme.Surface;
        public override Color MenuStripGradientEnd => DarkTheme.Surface;
        public override Color ToolStripDropDownBackground => DarkTheme.Surface;
        public override Color ImageMarginGradientBegin => DarkTheme.Surface;
        public override Color ImageMarginGradientMiddle => DarkTheme.Surface;
        public override Color ImageMarginGradientEnd => DarkTheme.Surface;
        public override Color SeparatorDark => DarkTheme.Border;
        public override Color SeparatorLight => DarkTheme.Border;
        public override Color CheckBackground => DarkTheme.Surface;
        public override Color CheckSelectedBackground => DarkTheme.ButtonHover;
        public override Color CheckPressedBackground => DarkTheme.ButtonHover;
    }
}
