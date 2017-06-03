using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace image_compression
{
    public class ImageCompressionService
    {
        private ImageCompressionService()
        {
        }

        public static ImageCompressionDetails compressByTemplate(CompressionTemplate compressionTemplate)
        {
            return new Worker(compressionTemplate).process();
        }

        private class Worker
        {
            private CompressionTemplate compressionTemplate;
            private ImageCompressionDetails compressionDetails = new ImageCompressionDetails();

            public Worker(CompressionTemplate compressionTemplate)
            {
                this.compressionTemplate = compressionTemplate;
                this.compressionDetails.SourceImage = this.compressionTemplate.SourceImage;
            }

            public ImageCompressionDetails process()
            {
                return new ConcurrentImageCompressor(this.compressionTemplate).process();
            }            
        }   
    }
}
