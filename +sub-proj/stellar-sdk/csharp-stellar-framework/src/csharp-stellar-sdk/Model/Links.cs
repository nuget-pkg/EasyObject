using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class Links
    {
        [JsonProperty("account")]
        public LinksData Account { get; set; }

        [JsonProperty("ledger")]
        public LinksData Ledger { get; set; }

        [JsonProperty("precedes")]
        public LinksData Precedes { get; set; }

        [JsonProperty("self")]
        public LinksData Self { get; set; }

        [JsonProperty("next")]
        public LinksData Next { get; set; }

        [JsonProperty("prev")]
        public LinksData Prev { get; set; }

        [JsonProperty("succeeds")]
        public LinksData Succeeds { get; set; }

        [JsonProperty("transactions")]
        public LinksData Transactions { get; set; }

        [JsonProperty("operations")]
        public LinksData Operations { get; set; }

        [JsonProperty("payments")]
        public LinksData Payments { get; set; }

        [JsonProperty("effects")]
        public LinksData Effects { get; set; }

        [JsonProperty("offers")]
        public LinksData Offers { get; set; }

        [JsonProperty("trades")]
        public LinksData Trades { get; set; }

        [JsonProperty("data")]
        public LinksData Data { get; set; }
    }
}
