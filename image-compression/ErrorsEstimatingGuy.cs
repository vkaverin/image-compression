using System;

namespace image_compression
{
    class ErrorsEstimatingGuy
    {
        private ErrorsEstimatingGuy()
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
    }
}
