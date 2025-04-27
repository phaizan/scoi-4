using System.Text;

namespace scoi_4
{
    public partial class Form1 : Form
    {
        private Bitmap origImage;
        private Bitmap curImage;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Выберите изображение";
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    origImage = new Bitmap(ofd.FileName);
                    curImage = (Bitmap)origImage.Clone();
                    pictureBox1.Image = origImage;
                }
            }
        }

        private void btnApplyLinear_Click(object sender, EventArgs e)
        {
            if (origImage == null)
                return;
            int size = (int)nudKernelSize.Value;
            float[,] kernel = ParseKernel(txtKernel.Text, size, size);
            var processor = new ImageProcessor(origImage);
            var outputBytes = new byte[processor.Pixels.Length];
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            for (int y = 0; y < processor.Height; y++)
            {
                for (int x = 0; x < processor.Width; x++)
                {
                    float[] rgb = new float[3];

                    for (int ky = -radiusY; ky <= radiusY; ky++)
                    {
                        for (int kx = -radiusX; kx <= radiusX; kx++)
                        {
                            float weight = kernel[ky + radiusY, kx + radiusX];
                            for (int c = 0; c < 3; c++)
                            {
                                rgb[c] += weight * processor.GetPixelSafe(x + kx, y + ky, c);
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        int index = y * processor.Stride + x * processor.BytesPerPixel + c;
                        outputBytes[index] = (byte)Math.Min(255, Math.Max(0, rgb[c]));
                    }
                }
            }
            processor.Pixels = outputBytes;
            curImage = processor.ToBitmap();
            pictureBox1.Image = curImage;
        }

        private void btnApplyMedian_Click(object sender, EventArgs e)
        {
            if (origImage == null)
                return;

            int size = (int)nudKernelSize.Value;
            if (size % 2 == 0) size++; // делаем нечётным, если вдруг пользователь выбрал чётное

            var processor = new ImageProcessor(origImage);
            var outputBytes = new byte[processor.Pixels.Length];
            int radius = size / 2;

            for (int y = 0; y < processor.Height; y++)
            {
                for (int x = 0; x < processor.Width; x++)
                {
                    for (int c = 0; c < 3; c++) // R, G, B
                    {
                        int[] window = new int[size * size];
                        int index = 0;

                        for (int ky = -radius; ky <= radius; ky++)
                        {
                            for (int kx = -radius; kx <= radius; kx++)
                            {
                                int px = Reflect(x + kx, processor.Width);
                                int py = Reflect(y + ky, processor.Height);
                                window[index++] = processor.GetPixelSafe(px, py, c);
                            }
                        }

                        int median = QuickSelect(window, window.Length / 2);
                        int outIndex = y * processor.Stride + x * processor.BytesPerPixel + c;
                        outputBytes[outIndex] = (byte)median;
                    }
                }
            }

            processor.Pixels = outputBytes;
            curImage = processor.ToBitmap();
            pictureBox1.Image = curImage;
        }

        private void btnApplyGaussian_Click(object sender, EventArgs e)
        {
            if (origImage == null)
                return;

            int size = (int)nudKernelSize.Value;
            if (size % 2 == 0) size++; // гарантируем нечётность
            float sigma = (float)nudSigma.Value;

            float[,] kernel = GenerateGaussianKernel(size, sigma);


            var processor = new ImageProcessor(origImage);
            var outputBytes = new byte[processor.Pixels.Length];
            int radius = size / 2;

            for (int y = 0; y < processor.Height; y++)
            {
                for (int x = 0; x < processor.Width; x++)
                {
                    float[] rgb = new float[3];

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            float weight = kernel[ky + radius, kx + radius];
                            int px = Reflect(x + kx, processor.Width);
                            int py = Reflect(y + ky, processor.Height);
                            for (int c = 0; c < 3; c++)
                            {
                                rgb[c] += weight * processor.GetPixelSafe(px, py, c);
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        int index = y * processor.Stride + x * processor.BytesPerPixel + c;
                        outputBytes[index] = (byte)Math.Min(255, Math.Max(0, rgb[c]));
                    }
                }
            }

            processor.Pixels = outputBytes;
            curImage = processor.ToBitmap();
            pictureBox1.Image = curImage;

        }






        private float[,] ParseKernel(string text, int rows, int cols)
        {
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            float[,] kernel = new float[rows, cols];
            for (int y = 0; y < rows; y++)
            {
                var values = lines[y].Trim().Split(new[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < cols; x++)
                {
                    if (float.TryParse(values[x].Replace(',', '.'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float f))
                        kernel[y, x] = f;
                }
            }
            return kernel;
        }

        private int Reflect(int x, int max)
        {
            if (x < 0) return -x;
            if (x >= max) return 2 * max - x - 2;
            return x;
        }

        private int QuickSelect(int[] array, int k)
        {
            return QuickSelect(array, 0, array.Length - 1, k);
        }

        private int QuickSelect(int[] array, int left, int right, int k)
        {
            if (left == right) return array[left];

            int pivotIndex = Partition(array, left, right);
            if (k == pivotIndex)
                return array[k];
            else if (k < pivotIndex)
                return QuickSelect(array, left, pivotIndex - 1, k);
            else
                return QuickSelect(array, pivotIndex + 1, right, k);
        }

        private int Partition(int[] array, int left, int right)
        {
            int pivot = array[right];
            int i = left;
            for (int j = left; j < right; j++)
            {
                if (array[j] <= pivot)
                {
                    (array[i], array[j]) = (array[j], array[i]);
                    i++;
                }
            }
            (array[i], array[right]) = (array[right], array[i]);
            return i;
        }

        private float[,] GenerateGaussianKernel(int size, float sigma)
        {
            float[,] kernel = new float[size, size];
            int r = size / 2;
            float sum = 0;
            float sigma2 = 2 * sigma * sigma;
            float piSigma = (float)(Math.PI * sigma2);

            for (int y = -r; y <= r; y++)
            {
                for (int x = -r; x <= r; x++)
                {
                    float value = (float)(Math.Exp(-(x * x + y * y) / sigma2) / piSigma);
                    kernel[y + r, x + r] = value;
                    sum += value;
                }
            }
            // Нормализация
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    kernel[y, x] /= sum;
            DisplayKernel(kernel, sum);
            return kernel;
        }
        private void DisplayKernel(float[,] kernel, float sum)
        {
            StringBuilder sb = new StringBuilder();
            int rows = kernel.GetLength(0);
            int cols = kernel.GetLength(1);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    sb.Append(kernel[y, x].ToString("0.0000").PadRight(8));
                }
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.Append("Сумма: " + sum.ToString("0.0000"));
            txtKernel.Text = sb.ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = origImage;
        }
    }
}

