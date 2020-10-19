using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace portrait_mode
{
    public partial class Form1 : Form
    {
        #region Private data
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        DeepPersonLab deepPersonLab;
        PortraitModeFilter portraitModeFilter;
        Bitmap image;
        Bitmap mask;
        Bitmap portrait;
        #endregion

        #region Form voids
        public Form1()
        {
            InitializeComponent();
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            trackBar1.MouseUp += TrackBar1_MouseUp;
            button1.Click += Button1_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "BMP|*.bmp|" +
                "JPEG|*.jpg; *.jpeg|" +
                "PNG|*.png|" +
                "GIF|*.gif|" +
                "TIFF|*.tiff";

            deepPersonLab = new DeepPersonLab(@"..\..\..\deeplabv3_mnv2_pascal_train_aug.onnx");
            portraitModeFilter = new PortraitModeFilter(0.0);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var file = ((string[])e.Data.GetData(DataFormats.FileDrop, true))[0];
            image = new Bitmap(file, false);
            mask = deepPersonLab.Fit(image);
            pictureBox1.Image = image;
            pictureBox2.Image = null;
            TrackBar1_MouseUp(sender, null);
            Cursor = Cursors.Default;
        }

        private void TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (image is object && mask is object)
            {
                var strength = trackBar1.Value / 100.0;
                portraitModeFilter.Strength = strength;
                label1.Text = $"Strenght: {strength}";

                portrait = portraitModeFilter.Apply(image, mask);
                pictureBox2.Image = portrait;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (portrait is object && saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    portrait.Save(stream, GetImageFormat(saveFileDialog.FilterIndex));
                }
            }
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Returns image format.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Image format</returns>
        private static ImageFormat GetImageFormat(int index)
        {
            switch (index)
            {
                case 1:
                    return ImageFormat.Bmp;
                case 2:
                    return ImageFormat.Jpeg;
                case 3:
                    return ImageFormat.Png;
                case 4:
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Tiff;
            }
        }
        #endregion
    }
}