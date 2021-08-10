using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HtmlAgilityPack;
using MuggleTranslator.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// GoogleTranslatorUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class GoogleTranslatorUserControl : UserControl
    {
        private GoogleTranslatorUserControlViewModel _viewModel;
        public GoogleTranslatorUserControl()
        {
            InitializeComponent();

            _viewModel = new GoogleTranslatorUserControlViewModel();
            DataContext = _viewModel;
        }

        public async void Translate(string originalText)
        {
            _viewModel.Translating = true;
            _viewModel.Checked = false;

            var targetText = await Task.Run(() =>
            {
                try
                {
                    var srcLanguage = GetSrcLanguage(originalText);
                    var targetLanguage = srcLanguage == "zh-CN" ? "en" : "zh-CN";

                    var request = (HttpWebRequest)WebRequest.Create($"https://translate.googleapis.com/translate_a/single?client=gtx&dt=t&sl=auto&tl={targetLanguage}&q={originalText}");
                    request.Method = "GET";
                    request.ContentType = "application/json; charset=UTF-8";

                    var response = (HttpWebResponse)request.GetResponse();
                    using (Stream myResponseStream = response.GetResponseStream())
                    {
                        var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);

                        var sb = new StringBuilder();

                        foreach (var item in JArray.Parse(myStreamReader.ReadToEnd())[0])
                            sb.Append(item[0]);

                        return sb.ToString();
                    }
                }
                catch
                {
                    return "暂无搜索结果";
                }
            });

            _viewModel.TargetText = targetText?.Trim();
            content_control.UpdateLayout();

            _viewModel.Checked = true;
            _viewModel.Translating = false;
        }

        private string GetSrcLanguage(string originalText)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://translate.googleapis.com/translate_a/single?client=gtx&dt=t&sl=auto&tl=zh-CN&q={originalText}");
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";

            var response = (HttpWebResponse)request.GetResponse();
            using (Stream myResponseStream = response.GetResponseStream())
            {
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                return JArray.Parse(retString)[2]?.ToString();
            }
        }
    }

    public class GoogleTranslatorUserControlViewModel : ViewModelBase
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
