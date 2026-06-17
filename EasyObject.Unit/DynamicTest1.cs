using System;
using T = Global.EasyObject;
using static Global.EasyObject;

[TestClass]
public class DynamicTest
{
    [TestMethod]
    public void Setup()
    {
        Console.WriteLine("Setup() called");
        ClearSettings();
    }

    [TestMethod]
    public void Test01()
    {
        Pass();
        ShowDetail = true;
        Global.EasyObject eo = "abc";
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