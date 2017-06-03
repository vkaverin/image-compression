namespace image_compression
{
    public class RGBChannelsContainer : AbstractChannelsContainer<byte>
    {
        private const int RED = 0;
        private const int GREEN = 1;
        private const int BLUE = 2;

        public RGBChannelsContainer(int height, int width) : base(3, height, width)
        {
        }

        public void setRed(int n, int m, byte value)
        {
            setValueOfChannel(RED, n, m, value);
        }

        public void setGreen(int n, int m, byte value)
        {
            setValueOfChannel(GREEN, n, m, value);
        }

        public void setBlue(int n, int m, byte value)
        {
            setValueOfChannel(BLUE, n, m, value);
        }

        public byte getRed(int n, int m)
        {
            return (byte) getValueOfChannel(RED, n, m);
        }

        public byte getGreen(int n, int m)
        {
            return (byte)getValueOfChannel(GREEN, n, m);
        }

        public byte getBlue(int n, int m)
        {
            return (byte)getValueOfChannel(BLUE, n, m);
        }

        public void setRed(byte[][] values)
        {
            fillChannel(RED, values);
        }

        public void setGreen(byte[][] values)
        {
            fillChannel(GREEN, values);
        }

        public void setBlue(byte[][] values)
        {
            fillChannel(BLUE, values);
        }

        public byte[][] RedChannel()
        {
            return getChannelAsMatrix(RED);
        }

        public byte[][] GreenChannel()
        {
            return getChannelAsMatrix(GREEN);
        }

        public byte[][] BlueChannel()
        {
            return getChannelAsMatrix(BLUE);
        }
    }
}
