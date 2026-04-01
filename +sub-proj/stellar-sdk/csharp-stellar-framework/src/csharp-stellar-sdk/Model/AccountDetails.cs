using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class AccountDetails
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("paging_token")]
        public string PagingToken { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("subentry_count")]
        public long SubentryCount { get; set; }

        [JsonProperty("thresholds")]
        public Thresholds Thresholds { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("balances")]
        public AccountBalance[] Balances { get; set; }

        [JsonProperty("signers")]
        public Signer[] Signers { get; set; }

        //[JsonProperty("data")]
        //public AccountDetailsData[] Data { get; set; }

        public static AccountDetails FromJson(string json) => JsonConvert.DeserializeObject<AccountDetails>(json, Converter.Settings);

        public static string ToJson(AccountDetails self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

}
