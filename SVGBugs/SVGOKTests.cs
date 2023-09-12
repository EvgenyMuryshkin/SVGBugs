namespace SVGBugs
{
    [TestClass]
    public class SVGOKTests : SVGBaseTest
    {
        [TestMethod]
        public void OKPNG()
        {
            var names = new[] { "no-fill", "with-fill" };
            foreach (var name in names)
            {
                SavePNGFromSVGFile($"{name}.png", $"{name}.svg");
            }
        }
    }
}