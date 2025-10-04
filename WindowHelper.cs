using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace StartButtonLauncher
{
    public static class WindowHelper
    {
        private const int SW_RESTORE = 9;

        public static bool IsWindowVisible(string title)
        {
            var process = Process.GetProcessesByName(title).FirstOrDefault();
            if (process != null && process.MainWindowHandle != IntPtr.Zero)
            {
                return IsWindowVisible(process.MainWindowHandle);
            }

            return false;
        }

        public static void ShowWindow(string title)
        {
            var process = Process.GetProcessesByName(title).FirstOrDefault();
            ShowWindow(process.MainWindowHandle, SW_RESTORE);
        }

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}