using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;

public class JSON書き出しテスト
{
    [SetUp]
    public void Setup()
    {
        Console.WriteLine("Setup() called");
        ClearSettings();
    }

    protected string RenderJson(EasyObject eo)
    {
        return eo.ToJson(indent: false, sort_keys: true);
    }

    [Test]
    public void 単純な辞書()
    {
        ShowDetail = true;
        var eo = FromObject(new { a = 123, b = "abc" });
        Assert.That(RenderJson(eo), Is.EqualTo("""
            {"a":123,"b":"abc"}
            """));
        eo["c"] = 777;
        Assert.That(RenderJson(eo), Is.EqualTo("""
            {"a":123,"b":"abc","c":777}
            """));
        Dictionary<string, EasyObject> dict = eo.AsDictionary;
        dict["d"] = 888;
        Assert.That(RenderJson(eo), Is.EqualTo("""
            {"a":123,"b":"abc","c":777,"d":888}
            """));
        eo[2] = 111;
        Assert.That(RenderJson(eo), Is.EqualTo("""
            [null,null,111]
            """));
    }
    [Test]
    public void 単純なリスト()
    {
        ShowDetail = true;
        var eo = FromObject(new object[] { 123, "abc" });
        Assert.That(RenderJson(eo), Is.EqualTo("""
            [123,"abc"]
            """));
        eo.Add(true);
        Assert.That(RenderJson(eo), Is.EqualTo("""
            [123,"abc",true]
            """));
        eo["c"] = 777;
        Assert.That(RenderJson(eo), Is.EqualTo("""
            {"c":777}
            """));
    }
}