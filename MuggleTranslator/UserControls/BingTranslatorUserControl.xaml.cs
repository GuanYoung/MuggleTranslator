using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HtmlAgilityPack;
using MuggleTranslator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// BingTranslatorUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class BingTranslatorUserControl : UserControl
    {
        private BingTranslatorUserControlViewModel _viewModel;
        public BingTranslatorUserControl()
        {
            InitializeComponent();

            _viewModel = new BingTranslatorUserControlViewModel();
            DataContext = _viewModel;
        }
        
        public async void Translate(string originalText)
        {
            _viewModel.Translating = true;
            _viewModel.Checked = false;

            var targetText = await Task.Run(() =>
            {
                // 使用bing词典的方式来翻译句子
                var url = $"https://cn.bing.com/dict/clientsearch?mkt=zh-CN&setLang=zh&form=BDVEHC&ClientVer=BDDTV3.5.1.4320&q={originalText}";
                HtmlWeb web = new HtmlWeb();
                var rootNode = web.Load(url).DocumentNode;

                // 判断翻译结果的类型
                try
                {
                    var webNode = rootNode.SelectSingleNode("//span[contains(@class,'client_def_title_web')]");
                    if (webNode == null)
                        // 长句
                        return rootNode.SelectSingleNode("//div[@class = 'client_tbl']").ParentNode.ParentNode.NextSibling.NextSibling?.InnerText;
                    else
                        // 单词、短句
                        return webNode.ParentNode.NextSibling.SelectSingleNode("./div[1]/div[1]/div[1]")?.InnerText;
                }
                catch
                {
                    return "暂无搜索结果";
                }
            });

            _viewModel.TargetText = targetText?.Trim();
            content_control.UpdateLayout();
            //await Task.Delay(100);

            _viewModel.Checked = true;
            _viewModel.Translating = false;
        }
    }

    public class BingTranslatorUserControlViewModel : ViewModelBase
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
