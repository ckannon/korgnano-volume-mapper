using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KorgVolumeMapper
{
    public partial class MixerList : Window
    {
        public string SelectedProcessName { get; set; }
        public bool IsCancelled { get; set; }

        public MixerList()
        {
            InitializeComponent();
            var allProcesses = Process.GetProcesses();
            var procList = allProcesses.ToList().Where(p => p.MainWindowTitle.Length > 0).Select(p => p.MainWindowTitle).ToList();
            procList.Sort((a, b) => a.CompareTo(b));
            listBox.ItemsSource = procList;
            listBox.SelectionMode = SelectionMode.Single;
        }

        private void buttonOK_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedProcessName = (string) listBox.SelectedValue;
            IsCancelled = false;
            Close();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            IsCancelled = true;
            Close();
        }
    }
}