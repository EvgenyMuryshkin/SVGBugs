using SkiaSharp;
using System.Diagnostics;
using System.Drawing;

namespace SVGBugs
{
    public static class ColorTools
    {
        public static Color FromHex(string hexColor)
        {
            var skColor = SKColor.Parse(hexColor);
            return ColorTools.FromUint((uint)skColor);
        }

        public static Color FromUint(uint color)
        {
            return Color.FromArgb((int)color);
        }

        public static Color FromRgb(int r, int g, int b)
        {
            return Color.FromArgb(255, r, g, b);
        }

        public static SKColor ToSKColor(this Color color)
        {
            return new SKColor((uint)color.ToArgb());
        }

        public static string ToRGB(this Color color)
        {
            return $"rgb({color.R},{color.G},{color.B})";
        }
        public static string StringFromString(string color)
        {
            if (color.StartsWith("#"))
            {
                var hexValue = color.Substring(1);
                switch (hexValue.Length)
                {
                    case 3:
                        var fullHexValue = string.Join("", hexValue.Select(c => $"{c}{c}"));
                        return FromHex(fullHexValue).ToRGB();
                    case 6:
                        return FromHex(hexValue).ToRGB();
                    default:
                        Debugger.Break();
                        return Color.Black.ToRGB();
                }
            }
            else if (color.StartsWith("rgb"))
            {
                return color;
            }
            else
            {
                return Color.FromName(color).ToRGB();
            }
        }
    }
}