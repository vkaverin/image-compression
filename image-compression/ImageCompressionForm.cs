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
            compressionInProgressLabel.Hide();
            compressionRateLabel.Hide();
            nonZeroBeforeTextLabel.Hide();
            nonZeroBeforeValueLabel.Hide();
            nonZeroAfterTextLabel.Hide();
            nonZeroAfterValueLabel.Hide();
            qualityLabel.Hide();

            // Buttons & Co
            compressionRateUpDown.Hide();
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

            compressionRateUpDown.Show();
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
            compressionRateUpDown.Enabled = false;
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

        private void compressionCompletedUIView(CompressionInfo compressionInfo)
        {
            compressionInProgressLabel.Hide();
            compressedImageBox.Show();
            compressionRateLabel.Text = String.Format("Compression rate: {0:F2}", compressionInfo.getRatio());
            compressionRateLabel.Show();
            nonZeroBeforeTextLabel.Show();
            nonZeroBeforeValueLabel.Text = compressionInfo.SourceNonZero.ToString();
            nonZeroBeforeValueLabel.Show();
            nonZeroAfterTextLabel.Show();
            nonZeroAfterValueLabel.Text = compressionInfo.CompressedNonZero.ToString();
            nonZeroAfterValueLabel.Show();

            compressionRateUpDown.Enabled = true;
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

            int quality = (int) compressionRateUpDown.Value;
            CompressionInfo compressionInfo = new HaarCompression(quality).compressImage(originalImageBox.Image);
            compressedImageBox.Image = compressionInfo.Target;

            saveToFile(compressionInfo.Target);
            compressionCompletedUIView(compressionInfo);
        }
    }
}
