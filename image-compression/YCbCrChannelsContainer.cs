namespace image_compression
{
    public class YCbCrChannelsContainer : AbstractChannelsContainer<float>
    {
        private const int Y = 0;
        private const int CB = 1;
        private const int CR = 2;

        public YCbCrChannelsContainer(int height, int width) : base(3, height, width)
        {
        }

        public void setY(int n, int m, float value)
        {
            setValueOfChannel(Y, n, m, value);
        }

        public void setCb(int n, int m, float value)
        {
            setValueOfChannel(CB, n, m, value);
        }

        public void setCr(int n, int m, float value)
        {
            setValueOfChannel(CR, n, m, value);
        }

        public float getY(int n, int m)
        {
            return (int)getValueOfChannel(Y, n, m);
        }

        public float getCb(int n, int m)
        {
            return (int)getValueOfChannel(CB, n, m);
        }

        public float getCr(int n, int m)
        {
            return (int)getValueOfChannel(CR, n, m);
        }

        public void setY(float[][] values)
        {
            fillChannel(Y, values);
        }

        public void setCb(float[][] values)
        {
            fillChannel(CB, values);
        }

        public void setCr(float[][] values)
        {
            fillChannel(CR, values);
        }

        public float[][] YChannel()
        {
            return getChannelAsMatrix(Y);
        }

        public float[][] CbChannel()
        {
            return getChannelAsMatrix(CB);
        }

        public float[][] CrChannel()
        {
            return getChannelAsMatrix(CR);
        }
    }
}
