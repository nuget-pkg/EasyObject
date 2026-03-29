// ReSharper disable RedundantUsingDirective
namespace Misc;

using Global;
using static Global.EasyObject;
using static Global.EasySystem;

public class Misc01
{
    public static void Main(string[] args)
    {
        SetupConsoleEncoding();
        ShowLineNumbers = false;
        ShowDetail = true;
        UseAnsiConsole = true;
        DebugOutput = true;
        Log("⭕️ハロー©⭕️");
        EasySystem.RunToConsole("bash", ["-c", "ls -ltr"]);
    }
}