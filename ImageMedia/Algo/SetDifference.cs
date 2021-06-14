using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ImageMedia.Models;

namespace ImageMedia.Algo
{
    public static partial class Algo
    {
        public static DifferenceResult SetDifference(Image image, Image compressedImage, bool IsRDifference, bool IsGDifference, bool IsBDifference)
        {
            DifferenceResult res = new DifferenceResult();

            var bitImage1 = (Bitmap)image;
            var bitImage2 = (Bitmap)compressedImage;

            int width = image.Width;
            int height = image.Height;
            var bitDifferenceIm = new Bitmap(width, height);

            int r;
            int g;
            int b;
            int multiplier = 10;
            int alpha = byte.MaxValue;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    r = IsRDifference ?
                        Math.Abs(bitImage1.GetPixel(i, j).R - bitImage2.GetPixel(i, j).R) : 0;

                    g = IsGDifference ?
                        Math.Abs(bitImage1.GetPixel(i, j).G - bitImage2.GetPixel(i, j).G) : 0;

                    b = IsBDifference ?
                        Math.Abs(bitImage1.GetPixel(i, j).B - bitImage2.GetPixel(i, j).B) : 0;

                    res.RgbDifference.Add(r, g, b);

                    r = Math.Min(r * multiplier, alpha);
                    g = Math.Min(g * multiplier, alpha);
                    b = Math.Min(b * multiplier, alpha);

                    bitDifferenceIm.SetPixel(i, j, Color.FromArgb(alpha, r, g, b));
                }
            }

            res.DifferenceImage =
                Imaging.CreateBitmapSourceFromHBitmap(
                    bitDifferenceIm.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(width, height)
                );

            return res;
        }

        private static readonly ImageCodecInfo ImageCodecInfo = GetImageCodecInfo("image/png");
    }
}
