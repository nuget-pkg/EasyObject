using System;
using System.Threading.Tasks;
using StellarSdk.Model;

namespace StellarSdk
{
    public class LedgerCallBuilder : CallBuilder
    {
        public LedgerCallBuilder(String serverUrl) : base(serverUrl)
        {
            isIdempotent = true;
            addSegment("ledgers");
        }

        public LedgerCallBuilder order(String o)
        {
            addParam("order", o);

            return this;
        }

        public LedgerCallBuilder limit(int l)
        {
            addParam("limit", Convert.ToString(l));

            return this;
        }

        public LedgerCallBuilder cursor(String c)
        {
            addParam("cursor", c);

            return this;
        }

        public async Task<AllLedgers> Call()
        {
            return AllLedgers.FromJson(await base.DoCall());
        }
    }
}
