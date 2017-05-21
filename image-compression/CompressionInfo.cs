using System;
using System.Drawing;
using System.Collections.Generic;

public class CompressionDetails
{
    public int Quality { get; set; }
    public Image SourceImage { get; set; }
    public Image CompressedImage { get; set; }
    public int SourceImageNonZeroPixelsCount { get; set; }
    public int CompressedImageNonZeroPixelsCount { get; set; }

    public double getRatio()
    {
        return (this.SourceImageNonZeroPixelsCount + 0.0) / this.CompressedImageNonZeroPixelsCount;
    }
}
