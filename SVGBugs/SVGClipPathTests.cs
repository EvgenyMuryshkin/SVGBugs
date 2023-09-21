using SkiaSharp;
using System.Text;

namespace SVGBugs
{
    [TestClass]
    public class SVGClipPathTests : SVGBaseTest
    {
        public string ClipPath()
        {
            var bounds = Bounds;
            using (var ms = new MemoryStream())
            using (var stream = new SKManagedWStream(ms))
            {
                using (var canvas = SKSvgCanvas.Create(bounds, stream))
                {
                    var svg = LoadSVG("with-fill.svg");
                    var Coordinate = new SKPoint(20, 20);
                    canvas.DrawPicture(svg.Picture, Coordinate);
                }

                var svgBytes = ms.ToArray();
                var svgContent = Encoding.UTF8.GetString(svgBytes);
                return svgContent;
            }
        }

        [TestMethod]
        public void ClipPath1()
        {
            SavePNGFromSVGData("ClipPath1.png", ClipPath());
        }

        [TestMethod]
        public void ClipPath2()
        {
            SavePNGFromSVGData("ClipPath2.png", ClipPath());
        }

        [TestMethod]
        public void ClipPath3()
        {
            SavePNGFromSVGData("ClipPath3.png", ClipPath());
        }
    }
}