using System;
using System.Runtime.InteropServices;

namespace RejTech.Drawing
{
    /// <summary>Simple windowed operations while hiding Interop details</summary>
    public static class Window
    {
        /// <summary>Get foreground window handle</summary>
        public static IntPtr GetForeground()
        {
            return GetForegroundWindow();
        }

        /// <summary>Sets specified foreground window</summary>
        public static bool SetForeground(IntPtr window)
        {
            return BringWindowToTop(window);
        }

        /// <summary>Set specified foreground window without setting focus to it</summary>
        public static bool SetForegroundWithoutFocus(IntPtr window)
        {
            SetWindowPos(window, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            return SetWindowPos(window, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

        /// <summary>Does the specified window exist?</summary>
        public static bool Exists(IntPtr window)
        {
            return IsWindow(window);
        }

        /// <summary>Show specified window</summary>
        public static bool Show(IntPtr window)
        {
            return SetWindowPos(window, IntPtr.Zero, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE);
        }

        /// <summary>Hide specified window</summary>
        public static bool Hide(IntPtr window)
        {
            return SetWindowPos(window, IntPtr.Zero, 0, 0, 0, 0, SWP_HIDEWINDOW | SWP_NOMOVE | SWP_NOSIZE);
        }

        /// <summary>Minimizes specified window</summary>
        public static bool Minimize(IntPtr window)
        {
            return 0 != ShowWindow(window, SW_SHOWMINIMIZED);
        }

        /// <summary>Maximizes specified window</summary>
        public static bool Maximize(IntPtr window)
        {
            return 0 != ShowWindow(window, SW_SHOWMAXIMIZED);
        }

        /// <summary>Normalizes specified window</summary>
        public static bool Normalize(IntPtr window)
        {
            return 0 != ShowWindow(window, SW_SHOWNORMAL);
        }

        /// <summary>Closes specified window</summary>
        public static void Close(IntPtr window)
        {
            SendMessage(window, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>Sets size of specified window</summary>
        public static bool SetSize(IntPtr window, int width, int height)
        {
            return SetWindowPos(window, IntPtr.Zero, 0, 0, width, height, SWP_SHOWWINDOW | SWP_NOMOVE);
        }

        /// <summary>Sets position of specified window</summary>
        public static bool SetPosition(IntPtr window, int x, int y)
        {
            return SetWindowPos(window, IntPtr.Zero, x, y, 0, 0, SWP_SHOWWINDOW | SWP_NOSIZE);
        }

        /// <summary>Sets position and size of specified window</summary>
        public static bool SetPosition(IntPtr window, int x, int y, int width, int height)
        {
            return SetWindowPos(window, IntPtr.Zero, x, y, width, height, SWP_SHOWWINDOW);
        }

        /// <summary>Removal of taskbar icon from window</summary>
        public static void HideTaskbarIcon(IntPtr window)
        {
            SetWindowLong(window, GWL_EXSTYLE, GetWindowLong(window, GWL_EXSTYLE) | ~WS_EX_APPWINDOW);
        }

        public const int WM_CLOSE = 0x0010;
        public static IntPtr HWND_TOP = new IntPtr(0);
        public static IntPtr HWND_BOTTOM = new IntPtr(1);
        public static IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public const int HWND_BROADCAST = 0xFFFF;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const uint SWP_HIDEWINDOW = 0x0080;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;
        public const int WS_EX_APPWINDOW = 0x40000;
        public const int WS_EX_TOOLWINDOW = 0x0080;
        public const int GWL_EXSTYLE = -0x14;

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
    }
}
