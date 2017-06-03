using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace image_compression
{
    class ConcurrentImageCompressor
    {
        private const int PIECE_SIZE = 8;

        private CompressionTemplate template;

        public ConcurrentImageCompressor(CompressionTemplate template)
        {
            this.template = template;
        }

        public ImageCompressionDetails process()
        {
            Bitmap input = new Bitmap(this.template.SourceImage);
            Bitmap output = null;
            generateOutput(ref input, out output);

            Stopwatch watches = Stopwatch.StartNew();
            CompressionStatictics stats = distributeWork(input, output);      

            ImageCompressionDetails details = new ImageCompressionDetails();
            details.SourceImage = template.SourceImage;
            details.CompressedImage = output;
            details.YChannelNonzeroElementsNumberOriginal = stats.Y.Item1;
            details.YChannelNonzeroElementsNumberCompressed = stats.Y.Item2;
            details.CbChannelNonzeroElementsNumberOriginal = stats.Cb.Item1;
            details.CbChannelNonzeroElementsNumberCompressed = stats.Cb.Item2;
            details.CrChannelNonzeroElementsNumberOriginal = stats.Cr.Item1;
            details.CrChannelNonzeroElementsNumberCompressed = stats.Cr.Item2;

            if (this.template.estimateErrors)
            {
                ErrorsEstimationService.estimateErrors(input, output, details);
            }
            watches.Stop();
            details.CompressionTime = watches.ElapsedMilliseconds;

            return details;
        }

        private void generateOutput(ref Bitmap input, out Bitmap output)
        {
            int width = (input.Width / PIECE_SIZE) * PIECE_SIZE;
            int height = (input.Height / PIECE_SIZE) * PIECE_SIZE;

            output = new Bitmap(width, height);
        }

        private CompressionStatictics distributeWork(Bitmap input, Bitmap output)
        {
            int pieces = 0;

            ConcurrentQueue<Tuple<Part, CompressionStatictics>> queue = new ConcurrentQueue<Tuple<Part, CompressionStatictics>>();

            for (int i = 0; i + PIECE_SIZE < input.Height; i += PIECE_SIZE)
            {
                for (int j = 0; j + PIECE_SIZE < input.Width; j += PIECE_SIZE)
                {

                    YCbCrChannelsContainer channels = ImageCompositionGuy.decompose(input, j, i, PIECE_SIZE);
                    int x = j;
                    int y = i;
                    Task task = Task.Factory.StartNew(() => {
                        CompressionStatictics statictics = ChannelsCompressionService.get().compress(channels, this.template);
                        ChannelsCompressionService.get().decompress(channels);
                        Part part = new Part(x, y, channels);
                        queue.Enqueue(new Tuple<Part, CompressionStatictics>(part, statictics));
                    }
                    );
                    ++pieces;
                }
            }

            CompressionStatictics stats = new CompressionStatictics();

            for (int i = 0; i < pieces; ++i)
            {
                Tuple<Part, CompressionStatictics> result;
                queue.TryDequeue(out result);

                YCbCrChannelsContainer channels = result.Item1.Channels;
                int x = result.Item1.X;
                int y = result.Item1.Y;
                ImageCompositionGuy.compose(output, ChannelsTransformingGuy.yCbCrToRGB(channels), x, y);
                accumulateStatistics(stats, result.Item2);
            }

            return stats;
        }

        private void accumulateStatistics(CompressionStatictics total, CompressionStatictics next)
        {
            total.Y = new Tuple<int, int>(total.Y.Item1 + next.Y.Item1, total.Y.Item2 + next.Y.Item2);
            total.Cb = new Tuple<int, int>(total.Cb.Item1 + next.Cb.Item1, total.Cb.Item2 + next.Cb.Item2);
            total.Cr = new Tuple<int, int>(total.Cr.Item1 + next.Cr.Item1, total.Cr.Item2 + next.Cr.Item2);
        }

        private class Part
        {
            public int X { get; set; }
            public int Y { get; set; }
            public YCbCrChannelsContainer Channels { get; set; }

            public Part(int x, int y, YCbCrChannelsContainer channels)
            {
                this.X = x;
                this.Y = y;
                this.Channels = channels;
            }
        }
    }
}
