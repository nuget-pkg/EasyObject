using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class Payments
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("_embedded")]
        public EmbeddedData Embedded { get; set; }

        public class EmbeddedData
        {
            [JsonProperty("records")]
            public Record[] Records { get; set; }
        }

        public class Record
        {
            [JsonProperty("_links")]
            public RecordLinks Links { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("paging_token")]
            public string PagingToken { get; set; }

            [JsonProperty("source_account")]
            public string SourceAccount { get; set; }

            [JsonProperty("type")]
            public string PurpleType { get; set; }

            [JsonProperty("type_i")]
            public long TypeI { get; set; }

            [JsonProperty("created_at")]
            public System.DateTime CreatedAt { get; set; }

            [JsonProperty("transaction_hash")]
            public string TransactionHash { get; set; }

            [JsonProperty("starting_balance")]
            public string StartingBalance { get; set; }

            [JsonProperty("funder")]
            public string Funder { get; set; }

            [JsonProperty("account")]
            public string Account { get; set; }

            [JsonProperty("into")]
            public string Into { get; set; }

            [JsonProperty("asset_type")]
            public string AssetType { get; set; }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("amount")]
            public string Amount { get; set; }
        }

        public class RecordLinks
        {
            [JsonProperty("self")]
            public Effects Self { get; set; }

            [JsonProperty("transaction")]
            public Effects Transaction { get; set; }

            [JsonProperty("effects")]
            public Effects Effects { get; set; }

            [JsonProperty("succeeds")]
            public Effects Succeeds { get; set; }

            [JsonProperty("precedes")]
            public Effects Precedes { get; set; }
        }

        public class Effects
        {
            [JsonProperty("href")]
            public string Href { get; set; }
        }

        public class PaymentsLinks
        {
            [JsonProperty("self")]
            public Effects Self { get; set; }

            [JsonProperty("next")]
            public Effects Next { get; set; }

            [JsonProperty("prev")]
            public Effects Prev { get; set; }
        }

        public static Payments FromJson(string json) => JsonConvert.DeserializeObject<Payments>(json, Converter.Settings);

        public static string ToJson(Payments self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
