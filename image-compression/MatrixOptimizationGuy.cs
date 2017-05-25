using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_compression
{
    public class MatrixOptimizationGuy
    {
        private MatrixOptimizationGuy()
        {

        }

        public static MatrixCompressionDetails optimize(double[][] matrix, int quality)
        {
            MatrixCompressionDetails details = new MatrixCompressionDetails();
            
            List<MatrixElement> elements = new List<MatrixElement>();

            int nonzeroOriginalNumber = 0;
            int thrown = 0;

            double[][] optimized = new double[matrix.Length][];

            for (int i = 0; i < matrix.Length; ++i)
            {
                optimized[i] = new double[matrix[i].Length];
                Array.Copy(matrix[i], optimized[i], matrix[i].Length);
            }

            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                {
                    if (Math.Abs(matrix[i][j]) > 0)
                    {
                        ++nonzeroOriginalNumber;
                        elements.Add(new MatrixElement(i, j, matrix[i][j]));
                    }
                }
            }

            elements.Sort();

            int percentToThrowAway = (100 - quality);
            int limit = (int)((elements.Count / 100.0) * (percentToThrowAway));
            int throwLimit = percentToThrowAway < 100 ? (int)((elements.Count / 100.0) * (percentToThrowAway)) : elements.Count;

            while (thrown < throwLimit && thrown < elements.Count)
            {
                Tuple<int, int> pos = elements[thrown].getPosition();
                optimized[pos.Item1][pos.Item2] = 0;
                ++thrown;
            }


            details.CompressedMatrix = optimized;
            details.nonzeroElementsNumberOriginal = nonzeroOriginalNumber;
            details.nonzeroElementsNumberCompressed = nonzeroOriginalNumber - thrown;

            return details;
        }

        struct MatrixElement : IComparable<MatrixElement>
        {
            Tuple<Int32, Int32> position;
            double value;

            public MatrixElement(int i, int j, double value)
            {
                this.position = new Tuple<int, int>(i, j);
                this.value = value;
            }

            public int CompareTo(MatrixElement other)
            {
                if (this.value == other.value)
                {
                    return 0;
                }

                return Math.Abs(this.value) < Math.Abs(other.value) ? -1 : 1;
            }

            public Tuple<Int32, Int32> getPosition()
            {
                return this.position;
            }
        }
    }
}
