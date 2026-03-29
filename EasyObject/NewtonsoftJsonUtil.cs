using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
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
        return EasyObject.FromObject(result);
    }
    public static T? DeserializeFromJson<T>(string json, T? fallback = default(T)) {
        if (String.IsNullOrEmpty(json)) return fallback;
        return JsonConvert.DeserializeObject<T>(json);
    }
    public static byte[] SerializeToToBson(dynamic? x) {
        x = EasyObject.FromObject(x).ToObject(asDynamicObject: false);
        MemoryStream ms = new MemoryStream();
        using (BsonWriter writer = new BsonWriter(ms)) {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, x);
        }
        return ms.ToArray();
    }
    public static EasyObject DeserializeFromFromBson(byte[] bson) {
        MemoryStream ms = new MemoryStream(bson);
        using (BsonReader reader = new BsonReader(ms)) {
            JsonSerializer serializer = new JsonSerializer();
            return EasyObject.FromObject(serializer.Deserialize(reader));
        }
    }
    public static T? DeserializeFromFromBson<T>(byte[] bson) {
        MemoryStream ms = new MemoryStream(bson);
        using (BsonReader reader = new BsonReader(ms)) {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<T>(reader);
        }
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
}