using System;
using System.Threading.Tasks;
using StellarSdk.Model;

namespace StellarSdk
{
    public class TransactionCallBuilder : CallBuilder
    {
        public TransactionCallBuilder(String serverUrl) : base(serverUrl)
        {
            isIdempotent = true;
            addSegment("transactions");
        }

        public TransactionCallBuilder transaction(String hash)
        {
            addFilter(hash);

            return this;
        }

        public TransactionCallBuilder submitTransaction(String signedTx)
        {
            isIdempotent = false;
            addBody("tx", signedTx);

            return this;
        }

        public async Task<TransactionDetails> Call()
        {
            return TransactionDetails.FromJson(await base.DoCall());
        }
    }
}
