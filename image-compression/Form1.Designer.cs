namespace image_compression
{
    partial class imageCompressionForm
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
            this.compressionRateUpDown = new System.Windows.Forms.NumericUpDown();
            this.compressionRateLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            this.originalImageFlowLayoutPanel.SuspendLayout();
            this.compressedImageFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compressedImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionRateUpDown)).BeginInit();
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
            this.openFileDialog1.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|All file" +
    "s (*.*)|*.* ";
            this.openFileDialog1.Title = "Image to compress";
            // 
            // chooseImageButton
            // 
            this.chooseImageButton.Location = new System.Drawing.Point(843, 550);
            this.chooseImageButton.Name = "chooseImageButton";
            this.chooseImageButton.Size = new System.Drawing.Size(254, 23);
            this.chooseImageButton.TabIndex = 2;
            this.chooseImageButton.Text = "Choose an image to compress";
            this.chooseImageButton.UseVisualStyleBackColor = true;
            this.chooseImageButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // originalImageFlowLayoutPanel
            // 
            this.originalImageFlowLayoutPanel.AutoScroll = true;
            this.originalImageFlowLayoutPanel.Controls.Add(this.originalImageBox);
            this.originalImageFlowLayoutPanel.Location = new System.Drawing.Point(38, 12);
            this.originalImageFlowLayoutPanel.Name = "originalImageFlowLayoutPanel";
            this.originalImageFlowLayoutPanel.Size = new System.Drawing.Size(512, 512);
            this.originalImageFlowLayoutPanel.TabIndex = 3;
            // 
            // compressedImageFlowLayoutPanel
            // 
            this.compressedImageFlowLayoutPanel.AutoScroll = true;
            this.compressedImageFlowLayoutPanel.Controls.Add(this.compressedImageBox);
            this.compressedImageFlowLayoutPanel.Controls.Add(this.compressionInProgressLabel);
            this.compressedImageFlowLayoutPanel.Location = new System.Drawing.Point(708, 12);
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
            // compressionRateUpDown
            // 
            this.compressionRateUpDown.Location = new System.Drawing.Point(570, 265);
            this.compressionRateUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.compressionRateUpDown.Name = "compressionRateUpDown";
            this.compressionRateUpDown.Size = new System.Drawing.Size(120, 20);
            this.compressionRateUpDown.TabIndex = 5;
            this.compressionRateUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.compressionRateUpDown.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.compressionRateUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.compressionRateUpDown_KeyUp);
            // 
            // compressionRateLabel
            // 
            this.compressionRateLabel.AutoSize = true;
            this.compressionRateLabel.Location = new System.Drawing.Point(584, 249);
            this.compressionRateLabel.Name = "compressionRateLabel";
            this.compressionRateLabel.Size = new System.Drawing.Size(88, 13);
            this.compressionRateLabel.TabIndex = 6;
            this.compressionRateLabel.Text = "Compression rate";
            // 
            // imageCompressionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.compressionRateLabel);
            this.Controls.Add(this.compressionRateUpDown);
            this.Controls.Add(this.compressedImageFlowLayoutPanel);
            this.Controls.Add(this.originalImageFlowLayoutPanel);
            this.Controls.Add(this.chooseImageButton);
            this.Name = "imageCompressionForm";
            this.Text = "Image compression";
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            this.originalImageFlowLayoutPanel.ResumeLayout(false);
            this.compressedImageFlowLayoutPanel.ResumeLayout(false);
            this.compressedImageFlowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compressedImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compressionRateUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button chooseImageButton;
        private System.Windows.Forms.FlowLayoutPanel originalImageFlowLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel compressedImageFlowLayoutPanel;
        private System.Windows.Forms.NumericUpDown compressionRateUpDown;
        private System.Windows.Forms.Label compressionRateLabel;
        private System.Windows.Forms.PictureBox compressedImageBox;
        private System.Windows.Forms.Label compressionInProgressLabel;
    }
}

