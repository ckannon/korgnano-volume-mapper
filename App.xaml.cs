using System;
using System.Runtime.InteropServices;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace KorgVolumeMapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
        // defines for commandline output
        public App()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
        }

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
    }
}