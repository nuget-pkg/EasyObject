using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;

public class DynamicTest
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
        EasyObject eo = "abc";
        Echo(eo, "eo");
        string s = eo.Dynamic;
        Echo(s, "s");
        Assert.That(s, Is.EqualTo("abc"));
        eo.Dynamic.A = "AAA";
        Echo(eo, "eo");
        Assert.That(eo.TypeValue, Is.EqualTo(@object));
        Console.WriteLine(eo);
        foreach(var e in eo.Dynamic)
        {
            Echo(e, "e");
            Assert.That(e.Key, Is.EqualTo("A"));
            Assert.That(e.Value.Cast<string>(), Is.EqualTo("AAA"));
            string ss = e.Value.Dynamic;
            Assert.That(ss, Is.EqualTo("AAA"));
            Assert.That((string)(e.Value.Dynamic), Is.EqualTo("AAA"));
        }
    }
}