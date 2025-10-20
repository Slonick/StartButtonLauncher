using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace StartButtonLauncher.Helpers
{
    public static class WindowHelper
    {
        public static bool IsWindowVisible(string title)
        {
            var process = Process.GetProcessesByName(title).FirstOrDefault();
            if (process != null && process.MainWindowHandle != IntPtr.Zero)
            {
                return IsWindowVisible(process.MainWindowHandle);
            }

            return false;
        }

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
    }
}