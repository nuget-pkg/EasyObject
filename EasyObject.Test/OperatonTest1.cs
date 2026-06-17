using System.Collections.Generic;
using System.Linq;
using Xunit;
using T = Global.EasyObject;
using static Global.EasyObject;

public class OperationTest
{
    private readonly ITestOutputHelper Out;
    public OperationTest(ITestOutputHelper testOutputHelper)
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
        ShowDetail = true;
        Global.EasyObject eo = Global.EasyObject.FromObject(new int[] { 1, 2, 3, 4 });
        var list = eo.AsList;
        var even = list.Where(x => x.Cast<int>() % 2 == 0).ToList();
        Echo(even, "even");
        Assert.True(even.Count == 2);
        Assert.Equal("2", even[0].ToJson());
        Assert.Equal("4", even[1].ToJson());
        List<int> intList = eo.AsList.Select(x => (int)x.Dynamic).ToList();
        Echo(intList, "intList");
        var odd = intList.Where(x => x % 2 == 1).ToList();
        Assert.True(odd.Count == 2);
        Assert.Equal(1, odd[0]);
        Assert.Equal(3, odd[1]);
    }
}