using System;
using System.ComponentModel;
using System.IO;
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
            yChannelLabel.Hide();
            greenChannelLabel.Hide();
            crChannelLabel.Hide();
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

            yChannelLabel.Show();
            redChannelQualityUpDown.Show();
            greenChannelLabel.Show();
            greenChannelQualityUpDown.Show();
            crChannelLabel.Show();
            blueChannelQualityUpDown.Show();

            goWorkButton.Show();
            qualityLabel.Show();
            statisticsGroupBox.Hide();
            statisticsLabel.Hide();
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
            statistics.AppendFormat("\nY channel:\n");
            statistics.AppendFormat("  Number of nonzero elements in original image: {0}\n", compressionDetails.YChannelNonzeroElementsNumberOriginal);
            statistics.AppendFormat("  Number of nonzero elements in compressed image: {0}\n", compressionDetails.YChannelNonzeroElementsNumberCompressed);
            statistics.AppendFormat("  Compression ratio: {0:F3}\n", compressionDetails.YChannelCompressionRatio());
            statistics.AppendFormat("  MSE: {0:F3}\n", compressionDetails.YChannelMSE);
            statistics.AppendFormat("  PSRN: {0:F3}\n", compressionDetails.YChannelPSNR);
            statistics.AppendFormat("\nCb channel:\n");
            statistics.AppendFormat("  Number of nonzero elements in original image: {0}\n", compressionDetails.CbChannelNonzeroElementsNumberOriginal);
            statistics.AppendFormat("  Number of nonzero elements in compressed image: {0}\n", compressionDetails.CbChannelNonzeroElementsNumberCompressed);
            statistics.AppendFormat("  Compression ratio: {0:F3}\n", compressionDetails.CbChannelCompressionRatio());
            statistics.AppendFormat("  MSE: {0:F3}\n", compressionDetails.CbChannelMSE);
            statistics.AppendFormat("  PSRN: {0:F3}\n", compressionDetails.CbChannelPSNR);
            statistics.AppendFormat("\nCr channel:\n");
            statistics.AppendFormat("  Number of nonzero elements in original image: {0}\n", compressionDetails.CrChannelNonzeroElementsNumberOriginal);
            statistics.AppendFormat("  Number of nonzero elements in compressed image: {0}\n", compressionDetails.CrChannelNonzeroElementsNumberCompressed);
            statistics.AppendFormat("  Compression ratio: {0:F3}\n", compressionDetails.CrChannelCompressionRatio());
            statistics.AppendFormat("  MSE: {0:F3}\n", compressionDetails.CrChannelMSE);
            statistics.AppendFormat("  PSRN: {0:F3}\n", compressionDetails.CrChannelPSNR);
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

            int redQuality = (int)redChannelQualityUpDown.Value;
            int greenQuality = (int)greenChannelQualityUpDown.Value;
            int blueQuality = (int)blueChannelQualityUpDown.Value;
            CompressionTemplate compressionTemplate = CompressionTemplateBuildingGuy.forImage(originalImageBox.Image)
                .withYQuality(redQuality)
                .withCbQuality(greenQuality)
                .withCrQuality(blueQuality)
                .make();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(compressionWorker_doWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(compressionWorker_completed);
            worker.RunWorkerAsync(compressionTemplate);
        }

        private void compressionWorker_doWork(object sender, DoWorkEventArgs args)
        {
            CompressionTemplate compressionTemplate = (CompressionTemplate)args.Argument;
            args.Result = ImageCompressionGuy.compressByTemplate(compressionTemplate);
        }

        private void compressionWorker_completed(object sender, RunWorkerCompletedEventArgs args)
        {
            ImageCompressionDetails compressionDetails = (ImageCompressionDetails)args.Result;
            compressedImageBox.Image = compressionDetails.CompressedImage;
            compressionCompletedUIView(compressionDetails);
        }

        private void saveImageButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                save(compressedImageBox.Image, saveFileDialog1.FileName);
            }
        }

        private void save(Image image, string name)
        {
            string storageDirectory = Application.StartupPath + System.IO.Path.DirectorySeparatorChar + "output";
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }

            string path = name;
            image.Save(path);
        }
    }
}
