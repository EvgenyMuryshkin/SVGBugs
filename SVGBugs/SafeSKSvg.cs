using SkiaSharp;
using System.Text;

namespace SVGBugs
{
    static class SKExt
    {
        public static SKMatrix Scale(this SKMatrix source, float scale)
        {
            return SKMatrix.CreateScale(source.ScaleX * scale, source.ScaleY * scale);
        }

        public static SKSize Scale(this SKSize source, float scale)
        {
            return new SKSize(source.Width * scale, source.Height * scale);
        }
    }

    class SafeSKSvg
    {
        private readonly SkiaSharp.Extended.Svg.SKSvg _svg;
        private readonly float _safeSize = 400;

        public SafeSKSvg(string svg)
        {
            if (File.Exists(svg))
            {
                using (var svgReader = new MemoryStream(File.ReadAllBytes(svg)))
                {
                    _svg = new SkiaSharp.Extended.Svg.SKSvg();
                    _svg.Load(svgReader);
                }
            }
            else
            {
                using (var svgReader = new MemoryStream(Encoding.Default.GetBytes(svg)))
                {
                    _svg = new SkiaSharp.Extended.Svg.SKSvg();
                    _svg.Load(svgReader);
                }
            }
        }

        public SafeSKSvg(SkiaSharp.Extended.Svg.SKSvg svg, float maxSize)
        {
            _svg = svg;
        }

        public SKImage SafeImage(float scale = 1)
        {
            return SKImage.FromPicture(_svg.Picture, SafeSize.Scale(scale).ToSizeI(), SafeScaleMatrix.Scale(scale));
        }

        public SKMatrix SafeScaleMatrix => SKMatrix.CreateScale(SafeScale, SafeScale);
        public SKSize SafeSize => new SKSize(UniformSize * SafeScale, UniformSize * SafeScale);
        public float UniformSize => Math.Max(_svg.CanvasSize.Width, _svg.CanvasSize.Height);
        public float SafeScale
        {
            get
            {
                var xScale = _safeSize / _svg.CanvasSize.Width;
                var yScale = _safeSize / _svg.CanvasSize.Height;

                if (xScale >= 1 && yScale >= 1)
                    return 1;

                return Math.Min(xScale, yScale);
            }
        }
    }
}