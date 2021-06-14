using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ImageMedia.Models;

namespace ImageMedia.Algo
{
    public static partial class Algo
    {
        private static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(e => e.MimeType == mimeType);
        }

        public static TransformationResult BmpRleTransformation(Image BmpWithoutCompression, string outputPath)
        {
            TransformationResult res = new TransformationResult();

            var imageCodeInfo = GetImageCodecInfo(MimeType.BMP);
            var encoder = System.Drawing.Imaging.Encoder.Compression;
            var encoderParameter = new EncoderParameter(
                encoder, (long)EncoderValue.CompressionRle);

            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = encoderParameter;

            Stopwatch timer = new Stopwatch();

            using (var memoryStream = new MemoryStream())
            {
                timer.Restart();
                BmpWithoutCompression.Save(memoryStream, ImageCodecInfo, encoderParameters);
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
                Image Bmp = Image.FromStream(fileStream);
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
                    var cl = imageCodeInfo.Clsid;
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
