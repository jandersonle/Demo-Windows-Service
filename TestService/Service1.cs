using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TestService.FTPLoggingService;

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


namespace TestService
{
    public partial class Service1 : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        private int eventId = 1;
        public Service1()
        {
            InitializeComponent();

            eventLog1 = new System.Diagnostics.EventLog();

            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                        "Mysource", "MyNewLog");
            }

            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
            // this must match the event log fomr the windows service, but you must recusrively go back on to the subdirectory
            // over and over however there must be a directory shift as you recursively create s

        }

        protected override void OnStart(string[] args)
        {
            // update the service state to pending 
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("Reached OnStart");
            Timer timer = new Timer();
            timer.Interval = 12000;
            // add elapased time
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("entered on stop");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // runner initliazes the ftp helper
            try
            {
                _ = new FTPRunner();
                eventLog1.WriteEntry("FTP Connection is running...", EventLogEntryType.Information, eventId++);

            } catch(Exception e)
            {
                eventLog1.WriteEntry(e.Message);        
            }
 
            
        }

    }
}
