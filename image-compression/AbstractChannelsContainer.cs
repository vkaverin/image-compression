using System;

namespace image_compression
{
    public abstract class AbstractChannelsContainer
    {
        public int Width { get; set; }
        public int Height { get; set; }

        protected double[][][] channels;

        public AbstractChannelsContainer(int numberOfChannels, int height, int width)
        {
            this.Width = width;
            this.Height = height;

            this.channels = new double[numberOfChannels][][];

            for (int i = 0; i < channels.Length; ++i)
            {
                alloc(out channels[i]);
            }
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

        protected void setValueOfChannel(int numberOfChannel, int n, int m, double value)
        {
            validateChannelNumber(numberOfChannel);

            this.channels[numberOfChannel][n][m] = value;
        }

        protected double getValueOfChannel(int numberOfChannel, int n, int m)
        {
            validateChannelNumber(numberOfChannel);

            return this.channels[numberOfChannel][n][m];
        }

        protected void fillChannel(int numberOfChannel, double[][] values)
        {
            validateChannelNumber(numberOfChannel);

            if (this.Height != values.Length)
            {
                throw new ArgumentException("Wrong height.");
            }

            for (int i = 0; i < values.Length; ++i)
            {
                if (this.Width != values[i].Length)
                {
                    throw new ArgumentException("Wrong width.");
                }

                for (int j = 0; j < values[i].Length; ++j)
                {
                    this.setValueOfChannel(numberOfChannel, i, j, values[i][j]);
                }
            }
        }

        protected double[][] getChannelAsMatrix(int numberOfChannel)
        {
            validateChannelNumber(numberOfChannel);

            return this.channels[numberOfChannel];
        }

        private void validateChannelNumber(int numberOfChannel)
        {
            if (numberOfChannel < 0 || numberOfChannel > this.channels.Length)
            {
                throw new ArgumentOutOfRangeException(String.Format("There's no channel with number {}.", numberOfChannel));
            }
        }
    }
}
