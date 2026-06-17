using System;
using Xunit;
using T = Global.EasyObject;
using static Global.EasyObject;

public class DynamicTest
{
    private readonly ITestOutputHelper Out;
    public DynamicTest(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
        T.ClearSettings();
        T.ShowDetail = true;
        T.EchoRedirector = Out.WriteLine;
        T.LogRedirector = Out.WriteLine;
        T.Log("Setup() called");
    }
    [Fact]
    public void Setup()
    {
        Console.WriteLine("Setup() called");
        ClearSettings();
    }

    [Fact]
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