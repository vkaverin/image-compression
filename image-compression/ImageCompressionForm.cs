using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace image_compression
{
    public partial class ImageCompressionForm : Form
    {
        public ImageCompressionForm()
        {
            InitializeComponent();
            initialView();
        }

        private void initialView()
        {
            originalImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            originalImageFlowLayoutPanel.Controls.Add(originalImageBox);

            compressedImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            compressedImageFlowLayoutPanel.Controls.Add(compressedImageBox);

            // Labels
            redChannelLabel.Hide();
            greenChannelLabel.Hide();
            blueChannelLabel.Hide();
            compressionInProgressLabel.Hide();
            qualityLabel.Hide();
            statisticsGroupBox.Hide();
            statisticsLabel.Hide();

            // Buttons & Co
            redChannelQualityUpDown.Hide();
            greenChannelQualityUpDown.Hide();
            blueChannelQualityUpDown.Hide();
            goWorkButton.Hide();
        }

        private void openNewImage_button(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image originalImage = Image.FromFile(openFileDialog1.FileName);
                originalImageBox.Image = originalImage;

                newImageChosenView();
            }
        }

        private void newImageChosenView()
        {
            compressedImageBox.Hide();

            redChannelLabel.Show();
            redChannelQualityUpDown.Show();
            greenChannelLabel.Show();
            greenChannelQualityUpDown.Show();
            blueChannelLabel.Show();
            blueChannelQualityUpDown.Show();

            goWorkButton.Show();
            qualityLabel.Show();
            statisticsGroupBox.Hide();
            statisticsLabel.Hide();
        }

        private static void saveToFile(Image image)
        {
            String storageDirectory = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "compressed";
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
            string filename = storageDirectory + System.IO.Path.DirectorySeparatorChar + "image-" + DateTime.Now.ToFileTime() + ".jpg";
            image.Save(filename);
        }

        private void inProgressUIView()
        {
            redChannelQualityUpDown.Enabled = false;
            greenChannelQualityUpDown.Enabled = false;
            blueChannelQualityUpDown.Enabled = false;
            goWorkButton.Enabled = false;
            chooseImageButton.Enabled = false;

            compressedImageBox.Hide();
            compressionInProgressLabel.Show();

            this.Update();
        }

        private void compressionCompletedUIView(ImageCompressionDetails compressionDetails)
        {
            compressionInProgressLabel.Hide();
            compressedImageBox.Show();
            fillStatistics(compressionDetails);

            redChannelQualityUpDown.Enabled = true;
            greenChannelQualityUpDown.Enabled = true;
            blueChannelQualityUpDown.Enabled = true;
            goWorkButton.Enabled = true;
            chooseImageButton.Enabled = true;
            this.Update();
        }

        private void fillStatistics(ImageCompressionDetails compressionDetails)
        {
            StringBuilder statistics = new StringBuilder();
            statistics.AppendFormat("Compression took {0} second(s)\n", compressionDetails.CompressionTime / 1000.0);
            statistics.AppendFormat("\nRed channel:\n");
            statistics.AppendFormat("  Number of nonzero elements in original image: {0}\n", compressionDetails.RedChannelNonzeroElementsNumberOriginal);
            statistics.AppendFormat("  Number of nonzero elements in compressed image: {0}\n", compressionDetails.RedChannelNonzeroElementsNumberCompressed);
            statistics.AppendFormat("  Compression ratio: {0:F3}\n", compressionDetails.RedChannelCompressionRatio());
            statistics.AppendFormat("  MSE: {0:F3}\n", compressionDetails.RedChannelMSE);
            statistics.AppendFormat("  PSRN: {0:F3}\n", compressionDetails.RedChannelPSNR);
            statistics.AppendFormat("\nGreen channel:\n");
            statistics.AppendFormat("  Number of nonzero elements in original image: {0}\n", compressionDetails.GreenChannelNonzeroElementsNumberOriginal);
            statistics.AppendFormat("  Number of nonzero elements in compressed image: {0}\n", compressionDetails.GreenChannelNonzeroElementsNumberCompressed);
            statistics.AppendFormat("  Compression ratio: {0:F3}\n", compressionDetails.GreenChannelCompressionRatio());
            statistics.AppendFormat("  MSE: {0:F3}\n", compressionDetails.GreenChannelMSE);
            statistics.AppendFormat("  PSRN: {0:F3}\n", compressionDetails.GreenChannelPSNR);
            statistics.AppendFormat("\nBlue channel:\n");
            statistics.AppendFormat("  Number of nonzero elements in original image: {0}\n", compressionDetails.BlueChannelNonzeroElementsNumberOriginal);
            statistics.AppendFormat("  Number of nonzero elements in compressed image: {0}\n", compressionDetails.BlueChannelNonzeroElementsNumberCompressed);
            statistics.AppendFormat("  Compression ratio: {0:F3}\n", compressionDetails.BlueChannelCompressionRatio());
            statistics.AppendFormat("  MSE: {0:F3}\n", compressionDetails.BlueChannelMSE);
            statistics.AppendFormat("  PSRN: {0:F3}\n", compressionDetails.BlueChannelPSNR);
            statisticsLabel.Text = statistics.ToString();
            statisticsGroupBox.Show();
            statisticsLabel.Show();
        }

        private void qualityUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.goWorkButton_click(sender, e);
            }
        }

        private void goWorkButton_click(object sender, EventArgs e)
        {
            inProgressUIView();

            int redQuality = (int) redChannelQualityUpDown.Value;
            int greenQuality = (int) greenChannelQualityUpDown.Value;
            int blueQuality = (int) blueChannelQualityUpDown.Value;
            CompressionTemplate compressionTemplate = HaarCompression.makeCompressionTemplateFor(originalImageBox.Image)
                .withRedQuality(redQuality)
                .withGreenQuality(greenQuality)
                .withBlueQuality(blueQuality)
                .make();
            ImageCompressionDetails compressionDetails = HaarCompression.process(compressionTemplate);
            compressedImageBox.Image = compressionDetails.CompressedImage;

            saveToFile(compressionDetails.CompressedImage);
            compressionCompletedUIView(compressionDetails);
        }
    }
}
