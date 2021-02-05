using CheckMonitoringService.Entities;
using CheckMonitoringService.Handlers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CheckMonitoringService
{
    public partial class MonitoringService : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
        // List<FileSystemWatcher> watcherList;

        public MonitoringService()
        {
            InitializeComponent();

            //watcherList = new List<FileSystemWatcher>();

            monitoringLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MonitoringServiceSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MonitoringServiceSource", "MonitoringServiceLog");
            }
            monitoringLog.Source = "MonitoringServiceSource";
            monitoringLog.Log = "MonitoringServiceLog";

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("conf.json", optional: true)
            .Build();
            Configurations.Configuration = configuration;
            //monitoringLog.WriteEntry("In OnStart.");
        }
        internal void ConsoleDebug(string[] args)
        {
            this.OnStart(args);
            Console.WriteLine("Press Enter to stop windows service ...");
            Console.ReadLine();
            this.OnStop();
            Environment.Exit(0);
        }
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            Logger.InitLogger();
            Configurations.ConfigLoader();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Logger.Log.Info("Process exiting");
            StartMonitoring();


            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            monitoringLog.WriteEntry("In OnStop.");

            //foreach (var watcher in watcherList)
            //{
            //    watcher.EnableRaisingEvents = false;

            //    watcher.Dispose();
            //}

            //watcherList.Clear();

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

       
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        public void StartMonitoring()
        {
            Thread thread = new Thread(MonitoringDirectory);
            thread.Start();
            //MonitoringDirectoryWeak();
        }

        public void MonitoringDirectory()
        {
                Logger.Log.Error(String.Format("Start monitoring directory {0} ...", Configurations.CurrentConfig.CheckFolderPath));
                while (true)
                {
                    IEnumerable<string> allfiles = Directory.EnumerateFiles(Configurations.CurrentConfig.CheckFolderPath);

                    foreach (string filename in allfiles)
                    {
                        FilesProcessor(filename, Path.GetFileName(filename));
                    }
                    Thread.Sleep(1000);
                }
        }
        private static string UniqueFilePath(string folderPath, string filefullName)
        {
            return String.Format(folderPath + "/" + (DateTime.UtcNow).ToString("dd_MM-HH_mm_ss") + filefullName);
        }
        private static void FilesProcessor(string fullPath, string fileName)
        {
            Logger.Log.Info(String.Format("Processing file in CheckFolder: {0}", fullPath));

            try
            {
                if (Path.GetExtension(fullPath).Equals(".txt"))
                {

                    CheckEntity check = (SerializeHandler.DeserializeFile<CheckEntity>(fullPath));

                    RequestHandler.SendRequest(Configurations.CurrentConfig.HostIp, Configurations.CurrentConfig.HostPort, "/PostCheck", check);

                    File.Move(fullPath, UniqueFilePath(Configurations.CurrentConfig.CompleteFolderPath, fileName));


                    Logger.Log.Info(String.Format("Move file to CompleteFolder: {0}", fullPath));
                }
                else
                {
                    throw new Exception("File format wrong");
                }

            }
            catch (Exception ex)
            {
                if (File.Exists(fullPath))
                {
                    File.Move(fullPath, UniqueFilePath(Configurations.CurrentConfig.GarbageFolderPath, fileName));


                    Logger.Log.Error(String.Format("Move file to GarbageFolder: {0}", fullPath), ex);
                }
                else if (Directory.Exists(fullPath))
                {
                    Directory.Move(fullPath, UniqueFilePath(Configurations.CurrentConfig.GarbageFolderPath, fileName));


                    Logger.Log.Error(String.Format("Move file to GarbageFolder: {0} , ", fullPath), ex);
                }
                else
                {
                    throw new Exception(String.Format("Unexpected location of moving file {0} ", fullPath));
                }

            }
        }


        /*

      private static void OnCreated(object source, FileSystemEventArgs e)
      {
          FilesProcessor(e.FullPath, e.Name);
      }

       [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
       private void MonitoringDirectoryWeak()
       {
           try
           {
               FileSystemWatcher watcher = new FileSystemWatcher();


               watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

               if (Directory.Exists(Configurations.CurrentConfig.CheckFolderPath))
               {
                   watcher.InternalBufferSize = 65536;
                   watcher.Path = Configurations.CurrentConfig.CheckFolderPath;
                   watcher.EnableRaisingEvents = true;
                   watcher.Filter = "*.*";
                   watcher.Created += OnCreated;



                   watcherList.Add(watcher);

                   Logger.Log.Info(String.Format("Monitoring files with extension({0}) in the folder({1})", watcher.Filter, watcher.Path));

               }
               else
               {
                   throw new Exception(String.Format("CheckFolder not found ({0})", Configurations.CurrentConfig.CheckFolderPath));
               }

           }
           catch (Exception ex)
           {
               Logger.Log.Error("Monitoring files failed", ex);
           }
       }
      */
    }
}