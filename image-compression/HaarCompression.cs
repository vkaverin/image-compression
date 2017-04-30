using System;
using System.Drawing;
using System.Collections.Generic;

public class HaarCompression
{
    private const int boxSize = 8;
    private int nonZeroOriginal = 0;
    private int nonZeroCompressed = 0;

    private CompressionInfo compressionInfo = new CompressionInfo();

    private int qualityPercent;

    public HaarCompression(int qualityPercent)
    {
        this.qualityPercent = qualityPercent;
    }

    public CompressionInfo compressImage(Image image)
    {
        Bitmap source = new Bitmap(image);
        Bitmap target = new Bitmap(source.Width, source.Height);

        for (int i = 0; i < image.Width; i += HaarCompression.boxSize)
        {
            for (int j = 0; j < image.Height; j += HaarCompression.boxSize)
            {
                RGBBox box = getNextBox(source, i, j);
                RGBBox compressedBox = compress(box);
                restoreImage(ref target, compressedBox, i, j);
            }
        }

        CompressionInfo compressionInfo = new CompressionInfo();
        compressionInfo.Source = source;
        compressionInfo.Target = target;
        compressionInfo.Quality = this.qualityPercent;
        compressionInfo.SourceNonZero = nonZeroOriginal;
        compressionInfo.CompressedNonZero = nonZeroCompressed;
        
        return compressionInfo;
    }

    private void restoreImage(ref Bitmap image, RGBBox source, int l, int r)
    {
        double[][] red = restoreMatrix(source.getRedPixelsMatrix());
        double[][] green = restoreMatrix(source.getGreenPixelsMatrix());
        double[][] blue = restoreMatrix(source.getBluePixelsMatrix());

        RGBBox restored = new RGBBox(red, green, blue);

        for (int i = l, boxLeftStart = 0; i < image.Width && i < l + HaarCompression.boxSize; ++i, ++boxLeftStart)
        {
            for (int j = r, boxTopStart = 0; j < image.Height && j < r + HaarCompression.boxSize; ++j, ++boxTopStart)
            {
                image.SetPixel(i, j, restored.getColor(boxLeftStart, boxTopStart));
            }
        }
    }

    private RGBBox getNextBox(Bitmap source, int startLeftIndex, int startTopIndex)
    {
        RGBBox box = new RGBBox(HaarCompression.boxSize);
        for (int i = startLeftIndex, boxHorizontalPosition = 0; i < source.Width && i < (startLeftIndex + HaarCompression.boxSize); ++i, ++boxHorizontalPosition)
        {
            for (int j = startTopIndex, boxVerticalPosition = 0; j < source.Height && j < (startTopIndex + HaarCompression.boxSize); ++j, ++boxVerticalPosition)
            {
                box.setPixel(source.GetPixel(i, j), boxHorizontalPosition, boxVerticalPosition);
            }
        }

        return box;
    }

    private RGBBox compress(RGBBox source)
    {
        double[][] red = haarCompression(source.getRedPixelsMatrix());
        double[][] green = haarCompression(source.getGreenPixelsMatrix());
        double[][] blue = haarCompression(source.getBluePixelsMatrix());

        return new RGBBox(red, green, blue);   
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
        }

        int percentToThrowAway = (100 - qualityPercent);
        int elementsToThrowAway = (int) ((elements.Count / 100.0) * (percentToThrowAway));

        elements.Sort();

        int thrownCount = 0;
        while (thrownCount < elementsToThrowAway && thrownCount < elements.Count)
        {
            Tuple<Int32, Int32> pos = elements[thrownCount].getPosition();
            matrix[pos.Item1][pos.Item2] = 0;
            ++thrownCount;
        }

        nonZeroOriginal += nonZeroPixelsCounterOriginal;
        nonZeroCompressed += nonZeroPixelsCounterOriginal - thrownCount;

        return matrix;
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

    private double[][] restoreMatrix(double[][] source)
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

    private class RGBBox
    {
        private double[][] red;
        private double[][] green;
        private double[][] blue;

        private int boxSize = 1;

        public RGBBox(int boxSize)
        {
            this.boxSize = boxSize;

            this.red = new double[HaarCompression.boxSize][];
            this.green = new double[HaarCompression.boxSize][];
            this.blue = new double[HaarCompression.boxSize][];

            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                this.red[i] = new double[HaarCompression.boxSize];
                this.green[i] = new double[HaarCompression.boxSize];
                this.blue[i] = new double[HaarCompression.boxSize];
            }
        }

        public RGBBox(RGBBox copy) : this(copy.boxSize)
        {
            for (int i = 0; i < HaarCompression.boxSize; ++i)
            {
                for (int j = 0; j < HaarCompression.boxSize; ++j)
                {
                    this.red[i][j] = copy.getRedPixel(i, j);
                    this.green[i][j] = copy.getGreenPixel(i, j);
                    this.blue[i][j] = copy.getBluePixel(i, j);
                }
            }
        }

        public RGBBox(double[][] red, double[][] green, double[][] blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public void setPixel(Color pixel, int x, int y)
        {
            this.red[x][y] = pixel.R;
            this.green[x][y] = pixel.G;
            this.blue[x][y] = pixel.B;
        }

        public void setRedPixel(double value, int i, int j)
        {
            this.red[i][j] = value;
        }

        public void setGreenPixel(double value, int i, int j)
        {
            this.green[i][j] = value;
        }

        public void setBluePixel(double value, int i, int j)
        {
            this.blue[i][j] = value;
        }

        public double getRedPixel(int i, int j)
        {
            if (this.red[i][j] < 0)
            {
                return 0;
            }
            return this.red[i][j];
        }

        public double getGreenPixel(int i, int j)
        {
            if (this.green[i][j] < 0)
            {
                return 0;
            }

            return this.green[i][j];
        }

        public double getBluePixel(int i, int j)
        {

            if (this.blue[i][j] < 0)
            {
                return 0;
            }
            return this.blue[i][j];
        }

        public double[][] getRedPixelsMatrix() {
            return this.red;
        }

        public double[][] getGreenPixelsMatrix()
        {
            return this.green;
        }

        public double[][] getBluePixelsMatrix()
        {
            return this.blue;
        }

        public int getBoxSize()
        {
            return HaarCompression.boxSize;
        }

        public Color getColor(int i, int j)
        {
            int normalizedRed = Math.Max(Math.Min((int)red[i][j], 255), 0);
            int normalizedGreen = Math.Max(Math.Min((int)green[i][j], 255), 0);
            int normalizedBlue = Math.Max(Math.Min((int)blue[i][j], 255), 0);
            return Color.FromArgb(normalizedRed, normalizedGreen, normalizedBlue);
        }
    }
}
