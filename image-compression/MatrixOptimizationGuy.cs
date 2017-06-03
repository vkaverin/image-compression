using System;
using System.Collections.Generic;

namespace image_compression
{
    public class MatrixOptimizationGuy
    {
        private MatrixOptimizationGuy()
        {

        }

        public static Tuple<int, int> optimize(float[][] matrix, int quality)
        {
            List<MatrixElement> elements = new List<MatrixElement>();

            int nonzeroBefore = 0;
            int thrown = 0;

            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                {
                    if (Math.Abs(matrix[i][j]) > 0)
                    {
                        ++nonzeroBefore;
                        elements.Add(new MatrixElement(i, j, matrix[i][j]));
                    }
                }
            }

            elements.Sort();

            int optimizationPercent = (100 - quality);

            if (optimizationPercent == 100)
            {
                for (int i = 0; i < matrix.Length; ++i)
                {
                    Array.Clear(matrix[i], 0, matrix[i].Length);
                }
            }

            int limit = (optimizationPercent == 100)
                ? elements.Count
                : (int)((elements.Count / 100.0) * (optimizationPercent));

            while (thrown < limit)
            {
                Tuple<int, int> pos = elements[thrown].getPosition();
                matrix[pos.Item1][pos.Item2] = 0;
                ++thrown;
            }

            return new Tuple<int, int>(nonzeroBefore, nonzeroBefore - limit);
        }

        struct MatrixElement : IComparable<MatrixElement>
        {
            Tuple<Int32, Int32> position;
            float value;

            public MatrixElement(int i, int j, float value)
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
