using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class AllLedgers
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("_embedded")]
        public EmbeddedData Embedded { get; set; }

        public class EmbeddedData
        {
            [JsonProperty("records")]
            public LedgerDetails[] Records { get; set; }
        }

        public static AllLedgers FromJson(string json) => JsonConvert.DeserializeObject<AllLedgers>(json, Converter.Settings);

        public static string ToJson(AllLedgers self) => JsonConvert.SerializeObject(self, Converter.Settings);

    }
}
