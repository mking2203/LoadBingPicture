using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

namespace LoadBingPicture
{
    public class ModPicture
    {
        public static Image ChangePicture(Image original, int resolution, LoadJson.BingPicture picture)
        {
            Image image1 = new Bitmap(original);
            Image image2 = new Bitmap(Properties.Resources.back75);

            int sz = image1.Height;
            int back = sz / 10;

            Font stringFont = new Font("Arial", 14);
            switch (resolution)
            {
                case 0:
                    stringFont = new Font("Arial", 10);
                    break;
                default:
                case 1:
                    //
                    break;
                case 2:
                    stringFont = new Font("Arial", 34);
                    break;
            }

            using (Graphics gr = Graphics.FromImage(image1))
            {
                SizeF l1 = gr.MeasureString(picture.title, stringFont);
                SizeF l2 = gr.MeasureString(picture.copyright, stringFont);

                int yValue = (int)l1.Height / 2;

                float l = l1.Width;
                if (l2.Width > l) l = l2.Width;

                int x = original.Width - (int)l - 50;

                gr.DrawImage(image2,
                    new Rectangle(new Point(x, back * 8), new Size((int)l + 20, (3 * yValue) + (2 * (int)l1.Height))));

                gr.DrawString(picture.title,
                    stringFont,
                    new SolidBrush(Color.White),
                    new PointF(x + 10, back * 8 + yValue));

                gr.DrawString(picture.copyright,
                    stringFont,
                    new SolidBrush(Color.White),
                    new PointF(x + 10, back * 8 + yValue + (int)l1.Height + yValue));
            }

            image2.Dispose();

            return image1;
        }

    }
}
