using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;
//using NUnit.Framework;

public class DynamicTest
{
    [NUnit.Framework.SetUp]
    public void Setup()
    {
        Console.WriteLine("Setup() called");
        ClearSettings();
    }

    [NUnit.Framework.Test]
    public void Test01()
    {
        Pass();
        ShowDetail = true;
        EasyObject eo = "abc";
        Echo(eo, "eo");
        string s = eo.Dynamic;
        Echo(s, "s");
        AssertIdentical(s, "abc");
        eo.Dynamic.A = "AAA";
        Echo(eo, "eo");
        AssertIdentical(eo.TypeValue, @object);
        Console.WriteLine(eo);
        foreach(var e in eo.Dynamic)
        {
            Echo(e, "e");
            AssertIdentical(e.Key, "A");
            AssertIdentical(e.Value.Cast<string>(), "AAA");
            string ss = e.Value.Dynamic;
            AssertIdentical(ss, "AAA");
            AssertIdentical((string)(e.Value.Dynamic), "AAA");
        }
        Pass();
    }
}