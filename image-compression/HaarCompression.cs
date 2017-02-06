using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

public class HaarCompression
{
    int boxSize = 1;

    public String compressImage(Image image, int ratio)
    {
        this.boxSize = 8; // (1 << ratio);

        Bitmap bitmap = new Bitmap(image);
        Bitmap compressedImage = new Bitmap(bitmap.Width, bitmap.Height);

        for (int i = 0; i < image.Width; i += this.boxSize)
        {
            for (int j = 0; j < image.Height; j += this.boxSize)
            {
                RGB box = getNextBox(bitmap, i, j);
                RGB compressedBox = compressColors(box, ratio);
                restoreImageFromRGB(ref compressedImage, compressedBox, i, j);
            }
        }

        if (!Directory.Exists(Application.StartupPath))
        {
            Directory.CreateDirectory(Application.StartupPath);
        }
        string filename = Application.StartupPath + "\\result" + DateTime.Now.ToFileTime();
        compressedImage.Save(filename);
        return filename;
    }

    private void restoreImageFromRGB(ref Bitmap image, RGB compressedBox, int l, int r)
    {
        restoreMatrix(compressedBox.getRedPixelsMatrix());
        restoreMatrix(compressedBox.getGreenPixelsMatrix());
        restoreMatrix(compressedBox.getBluePixelsMatrix());

        for (int i = l; i < image.Width && i < l + this.boxSize; ++i)
        {
            for (int j = r; j < image.Height && j < r + this.boxSize; ++j)
            {
                //Color pix = Color.FromArgb((int) compressedBox.getR(i - l, j - r), 
                //    (int) compressedBox.getG(i - l, j - r), 
                //    (int) compressedBox.getB(i - l, j - r));

                //Color pix = Color.FromArgb((int) compressedBox.getR(0, 0), 
                //    (int) compressedBox.getG(0, 0), 
                //    (int) compressedBox.getB(0, 0));

                Color pix = Color.FromArgb((int)compressedBox.getRedPixel(i - l, j - r),
                    (int)compressedBox.getGreenPixel(i - l, j - r),
                    (int)compressedBox.getBluePixel(i - l, j - r));

                image.SetPixel(i, j, pix);
            }
        }
    }

    private RGB getNextBox(Bitmap image, int l, int r)
    {
        RGB box = new RGB(this.boxSize);
        for (int i = l; i < image.Width && i < l + this.boxSize; ++i)
        {
            for (int j = r; j < image.Height && j < r + this.boxSize; ++j)
            {
                Color pix = image.GetPixel(i, j);
                box.setRedPixel(pix.R, i - l, j - r);
                box.setGreenPixel(pix.G, i - l, j - r);
                box.setBluePixel(pix.B, i - l, j - r);
            }
        }

        return box;
    }

    private RGB compressColors(RGB originalRGB, int compressionRate)
    {
        RGB res = new RGB(originalRGB);
        //for (int i = 0; i < 8; ++i)
        //{
        //    for (int j = 0; j < 8; ++j)
        //    {
        //        double r = originalRGB.getR(i, j);
        //        double g = originalRGB.getG(i, j);
        //        double b = originalRGB.getB(i, j);

        //        if (Math.Sqrt(r * r + g * g /*+ b * b*/) >= compressionRate)
        //        {
        //            res.setR(r, i, j);
        //            res.setG(g, i, j);
        //            res.setB(b, i, j);
        //        }
        //    }
        //}

        compressMatrixByHaar(res.getRedPixelsMatrix(), compressionRate);
        compressMatrixByHaar(res.getGreenPixelsMatrix(), compressionRate);
        compressMatrixByHaar(res.getBluePixelsMatrix(), compressionRate);


        return res;   
    }

    private double[][] compressMatrixByHaar(double[][] matrix, int compressionRate)
    {
        for (int i = 0; i < this.boxSize; ++i)
        {
            int columns = this.boxSize;
            double[] temp = new double[this.boxSize];

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

                for (int j = 0; j < this.boxSize; ++j)
                {
                    matrix[i][j] = temp[j];
                }

                columns /= 2;
            }
        }

        for (int i = 0; i < this.boxSize; ++i)
        {
            int rows = this.boxSize;
            double[] temp = new double[this.boxSize];
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

                for (int j = 0; j < this.boxSize; ++j)
                {
                    matrix[j][i] = temp[j];
                }

                rows /= 2;
            }
        }

        List<MatrixElement> elements = new List<MatrixElement>();

        for (int i = 0; i < this.boxSize; ++i)
        {
            for (int j = 0; j < this.boxSize; ++j)
            {
                if (matrix[i][j] > 0)
                {
                    elements.Add(new MatrixElement(i, j, matrix[i][j]));
                }
            }
        }

        int elementsToThrowAway = (int) ((elements.Count / 100.0) * (100 - compressionRate));

        elements.Sort();

        //for (int i = 0; i < this.boxSize; ++i)
        //{
        //    for (int j = 0; j < this.boxSize; ++j)
        //    {
        //        if (Math.Abs(matrix[i][j]) < compressionRate)
        //        {
        //            matrix[i][j] = 0;
        //        }
        //    }
        //}

        int thrown = 0;

        while (thrown < elementsToThrowAway && thrown < elements.Count)
        {
            Tuple<Int32, Int32> pos = elements[thrown].getPosition();
            matrix[pos.Item1][pos.Item2] = 0;
            ++thrown;
        }

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

            return this.value < other.value ? -1 : 1;
        }

        public Tuple<Int32, Int32> getPosition()
        {
            return this.position;
        }
    }

    private double[][] restoreMatrix(double[][] matrix)
    {
        for (int i = 0; i < this.boxSize; ++i)
        {
            int columns = 1;
            double[] temp = new double[this.boxSize];
            while (columns * 2 <= this.boxSize)
            {
                for (int j = 0; j < this.boxSize; ++j)
                {
                    temp[j] = matrix[i][j];
                }

                for (int j = 0; j < columns; ++j)
                {
                    matrix[i][2 * j] = temp[j] + temp[j + columns];
                    matrix[i][2 * j + 1] = temp[j] - temp[j + columns];

                    if (matrix[i][2 * j] > 255)
                    {
                        matrix[i][2 * j] = 255;
                    }
                    if (matrix[i][2 * j + 1] > 255)
                    {
                        matrix[i][2 * j + 1] = 255;
                    }
                }

                columns *= 2;
            }
        }

        for (int i = 0; i < this.boxSize; ++i)
        {
            int rows = 1;
            double[] temp = new double[this.boxSize];
            while (rows * 2 <= this.boxSize)
            {
                for (int j = 0; j < this.boxSize; ++j)
                {
                    temp[j] = matrix[j][i];
                }

                for (int j = 0; j < rows; ++j)
                {
                    matrix[2 * j][i] = temp[j] + temp[j + rows];
                    matrix[2 * j + 1][i] = temp[j] - temp[j + rows];

                    if (matrix[2 * j][i] > 255)
                    {
                        matrix[2 * j][i] = 255;
                    }
                    if (matrix[2 * j + 1][i] > 255)
                    {
                        matrix[2 * j + 1][i] = 255;
                    }
                }


                rows *= 2;
            }
        }

        return matrix;
    }

    private class RGB
    {
        private double[][] red;
        private double[][] green;
        private double[][] blue;

        private int boxSize = 1;

        public RGB(int boxSize)
        {
            this.boxSize = boxSize;

            this.red = new double[this.boxSize][];
            this.green = new double[this.boxSize][];
            this.blue = new double[this.boxSize][];

            for (int i = 0; i < this.boxSize; ++i)
            {
                this.red[i] = new double[this.boxSize];
                this.green[i] = new double[this.boxSize];
                this.blue[i] = new double[this.boxSize];
            }
        }

        public RGB(RGB copy) : this(copy.boxSize)
        {
            for (int i = 0; i < this.boxSize; ++i)
            {
                for (int j = 0; j < this.boxSize; ++j)
                {
                    this.red[i][j] = copy.getRedPixel(i, j);
                    this.green[i][j] = copy.getGreenPixel(i, j);
                    this.blue[i][j] = copy.getBluePixel(i, j);
                }
            }
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
            return this.boxSize;
        }
    }
}
