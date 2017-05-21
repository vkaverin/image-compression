using System.Drawing;

public class ImageCompressionDetails
{
    public int Quality { get; set; }
    public Image SourceImage { get; set; }
    public Image CompressedImage { get; set; }
    public int RedChannelNonzeroElementsNumberOriginal { get; set; }
    public int RedChannelNonzeroElementsNumberCompressed { get; set; }
    public int GreenChannelNonzeroElementsNumberOriginal { get; set; }
    public int GreenChannelNonzeroElementsNumberCompressed { get; set; }
    public int BlueChannelNonzeroElementsNumberOriginal { get; set; }
    public int BlueChannelNonzeroElementsNumberCompressed { get; set; }
    public long CompressionTime { get; set; }

    public double RedChannelCompressionRatio()
    {
        return ((double) RedChannelNonzeroElementsNumberOriginal) / this.RedChannelNonzeroElementsNumberCompressed;
    }

    public double GreenChannelCompressionRatio()
    {
        return ((double)GreenChannelNonzeroElementsNumberOriginal) / this.GreenChannelNonzeroElementsNumberCompressed;
    }

    public double BlueChannelCompressionRatio()
    {
        return ((double)BlueChannelNonzeroElementsNumberOriginal) / this.BlueChannelNonzeroElementsNumberCompressed;
    }
}
