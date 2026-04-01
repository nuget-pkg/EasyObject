using System;
using System.Text;
using Xunit;

namespace StellarBase.Tests.Tests
{
    public class AssetTests
    {
        [Fact]
        public void TestNativeAsset()
        {
            Asset asset = new Asset();

            Assert.Equal(Asset.AssetTypeEnum.ASSET_TYPE_NATIVE, asset.Type);

            StellarBase.Generated.Asset genAsset = asset.ToXDR();

            Assert.Equal(StellarBase.Generated.AssetType.AssetTypeEnum.ASSET_TYPE_NATIVE, genAsset.Discriminant.InnerValue);

            Asset resAsset = Asset.FromXDR(genAsset);

            Assert.Equal(Asset.AssetTypeEnum.ASSET_TYPE_NATIVE, resAsset.Type);
        }

        [Fact]
        public void TestAlphaNum4Asset()
        {
            var keyPair = KeyPair.Master();
            string code = "Test";
            Asset asset = new Asset(code, keyPair);

            Assert.Equal(code, asset.Code);
            Assert.Equal(keyPair, asset.Issuer);
            Assert.Equal(Asset.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM4, asset.Type);

            StellarBase.Generated.Asset genAsset = asset.ToXDR();

            Assert.Equal(Encoding.ASCII.GetBytes(code).ToString(), genAsset.AlphaNum4.AssetCode.ToString());
            Assert.Equal(keyPair.PublicKey.ToString(), genAsset.AlphaNum4.Issuer.InnerValue.Ed25519.InnerValue.ToString());
            Assert.Equal(StellarBase.Generated.AssetType.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM4, genAsset.Discriminant.InnerValue);

            Asset resAsset = Asset.FromXDR(genAsset);

            Assert.Equal(code, resAsset.Code);
            Assert.Equal(keyPair.Address, resAsset.Issuer.Address);
            Assert.Equal(keyPair.PublicKey.ToString(), resAsset.Issuer.PublicKey.ToString());
            Assert.Equal(Asset.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM4, resAsset.Type);
        }

        [Fact]
        public void TestAlphaNum12Asset()
        {
            var keyPair = KeyPair.Master();
            string code = "TestTestTest";
            Asset asset = new Asset(code, keyPair);

            Assert.Equal(code, asset.Code);
            Assert.Equal(keyPair, asset.Issuer);
            Assert.Equal(Asset.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM12, asset.Type);

            StellarBase.Generated.Asset genAsset = asset.ToXDR();

            Assert.Equal(Encoding.ASCII.GetBytes(code).ToString(), genAsset.AlphaNum12.AssetCode.ToString());
            Assert.Equal(keyPair.PublicKey.ToString(), genAsset.AlphaNum12.Issuer.InnerValue.Ed25519.InnerValue.ToString());
            Assert.Equal(StellarBase.Generated.AssetType.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM12, genAsset.Discriminant.InnerValue);

            Asset resAsset = Asset.FromXDR(genAsset);

            Assert.Equal(code, resAsset.Code);
            Assert.Equal(keyPair.Address, resAsset.Issuer.Address);
            Assert.Equal(keyPair.PublicKey.ToString(), resAsset.Issuer.PublicKey.ToString());
            Assert.Equal(Asset.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM12, resAsset.Type);
        }

        [Fact]
        public void TestAlphaNumAssetNullCode()
        {
            var ex = Assert.Throws<NullReferenceException>(() => new Asset(null, KeyPair.Master()));
            Assert.Equal(ex.Message, "code cannot be null.");
        }

        [Fact]
        public void TestAlphaNumAssetShortCode()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Asset("", KeyPair.Master()));
            Assert.Equal(ex.Message, "Invalid code, should have positive length, no larger than 12.");
        }

        [Fact]
        public void TestAlphaNumAssetLongCode()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Asset("ThisIsTooLongACode", KeyPair.Master()));
            Assert.Equal(ex.Message, "Invalid code, should have positive length, no larger than 12.");
        }

        [Fact]
        public void TestAlphaNumAssetNullIssuer()
        {
            var ex = Assert.Throws<NullReferenceException>(() => new Asset("Test", null));
            Assert.Equal(ex.Message, "issuer cannot be null.");
        }
    }
}
