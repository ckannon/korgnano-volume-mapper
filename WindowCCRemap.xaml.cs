using System;
using System.Linq;
using System.Windows;

namespace KorgVolumeMapper
{
    public partial class WindowCCRemap : Window
    {
        public bool Cancelled = false;
        private readonly string CCNumber;
        
        public CCMapping CcMapping =>
            new CCMapping
            {
                Mapping = new Mapping
                {
                    MixerFunction =
                        (MixerFunction) Enum.Parse(typeof(MixerFunction), comboMixerFunction.Text, true),
                    CCType = (ControlType) Enum.Parse(typeof(ControlType), comboCCType.Text, true),
                    MixerName = textMixerName.Text
                },
                CCNumber = CCNumber
            };
        
        public WindowCCRemap(CCMapping mapping)
        {
            InitializeComponent();
            Enum.GetValues(typeof(MixerFunction)).OfType<MixerFunction>().ToList().ForEach(function => comboMixerFunction.Items.Add(function));
            Enum.GetValues(typeof(ControlType)).OfType<ControlType>().ToList().ForEach(type => comboCCType.Items.Add(type));

            CCNumber = mapping.CCNumber;
            comboMixerFunction.SelectedValue = mapping.Mapping.MixerFunction;
            comboCCType.SelectedValue = mapping.Mapping.CCType;
            textMixerName.Text = mapping.Mapping.MixerName;
;        }

        private void ButtonFind_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new MixerList {Owner = this};
            window.ShowDialog();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Cancelled = true;
            Close();
        }

        private void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}