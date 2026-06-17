using T = Global.EasyObject;
using static Global.EasyObject;

[TestClass]
public class ObjectTest
{
    [TestMethod]
    public void Test01()
    {
        Pass();
        ShowDetail = true;
        Global.EasyObject eo = Global.EasyObject.FromObject(new { a=1, b=2 });
        Echo(eo, "eo");
        Assert.IsTrue(eo.ContainsKey("a"));
        Assert.IsFalse(eo.ContainsKey("c"));
        Pass();
    }
}