using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CheckServiceWCF.Handlers
{
    static class FileHandler
    {

        public static string ReadFile(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }

        }
        public static void WriteFile(string filePath, string str)
        {
            using (var sw = new StreamWriter(filePath))
            {
                sw.WriteLine(str);
            }

        }

    }
}
