namespace image_compression
{
    public class ChannelsTransformingGuy
    { 
        private ChannelsTransformingGuy()
        {
        }

        public static YCbCrChannelsContainer rgbToYCbCr(RGBChannelsContainer rgbChannels)
        {
            YCbCrChannelsContainer container = new YCbCrChannelsContainer(rgbChannels.Height, rgbChannels.Width);

            for (int i = 0; i < rgbChannels.Height; ++i)
            {
                for (int j = 0; j < rgbChannels.Width; ++j)
                {
                    int r = rgbChannels.getRed(i, j);
                    int g = rgbChannels.getGreen(i, j);
                    int b = rgbChannels.getBlue(i, j);

                    float y = (float) (0.299 * r + 0.587 * g + 0.114 * b);
                    float cb = (float)(128 - 0.168736 * r - 0.33264 * g + 0.5 * b);
                    float cr = (float)(128 + 0.5 * r - 0.418688 * g - 0.081312 * b);

                    container.setY(i, j, y);
                    container.setCb(i, j, cb);
                    container.setCr(i, j, cr);
                }
            }

            return container;
        }

        public static RGBChannelsContainer yCbCrToRGB(YCbCrChannelsContainer yCbCrChannels)
        {
            RGBChannelsContainer container = new RGBChannelsContainer(yCbCrChannels.Height, yCbCrChannels.Width);

            for (int i = 0; i < yCbCrChannels.Height; ++i)
            {
                for (int j = 0; j < yCbCrChannels.Width; ++j)
                {
                    float y = yCbCrChannels.getY(i, j);
                    float cb = yCbCrChannels.getCb(i, j);
                    float cr = yCbCrChannels.getCr(i, j);

                    int r = (int)(y + 1.402 * (cr - 128));
                    int g = (int)(y - 0.344136 * (cb - 128) - 0.714136 * (cr - 128));
                    int b = (int)(y + 1.772 * (cb - 128));

                    container.setRed(i, j, r);
                    container.setGreen(i, j, g);
                    container.setBlue(i, j, b);
                }
            }

            return container;
        }
    }

    
}
