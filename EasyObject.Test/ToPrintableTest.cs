namespace Global;
using static EasyObject;

using System;
using NUnit.Framework;

public class ToPrintableTest
{
    [SetUp]
    public void Setup()
    {
        Console.WriteLine("Setup() called");
        ClearSettings();
    }

    [Test]
    public void Test01()
    {
        ShowDetail = true;
        var @do = FromObject(new { a = 123, b = "abc" }).ExportToDynamicObject();
        Echo(@do, noIndent: true);
        var @printable = ToPrintable(@do);
        Assert.That(@printable, Is.EqualTo("<System.Dynamic.ExpandoObject> {\n  a: 123,\n  b: \"abc\"\n}"));
        @printable = ToPrintable(@do, noIndent: true);
        Assert.That(@printable, Is.EqualTo("<System.Dynamic.ExpandoObject> {a:123,b:\"abc\"}"));
    }
}