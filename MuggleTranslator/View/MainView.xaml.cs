using MuggleTranslator.Common;
using MuggleTranslator.Runtime;
using MuggleTranslator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuggleTranslator.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel _viewmodel;
        public MainView()
        {
            InitializeComponent();

            _viewmodel = new MainViewModel();
            DataContext = _viewmodel;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            Visibility = Visibility.Hidden;

            KeyboardMonitor.KeyDown += KeyboardMonitor_KeyDown;
            KeyboardMonitor.KeyUp += KeyboardMonitor_KeyUp;
            MouseMonitor.LeftButtonDown += MouseMonitor_LeftButtonDown;
        }

        private void MouseMonitor_LeftButtonDown(MouseHookEventArgs args)
        {
            if (!_viewmodel.Locked)
            {
                var clickPoint = PointFromScreen(new Point(args.MouseStruct.pt.x, args.MouseStruct.pt.y));

                if (!(clickPoint.X > 0 && clickPoint.X < Width && clickPoint.Y > 0 && clickPoint.Y < Height))
                    Visibility = Visibility.Hidden;
            }
        }

        private DateTime _downTime;
        private KeyboardHook.VKeys _downKey;

        private void KeyboardMonitor_KeyDown(KeyboardHook.VKeys key)
        {
            // 忽略用户按住Ctrl+鼠标的操作
            if (_downKey == KeyboardHook.VKeys.LCONTROL && key == KeyboardHook.VKeys.LCONTROL)
                return;

            _downTime = DateTime.Now;
            _downKey = key;
        }

        private async void KeyboardMonitor_KeyUp(KeyboardHook.VKeys key)
        {
            if (_downKey == KeyboardHook.VKeys.LCONTROL && key == KeyboardHook.VKeys.LCONTROL)
            {
                // 1、
                var deltaTime = DateTime.Now - _downTime;
                if (deltaTime.TotalMilliseconds < 300)
                {
                    var selectedText = string.Empty;
                    try
                    {
                        selectedText = (await AutomationHelper.GetSelectedText())?.Trim();
                        if (string.IsNullOrEmpty(selectedText) && UserConfig.Current.EnableScreenshotTranlate)
                        {
                            using (var screenImage = ScreenShotHelper.CaptureScreen())
                            {
                                var overlay = new Overlay(screenImage);
                                overlay.ShowDialog();
                                if (overlay.CropImage != null)
                                {
                                    using (var cropImage = overlay.CropImage)
                                    {
                                        var cropImageBase64 = ImageProcess.ImageToBase64String(cropImage);
                                        selectedText = new BaiduOCR(UserConfig.Current.ClientId, UserConfig.Current.ClientSecret).GeneralBasic(cropImageBase64);
                                    }
                                }
                            }
                        }
                    }
                    catch { }

                    // 2、
                    Visibility = Visibility.Visible;
                    selectedText = selectedText?.Trim();
                    if (string.IsNullOrEmpty(selectedText))
                        return;

                    textbox_origintext.Text = selectedText;
                    Translate(selectedText);
                }
            }

            _downKey = KeyboardHook.VKeys.NONAME;
        }

        private void btn_close_window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            MouseMonitor.LeftButtonDown -= MouseMonitor_LeftButtonDown;
            KeyboardMonitor.KeyUp -= KeyboardMonitor_KeyUp;
            KeyboardMonitor.KeyDown -= KeyboardMonitor_KeyDown;
        }

        private void search_btn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var originText = textbox_origintext.Text?.Trim();
            if (string.IsNullOrEmpty(originText))
                return;

            Translate(originText);
        }

        private void textbox_origintext_KeyDown(object sender, KeyEventArgs e)
        {
            var originText = textbox_origintext.Text?.Trim();
            if (string.IsNullOrEmpty(originText))
                return;

            if (e.Key == Key.Enter)
                Translate(originText);
        }

        private void Translate(string originText)
        {
            bing_translator_control.Translate(originText);
            youdao_translator_control.Translate(originText);
            google_translator_control.Translate(originText);
        }
    }
}
