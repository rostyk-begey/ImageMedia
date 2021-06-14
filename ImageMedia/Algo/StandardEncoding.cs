using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageMedia.Models;

namespace ImageMedia.Algo
{
    public static partial class Algo
    {
        public static TransformationResult JpegTransformation(Image BmpWithoutCompression, string outputPath)
        {
            TransformationResult res = new TransformationResult();

            Stopwatch timer = new Stopwatch();

            using (var memoryStream = new MemoryStream())
            {
                timer.Restart();
                BmpWithoutCompression.Save(memoryStream, ImageFormat.Jpeg);
                timer.Stop();
                res.EncodingTime = timer.Elapsed.TotalMilliseconds;

                memoryStream.Position = 0;
                timer.Restart();
                using (var fileStream = new FileStream(outputPath, FileMode.Create, System.IO.FileAccess.Write))
                {
                    memoryStream.CopyTo(fileStream);
                }

                timer.Stop();
                res.WritingTime = timer.Elapsed.Milliseconds;
            }

            using (var fileStream = new FileStream(outputPath, FileMode.Open))
            {
                timer.Restart();
                timer.Stop();
                res.ReadingTime = timer.Elapsed.Milliseconds;

                using (var memoryStream = new MemoryStream())
                {
                    fileStream.Position = 0;
                    fileStream.CopyTo(memoryStream);

                    memoryStream.Position = 0;
                    timer.Restart();
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    timer.Stop();
                    res.DecodingTime = timer.Elapsed.Milliseconds;
                }
            }
            res.CompressedImage = Image.FromFile(outputPath);

            res.CompressedImageSize = new FileInfo(outputPath).Length;

            return res;
        }
    }
}
