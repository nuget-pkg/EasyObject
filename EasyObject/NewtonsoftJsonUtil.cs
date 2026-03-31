#if MINIMAL
using EasyObject = Global.MiniEasyObject;
#endif
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework.Internal;
using Formatting = Newtonsoft.Json.Formatting;
// ReSharper disable CheckNamespace
namespace Global;
public partial class NewtonsoftJsonUtil {
    public static string SerializeToJson(dynamic? x, bool indent = false) {
        x = EasyObject.FromObject(x).ToObject(asDynamicObject: false);
        return JsonConvert.SerializeObject(x, indent ? Formatting.Indented : Formatting.None);
    }
    public static EasyObject DeserializeFromJson(string json) {
        var result = JsonConvert.DeserializeObject(json, new JsonSerializerSettings {
            DateParseHandling = DateParseHandling.None
        });
        return ParseNewtonsoftJson(result);
    }
    public static T? DeserializeFromJson<T>(string json, T? fallback = default(T)) {
        if (String.IsNullOrEmpty(json)) return fallback;
        return JsonConvert.DeserializeObject<T>(json);
    }
    public static EasyObject FromJsonFile(string filePath) {
        return DeserializeFromJson(File.ReadAllText(filePath));
    }
    public static byte[] SerializeToToBson(dynamic? x) {
        x = EasyObject.FromObject(x).ToObject(asDynamicObject: false);
        MemoryStream ms = new MemoryStream();
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
        using (BsonWriter writer = new BsonWriter(ms)) {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, x);
        }
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
        return ms.ToArray();
    }
    public static EasyObject DeserializeFromFromBson(byte[] bson) {
        MemoryStream ms = new MemoryStream(bson);
#pragma warning disable IDE0063 // 単純な 'using' ステートメントを使用する
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
        using (BsonReader reader = new BsonReader(ms)) {
            JsonSerializer serializer = new JsonSerializer();
            return EasyObject.FromObject(serializer.Deserialize(reader));
        }
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
#pragma warning restore IDE0063 // 単純な 'using' ステートメントを使用する
    }
    public static T? DeserializeFromFromBson<T>(byte[] bson) {
        MemoryStream ms = new MemoryStream(bson);
#pragma warning disable IDE0063 // 単純な 'using' ステートメントを使用する
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
        using (BsonReader reader = new BsonReader(ms)) {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<T>(reader);
        }
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
#pragma warning restore IDE0063 // 単純な 'using' ステートメントを使用する
    }
    public static EasyObject DeserializeFromObject(dynamic? x) {
        if (x == null) return EasyObject.Null;
        var o = (dynamic)JObject.FromObject(new { x = x },
            new JsonSerializer {
                DateParseHandling = DateParseHandling.None
            });
        return EasyObject.FromObject(o.x);
    }
    public static T? DeserializeFromObject<T>(dynamic? x) {
        dynamic? o = DeserializeFromObject(x);
        if (o == null) return default(T);
        return (T)(o.ToObject<T>());
    }
    public static string SerializeToToXml(dynamic? x) {
        if (x == null) return "<null />";
        if (x is System.Xml.Linq.XElement xelem) {
            return (xelem).ToString();
        }
        XDocument? doc;
        if (x is System.Xml.Linq.XDocument xdoc) {
            doc = xdoc;
        }
        else {
            x = EasyObject.FromObject(x).ToObject(asDynamicObject: false);
            string json = SerializeToJson(x);
            doc = JsonConvert.DeserializeXmlNode(json)?.ToXDocument();
        }
        return doc == null ? "<null />" : doc.ToStringWithDeclaration();
    }
    public static EasyObject DeserializeFromXml(string xml) {
        XElement statusElement = XElement.Parse(xml); // Use XElement.Parse()
        XmlDocument doc = new XmlDocument();
        XmlNode? xmlNode = doc.ReadNode(statusElement.CreateReader());
        if (xmlNode == null) return EasyObject.Null;
        string json = JsonConvert.SerializeXmlNode(xmlNode, Formatting.Indented);
        return EasyObject.FromJson(json);
    }
    private static EasyObject ParseNewtonsoftJson(dynamic? x) {
        if (x == null) return EasyObject.Null;
        if (x is JArray jarray) {
            List<object> array = jarray.ToObject<List<object>>()!;
            var result = EasyObject.NewArray();
            foreach (var item in array) {
                result.Add(ParseNewtonsoftJson(item));
            }
            return result;
        }
        if (x is JObject jobject) {
            Dictionary<string, object> dict = jobject.ToObject<Dictionary<string, object>>()!;
            var result = EasyObject.NewObject();
            var keys = dict.Keys;
            foreach (var key in keys) {
                result.Add(key, ParseNewtonsoftJson(dict[key]));
            }
            return result;
        }
        return x;
    }
}