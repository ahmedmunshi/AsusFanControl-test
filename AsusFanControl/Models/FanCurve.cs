using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsusFanControl.Models
{
    public class FanCurve
    {
        [JsonProperty("points")]
        public List<FanCurvePoint> Points { get; set; }

        public FanCurve()
        {
            Points = new List<FanCurvePoint>();
        }

        public FanCurve(List<FanCurvePoint> points)
        {
            Points = points ?? new List<FanCurvePoint>();
            SortPoints();
        }

        public void SortPoints()
        {
            Points = Points.OrderBy(p => p.Temperature).ToList();
        }

        /// <summary>
        /// Linearly interpolates the fan speed for the given temperature.
        /// Clamps to the min/max point values outside the defined range.
        /// </summary>
        public int Interpolate(int temperature)
        {
            if (Points == null || Points.Count == 0)
                return 0;

            if (Points.Count == 1)
                return Points[0].FanSpeedPercent;

            // Below the lowest defined point
            if (temperature <= Points[0].Temperature)
                return Points[0].FanSpeedPercent;

            // Above the highest defined point
            if (temperature >= Points[Points.Count - 1].Temperature)
                return Points[Points.Count - 1].FanSpeedPercent;

            // Find the two surrounding points and interpolate
            for (int i = 0; i < Points.Count - 1; i++)
            {
                var lower = Points[i];
                var upper = Points[i + 1];

                if (temperature >= lower.Temperature && temperature <= upper.Temperature)
                {
                    if (upper.Temperature == lower.Temperature)
                        return lower.FanSpeedPercent;

                    float ratio = (float)(temperature - lower.Temperature) / (upper.Temperature - lower.Temperature);
                    int speed = (int)Math.Round(lower.FanSpeedPercent + ratio * (upper.FanSpeedPercent - lower.FanSpeedPercent));
                    return Math.Max(0, Math.Min(100, speed));
                }
            }

            return Points[Points.Count - 1].FanSpeedPercent;
        }

        public void AddPoint(FanCurvePoint point)
        {
            Points.Add(point);
            SortPoints();
        }

        public bool RemovePoint(FanCurvePoint point)
        {
            if (Points.Count <= 2)
                return false;

            bool removed = Points.Remove(point);
            return removed;
        }

        public FanCurve Clone()
        {
            return new FanCurve(Points.Select(p => p.Clone()).ToList());
        }

        public static FanCurve CreateDefault()
        {
            return new FanCurve(new List<FanCurvePoint>
            {
                new FanCurvePoint(30, 30),
                new FanCurvePoint(50, 50),
                new FanCurvePoint(70, 80),
                new FanCurvePoint(85, 100)
            });
        }
    }
}
