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
    public float YChannelMSE { get; set; }
    public float YChannelPSNR { get; set; }
    public float CbChannelMSE { get; set; }
    public float CbChannelPSNR { get; set; }
    public float CrChannelMSE { get; set; }
    public float CrChannelPSNR { get; set; }
    public long CompressionTime { get; set; }

    public float YChannelCompressionRatio()
    {
        return ((float) YChannelNonzeroElementsNumberOriginal) / this.YChannelNonzeroElementsNumberCompressed;
    }

    public float CbChannelCompressionRatio()
    {
        return ((float)CbChannelNonzeroElementsNumberOriginal) / this.CbChannelNonzeroElementsNumberCompressed;
    }

    public float CrChannelCompressionRatio()
    {
        return ((float)CrChannelNonzeroElementsNumberOriginal) / this.CrChannelNonzeroElementsNumberCompressed;
    }
}
