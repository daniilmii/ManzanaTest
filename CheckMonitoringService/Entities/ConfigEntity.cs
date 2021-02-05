using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMonitoringService.Entities
{
    public class ConfigEntity
    {
        public ConfigEntity()
        {

        }
        public string CheckFolderPath { get; set; }
        public string GrabageFolderPath { get; set; }

        public string CompleteFolderPath { get; set; }
        public string HostIp { get; set; }
        public string HostPort { get; set; }

    }
}
