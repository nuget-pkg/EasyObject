//css_nuget EasyObject
using Global;
using System;
using System.IO;
using System.Text;
using static Global.EasyObject;
using static Global.EasySystem;

SetupConsoleEncoding();

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding shiftJisEncoding = Encoding.GetEncoding("Shift_JIS");

#if CS_SCRIPT
Console.WriteLine("Running as a CS_SCRIPT...");
#endif

try
{
    ShowDetail = true;
    Log("ハロー©⁅記号2026-0331⁆❝❞↪️ ↩️ ℴ➺➢ ▸ ヾ➠▶🈂＼／：＊“≪≫￤；‘｀＃％＄＆＾～￤⁅⁆≪≫＋ー＊＝◉");
    Log(new { args });
    Break();
    RunToConsole(
        Encoding.UTF8,
        "git.exe",
        ["rev-parse", "--show-toplevel"]
    );
    SilentFlag = true;
    string output = GetProcessStdout(
        Encoding.UTF8,
        "git.exe",
        "rev-parse",
        "--show-toplevel"
    );
    Log(output, "output");
    string tmpFile = HomeFile("tmp", "output.txt");
    File.WriteAllText(tmpFile, output);
    string output2 = File.ReadAllText(tmpFile);
    Log(output2, "output2");
    string ls = GetProcessStdout(
        Encoding.UTF8,
        "my-ls.exe",
        //HomeFolder()
        @"P:\@porn++++"
    );
    var lines = TextToLines(ls);
    //Log(lines, "lines");
    foreach(var line in lines)
    {
        // var info = MediaInfo.ParseMediaUrl(line);
        // if (info != null)
        // {
        //     DumpObjectAsJson(info, compact: true, keyAsSymbol: true);
        // }
    }
    throw new NotImplementedException();
}
catch (Exception e)
{
    Abort(e);
}
