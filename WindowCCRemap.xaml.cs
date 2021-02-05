using System;
using System.Linq;
using System.Text.RegularExpressions;
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
                    MixerMatchString = textMixerName.Text
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
            textMixerName.Text = mapping.Mapping.MixerMatchString;
;        }

        private void ButtonFind_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new MixerList {Owner = this};
            window.ShowDialog();
            if (!window.IsCancelled)
            {
                textMixerName.Text = $"^{window.SelectedProcessName}$";
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Cancelled = true;
            Close();
        }

        private void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
               new Regex(CcMapping.Mapping.MixerMatchString);    
            }
            catch
            {
                var b = MessageBox.Show(this, $"Error in regular expression: {CcMapping.Mapping.MixerMatchString}","Error With Mixer Match String");
                return;
            }
            Close();
        }
    }
}