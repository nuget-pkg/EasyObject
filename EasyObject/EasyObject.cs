using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Text;

namespace Global;

public enum EasyObjectType
{
    @string, @number, @boolean, @object, @array, @null
}

internal class _EasyObjectConverter : IObjectConverter
{
    public object ConvertResult(object x, string origTypeName)
    {
        if (x is Dictionary<string, object>)
        {
            var dict = x as Dictionary<string, object>;
            var keys = dict.Keys;
            var result = new Dictionary<string, EasyObject>();
            foreach (var key in keys)
            {
                var eo = new EasyObject();
                eo.m_data = dict[key];
                result[key] = eo;
            }
            return result;
        } else if (x is List<object>)
        {
            var list = x as List<object>;
            var result = new List<EasyObject>();
            foreach (var e in list)
            {
                var eo = new EasyObject();
                eo.m_data = e;
                result.Add(eo);
            }
            return result;
        }
        return x;
    }
}

public class EasyObject : DynamicObject, IObjectWrapper
{
    public object m_data = null;

#if false
    public static IJsonHandler DefaultJsonHandler = new CSharpJsonHandler(true, false);
#else
    public static IJsonHandler DefaultJsonHandler = new CSharpEasyLanguageHandler(true, false);
#endif
    public static IJsonHandler JsonHandler = null;
    public static bool DebugOutput = false;
    public static bool ShowDetail = false;
    public static bool ForceASCII = false;

    public static void ClearSettings()
    {
        EasyObject.JsonHandler = DefaultJsonHandler;
        EasyObject.DebugOutput = false;
        EasyObject.ShowDetail = false;
        EasyObject.ForceASCII = false;
    }

    static EasyObject()
    {
        EasyObject.ClearSettings();
    }

    public EasyObject()
    {
        this.m_data = null;
    }

    public EasyObject(object x)
    {
        this.m_data = new ObjectParser(false, new _EasyObjectConverter()).Parse(x, true);
    }

    public dynamic Dynamic {  get { return this; } }

    public override string ToString()
    {
        return this.ToPrintable();
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
        for (int i = 0; i < args.Length; i+= 2)
        {
            result.Add(args[i].ToString(), FromObject(args[i+1]));
        }
        return result;
    }

    public static EasyObjectType @string { get { return EasyObjectType.@string; } }
    public static EasyObjectType @boolean { get { return EasyObjectType.@boolean; } }
    public static EasyObjectType @object { get { return EasyObjectType.@object; } }
    public static EasyObjectType @array { get { return EasyObjectType.@array; } }
    public static EasyObjectType @null { get { return EasyObjectType.@null; } }

    public bool IsString { get { return this.TypeValue == EasyObjectType.@string; } }
    public bool IsNumber { get { return this.TypeValue == EasyObjectType.@number; } }
    public bool IsBoolean { get { return this.TypeValue == EasyObjectType.@boolean; } }
    public bool IsObject { get { return this.TypeValue == EasyObjectType.@object; } }
    public bool IsArray { get { return this.TypeValue == EasyObjectType.@array; } }
    public bool IsNull { get { return this.TypeValue == EasyObjectType.@null; } }

    private static object UnWrapInternal(object x)
    {
        while (x is EasyObject)
        {
            x = ((EasyObject)x).m_data;
        }
        return x;
    }

    private static EasyObject WrapInternal(object x)
    {
        if (x is EasyObject) return x as EasyObject;
        return new EasyObject(x);
    }

    public object UnWrap()
    {
        return EasyObject.UnWrapInternal(this);
    }

    public EasyObjectType TypeValue
    {
        get
        {
            object obj = UnWrapInternal(this);
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

    private List<EasyObject> list
    {
        get { return m_data as List<EasyObject>; }
    }

    private Dictionary<string, EasyObject> dictionary
    {
        get { return m_data as Dictionary<string, EasyObject>; }
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
        if (list == null) m_data = new List<EasyObject>();
        EasyObject eo = x is EasyObject ? x as EasyObject : new EasyObject(x);
        list.Add(eo);
        return this;
    }

    public EasyObject Add(string key, object x)
    {
        if (dictionary == null) m_data = new Dictionary<string, EasyObject>();
        EasyObject eo = x is EasyObject ? x as EasyObject : new EasyObject(x);
        dictionary.Add(key, eo);
        return this;
    }

    public override bool TryGetMember(
        GetMemberBinder binder, out object result)
    {
        result = Null;
        string name = binder.Name;
#if true
        if (list != null)
        {
            var assoc = TryAssoc(name);
            result = assoc;
        }
#endif
        if (dictionary == null) return true;
        EasyObject eo = Null;
        dictionary.TryGetValue(name, out eo);
        result = eo;
        return true;
    }

    public override bool TrySetMember(
        SetMemberBinder binder, object value)
    {
        value = UnWrapInternal(value);
        if (dictionary == null)
        {
            m_data = new Dictionary<string, EasyObject>();
        }
        string name = binder.Name;
        dictionary[name] = WrapInternal(value);
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
#if true
        if (list != null)
        {
            var assoc = TryAssoc((string)idx);
            result = assoc;
        }
#endif
        if (dictionary == null)
        {
            result = Null;
            return true;
        }
        EasyObject eo = Null;
        dictionary.TryGetValue((string)idx, out eo);
        result = eo;
        return true;
    }

    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
    {
        if (value is EasyObject) value = ((EasyObject)value).m_data;
        var idx = indexes[0];
        if (idx is int)
        {
            int pos = (int)indexes[0];
            if (pos < 0) throw new ArgumentException("index is below 0");
            if (list == null)
            {
                m_data = new List<EasyObject>();
            }
            while (list.Count < (pos + 1))
            {
                list.Add(null);
            }
            list[pos] = WrapInternal(value);
            return true;
        }
        if (dictionary == null)
        {
            m_data = new Dictionary<string, EasyObject>();
        }
        string name = (string)indexes[0];
        dictionary[name] = WrapInternal(value);
        return true;
    }

    public override bool TryConvert(ConvertBinder binder, out object result)
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
            result = Convert.ChangeType(m_data, binder.Type);
            return true;
        }
    }

    protected static string[] TextToLines(string text)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(text))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
        return lines.ToArray();
    }
    public static EasyObject FromObject(object obj)
    {
        return new EasyObject(obj);
    }

    public static EasyObject FromJson(string json)
    {
        if (json == null) return null;
        if (json.StartsWith("#!"))
        {
            string[] lines = TextToLines(json);
            lines = lines.Skip(1).ToArray(); ;
            json = String.Join("\n", lines);
        }
        return new EasyObject(JsonHandler.Parse(json));
    }

    public dynamic ToObject()
    {
        return new ObjectParser(false).Parse(m_data);
    }

    public string ToJson(bool indent = false, bool sort_keys = false)
    {
        return JsonHandler.Stringify(m_data, indent, sort_keys);
    }

#if false
    public static void AllocConsole()
    {
        WinConsole.Alloc();
    }
    public static void FreeConsole()
    {
        WinConsole.Free();
    }
    public static void ReallocConsole()
    {
        FreeConsole();
        AllocConsole();
    }
#endif

    public static string ToPrintable(object x, string title = null)
    {
        return ObjectParser.ToPrintable(ShowDetail, x, title);
    }

    public static void Echo(object x, string title = null)
    {
        String s = ToPrintable(x, title);
        Console.WriteLine(s);
        System.Diagnostics.Debug.WriteLine(s);
    }
    public static void Log(object x, string title = null)
    {
        String s = ToPrintable(x, title);
        Console.Error.WriteLine("[Log] " + s);
        System.Diagnostics.Debug.WriteLine("[Log] " + s);
    }
    public static void Debug(object x, string title = null)
    {
        if (!DebugOutput) return;
        String s = ToPrintable(x, title);
        Console.Error.WriteLine("[Debug] " + s);
        System.Diagnostics.Debug.WriteLine("[Debug] " + s);
    }
    public static void Message(object x, string title = null)
    {
        if (title == null) title = "Message";
        String s = ToPrintable(x, null);
        NativeMethods.MessageBoxW(IntPtr.Zero, s, title, 0);
    }
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBoxW(
            IntPtr hWnd, string lpText, string lpCaption, uint uType);
    }
    private EasyObject TryAssoc(string name)
    {
        try
        {
            for (int i = 0; i < list.Count; i++)
            {
                var pair = list[i].AsList;
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
            EasyObject eo = null;
            dictionary.TryGetValue(name, out eo);
            return eo;
        }
        set
        {
            if (dictionary == null)
            {
                m_data = new Dictionary<string, EasyObject>();
            }
            dictionary[name] = value;
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
                m_data = new List<EasyObject>();
            }
            while (list.Count < (pos + 1))
            {
                list.Add(null);
            }
            list[pos] = value;
        }
    }
    public T Cast<T>()
    {
        if (this.m_data is DateTime dt)
        {
            string s = null;
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
            return (T)Convert.ChangeType(s, typeof(T));
        }
        return (T)Convert.ChangeType(this.m_data, typeof(T));
    }
    public List<EasyObject> AsList
    {
        get
        {
#if false
            var result = new List<EasyObject>();
            if (list == null) return result;
            foreach(var item in list)
            {
                result.Add(WrapInternal(item));
            }
            return result;
#else
            return list;
#endif
        }
    }
    public Dictionary<string, EasyObject> AsDictionary
    {
        get
        {
#if false
            var result = new Dictionary<string, EasyObject>();
            if (dictionary == null) return result;
            foreach (var item in dictionary)
            {
                result[item.Key] = WrapInternal(item.Value);
            }
            return result;
#else
            return dictionary;
#endif
        }

    }

    public static string FullName(dynamic x)
    {
        if (x is null) return "null";
        string fullName = ((object)x).GetType().FullName;
        return fullName.Split('`')[0];
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
        this.m_data = null;
    }
}
