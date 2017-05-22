using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace image_compression
{
    public class HaarCompression
    {
        private const int boxSize = 8;

        private CompressionTemplate compressionTemplate;
        private ImageCompressionDetails compressionDetails = new ImageCompressionDetails();

        public static ImageCompressionDetails process(CompressionTemplate compressionTemplate)
        {
            return new HaarCompression(compressionTemplate).process();
        }

        private HaarCompression(CompressionTemplate compressionTemplate)
        {
            this.compressionTemplate = compressionTemplate;
            this.compressionDetails.SourceImage = this.compressionTemplate.SourceImage;
        }

        public static CompressionTemplateBuilder makeCompressionTemplateFor(Image image)
        {
            return new CompressionTemplateBuilder(image);
        }

        private ImageCompressionDetails process()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ChannelsContainer channels = splitChannels(compressionTemplate.SourceImage);
            ChannelsContainer compressedChannels = compressChannels(channels);
            ChannelsContainer restoredChannels = restoreChannels(compressedChannels);
            Image compressedImage = restoreImage(restoredChannels);
            this.compressionDetails.CompressionTime = stopwatch.ElapsedMilliseconds;
            this.compressionDetails.CompressedImage = compressedImage;

            estimateErrors(channels, restoredChannels);

            return compressionDetails;
        }

        private void estimateErrors(ChannelsContainer original, ChannelsContainer restored)
        {
            // Y channel
            compressionDetails.YChannelMSE = mse(original.YChannel(), restored.YChannel());
            compressionDetails.YChannelPSNR = psnr(compressionDetails.YChannelMSE);

            // Cr Channel
            compressionDetails.CbChannelMSE = mse(original.CbChannel(), restored.CbChannel());
            compressionDetails.CbChannelPSNR = psnr(compressionDetails.CbChannelMSE);

            // Cb channel
            compressionDetails.CrChannelMSE = mse(original.CrChannel(), restored.CrChannel());
            compressionDetails.CrChannelPSNR = psnr(compressionDetails.CrChannelMSE);
        }

        private double mse(double[][] original, double[][] restored)
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

        private double psnr(double mse)
        {
            return 10 * Math.Log(255 * 255 / mse);
        }

        private ChannelsContainer splitChannels(Image image)
        {
            Bitmap sourceBitmap = new Bitmap(image);

            ChannelsContainer channels = new ChannelsContainer(image.Height, image.Width);
            channels.allocateMemory();

            for (int i = 0; i < image.Height; ++i)
            {
                for (int j = 0; j < image.Width; ++j)
                {
                    channels.r[i][j] = sourceBitmap.GetPixel(j, i).R;
                    channels.g[i][j] = sourceBitmap.GetPixel(j, i).G;
                    channels.b[i][j] = sourceBitmap.GetPixel(j, i).B;
                }
            }

            channels.promoteYCbCr();

            return channels;
        }

        private ChannelsContainer compressChannels(ChannelsContainer source)
        {
            ChannelsContainer compressedChannels = new ChannelsContainer((source.Height / HaarCompression.boxSize) * HaarCompression.boxSize, (source.Width / HaarCompression.boxSize) * HaarCompression.boxSize);
            
            // Y
            MatrixCompressionDetails details = compressMatrix(source.YChannel(), this.compressionTemplate.YChannelQuality);
            compressedChannels.y = details.CompressedMatrix;
            compressionDetails.YChannelNonzeroElementsNumberOriginal = compressionDetails.YChannelNonzeroElementsNumberOriginal + details.nonzeroElementsNumberOriginal;
            compressionDetails.YChannelNonzeroElementsNumberCompressed = compressionDetails.YChannelNonzeroElementsNumberCompressed + details.nonzeroElementsNumberCompressed;
            // Cb
            details = compressMatrix(source.CbChannel(), this.compressionTemplate.CbChannelQuality);
            compressedChannels.cb = details.CompressedMatrix;
            compressionDetails.CbChannelNonzeroElementsNumberOriginal = compressionDetails.CbChannelNonzeroElementsNumberOriginal + details.nonzeroElementsNumberOriginal;
            compressionDetails.CbChannelNonzeroElementsNumberCompressed = compressionDetails.CbChannelNonzeroElementsNumberCompressed + details.nonzeroElementsNumberCompressed;
            // Cr
            details = compressMatrix(source.CrChannel(), this.compressionTemplate.CrChannelQuality);
            compressedChannels.cr = details.CompressedMatrix;
            compressionDetails.CrChannelNonzeroElementsNumberOriginal = compressionDetails.CrChannelNonzeroElementsNumberOriginal + details.nonzeroElementsNumberOriginal;
            compressionDetails.CrChannelNonzeroElementsNumberCompressed = compressionDetails.CrChannelNonzeroElementsNumberCompressed + details.nonzeroElementsNumberCompressed;

            return compressedChannels;
        }

        private class MatrixCompressionDetails
        {
            public double[][] CompressedMatrix { get; set; }
            public int nonzeroElementsNumberOriginal { get; set; }
            public int nonzeroElementsNumberCompressed { get; set; }
        }

        private MatrixCompressionDetails compressMatrix(double[][] matrix, int quality)
        {
            int croppedHeight = (matrix.Length / HaarCompression.boxSize) * HaarCompression.boxSize;
            double[][] compressedMatrix = new double[croppedHeight][];
            for (int i = 0; i < compressedMatrix.Length; ++i)
            {
                int croppedWidth = (matrix[i].Length / HaarCompression.boxSize) * HaarCompression.boxSize;
                compressedMatrix[i] = new double[croppedWidth];
            }

            MatrixCompressionDetails details = new MatrixCompressionDetails();
            for (int i = 0; i + HaarCompression.boxSize <= compressedMatrix.Length; i += HaarCompression.boxSize)
            {
                for (int j = 0; j + HaarCompression.boxSize <= compressedMatrix[i].Length; j += HaarCompression.boxSize)
                {
                    double[][] box = getBox(matrix, i, j);
                    double[][] compressedBox = haarCompression(box);

                    Tuple<int, int> thresholdDetails = threshold(ref compressedBox, quality);
                    details.nonzeroElementsNumberOriginal = details.nonzeroElementsNumberOriginal + thresholdDetails.Item1;
                    details.nonzeroElementsNumberCompressed = details.nonzeroElementsNumberCompressed + thresholdDetails.Item2;

                    putBox(compressedMatrix, compressedBox, i, j);
                }
            }

            details.CompressedMatrix = compressedMatrix;

            return details;
        }

        private double[][] getBox(double[][] matrix, int startTopIndex, int startLeftIndex)
        {
            double[][] box = new double[HaarCompression.boxSize][];
            for (int i = 0; i < box.Length; ++i)
            {
                box[i] = new double[HaarCompression.boxSize];
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

        private void putBox(double[][] matrix, double[][] box, int startTopIndex, int startLeftIndex)
        {
            for (int i = 0; i < box.Length; ++i)
            {
                for (int j = 0; j < box.Length; ++j)
                {
                    matrix[startTopIndex + i][startLeftIndex + j] = box[i][j];
                }
            }
        }

        private ChannelsContainer restoreChannels(ChannelsContainer source)
        {
            ChannelsContainer compressedChannels = new ChannelsContainer(source.Height, source.Width);
            compressedChannels.allocateMemory();
            compressedChannels.y = restoreMatrix(source.YChannel());
            compressedChannels.cb = restoreMatrix(source.CbChannel());
            compressedChannels.cr = restoreMatrix(source.CrChannel());
            compressedChannels.promoteRGB();
            return compressedChannels;
        }

        private Image restoreImage(ChannelsContainer channels)
        {
            Bitmap output = new Bitmap(channels.Width, channels.Height);
            for (int i = 0; i < channels.Height; ++i)
            {
                for (int j = 0; j < channels.Width; ++j)
                {
                    Color color = asColor(channels.r[i][j], channels.g[i][j], channels.b[i][j]);
                    output.SetPixel(j, i, color);
                }
            }

            return output;
        }

        private Color asColor(double red, double green, double blue)
        {
            return Color.FromArgb(toRGBRange(red), toRGBRange(green), toRGBRange(blue));
        }

        private int toRGBRange(double value)
        {
            int val = (int)value;
            return Math.Max(Math.Min(val, 255), 0);
        }

        private double[][] haarCompression(double[][] source)
        {
            double[][] matrix = new double[source.Length][];
            for (int i = 0; i < source.Length; ++i)
            {
                matrix[i] = new double[source[i].Length];
                source[i].CopyTo(matrix[i], 0);
            }

            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                int columns = HaarCompression.boxSize;
                double[] temp = new double[HaarCompression.boxSize];

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

                    for (int j = 0; j < HaarCompression.boxSize; ++j)
                    {
                        matrix[i][j] = temp[j];
                    }

                    columns /= 2;
                }
            }

            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                int rows = HaarCompression.boxSize;
                double[] temp = new double[HaarCompression.boxSize];
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

                    for (int j = 0; j < HaarCompression.boxSize; ++j)
                    {
                        matrix[j][i] = temp[j];
                    }

                    rows /= 2;
                }
            }

            return matrix;
        }

        private Tuple<int, int> threshold(ref double[][] matrix, int quality)
        {
            List<MatrixElement> elements = new List<MatrixElement>();

            int nonzeroOriginalNumber = 0;
            int thrown = 0;

            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                for (int j = 0; j < HaarCompression.boxSize; ++j)
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
            int throwLimit = (int)((elements.Count / 100.0) * (percentToThrowAway));
                            
            while (thrown < throwLimit && thrown < elements.Count)
            {
                Tuple<int, int> pos = elements[thrown].getPosition();
                matrix[pos.Item1][pos.Item2] = 0;
                ++thrown;
            }

            return new Tuple<int, int>(nonzeroOriginalNumber, nonzeroOriginalNumber - thrown);
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


        private double[][] restoreMatrix(double[][] matrix)
        {
            double[][] restoredMatrix = new double[matrix.Length][];
            for (int i = 0; i < restoredMatrix.Length; ++i)
            {
                restoredMatrix[i] = new double[matrix[i].Length];
            }

            for (int i = 0; i + HaarCompression.boxSize <= restoredMatrix.Length; i += HaarCompression.boxSize)
            {
                for (int j = 0; j + HaarCompression.boxSize <= restoredMatrix[i].Length; j += HaarCompression.boxSize)
                {
                    double[][] box = getBox(matrix, i, j);
                    double[][] compressedBox = restoreHaar(box);
                    putBox(restoredMatrix, compressedBox, i, j);
                }
            }

            return restoredMatrix;
        }

        private double[][] restoreHaar(double[][] source)
        {
            double[][] matrix = new double[source.Length][];
            for (int i = 0; i < source.Length; ++i)
            {
                matrix[i] = new double[source[i].Length];
                source[i].CopyTo(matrix[i], 0);
            }

            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                int columns = 1;
                double[] temp = new double[HaarCompression.boxSize];
                while (columns * 2 <= HaarCompression.boxSize)
                {
                    for (int j = 0; j < HaarCompression.boxSize; ++j)
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

            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                int rows = 1;
                double[] temp = new double[HaarCompression.boxSize];
                while (rows * 2 <= HaarCompression.boxSize)
                {
                    for (int j = 0; j < HaarCompression.boxSize; ++j)
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

        private class ChannelsContainer
        {
            public int Width { get; }
            public int Height { get; }

            public double[][] y;
            public double[][] cb;
            public double[][] cr;

            public int[][] r;
            public int[][] g;
            public int[][] b;

            public double[][] YChannel()
            {
                return this.y;
            }

            public double[][] CbChannel()
            {
                return this.cb;
            }

            public double[][] CrChannel()
            {
                return this.cr;
            }

            public ChannelsContainer()
            {
            }

            public ChannelsContainer(int height, int width)
            {
                this.Width = width;
                this.Height = height;
            }

            public void allocateMemory()
            {
                alloc(out this.y);
                alloc(out this.cb);
                alloc(out this.cr);
                alloc(out this.r);
                alloc(out this.g);
                alloc(out this.b);
            }

            private void alloc<T>(out T[][] matrix)
            {
                T[][] tmp = new T[this.Height][];
                for (int i = 0; i < this.Height; ++i)
                {
                    tmp[i] = new T[this.Width];
                }

                matrix = tmp;
            }

            public void promoteRGB()
            {
                for (int i = 0; i < this.Height; ++i)
                {
                    for (int j = 0; j < this.Width; ++j)
                    {
                        double y = this.y[i][j];
                        double cb = this.cb[i][j];
                        double cr = this.cr[i][j];

                        this.r[i][j] = (int) (y + 1.402 * (cr - 128));
                        this.g[i][j] = (int)(y - 0.344136 * (cb - 128) - 0.714136 * (cr - 128));
                        this.b[i][j] = (int)(y + 1.772 * (cb - 128));
                    }
                }
            }

            public void promoteYCbCr()
            {
                for (int i = 0; i < this.Height; ++i)
                {
                    for (int j = 0; j < this.Width; ++j)
                    {
                        int r = this.r[i][j];
                        int g = this.g[i][j];
                        int b = this.b[i][j];

                        this.y[i][j] = 0.299 * r + 0.587 * g + 0.114 * b;
                        this.cb[i][j] = 128 - 0.168736 * r - 0.33264 * g + 0.5 * b;
                        this.cr[i][j] = 128 + 0.5 * r - 0.418688 * g - 0.081312 * b;
                    }
                }
                        
            }
        }
    }
}