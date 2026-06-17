using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using T = Global.EasyObject;
using static Global.EasyObject;

public class UnitTest
{
    private readonly ITestOutputHelper Out;
    public UnitTest(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
        T.ClearSettings();
        T.ShowDetail = true;
        T.EchoRedirector = Out.WriteLine;
        T.LogRedirector = Out.WriteLine;
        T.Log("Setup() called");
    }

    [Fact]
    public void Test01()
    {
        Pass();
        ShowDetail = true;
        var eo = Global.EasyObject.FromObject(new { a = 123 });
        Echo(eo, "eo");
        Assert.True(eo.ToJson() == """
            {"a":123}
            """);
        Pass();
    }
    [Fact]
    public void Test02()
    {
        Pass();
        ShowDetail = true;
        var eo = Global.EasyObject.FromObject("helloハロー©");
        Echo(eo, "eo");
        Assert.Equal(actual: eo.ToJson(), expected:"""
            "helloハロー©"
            """);
        //EasyObject.JsonParser = new CSharpJsonHandler(numberAsDecimal: true); // ForceASCII
        Global.EasyObject.ForceAscii = true;
        Assert.Equal(actual: eo.ToJson(), expected: """
            "hello\u30CF\u30ED\u30FC\u00A9"
            """);
        Pass();
    }
    [Fact]
    public void Test03()
    {
        Pass();
        ShowDetail = true;

        var ary = Null.Add(11).Add("abc");
        Echo(ary, "ary");
        Assert.True(ary.TypeValue == @array);
        Assert.True(ary[0].TypeName == "@number");
        Assert.True(ary[1].TypeName == "@string");
        Assert.True(ary.Count == 2);

        var dic = Null.Add("a", 11).Add("b", "abc");
        Echo(dic, "dic");
        Assert.True(dic.TypeValue == @object);
        Assert.True(dic["a"].TypeName == "@number");
        Assert.True(dic["b"].TypeName == "@string");

        ary = EmptyArray;
        Assert.True(ary.TypeValue == @array);
        Assert.True(ary.Count == 0);

        dic = EmptyObject;
        Assert.True(dic.TypeValue == @object);
        Assert.True(dic.Count == 0);

        var eo = Global.EasyObject.FromObject(new { a = 123 });
        Echo(eo.TypeValue, "eo.TypeValue");
        Assert.True(eo.TypeValue == @object);
        Console.WriteLine(eo["a"]);
        Assert.True(eo["a"].Cast<int>() == 123);
        Assert.True(eo.Keys.SequenceEqual(new List<string> { "a" }));
        foreach (var pair in (dynamic)eo)
        {
            Echo(pair, "pair");
            Echo(FullName(pair), "FullName(pair)");
            Assert.True(FullName(pair) == "System.Collections.Generic.KeyValuePair");
        }
        eo.Nullify();
        Echo(eo.TypeValue, "eo.EasyType");
        Assert.True(eo.TypeValue == @null);
        Assert.True(eo.IsNull == true);
        eo["b"] = true;
        Assert.True(eo.Count == 1);
        Assert.True(eo["b"].Cast<bool>() == true);
        Echo(eo["b"].TypeValue, "eo.b.TypeValue");
        Assert.True(eo["b"].TypeValue == @boolean);
        eo[3] = 777;
        Echo(eo[3].Cast<int>());
        Echo(eo.TypeValue, "eo.EasyType");
        Assert.True(eo.TypeValue == @array);
        Assert.True(eo.Count == 4);
        Assert.True(eo[0].TypeValue == @null);
        Pass();
        Assert.Throws<System.InvalidCastException>(() => { var n = eo[0].Cast<int>(); });
        Pass();
        Assert.True(eo[3].Cast<int>() == 777);
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
    [Fact]
    public void Test04()
    {
        Pass();
        ShowDetail = true;
        Global.EasyObject eo = new DateTime(0);
        Assert.True(eo.TypeValue == Global.EasyObject.@string);
        string print = ToPrintable(eo);
        //Assert.True(print == """
        //    `0001-01-01T00:00:00.0000000`
        //    """));
        Assert.True(print == """
            <Global.EasyObject(System.String)> "0001-01-01T00:00:00.0000000"
            """);
        // <Global.EasyObject(System.String)> "0001-01-01T00:00:00.0000000"
        string s = eo.Cast<string>();
        Assert.True(s == """
            0001-01-01T00:00:00.0000000
            """);
        eo = Guid.Empty;
        Assert.True(eo.TypeValue == Global.EasyObject.@string);
        s = eo.Cast<string>();
        Assert.True(s == """
            00000000-0000-0000-0000-000000000000
            """);
        eo = new TimeSpan(1000);
        Assert.True(eo.TypeValue == Global.EasyObject.@string);
        s = eo.Cast<string>();
        Assert.True(s == """
            00:00:00.0001000
            """);
        Pass();
    }
    [Fact]
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
    [Fact]
    public void Test06()
    {
        Pass();
        ShowDetail = true;
        var eo = Global.EasyObject.FromJson("""
            { "a": 123 }
            """);
        Echo(eo, "eo");
        Assert.Equal(actual: eo.ToJson(), expected: """
            {"a":123}
            """);
        eo = Global.EasyObject.FromJson("""
            [11, 22, '33']
            """);
        Echo(eo, "eo");
        Assert.Equal(actual: eo.ToJson(), expected: """
            [11,22,"33"]
            """);
        Pass();
    }
}