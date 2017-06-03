using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace image_compression
{
    public class HaarCompression
    {
        private HaarCompression()
        {

        }

        public static void compress(float[][] input)
        {
            int n = input.Length;
            int m = input[0].Length;

            for (int i = 0; i < n; ++i)
            {
                float[] temp = new float[m];
                int columns = m;

                while (columns > 0)
                {
                    for (int j = 0; j < columns / 2; ++j)
                    {
                        temp[j] = (input[i][2 * j] + input[i][2 * j + 1]) / 2;
                    }

                    int k = columns / 2;
                    for (int j = 0; j < columns / 2; ++j, ++k)
                    {
                        temp[k] = (input[i][2 * j] - input[i][2 * j + 1]) / 2;
                    }

                    for (int j = 0; j < m; ++j)
                    {
                        input[i][j] = temp[j];
                    }

                    columns /= 2;
                }
            }

            for (int i = 0; i < m; ++i)
            {
                int rows = n;
                float[] temp = new float[n];
                while (rows > 0)
                {
                    for (int j = 0; j < rows / 2; ++j)
                    {
                        temp[j] = (input[2 * j][i] + input[2 * j + 1][i]) / 2;
                    }

                    int k = rows / 2;
                    for (int j = 0; j < rows / 2; ++j, ++k)
                    {
                        temp[k] = (input[2 * j][i] - input[2 * j + 1][i]) / 2;
                    }

                    for (int j = 0; j < n; ++j)
                    {
                        input[j][i] = temp[j];
                    }

                    rows /= 2;
                }
            }
        }

        public static void decompress(float[][] input)
        {
            int n = input.Length;
            int m = input[0].Length;

            for (int i = 0; i < n; ++i)
            {
                int columns = 1;
                float[] temp = new float[m];
                while (columns * 2 <= m)
                {
                    for (int j = 0; j < m; ++j)
                    {
                        temp[j] = input[i][j];
                    }

                    for (int j = 0; j < columns; ++j)
                    {
                        input[i][2 * j] = temp[j] + temp[j + columns];
                        input[i][2 * j + 1] = temp[j] - temp[j + columns];
                    }

                    columns *= 2;
                }
            }

            for (int i = 0; i < m; ++i)
            {
                int rows = 1;
                float[] temp = new float[n];
                while (rows * 2 <= n)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        temp[j] = input[j][i];
                    }

                    for (int j = 0; j < rows; ++j)
                    {
                        input[2 * j][i] = temp[j] + temp[j + rows];
                        input[2 * j + 1][i] = temp[j] - temp[j + rows];

                    }

                    rows *= 2;
                }
            }
        }
    }
}