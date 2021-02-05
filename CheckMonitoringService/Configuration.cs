using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckMonitoringService.Entities;
using Microsoft.Extensions.Configuration;

namespace CheckMonitoringService
{
    public static class Configurations
    {
        public static IConfiguration Configuration { get; set; }


        private static ConfigEntity _currentConfig = null;

        static Configurations() 
        {
            _currentConfig = new ConfigEntity();
        }

        public static ConfigEntity CurrentConfig
        {
            get { return _currentConfig; }
        }


        public static void ConfigLoader()
        {
            try
            {
                CurrentConfig.CheckFolderPath = Configuration["ChecksFolderPath"];
                CurrentConfig.GarbageFolderPath = Configuration["GarbageFolderPath"];
                CurrentConfig.CompleteFolderPath = Configuration["CompleteFolderPath"];
                CurrentConfig.HostIp = Configuration["HostIp"];
                CurrentConfig.HostPort = Configuration["HostPort"];
            }

            catch (Exception ex)
            {
                Logger.Log.Error("ConfigLoader failed", ex);
                Environment.Exit(0);
            }
        }

    }


}

