using System.Drawing;
using System;

namespace image_compression
{
    class ImageCompositionGuy
    {
        private ImageCompositionGuy()
        {

        }

        public static YCbCrChannelsContainer decompose(Image image) {
            Bitmap sourceBitmap = new Bitmap(image);

            RGBChannelsContainer rgbChannels = new RGBChannelsContainer(sourceBitmap.Height, sourceBitmap.Width);

            for (int i = 0; i < sourceBitmap.Height; ++i)
            {
                for (int j = 0; j < sourceBitmap.Width; ++j)
                {
                    rgbChannels.setRed(i, j, sourceBitmap.GetPixel(j, i).R);
                    rgbChannels.setGreen(i, j, sourceBitmap.GetPixel(j, i).G);
                    rgbChannels.setBlue(i, j, sourceBitmap.GetPixel(j, i).B);
                }
            }

            return ChannelsTransformingGuy.rgbToYCbCr(rgbChannels);
        }

        public static Image compose(RGBChannelsContainer channels)
        {
            Bitmap output = new Bitmap(channels.Width, channels.Height);
            for (int i = 0; i < channels.Height; ++i)
            {
                for (int j = 0; j < channels.Width; ++j)
                {
                    Color color = asColor(channels.getRed(i, j), channels.getGreen(i, j), channels.getBlue(i, j));
                    output.SetPixel(j, i, color);
                }
            }

            return output;
        }

        private static Color asColor(double red, double green, double blue)
        {
            return Color.FromArgb(toRGBRange(red), toRGBRange(green), toRGBRange(blue));
        }

        private static int toRGBRange(double value)
        {
            int val = (int)value;
            return Math.Max(Math.Min(val, 255), 0);
        }
    }
}
