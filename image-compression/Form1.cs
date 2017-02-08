using System;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace image_compression
{
    public partial class imageCompressionForm : Form
    {
        public imageCompressionForm()
        {
            InitializeComponent();

            originalImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            originalImageFlowLayoutPanel.Controls.Add(originalImageBox);

            compressedImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            compressedImageFlowLayoutPanel.Controls.Add(compressedImageBox);

            compressionInProgressLabel.Hide();
            compressionRateLabel.Hide();
            compressionRateUpDown.Hide();
            qualityLabel.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image originalImage = Image.FromFile(openFileDialog1.FileName);

                originalImageBox.Image = originalImage;
                compressedImageBox.Hide();

                compressionRateUpDown.Show();
                qualityLabel.Show();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            drawInProgressUI();

            int compressionRate = (int) compressionRateUpDown.Value;
            Tuple<Image, double> compression = new HaarCompression().compressImage(originalImageBox.Image, compressionRate);
            compressedImageBox.Image = compression.Item1;

            saveImageIntoFile(compression.Item1);
            drawAfterCompressionUI(compression.Item2);
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
            compressionRateLabel.Hide();
            this.Update();
        }

        private void switchUI(bool onOff)
        {
            compressionRateUpDown.Enabled = onOff;
            chooseImageButton.Enabled = onOff;
        }

        private void drawAfterCompressionUI(double compressionRate)
        {
            compressionInProgressLabel.Hide();
            compressedImageBox.Show();
            compressionRateLabel.Text = String.Format("Compression rate: {0:F2}", compressionRate);
            compressionRateLabel.Show();
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
