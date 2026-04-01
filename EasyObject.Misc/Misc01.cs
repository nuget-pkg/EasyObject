using System.Text;
namespace Misc;
using System;
using Global;
using static Global.MiniEasyObject;
using static Global.OpenSystem;
public class Misc01 {
    public static void Main(string[] args) {
        try {
            SetupConsoleEncoding();
            ShowLineNumbers = false;
            ShowDetail = true;
            UseAnsiConsole = true;
            DebugOutput = true;
            Log("⭕️ハロー©⭕️");
            OpenSystem.RunToConsole(Encoding.UTF8, "bash", ["-c", "ls -ltr"]);
            var newton = NewtonsoftJsonUtil.DeserializeFromJson("[11,null,33.15,[44,55], {'a': 123}]");
            Log(newton, title: "newton");
            Log(FromObject(newton));
            var xml01 = NewtonsoftJsonUtil.SerializeToToXml(new { a = new { x = 1, y = "xyz" } });
            Log(xml01);
            var xml01Eo = NewtonsoftJsonUtil.DeserializeFromXml(xml01);
            Log(xml01Eo);
            string? exe = FindExeRecursive(@"C:\Program Files\Vim", "gvim.exe");
            Log(exe);
        }
        catch (Exception ex) {
            Abort(ex);
        }
    }
}