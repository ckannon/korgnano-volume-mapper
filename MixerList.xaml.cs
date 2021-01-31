using System.Windows;

namespace KorgVolumeMapper
{
    public partial class MixerList : Window
    {
        public MixerList()
        {
            InitializeComponent();
        }

        private void buttonOK_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}