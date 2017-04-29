using System;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace image_compression
{
    public partial class ImageCompressionForm : Form
    {
        public ImageCompressionForm()
        {
            InitializeComponent();

            originalImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            originalImageFlowLayoutPanel.Controls.Add(originalImageBox);

            compressedImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            compressedImageFlowLayoutPanel.Controls.Add(compressedImageBox);

            hideLabels(compressionInProgressLabel,
            compressionRateLabel,
            nonZeroBeforeTextLabel,
            nonZeroBeforeValueLabel,
            nonZeroAfterTextLabel,
            nonZeroAfterValueLabel,
            qualityLabel);
            
            compressionRateUpDown.Hide();
        }

        private void hideLabels(params Label[] labels)
        {
            foreach (Label label in labels)
            {
                label.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image originalImage = Image.FromFile(openFileDialog1.FileName);

                originalImageBox.Image = originalImage;
                compressedImageBox.Hide();

                hideLabels(nonZeroBeforeTextLabel,
                    nonZeroBeforeValueLabel,
                    nonZeroAfterTextLabel,
                    nonZeroAfterValueLabel);

                compressionRateUpDown.Show();
                qualityLabel.Show();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            drawInProgressUI();

            int quality = (int) compressionRateUpDown.Value;
            CompressionInfo compressionInfo = new HaarCompression(quality).compressImage(originalImageBox.Image);
            compressedImageBox.Image = compressionInfo.Target;

            saveImageIntoFile(compressionInfo.Target);
            drawAfterCompressionUI(compressionInfo);
        }

        private static void saveImageIntoFile(Image image)
        {
            String storageDirectory = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "compressed";
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
            string filename = storageDirectory + System.IO.Path.DirectorySeparatorChar + "image-" + DateTime.Now.ToFileTime() + ".jpg";
            image.Save(filename);
        }

        private void drawInProgressUI()
        {
            switchUI(false);
            compressedImageBox.Hide();
            compressionInProgressLabel.Show();

            hideLabels(compressionRateLabel,
            nonZeroBeforeTextLabel,
            nonZeroBeforeValueLabel,
            nonZeroAfterTextLabel,
            nonZeroAfterValueLabel);

            this.Update();
        }

        private void switchUI(bool onOff)
        {
            compressionRateUpDown.Enabled = onOff;
            chooseImageButton.Enabled = onOff;
        }

        private void drawAfterCompressionUI(CompressionInfo compressionInfo)
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

            switchUI(true);
            this.Update();
        }

        private void compressionRateUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.numericUpDown1_ValueChanged(sender, e);
            }
        }
    }
}
