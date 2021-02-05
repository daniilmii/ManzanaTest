using CheckServiceWCF.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CheckServiceWCF.Handlers
{
    static class SerializeHandler
    {
        public static T DeserializeFile<T>(string filePath) where T : CheckBaseType
        {

            string fileInfo = FileHandler.ReadFile(filePath);
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
        public static string SerializeMessage(object obj)
        {
            var res = JsonConvert.SerializeObject(obj);
            return res;
        }
    }
}
