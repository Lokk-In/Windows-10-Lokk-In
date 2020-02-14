using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows10LokkIn.Classes
{
    internal static class ImageExtensions
    {
        // Resize any image to a certain size
        public static Image ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Changes the brightness of an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="value"></param>
        public static void AdjustBrightness(this Image image, int value)
        {
            float FinalValue = value / 255.0f;
            ColorMatrix tempMatrix = new ColorMatrix(new float[][]{
                       new float[] {1, 0, 0, 0, 0},
                       new float[] {0, 1, 0, 0, 0},
                       new float[] {0, 0, 1, 0, 0},
                       new float[] {0, 0, 0, 1, 0},
                       new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                   });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(tempMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(
                    image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes
                    );
            }

        }

        public static bool IsPixelBright(this Image image, int x, int y)
        {
            return ((Bitmap)image).GetPixel(x, y).GetBrightness() > 0.8f;
        }
    }
}
