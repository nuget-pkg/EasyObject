//css_nuget csharp-stellar-sdk
//css_nuget EasyObject
using System;
using static Global.EasyObject;
using static Global.OpenSystem;

namespace HelloWorld;

/// <summary>
/// This is a basic sample of FasterKV using value types
/// </summary>
public class Program
{
    static void Main()
    {
        try
        {
            SetupConsoleEncoding();
            UseAnsiConsole = true;

            Log(GetCwd());

            Break("[END]");

        }
        catch (Exception ex)
        {
            Abort(ex);
        }
    }
}
