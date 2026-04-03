using NUnit.Framework;
using System;
using static Global.EasyObject;
// ReSharper disable CheckNamespace
namespace Global.EasyObjectTest;
internal class AssertCapabilitiesTest {
    [SetUp]
    public void Setup() {
        ClearSettings();
        Echo("abc", "def");
        Log(FullName(this));
        //NUnitLog(TestContext.CurrentContext.Test.FullName);
    }
    [Test]
    public void Test901()
    {
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
        foreach (var e in eo.Dynamic)
        {
            Echo(e, "e");
            AssertIdentical(e.Key, "A");
            AssertIdentical(e.Value.Cast<string>(), "AAA");
            string ss = e.Value.Dynamic;
            AssertIdentical(ss, "AAA");
            AssertIdentical((string)(e.Value.Dynamic), "AAA");
        }
        var list0 = NewArray("A", "B", "C");
        var list1 = list0.AsStringArray;
        var list2 = list0.AsStringList;
        AssertIdentical(list1, list2);
        AssertEquivalent(list0, list1);
        AssertIdentical(list1, new object[] { "A", "B", "C" });
        var dict0 = NewObject("A", 11, "B", 22, "C", null);
        var dict1 = dict0.ToObject(asDynamicObject: false);
        var dict2 = dict0.ToObject(asDynamicObject: true);
        AssertIdentical(dict1, dict2);
        AssertEquivalent(dict0, dict1);
        Log("pass-01");
        AssertEquivalent(dict1, new { A = 11, B = 22, C = Null });
        Log("pass-02");
        // /*⁅FAILS⁆*/ AssertIdentical(dict1, new { A = 11, B = 22, C = Null });
        //Log("pass-03");
    }
    [Test]
    public void Test902()
    {
        ShowDetail = true;
        Line();
        EasyObject eo = NewArray("a", "b", "c");
        //Line();
        //Echo(eo, maxCount: 2, title: "1");
        //Line();
        //eo.Dump(maxCount: 2);
        //Line();
        //EasyObject.Dump(eo, maxCount: 2);
        //Line();
        eo.Trim(maxCount: 2);
        Line();
        Echo(eo);
    }
}