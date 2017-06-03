using System;
using System.Drawing;

namespace image_compression
{
    public class ErrorsEstimationService
    {
        private ErrorsEstimationService()
        {

        }
        
        public static float mse(float[][] original, float[][] restored)
        {
            float sum = 0;
            for (int i = 0; i < restored.Length; ++i)
            {
                for (int j = 0; j < restored[i].Length; ++j)
                {
                    float diff = restored[i][j] - original[i][j];
                    sum += diff * diff;
                }
            }

            return sum / restored.Length / restored[0].Length;
        }

        public static float psnr(float mse)
        {
            return (float)(10 * Math.Log(255 * 255 / mse));
        }


        public static void estimateErrors(Bitmap original, Bitmap restored, ImageCompressionDetails compressionDetails)
        {
            float[] diffs = new float[3];
            int n = restored.Height * restored.Width;
            for (int i = 0; i < restored.Height; ++i)
            {
                for (int j = 0; j < restored.Width; ++j)
                {
                    float[] orig = ImageCompositionGuy.extractYCbCr(original, j, i);
                    float[] rest = ImageCompositionGuy.extractYCbCr(restored, j, i);

                    for (int k = 0; k < 3; ++k)
                    {
                        float diff = orig[k] - rest[k];
                        diffs[k] += diff * diff;
                    }
                }
            }

            compressionDetails.YChannelMSE = diffs[0] / n;
            compressionDetails.YChannelPSNR = psnr(compressionDetails.YChannelMSE);
            compressionDetails.CbChannelMSE = diffs[1] / n;
            compressionDetails.CbChannelPSNR = psnr(compressionDetails.CbChannelMSE);
            compressionDetails.CrChannelMSE = diffs[2] / n;
            compressionDetails.CrChannelPSNR = psnr(compressionDetails.CrChannelMSE);
            compressionDetails.ErrorsEstimated = true;
        }
    }
}
