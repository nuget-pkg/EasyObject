using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
#if USE_SPECTRE_CONSOLE
using Spectre.Console;
using Spectre.Console.Json;
using Spectre.Console.Rendering;
#endif

// ReSharper disable InconsistentNaming
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable once CheckNamespace
namespace Global;

#if MINIMAL
using EasyObjectType = MiniEasyObjectType;
using EasyObjectConverter = MiniEasyObjectConverter;
using EasyObject = MiniEasyObject;
using EasyObjectEditor = MiniEasyObjectEditor;
#endif

#if !MINIMAL
public class EasyConsole //: IAnsiConsole
{
    public EasyConsole(TextWriter writer)
    {
        _writer = writer;
        _ansiConsole = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer)
        });
    }

    public TextWriter _writer { get; set; }
    public IAnsiConsole _ansiConsole { get; }

    public static string MarkupSafeString(string str)
    {
#if USE_SPECTRE_CONSOLE
        return Markup.Escape(str);
#else
        return str;
#endif
    }

    // public void Clear(bool home)
    // {
    //     _ansiConsole.Clear(home);
    // }
    //
    public void Write(IRenderable renderable)
    {
        _ansiConsole.Write(renderable);
    }

    // public void WriteLine(IRenderable renderable)
    // {
    //     _ansiConsole.WriteLine(renderable);
    // }

    public void Render(string s)
    {
#if !USE_SPECTRE_CONSOLE
        this._writer.WriteLine(s);
#else
        if (!s.StartsWith("⁅markup⁆"))
        {
            _ansiConsole.Write(s);
        }
        else
        {
            s = s.Replace("⁅markup⁆", "");
            _ansiConsole.Markup(s);
        }
#endif
    }

    public void RenderLine(string s = "")
    {
#if !USE_SPECTRE_CONSOLE
        this._writer.Write(s);
#else
        if (!s.StartsWith("⁅markup⁆"))
        {
            _ansiConsole.WriteLine(s);
        }
        else
        {
            s = s.Replace("⁅markup⁆", "");
            _ansiConsole.MarkupLine(s);
        }
#endif
    }

    public bool IsMarkupString(string str)
    {
        return str.StartsWith("⁅markup⁆");
    }

    public void Write(string s)
    {
        _writer.Write(s);
    }

    public void WriteLine(string s)
    {
        _writer.WriteLine(s);
    }
}
#endif

#if MINIMAL
public enum MiniEasyObjectType
#else
public enum EasyObjectType
#endif
{
    @string,
    number,
    boolean,
    @object,
    array,
    @null
}

#if MINIMAL
internal class MiniEasyObjectConverter : IConvertParsedResult
#else
internal class EasyObjectConverter : IConvertParsedResult
#endif
{
    public object? ConvertParsedResult(object? x, string origTypeName)
    {
        if (x is Dictionary<string, object>)
        {
            var dict = x as Dictionary<string, object>;
            var keys = dict!.Keys;
            var result = new Dictionary<string, EasyObject>();
            foreach (var key in keys)
            {
                var eo = new EasyObject();
                eo.RealData = dict[key];
                result[key] = eo;
            }

            return result;
        }

        if (x is List<object>)
        {
            var list = x as List<object>;
            var result = new List<EasyObject>();
            foreach (var e in list!)
            {
                var eo = new EasyObject();
                eo.RealData = e;
                result.Add(eo);
            }

            return result;
        }

        return x;
    }
}

#if MINIMAL
public class MiniEasyObject :
#else
public class
    EasyObject : // !! NOW THAT Global.EasyObject is `partial`; YOU CAN DEFINE EXTENDED METHODs LOCALLY !! (Since 2026/03/29 17:40 +09:00)
#endif
    DynamicObject,
    IExposeInternalObject,
    IExportToPlainObject,
    IImportFromPlainObject,
    IExportToCommonJson,
    IImportFromCommonJson
{
    public object? RealData /*= null*/;
    public static readonly IParseJson DefaultJsonParser = new CSharpEasyLanguageHandler(true);
    public static IParseJson? JsonParser /*= null*/;
    public static bool DebugOutput /*= false*/;
    public static bool ShowDetail /*= false*/;
    public static bool ForceAscii /*= false*/;
    public static bool UseAnsiConsole /*= false*/;
#if USE_SPECTRE_CONSOLE
    public static EasyConsole AnsiOutput;
    public static EasyConsole AnsiError;
#endif
    public static bool ShowLineNumbers = true; /* Introduced @ 2026-03-26 17:02 */
#if MINIMAL
    static MiniEasyObject()
#else
    static EasyObject()
#endif
    {
        ClearSettings();
#if USE_SPECTRE_CONSOLE
        AnsiOutput = new EasyConsole(Console.Out);
        AnsiError = new EasyConsole(Console.Error);
#endif
    }

    public static void ClearSettings()
    {
        JsonParser = DefaultJsonParser;
        DebugOutput = false;
        ShowDetail = false;
        ForceAscii = false;
        UseAnsiConsole = false;
        ShowLineNumbers = true;
    }

    public static void SetupConsoleEncoding(Encoding? encoding = null)
    {
        if (encoding == null) encoding = Encoding.UTF8;

        try
        {
            Console.OutputEncoding = encoding;
            Console.InputEncoding = encoding;
            Console.SetError(
                new StreamWriter(
                    Console.OpenStandardError(), encoding)
                {
                    AutoFlush = true
                });
#if USE_SPECTRE_CONSOLE
            AnsiOutput = new EasyConsole(Console.Out);
            AnsiError = new EasyConsole(Console.Error);
#endif
        }
        catch (Exception)
        {
            // Ignore exceptions related to console encoding
        }
    }

    private static void _EnsureCursorLeft()
    {
        try
        {
            Console.CursorLeft = 0;
        }
        catch
        {
        }
    }

#if MINIMAL
    public MiniEasyObject()
#else
    public EasyObject()
#endif
    {
        RealData = null;
    }

#if MINIMAL
    public MiniEasyObject(object? x)
#else
    public EasyObject(object? x)
#endif
    {
        RealData = new PlainObjectConverter(JsonParser, false,
            new EasyObjectConverter()).Parse(x, true);
    }

    public dynamic Dynamic => this;

    public override string ToString()
    {
        return ToPrintable();
    }

    public string ToPrintable(bool compact = false, uint maxDepth = 0, bool removeSurrogatePair = false)
    {
        return ToPrintable(this, compact: compact, maxDepth: maxDepth,
            removeSurrogatePair: removeSurrogatePair);
    }

    public static EasyObject Nil => new();

    public static EasyObject Null => new();

    public static EasyObject EmptyArray => new(new List<EasyObject>());

    public static EasyObject EmptyObject => new(new Dictionary<string, EasyObject>());

    public static EasyObject NewArray(params object?[] args)
    {
        var result = EmptyArray;
        for (var i = 0; i < args.Length; i++) result.Add(FromObject(args[i]));

        return result;
    }

    public static EasyObject NewObject(params object?[] args)
    {
        if (args.Length % 2 != 0) throw new ArgumentException("EasyObject.NewObject() requires even number arguments");
        var result = EmptyObject;
        for (var i = 0; i < args.Length; i += 2)
        {
            var key = args[i];
            if (key == null) continue;
            var keyString = key.ToString();
            if (keyString == null) continue;
            result.Add(keyString, FromObject(args[i + 1]));
        }

        return result;
    }

    public static EasyObjectType @string => EasyObjectType.@string;

    public static EasyObjectType boolean => EasyObjectType.boolean;

    public static EasyObjectType @object => EasyObjectType.@object;

    public static EasyObjectType array => EasyObjectType.array;

    public static EasyObjectType @null => EasyObjectType.@null;

    public bool IsString => TypeValue == EasyObjectType.@string;

    public bool IsNumber => TypeValue == EasyObjectType.number;

    public bool IsBoolean => TypeValue == EasyObjectType.boolean;

    public bool IsObject => TypeValue == EasyObjectType.@object;

    public bool IsArray => TypeValue == EasyObjectType.array;

    public bool IsNull => TypeValue == EasyObjectType.@null;

    private static object? ExposeInternalObjectHelper(object? x)
    {
        while (x is EasyObject) x = ((EasyObject)x).RealData;

        return x;
    }

    private static EasyObject WrapInternal(object? x)
    {
        if (x is EasyObject) return (x as EasyObject)!;

        return new EasyObject(x);
    }

    public object? ExposeInternalObject()
    {
        return ExposeInternalObjectHelper(this);
    }

    public EasyObjectType TypeValue
    {
        get
        {
            var obj = ExposeInternalObjectHelper(this);
            if (obj == null) return EasyObjectType.@null;

            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Boolean:
                    return EasyObjectType.boolean;
                case TypeCode.String:
                case TypeCode.Char:
                case TypeCode.DateTime:
                    return EasyObjectType.@string;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return EasyObjectType.number;
                case TypeCode.Object:
                    return obj is List<EasyObject> ? EasyObjectType.array : EasyObjectType.@object;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    if (obj is TimeSpan || obj is Guid) return @string;

                    return EasyObjectType.@null;
            }
        }
    }

    public string TypeName => TypeValue.ToString();

    internal List<EasyObject>? list => RealData as List<EasyObject>;

    internal Dictionary<string, EasyObject>? dictionary => RealData as Dictionary<string, EasyObject>;

    public int Count
    {
        get
        {
            if (list != null) return list.Count;

            if (dictionary != null) return dictionary.Count;

            return 0;
        }
    }

    public List<string> Keys
    {
        get
        {
            var keys = new List<string>();
            if (dictionary == null) return keys;

            foreach (var key in dictionary.Keys) keys.Add(key);

            return keys;
        }
    }

    public bool ContainsKey(string name)
    {
        if (dictionary == null) return false;

        return dictionary.ContainsKey(name);
    }

    public EasyObject Add(object? x)
    {
        if (list == null) RealData = new List<EasyObject>();

        var eo = x is EasyObject ? (x as EasyObject)! : new EasyObject(x);
        list!.Add(eo);
        return this;
    }

    public EasyObject Add(string key, object? x)
    {
        if (dictionary == null) RealData = new Dictionary<string, EasyObject>();

        var eo = x is EasyObject ? (x as EasyObject)! : new EasyObject(x);
        dictionary!.Add(key, eo);
        return this;
    }

    public override bool TryGetMember(
        GetMemberBinder binder, out object result)
    {
        result = Null;
        var name = binder.Name;
        if (list != null)
        {
            var assoc = TryAssoc(name);
            result = assoc;
        }

        if (dictionary == null) return true;

        EasyObject? eo;
        dictionary.TryGetValue(name, out eo);
        if (eo == null) eo = Null;

        result = eo;
        return true;
    }

    public override bool TrySetMember(
        SetMemberBinder binder, object? value)
    {
        value = ExposeInternalObjectHelper(value);
        if (dictionary == null) RealData = new Dictionary<string, EasyObject>();

        var name = binder.Name;
        dictionary![name] = WrapInternal(value);
        return true;
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
        result = Null;
        var idx = indexes[0];
        if (idx is int)
        {
            var pos = (int)indexes[0];
            if (list == null)
            {
                result = WrapInternal(null);
                return true;
            }

            if (list.Count < pos + 1)
            {
                result = WrapInternal(null);
                return true;
            }

            result = WrapInternal(list[pos]);
            return true;
        }

        if (list != null)
        {
            var assoc = TryAssoc((string)idx);
            result = assoc;
        }

        if (dictionary == null)
        {
            result = Null;
            return true;
        }

        EasyObject? eo /*= Null*/;
        dictionary.TryGetValue((string)idx, out eo);
        if (eo == null) eo = Null;

        result = eo;
        return true;
    }

    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
    {
        if (value is EasyObject) value = ((EasyObject)value).RealData;

        var idx = indexes[0];
        if (idx is int)
        {
            var pos = (int)indexes[0];
            if (pos < 0) throw new ArgumentException("index is below 0");

            if (list == null) RealData = new List<EasyObject>();

            while (list!.Count < pos + 1) list.Add(Null);

            list[pos] = WrapInternal(value);
            return true;
        }

        if (dictionary == null) RealData = new Dictionary<string, EasyObject>();

        var name = (string)indexes[0];
        dictionary![name] = WrapInternal(value);
        return true;
    }

    public override bool TryConvert(ConvertBinder binder, out object? result)
    {
        if (binder.Type == typeof(IEnumerable))
        {
            if (list != null)
            {
                var ie1 = list.Select(x => x);
                result = ie1;
                return true;
            }

            if (dictionary != null)
            {
                var ie2 = dictionary.Select(x => x);
                result = ie2;
                return true;
            }

            result = new List<EasyObject>().Select(x => x);
            return true;
        }

        result = Convert.ChangeType(RealData, binder.Type);
        return true;
    }

    public static EasyObject FromObject(object? obj, bool ignoreErrors = false)
    {
        if (!ignoreErrors) return new EasyObject(obj);

        try
        {
            return new EasyObject(obj);
        }
        catch (Exception)
        {
            return new EasyObject(null);
        }
    }

    public static EasyObject FromJson(string? json, bool ignoreErrors = false)
    {
        if (json == null) return Null;

        if (!ignoreErrors) return new EasyObject(JsonParser!.ParseJson(json));

        try
        {
            return new EasyObject(JsonParser!.ParseJson(json));
        }
        catch (Exception)
        {
            return new EasyObject(null);
        }
    }

    public static EasyObject FromFile(string path, bool ignoreErrors = false)
    {
        return FromJson(File.ReadAllText(path), ignoreErrors);
    }

    public static string? Utf8StringFromUrl(string url)
    {
#pragma warning disable SYSLIB0014
        var request = WebRequest.Create(url) as HttpWebRequest;
#pragma warning restore SYSLIB0014
        var response = (HttpWebResponse)request!.GetResponse();
        //WebHeaderCollection header = response.Headers;
        var respStream = response.GetResponseStream();
        if (respStream == null) return null;
        using (var reader = new StreamReader(respStream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }

    public void InjectToFile(
        string path,
        bool indent = false,
        bool sortKeys = false,
        bool keyAsSymbol = false,
        bool removeSurrogatePair = false
    )
    {
        var json = ToJson(indent, sortKeys, keyAsSymbol);
        EasyTextEmbedder.InjectEmbeddedText(path, json);
    }

    public static EasyObject ExtractFromFile(string pathOrUrl, bool ignoreErrors = false)
    {
        var json = EasyTextEmbedder.ExtractEmbeddedText(pathOrUrl) ?? "null";
        return FromJson(json, ignoreErrors);
    }

    private static List<string>? _FindFirstMatch(string s, params string[] patterns)
    {
        foreach (var pattern in patterns)
        {
            var r = new Regex(pattern);
            var m = r.Match(s);
            if (m.Success)
            {
                List<string> groups = [];
                for (var i = 0; i < m.Groups.Count; i++) groups.Add(m.Groups[i].Value);

                return groups;
            }
        }

        return null;
    }

    public static EasyObject FromUrl(string url, bool ignoreErrors = false)
    {
        var m = _FindFirstMatch(
            url,
            @"^(https://github[.]com/[^/]+/[^/]+/)blob(/.+)$",
            @"^(https://gitlab[.]com/nuget-tools/nuget-assets/-/)blob(/.+)$"
        );
        if (m != null) url = m[1] + "raw" + m[2];

        if (!ignoreErrors)
        {
            var json = Utf8StringFromUrl(url);
            return FromJson(json, ignoreErrors);
        }

        try
        {
            var json = Utf8StringFromUrl(url);
            return FromJson(json, ignoreErrors);
        }
        catch (Exception)
        {
            return new EasyObject(null);
        }
    }

    public dynamic? ToObject(bool asDynamicObject = false)
    {
        if (asDynamicObject) return ExportToDynamicObject();

        return ExportToPlainObject();
    }

    public string ToJson(bool indent = false, bool sortKeys = false, bool keyAsSymbol = false,
        bool removeSurrogatePair = false)
    {
        var poc = new PlainObjectConverter(JsonParser, ForceAscii);
        return poc.Stringify(RealData, indent, sortKeys, keyAsSymbol,
            removeSurrogatePair);
    }
#if USE_WINCONSOLE
    public static bool HasConsole()
    {
        try
        {
            // Attempt to get a console property
            int left = Console.CursorLeft;
            return true;
        }
        catch (IOException)
        {
            // If an exception is caught, no console is available
            return false;
        }
    }
    public static void AllocConsole()
    {
        if (IsConsoleApplication) return;
        WinConsole.Alloc();
    }
    public static void FreeConsole()
    {
        if (IsConsoleApplication) return;
        WinConsole.Free();
    }
    public static void ReallocConsole()
    {
        if (IsConsoleApplication) return;
        FreeConsole();
        AllocConsole();
    }
#endif
    public static string ToPrintable(object? x, string? title = null, bool compact = false, uint maxDepth = 0,
        bool removeSurrogatePair = false)
    {
        var poc = new PlainObjectConverter(JsonParser, ForceAscii);
        if (maxDepth != 0) x = FromObject(x).Clone(maxDepth, always: false);

        var printable = poc.ToPrintable(ShowDetail, x, title, compact,
            removeSurrogatePair);
        return printable;
    }

    public static string MarkupSafeString(string str)
    {
#if USE_SPECTRE_CONSOLE
        return Markup.Escape(str);
#else
        return str;
#endif
    }

    public static void Write(
        string str,
        string? title = null
    )
    {
#if USE_SPECTRE_CONSOLE
        if (title != null) AnsiOutput.Render($"{title}: ");

        AnsiOutput.Render(str);
#else
        if (title != null) Console.Write($"{title}: ");

        Console.Write(str);
#endif
    }

    public static void WriteLine(
        string str = "",
        string? title = null
    )
    {
#if USE_SPECTRE_CONSOLE
        if (title != null) AnsiOutput.Render($"{title}: ");

        AnsiOutput.RenderLine(str);
#else
        if (title != null) Console.Write($"{title}: ");

        Console.WriteLine(str);
#endif
    }

    public static void Echo(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
    )
    {
        _EnsureCursorLeft();
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0)
        {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth,
                hideKeys,
                false);
        }
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole)
        {
            if (title != null) AnsiOutput.Render($"{title}: ");

            if (x != null && x is string str)
                if (AnsiOutput.IsMarkupString(str))
                {
                    AnsiOutput.RenderLine(str);
                    return;
                }

            var s2 = ToPrintable(x, null, compact, maxDepth,
                removeSurrogatePair);
            AnsiOutput.WriteLine(s2);
            return;
        }
#endif
        var s = ToPrintable(x, title, compact, maxDepth,
            removeSurrogatePair);
        Console.WriteLine(s);
    }

    public static void Log(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
    )
    {
        _EnsureCursorLeft();
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0)
        {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth,
                hideKeys,
                false);
        }
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole)
        {
            AnsiError.Render("⁅markup⁆[cyan][[Log]][/] ");
            if (title != null) AnsiError.Render($"{title}: ");

            if (x != null && x is string str)
                if (AnsiError.IsMarkupString(str))
                {
                    AnsiError.RenderLine(str);
                    if (ShowLineNumbers)
                        AnsiError.RenderLine($"      [blue]{MarkupSafeString(CurrentSourceCodeLine())}[/]");
                    return;
                }

            var s2 = ToPrintable(x, null, compact, maxDepth,
                removeSurrogatePair);
            //var s3 = MarkupSafeString(s2);
            AnsiError.WriteLine(s2);
            if (ShowLineNumbers)
                AnsiError.RenderLine($"⁅markup⁆      [blue]{MarkupSafeString(CurrentSourceCodeLine())}[/]");

            return;
        }
#endif
        var s = ToPrintable(x, title, compact, maxDepth,
            removeSurrogatePair);
        Console.Error.WriteLine("[Log] " + s);
        if (ShowLineNumbers) Console.Error.WriteLine($"      {CurrentSourceCodeLine()}");
    }

    public static void Debug(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
    )
    {
        if (!DebugOutput) return;

        _EnsureCursorLeft();
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0)
        {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth,
                hideKeys,
                false);
        }
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole)
        {
            AnsiError.Render("⁅markup⁆[purple][[Debug]][/] ");
            if (title != null) AnsiError.Render($"⁅markup⁆[purple]{MarkupSafeString(title)}:[/] ");
            var s2 = ToPrintable(x, null, compact, maxDepth,
                removeSurrogatePair);
            var s3 = MarkupSafeString(s2);
            AnsiError.RenderLine($"⁅markup⁆[purple]{s3}[/]");
            AnsiError.RenderLine($"⁅markup⁆        [purple]{MarkupSafeString(CurrentSourceCodeLine())}[/]");
            return;
        }
#endif
        var s = ToPrintable(x, title, compact, maxDepth,
            removeSurrogatePair);
        Console.Error.WriteLine("[Debug] " + s);
        Console.Error.WriteLine($"  {CurrentSourceCodeLine()}");
    }

    public static void Message(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null
    )
    {
        if (title == null) title = "Message";

        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0)
        {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth,
                hideKeys,
                false);
        }

        var s = ToPrintable(x, null, compact);
        NativeMethods.MessageBoxW(IntPtr.Zero, s, title, 0);
    }

    public static void DumpObject(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
    )
    {
        _EnsureCursorLeft();
        Echo(
            x,
            title,
            compact,
            maxDepth,
            hideKeys,
            removeSurrogatePair
        );
    }

    public void Dump(
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
    )
    {
        DumpObject(this, title, compact, maxDepth, hideKeys,
            removeSurrogatePair);
    }

    private static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBoxW(
            IntPtr hWnd, string lpText, string lpCaption, uint uType);
    }

    private EasyObject TryAssoc(string name)
    {
        try
        {
            if (list == null) return Null;

            for (var i = 0; i < list.Count; i++)
            {
                var pair = list[i].AsList!;
                if (pair[0].Cast<string>() == name) return pair[1];
            }

            return Null;
        }
        catch (Exception /*e*/)
        {
            return Null;
        }
    }

    public EasyObject this[string name]
    {
        get
        {
            if (list != null) return TryAssoc(name);

            if (dictionary == null) return Null;

            EasyObject? eo;
            dictionary.TryGetValue(name, out eo);
            if (eo == null) return Null;

            return eo;
        }
        set
        {
            if (dictionary == null) RealData = new Dictionary<string, EasyObject>();

            dictionary![name] = value;
        }
    }

    public EasyObject this[int pos]
    {
        get
        {
            if (list == null) return WrapInternal(null);

            if (list.Count < pos + 1) return WrapInternal(null);

            return WrapInternal(list[pos]);
        }
        set
        {
            if (pos < 0) throw new ArgumentException("index below 0");

            if (list == null) RealData = new List<EasyObject>();

            while (list!.Count < pos + 1) list.Add(Null);

            list[pos] = value;
        }
    }

    public T Cast<T>()
    {
        if (RealData is DateTime dt)
        {
            string? s = null;
            switch (dt.Kind)
            {
                case DateTimeKind.Local:
                    s = dt.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                    break;
                case DateTimeKind.Utc:
                    s = dt.ToString("o");
                    break;
                case DateTimeKind.Unspecified:
                    s = dt.ToString("o").Replace("Z", "");
                    break;
            }

            return (T)Convert.ChangeType(s, typeof(T))!;
        }

        return (T)Convert.ChangeType(RealData, typeof(T))!;
    }

    public List<EasyObject>? AsList => list;

    public Dictionary<string, EasyObject>? AsDictionary => dictionary;

    public static string FullName(dynamic? x)
    {
        if (x is null) return "null";

        var fullName = ((object)x).GetType().FullName!;
        if (fullName.StartsWith("<>f__AnonymousType")) return "AnonymousType";

        return fullName.Split('`')[0];
    }

    public static implicit operator EasyObject(bool x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(string x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(char x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(short x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(int x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(long x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(ushort x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(uint x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(ulong x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(float x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(double x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(decimal x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(sbyte x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(byte x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(DateTime x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(TimeSpan x)
    {
        return new EasyObject(x);
    }

    public static implicit operator EasyObject(Guid x)
    {
        return new EasyObject(x);
    }

    public void Nullify()
    {
        RealData = null;
    }

    public void Trim(
        uint maxDepth = 0,
        List<string>? hideKeys = null
    )
    {
        EasyObjectEditor.Trim(this, maxDepth, hideKeys);
    }

    public EasyObject Clone(
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool always = true
    )
    {
        return EasyObjectEditor.Clone(this, maxDepth, hideKeys, always);
    }

    public EasyObject? Shift()
    {
        if (list == null) return null;

        if (list.Count == 0) return null;

        var result = list[0];
        list.RemoveAt(0);
        return result;
    }

    public EasyObject Shuffle()
    {
        if (list != null)
        {
            var list2 = list!.Select(i => i).OrderBy(_ => Guid.NewGuid()).ToList();
            return FromObject(list2);
        }

        if (dictionary != null)
        {
            var keys = dictionary.Keys.Select(i => i).OrderBy(_ => Guid.NewGuid()).ToList();
            var result = NewObject();
            foreach (var key in keys) result[key] = dictionary[key];

            return result;
        }

        return Clone();
    }

    private static readonly Random Rnd = new();

    public static int PickRandomItem<T>(List<T> list)
    {
        if (list.Count == 0) return -1;

        var index = Rnd.Next(list.Count);
        return index;
    }

    public EasyObject Pick(int n)
    {
        var result = NewArray();
        if (list != null)
        {
            var cloneList =
                AsList!.Select(i => i)
                    .ToList(); // !! SHALLOW COPY...IN ORDER TO RETURN ORIGINAL ELEMTNS AS PICKS !!
            if (n > list.Count) n = list.Count;
            for (var i = 0; i < n; i++)
            {
                var pick = PickRandomItem(cloneList);
                result.Add(cloneList[pick]);
                cloneList.RemoveAt(pick);
            }
        }

        if (dictionary != null)
        {
            var keys = Keys;
            if (n > keys.Count) n = keys.Count;
            for (var i = 0; i < n; i++)
            {
                var pick = PickRandomItem(keys);
                result.Add(keys[pick]);
                keys.RemoveAt(pick);
            }
        }

        return FromObject(result);
    }

    public EasyObject Reverse()
    {
        if (list != null)
        {
            var list2 = list!.AsEnumerable().Reverse().Take(5).ToList();
            return FromObject(list2);
        }

        if (dictionary != null)
        {
            var keys = dictionary.Keys.AsEnumerable().Reverse().Take(5).ToList();
            var result = NewObject();
            foreach (var key in keys) result[key] = dictionary[key];

            return result;
        }

        return Clone();
    }

    public EasyObject Skip(int n)
    {
        if (list != null)
        {
            var list2 = list!.Select(i => i).Skip(n).ToList();
            return FromObject(list2);
        }

        if (dictionary != null)
        {
            var keys = dictionary.Keys.Select(i => i).Skip(n).ToList();
            var result = NewObject();
            foreach (var key in keys) result[key] = dictionary[key];

            return result;
        }

        return Clone();
    }

    public EasyObject Take(int n)
    {
        if (list != null)
        {
            var list2 = list!.Select(i => i).Take(n).ToList();
            return FromObject(list2);
        }

        if (dictionary != null)
        {
            var keys = dictionary.Keys.Select(i => i).Take(n).ToList();
            var result = NewObject();
            foreach (var key in keys) result[key] = dictionary[key];

            return result;
        }

        return Clone();
    }

    public string[] AsStringArray
    {
        get
        {
            if (list != null)
                return
                    list!
                        .Select(i =>
                            i.IsString ? i.Cast<string>() : i.ToJson(keyAsSymbol: true, indent: false))
                        .ToArray();

            if (dictionary != null) return dictionary.Keys.Select(i => i).ToArray();

            return [];
        }
    }

    public List<string> AsStringList => AsStringArray.ToList();

    public void ImportFromPlainObject(object? x)
    {
        var eo = FromObject(x);
        RealData = eo.RealData;
    }

    public void ImportFromCommonJson(string x)
    {
        var eo = FromJson(x);
        RealData = eo.RealData;
    }

    public string ExportToCommonJson()
    {
        return ToJson(
            true
        );
    }

    public object? ExportToPlainObject()
    {
        return new PlainObjectConverter(null, ForceAscii).Parse(RealData);
    }

    public dynamic? ExportToDynamicObject()
    {
        return EasyObjectEditor.ExportToExpandoObject(this);
    }

    public static string ObjectToJson(object? x, bool indent = false)
    {
        return FromObject(x).ToJson(indent);
    }

    public static object? ObjectToObject(object? x, bool asDynamicObject = false)
    {
        return FromObject(x).ToObject(asDynamicObject);
    }

    public static string ToClickableUri(string pathOrUrl)
    {
        if (pathOrUrl.StartsWith("http:") || pathOrUrl.StartsWith("https:") || pathOrUrl.StartsWith("file:"))
            return pathOrUrl;

        var filePath = pathOrUrl;
        filePath = Path.GetFullPath(filePath);
        return new Uri(filePath).AbsoluteUri;
    }

    public static void LogWebLink(string title, string url)
    {
        url = ToClickableUri(url);
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole)
            Log(
                $"⁅markup⁆[green][link={url}]{MarkupSafeString(title)}[/][/] => [blue]{MarkupSafeString(url)}[/]");
        else
            Log($"{MarkupSafeString(title)} => {MarkupSafeString(url)}");
#else
        Log($"{title} => {url}");
#endif
    }

    public static void EchoWebLink(string title, string url)
    {
        url = ToClickableUri(url);
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole)
            Echo(
                $"⁅markup⁆[green][link={url}]{MarkupSafeString(title)}[/][/] => [blue]{MarkupSafeString(url)}[/]");
        else
            Echo($"{MarkupSafeString(title)} => {MarkupSafeString(url)}");
#else
        Echo($"{title} => {url}");
#endif
    }

    private static List<string> TextToLines(string text)
    {
        List<string> lines = [];
        using (var sr = new StringReader(text))
        {
            string? line;
            while ((line = sr.ReadLine()) != null) lines.Add(line);
        }

        return lines;
    }

    public static string CurrentSourceCodeLine()
    {
        var trace = Environment.StackTrace;
        var lines = TextToLines(trace);
        if (lines.Count == 0) return "[!! UNKNOWN SOURCE CODE LINE !!]";
        return ReplacePathsWithUrls(lines[lines.Count - 1]).Trim();
    }

    public static string ReplacePathsWithUrls(string stackTrace)
    {
#if true
        //return stackTrace;
#endif
        // https://shorturl.ly/FToES C# search through string like stack trace for source code path and replace all of them to a "file:" urls - Google
        stackTrace = stackTrace.Replace("場所 ", "in ");
        stackTrace = stackTrace.Replace(":行 ", ":line ");
        // This regex looks for common file path patterns, especially those with drive letters (C:\)
        // or starting with a slash (/) often followed by common extensions like .cs, .vb, etc., within the context of a stack trace line.
        // The pattern aims to capture the full file path including extension and line number info if present.
        // Group 1 captures the path part for replacement.
        var filePathRegex =
            new Regex(@"(?:in\s+)(?<path>[a-zA-Z]:\\(?:[^<>:""/\\|?*]+\\)*[^<>:""/\\|?*]+):line\s+(?<line_num>\d+)$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
        // Use a MatchEvaluator delegate for the replacement to apply the Uri conversion logic to each match.
        var result = filePathRegex.Replace(stackTrace, match =>
        {
            var filePath = match.Groups["path"].Value;
            //Console.WriteLine($"filePath={filePath}");
            var line_num = match.Groups["line_num"].Value;
            //Console.WriteLine($"line_num(1)={line_num}");
            line_num = line_num.Replace(":line ", "");
            //Console.WriteLine($"line_num(2)={line_num}");
            try
            {
                // The System.Uri constructor handles the specific formatting requirements for file URIs,
                // including correct handling of slashes and special characters like spaces.
                var fileUri = new Uri(filePath);
                // We use AbsoluteUri which correctly formats the scheme (file://) and path for a URL.
                //Console.WriteLine($"line_num(3)={line_num}");
                var result = $"in {fileUri.AbsoluteUri} : line {line_num}";
                //Console.WriteLine($"result={result}");
                return result;
            }
            catch (UriFormatException)
            {
                // Fallback for paths that the Uri class might not handle correctly (e.g., highly unusual formats)
                return match.Value;
            }
        });
        result = result.Replace("   in ", "   at ");
        return result;
    }

    public static void Abort(object? message = null, int exitCode = 1)
    {
        ShowDetail = false;
        ShowLineNumbers = false;
        UseAnsiConsole = true;
        Log("⁅markup⁆[red][[!! ABORTING PROGRAM !!]][/]");
        UseAnsiConsole = false;
        if (message != null && !(message is Exception)) Log(message, "MESSAGE (FOR ABORTING PROGRAM)");

        if (message is Exception e)
        {
            var exTrace = e.ToString();
            try
            {
                exTrace = ReplacePathsWithUrls(exTrace);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }

            Console.Error.WriteLine(
                exTrace
            );
            UseAnsiConsole = true;
            Log($"⁅markup⁆[red][[!! ABORTING...WITH EXIT CODE {exitCode} !!]][/]");
            Environment.Exit(exitCode);
        }

        var trace = Environment.StackTrace;
        var lines = TextToLines(trace);
        lines = lines.Skip(2).ToList();
        trace = "\n" + string.Join("\n", lines);
        trace = ReplacePathsWithUrls(trace);
        Log(trace, "STACK TRACE");
        UseAnsiConsole = true;
        Log($"⁅markup⁆[red][[!! ABORTING...WITH EXIT CODE {exitCode} !!]][/]");
        Environment.Exit(exitCode);
    }

    private static void _AbortOnAssertionFailure(Exception ex, object? hint, int exitCode)
    {
        ShowDetail = false;
        ShowLineNumbers = false;
        UseAnsiConsole = true;
        Log("⁅markup⁆[red][[!! ASSERTION FAILED !!]][/]");
        Log($"⁅markup⁆[red]{MarkupSafeString(CurrentSourceCodeLine())}[/]");
        UseAnsiConsole = false;
        if (hint != null) Log(hint, "HINT MESSAGE (FOR THIS ASSERTION)");

        UseAnsiConsole = true;
        WriteLine(
            $"⁅markup⁆[blue]{MarkupSafeString(ReplacePathsWithUrls(ex.ToString()))}[/]",
            "⁅markup⁆[blue]EXCEPTION[/]");
        Log($"⁅markup⁆[red][[!! ABORTING...WITH EXIT CODE {exitCode} !!]][/]");
        Environment.Exit(exitCode);
    }

    public static void AssertTrue(bool condition, object? hint = null, int exitCode = 1)
    {
        try
        {
            Assert.IsTrue(condition, "");
        }
        catch (Exception ex)
        {
            _AbortOnAssertionFailure(ex, hint, exitCode);
        }
    }

    public static void AssertFalse(bool condition, object? hint = null, int exitCode = 1)
    {
        try
        {
            Assert.IsFalse(condition, "");
        }
        catch (Exception ex)
        {
            _AbortOnAssertionFailure(ex, hint, exitCode);
        }
    }

    public static void AssertEqual(object? expected, object? actual, object? hint = null, int exitCode = 1)
    {
        try
        {
            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            _AbortOnAssertionFailure(ex, hint, exitCode);
        }
    }

    public static void AssertNotEqual(object? expected, object? actual, object? hint = null, int exitCode = 1)
    {
        try
        {
            Assert.AreNotEqual(expected, actual);
        }
        catch (Exception ex)
        {
            _AbortOnAssertionFailure(ex, hint, exitCode);
        }
    }
}