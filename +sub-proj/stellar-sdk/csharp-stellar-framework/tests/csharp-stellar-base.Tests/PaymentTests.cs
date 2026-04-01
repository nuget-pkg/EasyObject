using StellarSdk;
using Xunit;

namespace StellarBase.Tests.Tests
{
    public class PaymentTests
    {
        static string horizon_url = "https://horizon-testnet.stellar.org/";

        [Fact]
        public async void TestPayments()
        {
            PaymentCallBuilder builder = new PaymentCallBuilder(horizon_url);
            builder.accountId("GBLQWS2KU3GW67KXQKAWWAML33465ZDVOWCEVV5TU2PHXMZUA3PFQM5C");
            builder.order("desc").limit(2);

            var payments = await builder.Call();

            Assert.Equal(2, payments.Embedded.Records.Length);
        }
    }
}
