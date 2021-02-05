using CheckServiceWCF.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                CurrentConfig.ConnectionString = Configuration["ConnectionString"];
               
            }

            catch (Exception ex)
            {
                Logger.Log.Error("ConfigLoader failed", ex);
            }
        }

    }

}