using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_compression
{
    public class CompressionStatictics
    {
        public Tuple<int, int> Y { get; set; } = new Tuple<int, int>(0, 0);
        public Tuple<int, int> Cb { get; set; } = new Tuple<int, int>(0, 0);
        public Tuple<int, int> Cr { get; set; } = new Tuple<int, int>(0, 0);
    }
}
