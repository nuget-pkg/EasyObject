#if !MINIMAL
using System.IO;
using Spectre.Console;
using Spectre.Console.Rendering;
using Universal;
// ReSharper disable All
namespace Global;
public class EasyConsole //: IAnsiConsole
{
    public EasyConsole(TextWriter writer) {
        _writer = writer;
        _ansiConsole = AnsiConsole.Create(new AnsiConsoleSettings {
            Out = new AnsiConsoleOutput(writer)
        });
    }
    public TextWriter _writer { get; set; }
    public IAnsiConsole _ansiConsole { get; }
    public static string MarkupSafeString(string str) {
#if USE_SPECTRE_CONSOLE
        return Markup.Escape(str);
#else
        return str;
#endif
    }
    public void Write(IRenderable renderable) {
        _ansiConsole.Write(renderable);
    }
    public void Render(string s) {
        if (!EasyObject.EmojiCompatibleEnvironment) {
            s = UniversalTransformer.ReplaceSurrogatePair(s, "❗");
        }
#if !USE_SPECTRE_CONSOLE
        this._writer.WriteLine(s);
#else
        if (!s.Contains("⁅markup⁆")) { /**/
            _ansiConsole.Write(s);
        }
        else {
            s = s.Replace("⁅markup⁆", "");
            _ansiConsole.Markup(s);
        }
#endif
    }
    public void RenderLine(string s = "") {
        if (!EasyObject.EmojiCompatibleEnvironment) {
            s = UniversalTransformer.ReplaceSurrogatePair(s, "❗");
        }
#if !USE_SPECTRE_CONSOLE
        this._writer.Write(s);
#else
        if (!s.Contains("⁅markup⁆")) {
            _ansiConsole.WriteLine(s);
        }
        else {
            s = s.Replace("⁅markup⁆", "");
            _ansiConsole.MarkupLine(s);
        }
#endif
    }
    public bool IsMarkupString(string str) {
        return str.Contains("⁅markup⁆");
    }
    public void Write(string s) {
        if (!EasyObject.EmojiCompatibleEnvironment) {
            s = UniversalTransformer.ReplaceSurrogatePair(s, "❗");
        }
        _writer.Write(s);
    }
    public void WriteLine(string s) {
        if (!EasyObject.EmojiCompatibleEnvironment) {
            s = UniversalTransformer.ReplaceSurrogatePair(s, "❗");
        }
        _writer.WriteLine(s);
    }
}
#endif