using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MuggleTranslator.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MuggleTranslator.ViewModel
{
    public class UserConfigViewModel : ViewModelBase
    {
        #region BindingProperty
        private bool _enableScreenshotTranlate;
        public bool EnableScreenshotTranlate
        {
            get => _enableScreenshotTranlate;
            set
            {
                _enableScreenshotTranlate = value;
                RaisePropertyChanged(nameof(EnableScreenshotTranlate));
            }
        }

        private string _clientId;
        public string ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                RaisePropertyChanged(nameof(ClientId));
            }
        }

        private string _clientSecret;
        public string ClientSecret
        {
            get => _clientSecret;
            set
            {
                _clientSecret = value;
                RaisePropertyChanged(nameof(ClientSecret));
            }
        }
        #endregion

        #region BindingCommand
        private RelayCommand _updateUserConfigCommand;
        public ICommand UpdateUserConfigCommand
        {
            get
            {
                if (_updateUserConfigCommand == null)
                    _updateUserConfigCommand = new RelayCommand(() => UpdateUserConfig());

                return _updateUserConfigCommand;
            }
        }

        private void UpdateUserConfig()
        {
            UserConfig.Update(_enableScreenshotTranlate, _clientId, _clientSecret);

            MessageBox.Show("保存成功", "提示");
        }

        private RelayCommand<RoutedEventArgs> _windowLoaded;
        public ICommand WindowLoadedCommand
        {
            get
            {
                if (_windowLoaded == null)
                    _windowLoaded = new RelayCommand<RoutedEventArgs>(OnWindowLoaded);

                return _windowLoaded;
            }
        }

        private void OnWindowLoaded(RoutedEventArgs e)
        {
            EnableScreenshotTranlate = UserConfig.Current.EnableScreenshotTranlate;
            ClientId = UserConfig.Current.ClientId;
            ClientSecret = UserConfig.Current.ClientSecret;
        }
        #endregion
    }
}
