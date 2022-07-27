using System.Diagnostics;
using RejTech.Drawing;

namespace RejTech
{
    public class BrowserLauncher
    {
        public bool Launch(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path);
            psi.WindowStyle = ProcessWindowStyle.Normal;
            Process process = Process.Start(psi);
            return (process != null);
        }

        public bool LaunchMax(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path);
            psi.WindowStyle = ProcessWindowStyle.Maximized;
            Process process = Process.Start(psi);
            Window.Maximize(process.Handle);
            return (process != null);
        }

        public bool LaunchPos(string path, int left, int top, int width, int height)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path);
            psi.WindowStyle = ProcessWindowStyle.Normal;
            Process process = Process.Start(psi);
            Window.SetPosition(process.Handle, left, top, width, height);
            return (process != null);
        }
    }
}
