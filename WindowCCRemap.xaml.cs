using System.Windows;

namespace KorgVolumeMapper
{
    public partial class WindowCCRemap : Window
    {
        public WindowCCRemap()
        {
            InitializeComponent();
        }

        private void ButtonFind_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new MixerList {Owner = this};
            window.ShowDialog();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}