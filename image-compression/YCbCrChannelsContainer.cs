namespace image_compression
{
    public class YCbCrChannelsContainer : AbstractChannelsContainer
    {
        private const int Y = 0;
        private const int CB = 1;
        private const int CR = 2;

        public YCbCrChannelsContainer(int height, int width) : base(3, height, width)
        {
        }

        public void setY(int n, int m, double value)
        {
            setValueOfChannel(Y, n, m, value);
        }

        public void setCb(int n, int m, double value)
        {
            setValueOfChannel(CB, n, m, value);
        }

        public void setCr(int n, int m, double value)
        {
            setValueOfChannel(CR, n, m, value);
        }

        public double getY(int n, int m)
        {
            return (int)getValueOfChannel(Y, n, m);
        }

        public double getCb(int n, int m)
        {
            return (int)getValueOfChannel(CB, n, m);
        }

        public double getCr(int n, int m)
        {
            return (int)getValueOfChannel(CR, n, m);
        }

        public void setY(double[][] values)
        {
            fillChannel(Y, values);
        }

        public void setCb(double[][] values)
        {
            fillChannel(CB, values);
        }

        public void setCr(double[][] values)
        {
            fillChannel(CR, values);
        }

        public double[][] YChannel()
        {
            return getChannelAsMatrix(Y);
        }

        public double[][] CbChannel()
        {
            return getChannelAsMatrix(CB);
        }

        public double[][] CrChannel()
        {
            return getChannelAsMatrix(CR);
        }
    }
}
