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
            compressionRateUpDown.Hide();
            compressionRateLabel.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image originalImage = Image.FromFile(openFileDialog1.FileName);

                originalImageBox.Image = originalImage;
                compressedImageBox.Hide();

                compressionRateUpDown.Show();
                compressionRateLabel.Show();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            drawInProgressUI();

            int compressionRate = (int) compressionRateUpDown.Value;
            Image compressedImage = new HaarCompression().compressImage(originalImageBox.Image, compressionRate);
            compressedImageBox.Image = compressedImage;

            saveImageIntoFile(compressedImage);
            drawNormalUI();
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
            this.Update();
        }

        private void switchUI(bool onOff)
        {
            compressionRateUpDown.Enabled = onOff;
            chooseImageButton.Enabled = onOff;
        }

        private void drawNormalUI()
        {
            compressionInProgressLabel.Hide();
            compressedImageBox.Show();
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
