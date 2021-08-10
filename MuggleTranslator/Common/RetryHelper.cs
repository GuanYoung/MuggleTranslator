using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuggleTranslator.Common
{
    /// <summary>
    /// https://github.com/App-vNext/Polly
    /// </summary>
    public static class RetryHelper
    {
        public static bool Retry(Func<bool> func, int timeout, int times = -1, int interval = 200, int delay = 0)
        {
            if (delay > 0)
                System.Threading.Thread.Sleep(delay);
            var sw = Stopwatch.StartNew();
            do
            {
                if (func())
                    return true;
                times--;
                System.Threading.Thread.Sleep(interval);
            } while (sw.ElapsedMilliseconds < timeout || times == 0);
            sw.Stop();
            return false;
        }
    }
}
