using System;
using System.Drawing;

namespace image_compression
{
    public class CompressionTemplate
    {
        public Image SourceImage { get; set; }

        public int YChannelQuality { get; set; }
        public int CbChannelQuality { get; set; }
        public int CrChannelQuality { get; set; }
    }

    public class CompressionTemplateBuilder
    {
        private CompressionTemplate compressionTemplate;

        public CompressionTemplateBuilder(Image image)
        {
            if (image == null)
            {
                throw new NullReferenceException("Image can not be null");
            }

            this.compressionTemplate = new CompressionTemplate();
            this.compressionTemplate.SourceImage = image;
        }

        public CompressionTemplateBuilder withYQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.YChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuilder withCbQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.CbChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuilder withCrQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.CrChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuilder withSourceImage(Image image)
        {
            if (image == null)
            {
                throw new NullReferenceException("Reference to image is null.");
            }

            this.compressionTemplate.SourceImage = image;
            return this;
        }

        public CompressionTemplate make()
        {
            return this.compressionTemplate;
        }

        private void validateQuality(int quality)
        {
            if (quality < 1)
            {
                throw new ArgumentException("Quality can not be less than one percent.");
            }
        }
    }
}
