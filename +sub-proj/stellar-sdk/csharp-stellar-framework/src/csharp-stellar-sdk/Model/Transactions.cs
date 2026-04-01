using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class Transactions
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("_embedded")]
        public EmbeddedData Embedded { get; set; }

        public class EmbeddedData
        {
            [JsonProperty("records")]
            public TransactionDetails[] Records { get; set; }
        }

        public static Transactions FromJson(string json) => JsonConvert.DeserializeObject<Transactions>(json, Converter.Settings);

        public static string ToJson(Transactions self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
