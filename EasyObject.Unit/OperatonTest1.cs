using T = Global.EasyObject;
using static Global.EasyObject;

[TestClass]
public class OperationTest
{
    [TestMethod]
    public void Test01()
    {
        ShowDetail = true;
        Global.EasyObject eo = Global.EasyObject.FromObject(new int[] { 1, 2, 3, 4 });
        var list = eo.AsList;
        var even = list.Where(x => x.Cast<int>() % 2 == 0).ToList();
        Echo(even, "even");
        Assert.HasCount(2, even);
        Assert.AreEqual("2", even[0].ToJson());
        Assert.AreEqual("4", even[1].ToJson());
        List<int> intList = eo.AsList.Select(x => (int)x.Dynamic).ToList();
        Echo(intList, "intList");
        var odd = intList.Where(x => x % 2 == 1).ToList();
        Assert.HasCount(2, odd);
        Assert.AreEqual(1, odd[0]);
        Assert.AreEqual(3, odd[1]);
    }
}