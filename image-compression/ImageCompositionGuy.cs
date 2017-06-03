using System.Drawing;
using System;

namespace image_compression
{
    class ImageCompositionGuy
    {
        private ImageCompositionGuy()
        {

        }

        public static YCbCrChannelsContainer decompose(Bitmap source, int x, int y, int size) {
            YCbCrChannelsContainer channels = new YCbCrChannelsContainer(size, size);

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    float[] yCbCr = extractYCbCr(source, x + j, y + i);
                    channels.setY(i, j, yCbCr[0]);
                    channels.setCb(i, j, yCbCr[1]);
                    channels.setCr(i, j, yCbCr[2]);
                }
            }

            return channels;
        }

        public static void compose(Bitmap output, RGBChannelsContainer channels, int x, int y)
        {
            for (int i = 0; i < channels.Height; ++i)
            {
                for (int j = 0; j < channels.Width; ++j)
                {
                    byte[] rgb = new byte[] { channels.getRed(i, j), channels.getGreen(i, j), channels.getBlue(i, j) };
                    Color color = asRgbColor(rgb);
                    output.SetPixel(x + j, y + i, color);
                }
            }
        }

        public static float[] extractYCbCr(Bitmap source, int x, int y)
        {
            return ChannelsTransformingGuy.rgbToYCbCr(extractRgb(source, x, y));
        }

        public static byte[] extractRgb(Bitmap source, int x, int y)
        {
            return new byte[] {
                        source.GetPixel(x, y).R,
                        source.GetPixel(x, y).G,
                        source.GetPixel(x, y).B
                    };
        }

        private static Color asRgbColor(byte[] rgb)
        {
            return Color.FromArgb(rgb[0], rgb[1], rgb[2]);
        }
    }
}
