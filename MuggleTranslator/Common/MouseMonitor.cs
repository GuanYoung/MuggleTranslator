using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MuggleTranslator.Common.MouseHook;

namespace MuggleTranslator.Common
{
    public static class MouseMonitor
    {
        private static MouseHook _mouseHook;

        public static void Initialize()
        {
            if (_mouseHook != null)
                throw new InvalidOperationException("鼠标监听程序已经初始化");

            _mouseHook = new MouseHook();
            _mouseHook.MouseMove += _mouseHook_MouseMove;
            _mouseHook.MouseWheel += _mouseHook_MouseWheel;
            _mouseHook.DoubleClick += _mouseHook_DoubleClick;
            _mouseHook.MiddleButtonDown += _mouseHook_MiddleButtonDown;
            _mouseHook.MiddleButtonUp += _mouseHook_MiddleButtonUp;
            _mouseHook.RightButtonDown += _mouseHook_RightButtonDown;
            _mouseHook.RightButtonUp += _mouseHook_RightButtonUp;
            _mouseHook.LeftButtonDown += _mouseHook_LeftButtonDown;
            _mouseHook.LeftButtonUp += _mouseHook_LeftButtonUp;
            _mouseHook.Install();
        }

        public static void Release()
        {
            _mouseHook.Uninstall();
            _mouseHook.MouseMove -= _mouseHook_MouseMove;
            _mouseHook.MouseWheel -= _mouseHook_MouseWheel;
            _mouseHook.DoubleClick -= _mouseHook_DoubleClick;
            _mouseHook.MiddleButtonDown -= _mouseHook_MiddleButtonDown;
            _mouseHook.MiddleButtonUp -= _mouseHook_MiddleButtonUp;
            _mouseHook.RightButtonDown -= _mouseHook_RightButtonDown;
            _mouseHook.RightButtonUp -= _mouseHook_RightButtonUp;
            _mouseHook.LeftButtonDown -= _mouseHook_LeftButtonDown;
            _mouseHook.LeftButtonUp -= _mouseHook_LeftButtonUp;
            _mouseHook = null;
        }

        private static void _mouseHook_RightButtonUp(MouseHookEventArgs e)
        {
            RightButtonUp?.Invoke(e);
        }

        private static void _mouseHook_RightButtonDown(MouseHookEventArgs e)
        {
            RightButtonDown?.Invoke(e);
        }

        private static void _mouseHook_LeftButtonUp(MouseHookEventArgs e)
        {
            LeftButtonUp?.Invoke(e);
        }

        private static void _mouseHook_LeftButtonDown(MouseHookEventArgs e)
        {
            LeftButtonDown?.Invoke(e);
        }

        private static void _mouseHook_MouseMove(MouseHookEventArgs e)
        {
            MouseMove?.Invoke(e);
        }

        private static void _mouseHook_MouseWheel(MouseHookEventArgs e)
        {
            MouseWheel?.Invoke(e);
        }

        private static void _mouseHook_MiddleButtonUp(MouseHookEventArgs e)
        {
            MiddleButtonUp?.Invoke(e);
        }

        private static void _mouseHook_MiddleButtonDown(MouseHookEventArgs e)
        {
            MiddleButtonDown?.Invoke(e);
        }

        private static void _mouseHook_DoubleClick(MouseHookEventArgs e)
        {
            DoubleClick?.Invoke(e);
        }

        #region Events

        public static event MouseHookCallback LeftButtonDown;
        public static event MouseHookCallback LeftButtonUp;
        public static event MouseHookCallback RightButtonDown;
        public static event MouseHookCallback RightButtonUp;
        public static event MouseHookCallback MouseMove;
        public static event MouseHookCallback MouseWheel;
        public static event MouseHookCallback DoubleClick;
        public static event MouseHookCallback MiddleButtonDown;
        public static event MouseHookCallback MiddleButtonUp;

        #endregion

    }
}
