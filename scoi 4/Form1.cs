using System.Drawing.Imaging;
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

        public void btnApplyLinear_Click(object sender, EventArgs e)
        {
            if (origImage == null)
                return;

            int size = (int)nudKernelSize.Value;
            float[,] kernel = ParseKernel(txtKernel.Text, size, size);
            var processor = new ImageProcessor(origImage);

            curImage = processor.ToBitmap();
            pictureBox1.Image = curImage;
            int width = processor.Width;
            int height = processor.Height;
            int kernelW = kernel.GetLength(1);
            int kernelH = kernel.GetLength(0);
            int radiusX = kernelW / 2;
            int radiusY = kernelH / 2;

            int bytesPerPixel = processor.BytesPerPixel;
            int stride = processor.Stride;
            byte[] input = processor.Pixels;
            byte[] output = new byte[input.Length];

            unsafe
            {
                fixed (byte* srcStart = input)
                fixed (byte* dstStart = output)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            float[] sum = new float[3];

                            for (int ky = -radiusY; ky <= radiusY; ky++)
                            {
                                int iy = Math.Clamp(y + ky, 0, height - 1);
                                for (int kx = -radiusX; kx <= radiusX; kx++)
                                {
                                    int ix = Math.Clamp(x + kx, 0, width - 1);
                                    float weight = kernel[ky + radiusY, kx + radiusX];

                                    byte* srcPixel = srcStart + iy * stride + ix * bytesPerPixel;
                                    for (int c = 0; c < 3; c++)
                                        sum[c] += weight * srcPixel[c];
                                }
                            }

                            byte* dstPixel = dstStart + y * stride + x * bytesPerPixel;
                            for (int c = 0; c < 3; c++)
                                dstPixel[c] = (byte)Math.Clamp((int)sum[c], 0, 255);
                        }
                    }
                }
            }

            processor.Pixels = output;
            curImage = processor.ToBitmap();
            pictureBox1.Image = curImage;
        }

        private void btnApplyMedian_Click(object sender, EventArgs e)
        {
            if (origImage == null) return;

            int size = (int)nudKernelSize.Value;
            if (size % 2 == 0) size++; // нечётный

            int radius = size / 2;
            var processor = new ImageProcessor(origImage);
            byte[] input = processor.Pixels;
            byte[] output = new byte[input.Length];

            int width = processor.Width;
            int height = processor.Height;
            int stride = processor.Stride;
            int bpp = processor.BytesPerPixel;

            unsafe
            {
                fixed (byte* srcStart = input)
                fixed (byte* dstStart = output)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            for (int c = 0; c < 3; c++)
                            {
                                int[] window = new int[size * size];
                                int index = 0;

                                for (int ky = -radius; ky <= radius; ky++)
                                {
                                    int py = Reflect(y + ky, height);
                                    for (int kx = -radius; kx <= radius; kx++)
                                    {
                                        int px = Reflect(x + kx, width);
                                        byte* p = srcStart + py * stride + px * bpp;
                                        window[index++] = p[c];
                                    }
                                }

                                int median = QuickSelect(window, window.Length / 2);
                                byte* dstPixel = dstStart + y * stride + x * bpp;
                                dstPixel[c] = (byte)median;
                            }
                        }
                    }
                }
            }

            processor.Pixels = output;
            curImage = processor.ToBitmap();
            pictureBox1.Image = curImage;
        }



        private void btnApplyGaussian_Click(object sender, EventArgs e)
        {
            if (origImage == null) return;

            int size = (int)nudKernelSize.Value;
            if (size % 2 == 0) size++; // нечётный
            float sigma = (float)nudSigma.Value;

            float[,] kernel = GenerateGaussianKernel(size, sigma);
            var processor = new ImageProcessor(origImage);
            byte[] input = processor.Pixels;
            byte[] output = new byte[input.Length];

            int width = processor.Width;
            int height = processor.Height;
            int stride = processor.Stride;
            int bpp = processor.BytesPerPixel;
            int radius = size / 2;

            unsafe
            {
                fixed (byte* srcStart = input)
                fixed (byte* dstStart = output)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            float[] sum = new float[3];

                            for (int ky = -radius; ky <= radius; ky++)
                            {
                                int py = Reflect(y + ky, height);
                                for (int kx = -radius; kx <= radius; kx++)
                                {
                                    int px = Reflect(x + kx, width);
                                    float weight = kernel[ky + radius, kx + radius];

                                    byte* srcPixel = srcStart + py * stride + px * bpp;
                                    for (int c = 0; c < 3; c++)
                                        sum[c] += weight * srcPixel[c];
                                }
                            }

                            byte* dstPixel = dstStart + y * stride + x * bpp;
                            for (int c = 0; c < 3; c++)
                                dstPixel[c] = (byte)Math.Clamp((int)sum[c], 0, 255);
                        }
                    }
                }
            }

            processor.Pixels = output;
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

