using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using RejTech.Drawing;

namespace RejTech
{
    static class Program
    {
		private static Mutex mutex = null;
		private static string cachedGUID = null;
		
		public static readonly int WM_SHOWEXISTINGAPP = Window.RegisterWindowMessage("WM_SHOWEXISTINGAPP|" + AssemblyGUID);
		public static readonly int WM_EXITEXISTINGAPP = Window.RegisterWindowMessage("WM_EXITEXISTINGAPP|" + AssemblyGUID);

		/// <summary>Get this app's GUID</summary>
		private static string AssemblyGUID //Ensures that there is only one instance of the application by giving each instance its own code
		{
			get
			{
				if (!String.IsNullOrEmpty(cachedGUID)) return cachedGUID;
				object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), false);
				if (attributes.Length == 0) return String.Empty;
				cachedGUID = ((GuidAttribute)attributes[0]).Value;
				return cachedGUID;
			}
		}

		/// <summary>Check if our app is already running</summary>
		/// <returns>returns true if already running</returns>
		private static bool IsAppAlreadyRunning()
		{
			bool bCreatedNew;
			if (mutex != null)
            {
				try
				{
					mutex.ReleaseMutex();
				}
				catch (Exception) { }
			}
			mutex = new Mutex(true, "Global\\" + AssemblyGUID, out bCreatedNew);
			return !bCreatedNew;
		}

		/// <summary>Send signal to show first instance even if minimized to system tray</summary>
		public static void WakeExistingApp()
		{
			Window.PostMessage(
				(IntPtr)Window.HWND_BROADCAST,
				Program.WM_SHOWEXISTINGAPP,
				IntPtr.Zero,
				IntPtr.Zero);
		}

		/// <summary>Send signal to quit first instance even if minimized to system tray</summary>
		public static void ExitExistingApp()
		{
			Window.PostMessage(
				(IntPtr)Window.HWND_BROADCAST,
				Program.WM_EXITEXISTINGAPP,
				IntPtr.Zero,
				IntPtr.Zero);
		}

		/// <summary>Cleans up and releases mutex</summary>
		private static void Exit()
		{
			if (mutex != null)
			{
				try
				{
					mutex.ReleaseMutex();
				}
				catch (Exception) { }
				mutex = null;
			}
			Application.Exit();
		}

		/// <summary>Run our instance of app</summary>
		/// <param name="autostart">true if we're launching to system tray</param>
		private static void Run(string options)
        {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainUtilityForm(options));
		}

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
        static void Main(string[] args)
        {
			string command = "";
			if (args.Length > 0) command = args[0].ToLower();
			if (command.StartsWith("--")) command.Remove(0, 1);

			if (command.Equals("-help") || command.Equals("-h") || command.Equals("-?"))
            {
				MessageBox.Show(System.AppDomain.CurrentDomain.FriendlyName + " command line options:\n\n" +
					"  -autostart  \tLaunch app silently to system tray.\n" +
					"  -notest        \tLaunch without test.\n" +
					"  -exit          \tExit already-running app.\n" +
					"  -help          \tDisplay this screen.\n",
					"Command Line Help");
			}
			else if (command.Equals("-exit"))
			{
				// Exit existing app instance (e.g. installer wants to upgrade me)
				Program.ExitExistingApp();
			}
			else if (command.Equals("-autostart"))
			{
				// Launch to system tray only if not already running
				if (!Program.IsAppAlreadyRunning()) Program.Run("-autostart");
			}
			else if (Program.IsAppAlreadyRunning())
			{
				// No command line options, already running
				Program.WakeExistingApp();
            }
			else if (command.Equals("-notest"))
			{
				// Launch without test
				Program.Run("-notest");
			}
			else
			{
				// No command line options, NOT already running
				Program.Run("");
			}
			Program.Exit();
		}
	}
}
