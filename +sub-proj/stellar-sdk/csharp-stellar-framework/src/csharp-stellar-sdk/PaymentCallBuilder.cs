using System;
using System.Threading.Tasks;
using StellarSdk.Model;

namespace StellarSdk
{
    public class PaymentCallBuilder : CallBuilder
    {
        public PaymentCallBuilder(String serverUrl) : base(serverUrl)
        {
            isIdempotent = true;
        }

        public PaymentCallBuilder accountId(String id)
        {
            addSegment("accounts");
            addSegment(id);
            addSegment("payments");

            return this;
        }

        public PaymentCallBuilder ledger(String id)
        {
            addSegment("ledgers");
            addSegment(id);
            addSegment("payments");

            return this;
        }

        public PaymentCallBuilder transaction(String hash)
        {
            addSegment("transactions");
            addSegment(hash);
            addSegment("payments");

            return this;
        }

        public PaymentCallBuilder order(String o)
        {
            addParam("order", o);

            return this;
        }

        public PaymentCallBuilder limit(int l)
        {
            addParam("limit", Convert.ToString(l));

            return this;
        }

        public PaymentCallBuilder cursor(String c)
        {
            addParam("cursor", c);

            return this;
        }

        public async Task<Payments> Call()
        {
            return Payments.FromJson(await base.DoCall());
        }
    }
}
