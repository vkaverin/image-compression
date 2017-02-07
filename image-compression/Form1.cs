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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void originalImageBox_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            drawInProgressUI();

            int compressionRate = (int) compressionRateUpDown.Value;
            Image compressedImage = new HaarCompression().compressImage(originalImageBox.Image, compressionRate);
            compressedImageBox.Image = compressedImage;

            saveImageIntoFile(compressedImage)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

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
