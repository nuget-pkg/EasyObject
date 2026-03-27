namespace Global;

#if USE_SPECTRE_CONSOLE
using Spectre.Console;
using Spectre.Console.Json;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
public enum EasyObjectType {
    @string,
    @number,
    @boolean,
    @object,
    @array,
    @null
}
internal class EasyObjectConverter : IConvertParsedResult {
    public object? ConvertParsedResult(object? x, string origTypeName) {
        if (x is Dictionary<string, object>) {
            var dict = x as Dictionary<string, object>;
            var keys = dict!.Keys;
            var result = new Dictionary<string, EasyObject>();
            foreach (var key in keys) {
                var eo = new EasyObject();
                eo.RealData = dict[key];
                result[key] = eo;
            }
            return result;
        } else if (x is List<object>) {
            var list = x as List<object>;
            var result = new List<EasyObject>();
            foreach (var e in list!) {
                var eo = new EasyObject();
                eo.RealData = e;
                result.Add(eo);
            }
            return result;
        }
        return x;
    }
}
public class EasyObject :
    DynamicObject,
    IExposeInternalObject,
    IExportToPlainObject,
    IImportFromPlainObject,
    IExportToCommonJson,
    IImportFromCommonJson {
    public object? RealData /*= null*/;
    public static readonly IParseJson DefaultJsonParser = new CSharpEasyLanguageHandler(numberAsDecimal: true);
    public static IParseJson? JsonParser /*= null*/;
    public static bool DebugOutput /*= false*/;
    public static bool ShowDetail /*= false*/;
    public static bool ForceAscii /*= false*/;
    public static bool UseAnsiConsole /*= false*/;
#if USE_SPECTRE_CONSOLE
    public static IAnsiConsole AnsiErrorConsole;
#endif
    public static bool ShowLineNumbers = true; /* Introduced @ 2026-03-26 17:02 */
    static EasyObject() {
        EasyObject.ClearSettings();
#if USE_SPECTRE_CONSOLE
        AnsiErrorConsole = AnsiConsole.Create(new AnsiConsoleSettings {
            Out = new AnsiConsoleOutput(Console.Error)
        });
#endif
    }
    public static void ClearSettings() {
        EasyObject.JsonParser = DefaultJsonParser;
        EasyObject.DebugOutput = false;
        EasyObject.ShowDetail = false;
        EasyObject.ForceAscii = false;
        EasyObject.UseAnsiConsole = false;
        EasyObject.ShowLineNumbers = true;
    }
    public static void SetupConsoleEncoding(Encoding? encoding = null) {
        if (encoding == null) {
            encoding = Encoding.UTF8;
        }
        try {
            Console.OutputEncoding = encoding;
            Console.InputEncoding = encoding;
            Console.SetError(
                new StreamWriter(
                    Console.OpenStandardError(), encoding) {
                    AutoFlush = true
                });
#if USE_SPECTRE_CONSOLE
            AnsiErrorConsole = AnsiConsole.Create(new AnsiConsoleSettings {
                Out = new AnsiConsoleOutput(Console.Error)
            });
#endif
            //EasySystem.ConsoleClearCurrentLine();
            //Console.CursorLeft = 0;
        } catch (Exception) {
            // Ignore exceptions related to console encoding
        }
    }
    private static void _EnsureCursorLeft() {
        try {
            if (UseAnsiConsole) {
                Console.CursorLeft = 0;
            }
        } catch {
        }
    }
    public EasyObject() {
        RealData = null;
    }
    public EasyObject(object? x) {
        RealData = new PlainObjectConverter(jsonParser: JsonParser, forceAscii: false, iConvertParsedResult: new EasyObjectConverter()).Parse(x, numberAsDecimal: true);
    }
    public dynamic Dynamic { get { return this; } }
    public override string ToString() {
        return ToPrintable();
    }
    public string ToPrintable(bool compact = false, uint maxDepth = 0, bool removeSurrogatePair = false) {
        return EasyObject.ToPrintable(this, compact: compact, maxDepth: maxDepth, removeSurrogatePair: removeSurrogatePair);
    }
    public static EasyObject Nil { get { return new EasyObject(); } }
    public static EasyObject Null { get { return new EasyObject(); } }
    public static EasyObject EmptyArray { get { return new EasyObject(new List<EasyObject>()); } }
    public static EasyObject EmptyObject { get { return new EasyObject(new Dictionary<string, EasyObject>()); } }
    public static EasyObject NewArray(params object?[] args) {
        EasyObject result = EmptyArray;
        for (int i = 0; i < args.Length; i++) {
            result.Add(FromObject(args[i]));
        }
        return result;
    }
    public static EasyObject NewObject(params object?[] args) {
        if ((args.Length % 2) != 0) {
            throw new ArgumentException("EasyObject.NewObject() requires even number arguments");
        }
        EasyObject result = EmptyObject;
        for (int i = 0; i < args.Length; i += 2) {
            if (args[i] == null) {
                continue;
            }
            result.Add(args[i]!.ToString()!, FromObject(args[i + 1]));
        }
        return result;
    }
    public static EasyObjectType @string { get { return EasyObjectType.@string; } }
    public static EasyObjectType @boolean { get { return EasyObjectType.@boolean; } }
    public static EasyObjectType @object { get { return EasyObjectType.@object; } }
    public static EasyObjectType @array { get { return EasyObjectType.@array; } }
    public static EasyObjectType @null { get { return EasyObjectType.@null; } }

    public bool IsString { get { return TypeValue == EasyObjectType.@string; } }
    public bool IsNumber { get { return TypeValue == EasyObjectType.@number; } }
    public bool IsBoolean { get { return TypeValue == EasyObjectType.@boolean; } }
    public bool IsObject { get { return TypeValue == EasyObjectType.@object; } }
    public bool IsArray { get { return TypeValue == EasyObjectType.@array; } }
    public bool IsNull { get { return TypeValue == EasyObjectType.@null; } }
    private static object? ExposeInternalObjectHelper(object? x) {
        while (x is EasyObject) {
            x = ((EasyObject)x).RealData;
        }
        return x;
    }
    private static EasyObject WrapInternal(object? x) {
        if (x is EasyObject) {
            return (x as EasyObject)!;
        }

        return new EasyObject(x);
    }
    public object? ExposeInternalObject() {
        return EasyObject.ExposeInternalObjectHelper(this);
    }
    public EasyObjectType TypeValue {
        get {
            object? obj = ExposeInternalObjectHelper(this);
            if (obj == null) {
                return EasyObjectType.@null;
            }
            switch (Type.GetTypeCode(obj.GetType())) {
                case TypeCode.Boolean:
                    return EasyObjectType.@boolean;
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
                    return EasyObjectType.@number;
                case TypeCode.Object:
                    return (obj is List<EasyObject>) ? EasyObjectType.@array : EasyObjectType.@object;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    if (obj is TimeSpan || obj is Guid) {
                        return EasyObject.@string;
                    }
                    return EasyObjectType.@null;
            }
        }
    }
    public string TypeName {
        get {
            return TypeValue.ToString();
        }
    }
    internal List<EasyObject>? list {
        get { return RealData as List<EasyObject>; }
    }
    internal Dictionary<string, EasyObject>? dictionary {
        get { return RealData as Dictionary<string, EasyObject>; }
    }
    public int Count {
        get {
            if (list != null) {
                return list.Count;
            }
            if (dictionary != null) {
                return dictionary.Count;
            }
            return 0;
        }
    }
    public List<string> Keys {
        get {
            var keys = new List<string>();
            if (dictionary == null) {
                return keys;
            }
            foreach (var key in dictionary.Keys) {
                keys.Add(key);
            }
            return keys;
        }
    }
    public bool ContainsKey(string name) {
        if (dictionary == null) {
            return false;
        }
        return dictionary.ContainsKey(name);
    }
    public EasyObject Add(object? x) {
        if (list == null) {
            RealData = new List<EasyObject>();
        }
        EasyObject eo = x is EasyObject ? (x as EasyObject)! : new EasyObject(x);
        list!.Add(eo);
        return this;
    }
    public EasyObject Add(string key, object? x) {
        if (dictionary == null) {
            RealData = new Dictionary<string, EasyObject>();
        }
        EasyObject eo = x is EasyObject ? (x as EasyObject)! : new EasyObject(x);
        dictionary!.Add(key, eo);
        return this;
    }
    public override bool TryGetMember(
        GetMemberBinder binder, out object result) {
        result = Null;
        string name = binder.Name;
        if (list != null) {
            var assoc = TryAssoc(name);
            result = assoc;
        }
        if (dictionary == null) {
            return true;
        }
        EasyObject? eo;
        dictionary.TryGetValue(name, out eo);
        if (eo == null) {
            eo = Null;
        }
        result = eo;
        return true;
    }
    public override bool TrySetMember(
        SetMemberBinder binder, object? value) {
        value = ExposeInternalObjectHelper(value);
        if (dictionary == null) {
            RealData = new Dictionary<string, EasyObject>();
        }
        string name = binder.Name;
        dictionary![name] = WrapInternal(value);
        return true;
    }
    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
        result = Null;
        var idx = indexes[0];
        if (idx is int) {
            int pos = (int)indexes[0];
            if (list == null) {
                result = WrapInternal(null);
                return true;
            }
            if (list.Count < (pos + 1)) {
                result = WrapInternal(null);
                return true;
            }
            result = WrapInternal(list[pos]);
            return true;
        }
        if (list != null) {
            var assoc = TryAssoc((string)idx);
            result = assoc;
        }
        if (dictionary == null) {
            result = Null;
            return true;
        }
        EasyObject? eo /*= Null*/;
        dictionary.TryGetValue((string)idx, out eo);
        if (eo == null) {
            eo = Null;
        }
        result = eo;
        return true;
    }
    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value) {
        if (value is EasyObject) {
            value = ((EasyObject)value).RealData;
        }
        var idx = indexes[0];
        if (idx is int) {
            int pos = (int)indexes[0];
            if (pos < 0) {
                throw new ArgumentException("index is below 0");
            }
            if (list == null) {
                RealData = new List<EasyObject>();
            }
            while (list!.Count < (pos + 1)) {
                list.Add(Null);
            }
            list[pos] = WrapInternal(value);
            return true;
        }
        if (dictionary == null) {
            RealData = new Dictionary<string, EasyObject>();
        }
        string name = (string)indexes[0];
        dictionary![name] = WrapInternal(value);
        return true;
    }
    public override bool TryConvert(ConvertBinder binder, out object? result) {
        if (binder.Type == typeof(IEnumerable)) {
            if (list != null) {
                var ie1 = list.Select(x => x);
                result = ie1;
                return true;
            }
            if (dictionary != null) {
                var ie2 = dictionary.Select(x => x);
                result = ie2;
                return true;
            }
            result = new List<EasyObject>().Select(x => x);
            return true;
        } else {
            result = Convert.ChangeType(RealData, binder.Type);
            return true;
        }
    }
    public static EasyObject FromObject(object? obj, bool ignoreErrors = false) {
        if (!ignoreErrors) {
            return new EasyObject(obj);
        }
        try {
            return new EasyObject(obj);
        } catch (Exception) {
            return new EasyObject(null);
        }
    }
    public static EasyObject FromJson(string? json, bool ignoreErrors = false) {
        if (json == null) {
            return Null;
        }
        if (!ignoreErrors) {
            return new EasyObject(JsonParser!.ParseJson(json));
        }
        try {
            return new EasyObject(JsonParser!.ParseJson(json));
        } catch (Exception) {
            return new EasyObject(null);
        }
    }
    public static EasyObject FromFile(string path, bool ignoreErrors = false) {
        return FromJson(File.ReadAllText(path), ignoreErrors: ignoreErrors);
    }
    public static string Utf8StringFromUrl(string url) {
#pragma warning disable SYSLIB0014
        HttpWebRequest? request = WebRequest.Create(url) as HttpWebRequest;
#pragma warning restore SYSLIB0014
        HttpWebResponse response = (HttpWebResponse)request!.GetResponse();
        //WebHeaderCollection header = response.Headers;
        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8)) {
            return reader.ReadToEnd();
        }
    }
    public void InjectToFile(
        string path,
        bool indent = false,
        bool sortKeys = false,
        bool keyAsSymbol = false,
        bool removeSurrogatePair = false
        ) {
        string json = ToJson(indent: indent, sortKeys: sortKeys, keyAsSymbol: keyAsSymbol);
        EasyTextEmbedder.InjectEmbeddedText(path, json);
    }
    public static EasyObject ExtractFromFile(string pathOrUrl, bool ignoreErrors = false) {
        string json = EasyTextEmbedder.ExtractEmbeddedText(pathOrUrl) ?? "null";
        return FromJson(json, ignoreErrors: ignoreErrors);
    }
    public static EasyObject FromUrl(string url, bool ignoreErrors = false) {
        var m = EasySystem.FindFirstMatch(
            url,
            @"^(https://github[.]com/[^/]+/[^/]+/)blob(/.+)$",
            @"^(https://gitlab[.]com/nuget-tools/nuget-assets/-/)blob(/.+)$"
            );
        if (m != null) {
            url = m[1] + "raw" + m[2];
        }
        if (!ignoreErrors) {
            string json = Utf8StringFromUrl(url);
            return FromJson(json, ignoreErrors: ignoreErrors);
        }
        try {
            string json = Utf8StringFromUrl(url);
            return FromJson(json, ignoreErrors: ignoreErrors);
        } catch (Exception) {
            return new EasyObject(null);
        }
    }
    public dynamic? ToObject(bool asDynamicObject = false) {
        if (asDynamicObject) {
            return ExportToDynamicObject();
        } else {
            return ExportToPlainObject();
        }
    }
    public string ToJson(bool indent = false, bool sortKeys = false, bool keyAsSymbol = false, bool removeSurrogatePair = false) {
        PlainObjectConverter poc = new PlainObjectConverter(jsonParser: JsonParser, forceAscii: ForceAscii);
        return poc.Stringify(RealData, indent, sortKeys, keyAsSymbol: keyAsSymbol, removeSurrogatePair: removeSurrogatePair);
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
    public static string ToPrintable(object? x, string? title = null, bool compact = false, uint maxDepth = 0, bool removeSurrogatePair = false) {
        PlainObjectConverter poc = new PlainObjectConverter(jsonParser: JsonParser, forceAscii: ForceAscii);
        if (maxDepth != 0) {
            x = FromObject(x).Clone(maxDepth: maxDepth, always: false);
        }
        string printable = poc.ToPrintable(ShowDetail, x, title, compact: compact, removeSurrogatePair: removeSurrogatePair);
        return printable;
    }
    public static string MarkupSafeString(string str) {
#if USE_SPECTRE_CONSOLE
        return Markup.Escape(str);
#else
        return str;
#endif
    }
    public static void Write(
        string str,
        string? title = null
        ) {
#if USE_SPECTRE_CONSOLE
        if (title != null) {
            if (title.StartsWith("⁅markup⁆")) {
                title = title.Replace("⁅markup⁆", "");
                AnsiConsole.Markup($"{title}: ");
            } else {
                AnsiConsole.Write($"{title}: ");
            }
        }
        if (str.StartsWith("⁅markup⁆")) {
            str = str.Replace("⁅markup⁆", "");
            AnsiConsole.MarkupLine($"[purple]{str}[/]");
        } else {
            AnsiConsole.Write(str);
        }
#else
        if (title != null) {
            Console.Write($"{title}: ");
        }
        Console.Write(str);
#endif
    }
    public static void WriteLine(
        string str,
        string? title = null
        ) {
#if USE_SPECTRE_CONSOLE
        if (title != null) {
            if (title.StartsWith("⁅markup⁆")) {
                title = title.Replace("⁅markup⁆", "");
                AnsiConsole.MarkupLine($"{title}: ");
            } else {
                AnsiConsole.Write($"{title}: ");
            }
        }
        if (str.StartsWith("⁅markup⁆")) {
            str = str.Replace("⁅markup⁆", "");
            AnsiConsole.MarkupLine($"[purple]{str}[/]");
        } else {
            AnsiConsole.WriteLine(str);
        }
#else
        if (title != null) {
            Console.Write($"{title}: ");
        }
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
        ) {
        _EnsureCursorLeft();
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0) {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth: maxDepth,
                hideKeys: hideKeys,
                always: false);
        }
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole) {
            if (title != null) {
                if (title.StartsWith("⁅markup⁆")) {
                    title = title.Replace("⁅markup⁆", "");
                    AnsiConsole.Markup($"{title}: ");
                } else {
                    AnsiConsole.Write($"{title}: ");
                }
            }
            if (x != null && x is string str) {
                if (str.StartsWith("⁅markup⁆")) {
                    str = str.Replace("⁅markup⁆", "");
                    AnsiConsole.MarkupLine(str);
                    return;
                }
            }
            string s2 = ToPrintable(x, title: null, compact: compact, maxDepth: maxDepth, removeSurrogatePair: removeSurrogatePair);
            string s3 = MarkupSafeString(s2);
            AnsiConsole.MarkupLine(s3);
            return;
        }
#endif
        string s = ToPrintable(x, title, compact: compact, maxDepth: maxDepth, removeSurrogatePair: removeSurrogatePair);
        Console.WriteLine(s);
    }
    public static void Log(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
        ) {
        _EnsureCursorLeft();
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0) {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth: maxDepth,
                hideKeys: hideKeys,
                always: false);
        }
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole) {
            AnsiErrorConsole.Markup("[cyan][[Log]][/] ");
            if (title != null) {
                if (title.StartsWith("⁅markup⁆")) {
                    title = title.Replace("⁅markup⁆", "");
                    AnsiErrorConsole.Markup($"{title}: ");
                } else {
                    AnsiErrorConsole.Write($"{title}: ");
                }
            }
            if (x != null && x is string str) {
                if (str.StartsWith("⁅markup⁆")) {
                    str = str.Replace("⁅markup⁆", "");
                    AnsiErrorConsole.MarkupLine(str);
                    if (ShowLineNumbers) {
                        AnsiErrorConsole.MarkupLine($"      [blue]{MarkupSafeString(CurrentSourceCodeLine())}[/]");
                    }
                    return;
                }
            }
            string s2 = ToPrintable(x, title: null, compact: compact, maxDepth: maxDepth, removeSurrogatePair: removeSurrogatePair);
            string s3 = MarkupSafeString(s2);
            AnsiErrorConsole.MarkupLine(s3);
            if (ShowLineNumbers) {
                AnsiErrorConsole.MarkupLine($"      [blue]{MarkupSafeString(CurrentSourceCodeLine())}[/]");
            }
            return;
        }
#endif
        string s = ToPrintable(x, title, compact: compact, maxDepth: maxDepth, removeSurrogatePair: removeSurrogatePair);
        Console.Error.WriteLine("[Log] " + s);
        if (ShowLineNumbers) {
            Console.Error.WriteLine($"      {CurrentSourceCodeLine()}");
        }
    }
    public static void Debug(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
        ) {
        if (!DebugOutput) {
            return;
        }
        _EnsureCursorLeft();
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0) {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth: maxDepth,
                hideKeys: hideKeys,
                always: false);
        }
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole) {
            AnsiErrorConsole.Markup("[purple][[Debug]][/] ");
            if (title != null) {
                if (title.StartsWith("⁅markup⁆")) {
                    title = title.Replace("⁅markup⁆", "");
                    AnsiErrorConsole.Markup($"{title}: ");
                } else {
                    AnsiErrorConsole.Markup($"[purple]{MarkupSafeString(title)}:[/] ");
                }
            }
            if (x != null && x is string str) {
                if (str.StartsWith("⁅markup⁆")) {
                    str = str.Replace("⁅markup⁆", "");
                    AnsiErrorConsole.MarkupLine($"[purple]{str}[/]");
                    //if (ShowLineNumbers) {
                    AnsiErrorConsole.MarkupLine($"        [purple]{MarkupSafeString(CurrentSourceCodeLine())}[/]");
                    //}
                    return;
                }
            }
            string s2 = ToPrintable(x, title: null, compact: compact, maxDepth: maxDepth, removeSurrogatePair: removeSurrogatePair);
            string s3 = MarkupSafeString(s2);
            AnsiErrorConsole.MarkupLine($"[purple]{s3}[/]");
            //if (ShowLineNumbers) {
            AnsiErrorConsole.MarkupLine($"        [purple]{MarkupSafeString(CurrentSourceCodeLine())}[/]");
            //}
            return;
        }
#endif
        string s = ToPrintable(x, title, compact: compact, maxDepth: maxDepth, removeSurrogatePair: removeSurrogatePair);
        Console.Error.WriteLine("[Debug] " + s);
        //if (ShowLineNumbers) {
        Console.Error.WriteLine($"  {CurrentSourceCodeLine()}");
        //}
    }
    public static void Message(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null
        ) {
        if (title == null) {
            title = "Message";
        }
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0) {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth: maxDepth,
                hideKeys: hideKeys,
                always: false);
        }
        string s = ToPrintable(x, title: null, compact: compact);
        NativeMethods.MessageBoxW(IntPtr.Zero, s, title, 0);
    }
    public static void DumpObject(
        object? x,
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
        ) {
        _EnsureCursorLeft();
#if USE_SPECTRE_CONSOLE
        var printable = FromObject(x).Clone(maxDepth: maxDepth, hideKeys: hideKeys, always: false);
        var json = printable.ToJson(indent: !compact, removeSurrogatePair: removeSurrogatePair);
        var jsonText = new JsonText(json);
        if (title != null) {
            if (title.StartsWith("⁅markup⁆")) {
                title = title.Replace("⁅markup⁆", "");
                AnsiConsole.Markup($"{title}: ");
            } else {
                AnsiConsole.Write($"{title}: ");
            }
        }
        AnsiConsole.Write(jsonText);
        AnsiConsole.WriteLine();
#else
        Echo(
            x,
            title: title,
            compact: compact,
            maxDepth: maxDepth,
            hideKeys: hideKeys,
            removeSurrogatePair: removeSurrogatePair
            );
#endif
    }
    public void Dump(
        string? title = null,
        bool compact = false,
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool removeSurrogatePair = false
        ) {
        DumpObject(this, title: title, compact: compact, maxDepth: maxDepth, hideKeys: hideKeys, removeSurrogatePair: removeSurrogatePair);
    }
    private static class NativeMethods {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBoxW(
            IntPtr hWnd, string lpText, string lpCaption, uint uType);
    }
    private EasyObject TryAssoc(string name) {
        try {
            if (list == null) {
                return Null;
            }
            for (int i = 0; i < list.Count; i++) {
                var pair = list[i].AsList!;
                if (pair[0].Cast<string>() == name) {
                    return pair[1];
                }
            }
            return Null;
        } catch (Exception /*e*/) {
            return Null;
        }
    }
    public EasyObject this[string name] {
        get {
            if (list != null) {
                return TryAssoc(name);
            }
            if (dictionary == null) {
                return Null;
            }
            EasyObject? eo;
            dictionary.TryGetValue(name, out eo);
            if (eo == null) {
                return Null;
            }
            return eo;
        }
        set {
            if (dictionary == null) {
                RealData = new Dictionary<string, EasyObject>();
            }
            dictionary![name] = value;
        }
    }
    public EasyObject this[int pos] {
        get {
            if (list == null) {
                return WrapInternal(null);
            }
            if (list.Count < (pos + 1)) {
                return WrapInternal(null);
            }
            return WrapInternal(list[pos]);
        }
        set {
            if (pos < 0) {
                throw new ArgumentException("index below 0");
            }
            if (list == null) {
                RealData = new List<EasyObject>();
            }
            while (list!.Count < (pos + 1)) {
                list.Add(Null);
            }
            list[pos] = value;
        }
    }
    public T Cast<T>() {
        if (RealData is DateTime dt) {
            string? s = null;
            switch (dt.Kind) {
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
    public List<EasyObject>? AsList {
        get {
            return list;
        }
    }
    public Dictionary<string, EasyObject>? AsDictionary {
        get {
            return dictionary;
        }
    }
    public static string FullName(dynamic? x) {
        if (x is null) {
            return "null";
        }
        string fullName = ((object)x).GetType().FullName!;
        if (fullName.StartsWith("<>f__AnonymousType")) {
            return "AnonymousType";
        }
        return fullName!.Split('`')[0];
    }
    public static implicit operator EasyObject(bool x) { return new EasyObject(x); }
    public static implicit operator EasyObject(string x) { return new EasyObject(x); }
    public static implicit operator EasyObject(char x) { return new EasyObject(x); }
    public static implicit operator EasyObject(short x) { return new EasyObject(x); }
    public static implicit operator EasyObject(int x) { return new EasyObject(x); }
    public static implicit operator EasyObject(long x) { return new EasyObject(x); }
    public static implicit operator EasyObject(ushort x) { return new EasyObject(x); }
    public static implicit operator EasyObject(uint x) { return new EasyObject(x); }
    public static implicit operator EasyObject(ulong x) { return new EasyObject(x); }
    public static implicit operator EasyObject(float x) { return new EasyObject(x); }
    public static implicit operator EasyObject(double x) { return new EasyObject(x); }
    public static implicit operator EasyObject(decimal x) { return new EasyObject(x); }
    public static implicit operator EasyObject(sbyte x) { return new EasyObject(x); }
    public static implicit operator EasyObject(byte x) { return new EasyObject(x); }
    public static implicit operator EasyObject(DateTime x) { return new EasyObject(x); }
    public static implicit operator EasyObject(TimeSpan x) { return new EasyObject(x); }
    public static implicit operator EasyObject(Guid x) { return new EasyObject(x); }
    public void Nullify() {
        RealData = null;
    }
    public void Trim(
            uint maxDepth = 0,
            List<string>? hideKeys = null
        ) {
        EasyObjectEditor.Trim(this, maxDepth, hideKeys);
    }
    public EasyObject Clone(
        uint maxDepth = 0,
        List<string>? hideKeys = null,
        bool always = true
        ) {
        return EasyObjectEditor.Clone(this, maxDepth, hideKeys, always);
    }
    public EasyObject? Shift() {
        if (list == null) {
            return null;
        }
        if (list.Count == 0) {
            return null;
        }
        EasyObject result = list[0];
        list.RemoveAt(0);
        return result;
    }
    public EasyObject Shuffle() {
        if (list != null) {
            var list2 = list!.Select(i => i).OrderBy(i => Guid.NewGuid()).ToList();
            return FromObject(list2);
        }
        if (dictionary != null) {
            var keys = dictionary.Keys!.Select(i => i).OrderBy(i => Guid.NewGuid()).ToList();
            var result = NewObject();
            foreach (var key in keys) {
                result[key] = dictionary[key];
            }
            return result;
        }
        return Clone();
    }
    public EasyObject Reverse() {
        if (list != null) {
            var list2 = list!.AsEnumerable().Reverse().Take(5).ToList();
            return FromObject(list2);
        }
        if (dictionary != null) {
            var keys = dictionary.Keys!.AsEnumerable().Reverse().Take(5).ToList();
            var result = NewObject();
            foreach (var key in keys) {
                result[key] = dictionary[key];
            }
            return result;
        }
        return Clone();
    }
    public EasyObject Skip(int n) {
        if (list != null) {
            var list2 = list!.Select(i => i).Skip(n).ToList();
            return FromObject(list2);
        }
        if (dictionary != null) {
            var keys = dictionary.Keys!.Select(i => i).Skip(n).ToList();
            var result = NewObject();
            foreach (var key in keys) {
                result[key] = dictionary[key];
            }
            return result;
        }
        return Clone();
    }
    public EasyObject Take(int n) {
        if (list != null) {
            var list2 = list!.Select(i => i).Take(n).ToList();
            return FromObject(list2);
        }
        if (dictionary != null) {
            var keys = dictionary.Keys!.Select(i => i).Take(n).ToList();
            var result = NewObject();
            foreach (var key in keys) {
                result[key] = dictionary[key];
            }
            return result;
        }
        return Clone();
    }
    public string[] AsStringArray {
        get {
            if (list != null) {
                return
                    list!
                    .Select(
                        i =>
                        i.IsString ?
                        i.Cast<string>() :
                        i.ToJson(keyAsSymbol: true, indent: false))
                    .ToArray();
            }
            if (dictionary != null) {
                return dictionary.Keys!.Select(i => i).ToArray();
            }
            return [];
        }
    }
    public List<string> AsStringList {
        get {
            return AsStringArray.ToList();
        }
    }
    public void ImportFromPlainObject(object? x) {
        var eo = FromObject(x);
        RealData = eo.RealData;
    }
    public void ImportFromCommonJson(string x) {
        var eo = FromJson(x);
        if (eo == null) {
            eo = Null;
        }
        RealData = eo!.RealData;
    }
    public string ExportToCommonJson() {
        return ToJson(
            indent: true,
            sortKeys: false
            );
    }
    public object? ExportToPlainObject() {
        return new PlainObjectConverter(jsonParser: null, forceAscii: ForceAscii).Parse(RealData);
    }
    public dynamic? ExportToDynamicObject() {
        return EasyObjectEditor.ExportToExpandoObject(this);
    }
    public static string ObjectToJson(object? x, bool indent = false) {
        return FromObject(x).ToJson(indent: indent);
    }
    public static object? ObjectToObject(object? x, bool asDynamicObject = false) {
        return FromObject(x).ToObject(asDynamicObject: asDynamicObject);
    }
    public static string ToClickableUri(string pathOrUrl) {
        Debug(pathOrUrl, "pathOrUrl");
        if (pathOrUrl.StartsWith("http:") || pathOrUrl.StartsWith("https:") || pathOrUrl.StartsWith("file:")) {
            return pathOrUrl;
        }
        string filePath = pathOrUrl;
        filePath = Path.GetFullPath(filePath);
        return new Uri(filePath).AbsoluteUri;
    }
    public static void LogWebLink(string title, string url) {
        url = ToClickableUri(url);
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole) {
            EasyObject.Log($"⁅markup⁆[green][link={url}]{EasyObject.MarkupSafeString(title)}[/][/] => [blue]{EasyObject.MarkupSafeString(url)}[/]");
        } else {
            EasyObject.Log($"{EasyObject.MarkupSafeString(title)} => {EasyObject.MarkupSafeString(url)}");
        }
#else
        EasyObject.Log($"{title} => {url}");
#endif
    }
    public static void EchoWebLink(string title, string url) {
        url = ToClickableUri(url);
#if USE_SPECTRE_CONSOLE
        if (UseAnsiConsole) {
            EasyObject.Echo($"⁅markup⁆[green][link={url}]{EasyObject.MarkupSafeString(title)}[/][/] => [blue]{EasyObject.MarkupSafeString(url)}[/]");
        } else {
            EasyObject.Echo($"{EasyObject.MarkupSafeString(title)} => {EasyObject.MarkupSafeString(url)}");

        }
#else
        EasyObject.Echo($"{title} => {url}");
#endif
    }
    public static string CurrentSourceCodeLine() {
        string trace = Environment.StackTrace;
        List<string> lines = EasySystem.TextToLines(trace);
        if (lines.Count == 0) {
            return "[!! UNKNOWN SOURCE CODE LINE !!]";
        }
        return ReplacePathsWithUrls(lines[lines.Count - 1].Trim());
    }
    public static string ReplacePathsWithUrls(string stackTrace) { // https://shorturl.ly/FToES C# search through string like stack trace for source code path and replace all of them to a "file:" urls - Google
        stackTrace = stackTrace.Replace("場所 ", "in ");
        stackTrace = stackTrace.Replace(":行 ", ":line ");
        // This regex looks for common file path patterns, especially those with drive letters (C:\) 
        // or starting with a slash (/) often followed by common extensions like .cs, .vb, etc., within the context of a stack trace line.
        // The pattern aims to capture the full file path including extension and line number info if present.
        // Group 1 captures the path part for replacement.
        var filePathRegex = new Regex(@"(?:in\s+)(?<path>[a-zA-Z]:\\(?:[^<>:""/\\|?*]+\\)*[^<>:""/\\|?*]+|/(?:[^/]+\s?)+(?<enging>:line\s+(?<line_num>\d+))$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        // Use a MatchEvaluator delegate for the replacement to apply the Uri conversion logic to each match.
        string result = filePathRegex.Replace(stackTrace, match =>
        {
            string filePath = match.Groups["path"].Value;
            Log(filePath, "filePath");
            string line_num = match.Groups["line_num"].Value;
            Log(line_num, "line_num");
            try {
                // The System.Uri constructor handles the specific formatting requirements for file URIs, 
                // including correct handling of slashes and special characters like spaces.
                var fileUri = new Uri(filePath);
                // We use AbsoluteUri which correctly formats the scheme (file://) and path for a URL.
                return $"in {fileUri.AbsoluteUri} {ending}";
            } catch (UriFormatException) {
                // Fallback for paths that the Uri class might not handle correctly (e.g., highly unusual formats)
                return match.Value;
            }
        });
        return result;
    }
    public static void Crash(object? message = null, int exitCode = 1) {
        ShowDetail = false;
        ShowLineNumbers = false;
        UseAnsiConsole = false;
        Log("[!! PROGRAM CRASHED !!]");
        if (message != null && !(message is Exception)) {
            Log(message, "MESSAGE");
        }
        if (message is Exception e) {
            string exTrace = e.ToString();
            //Log(exTrace, "(1)");
            try {
                exTrace = ReplacePathsWithUrls(exTrace);
            } catch (Exception ex) {
                Console.Error.WriteLine(ex);
            }
            //Log(exTrace, "(2)");
            Console.Error.WriteLine(
                exTrace
                );
            return;
        }
        string trace = Environment.StackTrace;
        List<string> lines = EasySystem.TextToLines(trace);
        lines = lines.Skip(3).ToList();
        trace = "\n" + string.Join("\n", lines);
        trace = ReplacePathsWithUrls(trace);
        Log(trace, "STACK TRACE");
        Log($"[!! ABORTING...WITH EXIT CODE {exitCode} !!]");
        Environment.Exit(exitCode);
    }
}
