using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace image_compression
{
    public class HaarCompressingGuy
    {
        private const int boxSize = 8;

        private HaarCompressingGuy()
        {

        }

        public static float[][] compressMatrix(float[][] matrix, int quality)
        {
            int croppedHeight = (matrix.Length / HaarCompressingGuy.boxSize) * HaarCompressingGuy.boxSize;
            float[][] compressedMatrix = new float[croppedHeight][];
            for (int i = 0; i < compressedMatrix.Length; ++i)
            {
                int croppedWidth = (matrix[i].Length / HaarCompressingGuy.boxSize) * HaarCompressingGuy.boxSize;
                compressedMatrix[i] = new float[croppedWidth];
            }
                        
            for (int i = 0; i + HaarCompressingGuy.boxSize <= compressedMatrix.Length; i += HaarCompressingGuy.boxSize)
            {
                for (int j = 0; j + HaarCompressingGuy.boxSize <= compressedMatrix[i].Length; j += HaarCompressingGuy.boxSize)
                {
                    float[][] box = getBox(matrix, i, j);
                    float[][] compressedBox = haarCompression(box);
                    putBox(compressedMatrix, compressedBox, i, j);
                }
            }

            return compressedMatrix;
        }

        private static float[][] getBox(float[][] matrix, int startTopIndex, int startLeftIndex)
        {
            float[][] box = new float[HaarCompressingGuy.boxSize][];
            for (int i = 0; i < box.Length; ++i)
            {
                box[i] = new float[HaarCompressingGuy.boxSize];
            }

            for (int i = 0; i < box.Length; ++i)
            {
                for (int j = 0; j < box.Length; ++j)
                {
                    box[i][j] = matrix[startTopIndex + i][startLeftIndex + j];
                }
            }

            return box;
        }

        private static void putBox(float[][] matrix, float[][] box, int startTopIndex, int startLeftIndex)
        {
            for (int i = 0; i < box.Length; ++i)
            {
                for (int j = 0; j < box.Length; ++j)
                {
                    matrix[startTopIndex + i][startLeftIndex + j] = box[i][j];
                }
            }
        }

        private static float[][] haarCompression(float[][] source)
        {
            float[][] matrix = new float[source.Length][];
            for (int i = 0; i < source.Length; ++i)
            {
                matrix[i] = new float[source[i].Length];
                source[i].CopyTo(matrix[i], 0);
            }

            for (int i = 0; i < HaarCompressingGuy.boxSize; ++i)
            {
                int columns = HaarCompressingGuy.boxSize;
                float[] temp = new float[HaarCompressingGuy.boxSize];

                while (columns > 0)
                {
                    for (int j = 0; j < columns / 2; ++j)
                    {
                        temp[j] = (matrix[i][2 * j] + matrix[i][2 * j + 1]) / 2;
                    }

                    int k = columns / 2;
                    for (int j = 0; j < columns / 2; ++j, ++k)
                    {
                        temp[k] = (matrix[i][2 * j] - matrix[i][2 * j + 1]) / 2;
                    }

                    for (int j = 0; j < HaarCompressingGuy.boxSize; ++j)
                    {
                        matrix[i][j] = temp[j];
                    }

                    columns /= 2;
                }
            }

            for (int i = 0; i < HaarCompressingGuy.boxSize; ++i)
            {
                int rows = HaarCompressingGuy.boxSize;
                float[] temp = new float[HaarCompressingGuy.boxSize];
                while (rows > 0)
                {
                    for (int j = 0; j < rows / 2; ++j)
                    {
                        temp[j] = (matrix[2 * j][i] + matrix[2 * j + 1][i]) / 2;
                    }

                    int k = rows / 2;
                    for (int j = 0; j < rows / 2; ++j, ++k)
                    {
                        temp[k] = (matrix[2 * j][i] - matrix[2 * j + 1][i]) / 2;
                    }

                    for (int j = 0; j < HaarCompressingGuy.boxSize; ++j)
                    {
                        matrix[j][i] = temp[j];
                    }

                    rows /= 2;
                }
            }

            return matrix;
        }

        public static float[][] restoreMatrix(float[][] matrix)
        {
            float[][] restoredMatrix = new float[matrix.Length][];
            for (int i = 0; i < restoredMatrix.Length; ++i)
            {
                restoredMatrix[i] = new float[matrix[i].Length];
            }

            for (int i = 0; i + HaarCompressingGuy.boxSize <= restoredMatrix.Length; i += HaarCompressingGuy.boxSize)
            {
                for (int j = 0; j + HaarCompressingGuy.boxSize <= restoredMatrix[i].Length; j += HaarCompressingGuy.boxSize)
                {
                    float[][] box = getBox(matrix, i, j);
                    float[][] compressedBox = restoreHaar(box);
                    putBox(restoredMatrix, compressedBox, i, j);
                }
            }

            return restoredMatrix;
        }

        private static float[][] restoreHaar(float[][] source)
        {
            float[][] matrix = new float[source.Length][];
            for (int i = 0; i < source.Length; ++i)
            {
                matrix[i] = new float[source[i].Length];
                source[i].CopyTo(matrix[i], 0);
            }

            for (int i = 0; i < HaarCompressingGuy.boxSize; ++i)
            {
                int columns = 1;
                float[] temp = new float[HaarCompressingGuy.boxSize];
                while (columns * 2 <= HaarCompressingGuy.boxSize)
                {
                    for (int j = 0; j < HaarCompressingGuy.boxSize; ++j)
                    {
                        temp[j] = matrix[i][j];
                    }

                    for (int j = 0; j < columns; ++j)
                    {
                        matrix[i][2 * j] = temp[j] + temp[j + columns];
                        matrix[i][2 * j + 1] = temp[j] - temp[j + columns];
                    }

                    columns *= 2;
                }
            }

            for (int i = 0; i < HaarCompressingGuy.boxSize; ++i)
            {
                int rows = 1;
                float[] temp = new float[HaarCompressingGuy.boxSize];
                while (rows * 2 <= HaarCompressingGuy.boxSize)
                {
                    for (int j = 0; j < HaarCompressingGuy.boxSize; ++j)
                    {
                        temp[j] = matrix[j][i];
                    }

                    for (int j = 0; j < rows; ++j)
                    {
                        matrix[2 * j][i] = temp[j] + temp[j + rows];
                        matrix[2 * j + 1][i] = temp[j] - temp[j + rows];

                    }


                    rows *= 2;
                }
            }

            return matrix;
        }
    }
}