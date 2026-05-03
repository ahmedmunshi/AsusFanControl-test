using AsusFanControl.Models;
using AsusFanControlGUI.Theme;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace AsusFanControlGUI.Controls
{
    public class FanCurveEditor : UserControl
    {
        // Axis range
        private const int TEMP_MIN = 20;
        private const int TEMP_MAX = 100;
        private const int SPEED_MIN = 0;
        private const int SPEED_MAX = 100;

        // Layout margins (pixels)
        private const int MARGIN_LEFT = 45;
        private const int MARGIN_RIGHT = 15;
        private const int MARGIN_TOP = 15;
        private const int MARGIN_BOTTOM = 30;

        // Point interaction
        private const int POINT_RADIUS = 6;
        private const int HIT_RADIUS = 12;

        private FanCurve _curve;
        private int? _currentTemperature;
        private int? _unsafeThreshold;

        private int _dragIndex = -1;
        private int _hoverIndex = -1;
        private bool _isDragging;

        private ContextMenuStrip _contextMenu;
        private Point _contextMenuLocation;

        public event EventHandler CurveChanged;

        public FanCurve Curve
        {
            get => _curve;
            set
            {
                _curve = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Set this to show a vertical marker at the current CPU temperature.
        /// </summary>
        public int? CurrentTemperature
        {
            get => _currentTemperature;
            set
            {
                _currentTemperature = value;
                Invalidate();
            }
        }

        /// <summary>
        /// If set, draws a danger zone below this fan speed percentage.
        /// </summary>
        public int? UnsafeThreshold
        {
            get => _unsafeThreshold;
            set
            {
                _unsafeThreshold = value;
                Invalidate();
            }
        }

        public FanCurveEditor()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = DarkTheme.Surface;
            MinimumSize = new Size(300, 200);

            BuildContextMenu();
        }

        private void BuildContextMenu()
        {
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Renderer = new DarkMenuRenderer();
            _contextMenu.BackColor = DarkTheme.Surface;
            _contextMenu.ForeColor = DarkTheme.TextPrimary;

            var addItem = new ToolStripMenuItem("Add Point");
            addItem.BackColor = DarkTheme.Surface;
            addItem.ForeColor = DarkTheme.TextPrimary;
            addItem.Click += OnAddPointClick;

            var removeItem = new ToolStripMenuItem("Remove Point");
            removeItem.BackColor = DarkTheme.Surface;
            removeItem.ForeColor = DarkTheme.TextPrimary;
            removeItem.Click += OnRemovePointClick;

            _contextMenu.Items.Add(addItem);
            _contextMenu.Items.Add(removeItem);
        }

        #region Coordinate Conversion

        private RectangleF GetPlotArea()
        {
            return new RectangleF(
                MARGIN_LEFT,
                MARGIN_TOP,
                Width - MARGIN_LEFT - MARGIN_RIGHT,
                Height - MARGIN_TOP - MARGIN_BOTTOM
            );
        }

        private PointF CurvePointToPixel(FanCurvePoint pt)
        {
            var area = GetPlotArea();
            float x = area.X + (float)(pt.Temperature - TEMP_MIN) / (TEMP_MAX - TEMP_MIN) * area.Width;
            float y = area.Y + area.Height - (float)(pt.FanSpeedPercent - SPEED_MIN) / (SPEED_MAX - SPEED_MIN) * area.Height;
            return new PointF(x, y);
        }

        private FanCurvePoint PixelToCurvePoint(Point pixel)
        {
            var area = GetPlotArea();
            int temp = (int)Math.Round(TEMP_MIN + (pixel.X - area.X) / area.Width * (TEMP_MAX - TEMP_MIN));
            int speed = (int)Math.Round(SPEED_MAX - (pixel.Y - area.Y) / area.Height * (SPEED_MAX - SPEED_MIN));

            temp = Math.Max(TEMP_MIN, Math.Min(TEMP_MAX, temp));
            speed = Math.Max(SPEED_MIN, Math.Min(SPEED_MAX, speed));

            return new FanCurvePoint(temp, speed);
        }

        private float TempToPixelX(int temp)
        {
            var area = GetPlotArea();
            return area.X + (float)(temp - TEMP_MIN) / (TEMP_MAX - TEMP_MIN) * area.Width;
        }

        private float SpeedToPixelY(int speed)
        {
            var area = GetPlotArea();
            return area.Y + area.Height - (float)(speed - SPEED_MIN) / (SPEED_MAX - SPEED_MIN) * area.Height;
        }

        #endregion

        #region Hit Testing

        private int HitTestPoint(Point mousePos)
        {
            if (_curve == null || _curve.Points == null)
                return -1;

            for (int i = 0; i < _curve.Points.Count; i++)
            {
                var px = CurvePointToPixel(_curve.Points[i]);
                float dx = mousePos.X - px.X;
                float dy = mousePos.Y - px.Y;

                if (dx * dx + dy * dy <= HIT_RADIUS * HIT_RADIUS)
                    return i;
            }

            return -1;
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var area = GetPlotArea();

            DrawBackground(g, area);
            DrawUnsafeZone(g, area);
            DrawGrid(g, area);
            DrawCurve(g, area);
            DrawTemperatureMarker(g, area);
            DrawPoints(g);
        }

        private void DrawBackground(Graphics g, RectangleF area)
        {
            using (var brush = new SolidBrush(DarkTheme.Background))
            {
                g.FillRectangle(brush, area);
            }

            using (var pen = new Pen(DarkTheme.Border))
            {
                g.DrawRectangle(pen, area.X, area.Y, area.Width, area.Height);
            }
        }

        private void DrawUnsafeZone(Graphics g, RectangleF area)
        {
            if (!_unsafeThreshold.HasValue)
                return;

            float y = SpeedToPixelY(_unsafeThreshold.Value);

            if (y < area.Bottom)
            {
                using (var brush = new SolidBrush(DarkTheme.DangerZone))
                {
                    g.FillRectangle(brush, area.X, y, area.Width, area.Bottom - y);
                }
            }
        }

        private void DrawGrid(Graphics g, RectangleF area)
        {
            using (var gridPen = new Pen(DarkTheme.GridLines) { DashStyle = DashStyle.Dot })
            using (var axisFont = new Font("Segoe UI", 7.5f))
            using (var textBrush = new SolidBrush(DarkTheme.TextSecondary))
            {
                var textFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                var tempFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };

                // Horizontal grid lines (speed)
                for (int speed = SPEED_MIN; speed <= SPEED_MAX; speed += 10)
                {
                    float y = SpeedToPixelY(speed);

                    if (speed > SPEED_MIN && speed < SPEED_MAX)
                        g.DrawLine(gridPen, area.X, y, area.Right, y);

                    // Y-axis labels
                    if (speed % 20 == 0)
                    {
                        g.DrawString($"{speed}%", axisFont, textBrush,
                            new RectangleF(0, y - 8, MARGIN_LEFT - 5, 16), textFormat);
                    }
                }

                // Vertical grid lines (temperature)
                for (int temp = TEMP_MIN; temp <= TEMP_MAX; temp += 10)
                {
                    float x = TempToPixelX(temp);

                    if (temp > TEMP_MIN && temp < TEMP_MAX)
                        g.DrawLine(gridPen, x, area.Y, x, area.Bottom);

                    // X-axis labels
                    g.DrawString($"{temp}°", axisFont, textBrush,
                        new RectangleF(x - 15, area.Bottom + 4, 30, 20), tempFormat);
                }
            }
        }

        private void DrawCurve(Graphics g, RectangleF area)
        {
            if (_curve == null || _curve.Points == null || _curve.Points.Count < 2)
                return;

            var pixelPoints = _curve.Points.Select(p => CurvePointToPixel(p)).ToArray();

            // Fill area under curve
            if (pixelPoints.Length >= 2)
            {
                using (var fillPath = new GraphicsPath())
                {
                    fillPath.AddLines(pixelPoints);
                    fillPath.AddLine(pixelPoints[pixelPoints.Length - 1].X, pixelPoints[pixelPoints.Length - 1].Y,
                                     pixelPoints[pixelPoints.Length - 1].X, area.Bottom);
                    fillPath.AddLine(pixelPoints[pixelPoints.Length - 1].X, area.Bottom,
                                     pixelPoints[0].X, area.Bottom);
                    fillPath.CloseFigure();

                    using (var fillBrush = new SolidBrush(DarkTheme.CurveFill))
                    {
                        g.FillPath(fillBrush, fillPath);
                    }
                }
            }

            // Draw curve line
            using (var curvePen = new Pen(DarkTheme.CurveLine, 2.5f))
            {
                curvePen.LineJoin = LineJoin.Round;

                // Extend line to edges
                var allPoints = new List<PointF>();

                // Extend left
                if (_curve.Points[0].Temperature > TEMP_MIN)
                {
                    float leftX = TempToPixelX(TEMP_MIN);
                    float leftY = SpeedToPixelY(_curve.Points[0].FanSpeedPercent);
                    allPoints.Add(new PointF(leftX, leftY));
                }

                allPoints.AddRange(pixelPoints);

                // Extend right
                if (_curve.Points[_curve.Points.Count - 1].Temperature < TEMP_MAX)
                {
                    float rightX = TempToPixelX(TEMP_MAX);
                    float rightY = SpeedToPixelY(_curve.Points[_curve.Points.Count - 1].FanSpeedPercent);
                    allPoints.Add(new PointF(rightX, rightY));
                }

                if (allPoints.Count >= 2)
                    g.DrawLines(curvePen, allPoints.ToArray());
            }
        }

        private void DrawTemperatureMarker(Graphics g, RectangleF area)
        {
            if (!_currentTemperature.HasValue)
                return;

            int temp = _currentTemperature.Value;
            if (temp < TEMP_MIN || temp > TEMP_MAX)
                return;

            float x = TempToPixelX(temp);

            using (var markerPen = new Pen(DarkTheme.TempMarker, 1.5f) { DashStyle = DashStyle.Dash })
            {
                g.DrawLine(markerPen, x, area.Y, x, area.Bottom);
            }

            // Draw temperature label at the top
            using (var font = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var brush = new SolidBrush(DarkTheme.TempMarkerLabel))
            {
                var text = $"{temp}°C";
                var textSize = g.MeasureString(text, font);
                float labelX = x - textSize.Width / 2;
                float labelY = area.Y - textSize.Height - 1;

                // Clamp to visible area
                labelX = Math.Max(area.X, Math.Min(area.Right - textSize.Width, labelX));
                if (labelY < 0) labelY = area.Y + 2;

                g.DrawString(text, font, brush, labelX, labelY);
            }

            // Draw the interpolated speed at this temp
            if (_curve != null && _curve.Points != null && _curve.Points.Count >= 2)
            {
                int speed = _curve.Interpolate(temp);
                float y = SpeedToPixelY(speed);

                // Draw a dot at the intersection
                using (var dotBrush = new SolidBrush(DarkTheme.TempMarker))
                {
                    g.FillEllipse(dotBrush, x - 4, y - 4, 8, 8);
                }
            }
        }

        private void DrawPoints(Graphics g)
        {
            if (_curve == null || _curve.Points == null)
                return;

            for (int i = 0; i < _curve.Points.Count; i++)
            {
                var px = CurvePointToPixel(_curve.Points[i]);
                bool isHovered = (i == _hoverIndex);
                bool isDragged = (i == _dragIndex && _isDragging);

                int radius = isHovered || isDragged ? POINT_RADIUS + 2 : POINT_RADIUS;

                // Outer ring
                using (var borderPen = new Pen(isHovered ? DarkTheme.PointHover : DarkTheme.PointBorder, 2f))
                {
                    g.DrawEllipse(borderPen, px.X - radius, px.Y - radius, radius * 2, radius * 2);
                }

                // Inner fill
                using (var fillBrush = new SolidBrush(isDragged ? DarkTheme.PointHover : DarkTheme.PointFill))
                {
                    g.FillEllipse(fillBrush, px.X - radius + 1, px.Y - radius + 1,
                        radius * 2 - 2, radius * 2 - 2);
                }

                // Tooltip label when hovered or dragged
                if (isHovered || isDragged)
                {
                    DrawPointLabel(g, _curve.Points[i], px);
                }
            }
        }

        private void DrawPointLabel(Graphics g, FanCurvePoint pt, PointF px)
        {
            using (var font = new Font("Segoe UI", 7.5f))
            using (var bgBrush = new SolidBrush(Color.FromArgb(200, DarkTheme.Surface.R, DarkTheme.Surface.G, DarkTheme.Surface.B)))
            using (var textBrush = new SolidBrush(DarkTheme.TextPrimary))
            using (var borderPen = new Pen(DarkTheme.Border))
            {
                var text = $"{pt.Temperature}°C : {pt.FanSpeedPercent}%";
                var textSize = g.MeasureString(text, font);
                float labelX = px.X - textSize.Width / 2;
                float labelY = px.Y - POINT_RADIUS - textSize.Height - 6;

                // If too close to top, draw below
                if (labelY < GetPlotArea().Y)
                    labelY = px.Y + POINT_RADIUS + 4;

                var labelRect = new RectangleF(labelX - 3, labelY - 1, textSize.Width + 6, textSize.Height + 2);
                g.FillRectangle(bgBrush, labelRect);
                g.DrawRectangle(borderPen, labelRect.X, labelRect.Y, labelRect.Width, labelRect.Height);
                g.DrawString(text, font, textBrush, labelX, labelY);
            }
        }

        #endregion

        #region Mouse Interaction

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                int hitIndex = HitTestPoint(e.Location);
                if (hitIndex >= 0)
                {
                    _dragIndex = hitIndex;
                    _isDragging = true;
                    Cursor = Cursors.Hand;
                    Capture = true;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                _contextMenuLocation = e.Location;
                int hitIndex = HitTestPoint(e.Location);

                // Enable/disable remove based on hit test and minimum points
                _contextMenu.Items[1].Visible = hitIndex >= 0 && _curve != null && _curve.Points.Count > 2;
                _contextMenu.Items[1].Tag = hitIndex;

                // Only allow add if clicking in the plot area
                var area = GetPlotArea();
                _contextMenu.Items[0].Visible = hitIndex < 0 && area.Contains(e.Location);

                if (_contextMenu.Items[0].Visible || _contextMenu.Items[1].Visible)
                    _contextMenu.Show(this, e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isDragging && _dragIndex >= 0 && _curve != null)
            {
                var newPt = PixelToCurvePoint(e.Location);

                // Constrain temperature so points don't cross each other
                if (_dragIndex > 0)
                    newPt.Temperature = Math.Max(_curve.Points[_dragIndex - 1].Temperature + 1, newPt.Temperature);
                if (_dragIndex < _curve.Points.Count - 1)
                    newPt.Temperature = Math.Min(_curve.Points[_dragIndex + 1].Temperature - 1, newPt.Temperature);

                _curve.Points[_dragIndex].Temperature = newPt.Temperature;
                _curve.Points[_dragIndex].FanSpeedPercent = newPt.FanSpeedPercent;

                Invalidate();
            }
            else
            {
                int oldHover = _hoverIndex;
                _hoverIndex = HitTestPoint(e.Location);

                if (_hoverIndex != oldHover)
                {
                    Cursor = _hoverIndex >= 0 ? Cursors.Hand : Cursors.Default;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isDragging)
            {
                _isDragging = false;
                _dragIndex = -1;
                Capture = false;
                Cursor = _hoverIndex >= 0 ? Cursors.Hand : Cursors.Default;
                CurveChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (!_isDragging)
            {
                _hoverIndex = -1;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        #endregion

        #region Context Menu Actions

        private void OnAddPointClick(object sender, EventArgs e)
        {
            if (_curve == null)
                return;

            var newPoint = PixelToCurvePoint(_contextMenuLocation);

            // Ensure no duplicate temperature
            if (_curve.Points.Any(p => p.Temperature == newPoint.Temperature))
                newPoint.Temperature++;

            _curve.AddPoint(newPoint);
            CurveChanged?.Invoke(this, EventArgs.Empty);
            Invalidate();
        }

        private void OnRemovePointClick(object sender, EventArgs e)
        {
            if (_curve == null)
                return;

            var menuItem = sender as ToolStripMenuItem;
            if (menuItem?.Tag is int index && index >= 0 && index < _curve.Points.Count)
            {
                _curve.RemovePoint(_curve.Points[index]);
                _hoverIndex = -1;
                CurveChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        #endregion
    }
}
