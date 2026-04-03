//css_nuget Microsoft.CodeAnalysis.CSharp
//css_nuget Microsoft.CodeAnalysis.CSharp.Workspaces
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System.Reflection;

// 1. Define your source code
string sourceCode = @"
    using Newtonsoft.Json;
    public class Evaluator {
        public string GetJson() => JsonConvert.SerializeObject(new { Greeting = ""Hello Roslyn"" });
    }";

// 2. Identify the path to the NuGet DLL (Must be accessible at runtime)
string jsonPath = @"C:\Users\Name\.nuget\packages\newtonsoft.json\13.0.1\lib\netstandard2.0\Newtonsoft.Json.dll";

// 3. Create the compilation
var compilation = CSharpCompilation.Create("DynamicAssembly")
    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
    .AddReferences(
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(jsonPath) // Adding the NuGet package reference
    )
    .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));

// 4. Emit to memory or file
using var ms = new MemoryStream();
var result = compilation.Emit(ms);

if (result.Success)
{
    ms.Seek(0, SeekOrigin.Begin);
    Assembly assembly = Assembly.Load(ms.ToArray());
    // Use reflection to run the code...
}
