using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_compression
{
    public class MatrixCompressionDetails
    {
        public double[][] CompressedMatrix { get; set; }
        public int nonzeroElementsNumberOriginal { get; set; }
        public int nonzeroElementsNumberCompressed { get; set; }
    }
}
