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
    /// <summary>
    /// Contains functions to ease working with images
    /// </summary>
    internal static class ImageExtensions
    {

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
