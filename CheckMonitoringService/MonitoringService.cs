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
        List<FileSystemWatcher> watcherList;
        public MonitoringService()
        {
            InitializeComponent();

            watcherList = new List<FileSystemWatcher>();

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
            Console.ReadLine();
            this.OnStop();
        }


        protected override void OnStart(string[] args)
        {
            Main(args);
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            monitoringLog.WriteEntry("In OnStop.");

            foreach (var watcher in watcherList)
            {
                watcher.EnableRaisingEvents = false;

                watcher.Dispose();
            }

            watcherList.Clear();



            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }


        public void Main(string[] arg)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            Logger.InitLogger();
            Configurations.ConfigLoader();

            FilesMonitoring();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void FilesMonitoring()
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher();

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastWrite |
                                       NotifyFilters.FileName |
                                       NotifyFilters.DirectoryName |
                                        NotifyFilters.LastAccess |
                                        NotifyFilters.Attributes |
                                        NotifyFilters.Size
                                        ;


                if (Directory.Exists(Configurations.CurrentConfig.CheckFolderPath))
                {
                    watcher.Path = Configurations.CurrentConfig.CheckFolderPath;
                    watcher.EnableRaisingEvents = true;
                    watcher.IncludeSubdirectories = true;
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
        private static void OnCreated(object source, FileSystemEventArgs e)
        {

            Logger.Log.Info(String.Format("Add File to CheckFolder: {0} , {1}", e.FullPath, e.ChangeType));

            try
            {

            }
            finally 
            {

            }
            if (Path.GetExtension(e.FullPath).Equals(".txt"))
            {

                CheckEntity check = FileHandler.SerializeFile<CheckEntity>(e.FullPath);

                string d = String.Format(Configurations.CurrentConfig.CompleteFolderPath + "/" + (DateTime.UtcNow).ToString("dd_MM-HH_mm_ss") + e.Name);
                File.Move(e.FullPath, d);
            }
            else
            {
                string d = String.Format(Configurations.CurrentConfig.GrabageFolderPath + "/" + (DateTime.UtcNow).ToString("dd_MM-HH_mm_ss") + e.Name);
                File.Move(e.FullPath, d);
            }



            
        }



       

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
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


    }
}