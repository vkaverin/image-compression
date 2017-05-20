using System;
using System.Drawing;
using System.Collections.Generic;

namespace image_compression
{

public class HaarCompression
{
    private const int boxSize = 8;
    private int nonZeroOriginal = 0;
    private int nonZeroCompressed = 0;

    private Image sourceImage;

    private int redChannelQuality;
    private int greenChannelQuality;
    private int blueChannelQuality;

    public void setRedChannelQuality(int quality)
    {
        this.redChannelQuality = quality;
    }

    public void setGreenChannelQuality(int quality)
    {
        this.greenChannelQuality = quality;
    }

    public void setBlueChannelQuality(int quality)
    {
        this.blueChannelQuality = quality;
    }

    public void setSourceImage(Image image)
    {
        this.sourceImage = image;
    }

    public static CompressionProcessBuilder compress(Image image)
    {
        return new CompressionProcessBuilder(image);
    }

    public CompressionDetails process()
    {
            ////////////////
            ChannelsContainer channels = splitChannels(sourceImage);
            ChannelsContainer compressedChannels = compressChannels(channels);
            ChannelsContainer restoredChannels = restoreChannels(compressedChannels);
            Image compressedImage = restoreImage(restoredChannels);
            ////////////////

        CompressionDetails compressionDetails = new CompressionDetails();
        compressionDetails.SourceImage = sourceImage;
        compressionDetails.CompressedImage = compressedImage;
        compressionDetails.SourceImageNonZeroPixelsCount = nonZeroOriginal;
        compressionDetails.CompressedImageNonZeroPixelsCount = nonZeroCompressed;
        
        return compressionDetails;
    }

        private ChannelsContainer splitChannels(Image image)
        {
            Bitmap sourceBitmap = new Bitmap(sourceImage);

            ChannelsContainer channels = new ChannelsContainer(image.Height, image.Width);
            channels.allocateMemory();

            for (int i = 0; i < image.Height; ++i)
            {
                for (int j = 0; j < image.Width; ++j)
                {
                    channels.red[i][j] = sourceBitmap.GetPixel(j, i).R;
                    channels.green[i][j] = sourceBitmap.GetPixel(j ,i).G;
                    channels.blue[i][j] = sourceBitmap.GetPixel(j, i).B;
                }
            }

            return channels;
        }

        private ChannelsContainer compressChannels(ChannelsContainer source)
        {
            ChannelsContainer compressedChannels = new ChannelsContainer((source.Height / HaarCompression.boxSize) * HaarCompression.boxSize, (source.Width / HaarCompression.boxSize) * HaarCompression.boxSize);
            compressedChannels.red = compressMatrix(source.redChannel(), this.redChannelQuality);
            compressedChannels.green = compressMatrix(source.greenChannel(), this.greenChannelQuality);
            compressedChannels.blue = compressMatrix(source.blueChannel(), this.blueChannelQuality);
            return compressedChannels;
        }

        private double[][] compressMatrix(double[][] matrix, int quality)
        {
            int croppedHeight = (matrix.Length / HaarCompression.boxSize) * HaarCompression.boxSize;
            double[][] compressedMatrix = new double[croppedHeight][];
            for (int i = 0; i < compressedMatrix.Length; ++i)
            {
                int croppedWidth = (matrix[i].Length / HaarCompression.boxSize) * HaarCompression.boxSize;
                compressedMatrix[i] = new double[croppedWidth];
            }

            for (int i = 0; i + HaarCompression.boxSize <= compressedMatrix.Length; i += HaarCompression.boxSize)
            {
                for (int j = 0; j + HaarCompression.boxSize <= compressedMatrix[i].Length; j += HaarCompression.boxSize)
                {
                    double[][] box = getBox(matrix, i, j);
                    double[][] compressedBox = haarCompression(box);
                    
                    threshold(ref compressedBox, quality);
                    putBox(compressedMatrix, compressedBox, i, j);
                }
            }

            return compressedMatrix;
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
            compressedChannels.red = restoreMatrix(source.redChannel());
            compressedChannels.green = restoreMatrix(source.greenChannel());
            compressedChannels.blue = restoreMatrix(source.blueChannel());
            return compressedChannels;
        }

        private Image restoreImage(ChannelsContainer channels)
    {
            Bitmap output = new Bitmap(channels.Width, channels.Height);
            for (int i = 0; i < channels.Height; ++i)
            {
                for (int j = 0; j < channels.Width; ++j)
                {
                    Color color = asColor(channels.red[i][j], channels.green[i][j], channels.blue[i][j]);
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
            int val = (int) value;
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

        private void threshold(ref double[][] matrix, int quality)
        {
            List<MatrixElement> elements = new List<MatrixElement>();

            int nonZeroPixelsCounterOriginal = 0;

            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                for (int j = 0; j < HaarCompression.boxSize; ++j)
                {
                    if (Math.Abs(matrix[i][j]) > 0)
                    {
                        ++nonZeroPixelsCounterOriginal;
                        elements.Add(new MatrixElement(i, j, matrix[i][j]));
                    }
                }


                elements.Sort();

                int percentToThrowAway = (100 - quality);
                int elementsToThrowAway = (int)((elements.Count / 100.0) * (percentToThrowAway));

                int thrownCount = 0;
                while (thrownCount < elementsToThrowAway && thrownCount < elements.Count)
                {
                    Tuple<Int32, Int32> pos = elements[thrownCount].getPosition();
                    matrix[pos.Item1][pos.Item2] = 0;
                    ++thrownCount;
                }

                nonZeroOriginal += nonZeroPixelsCounterOriginal;
                nonZeroCompressed += nonZeroPixelsCounterOriginal - thrownCount;
            }
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

            public double[][] red;
            // public double[][] Red { get; set; }
            public double[][] green;
            // public double[][] Green { get; set; }
            public double[][] blue;
            // public double[][] Blue { get; set; }

            public double[][] redChannel()
            {
                return this.red;
            }

            public double[][] greenChannel()
            {
                return this.green;
            }

            public double[][] blueChannel()
            {
                return this.blue;
            }

            public ChannelsContainer() {
            }

            public ChannelsContainer(int height, int width)
            {
                this.Width = width;
                this.Height = height;
            }

            public void allocateMemory()
            {
                init(out this.red);
                init(out this.green);
                init(out this.blue);
            }

            private void init(out double[][] matrix)
            {
                double[][] tmp = new double[this.Height][];
                for (int i = 0; i < this.Height; ++i)
                {
                    tmp[i] = new double[this.Width];
                }

                matrix = tmp;
            }
    }
}

}