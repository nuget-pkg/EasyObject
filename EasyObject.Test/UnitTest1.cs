using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        Console.WriteLine("Setup() called");
        ClearSettings();
    }

    [Test]
    public void Test01()
    {
        ShowDetail = true;
        var eo = EasyObject.FromObject(new { a = 123 });
        Echo(eo, "eo");
        Assert.That(eo.ToJson(), Is.EqualTo("""
            {"a":123}
            """));
    }
    [Test]
    public void Test02()
    {
        ShowDetail = true;
        var eo = EasyObject.FromObject("helloハロー©");
        Echo(eo, "eo");
        Assert.That(eo.ToJson(), Is.EqualTo("""
            "helloハロー©"
            """));
        EasyObject.JsonHandler = new CSharpJsonHandler(true, true); // ForceASCII
        Assert.That(eo.ToJson(), Is.EqualTo("""
            "hello\u30CF\u30ED\u30FC\u00A9"
            """));
    }
    [Test]
    public void Test03()
    {
        ShowDetail = true;

        var ary = Null.Add(11).Add("abc");
        Echo(ary, "ary");
        Assert.That(ary.TypeValue, Is.EqualTo(@array));
        Assert.That(ary[0].TypeName, Is.EqualTo("number"));
        Assert.That(ary[1].TypeName, Is.EqualTo("string"));
        Assert.That(ary.Count, Is.EqualTo(2));

        var dic = Null.Add("a", 11).Add("b", "abc");
        Echo(dic, "dic");
        Assert.That(dic.TypeValue, Is.EqualTo(@object));
        Assert.That(dic["a"].TypeName, Is.EqualTo("number"));
        Assert.That(dic["b"].TypeName, Is.EqualTo("string"));

        ary = EmptyArray;
        Assert.That(ary.TypeValue, Is.EqualTo(@array));
        Assert.That(ary.Count, Is.EqualTo(0));

        dic = EmptyObject;
        Assert.That(dic.TypeValue, Is.EqualTo(@object));
        Assert.That(dic.Count, Is.EqualTo(0));

        var eo = EasyObject.FromObject(new { a = 123 });
        Echo(eo.TypeValue, "eo.TypeValue");
        Assert.That(eo.TypeValue, Is.EqualTo(@object));
        Console.WriteLine(eo["a"]);
        Assert.That(eo["a"].Cast<int>(), Is.EqualTo(123));
        Assert.That(eo.Keys, Is.EqualTo(new List<string> { "a" }));
        foreach (var pair in (dynamic)eo)
        {
            Echo(pair, "pair");
            Echo(FullName(pair), "FullName(pair)");
            Assert.That(FullName(pair), Is.EqualTo("System.Collections.Generic.KeyValuePair"));
        }
        eo.Nullify();
        Echo(eo.TypeValue, "eo.EasyType");
        Assert.That(eo.TypeValue, Is.EqualTo(@null));
        Assert.That(eo.IsNull, Is.EqualTo(true));
        eo["b"] = true;
        Assert.That(eo.Count, Is.EqualTo(1));
        Assert.That(eo["b"].Cast<bool>(), Is.EqualTo(true));
        Echo(eo["b"].TypeValue, "eo.b.TypeValue");
        Assert.That(eo["b"].TypeValue, Is.EqualTo(@boolean));
        eo[3] = 777;
        Echo(eo[3].Cast<int>());
        Echo(eo.TypeValue, "eo.EasyType");
        Assert.That(eo.TypeValue, Is.EqualTo(@array));
        Assert.That(eo.Count, Is.EqualTo(4));
        Assert.That(eo[0].TypeValue, Is.EqualTo(@null));
        Assert.That(() => { var n = eo[0].Cast<int>(); },
            Throws.TypeOf<System.InvalidCastException>()
            .With.Message.EqualTo("Null オブジェクトを値型に変換することはできません。")
            );
        Assert.That(eo[3].Cast<int>(), Is.EqualTo(777));
        foreach (var e in (dynamic)eo)
        {
            Echo(e, "e");
        }
        var eo2 = EasyObject.FromObject(eo);
        EasyObject eo3 = EasyObject.FromJson("""
            { a: 123, b: [11, 22, 33] }
            """);
        Echo(eo3, "eo3");
        Echo(eo3["b"][1]);
        List<int> list = new List<int>();
        foreach (var e in (dynamic)eo3["b"]) list.Add((int)e);
        Echo(list, "list");
        Echo(eo3["b"].TypeName);
        Echo(eo3["b"].IsArray);
        Echo(eo3["b"].IsNull);
        eo3["b"].Add(777);
        Echo(eo3);
        var i = FromObject(123);
        Echo(i.Cast<double>());
        var iList1 = eo3["b"].AsList.Select(x => x.Cast<int>()).ToList();
        Echo(iList1.GetType().FullName);
        var dict = eo3.AsDictionary;
        Echo(dict);
        Echo(dict["a"].Cast<double>());
        foreach (var e in (dynamic)i)
        {
            Echo(e);
        }
    }
    [Test]
    public void Test04()
    {
        ShowDetail = true;
        EasyObject eo = new DateTime(0);
        Assert.That(eo.TypeValue, Is.EqualTo(EasyObject.@string));
        string print = ToPrintable(eo);
        Assert.That(print, Is.EqualTo("""
            `0001-01-01T00:00:00.0000000`
            """));
        string s = eo.Cast<string>();
        Assert.That(s, Is.EqualTo("""
            0001-01-01T00:00:00.0000000
            """));
        eo = Guid.Empty;
        Assert.That(eo.TypeValue, Is.EqualTo(EasyObject.@string));
        s = eo.Cast<string>();
        Assert.That(s, Is.EqualTo("""
            00000000-0000-0000-0000-000000000000
            """));
        eo = new TimeSpan(1000);
        Assert.That(eo.TypeValue, Is.EqualTo(EasyObject.@string));
        s = eo.Cast<string>();
        Assert.That(s, Is.EqualTo("""
            00:00:00.0001000
            """));
    }
    [Test]
    public void Test05()
    {
        ShowDetail = true;
        Echo(Null);
        Echo(DateTime.Now);
        Echo(new { a = 123 });
        Echo(FromObject(Null));
        Echo(FromObject(DateTime.Now));
        Echo(FromObject(new { a = 123 }));
    }
    [Test]
    public void Test06()
    {
        ShowDetail = true;
        var eo = EasyObject.FromJson("""
            { "a": 123 }
            """);
        Echo(eo, "eo");
        Assert.That(eo.ToJson(), Is.EqualTo("""
            {"a":123}
            """));
        eo = EasyObject.FromJson("""
            [11, 22, '33']
            """);
        Echo(eo, "eo");
        Assert.That(eo.ToJson(), Is.EqualTo("""
            [11,22,"33"]
            """));
    }
}