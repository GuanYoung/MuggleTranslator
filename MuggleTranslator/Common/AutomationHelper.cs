using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuggleTranslator.Common
{
    public class AutomationHelper
    {
        /// <summary>
        /// 获取用户当前选中文本
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetSelectedText()
        {
            ClipboardHelper.Clear();
            //AutoIt.AutoItX.Send("^c");    // 这个方法似乎有bug

            // https://stackoverflow.com/questions/14395377/how-to-simulate-a-ctrl-a-ctrl-c-using-keybd-event
            // Hold Control down and press C
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(C, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(C, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);

            // 获取数据
            await Task.Delay(200);

            return ClipboardHelper.GetText();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public const int KEYEVENTF_KEYDOWN = 0x0000; // New definition
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int VK_LCONTROL = 0xA2; //Left Control key code
        public const int A = 0x41; //A key code
        public const int C = 0x43; //C key code
    }
}
