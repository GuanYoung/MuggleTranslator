using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MuggleTranslator.Common
{
    public static class DpiConverter
    {
        public static Point TransformPoint(Point pt)
        {
            var scale = GetScalingFactor();
            return new Point((int)(pt.X / scale), (int)(pt.Y / scale));
        }

        public static Point TransformBackPoint(Point pt)
        {
            var scale = GetScalingFactor();
            return new Point((int)(pt.X * scale), (int)(pt.Y * scale));
        }

        public static Rectangle TransformRectangle(Rectangle rect)
        {
            var scale = GetScalingFactor();
            return new Rectangle((int)(rect.Left / scale), (int)(rect.Top / scale), (int)(rect.Width / scale), (int)(rect.Height / scale));
        }

        public static Rectangle TransformBackRectangle(Rectangle rect)
        {
            var scale = GetScalingFactor();
            return new Rectangle((int)(rect.Left * scale), (int)(rect.Top * scale), (int)(rect.Width * scale), (int)(rect.Height * scale));
        }

        #region Win32
        // FROM https://stackoverflow.com/a/21450169
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private const int VERTRES = 10;
        private const int DESKTOPVERTRES = 117;
        private const int LOGPIXELSX = 88;    // Used for GetDeviceCaps().
        private const int LOGPIXELSY = 90;    // Used for GetDeviceCaps().

        private static double _scalingFactor;

        public static double GetScalingFactor()
        {
            if (_scalingFactor == 0)
            {
                using (var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr desktop = graphics.GetHdc();
                    int logicalScreenHeight = GetDeviceCaps(desktop, VERTRES);
                    int physicalScreenHeight = GetDeviceCaps(desktop, DESKTOPVERTRES);
                    int logpixelsx = GetDeviceCaps(desktop, LOGPIXELSX);
                    graphics.ReleaseHdc(desktop);

                    _scalingFactor = Math.Round(logpixelsx / 96d, 2);
                    if (_scalingFactor == 1)
                        _scalingFactor = Math.Round((double)physicalScreenHeight / logicalScreenHeight, 2);
                }
            }
            return _scalingFactor;
        }
        #endregion
    }
}
