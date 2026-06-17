using System.Collections.Generic;
using T = Global.EasyObject;
using static Global.EasyObject;

[TestClass]
public class JSON読み出しテスト
{
    protected string RenderJson(Global.EasyObject eo)
    {
        return eo.ToJson(indent: false, sortKeys: true);
    }

    [TestMethod]
    public void 単純な辞書()
    {
        Pass();
        ShowDetail = true;
        var eo = FromJson("""{ a: 123, b: "abc" }""");
        Assert.AreEqual(actual: RenderJson(eo), expected: """
            {"a":123,"b":"abc"}
            """);
        eo["c"] = 777;
        Assert.AreEqual(actual: RenderJson(eo), expected: """
            {"a":123,"b":"abc","c":777}
            """);
        Dictionary<string, Global.EasyObject> dict = eo.AsDictionary;
        dict["d"] = 888;
        Assert.AreEqual(actual: RenderJson(eo), expected: """
            {"a":123,"b":"abc","c":777,"d":888}
            """);
        eo[2] = 111;
        Assert.AreEqual(actual: RenderJson(eo), expected: """
            [null,null,111]
            """);
        Pass();
    }
    [TestMethod]
    public void 単純なリスト()
    {
        Pass();
        ShowDetail = true;
        var eo = FromJson("""[123, "abc"]""");
        Assert.AreEqual(actual: RenderJson(eo), expected: """
            [123,"abc"]
            """);
        eo.Add(true);
        Assert.AreEqual(actual: RenderJson(eo), expected: """
            [123,"abc",true]
            """);
        eo["c"] = 777;
        Assert.AreEqual(actual: RenderJson(eo), expected: """
            {"c":777}
            """);
        Pass();
    }
}