using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace image_compression
{
    public class CompressionProcessBuilder
    {
        private HaarCompression compressionTemplate;

        public CompressionProcessBuilder(Image image)
        {
            if (image == null)
            {
                throw new NullReferenceException("Image can not be null");
            }

            this.compressionTemplate = new HaarCompression();
            this.compressionTemplate.setSourceImage(image);
        }

        public CompressionProcessBuilder withRedQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.setRedChannelQuality(quality);
            return this;
        }

        public CompressionProcessBuilder withGreenQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.setGreenChannelQuality(quality);
            return this;
        }

        public CompressionProcessBuilder withBlueQuality(int quality)
        {
            validateQuality(quality);

            this.compressionTemplate.setBlueChannelQuality(quality);
            return this;
        }

        public CompressionProcessBuilder withSourceImage(Image image)
        {
            if (image == null)
            {
                throw new NullReferenceException("Reference to image is null.");
            }

            this.compressionTemplate.setSourceImage(image);
            return this;
        }

        public HaarCompression build()
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
