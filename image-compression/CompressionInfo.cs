using System;
using System.Drawing;
using System.Collections.Generic;

public class CompressionInfo
{
    public int Quality { get; set; }
    public Image Source { get; set; }
    public Image Target { get; set; }
    public int SourceNonZero { get; set; }
    public int CompressedNonZero { get; set; }

    public double getRatio()
    {
        return (this.SourceNonZero + 0.0) / this.CompressedNonZero;
    }
}
