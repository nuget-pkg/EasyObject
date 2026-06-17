namespace TestProject1
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int a = 5;
            int b = 6;
            Console.WriteLine($"a+b = {a + b}");
            Assert.AreEqual(11, a + b);
        }
    }
}
