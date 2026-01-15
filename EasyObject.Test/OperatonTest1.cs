using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using static Global.EasyObject;

public class OperationTest
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
        EasyObject eo = EasyObject.FromObject(new int[] { 1, 2, 3, 4 });
        var list = eo.AsList;
        var even = list.Where(x => x.Cast<int>() % 2 == 0).ToList();
        Echo(even, "even");
        Assert.That(even.Count, Is.EqualTo(2));
        Assert.That(even[0].ToJson(), Is.EqualTo("2"));
        Assert.That(even[1].ToJson(), Is.EqualTo("4"));
        List<int> intList = eo.AsList.Select(x => (int)x.Dynamic).ToList();
        Echo(intList, "intList");
        var odd = intList.Where(x => x % 2 == 1).ToList();
        Assert.That(odd.Count, Is.EqualTo(2));
        Assert.That(odd[0], Is.EqualTo(1));
        Assert.That(odd[1], Is.EqualTo(3));
    }
}