using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuggleTranslator.CustomControls
{
    public class ToggleButtonControl : ToggleButton
    {
        static ToggleButtonControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButtonControl), new FrameworkPropertyMetadata(typeof(ToggleButtonControl)));
        }

        public static readonly DependencyProperty CheckedIconProperty = DependencyProperty.Register(
          nameof(CheckedIcon),
          typeof(string),
          typeof(ToggleButtonControl)
        );

        public string CheckedIcon
        {
            get { return (string)GetValue(CheckedIconProperty); }
            set { SetValue(CheckedIconProperty, value); }
        }

        public static readonly DependencyProperty UnCheckedIconProperty = DependencyProperty.Register(
          nameof(UnCheckedIcon),
          typeof(string),
          typeof(ToggleButtonControl)
        );

        public string UnCheckedIcon
        {
            get { return (string)GetValue(UnCheckedIconProperty); }
            set { SetValue(UnCheckedIconProperty, value); }
        }

        public static readonly DependencyProperty ImageSizeProperty = DependencyProperty.Register(
          nameof(ImageSize),
          typeof(double),
          typeof(LoadingControl),
          new FrameworkPropertyMetadata(12.0)
        );

        public double ImageSize
        {
            get { return (double)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }
    }
}
