using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameShopProg 
{
    public class Captcha : INotifyPropertyChanged
    {
        public Captcha()
        {
            Regenerate();
        }

        public string CaptchaText { get; private set; }
        public BitmapImage CaptchaImg { get => GetCaptchaImg(); }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void Regenerate()
        {
            var rnd = new Random();
            var strB = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                strB.Append((char)rnd.Next(65, 122));
            }

            CaptchaText = strB.ToString();
            OnPropertyChanged("CaptchaText");
            OnPropertyChanged("CaptchaImg");
        }

        public BitmapImage GetCaptchaImg()
        {
            var font = new Font("Arial", 15);
            var img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            SizeF textSize = drawing.MeasureString(CaptchaText, font);

            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            drawing.Clear(Color.White);

            Brush textBrush = new SolidBrush(Color.Black);

            drawing.DrawString(CaptchaText, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            var result = new Bitmap(img.Width, img.Height);
            for (int y = 0; y < result.Height; y++)
                for (int x = 0; x < result.Width; x++)
                {
                    var point = Twirl(img, new Point(x, y));
                    result.SetPixel(x, y, img.GetPixel(point.X, point.Y));
                }

            return BitmapToImageSource(result);
        }

        Point Twirl(Bitmap img, Point p)
        {
            var tmp =
                new Point(p.X - img.Width / 2, p.Y - img.Height / 2);
            var r = Math.Sqrt(tmp.X * tmp.X + tmp.Y * tmp.Y);
            var maxr = img.Width / 2;
            if (r > maxr) return p;
            var a = Math.Atan2(tmp.Y, tmp.X);
            a += 1 - r / maxr;
            var dx = Math.Cos(a) * r;
            var dy = Math.Sin(a) * r;
            var result = new Point((int)dx + img.Width / 2, (int)dy + img.Height / 2);
            if (result.X >= img.Width || result.X < 0 ||
                result.Y >= img.Height || result.Y < 0) return p;
            return result;
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
