using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_compression
{
    class ErrorsEstimatingGuy
    {
        private ErrorsEstimatingGuy()
        {

        }

        public static double mse(double[][] original, double[][] restored)
        {
            double sum = 0;
            for (int i = 0; i < restored.Length; ++i)
            {
                for (int j = 0; j < restored[i].Length; ++j)
                {
                    double diff = restored[i][j] - original[i][j];
                    sum += diff * diff;
                }
            }

            return sum / restored.Length / restored[0].Length;
        }

        public static double psnr(double mse)
        {
            return 10 * Math.Log(255 * 255 / mse);
        }
    }
}
