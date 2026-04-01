using System;
using System.Text;
using Xunit;

namespace StellarBase.Tests.Tests
{
    public class ConversionTests
    {
        [Fact]
        public void AccountIdConversion()
        {
            StellarBase.Network.CurrentNetwork = "ProjectQ";

            var masterPair = KeyPair.Master();
            var masterAccount = masterPair.AccountId;

            string sample64 = "AAAAAP7Ru1nO+h1oAv7VJP5i+LRBxajZxBQ+gOtOLhkssYBm";
            byte[] sample = Convert.FromBase64String(sample64);

            var reader = new StellarBase.Generated.ByteReader(sample);
            var sampleAccount = StellarBase.Generated.AccountID.Decode(reader);

            Assert.Equal(
                masterAccount.InnerValue.Discriminant.InnerValue,
                sampleAccount.InnerValue.Discriminant.InnerValue);

            Assert.Equal(
                masterAccount.InnerValue.Ed25519.InnerValue,
                sampleAccount.InnerValue.Ed25519.InnerValue);

            var writer = new StellarBase.Generated.ByteWriter();
            StellarBase.Generated.AccountID.Encode(writer, masterAccount);
            string master64 = Convert.ToBase64String(writer.ToArray());

            Assert.Equal(sample64, master64);
        }

        [Fact]
        public void NativeAssetConversion()
        {
            StellarBase.Network.CurrentNetwork = "";

            var native = new Asset().ToXDR();

            string sample64 = "AAAAAA==";
            byte[] sample = Convert.FromBase64String(sample64);

            var reader = new StellarBase.Generated.ByteReader(sample);
            var sampleAsset = StellarBase.Generated.Asset.Decode(reader);

            Assert.Equal(
                native.Discriminant.InnerValue,
                sampleAsset.Discriminant.InnerValue);

            var writer = new StellarBase.Generated.ByteWriter();
            StellarBase.Generated.Asset.Encode(writer, native);
            string native64 = Convert.ToBase64String(writer.ToArray());

            Assert.Equal(sample64, native64);
        }

        [Fact]
        public void CustomAssetConversion()
        {
            StellarBase.Network.CurrentNetwork = "ProjectQ";
            var master = KeyPair.Master();

            var alphaNum4 = new StellarBase.Generated.Asset.AssetAlphaNum4
            {
                AssetCode = ASCIIEncoding.ASCII.GetBytes("USD\0"),
                Issuer = master.AccountId
            };

            var asset = new StellarBase.Generated.Asset
            {
                AlphaNum4 = alphaNum4,
                Discriminant = StellarBase.Generated.AssetType.Create(StellarBase.Generated.AssetType.AssetTypeEnum.ASSET_TYPE_CREDIT_ALPHANUM4)
            };

            string sample64 = "AAAAAVVTRAAAAAAA/tG7Wc76HWgC/tUk/mL4tEHFqNnEFD6A604uGSyxgGY=";
            byte[] sample = Convert.FromBase64String(sample64);

            var reader = new StellarBase.Generated.ByteReader(sample);
            var sampleAsset = StellarBase.Generated.Asset.Decode(reader);

            Assert.Equal(
                asset.Discriminant.InnerValue,
                sampleAsset.Discriminant.InnerValue);

            Assert.Equal(
                asset.AlphaNum4.AssetCode,
                sampleAsset.AlphaNum4.AssetCode);

            Assert.Equal(
                asset.AlphaNum4.Issuer.InnerValue.Ed25519.InnerValue,
                sampleAsset.AlphaNum4.Issuer.InnerValue.Ed25519.InnerValue);

            var writer = new StellarBase.Generated.ByteWriter();
            StellarBase.Generated.Asset.Encode(writer, asset);
            string native64 = Convert.ToBase64String(writer.ToArray());

            Assert.Equal(sample64, native64);
        }
    }
}