using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class TransactionResult
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("ledger")]
        public long Ledger { get; set; }

        [JsonProperty("envelope_xdr")]
        public string EnvelopeXdr { get; set; }

        [JsonProperty("result_xdr")]
        public string ResultXdr { get; set; }

        [JsonProperty("result_meta_xdr")]
        public string ResultMetaXdr { get; set; }

        public static TransactionResult FromJson(string json) => JsonConvert.DeserializeObject<TransactionResult>(json, Converter.Settings);

        public static string ToJson(TransactionResult self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
