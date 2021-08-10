using MuggleTranslator.Common;
using MuggleTranslator.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SWF = System.Windows.Forms;

namespace MuggleTranslator
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private SWF.NotifyIcon notifyIcon = new SWF.NotifyIcon();

        public App()
        {
            KeyboardMonitor.Initialize();
            MouseMonitor.Initialize();

            notifyIcon.Icon = new Icon("muggle.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(2000, "麻瓜翻译启动了", "选中文本后, 按一次Ctrl即翻译", SWF.ToolTipIcon.Info);
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // 退出按钮
            SWF.MenuItem exitMenu = new SWF.MenuItem("退出", new EventHandler(
                                                                        (sender, args) => { Application.Current.Shutdown(); }
                                                            )
                                    );
            SWF.MenuItem settingMenu = new SWF.MenuItem("用户设置", new EventHandler(
                                                                        (sender, args) => { new UserConfigView().Show(); }
                                                            )
                                    );

            // 右键菜单列表
            notifyIcon.ContextMenu = new SWF.ContextMenu(
                                                    new SWF.MenuItem[] { 
                                                        settingMenu, 
                                                        exitMenu 
                                                    }
                                    );
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;
            if(mainWindow.Visibility == Visibility.Hidden)
                mainWindow.Visibility = Visibility.Visible;
        }
    }
}
