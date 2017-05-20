namespace image_compression
{
    partial class ImageCompressionForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.originalImageBox = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chooseImageButton = new System.Windows.Forms.Button();
            this.originalImageFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.compressedImageFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.compressedImageBox = new System.Windows.Forms.PictureBox();
            this.compressionInProgressLabel = new System.Windows.Forms.Label();
            this.blueChannelQualityUpDown = new System.Windows.Forms.NumericUpDown();
            this.qualityLabel = new System.Windows.Forms.Label();
            this.compressionRateLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nonZeroBeforeTextLabel = new System.Windows.Forms.Label();
            this.nonZeroBeforeValueLabel = new System.Windows.Forms.Label();
            this.nonZeroAfterTextLabel = new System.Windows.Forms.Label();
            this.nonZeroAfterValueLabel = new System.Windows.Forms.Label();
            this.goWorkButton = new System.Windows.Forms.Button();
            this.greenChannelQualityUpDown = new System.Windows.Forms.NumericUpDown();
            this.redChannelQualityUpDown = new System.Windows.Forms.NumericUpDown();
            this.redChannelLabel = new System.Windows.Forms.Label();
            this.greenChannelLabel = new System.Windows.Forms.Label();
            this.blueChannelLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            this.originalImageFlowLayoutPanel.SuspendLayout();
            this.compressedImageFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compressedImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelQualityUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelQualityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelQualityUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // originalImageBox
            // 
            this.originalImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originalImageBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.originalImageBox.Location = new System.Drawing.Point(3, 3);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(481, 0);
            this.originalImageBox.TabIndex = 0;
            this.originalImageBox.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp";
            this.openFileDialog1.Title = "Image to compress";
            // 
            // chooseImageButton
            // 
            this.chooseImageButton.Location = new System.Drawing.Point(879, 596);
            this.chooseImageButton.Name = "chooseImageButton";
            this.chooseImageButton.Size = new System.Drawing.Size(254, 23);
            this.chooseImageButton.TabIndex = 2;
            this.chooseImageButton.Text = "Choose an image to compress";
            this.chooseImageButton.UseVisualStyleBackColor = true;
            this.chooseImageButton.Click += new System.EventHandler(this.openNewImage_button);
            // 
            // originalImageFlowLayoutPanel
            // 
            this.originalImageFlowLayoutPanel.AutoScroll = true;
            this.originalImageFlowLayoutPanel.Controls.Add(this.originalImageBox);
            this.originalImageFlowLayoutPanel.Location = new System.Drawing.Point(14, 19);
            this.originalImageFlowLayoutPanel.Name = "originalImageFlowLayoutPanel";
            this.originalImageFlowLayoutPanel.Size = new System.Drawing.Size(512, 512);
            this.originalImageFlowLayoutPanel.TabIndex = 3;
            // 
            // compressedImageFlowLayoutPanel
            // 
            this.compressedImageFlowLayoutPanel.AutoScroll = true;
            this.compressedImageFlowLayoutPanel.Controls.Add(this.compressedImageBox);
            this.compressedImageFlowLayoutPanel.Controls.Add(this.compressionInProgressLabel);
            this.compressedImageFlowLayoutPanel.Location = new System.Drawing.Point(726, 34);
            this.compressedImageFlowLayoutPanel.Name = "compressedImageFlowLayoutPanel";
            this.compressedImageFlowLayoutPanel.Size = new System.Drawing.Size(512, 512);
            this.compressedImageFlowLayoutPanel.TabIndex = 4;
            // 
            // compressedImageBox
            // 
            this.compressedImageBox.Location = new System.Drawing.Point(3, 3);
            this.compressedImageBox.Name = "compressedImageBox";
            this.compressedImageBox.Size = new System.Drawing.Size(100, 50);
            this.compressedImageBox.TabIndex = 0;
            this.compressedImageBox.TabStop = false;
            // 
            // compressionInProgressLabel
            // 
            this.compressionInProgressLabel.AutoSize = true;
            this.compressionInProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.compressionInProgressLabel.Location = new System.Drawing.Point(109, 0);
            this.compressionInProgressLabel.Name = "compressionInProgressLabel";
            this.compressionInProgressLabel.Size = new System.Drawing.Size(165, 26);
            this.compressionInProgressLabel.TabIndex = 7;
            this.compressionInProgressLabel.Text = "Compressing ...";
            // 
            // blueChannelQualityUpDown
            // 
            this.blueChannelQualityUpDown.Location = new System.Drawing.Point(632, 237);
            this.blueChannelQualityUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.blueChannelQualityUpDown.Name = "blueChannelQualityUpDown";
            this.blueChannelQualityUpDown.Size = new System.Drawing.Size(72, 20);
            this.blueChannelQualityUpDown.TabIndex = 5;
            this.blueChannelQualityUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.blueChannelQualityUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.qualityUpDown_KeyUp);
            // 
            // qualityLabel
            // 
            this.qualityLabel.AutoSize = true;
            this.qualityLabel.Location = new System.Drawing.Point(616, 160);
            this.qualityLabel.Name = "qualityLabel";
            this.qualityLabel.Size = new System.Drawing.Size(53, 13);
            this.qualityLabel.TabIndex = 6;
            this.qualityLabel.Text = "Quality, %";
            // 
            // compressionRateLabel
            // 
            this.compressionRateLabel.AutoSize = true;
            this.compressionRateLabel.Location = new System.Drawing.Point(589, 317);
            this.compressionRateLabel.Name = "compressionRateLabel";
            this.compressionRateLabel.Size = new System.Drawing.Size(115, 13);
            this.compressionRateLabel.TabIndex = 7;
            this.compressionRateLabel.Text = "Compression rate: XXX";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.originalImageFlowLayoutPanel);
            this.groupBox1.Location = new System.Drawing.Point(38, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 543);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Original image";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(710, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(542, 543);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Compressed image";
            // 
            // nonZeroBeforeTextLabel
            // 
            this.nonZeroBeforeTextLabel.AutoSize = true;
            this.nonZeroBeforeTextLabel.Location = new System.Drawing.Point(592, 343);
            this.nonZeroBeforeTextLabel.Name = "nonZeroBeforeTextLabel";
            this.nonZeroBeforeTextLabel.Size = new System.Drawing.Size(86, 13);
            this.nonZeroBeforeTextLabel.TabIndex = 10;
            this.nonZeroBeforeTextLabel.Text = "Non-zero before:";
            // 
            // nonZeroBeforeValueLabel
            // 
            this.nonZeroBeforeValueLabel.AutoSize = true;
            this.nonZeroBeforeValueLabel.Location = new System.Drawing.Point(595, 367);
            this.nonZeroBeforeValueLabel.Name = "nonZeroBeforeValueLabel";
            this.nonZeroBeforeValueLabel.Size = new System.Drawing.Size(88, 13);
            this.nonZeroBeforeValueLabel.TabIndex = 11;
            this.nonZeroBeforeValueLabel.Text = "#non-zero-before";
            // 
            // nonZeroAfterTextLabel
            // 
            this.nonZeroAfterTextLabel.AutoSize = true;
            this.nonZeroAfterTextLabel.Location = new System.Drawing.Point(592, 397);
            this.nonZeroAfterTextLabel.Name = "nonZeroAfterTextLabel";
            this.nonZeroAfterTextLabel.Size = new System.Drawing.Size(77, 13);
            this.nonZeroAfterTextLabel.TabIndex = 12;
            this.nonZeroAfterTextLabel.Text = "Non-zero after:";
            // 
            // nonZeroAfterValueLabel
            // 
            this.nonZeroAfterValueLabel.AutoSize = true;
            this.nonZeroAfterValueLabel.Location = new System.Drawing.Point(595, 425);
            this.nonZeroAfterValueLabel.Name = "nonZeroAfterValueLabel";
            this.nonZeroAfterValueLabel.Size = new System.Drawing.Size(79, 13);
            this.nonZeroAfterValueLabel.TabIndex = 13;
            this.nonZeroAfterValueLabel.Text = "#non-zero-after";
            // 
            // goWorkButton
            // 
            this.goWorkButton.Location = new System.Drawing.Point(603, 274);
            this.goWorkButton.Name = "goWorkButton";
            this.goWorkButton.Size = new System.Drawing.Size(75, 23);
            this.goWorkButton.TabIndex = 14;
            this.goWorkButton.Text = "Go work";
            this.goWorkButton.UseVisualStyleBackColor = true;
            this.goWorkButton.Click += new System.EventHandler(this.goWorkButton_click);
            // 
            // greenChannelQualityUpDown
            // 
            this.greenChannelQualityUpDown.Location = new System.Drawing.Point(632, 211);
            this.greenChannelQualityUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.greenChannelQualityUpDown.Name = "greenChannelQualityUpDown";
            this.greenChannelQualityUpDown.Size = new System.Drawing.Size(73, 20);
            this.greenChannelQualityUpDown.TabIndex = 15;
            this.greenChannelQualityUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // redChannelQualityUpDown
            // 
            this.redChannelQualityUpDown.Location = new System.Drawing.Point(632, 185);
            this.redChannelQualityUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.redChannelQualityUpDown.Name = "redChannelQualityUpDown";
            this.redChannelQualityUpDown.Size = new System.Drawing.Size(73, 20);
            this.redChannelQualityUpDown.TabIndex = 16;
            this.redChannelQualityUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // redChannelLabel
            // 
            this.redChannelLabel.AutoSize = true;
            this.redChannelLabel.Location = new System.Drawing.Point(586, 187);
            this.redChannelLabel.Name = "redChannelLabel";
            this.redChannelLabel.Size = new System.Drawing.Size(27, 13);
            this.redChannelLabel.TabIndex = 17;
            this.redChannelLabel.Text = "Red";
            // 
            // greenChannelLabel
            // 
            this.greenChannelLabel.AutoSize = true;
            this.greenChannelLabel.Location = new System.Drawing.Point(586, 213);
            this.greenChannelLabel.Name = "greenChannelLabel";
            this.greenChannelLabel.Size = new System.Drawing.Size(36, 13);
            this.greenChannelLabel.TabIndex = 18;
            this.greenChannelLabel.Text = "Green";
            // 
            // blueChannelLabel
            // 
            this.blueChannelLabel.AutoSize = true;
            this.blueChannelLabel.Location = new System.Drawing.Point(586, 239);
            this.blueChannelLabel.Name = "blueChannelLabel";
            this.blueChannelLabel.Size = new System.Drawing.Size(28, 13);
            this.blueChannelLabel.TabIndex = 19;
            this.blueChannelLabel.Text = "Blue";
            // 
            // ImageCompressionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.blueChannelLabel);
            this.Controls.Add(this.greenChannelLabel);
            this.Controls.Add(this.redChannelLabel);
            this.Controls.Add(this.redChannelQualityUpDown);
            this.Controls.Add(this.greenChannelQualityUpDown);
            this.Controls.Add(this.goWorkButton);
            this.Controls.Add(this.nonZeroAfterValueLabel);
            this.Controls.Add(this.nonZeroAfterTextLabel);
            this.Controls.Add(this.nonZeroBeforeValueLabel);
            this.Controls.Add(this.nonZeroBeforeTextLabel);
            this.Controls.Add(this.compressedImageFlowLayoutPanel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.compressionRateLabel);
            this.Controls.Add(this.qualityLabel);
            this.Controls.Add(this.blueChannelQualityUpDown);
            this.Controls.Add(this.chooseImageButton);
            this.Name = "ImageCompressionForm";
            this.Text = "Image compression";
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            this.originalImageFlowLayoutPanel.ResumeLayout(false);
            this.compressedImageFlowLayoutPanel.ResumeLayout(false);
            this.compressedImageFlowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compressedImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelQualityUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelQualityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelQualityUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button chooseImageButton;
        private System.Windows.Forms.FlowLayoutPanel originalImageFlowLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel compressedImageFlowLayoutPanel;
        private System.Windows.Forms.NumericUpDown blueChannelQualityUpDown;
        private System.Windows.Forms.Label qualityLabel;
        private System.Windows.Forms.PictureBox compressedImageBox;
        private System.Windows.Forms.Label compressionInProgressLabel;
        private System.Windows.Forms.Label compressionRateLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label nonZeroBeforeTextLabel;
        private System.Windows.Forms.Label nonZeroBeforeValueLabel;
        private System.Windows.Forms.Label nonZeroAfterTextLabel;
        private System.Windows.Forms.Label nonZeroAfterValueLabel;
        private System.Windows.Forms.Button goWorkButton;
        private System.Windows.Forms.NumericUpDown greenChannelQualityUpDown;
        private System.Windows.Forms.NumericUpDown redChannelQualityUpDown;
        private System.Windows.Forms.Label redChannelLabel;
        private System.Windows.Forms.Label greenChannelLabel;
        private System.Windows.Forms.Label blueChannelLabel;
    }
}

