namespace Global;

using Xunit;
using T = Global.EasyObject;
using static Global.EasyObject;

public class ToPrintableTest {
    private readonly ITestOutputHelper Out;
    public ToPrintableTest(ITestOutputHelper testOutputHelper) {
        Out = testOutputHelper;
        T.ClearSettings();
        T.ShowDetail = true;
        T.EchoRedirector = Out.WriteLine;
        T.LogRedirector = Out.WriteLine;
        T.Log("Setup() called");
    }

    [Fact]
    public void Test01() {
        Pass();
        ShowDetail = true;
        var @do = FromObject(new { a = 123, b = "abc" }).ExportToDynamicObject();
        Echo(@do, compact: true);
        var @printable = ToPrintable(@do);
        Assert.True(@printable == "<System.Dynamic.ExpandoObject> {\n  a: 123,\n  b: \"abc\"\n}");
        @printable = ToPrintable(@do, compact: true);
        Assert.Equal("<System.Dynamic.ExpandoObject> {a:123,b:\"abc\"}", @printable);
        Pass();
    }
}