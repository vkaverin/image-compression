using System.Drawing;

public class ImageCompressionDetails
{
    public int Quality { get; set; }
    public Image SourceImage { get; set; }
    public Image CompressedImage { get; set; }
    public int YChannelNonzeroElementsNumberOriginal { get; set; }
    public int YChannelNonzeroElementsNumberCompressed { get; set; }
    public int CbChannelNonzeroElementsNumberOriginal { get; set; }
    public int CbChannelNonzeroElementsNumberCompressed { get; set; }
    public int CrChannelNonzeroElementsNumberOriginal { get; set; }
    public int CrChannelNonzeroElementsNumberCompressed { get; set; }
    public double YChannelMSE { get; set; }
    public double YChannelPSNR { get; set; }
    public double CbChannelMSE { get; set; }
    public double CbChannelPSNR { get; set; }
    public double CrChannelMSE { get; set; }
    public double CrChannelPSNR { get; set; }
    public long CompressionTime { get; set; }

    public double YChannelCompressionRatio()
    {
        return ((double) YChannelNonzeroElementsNumberOriginal) / this.YChannelNonzeroElementsNumberCompressed;
    }

    public double CbChannelCompressionRatio()
    {
        return ((double)CbChannelNonzeroElementsNumberOriginal) / this.CbChannelNonzeroElementsNumberCompressed;
    }

    public double CrChannelCompressionRatio()
    {
        return ((double)CrChannelNonzeroElementsNumberOriginal) / this.CrChannelNonzeroElementsNumberCompressed;
    }
}
