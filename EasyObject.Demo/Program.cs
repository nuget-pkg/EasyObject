using Global;
using static Global.EasyObject;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using Newtonsoft.Json.Linq;
using Razorvine.Pickle;

class Program
{
    static void Main()
    {
        ShowDetail = true;
        Echo(Null);
        var eo = EasyObject.FromObject(new { a = 123 });
        Echo(eo.TypeValue, "eo.TypeValue");
        Assert.That(eo.TypeValue, Is.EqualTo(@object));
        Console.WriteLine(eo["a"]);
        Assert.That(eo["a"].Cast<int>(), Is.EqualTo(123));
        Assert.That(eo.Keys, Is.EqualTo(new List<string> { "a" }));
        Echo(eo[0], "eo[0]");
        Assert.That(eo[0].TypeValue, Is.EqualTo(@null));
        Assert.That(eo[0].IsNull, Is.True);
        Echo(eo[1], "eo[1]");
        Assert.That(eo[1].TypeValue, Is.EqualTo(@null));
        Assert.That(eo[1].IsNull, Is.True);
        foreach (var pair in eo.Dynamic)
        {
            Echo(pair, "pair");
        }
        eo = EasyObject.FromObject(null);
        Echo(eo.TypeValue, "eo.TypeValue");
        Assert.That(eo.TypeValue, Is.EqualTo(@null));
        eo["b"] = true;
        Assert.That(eo["b"].Cast<bool>(), Is.EqualTo(true));
        Echo(eo["b"].TypeValue, "eo.b.TypeValue");
        Assert.That(eo["b"].TypeValue, Is.EqualTo(@boolean));
        eo[3] = 777;
        Echo(eo[3].Cast<int>());
        Echo(eo.TypeValue, "eo.TypeValue");
        Assert.That(eo.TypeValue, Is.EqualTo(EasyObject.array));
        Assert.That(eo.Count, Is.EqualTo(4));
        Assert.That(eo[0].TypeValue, Is.EqualTo(@null));
#if false
        Assert.That(() => { var n = eo[0].Cast<int>(); },
            Throws.TypeOf<System.InvalidCastException>()
            .With.Message.EqualTo("Null オブジェクトを値型に変換することはできません。")
            );
#endif
        //Assert.That((int?)eo[0], Is.EqualTo(null));
        Assert.That(eo[3].Cast<int>(), Is.EqualTo(777));
        foreach(var e in eo.Dynamic)
        {
            Echo(e, "e");
        }
        var eo2 = EasyObject.FromObject(eo); // UnWrap() test
        EasyObject eo3 = EasyObject.FromJson("""
            { a: 123, b: [11, 22, 33] }
            """);
        Echo(eo3, "eo3");
        Echo(eo3["b"][1]);
        List<int> list = new List<int>();
        foreach(var e in eo3["b"].Dynamic) list.Add((int)e);
        Echo(list, "list");
        Echo(eo3["b"].TypeName);
        Echo(eo3["b"].IsArray);
        Echo(eo3["b"].IsNull);
        eo3["b"].Add(777);
        eo3.AsDictionary["b"].Add(888);
        Echo(eo3);
        var i = FromObject(123);
        Echo(i.Cast<double>());
        var iList1 = eo3["b"].AsList.Select(x => x.Cast<int>()).ToList();
        Echo(iList1.GetType().FullName);
        Echo(iList1.GetType().ToString());
        var dict = eo3.AsDictionary;
        Echo(dict);
        Echo(dict["a"].Cast<double>());
        foreach(var e in i.Dynamic)
        {
            Echo(e);
        }
        string bigJson = File.ReadAllText("assets/qiita-9ea0c8fd43b61b01a8da.json");
        //Echo(bigJson);
        var sw = new System.Diagnostics.Stopwatch();
        TimeSpan ts;
        sw.Start();
        for (int c = 0; c < 5; c++)
        {
            //var test = FromJson(bigJson);
        }
        sw.Stop();
        Console.WriteLine("■EasyObject");
        ts = sw.Elapsed;
        Console.WriteLine($"　{ts}");
        Console.WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
        Console.WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");
        sw.Start();
        for (int c = 0; c < 5; c++)
        {
            //JObject jsonObject = JObject.Parse(bigJson);
        }
        sw.Stop();
        Console.WriteLine("■Newtonsoft.Json");
        ts = sw.Elapsed;
        Console.WriteLine($"　{ts}");
        Console.WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
        Console.WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");
        //var list01_txt = File.ReadAllText("assets/list01.txt");
        var list01_txt = File.ReadAllText("assets/mydict.txt");
        Echo(list01_txt);
        var list01_bytes = Convert.FromBase64String(list01_txt);
        var unpickler = new Unpickler();
        object result = unpickler.loads(list01_bytes);
        Echo(result, "result");
        var o = new ObjectParser(false).Parse(result);
        Echo(o, "o");
        var pickler = new Pickler();
        var bytes = pickler.dumps(o);
        var ox = unpickler.loads(bytes);
        Echo(ox, "ox");
        var eo_ox = EasyObject.FromObject(ox);
        Echo(eo_ox, "eo_ox");
        Echo(eo_ox.ToJson(true, true), "eo_ox.ToJson(true, true)");
        Echo(DateTime.Now);

        string progJson = """
            #! /usr/bin/env program
            [11, null, "abc"]
            """;
        Echo(EasyObject.FromJson(progJson));
        Echo(EasyObject.FromJson(null));
        var array = EasyObject.NewArray(1, null, "abc", EasyObject.FromJson(progJson));
        Echo(array, "array");
        var obj = EasyObject.NewObject("a", 111, "b", EasyObject.FromJson(progJson));
        Echo(obj, "obj");
        // Test newLisp expression
        EasyObject assocList = EasyObject.FromJson("""
            ( ("a" 123) ("b" true) ("c" false) ("d" nil) )
            """);
        Echo(assocList, "assocList");
        var member = assocList["a"];
        Echo(member, "member");
        dynamic assocDyn = assocList;
        var member2 = assocDyn["a"];
        Echo(member2, "member2");
        var member3 = assocDyn.a;
        Echo(member3, "member3");
        Log("[END]");
    }
}