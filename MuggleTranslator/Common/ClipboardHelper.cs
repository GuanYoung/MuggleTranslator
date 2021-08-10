using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MuggleTranslator.Common
{
    public class ClipboardHelper
    {
        public static string GetText()
        {
            if (!IsClipboardFormatAvailable(CF_UNICODETEXT))
                return null;

            try
            {
                if (!OpenClipboard(IntPtr.Zero))
                    return null;

                IntPtr handle = GetClipboardData(CF_UNICODETEXT);
                if (handle == IntPtr.Zero)
                    return null;

                IntPtr pointer = IntPtr.Zero;

                try
                {
                    pointer = GlobalLock(handle);
                    if (pointer == IntPtr.Zero)
                        return null;

                    int size = GlobalSize(handle);
                    byte[] buff = new byte[size];

                    Marshal.Copy(pointer, buff, 0, size);

                    return Encoding.Unicode.GetString(buff).TrimEnd('\0');
                }
                finally
                {
                    if (pointer != IntPtr.Zero)
                        GlobalUnlock(handle);
                }
            }
            finally
            {
                CloseClipboard();
            }
        }

        public static void SetText(string text)
        {
            text = text ?? string.Empty;
            if (ClipboardIdle())
            {
                unsafe
                {
                    byte[] textBytes = System.Text.Encoding.Unicode.GetBytes(text);
                    var memorySize = textBytes.Length + 2; //2是unicode的空字符长度

                    void* hGlobalMemory = GlobalAlloc(66, new UIntPtr((uint)memorySize)).ToPointer();

                    IntPtr pGlobalMemory = GlobalLock(new IntPtr(hGlobalMemory));
                    MemSet(pGlobalMemory.ToPointer(), 0, memorySize);
                    MemCopy(pGlobalMemory.ToPointer(), textBytes, textBytes.Length);
                    GlobalUnlock(pGlobalMemory);

                    OpenClipboard(IntPtr.Zero);
                    EmptyClipboard();
                    SetClipboardData(13, pGlobalMemory); //13是CF_UNICODETEXT
                    CloseClipboard();
                }
            }
        }

        public static void SetImage(Image img)
        {
            //var bitmap = img as Bitmap;
            //using (bitmap)
            //{
            //    var handle = bitmap.GetHbitmap();
            //    try
            //    {
            //        OpenClipboard(IntPtr.Zero);
            //        EmptyClipboard();

            //        SetClipboardData(CF_BITMAP, handle);

            //        CloseClipboard();
            //    }
            //    finally { User32X.DeleteObject(handle); }
            //}
        }

        public static Image GetImage()
        {
            var imgSorce = Clipboard.GetImage();
            var bmp = new Bitmap(imgSorce.PixelWidth, imgSorce.PixelHeight, PixelFormat.Format32bppPArgb);
            var bmpdata = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            imgSorce.CopyPixels(Int32Rect.Empty, bmpdata.Scan0, bmpdata.Height * bmpdata.Stride, bmpdata.Stride);
            bmp.UnlockBits(bmpdata);
            return bmp as Image;
        }

        public static void Clear()
        {
            if (ClipboardIdle())
            {
                OpenClipboard(IntPtr.Zero);
                EmptyClipboard();
                CloseClipboard();

                return;
            }
            throw new Exception("剪切板被占用，清空剪切板内容失败");
        }

        public static bool ClipboardIdle()
        {
            var result = RetryHelper.Retry(() =>
            {
                var windowHandle = GetOpenClipboardWindow();
                if (windowHandle != IntPtr.Zero)    // 剪贴板被占用
                    return false;
                return true;
            }, 3000, interval: 200);    // 稍微等待一下即可，剪贴板不可能一直被占用的

            return result;
        }

        #region Pinvoke
        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public unsafe static extern void MemCopy(void* dest, byte[] src, int count);

        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public unsafe static extern void MemSet(void* dest, int value, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr GetOpenClipboardWindow();
        [DllImport("user32.dll")]
        static extern bool EmptyClipboard();

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseClipboard();

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern int GlobalSize(IntPtr hMem);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/win32/dataxchg/standard-clipboard-formats
        /// </summary>
        private const uint CF_UNICODETEXT = 13U;
        private const uint CF_BITMAP = 2U;
        #endregion
    }
}
