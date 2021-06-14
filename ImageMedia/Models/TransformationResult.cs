using System.Drawing;

namespace ImageMedia.Models
{
    class TransformationResult
    {
        public double EncodingTime { get; set; }

        public double ReadingTime { get; set; }

        public double DecodingTime { get; set; }


        public double WritingTime { get; set; }

        public Image CompressedImage { get; set; }
        public long CompressedImageSize { get; set; } 

        public TransformationResult()
        {}

        public override string ToString()
        {
            return $@"=================== RESULT ===================
                    = Reading time: {ReadingTime}
                    = Encoding time: {EncodingTime}
                    = Decoding time: {DecodingTime}
                    = Writing time: {WritingTime}
                    = Compressed image size: {CompressedImageSize}
                    ==============================================";
        }
    }
}
