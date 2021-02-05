using System;
using System.Runtime.InteropServices;

namespace KorgVolumeMapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
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