using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace image_compression
{
    public class ImageCompressionGuy
    {
        private ImageCompressionGuy()
        {
        }

        public static ImageCompressionDetails compressByTemplate(CompressionTemplate compressionTemplate)
        {
            return new TheOnlyGuyThatWorks(compressionTemplate).process();
        }

        private class TheOnlyGuyThatWorks
        {
            private CompressionTemplate compressionTemplate;
            private ImageCompressionDetails compressionDetails = new ImageCompressionDetails();

            public TheOnlyGuyThatWorks(CompressionTemplate compressionTemplate)
            {
                this.compressionTemplate = compressionTemplate;
                this.compressionDetails.SourceImage = this.compressionTemplate.SourceImage;
            }

            public ImageCompressionDetails process()
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                // Idea: Divide and conquer by boxes 8x8.

                YCbCrChannelsContainer channels = ImageCompositionGuy.decompose(compressionTemplate.SourceImage);
                YCbCrChannelsContainer compressedChannels = compressChannels(channels);
                YCbCrChannelsContainer restoredChannels = restoreChannels(compressedChannels);
                Image restoredImage = ImageCompositionGuy.compose(ChannelsTransformingGuy.yCbCrToRGB(restoredChannels));
                this.compressionDetails.CompressionTime = stopwatch.ElapsedMilliseconds;
                this.compressionDetails.CompressedImage = restoredImage;

                estimateErrors(channels, restoredChannels);

                return compressionDetails;
            }

            private YCbCrChannelsContainer compressChannels(YCbCrChannelsContainer source)
            {
                // Y
                float[][] yMatrix = HaarCompressingGuy.compressMatrix(source.YChannel(), this.compressionTemplate.YChannelQuality);
                MatrixCompressionDetails optimizedYDetails =  MatrixOptimizationGuy.optimize(yMatrix, this.compressionTemplate.YChannelQuality);
                compressionDetails.YChannelNonzeroElementsNumberOriginal = compressionDetails.YChannelNonzeroElementsNumberOriginal + optimizedYDetails.nonzeroElementsNumberOriginal;
                compressionDetails.YChannelNonzeroElementsNumberCompressed = compressionDetails.YChannelNonzeroElementsNumberCompressed + optimizedYDetails.nonzeroElementsNumberCompressed;
                // Cb
                float[][] cbMatrix = HaarCompressingGuy.compressMatrix(source.CbChannel(), this.compressionTemplate.CbChannelQuality);
                MatrixCompressionDetails optimizedCbDetails = MatrixOptimizationGuy.optimize(cbMatrix, this.compressionTemplate.CbChannelQuality);
                compressionDetails.CbChannelNonzeroElementsNumberOriginal = compressionDetails.CbChannelNonzeroElementsNumberOriginal + optimizedCbDetails.nonzeroElementsNumberOriginal;
                compressionDetails.CbChannelNonzeroElementsNumberCompressed = compressionDetails.CbChannelNonzeroElementsNumberCompressed + optimizedCbDetails.nonzeroElementsNumberCompressed;
                // Cr
                float[][] crMatrix = HaarCompressingGuy.compressMatrix(source.CrChannel(), this.compressionTemplate.CrChannelQuality);
                MatrixCompressionDetails optimizedCrDetails = MatrixOptimizationGuy.optimize(crMatrix, this.compressionTemplate.CrChannelQuality);
                compressionDetails.CrChannelNonzeroElementsNumberOriginal = compressionDetails.CrChannelNonzeroElementsNumberOriginal + optimizedCrDetails.nonzeroElementsNumberOriginal;
                compressionDetails.CrChannelNonzeroElementsNumberCompressed = compressionDetails.CrChannelNonzeroElementsNumberCompressed + optimizedCrDetails.nonzeroElementsNumberCompressed;

                // Y, Cb or Cr's lenght - doesn't matter.
                YCbCrChannelsContainer compressedChannels = new YCbCrChannelsContainer(optimizedYDetails.CompressedMatrix.Length, optimizedYDetails.CompressedMatrix[0].Length);

                compressedChannels.setY(optimizedYDetails.CompressedMatrix);
                compressedChannels.setCb(optimizedCbDetails.CompressedMatrix);
                compressedChannels.setCr(optimizedCrDetails.CompressedMatrix);

                return compressedChannels;
            }

            private YCbCrChannelsContainer restoreChannels(YCbCrChannelsContainer source)
            {
                YCbCrChannelsContainer compressedChannels = new YCbCrChannelsContainer(source.Height, source.Width);
                compressedChannels.setY(HaarCompressingGuy.restoreMatrix(source.YChannel()));
                compressedChannels.setCb(HaarCompressingGuy.restoreMatrix(source.CbChannel()));
                compressedChannels.setCr(HaarCompressingGuy.restoreMatrix(source.CrChannel()));
                return compressedChannels;
            }

            private void estimateErrors(YCbCrChannelsContainer original, YCbCrChannelsContainer restored)
            {
                float mse = ErrorsEstimatingGuy.mse(original.YChannel(), restored.YChannel());
                this.compressionDetails.YChannelMSE = mse;
                this.compressionDetails.YChannelPSNR = ErrorsEstimatingGuy.psnr(mse);

                mse = ErrorsEstimatingGuy.mse(original.CbChannel(), restored.CbChannel());
                this.compressionDetails.CbChannelMSE = mse;
                this.compressionDetails.CbChannelPSNR = ErrorsEstimatingGuy.psnr(mse);

                mse = ErrorsEstimatingGuy.mse(original.CrChannel(), restored.CrChannel());
                this.compressionDetails.CrChannelMSE = mse;
                this.compressionDetails.CrChannelPSNR = ErrorsEstimatingGuy.psnr(mse);
            }
        }   
    }
}
