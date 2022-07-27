// From http://stackoverflow.com/questions/217902/reading-writing-an-ini-file
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// Change this to match your program's normal namespace
namespace RejTech
{
    public class IniFile   // revision 11
    {
        private string Path;
        private string BaseName;
        private string ExecutablePath;
        private string ExecutableFolder;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = "")
        {
            BaseName = Assembly.GetExecutingAssembly().GetName().Name;
            ExecutablePath = Assembly.GetExecutingAssembly().Location;
            ExecutableFolder = System.IO.Path.GetDirectoryName(ExecutablePath);
            if (IniPath == "") IniPath = ExecutableFolder;
            Path = new FileInfo(IniPath + "\\" + BaseName + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? BaseName, Key, "", RetVal, 255, Path.Replace("\\\\", "\\"));
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section)
        {
            WritePrivateProfileString(Section ?? BaseName, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section)
        {
            Write(Key, null, Section ?? BaseName);
        }

        public void DeleteSection(string Section)
        {
            Write(null, null, Section ?? BaseName);
        }

        public bool KeyExists(string Key, string Section)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}