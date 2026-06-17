using System;
using Xunit;
using T = Global.EasyObject;

using static Global.EasyObject;
// ReSharper disable CheckNamespace
namespace Global.EasyObjectTest;

public class AssertCapabilitiesTest {
    private readonly ITestOutputHelper Out;
    public AssertCapabilitiesTest(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
        T.ClearSettings();
        T.ShowDetail = true;
        T.EchoRedirector = Out.WriteLine;
        T.LogRedirector = Out.WriteLine;
        T.Log("Setup() called");
    }
    [Fact]
    public void Test901()
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
        Pass();
    }
    [Fact]
    public void Test902()
    {
        Pass();
        ShowDetail = true;
        DebugOutput = true;
        Pass();
        EasyObject eo = NewArray(NewArray(NewObject("a", NewObject("b", 222, "d", 111), "b", 20, "c", 30), 11, 22, 33), "a", "b", "c");
        Pass();
        Echo(eo, maxCount: 2, hideKeys: ["b"], title: "1");
        eo.Trim(maxCount: 2, hideKeys: ["b"], maxDepth: 3);
        Pass();
        AssertIdentical(actual: eo.ToJson(), expected: """[[{"a":{},"c":30},11],"a"]""");
        Echo(eo);
        eo.Trim(maxCount: 2, hideKeys: ["b"], maxDepth: 2);
        Pass();
        AssertIdentical(actual: eo.ToJson(), expected: """[[{},11],"a"]""");
        eo.Trim(maxCount: 2, hideKeys: ["b"], maxDepth: 1);
        Echo(eo);
        Pass();
        AssertIdentical(actual: eo.ToJson(), expected: """[[],"a"]""");
    }
}