# csharp-stellar-framework
Library to create, encode and decode Stellar transactions as well as to interact
with the Horizon REST Api.

This code bases on a fork of https://github.com/QuantozTechnology/csharp-stellar-base

# Dependencies
- .NET Standard 2.0

# Nuget
- https://www.nuget.org/packages/csharp-stellar-base/
- https://www.nuget.org/packages/csharp-stellar-sdk/

# Usage
- Open in project Visual Studio (tested in VS 2017)
- Build Solution

# Tests
- Run self tests using [TEST] -> Run -> All tests
	- or [Ctrl+R], A

# Generate code (LINUX commandline)
use ruby > 2.2.6 
run:
``` 
bundle
rake xdr:generate
```
