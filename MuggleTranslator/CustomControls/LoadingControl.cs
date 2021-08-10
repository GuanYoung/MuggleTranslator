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

namespace MuggleTranslator.CustomControls
{
    public class LoadingControl : Control
    {
        static LoadingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingControl), new FrameworkPropertyMetadata(typeof(LoadingControl)));
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
          nameof(Radius),
          typeof(double),
          typeof(LoadingControl),
          new FrameworkPropertyMetadata(5.0)
        );

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
          nameof(Thickness),
          typeof(double),
          typeof(LoadingControl),
          new FrameworkPropertyMetadata(1.0)
        );

        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }
    }
}