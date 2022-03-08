//
// Mark König, 03/2022
//

using System.Drawing;
using System.Windows.Forms;

namespace LoadBingPicture
{
    public class ModPicture
    {
        public static Image ChangePicture(Image original, int resolution, LoadJson.BingPicture picture)
        {
            Image image1 = new Bitmap(original);
            Image image2 = new Bitmap(Properties.Resources.back75);

            Rectangle ScreenResolution = Screen.PrimaryScreen.Bounds;

            int sWidth = ScreenResolution.Width;
            int pWidth = original.Width;

            float factor = (float)pWidth / (float)sWidth;
            int sFont = 8;
            bool run = true;

            int sz = image1.Height;
            int back = sz / 10;

            float l = 0.0f;
            SizeF l1 = new SizeF(0,0);
            SizeF l2 = new SizeF(0, 0);
            Font stringFont = new Font("Arial", sFont);

            using (Graphics gr = Graphics.FromImage(image1))
            {
                while (run)
                {
                    stringFont = new Font("Arial", sFont);
                    l1 = gr.MeasureString(picture.title, stringFont);
                    l2 = gr.MeasureString(picture.description, stringFont);

                    l = l1.Width;
                    if (l2.Width > l) l = l2.Width;

                    if ((int)l > (sWidth / 2) * factor)
                        run = false;
                    else
                        sFont = sFont + 1;
                }

                int yValue = (int)l1.Height / 2;
                int x = original.Width - (int)l - 50;

                gr.DrawImage(image2,
                    new Rectangle(new Point(x, back * 8), new Size((int)l + 20, (3 * yValue) + (2 * (int)l1.Height))));

                gr.DrawString(picture.title,
                    stringFont,
                    new SolidBrush(Color.White),
                    new PointF(x + 10, back * 8 + yValue));

                gr.DrawString(picture.description,
                    stringFont,
                    new SolidBrush(Color.White),
                    new PointF(x + 10, back * 8 + yValue + (int)l1.Height + yValue));
            }

            image2.Dispose();

            return image1;
        }

    }
}
