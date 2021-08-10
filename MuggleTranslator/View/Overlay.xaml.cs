using MuggleTranslator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SD = System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MuggleTranslator.View
{
    /// <summary>
    /// Overlay.xaml 的交互逻辑
    /// </summary>
    public partial class Overlay : Window
    {
        private System.Drawing.Image _backImage;
        public System.Drawing.Image CropImage;
        public Overlay(System.Drawing.Image image)
        {
            InitializeComponent();
            ImageBrush bg = new ImageBrush();
            _backImage = image;
            bg.ImageSource = ImageProcess.ImageToBitmapImage(image);
            Background = bg;
        }

        private SD.Point _downPoint = new SD.Point(-1, -1);
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _downPoint = AutoIt.AutoItX.MouseGetPos();

            var point = DpiConverter.TransformPoint(_downPoint);
            rectangle.SetValue(Canvas.LeftProperty, (double)Math.Max(point.X, 0));
            rectangle.SetValue(Canvas.TopProperty, (double)Math.Max(point.Y, 0));
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_downPoint.X == -1 && _downPoint.Y == -1)
                return;

            var upPoint = AutoIt.AutoItX.MouseGetPos();

            var minx = Math.Min(upPoint.X, _downPoint.X);
            var miny = Math.Min(upPoint.Y, _downPoint.Y);

            CropImage = ImageProcess.Crop(_backImage,
                Math.Max(minx, 1),
                Math.Max(miny, 1),
                Math.Max(Math.Abs(upPoint.X - _downPoint.X), 1),
                Math.Max(Math.Abs(upPoint.Y - _downPoint.Y), 1)
                );
            Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_downPoint.X == -1 && _downPoint.Y == -1)
                return;

            var pos = AutoIt.AutoItX.MouseGetPos();
            var delta = new SD.Point(pos.X - _downPoint.X, pos.Y - _downPoint.Y);
            var point = DpiConverter.TransformPoint(delta);

            rectangle.Width = Math.Max(point.X, 0);
            rectangle.Height = Math.Max(point.Y, 0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rectangle.SetValue(Canvas.LeftProperty, 0.0);
            rectangle.SetValue(Canvas.TopProperty, 0.0);
            rectangle.Width = Width;
            rectangle.Height = Height;

            this.Activate();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (_downPoint.X != -1 || _downPoint.Y != -1)
                    return;

                this.Close();
            }
        }
    }
}
