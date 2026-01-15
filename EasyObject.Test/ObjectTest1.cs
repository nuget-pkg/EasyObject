using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;

public class ObjectTest
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
        EasyObject eo = EasyObject.FromObject(new { a=1, b=2 });
        Echo(eo, "eo");
        Assert.That(eo.ContainsKey("a"), Is.True);
        Assert.That(eo.ContainsKey("c"), Is.False);
    }
}