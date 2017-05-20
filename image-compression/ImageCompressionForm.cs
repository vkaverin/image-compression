using System;
using System.IO;
using System.Drawing;
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
            compressionRateLabel.Hide();
            nonZeroBeforeTextLabel.Hide();
            nonZeroBeforeValueLabel.Hide();
            nonZeroAfterTextLabel.Hide();
            nonZeroAfterValueLabel.Hide();
            qualityLabel.Hide();

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
            compressionRateLabel.Hide();
            nonZeroBeforeTextLabel.Hide();
            nonZeroBeforeValueLabel.Hide();
            nonZeroAfterTextLabel.Hide();
            nonZeroAfterValueLabel.Hide();

            redChannelLabel.Show();
            redChannelQualityUpDown.Show();
            greenChannelLabel.Show();
            greenChannelQualityUpDown.Show();
            blueChannelLabel.Show();
            blueChannelQualityUpDown.Show();

            goWorkButton.Show();
            qualityLabel.Show();
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

            compressionRateLabel.Hide();
            nonZeroBeforeTextLabel.Hide();
            nonZeroBeforeValueLabel.Hide();
            nonZeroAfterTextLabel.Hide();
            nonZeroAfterValueLabel.Hide();

            this.Update();
        }

        private void compressionCompletedUIView(CompressionDetails compressionInfo)
        {
            compressionInProgressLabel.Hide();
            compressedImageBox.Show();
            compressionRateLabel.Text = String.Format("Compression rate: {0:F2}", compressionInfo.getRatio());
            compressionRateLabel.Show();
            nonZeroBeforeTextLabel.Show();
            nonZeroBeforeValueLabel.Text = compressionInfo.SourceImageNonZeroPixelsCount.ToString();
            nonZeroBeforeValueLabel.Show();
            nonZeroAfterTextLabel.Show();
            nonZeroAfterValueLabel.Text = compressionInfo.CompressedImageNonZeroPixelsCount.ToString();
            nonZeroAfterValueLabel.Show();

            redChannelQualityUpDown.Enabled = true;
            greenChannelQualityUpDown.Enabled = true;
            blueChannelQualityUpDown.Enabled = true;
            goWorkButton.Enabled = true;
            chooseImageButton.Enabled = true;
            this.Update();
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
            HaarCompression compression = HaarCompression.compress(originalImageBox.Image)
                .withRedQuality(redQuality)
                .withGreenQuality(greenQuality)
                .withBlueQuality(blueQuality)
                .build();
            CompressionDetails compressionDetails = compression.process();
            compressedImageBox.Image = compressionDetails.CompressedImage;

            saveToFile(compressionDetails.CompressedImage);
            compressionCompletedUIView(compressionDetails);
        }
    }
}
