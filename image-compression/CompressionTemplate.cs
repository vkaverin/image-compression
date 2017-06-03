using System.Drawing;

namespace image_compression
{
    public class CompressionTemplate
    {
        public Image SourceImage { get; set; }
        public bool estimateErrors { get; set; }

        public int YChannelQuality { get; set; }
        public int CbChannelQuality { get; set; }
        public int CrChannelQuality { get; set; }
    }
}
