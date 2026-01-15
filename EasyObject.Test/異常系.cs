using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;

public class 異常系
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
    public void Test01()
    {
        ShowDetail = true;
        EasyObject eo = Null;
        Echo(eo);
        Assert.That(eo[2].IsNull, Is.True);
        var a = eo["a"];
        Assert.That(eo["a"].IsNull, Is.True);
    }
}