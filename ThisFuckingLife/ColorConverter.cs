using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ThisFuckingLife
{
    public class ColorConverter
    {
        public static Color CMYKtoRGB(decimal[] cmyk)
        {
            Color newClr = new Color();
            newClr.R = (byte)(255 * (1 - cmyk[0]) * (1 - cmyk[3]));
            newClr.G = (byte)(255 * (1 - cmyk[1]) * (1 - cmyk[3]));
            newClr.B = (byte)(255 * (1 - cmyk[2]) * (1 - cmyk[3]));
            newClr.A = 255;
            return newClr;
        }

        public static decimal[] HexToCMYK(string hex)
        {
            decimal computedC = 0;
            decimal computedM = 0;
            decimal computedY = 0;
            decimal computedK = 0;

            hex = (hex[0] == '#') ? hex.Substring(1, 6) : hex;

            if (hex.Length != 6)
            {
                return null;
            }

            decimal r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            decimal g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            decimal b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            // BLACK
            if (r == 0 && g == 0 && b == 0)
            {
                computedK = 1;
                return new[] { 0, 0, 0, computedK };
            }

            computedC = 1 - (r / 255);
            computedM = 1 - (g / 255);
            computedY = 1 - (b / 255);

            var minCMY = Math.Min(computedC, Math.Min(computedM, computedY));

            computedC = (computedC - minCMY) / (1 - minCMY);
            computedM = (computedM - minCMY) / (1 - minCMY);
            computedY = (computedY - minCMY) / (1 - minCMY);
            computedK = minCMY;

            return new[] { computedC, computedM, computedY, computedK };
        }

        public static float[] RGBtoRYB(Color color)
        {
            float r = color.R;
            float g = color.G;
            float b = color.B;
            // Remove the whiteness from the color.
            float w = Math.Min(Math.Min(r, g), b);
            r -= w;
            g -= w;
            b -= w;

            float mg = Math.Max(Math.Max(r, g), b);

            // Get the yellow out of the red+green.
            float y = Math.Min(r, g);
            r -= y;
            g -= y;

            // If this unfortunate conversion combines blue and green, then cut each in
            // half to preserve the value's maximum range.
            if (b!=0 && g!=0)
            {
                b /= 2.0f;
                g /= 2.0f;
            }

            // Redistribute the remaining green.
            y += g;
            b += g;

            // Normalize to values.
            float my = Math.Max(Math.Max(r, y), b);
            if (my != 0)
            {
                float n = mg / my;
                r *= n;
                y *= n;
                b *= n;
            }

            // Add the white back in.
            r += w;
            y += w;
            b += w;

            // And return back the ryb typed accordingly.
            return new float[3]{r,y,b};
        }

        public static Color RYBtoRGB(float[] color)
        {
            float r = color[0];
            float y = color[1];
            float b = color[2];
            // Remove the whiteness from the color.
            float w = Math.Min(Math.Min(r, y), b);
            r -= w;
            y -= w;
            b -= w;

            float my = Math.Max(Math.Max(r, y), b);

            // Get the green out of the yellow and blue
            float g = Math.Min(y, b);
            y -= g;
            b -= g;

            if (b!=0 && g!=0)
            {
                b *= 2.0f;
                g *= 2.0f;
            }

            // Redistribute the remaining yellow.
            r += y;
            g += y;

            // Normalize to values.
            float mg = Math.Max(Math.Max(r, g), b);
            if (mg != 0)
            {
                float n = my / mg;
                r *= n;
                g *= n;
                b *= n;
            }

            // Add the white back in.
            r += w;
            g += w;
            b += w;

            // And return back the ryb typed accordingly.
            return new Color {R= (byte)r,G= (byte)g,B=(byte)b, A = 255};
        }

        public static Color MixColors(List<Color> colors)
        {
            float[] newRYB = new float[3];

            List<float[]> colorsRYB = new List<float[]>();
            foreach (var color in colors)
                colorsRYB.Add(RGBtoRYB(color));
            foreach(var colorRYB in colorsRYB)
            {
                newRYB[0] += colorRYB[0];
                newRYB[1] += colorRYB[1];
                newRYB[2] += colorRYB[2];
            }
            float max = System.Math.Max(System.Math.Max(newRYB[0], newRYB[1]), newRYB[2]);
            newRYB[0] = (float)Math.Floor(newRYB[0] / max * 255);
            newRYB[1] = (float)Math.Floor(newRYB[1] / max * 255);
            newRYB[2] = (float)Math.Floor(newRYB[2] / max * 255);

            return RYBtoRGB(newRYB);
        }
    }
}
