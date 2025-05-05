namespace scoi_4
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            btnLoadImage = new Button();
            btnApplyLinear = new Button();
            btnApplyMedian = new Button();
            btnGenerateGaussian = new Button();
            nudKernelSize = new NumericUpDown();
            nudSigma = new NumericUpDown();
            txtKernel = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btnReset = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudKernelSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSigma).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(545, 419);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new Point(617, 142);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(75, 23);
            btnLoadImage.TabIndex = 1;
            btnLoadImage.Text = "Загрузить изображение";
            btnLoadImage.UseVisualStyleBackColor = true;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // btnApplyLinear
            // 
            btnApplyLinear.Location = new Point(617, 189);
            btnApplyLinear.Name = "btnApplyLinear";
            btnApplyLinear.Size = new Size(75, 23);
            btnApplyLinear.TabIndex = 2;
            btnApplyLinear.Text = "Линейная фильтрация";
            btnApplyLinear.UseVisualStyleBackColor = true;
            btnApplyLinear.Click += btnApplyLinear_Click;
            // 
            // btnApplyMedian
            // 
            btnApplyMedian.Location = new Point(617, 233);
            btnApplyMedian.Name = "btnApplyMedian";
            btnApplyMedian.Size = new Size(75, 23);
            btnApplyMedian.TabIndex = 3;
            btnApplyMedian.Text = "Медианная фильтрация";
            btnApplyMedian.UseVisualStyleBackColor = true;
            btnApplyMedian.Click += btnApplyMedian_Click;
            // 
            // btnGenerateGaussian
            // 
            btnGenerateGaussian.Location = new Point(617, 281);
            btnGenerateGaussian.Name = "btnGenerateGaussian";
            btnGenerateGaussian.Size = new Size(75, 23);
            btnGenerateGaussian.TabIndex = 4;
            btnGenerateGaussian.Text = "Гауссово ядро";
            btnGenerateGaussian.UseVisualStyleBackColor = true;
            btnGenerateGaussian.Click += btnApplyGaussian_Click;
            // 
            // nudKernelSize
            // 
            nudKernelSize.Increment = new decimal(new int[] { 2, 0, 0, 0 });
            nudKernelSize.Location = new Point(608, 39);
            nudKernelSize.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            nudKernelSize.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            nudKernelSize.Name = "nudKernelSize";
            nudKernelSize.Size = new Size(100, 23);
            nudKernelSize.TabIndex = 6;
            nudKernelSize.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // nudSigma
            // 
            nudSigma.DecimalPlaces = 1;
            nudSigma.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudSigma.Location = new Point(608, 91);
            nudSigma.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudSigma.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            nudSigma.Name = "nudSigma";
            nudSigma.Size = new Size(100, 23);
            nudSigma.TabIndex = 7;
            nudSigma.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtKernel
            // 
            txtKernel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtKernel.Location = new Point(742, 23);
            txtKernel.Multiline = true;
            txtKernel.Name = "txtKernel";
            txtKernel.Size = new Size(747, 246);
            txtKernel.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(608, 12);
            label1.Name = "label1";
            label1.Size = new Size(100, 15);
            label1.TabIndex = 9;
            label1.Text = "Размер матрицы";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(608, 73);
            label2.Name = "label2";
            label2.Size = new Size(72, 15);
            label2.TabIndex = 10;
            label2.Text = "Параметр σ";
            // 
            // btnReset
            // 
            btnReset.Location = new Point(617, 345);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 11;
            btnReset.Text = "Сброс";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1662, 697);
            Controls.Add(btnReset);
            Controls.Add(txtKernel);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(nudSigma);
            Controls.Add(nudKernelSize);
            Controls.Add(btnGenerateGaussian);
            Controls.Add(btnApplyMedian);
            Controls.Add(btnApplyLinear);
            Controls.Add(btnLoadImage);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudKernelSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSigma).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnLoadImage;
        private Button btnApplyLinear;
        private Button btnApplyMedian;
        private Button btnGenerateGaussian;
        private Button button5;
        private NumericUpDown nudSigma;
        private NumericUpDown nudKernelSize;
        private TextBox txtKernel;
        private Label label1;
        private Label label2;
        private Button btnReset;
    }
}
