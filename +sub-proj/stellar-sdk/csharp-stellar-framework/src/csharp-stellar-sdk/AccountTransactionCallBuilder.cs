using System;
using System.Threading.Tasks;
using StellarSdk.Model;

namespace StellarSdk
{
    public class AccountTransactionCallBuilder : CallBuilder
    {
        public AccountTransactionCallBuilder(String serverUrl) : base(serverUrl)
        {
            isIdempotent = true;
            addSegment("accounts");
            addDetails("transactions");
        }

        public AccountTransactionCallBuilder accountId(String id)
        {
            addFilter(id);

            return this;
        }

        public AccountTransactionCallBuilder order(String o)
        {
            addParam("order", o);

            return this;
        }

        public AccountTransactionCallBuilder limit(int l)
        {
            addParam("limit", Convert.ToString(l));

            return this;
        }

        public AccountTransactionCallBuilder cursor(String c)
        {
            addParam("cursor", c);

            return this;
        }

        public async Task<Transactions> Call()
        {
            return Transactions.FromJson(await base.DoCall());
        }
    }
}
