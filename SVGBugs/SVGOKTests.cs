using SkiaSharp;

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

        //[TestMethod]
        public void Example()
        {
            var info = new SKImageInfo(256, 256);
            using (var surface = SKSurface.Create(info))
            {
                // the the canvas and properties
                var canvas = surface.Canvas;

                // make sure the canvas is blank
                canvas.Clear(SKColors.White);

                // draw some text
                var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill
                };
                var coord = new SKPoint(info.Width / 2, (info.Height + paint.TextSize) / 2);
                canvas.DrawText("SkiaSharp", coord, paint);

                // save the file
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite("output.png"))
                {
                    data.SaveTo(stream);
                }
            }
        }
    }
}