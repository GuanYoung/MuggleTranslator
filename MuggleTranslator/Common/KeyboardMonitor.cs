using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MuggleTranslator.Common.KeyboardHook;

namespace MuggleTranslator.Common
{
    public class KeyboardMonitor
    {
        private static KeyboardHook _keyboardHook;

        public static void Initialize()
        {
            if (_keyboardHook != null)
                throw new InvalidOperationException("键盘监听程序已经初始化");

            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyDown += _keyboardHook_KeyDown;
            _keyboardHook.KeyUp += _keyboardHook_KeyUp;
            _keyboardHook.Install();
        }

        public static void Release()
        {
            _keyboardHook.KeyDown -= _keyboardHook_KeyDown;
            _keyboardHook.KeyUp -= _keyboardHook_KeyUp;
            _keyboardHook.Uninstall();
            _keyboardHook = null;
        }

        private static void _keyboardHook_KeyDown(KeyboardHook.VKeys key)
        {
            KeyDown?.Invoke(key);
        }

        private static void _keyboardHook_KeyUp(KeyboardHook.VKeys key)
        {
            KeyUp?.Invoke(key);
        }

        public static event KeyboardHookCallback KeyDown;
        public static event KeyboardHookCallback KeyUp;
    }
}
