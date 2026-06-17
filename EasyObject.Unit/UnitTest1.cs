using T = Global.EasyObject;
using static Global.EasyObject;

[TestClass]
public class UnitTest
{
    [TestMethod]
    public void Test01()
    {
        Pass();
        ShowDetail = true;
        var eo = Global.EasyObject.FromObject(new { a = 123 });
        Echo(eo, "eo");
        Assert.AreEqual("""
            {"a":123}
            """, eo.ToJson());
        Pass();
    }
    [TestMethod]
    public void Test02()
    {
        Pass();
        ShowDetail = true;
        var eo = Global.EasyObject.FromObject("helloハロー©");
        Echo(eo, "eo");
        Assert.AreEqual(actual: eo.ToJson(), expected:"""
            "helloハロー©"
            """);
        //EasyObject.JsonParser = new CSharpJsonHandler(numberAsDecimal: true); // ForceASCII
        Global.EasyObject.ForceAscii = true;
        Assert.AreEqual(actual: eo.ToJson(), expected: """
            "hello\u30CF\u30ED\u30FC\u00A9"
            """);
        Pass();
    }
    [TestMethod]
    public void Test03()
    {
        Pass();
        ShowDetail = true;

        var ary = Null.Add(11).Add("abc");
        Echo(ary, "ary");
        Assert.AreEqual(@array, ary.TypeValue);
        Assert.AreEqual("@number", ary[0].TypeName);
        Assert.AreEqual("@string", ary[1].TypeName);
        Assert.AreEqual(2, ary.Count);

        var dic = Null.Add("a", 11).Add("b", "abc");
        Echo(dic, "dic");
        Assert.AreEqual(@object, dic.TypeValue);
        Assert.AreEqual("@number", dic["a"].TypeName);
        Assert.AreEqual("@string", dic["b"].TypeName);

        ary = EmptyArray;
        Assert.AreEqual(@array, ary.TypeValue);
        Assert.AreEqual(0, ary.Count);

        dic = EmptyObject;
        Assert.AreEqual(@object, dic.TypeValue);
        Assert.AreEqual(0, dic.Count);

        var eo = Global.EasyObject.FromObject(new { a = 123 });
        Echo(eo.TypeValue, "eo.TypeValue");
        Assert.AreEqual(@object, eo.TypeValue);
        Console.WriteLine(eo["a"]);
        Assert.AreEqual(123, eo["a"].Cast<int>());
        Assert.IsTrue(eo.Keys.SequenceEqual(new List<string> { "a" }));
        foreach (var pair in (dynamic)eo)
        {
            Echo(pair, "pair");
            Echo(FullName(pair), "FullName(pair)");
            Assert.IsTrue(FullName(pair) == "System.Collections.Generic.KeyValuePair");
        }
        eo.Nullify();
        Echo(eo.TypeValue, "eo.EasyType");
        Assert.AreEqual(@null, eo.TypeValue);
        Assert.IsTrue(eo.IsNull);
        eo["b"] = true;
        Assert.AreEqual(1, eo.Count);
        Assert.IsTrue(eo["b"].Cast<bool>());
        Echo(eo["b"].TypeValue, "eo.b.TypeValue");
        Assert.AreEqual(@boolean, eo["b"].TypeValue);
        eo[3] = 777;
        Echo(eo[3].Cast<int>());
        Echo(eo.TypeValue, "eo.EasyType");
        Assert.AreEqual(@array, eo.TypeValue);
        Assert.AreEqual(4, eo.Count);
        Assert.AreEqual(@null, eo[0].TypeValue);
        Pass();
        Assert.Throws<System.InvalidCastException>(() => { var n = eo[0].Cast<int>(); });
        Pass();
        Assert.AreEqual(777, eo[3].Cast<int>());
        foreach (var e in (dynamic)eo)
        {
            Echo(e, "e");
        }
        var eo2 = Global.EasyObject.FromObject(eo);
        Global.EasyObject eo3 = Global.EasyObject.FromJson("""
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
        Pass();
    }
    [TestMethod]
    public void Test04()
    {
        Pass();
        ShowDetail = true;
        Global.EasyObject eo = new DateTime(0);
        Assert.AreEqual(Global.EasyObject.@string, eo.TypeValue);
        string print = ToPrintable(eo);
        //Assert.True(print == """
        //    `0001-01-01T00:00:00.0000000`
        //    """));
        Assert.AreEqual("""
            <Global.EasyObject(System.String)> "0001-01-01T00:00:00.0000000"
            """, print);
        // <Global.EasyObject(System.String)> "0001-01-01T00:00:00.0000000"
        string s = eo.Cast<string>();
        Assert.AreEqual("""
            0001-01-01T00:00:00.0000000
            """, s);
        eo = Guid.Empty;
        Assert.AreEqual(Global.EasyObject.@string, eo.TypeValue);
        s = eo.Cast<string>();
        Assert.AreEqual("""
            00000000-0000-0000-0000-000000000000
            """, s);
        eo = new TimeSpan(1000);
        Assert.AreEqual(Global.EasyObject.@string, eo.TypeValue);
        s = eo.Cast<string>();
        Assert.AreEqual("""
            00:00:00.0001000
            """, s);
        Pass();
    }
    [TestMethod]
    public void Test05()
    {
        Pass();
        ShowDetail = true;
        Echo(Null);
        Echo(DateTime.Now);
        Echo(new { a = 123 });
        Echo(FromObject(Null));
        Echo(FromObject(DateTime.Now));
        Echo(FromObject(new { a = 123 }));
        Pass();
    }
    [TestMethod]
    public void Test06()
    {
        Pass();
        ShowDetail = true;
        var eo = Global.EasyObject.FromJson("""
            { "a": 123 }
            """);
        Echo(eo, "eo");
        Assert.AreEqual(actual: eo.ToJson(), expected: """
            {"a":123}
            """);
        eo = Global.EasyObject.FromJson("""
            [11, 22, '33']
            """);
        Echo(eo, "eo");
        Assert.AreEqual(actual: eo.ToJson(), expected: """
            [11,22,"33"]
            """);
        Pass();
    }
}