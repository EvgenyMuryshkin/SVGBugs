using SkiaSharp;
using SkiaSharp.Extended.Svg;

namespace SVGBugs
{
    public class SVGBaseTest
    {
        public byte[] SVG2PNG(string svg)
        {
            var ImageQuality = 100;
            var safeSVG = new SafeSKSvg(svg);

            using (var img = safeSVG.SafeImage())
            using (var bmp = SKBitmap.FromImage(img))
            using (var pixels = bmp.PeekPixels())
            {
                return pixels.Encode(SKEncodedImageFormat.Png, ImageQuality).ToArray();
            }
        }

        public string SourceSVGPath(string name) => Path.Combine(Solution.SolutionPath, "source", name);
        public string TargetPNGPath(string name) => Path.Combine(Solution.SolutionPath, "target", name);
        public Stream SourceSVGStream(string name) => new MemoryStream(File.ReadAllBytes(SourceSVGPath(name)));

        public string SourceSVG(string name)
        {
            if (name.EndsWith(".svg"))
                return File.ReadAllText(SourceSVGPath(name));

            return name;
        }

        public void SavePNGFromSVGFile(string pngName, string svgName)
        {
            var data = SVG2PNG(SourceSVG(svgName));
            File.WriteAllBytes(TargetPNGPath(pngName), data);
        }

        public void SavePNGFromSVGData(string pngName, string svgData)
        {
            var data = SVG2PNG(SourceSVG(svgData));

            var targetPNG = TargetPNGPath(pngName);

            File.WriteAllBytes(targetPNG, data);
            File.WriteAllText($"{targetPNG}.svg", svgData);
        }

        public void SaveResult(string pngName, byte[] data)
        {
            File.WriteAllBytes(TargetPNGPath(pngName), data);
        }

        /// <summary>
        /// 100
        /// </summary>
        protected float IconHeight = 100;

        /// <summary>
        /// 100
        /// </summary>
        protected float IconWidth = 100;

        /// <summary>
        /// 140x140
        /// </summary>
        protected SKRect Bounds => SKRect.Create(0, 0, 140, 140);

        protected SkiaSharp.Extended.Svg.SKSvg LoadSVG(string name)
        {
            var SVGimage = new SkiaSharp.Extended.Svg.SKSvg(new SKSize(IconWidth, IconHeight));
            using (Stream stream = SourceSVGStream(name))
            {
                SVGimage.Load(stream);
            }

            return SVGimage;
        }
    }
}