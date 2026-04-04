#if MINIMAL
using EasyObject = Global.MiniEasyObject;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace Global;
#if MINIMAL
internal class MiniEasyObjectConverter : IConvertParsedResult
#else
internal class EasyObjectConverter : IConvertParsedResult
#endif
{
    public object? ConvertParsedResult(object? x, string origTypeName) {
        if (x is Dictionary<string, object> dict) {
            var keys = dict.Keys;
            var result = new Dictionary<string, EasyObject>();
            foreach (var key in keys) {
                var eo = new EasyObject {
                    RealData = dict[key]
                };
                result[key] = eo;
            }
            return result;
        }
        if (x is List<object> list) {
            var result = new List<EasyObject>();
            foreach (var e in list) {
                var eo = new EasyObject {
                    RealData = e
                };
                result.Add(eo);
            }
            return result;
        }
        return x;
    }
}
internal static class EasyObjectDetails {
    internal static void _ViewInFavoriteEditor(
        StackFrame? currFrame,
        bool wait = false
    ) {
        EasyObject.Message(new {
            method = "EasyObjectDetails#_ViewInFavoriteEditor()", wait,
            isWindoes = HyperOperatingSystem.IsWindowsPlatform(),
            isBoundCurrFrame = currFrame != null,
        });
        if (!HyperOperatingSystem.IsWindowsPlatform()) return;
        string? _filePath = null;
        string? _lineNumber = null;
        if (currFrame != null) {
            _filePath = currFrame.GetFileName();
            _lineNumber = currFrame.GetFileLineNumber().ToString();
        }
        EasyObject.Message(new {
            method = "EasyObjectDetails#_ViewInFavoriteEditor()",
            _filePath,
            _lineNumber,
        });
        if (_filePath != null && File.Exists(_filePath)) {
            void DelayForEditorStart(Process? p, int msec = 200) {
                if (p == null) return;
                try {
                    var isReady = p.WaitForInputIdle(5000);
                    if (isReady) {
                        p.Refresh(); // Refresh to ensure MainWindowHandle is updated
                        if (p.MainWindowHandle != IntPtr.Zero) Console.WriteLine("Window successfully opened.");
                    }
                }
                catch {
                    ;
                }
                HyperOperatingSystem.Sleep(msec);
            }
            if (_lineNumber == null) _lineNumber = "1";
            string? exe = null;
            Process? p /*= null*/;
            if (HyperOperatingSystem.GetEnv("I_HATE_VSCODE") != "1") {
                // [Visual Studio Code]
                exe = HyperOperatingSystem.FindExePath("code.cmd");
                if (exe != null) {
                    if (_lineNumber == null) {
                        if (wait)
                            p = HyperOperatingSystem.LaunchProcess(exe, ["--wait", _filePath]);
                        else
                            p = HyperOperatingSystem.LaunchProcess(exe, [_filePath]);
                        DelayForEditorStart(p);
                        if (p != null && wait) p.WaitForExit();
                        return;
                    }
                    if (wait)
                        p = HyperOperatingSystem.LaunchProcess(exe, [
                            "--wait", "-g", $"{_filePath}:{_lineNumber}"
                        ]);
                    else
                        p = HyperOperatingSystem.LaunchProcess(exe, ["-g", $"{_filePath}:{_lineNumber}"]);
                    DelayForEditorStart(p);
                    if (p != null && wait) p.WaitForExit();
                    return;
                }
            }
            if (HyperOperatingSystem.GetEnv("I_HATE_NOTEPAD_PP") != "1") {
                // [Notepad++]
                exe = HyperOperatingSystem.FindExePath("Notepad++.exe");
                if (exe != null) {
                    if (_lineNumber == null) {
                        p = HyperOperatingSystem.LaunchProcess(exe, [_filePath, "-n1"]);
                        DelayForEditorStart(p);
                        if (p != null && wait) p.WaitForExit();
                        return;
                    }
                    p = HyperOperatingSystem.LaunchProcess(exe, [_filePath, $"-n{_lineNumber}"]);
                    DelayForEditorStart(p);
                    if (p != null && wait) p.WaitForExit();
                    return;
                }
            }
            if (HyperOperatingSystem.GetEnv("I_HATE_EMACS") != "1") {
                exe = HyperOperatingSystem.FindExePath("emacsclient.exe");
                if (exe != null) {
                    if (wait) {
                        p = HyperOperatingSystem.LaunchProcess(exe,
                        [
                            "-nw", "-a", "\"\"", $"+{_lineNumber}", _filePath, "--eval" /*, "(recenter-top-bottom)"*/
                        ]);
                        DelayForEditorStart(p);
                        if (p != null) p.WaitForExit();
                        return;
                    }
                    p = HyperOperatingSystem.LaunchProcess(exe,
                    [
                        "-r", "-n", "-a", "\"\"", $"+{_lineNumber}",
                        _filePath /*, "--eval", "(recenter-top-bottom)"*/
                    ]);
                    DelayForEditorStart(p);
                    return;
                }
            }
            if (HyperOperatingSystem.GetEnv("I_HATE_ZED") != "1") {
                // [Zed Editor]
                exe = HyperOperatingSystem.FindExePath("Zed.exe");
                if (exe != null) {
                    if (_lineNumber == null) {
                        if (wait)
                            p = HyperOperatingSystem.LaunchProcess(exe, ["--wait", _filePath]);
                        else
                            p = HyperOperatingSystem.LaunchProcess(exe, [_filePath]);
                        DelayForEditorStart(p);
                        if (p != null && wait) p.WaitForExit();
                        return;
                    }
                    if (wait)
                        p = HyperOperatingSystem.LaunchProcess(exe, ["--wait", $"{_filePath}:{_lineNumber}"]);
                    else
                        p = HyperOperatingSystem.LaunchProcess(exe, [$"{_filePath}:{_lineNumber}"]);
                    DelayForEditorStart(p);
                    if (p != null && wait) p.WaitForExit();
                    return;
                }
            }
            if (HyperOperatingSystem.GetEnv("I_HATE_NOTEPAD_3") != "1") {
                // [Notepad3.exe]
                exe = HyperOperatingSystem.FindExePath("Notepad3.exe");
                if (exe != null) {
                    if (_lineNumber == null) {
                        p = HyperOperatingSystem.LaunchProcess(exe, ["/g", "1", _filePath]);
                        DelayForEditorStart(p);
                        if (p != null && wait) p.WaitForExit();
                        return;
                    }
                    p = HyperOperatingSystem.LaunchProcess(exe, ["/g", _lineNumber, _filePath]);
                    DelayForEditorStart(p);
                    if (p != null && wait) p.WaitForExit();
                    return;
                }
            }
            if (exe == null) {
                EasyObject.Log(
                    "⁅markup⁆[green]Emacs Edtor was not found in PATH; automatic source code viewing canelled![/]"
                );
                EasyObject.Log(
                    "⁅markup⁆[green]Zed Edtor was not found in PATH; automatic source code viewing canelled![/]"
                );
                EasyObject.Log(
                    "⁅markup⁆[green]Visual Studio Code (code.cmd) was not found in PATH; automatic source code viewing canelled![/]"
                );
                EasyObject.Log(
                    "⁅markup⁆[green]Notepad++.exe was not found in PATH; automatic source code viewing canelled![/]"
                );
                EasyObject.Log(
                    "⁅markup⁆[green]Notepad3.exe was not found in PATH; automatic source code viewing canelled![/]"
                );
            }
        }
    }
}