using Newtonsoft.Json;

namespace AsusFanControl.Models
{
    public class FanCurvePoint
    {
        [JsonProperty("temperature")]
        public int Temperature { get; set; }

        [JsonProperty("fanSpeedPercent")]
        public int FanSpeedPercent { get; set; }

        public FanCurvePoint() { }

        public FanCurvePoint(int temperature, int fanSpeedPercent)
        {
            Temperature = temperature;
            FanSpeedPercent = fanSpeedPercent;
        }

        public FanCurvePoint Clone()
        {
            return new FanCurvePoint(Temperature, FanSpeedPercent);
        }
    }
}
