using System.IO;
using System.Windows.Media.Imaging;

namespace ImageMedia.Models
{
    public class RgbDifference
    {
        public int R { get; private set; }

        public int G { get; private set; }

        public int B { get; private set; }

        public RgbDifference()
        {
            R = 0;
            G = 0;
            B = 0;
        }

        public RgbDifference(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public void Add(int r, int g, int b)
        {
            R += r;
            G += g;
            B += b;
        }
    }

    public class DifferenceResult
    {
        public BitmapSource DifferenceImage { get; set; }

        public RgbDifference RgbDifference { get; }

        public DifferenceResult()
        {
            RgbDifference = new RgbDifference();
        }

        public override string ToString()
        {
            return $@"= Red: {RgbDifference.R}
                      = Blue: {RgbDifference.G}
                      = Green: {RgbDifference.B}";
        }

        public void WriteDifferenceImage(string outputPath)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(DifferenceImage);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = 100;

            using (FileStream file = File.OpenWrite(outputPath))
            {
                encoder.Save(file);
            }
        }
    }
}

