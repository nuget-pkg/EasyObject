using System;
using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class LedgerDetails
    {
        [JsonProperty("_links")]
        public Links Links { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("paging_token")]
        public string PagingToken { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("prev_hash")]
        public string PrevHash { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        [JsonProperty("transaction_count")]
        public long TransactionCount { get; set; }

        [JsonProperty("operation_count")]
        public long OperationCount { get; set; }

        [JsonProperty("closed_at")]
        public DateTime ClosedAt { get; set; }

        [JsonProperty("total_coins")]
        public string TotalCoins { get; set; }

        [JsonProperty("fee_pool")]
        public string FeePool { get; set; }

        [JsonProperty("base_fee_in_stroops")]
        public long BaseFee { get; set; }

        [JsonProperty("base_reserve_in_stroops")]
        public string BaseReserve { get; set; }

        [JsonProperty("max_tx_set_size")]
        public long MaxTxSetSize { get; set; }

        [JsonProperty("protocol_version")]
        public long ProtocolVersion { get; set; }

        [JsonProperty("header_xdr")]
        public string HeaderXdr { get; set; }
    }
}
