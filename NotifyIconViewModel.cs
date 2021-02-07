using System.Windows;
using System.Windows.Input;

namespace KorgVolumeMapper
{
    public class NotifyIconViewModel
    {
        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow.WindowState = WindowState.Normal;
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => Application.Current.MainWindow.WindowState = WindowState.Minimized,
                };
            }
        }


        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get { return new DelegateCommand {CommandAction = () => Application.Current.Shutdown()}; }
        }
    }
}