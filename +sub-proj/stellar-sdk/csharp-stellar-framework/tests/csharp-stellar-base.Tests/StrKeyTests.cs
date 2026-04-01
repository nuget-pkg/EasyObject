using System;
using Xunit;

namespace StellarBase.Tests.Tests
{
    public class StrKeyTests
    {
        [Fact]
        public void TestDecodeEncode()
        {
            String seed = "SDJHRQF4GCMIIKAAAQ6IHY42X73FQFLHUULAPSKKD4DFDM7UXWWCRHBE";
            byte[] secret = StrKey.DecodeCheck(VersionByte.ed25519SecretSeed, seed);
            String encoded = StrKey.EncodeCheck(VersionByte.ed25519SecretSeed, secret);
            Assert.Equal(seed, encoded);
        }

        [Fact]
        public void TestDecodeInvalidVersionByte()
        {
            String accountId = "GCZHXL5HXQX5ABDM26LHYRCQZ5OJFHLOPLZX47WEBP3V2PF5AVFK2A5D";
            var ex = Assert.Throws<FormatException>(() => StrKey.DecodeCheck(VersionByte.ed25519SecretSeed, accountId));
        }

        [Fact]
        public void TestDecodeInvalidSeed()
        {
            String seed = "SAA6NXOBOXP3RXGAXBW6PGFI5BPK4ODVAWITS4VDOMN5C2M4B66ZML";
            var ex = Assert.Throws<FormatException>(() => StrKey.DecodeCheck(VersionByte.ed25519SecretSeed, seed));
        }
    }
}
