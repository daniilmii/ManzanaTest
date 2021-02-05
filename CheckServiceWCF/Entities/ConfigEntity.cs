using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckServiceWCF.Entities
{
    public class ConfigEntity
    {
        public ConfigEntity()
        {
            
        }
        public string ConnectionString { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }


    }
}