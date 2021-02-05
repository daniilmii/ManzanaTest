using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
            bool configCorrect = true;
            try
            {

                CurrentConfig.CheckFolderPath = Configuration["ChecksFolderPath"];
                CurrentConfig.GarbageFolderPath = Configuration["GarbageFolderPath"];
                CurrentConfig.CompleteFolderPath = Configuration["CompleteFolderPath"];
                CurrentConfig.HostIp = Configuration["HostIp"];
                CurrentConfig.HostPort = Configuration["HostPort"];

                if (!Directory.Exists(CurrentConfig.CheckFolderPath)) configCorrect = false;
                else if (!Directory.Exists(CurrentConfig.GarbageFolderPath)) configCorrect = false;
                else if (!Directory.Exists(CurrentConfig.CompleteFolderPath)) configCorrect = false;
                else if (!Regex.IsMatch(CurrentConfig.HostIp, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$")) configCorrect = false;
                else if (!UInt16.TryParse(CurrentConfig.HostPort, out ushort port)) configCorrect = false;



                if (!configCorrect)
                {
                    Logger.Log.Error(String.Format("Config file with errors..."));
                    Environment.Exit(0);
                }



            }

            catch (Exception ex)
            {
                Logger.Log.Error("ConfigLoader failed", ex);
                Environment.Exit(0);
            }
        }

    }


}

