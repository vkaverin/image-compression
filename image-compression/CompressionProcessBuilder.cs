using System;
using System.Drawing;

namespace image_compression
{
    public class CompressionTemplate
    {
        public Image SourceImage { get; set; }

        public int RedChannelQuality { get; set; }
        public int GreenChannelQuality { get; set; }
        public int BlueChannelQuality { get; set; }
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

        public CompressionTemplateBuilder withRedQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.RedChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuilder withGreenQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.GreenChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuilder withBlueQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.BlueChannelQuality = quality;
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
