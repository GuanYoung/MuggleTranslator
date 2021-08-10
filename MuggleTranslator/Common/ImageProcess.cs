using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MuggleTranslator.Common
{
    public class ImageProcess
    {
        public static string ImageToBase64String(Image image)
        {
            var bytes = ImageToBytes(image);
            return Convert.ToBase64String(bytes);
        }

        public static byte[] ImageToBytes(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static Image BytesToImage(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Image image = Image.FromStream(ms);
            return image;
        }

        public static BitmapImage ImageToBitmapImage(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static Image Crop(Image image, int x, int y, int width, int height)
        {
            var targetImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(targetImage))
            {
                g.DrawImage(image,
                            new Rectangle(0, 0, width, height),
                            new Rectangle(x, y, width, height),
                            GraphicsUnit.Pixel
                            );
            }

            return targetImage;
        }
    }
}
