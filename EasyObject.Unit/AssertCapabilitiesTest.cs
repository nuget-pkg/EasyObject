using System;
using T = Global.EasyObject;

using static Global.EasyObject;
// ReSharper disable CheckNamespace
namespace Global.EasyObjectTest;

[TestClass]
public class AssertCapabilitiesTest
{
    public AssertCapabilitiesTest()
    {
        LogRedirector = (s) => Console.WriteLine(s);
    }
    [TestMethod]
    public void Test901()
    {
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
        ShowDetail = true;
        EasyObject eo = "abc";
        Log(eo, "eo");
        string s = eo.Dynamic;
        Log(s, "s");
        AssertIdentical(s, "abc");
        eo.Dynamic.A = "AAA";
        Log(eo, "eo");
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
        AssertIdentical(list0, list1);
        AssertIdentical(list1, new object[] { "A", "B", "C" });
        var dict0 = NewObject("A", 11, "B", 22, "C", null);
        var dict1 = dict0.ToObject(asDynamicObject: false);
        var dict2 = dict0.ToObject(asDynamicObject: true);
        AssertIdentical(dict1, dict2);
        Log("pass-01");
        AssertIdentical(dict1, new { A = 11, B = 22, C = Null });
        Log("pass-02");
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
    [TestMethod]
    public void Test902()
    {
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
        ShowDetail = true;
        DebugOutput = true;
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
        EasyObject eo = NewArray(NewArray(NewObject("a", NewObject("b", 222, "d", 111), "b", 20, "c", 30), 11, 22, 33), "a", "b", "c");
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Log(eo, maxCount: 2, hideKeys: ["b"], title: "1");
        eo.Trim(maxCount: 2, hideKeys: ["b"], maxDepth: 3);
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
        AssertIdentical(actual: eo.ToJson(), expected: """[[{"a":{},"c":30},11],"a"]""");
        Log(eo);
        eo.Trim(maxCount: 2, hideKeys: ["b"], maxDepth: 2);
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
        AssertIdentical(actual: eo.ToJson(), expected: """[[{},11],"a"]""");
        eo.Trim(maxCount: 2, hideKeys: ["b"], maxDepth: 1);
        Log(eo);
        Pass(System.Reflection.MethodBase.GetCurrentMethod().Name);
        AssertIdentical(actual: eo.ToJson(), expected: """[[],"a"]""");
    }
}
