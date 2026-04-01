using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class Thresholds
    {
        [JsonProperty("low_threshold")]
        public long LowThreshold { get; set; }

        [JsonProperty("med_threshold")]
        public long MedThreshold { get; set; }

        [JsonProperty("high_threshold")]
        public long HighThreshold { get; set; }
    }
}