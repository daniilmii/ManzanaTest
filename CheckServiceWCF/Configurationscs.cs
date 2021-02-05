using CheckServiceWCF.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CheckServiceWCF
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

               
                CurrentConfig.ConnectionString = Configuration["ConnectionString"];
                CurrentConfig.Ip = Configuration["Ip"];
                CurrentConfig.Port = Configuration["Port"];

                
                if ((CurrentConfig.ConnectionString.Equals(""))) configCorrect = false;
                else if (!Regex.IsMatch(CurrentConfig.Ip, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$")) configCorrect = false;
                else if (!UInt16.TryParse(CurrentConfig.Port, out ushort port)) configCorrect = false;



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