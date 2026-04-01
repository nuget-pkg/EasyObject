using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class Signer
    {
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public string PurpleType { get; set; }
    }
}
