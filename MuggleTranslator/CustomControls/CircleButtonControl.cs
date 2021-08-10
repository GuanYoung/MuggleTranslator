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
    public class CircleButtonControl : Button
    {
        static CircleButtonControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleButtonControl), new FrameworkPropertyMetadata(typeof(CircleButtonControl)));
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
          nameof(Icon),
          typeof(string),
          typeof(CircleButtonControl)
        );

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}
