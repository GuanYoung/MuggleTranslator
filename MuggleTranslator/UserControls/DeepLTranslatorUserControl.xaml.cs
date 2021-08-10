using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MuggleTranslator.Common;
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

namespace MuggleTranslator.UserControls
{
    /// <summary>
    /// DeepLTranslatorUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DeepLTranslatorUserControl : UserControl
    {
        private DeepLTranslatorUserControlViewModel _viewModel;
        public DeepLTranslatorUserControl()
        {
            InitializeComponent();

            _viewModel = new DeepLTranslatorUserControlViewModel();
            DataContext = _viewModel;
        }

        public async void Translate(string originalText)
        {
            _viewModel.Translating = true;
            _viewModel.Checked = false;

            var targetText = await Task.Run(() =>
            {
                return "正在努力开发中。。";
            });

            _viewModel.TargetText = targetText?.Trim();
            content_control.UpdateLayout();
            //await Task.Delay(100);

            _viewModel.Checked = true;
            _viewModel.Translating = false;
        }
    }

    public class DeepLTranslatorUserControlViewModel : ViewModelBase
    {
        #region BindingProperty
        private bool _translating;
        public bool Translating
        {
            get => _translating;
            set
            {
                _translating = value;
                RaisePropertyChanged(nameof(Translating));
            }
        }

        private string _targetText;
        public string TargetText
        {
            get => _targetText;
            set
            {
                _targetText = value;
                RaisePropertyChanged(nameof(TargetText));
            }
        }

        private bool _checked;
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                RaisePropertyChanged(nameof(Checked));
            }
        }
        #endregion

        #region BindingCommand
        private RelayCommand _copyCommand;
        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                    _copyCommand = new RelayCommand(() => Copy());

                return _copyCommand;
            }
        }

        private void Copy()
        {
            ClipboardHelper.SetText(TargetText);
        }
        #endregion
    }
}
