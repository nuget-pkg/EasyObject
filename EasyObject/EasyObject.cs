using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Global;

public enum EasyObjectType
{
    // ReSharper disable once InconsistentNaming
    @string,
    // ReSharper disable once InconsistentNaming
    @number,
    // ReSharper disable once InconsistentNaming
    @boolean,
    // ReSharper disable once InconsistentNaming
    @object,
    // ReSharper disable once InconsistentNaming
    @array,
    // ReSharper disable once InconsistentNaming
    @null
}

internal class EasyObjectConverter : IConvertParsedResult
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
        else if (x is List<object>)
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

public class EasyObject :
    DynamicObject,
    IExposeInternalObject,
    IExportToPlainObject,
    IImportFromPlainObject,
    IExportToCommonJson,
    IImportFromCommonJson
{
    public object? RealData /*= null*/;
    public static readonly bool IsConsoleApplication = HasConsole();

    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly IParseJson DefaultJsonParser = new CSharpEasyLanguageHandler(numberAsDecimal: true);
    public static IParseJson? JsonParser /*= null*/;
    // ReSharper disable once MemberCanBePrivate.Global
    public static bool DebugOutput /*= false*/;
    public static bool ShowDetail /*= false*/;
    // ReSharper disable once MemberCanBePrivate.Global
    public static bool ForceAscii /*= false*/;

    public static void ClearSettings()
    {
        EasyObject.JsonParser = DefaultJsonParser;
        EasyObject.DebugOutput = false;
        EasyObject.ShowDetail = false;
        EasyObject.ForceAscii = false;
    }

    static EasyObject()
    {
        EasyObject.ClearSettings();
    }

    public EasyObject()
    {
        this.RealData = null;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public EasyObject(object? x)
    {
        this.RealData = new PlainObjectConverter(jsonParser: JsonParser, forceAscii: false, iConvertParsedResult: new EasyObjectConverter()).Parse(x, numberAsDecimal: true);
    }

    public dynamic Dynamic { get { return this; } }

    public override string ToString()
    {
        return this.ToPrintable();
    }

    public object? ToPlainObject()
    {
        return this.ToObject();
    }

    public string ToPrintable()
    {
        return EasyObject.ToPrintable(this);
    }

    public static EasyObject Null { get { return new EasyObject(); } }
    public static EasyObject EmptyArray { get { return new EasyObject(new List<EasyObject>()); } }
    public static EasyObject EmptyObject { get { return new EasyObject(new Dictionary<string, EasyObject>()); } }

    public static EasyObject NewArray(params object[] args)
    {
        EasyObject result = EmptyArray;
        for (int i = 0; i < args.Length; i++)
        {
            result.Add(FromObject(args[i]));
        }
        return result;
    }
    public static EasyObject NewObject(params object[] args)
    {
        if ((args.Length % 2) != 0) throw new ArgumentException("EasyObject.NewObject() requires even number arguments");
        EasyObject result = EmptyObject;
        for (int i = 0; i < args.Length; i += 2)
        {
            result.Add(args[i].ToString()!, FromObject(args[i + 1]));
        }
        return result;
    }

    // ReSharper disable once InconsistentNaming
    public static EasyObjectType @string { get { return EasyObjectType.@string; } }
    // ReSharper disable once InconsistentNaming
    public static EasyObjectType @boolean { get { return EasyObjectType.@boolean; } }
    // ReSharper disable once InconsistentNaming
    public static EasyObjectType @object { get { return EasyObjectType.@object; } }
    // ReSharper disable once InconsistentNaming
    public static EasyObjectType @array { get { return EasyObjectType.@array; } }
    // ReSharper disable once InconsistentNaming
    public static EasyObjectType @null { get { return EasyObjectType.@null; } }

    public bool IsString { get { return this.TypeValue == EasyObjectType.@string; } }
    public bool IsNumber { get { return this.TypeValue == EasyObjectType.@number; } }
    public bool IsBoolean { get { return this.TypeValue == EasyObjectType.@boolean; } }
    public bool IsObject { get { return this.TypeValue == EasyObjectType.@object; } }
    public bool IsArray { get { return this.TypeValue == EasyObjectType.@array; } }
    public bool IsNull { get { return this.TypeValue == EasyObjectType.@null; } }

    private static object? ExposeInternalObjectHelper(object? x)
    {
        while (x is EasyObject)
        {
            x = ((EasyObject)x).RealData;
        }
        return x;
    }

    private static EasyObject WrapInternal(object? x)
    {
        if (x is EasyObject) return (x as EasyObject)!;
        return new EasyObject(x);
    }

    public object? ExposeInternalObject()
    {
        return EasyObject.ExposeInternalObjectHelper(this);
    }

    public EasyObjectType TypeValue
    {
        get
        {
            object? obj = ExposeInternalObjectHelper(this);
            if (obj == null) return EasyObjectType.@null;
            switch (Type.GetTypeCode(obj.GetType()))
            {
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
                    if (obj is TimeSpan || obj is Guid) return EasyObject.@string;
                    return EasyObjectType.@null;
            }
        }
    }

    public string TypeName
    {
        get
        {
            return this.TypeValue.ToString();
        }
    }

    // ReSharper disable once InconsistentNaming
    internal List<EasyObject>? list
    {
        get { return RealData as List<EasyObject>; }
    }

    // ReSharper disable once InconsistentNaming
    internal Dictionary<string, EasyObject>? dictionary
    {
        get { return RealData as Dictionary<string, EasyObject>; }
    }

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

    public EasyObject Add(object x)
    {
        if (list == null) RealData = new List<EasyObject>();
        EasyObject eo = x is EasyObject ? (x as EasyObject)! : new EasyObject(x);
        list!.Add(eo);
        return this;
    }

    public EasyObject Add(string key, object? x)
    {
        if (dictionary == null) RealData = new Dictionary<string, EasyObject>();
        EasyObject eo = x is EasyObject ? (x as EasyObject)! : new EasyObject(x);
        dictionary!.Add(key, eo);
        return this;
    }

    public override bool TryGetMember(
        GetMemberBinder binder, out object result)
    {
        result = Null;
        string name = binder.Name;
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
        if (dictionary == null)
        {
            RealData = new Dictionary<string, EasyObject>();
        }
        string name = binder.Name;
        dictionary![name] = WrapInternal(value);
        return true;
    }
    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
        result = Null;
        var idx = indexes[0];
        if (idx is int)
        {
            int pos = (int)indexes[0];
            if (list == null)
            {
                result = WrapInternal(null);
                return true;
            }
            if (list.Count < (pos + 1))
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
            int pos = (int)indexes[0];
            if (pos < 0) throw new ArgumentException("index is below 0");
            if (list == null)
            {
                RealData = new List<EasyObject>();
            }
            while (list!.Count < (pos + 1))
            {
                list.Add(Null);
            }
            list[pos] = WrapInternal(value);
            return true;
        }
        if (dictionary == null)
        {
            RealData = new Dictionary<string, EasyObject>();
        }
        string name = (string)indexes[0];
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
            result = (new List<EasyObject>()).Select(x => x);
            return true;
        }
        else
        {
            result = Convert.ChangeType(RealData, binder.Type);
            return true;
        }
    }

    private static string[] TextToLines(string text)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(text))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
        return lines.ToArray();
    }
    public static EasyObject FromObject(object? obj, bool ignoreErrors = false)
    {
        if (!ignoreErrors)
        {
            return new EasyObject(obj);
        }
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
        if (json.StartsWith("#!"))
        {
            string[] lines = TextToLines(json);
            lines = lines.Skip(1).ToArray();
            json = String.Join("\n", lines);
        }
        if (!ignoreErrors)
        {
            return new EasyObject(JsonParser!.ParseJson(json));
        }
        try
        {
            return new EasyObject(JsonParser!.ParseJson(json));
        }
        catch (Exception)
        {
            return new EasyObject(null);
        }
    }

    public dynamic? ToObject()
    {
        return new PlainObjectConverter(jsonParser: null, forceAscii: ForceAscii).Parse(RealData);
    }

    public string ToJson(bool indent = false, bool sortKeys = false)
    {
        PlainObjectConverter poc = new PlainObjectConverter(jsonParser: JsonParser, forceAscii: ForceAscii);
        return poc.Stringify(RealData, indent, sortKeys);
    }

#if USE_WINCONSOLE
    // ReSharper disable once MemberCanBePrivate.Global
    public static void AllocConsole()
    {
        if (IsConsoleApplication) return;
        WinConsole.Alloc();
    }
    // ReSharper disable once MemberCanBePrivate.Global
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

    public static string ToPrintable(object? x, string? title = null)
    {
        //x = FromObject(x).ExportToPlainObject(); /**/
        PlainObjectConverter poc = new PlainObjectConverter(jsonParser: JsonParser, forceAscii: ForceAscii);
        return poc.ToPrintable(ShowDetail, x, title);
    }

    public static void Echo(
        object? x,
        string? title = null,
        uint maxDepth = 0,
        List<string>? hideKeys = null
        )
    {
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0)
        {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth: maxDepth,
                hideKeys: hideKeys,
                always: false);
        }
        string s = ToPrintable(x, title);
        Console.WriteLine(s);
        System.Diagnostics.Debug.WriteLine(s);
    }
    public static void Log(
        object? x,
        string? title = null,
        uint maxDepth = 0,
        List<string>? hideKeys = null
        )
    {
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0)
        {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth: maxDepth,
                hideKeys: hideKeys,
                always: false);
        }
        string s = ToPrintable(x, title);
        Console.Error.WriteLine("[Log] " + s);
        System.Diagnostics.Debug.WriteLine("[Log] " + s);
    }
    public static void Debug(
        object? x,
        string? title = null,
        uint maxDepth = 0,
        List<string>? hideKeys = null
        )
    {
        if (!DebugOutput) return;
        hideKeys ??= new List<string>();
        if (maxDepth > 0 || hideKeys.Count > 0)
        {
            var eo = FromObject(x);
            x = eo.Clone(
                maxDepth: maxDepth,
                hideKeys: hideKeys,
                always: false);
        }
        string s = ToPrintable(x, title);
        Console.Error.WriteLine("[Debug] " + s);
        System.Diagnostics.Debug.WriteLine("[Debug] " + s);
    }
    public static void Message(
        object? x,
        string? title = null,
        uint maxDepth = 0,
        List<string>? hideKeys = null
        )
    {
        if (title == null) title = "Message";
        string s = ToPrintable(x, title: title);
        NativeMethods.MessageBoxW(IntPtr.Zero, s, title, 0);
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
            for (int i = 0; i < list.Count; i++)
            {
                var pair = list[i].AsList!;
                if (pair[0].Cast<string>() == name)
                {
                    return pair[1];
                }
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
            if (list != null)
            {
                return TryAssoc(name);
            }
            if (dictionary == null) return Null;
            EasyObject? eo;
            dictionary.TryGetValue(name, out eo);
            if (eo == null) return Null;
            return eo;
        }
        set
        {
            if (dictionary == null)
            {
                RealData = new Dictionary<string, EasyObject>();
            }
            dictionary![name] = value;
        }
    }
    public EasyObject this[int pos]
    {
        get
        {
            if (list == null)
            {
                return WrapInternal(null);
            }
            if (list.Count < (pos + 1))
            {
                return WrapInternal(null);
            }
            return WrapInternal(list[pos]);
        }
        set
        {
            if (pos < 0) throw new ArgumentException("index below 0");
            if (list == null)
            {
                RealData = new List<EasyObject>();
            }
            while (list!.Count < (pos + 1))
            {
                list.Add(Null);
            }
            list[pos] = value;
        }
    }
    public T Cast<T>()
    {
        if (this.RealData is DateTime dt)
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
        return (T)Convert.ChangeType(this.RealData, typeof(T))!;
    }
    public List<EasyObject>? AsList
    {
        get
        {
            // ReSharper disable once ArrangeAccessorOwnerBody
            return list;
        }
    }
    public Dictionary<string, EasyObject>? AsDictionary
    {
        get
        {
            // ReSharper disable once ArrangeAccessorOwnerBody
            return dictionary;
        }
    }

    public static string FullName(dynamic x)
    {
        if (x is null) return "null";
        string fullName = ((object)x).GetType().FullName!;
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

    public void Nullify()
    {
        this.RealData = null;
    }

    public void Trim(
            uint maxDepth = 0,
            List<string>? hideKeys = null
        )
    {
        EasyObjectEditor.Trim( this, maxDepth, hideKeys );
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
        if (this.list == null) return null;
        if (this.list.Count == 0) return null;
        EasyObject result = this.list[0];
        this.list.RemoveAt(0);
        return result;
    }

    public object? ExportToPlainObject()
    {
        return this.ToObject();
    }
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

    public void ImportFromPlainObject(object? x)
    {
        var eo = FromObject(x);
        this.RealData = eo.RealData;
    }

    public void ImportFromCommonJson(string x)
    {
        var eo = FromJson(x);
        if (eo == null)
        {
            eo = Null;
        }
        this.RealData = eo!.RealData;
    }

    public string ExportToCommonJson()
    {
        return this.ToJson(
            indent: true,
            sortKeys: false
            );
    }
    public dynamic? ExportToExpandoObject()
    {
        return EasyObjectEditor.ExportToExpandoObject(this);
    }
}
