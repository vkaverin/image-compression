using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if (compressionRate > 0)
            {
                String pathToComressedImage = new HaarCompression().compressImage(originalImageBox.Image, compressionRate);
                Image compressedImage = Image.FromFile(pathToComressedImage);
                compressedImageBox.Image = compressedImage;
            } else
            {
                compressedImageBox.Image = originalImageBox.Image;
            }

            drawNormalUI();
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
