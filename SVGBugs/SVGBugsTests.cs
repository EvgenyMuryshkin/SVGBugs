using SkiaSharp;
using System.Drawing;
using System.Text;

namespace SVGBugs
{
    [TestClass]
    public class SVGBugsTests : SVGBaseTest
    {
        /// <summary>
        /// These PNG do not redner, or rendered weirdly
        /// </summary>
        [TestMethod]
        public void NoPNG()
        {
            var names = new[] { "no-png-1", "no-png-2", "no-png-3", "no-png-4", "no-png-5" };
            foreach (var name in names)
            {
                SavePNGFromSVGFile($"{name}.png", $"{name}.svg");
            }
        }

        /// <summary>
        /// Having transparent background color breaks PNG rendering
        /// </summary>
        [TestMethod]
        public void DrawPictureTransaprentBackground_NoFill()
        {
            var bounds = Bounds;
            using (var ms = new MemoryStream())
            using (var stream = new SKManagedWStream(ms))
            {
                using (var canvas = SKSvgCanvas.Create(bounds, stream))
                {
                    var svg = LoadSVG("no-fill.svg");
                    var Coordinate = new SKPoint(20, 20);
                    var backgroundColor = Color.Transparent;
                    canvas.Clear(backgroundColor.ToSKColor()); // This is preventing PNG rendering - result PNG is blank
                    canvas.DrawPicture(svg.Picture, Coordinate);
                }

                var svgBytes = ms.ToArray();
                var svgContent = Encoding.UTF8.GetString(svgBytes);
                SavePNGFromSVGData("DrawPictureTransaprentBackground_NoFill.png", svgContent);
            }
        }

        /// <summary>
        /// Having transaprent background color breaks PNG rendering
        /// </summary>
        [TestMethod]
        public void DrawPictureTransaprentBackground_WithFill()
        {
            var bounds = Bounds;
            using (var ms = new MemoryStream())
            using (var stream = new SKManagedWStream(ms))
            {
                using (var canvas = SKSvgCanvas.Create(bounds, stream))
                {
                    var svg = LoadSVG("with-fill.svg");
                    var Coordinate = new SKPoint(20, 20);
                    var backgroundColor = Color.Transparent;
                    canvas.Clear(backgroundColor.ToSKColor()); // This is preventing PNG rendering - result PNG is blank, fill in source svg is ignored
                    canvas.DrawPicture(svg.Picture, Coordinate);
                }

                var svgBytes = ms.ToArray();
                var svgContent = Encoding.UTF8.GetString(svgBytes);
                SavePNGFromSVGData("DrawPictureTransaprentBackground_WithFill.png", svgContent);
            }
        }

        /// <summary>
        /// Paint is not propagate into DrawPicture
        /// </summary>
        [TestMethod]
        public void DrawPictureDisregardPaint()
        {
            var bounds = Bounds;
            using (var ms = new MemoryStream())
            using (var stream = new SKManagedWStream(ms))
            {
                using (var canvas = SKSvgCanvas.Create(bounds, stream))
                {
                    var svg = LoadSVG("no-fill.svg");
                    var Coordinate = new SKPoint(20, 20);
                    var paint = new SKPaint()
                    {
                        Style = SKPaintStyle.Fill,
                        Color = new SKColor(255, 0, 0)
                    };
                    canvas.DrawPicture(svg.Picture, Coordinate, paint); // red paint is not going into SVG picture 
                }

                var svgBytes = ms.ToArray();
                var svgContent = Encoding.UTF8.GetString(svgBytes);
                SavePNGFromSVGData("DrawPictureDisregardPaint.png", svgContent);
            }
        }

        [TestMethod]
        public void DrawPicture_WithBackground_NoSourceFill()
        {
            var bounds = Bounds;
            using (var ms = new MemoryStream())
            using (var stream = new SKManagedWStream(ms))
            {
                using (var canvas = SKSvgCanvas.Create(bounds, stream))
                {
                    var svg = LoadSVG("no-fill.svg");
                    var Coordinate = new SKPoint(20, 20);

                    // source SVG has no color information. If background is not set, picture color is black.
                    // if background rect is drawn, then picture has same color, 

                    var backgroundColor = Color.Red;
                    canvas.Clear(backgroundColor.ToSKColor());
                    canvas.DrawPicture(svg.Picture, Coordinate);
                }

                var svgBytes = ms.ToArray();
                var svgContent = Encoding.UTF8.GetString(svgBytes);
                SavePNGFromSVGData("DrawPicture_WithBackground_NoSourceFill.png", svgContent);
            }
        }

        [TestMethod]
        public void DrawPicture_WithBackground_WithLine_NoSourceFill()
        {
            var bounds = Bounds;
            using (var ms = new MemoryStream())
            using (var stream = new SKManagedWStream(ms))
            {
                using (var canvas = SKSvgCanvas.Create(bounds, stream))
                {
                    var svg = LoadSVG("no-fill.svg");
                    var Coordinate = new SKPoint(20, 20);
                    var backgroundColor = Color.Red;
                    canvas.Clear(backgroundColor.ToSKColor());

                    var paint = new SKPaint()
                    {
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 2,
                        Color = new SKColor(0, 0, 255)
                    };
                    canvas.DrawLine(new SKPoint(10, 10), new SKPoint(20, 20), paint);

                    canvas.DrawPicture(svg.Picture, Coordinate);
                }

                var svgBytes = ms.ToArray();
                var svgContent = Encoding.UTF8.GetString(svgBytes);
                SavePNGFromSVGData("DrawPicture_WithBackground_WithLine_NoSourceFill.png", svgContent);
            }
        }

        /// <summary>
        /// Some rubbish in text coordinates x and y, does not render png or in browser
        /*
	<text fill="blue" font-size="12" font-family="Segoe UI" x="0, 6.8789063, 13.154297, 16.910156, 23.941406, 28.113281, " y="0, ">
			Before
	</text>
        
	<text fill="blue" font-size="12" font-family="Segoe UI" x="0, 7.7402344, 11.496094, 15.5625, 21.837891, " y="100, ">
			After
	</text>

         */
        /// </summary>
        [TestMethod]
        public void DrawText()
        {
            var bounds = Bounds;
            using (var ms = new MemoryStream())
            using (var stream = new SKManagedWStream(ms))
            {
                using (var canvas = SKSvgCanvas.Create(bounds, stream))
                {
                    var svg = LoadSVG("with-fill.svg");
                    var Coordinate = new SKPoint(20, 20);

                    var paint = new SKPaint()
                    {
                        Style = SKPaintStyle.Fill,
                        Color = new SKColor(0, 0, 255)
                    };

                    canvas.DrawText("Before", new SKPoint(0, 0), paint);
                    canvas.DrawPicture(svg.Picture, Coordinate);
                    canvas.DrawText("After", new SKPoint(0, 100), paint);
                }

                var svgBytes = ms.ToArray();
                var svgContent = Encoding.UTF8.GetString(svgBytes);
                SavePNGFromSVGData("DrawText.png", svgContent);
            }
        }

        /// <summary>
        /// Output svg color are converted to knowncolors e.g. lime, or gets shortened e.g. #CC00FF => #C0F
        /// This does not render well in Xamarin forms SkiaSharp.
        /// Colors should stay hexadecimal all the times,
        /// </summary>
        [TestMethod]
        public void ToKnownColorShouldNotBeConverted()
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
                Assert.IsTrue(svgContent.Contains("#00ff00"));
            }
        }
    }
}