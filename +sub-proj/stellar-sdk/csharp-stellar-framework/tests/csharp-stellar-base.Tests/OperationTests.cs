using System;
using Xunit;

namespace StellarBase.Tests.Tests
{
    public class OperationTests
    {
        [Fact]
        public void ChangeTrustOperation()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            var assetCode = "EUR";
            var asset = new Asset(assetCode, source);

            long limit = 100;

            ChangeTrustOperation operation = new ChangeTrustOperation.Builder(asset, limit)
                .SetSourceAccount(source)
                .Build();

            Assert.Equal(source.Address, operation.SourceAccount.Address);
            Assert.Equal(assetCode, operation.Asset.Code);
            Assert.Equal(asset.Issuer.Address, operation.Asset.Issuer.Address);
            Assert.Equal(asset.Type, operation.Asset.Type);
            Assert.Equal(limit, operation.Limit);
            Assert.Equal("AAAAAQAAAAC7JAuE3XvquOnbsgv2SRztjuk4RoBVefQ0rlrFMMQvfAAAAAYAAAABRVVSAAAAAAC7JAuE3XvquOnbsgv2SRztjuk4RoBVefQ0rlrFMMQvfAAAAAAAAABk",
                    operation.ToXdrBase64());

            StellarBase.Generated.Operation xdr = operation.ToXDR();
            ChangeTrustOperation parsedOperation = StellarBase.ChangeTrustOperation.FromXDR(xdr);

            Assert.Equal(source.Address, parsedOperation.SourceAccount.Address);
            Assert.Equal("EUR", parsedOperation.Asset.Code);
            Assert.Equal(source.Address, parsedOperation.Asset.Issuer.Address);
            Assert.Equal(limit, parsedOperation.Limit);
        }

        [Fact]
        public void ChangeTrustOperationNullAsset()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");

            long limit = 100;
            
            var ex = Assert.Throws<NullReferenceException>(() => new ChangeTrustOperation.Builder(null, limit)
                .SetSourceAccount(source)
                .Build());
            Assert.Equal(ex.Message, "asset cannot be null.");
        }

        [Fact]
        public void ChangeTrustOperationNegativeLimit()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            var assetCode = "EUR";
            var asset = new Asset(assetCode, source);

            var ex = Assert.Throws<ArgumentException>(() => new ChangeTrustOperation.Builder(asset, -1)
                .SetSourceAccount(source)
                .Build());
            Assert.Equal(ex.Message, "limit must be non-negative.");
        }

        [Fact]
        public void ChangeTrustOperationNullSource()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            var assetCode = "EUR";
            var asset = new Asset(assetCode, source);

            long limit = 100;

            var ex = Assert.Throws<NullReferenceException>(() => new ChangeTrustOperation.Builder(asset, limit)
                .SetSourceAccount(null)
                .Build());
            Assert.Equal(ex.Message, "sourceAccount cannot be null.");
        }

        [Fact]
        public void CreateAccountOperation()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            // GDW6AUTBXTOC7FIKUO5BOO3OGLK4SF7ZPOBLMQHMZDI45J2Z6VXRB5NR
            KeyPair destination = KeyPair.FromSeed("SDHZGHURAYXKU2KMVHPOXI6JG2Q4BSQUQCEOY72O3QQTCLR2T455PMII");
            var balance = 1000;

            CreateAccountOperation operation = new CreateAccountOperation.Builder(destination, balance)
                .SetSourceAccount(source)
                .Build();

            Assert.Equal(source.Address, operation.SourceAccount.Address);
            Assert.Equal(destination.Address, operation.Destination.Address);
            Assert.Equal(balance, operation.StartingBalance);
            Assert.Equal("AAAAAQAAAAC7JAuE3XvquOnbsgv2SRztjuk4RoBVefQ0rlrFMMQvfAAAAAAAAAAA7eBSYbzcL5UKo7oXO24y1ckX+XuCtkDsyNHOp1n1bxAAAAAAAAAD6A==",
                    operation.ToXdrBase64());

            StellarBase.Generated.Operation xdr = operation.ToXDR();
            CreateAccountOperation parsedOperation = StellarBase.CreateAccountOperation.FromXDR(xdr);

            Assert.Equal(source.Address, parsedOperation.SourceAccount.Address);
            Assert.Equal(destination.Address, parsedOperation.Destination.Address);
            Assert.Equal(1000, parsedOperation.StartingBalance);
        }

        [Fact]
        public void CreateAccountOperationNullDestination()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            var balance = 1000;
            
            var ex = Assert.Throws<NullReferenceException>(() => new CreateAccountOperation.Builder(null, balance)
                .SetSourceAccount(source)
                .Build());
            Assert.Equal(ex.Message, "destination cannot be null.");
        }

        [Fact]
        public void CreateAccountOperationNegativeStartingBalance()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            // GDW6AUTBXTOC7FIKUO5BOO3OGLK4SF7ZPOBLMQHMZDI45J2Z6VXRB5NR
            KeyPair destination = KeyPair.FromSeed("SDHZGHURAYXKU2KMVHPOXI6JG2Q4BSQUQCEOY72O3QQTCLR2T455PMII");
            
            var ex = Assert.Throws<ArgumentException>(() => new CreateAccountOperation.Builder(destination, -1)
                .SetSourceAccount(source)
                .Build());
            Assert.Equal(ex.Message, "startingBalance must be non-negative.");
        }

        [Fact]
        public void CreateAccountOperationNullSource()
        {
            // GDW6AUTBXTOC7FIKUO5BOO3OGLK4SF7ZPOBLMQHMZDI45J2Z6VXRB5NR
            KeyPair destination = KeyPair.FromSeed("SDHZGHURAYXKU2KMVHPOXI6JG2Q4BSQUQCEOY72O3QQTCLR2T455PMII");
            var balance = 1000;

            var ex = Assert.Throws<NullReferenceException>(() => new CreateAccountOperation.Builder(destination, balance)
                .SetSourceAccount(null)
                .Build());
            Assert.Equal(ex.Message, "sourceAccount cannot be null.");
        }

        [Fact]
        public void PaymentOperation()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            // GDW6AUTBXTOC7FIKUO5BOO3OGLK4SF7ZPOBLMQHMZDI45J2Z6VXRB5NR
            KeyPair destination = KeyPair.FromSeed("SDHZGHURAYXKU2KMVHPOXI6JG2Q4BSQUQCEOY72O3QQTCLR2T455PMII");

            Asset asset = new StellarBase.Asset();
            long amount = 1000;

            PaymentOperation operation = new PaymentOperation.Builder(destination, asset, amount)
                .SetSourceAccount(source)
                .Build();

            Assert.Equal(source.Address, operation.SourceAccount.Address);
            Assert.Equal(destination.Address, operation.Destination.Address);
            Assert.Equal(asset.Type, operation.Asset.Type);
            Assert.Equal(amount, operation.Amount);
            Assert.Equal(
                    "AAAAAQAAAAC7JAuE3XvquOnbsgv2SRztjuk4RoBVefQ0rlrFMMQvfAAAAAEAAAAA7eBSYbzcL5UKo7oXO24y1ckX+XuCtkDsyNHOp1n1bxAAAAAAAAAAAAAAA+g=",
                    operation.ToXdrBase64());

            StellarBase.Generated.Operation xdr = operation.ToXDR();
            PaymentOperation parsedOperation = StellarBase.PaymentOperation.FromXDR(xdr);

            Assert.Equal(source.Address, parsedOperation.SourceAccount.Address);
            Assert.Equal(destination.Address, parsedOperation.Destination.Address);
            Assert.Equal(asset.Type, parsedOperation.Asset.Type);
            Assert.Equal(amount, parsedOperation.Amount);
        }

        [Fact]
        public void PaymentOperationNullDestination()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");

            Asset asset = new StellarBase.Asset();
            long amount = 1000;
            
            var ex = Assert.Throws<NullReferenceException>(() => new PaymentOperation.Builder(null, asset, amount)
                .SetSourceAccount(source)
                .Build());
            Assert.Equal(ex.Message, "destination cannot be null.");
        }

        [Fact]
        public void PaymentOperationNullAsset()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            // GDW6AUTBXTOC7FIKUO5BOO3OGLK4SF7ZPOBLMQHMZDI45J2Z6VXRB5NR
            KeyPair destination = KeyPair.FromSeed("SDHZGHURAYXKU2KMVHPOXI6JG2Q4BSQUQCEOY72O3QQTCLR2T455PMII");
            
            long amount = 1000;

            var ex = Assert.Throws<NullReferenceException>(() => new PaymentOperation.Builder(destination, null, amount)
                .SetSourceAccount(source)
                .Build());
            Assert.Equal(ex.Message, "asset cannot be null.");
        }

        [Fact]
        public void PaymentOperationNegativeAmount()
        {
            // GC5SIC4E3V56VOHJ3OZAX5SJDTWY52JYI2AFK6PUGSXFVRJQYQXXZBZF
            KeyPair source = KeyPair.FromSeed("SC4CGETADVYTCR5HEAVZRB3DZQY5Y4J7RFNJTRA6ESMHIPEZUSTE2QDK");
            // GDW6AUTBXTOC7FIKUO5BOO3OGLK4SF7ZPOBLMQHMZDI45J2Z6VXRB5NR
            KeyPair destination = KeyPair.FromSeed("SDHZGHURAYXKU2KMVHPOXI6JG2Q4BSQUQCEOY72O3QQTCLR2T455PMII");

            Asset asset = new StellarBase.Asset();

            var ex = Assert.Throws<ArgumentException>(() => new PaymentOperation.Builder(destination, asset, -1)
                .SetSourceAccount(source)
                .Build());
            Assert.Equal(ex.Message, "amount must be non-negative.");
        }

        [Fact]
        public void PaymentOperationNullSource()
        {
            // GDW6AUTBXTOC7FIKUO5BOO3OGLK4SF7ZPOBLMQHMZDI45J2Z6VXRB5NR
            KeyPair destination = KeyPair.FromSeed("SDHZGHURAYXKU2KMVHPOXI6JG2Q4BSQUQCEOY72O3QQTCLR2T455PMII");

            Asset asset = new StellarBase.Asset();
            long amount = 1000;
            
            var ex = Assert.Throws<NullReferenceException>(() => new PaymentOperation.Builder(destination, asset, amount)
                .SetSourceAccount(null)
                .Build());
            Assert.Equal(ex.Message, "sourceAccount cannot be null.");
        }
    }
}
