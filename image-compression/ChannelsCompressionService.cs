using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_compression
{

    public class ChannelsCompressionService
    {
        private static ChannelsCompressionService instance = new ChannelsCompressionService();

        private ChannelsCompressionService()
        {

        }

        public static ChannelsCompressionService get()
        {
            return instance;
        }

        public CompressionStatictics compress(YCbCrChannelsContainer channels, CompressionTemplate compressionTemplate)
        {
            CompressionStatictics stats = new CompressionStatictics();

            // Y
            float[][] y = channels.YChannel();
            HaarCompression.compress(y);
            stats.Y = MatrixOptimizationGuy.optimize(y, compressionTemplate.YChannelQuality);

            // Cb
            float[][] cb = channels.CbChannel();
            HaarCompression.compress(cb);
            stats.Cb = MatrixOptimizationGuy.optimize(cb, compressionTemplate.CbChannelQuality);

            // Cr
            float[][] cr = channels.CrChannel();
            HaarCompression.compress(cr);
            stats.Cr = MatrixOptimizationGuy.optimize(cr, compressionTemplate.CrChannelQuality);

            return stats;
        }

        public void decompress(YCbCrChannelsContainer source)
        {
            HaarCompression.decompress(source.YChannel());
            HaarCompression.decompress(source.CbChannel());
            HaarCompression.decompress(source.CrChannel());
        }
    }

    
}
