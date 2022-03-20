//
// Mark König, 03/2022
//

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace LoadBingPicture
{
    public class ModPicture
    {
        public static Image ChangePicture(Image original, int resolution, LoadJson.BingPicture picture)
        {
            // load original image
            Image image = new Bitmap(original);
            // rezize to the new size
            Rectangle ScreenResolution = Screen.PrimaryScreen.Bounds;
            Image image1 = FixedSize(image, ScreenResolution.Width, ScreenResolution.Height);
            // dispose the original
            image.Dispose();
            // load the back gfx
            Image image2 = new Bitmap(Properties.Resources.back75);

            // we want to cover 1/10 of the back
            int back = image1.Height / 10;

            using (Graphics gr = Graphics.FromImage(image1))
            {
                // we have 2 line and 3 spaces 
                Font stringFont = new Font("Arial", back / 5);
                // measure the size
                SizeF l1 = gr.MeasureString(picture.title, stringFont);
                SizeF l2 = gr.MeasureString(picture.description, stringFont);

                float l = l1.Width;
                if (l2.Width > l) l = l2.Width;

                int yValue = (int)l1.Height / 2;
                int x = image1.Width - (int)l - 50;

                gr.DrawImage(image2,
                    new Rectangle(new Point(x, back * 8),
                                  new Size((int)l + 20, (3 * yValue) + (2 * (int)l1.Height))));

                gr.DrawString(picture.title,
                    stringFont,
                    new SolidBrush(Color.White),
                    new PointF(x + 10, back * 8 + yValue));

                gr.DrawString(picture.description,
                    stringFont,
                    new SolidBrush(Color.White),
                    new PointF(x + 10, back * 8 + yValue + (int)l1.Height + yValue));
            }

            // dispose the back gfx
            image2.Dispose();

            return image1;
        }
        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

    }
}
