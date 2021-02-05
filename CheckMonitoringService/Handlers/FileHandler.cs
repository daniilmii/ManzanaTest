﻿using CheckMonitoringService.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CheckMonitoringService.Handlers
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

    }
}
