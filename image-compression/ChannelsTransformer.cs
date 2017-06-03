using System;

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
                    byte[] rgb = new byte[3] { rgbChannels.getRed(i, j), rgbChannels.getGreen(i, j), rgbChannels.getBlue(i, j) };
                    float[] yCbCr = rgbToYCbCr(rgb);

                    container.setY(i, j, yCbCr[0]);
                    container.setCb(i, j, yCbCr[1]);
                    container.setCr(i, j, yCbCr[2]);
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
                    float[] yCbCr = new float[3] { yCbCrChannels.getY(i, j), yCbCrChannels.getCb(i, j), yCbCrChannels.getCr(i, j) };
                    byte[] rgb = yCbCrToRGB(yCbCr);

                    container.setRed(i, j, rgb[0]);
                    container.setGreen(i, j, rgb[1]);
                    container.setBlue(i, j, rgb[2]);
                }
            }

            return container;
        }

        public static byte[] yCbCrToRGB(float[] values)
        {
            float y = values[0];
            float cb = values[1];
            float cr = values[2];

            return new byte[3]
            {
                toRGBRange(y + 1.402 * (cr - 128)),
                toRGBRange(y - 0.344136 * (cb - 128) - 0.714136 * (cr - 128)),
                toRGBRange(y + 1.772 * (cb - 128))
            };
        }

        public static float[] rgbToYCbCr(byte[] values)
        {
            byte r = values[0];
            byte g = values[1];
            byte b = values[2];

            return new float[3]
            {
                (float) (0.299 * r + 0.587 * g + 0.114 * b),
                (float) (128 - 0.168736 * r - 0.33264 * g + 0.5 * b),
                (float) (128 + 0.5 * r - 0.418688 * g - 0.081312 * b)
        };
        }

        private static byte toRGBRange(double value)
        {
            return (byte) Math.Max(Math.Min(value, 255), 0);
        }
    }

    
}
