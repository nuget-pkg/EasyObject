namespace Global;

using T = Global.EasyObject;
using static Global.EasyObject;

[TestClass]
public class ToPrintableTest {
    [TestMethod]
    public void Test01() {
        Pass();
        ShowDetail = true;
        var @do = FromObject(new { a = 123, b = "abc" }).ExportToDynamicObject();
        Echo(@do, compact: true);
        var @printable = ToPrintable(@do);
        Assert.IsTrue(@printable == "<System.Dynamic.ExpandoObject> {\n  a: 123,\n  b: \"abc\"\n}");
        @printable = ToPrintable(@do, compact: true);
        Assert.AreEqual("<System.Dynamic.ExpandoObject> {a:123,b:\"abc\"}", @printable);
        Pass();
    }
}