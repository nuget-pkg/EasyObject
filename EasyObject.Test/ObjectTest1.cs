using Xunit;
using T = Global.EasyObject;
using static Global.EasyObject;

public class ObjectTest
{
    private readonly ITestOutputHelper Out;
    public ObjectTest(ITestOutputHelper testOutputHelper)
    {
        Out = testOutputHelper;
        T.ClearSettings();
        T.ShowDetail = true;
        T.EchoRedirector = Out.WriteLine;
        T.LogRedirector = Out.WriteLine;
        T.Log("Setup() called");
    }

    [Fact]
    public void Test01()
    {
        Pass();
        ShowDetail = true;
        Global.EasyObject eo = Global.EasyObject.FromObject(new { a=1, b=2 });
        Echo(eo, "eo");
        Assert.True(eo.ContainsKey("a"));
        Assert.False(eo.ContainsKey("c"));
        Pass();
    }
}