using Xunit;
using StellarSdk;

namespace StellarBase.Tests.Tests
{
    public class LedgerTests
    {
        static string horizon_url = "https://horizon-testnet.stellar.org/";

        [Fact]
        public async void TestLedgers()
        {
            LedgerCallBuilder builder = new LedgerCallBuilder(horizon_url);
            builder.order("desc").limit(2);

            var ledgers = await builder.Call();

            Assert.Equal(2, ledgers.Embedded.Records.Length);
            Assert.NotNull(ledgers.Embedded.Records[0].Id);
        }
    }
}
