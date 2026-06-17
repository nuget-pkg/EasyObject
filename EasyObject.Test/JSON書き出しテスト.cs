using System.Collections.Generic;
using Xunit;
using T = Global.EasyObject;
using static Global.EasyObject;

public class JSON書き出しテスト
{
    private readonly ITestOutputHelper Out;
    public JSON書き出しテスト(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
        T.ClearSettings();
        T.ShowDetail = true;
        T.EchoRedirector = Out.WriteLine;
        T.LogRedirector = Out.WriteLine;
        T.Log("Setup() called");
    }

    protected string RenderJson(Global.EasyObject eo)
    {
        return eo.ToJson(indent: false, sortKeys: true);
    }

    [Fact]
    public void 単純な辞書()
    {
        Pass();
        ShowDetail = true;
        var eo = FromObject(new { a = 123, b = "abc" });
        Assert.Equal(actual: RenderJson(eo), expected: """
            {"a":123,"b":"abc"}
            """);
        eo["c"] = 777;
        Assert.Equal(actual: RenderJson(eo), expected: """
            {"a":123,"b":"abc","c":777}
            """);
        Dictionary<string, Global.EasyObject> dict = eo.AsDictionary;
        dict["d"] = 888;
        Assert.Equal(actual: RenderJson(eo), expected: """
            {"a":123,"b":"abc","c":777,"d":888}
            """);
        eo[2] = 111;
        Assert.Equal(actual: RenderJson(eo), expected: """
            [null,null,111]
            """);
        Pass();
    }
    [Fact]
    public void 単純なリスト()
    {
        Pass();
        ShowDetail = true;
        var eo = FromObject(new object[] { 123, "abc" });
        Assert.Equal(actual: RenderJson(eo), expected: """
            [123,"abc"]
            """);
        eo.Add(true);
        Assert.Equal(actual: RenderJson(eo), expected: """
            [123,"abc",true]
            """);
        eo["c"] = 777;
        Assert.Equal(actual: RenderJson(eo), expected: """
            {"c":777}
            """);
        Pass();
    }
}