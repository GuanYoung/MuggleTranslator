using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HtmlAgilityPack;
using MuggleTranslator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// YoudaoTranslatorUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class YoudaoTranslatorUserControl : UserControl
    {
        private YoudaoTranslatorUserControlViewModel _viewModel;
        public YoudaoTranslatorUserControl()
        {
            InitializeComponent();

            _viewModel = new YoudaoTranslatorUserControlViewModel();
            DataContext = _viewModel;
        }

        public async void Translate(string originalText)
        {
            _viewModel.Translating = true;
            _viewModel.Checked = false;

            var targetText = await Task.Run(() =>
            {
                var html = $"https://dict.youdao.com/w/{originalText}";
                HtmlWeb web = new HtmlWeb();
                var rootNode = web.Load(html).DocumentNode;

                try 
                {
                    var ydTransNode = rootNode.SelectSingleNode("//div[@id='ydTrans']");
                    if (ydTransNode != null)
                    {
                        return rootNode.SelectSingleNode("//div[@id='fanyiToggle']/div/p[2]")?.InnerText;
                    }
                    else
                    {
                        var sb = new StringBuilder();

                        // 中 -> 英 时候的排版格式
                        var liNodes = rootNode.SelectNodes("//div[@id='phrsListTab']/div[contains(@class, 'trans-container')]/ul/li");
                        if (liNodes != null)
                        {
                            foreach (var liNode in liNodes)
                            {
                                var translationItemText = liNode.InnerText;
                                var ks = translationItemText.Split(new char[] { ' ' });

                                var kind = ks.Count() >= 1 ? ks[0] : string.Empty;
                                var translation = ks.Count() >= 2 ? ks[1] : string.Empty;
                                sb.AppendLine($"{kind} {translation}");
                            }
                        }
                        // 英 -> 中 时候的排版格式
                        else
                        {
                            var pNodes = rootNode.SelectNodes("//div[@id='phrsListTab']/div[contains(@class, 'trans-container')]/ul/p");
                            foreach (var pNode in pNodes)
                            {
                                var kind = pNode.SelectSingleNode("./span[not(@class='contentTitle')]")?.InnerText;

                                var translation = new StringBuilder();
                                foreach (var aNode in pNode.SelectNodes("./span[@class='contentTitle']/a"))
                                    translation.Append($"{aNode.InnerText?.Trim()};");
                                sb.AppendLine($"{kind} {translation}");
                            }
                        }

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
    }

    public class YoudaoTranslatorUserControlViewModel : ViewModelBase
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
