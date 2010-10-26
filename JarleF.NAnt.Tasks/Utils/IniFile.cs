using System.Runtime.InteropServices;
using System.Text;

namespace JarleF.NAnt.Tasks.Utils
{
    public class IniFile
    {
        // Methods
        public IniFile(string fileName)
        {
            FileName = fileName;
        }
        

        [DllImport("Kernel32.dll", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        public string GetString(string section, string key, string @default)
        {
            var returnedString = new StringBuilder(1000);
            if (GetPrivateProfileString(section, key, @default, returnedString, returnedString.Capacity, FileName) > 0)
            {
                return returnedString.ToString();
            }
            return string.Empty;
        }

        [DllImport("Kernel32.dll", EntryPoint = "WritePrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        public void WriteString(string section, string key, string text)
        {
            WritePrivateProfileString(section, key, text, FileName);
        }

        // Properties
        public string FileName
        {
            get; private set;
        }
    }


}
