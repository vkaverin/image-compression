using System;
using System.Drawing;

namespace image_compression
{
    public class CompressionTemplateBuildingGuy
    {
        private CompressionTemplate compressionTemplate;

        private CompressionTemplateBuildingGuy(Image image)
        {
            if (image == null)
            {
                throw new NullReferenceException("Image can not be null");
            }

            this.compressionTemplate = new CompressionTemplate();
            this.compressionTemplate.SourceImage = image;
        }

        public static CompressionTemplateBuildingGuy forImage(Image image)
        {
            return new CompressionTemplateBuildingGuy(image);
        }

        public CompressionTemplateBuildingGuy withYQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.YChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuildingGuy withCbQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.CbChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuildingGuy withCrQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.CrChannelQuality = quality;
            return this;
        }

        public CompressionTemplateBuildingGuy withSourceImage(Image image)
        {
            if (image == null)
            {
                throw new NullReferenceException("Reference to image is null.");
            }

            this.compressionTemplate.SourceImage = image;
            return this;
        }

        public CompressionTemplateBuildingGuy withErrorsEstimation(bool val)
        {
            this.compressionTemplate.estimateErrors = val;
            return this;
        }

        public CompressionTemplate make()
        {
            return this.compressionTemplate;
        }

        private void validateQuality(int quality)
        {
            if (quality < 0)
            {
                throw new ArgumentException("Quality can not be less than one percent.");
            }
        }
    }
}
