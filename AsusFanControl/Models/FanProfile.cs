using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AsusFanControl.Models
{
    public class FanProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; } // "fixed" or "curve"

        [JsonProperty("fixedSpeedPercent")]
        public int FixedSpeedPercent { get; set; }

        [JsonProperty("defaultCurve")]
        public FanCurve DefaultCurve { get; set; }

        [JsonProperty("perFanCurves")]
        public Dictionary<int, FanCurve> PerFanCurves { get; set; }

        [JsonProperty("usePerFanCurves")]
        public bool UsePerFanCurves { get; set; }

        public FanProfile()
        {
            Mode = "curve";
            FixedSpeedPercent = 80;
            DefaultCurve = FanCurve.CreateDefault();
            PerFanCurves = new Dictionary<int, FanCurve>();
            UsePerFanCurves = false;
        }

        public FanCurve GetCurveForFan(int fanIndex)
        {
            if (UsePerFanCurves && PerFanCurves.ContainsKey(fanIndex))
                return PerFanCurves[fanIndex];

            return DefaultCurve;
        }

        public FanProfile Clone()
        {
            return new FanProfile
            {
                Name = Name,
                Mode = Mode,
                FixedSpeedPercent = FixedSpeedPercent,
                DefaultCurve = DefaultCurve.Clone(),
                PerFanCurves = PerFanCurves.ToDictionary(kv => kv.Key, kv => kv.Value.Clone()),
                UsePerFanCurves = UsePerFanCurves
            };
        }

        public static FanProfile CreateDefault()
        {
            return new FanProfile
            {
                Name = "Default",
                Mode = "curve",
                FixedSpeedPercent = 80,
                DefaultCurve = FanCurve.CreateDefault(),
                PerFanCurves = new Dictionary<int, FanCurve>(),
                UsePerFanCurves = false
            };
        }

        public static FanProfile CreateSilent()
        {
            return new FanProfile
            {
                Name = "Silent",
                Mode = "curve",
                FixedSpeedPercent = 40,
                DefaultCurve = new FanCurve(new System.Collections.Generic.List<FanCurvePoint>
                {
                    new FanCurvePoint(30, 25),
                    new FanCurvePoint(50, 35),
                    new FanCurvePoint(70, 55),
                    new FanCurvePoint(85, 80)
                }),
                PerFanCurves = new Dictionary<int, FanCurve>(),
                UsePerFanCurves = false
            };
        }

        public static FanProfile CreatePerformance()
        {
            return new FanProfile
            {
                Name = "Performance",
                Mode = "curve",
                FixedSpeedPercent = 100,
                DefaultCurve = new FanCurve(new System.Collections.Generic.List<FanCurvePoint>
                {
                    new FanCurvePoint(30, 50),
                    new FanCurvePoint(50, 70),
                    new FanCurvePoint(70, 90),
                    new FanCurvePoint(80, 100)
                }),
                PerFanCurves = new Dictionary<int, FanCurve>(),
                UsePerFanCurves = false
            };
        }
    }
}
