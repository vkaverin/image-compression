namespace image_compression
{
    public class RGBChannelsContainer : AbstractChannelsContainer
    {
        private const int RED = 0;
        private const int GREEN = 1;
        private const int BLUE = 2;

        public RGBChannelsContainer(int height, int width) : base(3, height, width)
        {
        }

        public void setRed(int n, int m, int value)
        {
            setValueOfChannel(RED, n, m, value);
        }

        public void setGreen(int n, int m, int value)
        {
            setValueOfChannel(GREEN, n, m, value);
        }

        public void setBlue(int n, int m, int value)
        {
            setValueOfChannel(BLUE, n, m, value);
        }

        public int getRed(int n, int m)
        {
            return (int) getValueOfChannel(RED, n, m);
        }

        public int getGreen(int n, int m)
        {
            return (int)getValueOfChannel(GREEN, n, m);
        }

        public int getBlue(int n, int m)
        {
            return (int)getValueOfChannel(BLUE, n, m);
        }

        public void setRed(double[][] values)
        {
            fillChannel(RED, values);
        }

        public void setGreen(double[][] values)
        {
            fillChannel(GREEN, values);
        }

        public void setBlue(double[][] values)
        {
            fillChannel(BLUE, values);
        }

        public double[][] RedChannel()
        {
            return getChannelAsMatrix(RED);
        }

        public double[][] GreenChannel()
        {
            return getChannelAsMatrix(GREEN);
        }

        public double[][] BlueChannel()
        {
            return getChannelAsMatrix(BLUE);
        }
    }
}
