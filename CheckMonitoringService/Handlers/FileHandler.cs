using CheckMonitoringService.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMonitoringService.Handlers
{
    class FileHandler
    {

        public static string  ReadFile(string filePath) 
        {
            using (var sr = new StreamReader(filePath))
            {
               return sr.ReadToEnd();
            }
           
        }
        public static T SerializeFile<T>(string filePath) where T : CheckBaseType
        {

            string fileInfo = ReadFile(filePath);
            if (!String.IsNullOrEmpty(fileInfo))
            {
                var obj = JsonConvert.DeserializeObject<T>(fileInfo);
                return obj;
            }
            else 
            {
                throw new Exception("Empty file");
            }
        }
    }
}
