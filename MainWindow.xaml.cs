using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KorgVolumeMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Dictionary<string, CCMapping> mappingHash = new Dictionary<string, CCMapping>();
        private readonly Dictionary<string, Button> ccButtonsHash = new Dictionary<string, Button>();
        private List<CCMapping> ccItems = new List<CCMapping>();
        private Button currentSelectionButton;
        private CCMapping currentSelectionItem;
        private readonly SolidColorBrush SelectionBrush = new SolidColorBrush(Color.FromRgb(0,50,50));
        private Brush UnselectedBrush;
        private EventMapper _eventMapper;
        private MidiManager _midiManager;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeCCMappings();
            InitializeHashes();
            
            _eventMapper = new EventMapper(ccItems);
            _midiManager = new MidiManager(_eventMapper);
            InitializeButtons();
            InitializeControls();
        }

        private void cc_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button button)
            {
                SelectCCByButton((string)button.Content);
            }
        }

        private void SelectCCByButton(string cc)
        {
            if (!mappingHash.ContainsKey(cc)) return;
            listBox.SelectedItem = mappingHash[cc];
            listBox.ScrollIntoView(listBox.SelectedItem);
        }
        
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                UnselectListItem((CCMapping) e.RemovedItems[0]);
            } 
            if (e.AddedItems.Count > 0)
            {
                SelectListItem((CCMapping) e.AddedItems[0]);
            }
        }

        private void SelectListItem(CCMapping newItem)
        {
            if (!ccButtonsHash.ContainsKey(newItem.CCNumber)) return;
            currentSelectionButton = ccButtonsHash[newItem.CCNumber];
            currentSelectionButton.Background = SelectionBrush;
            textCCNumber.Text = newItem.CCNumber;
            textMapping.Text = newItem.Mapping.MixerName + "|" + newItem.Mapping.MixerFunction;
            currentSelectionItem = newItem;
        }

        private void UnselectListItem(CCMapping removedItem)
        {
            if (currentSelectionButton == null) return;
            currentSelectionButton.Background = UnselectedBrush;
            currentSelectionButton = null;
            textCCNumber.Text = "";
            textMapping.Text = "";
            currentSelectionItem = null;
        }

        private void InitializeCCMappings()
        {
                        ccItems = new List<CCMapping>
            {
                new CCMapping() {CCNumber = "0", Mapping = new Mapping(ControlType.Range, "System Volume", MixerFunction.Volume)},
                new CCMapping() {CCNumber = "1"},
                new CCMapping() {CCNumber = "2"},
                new CCMapping() {CCNumber = "3"},
                new CCMapping() {CCNumber = "4"},
                new CCMapping() {CCNumber = "5"},
                new CCMapping() {CCNumber = "6"},
                new CCMapping() {CCNumber = "7"},
                new CCMapping() {CCNumber = "45"},
                new CCMapping() {CCNumber = "41"},
                new CCMapping() {CCNumber = "42"},
                new CCMapping() {CCNumber = "44"},
                new CCMapping() {CCNumber = "43"},
                new CCMapping() {CCNumber = "46"},
                new CCMapping() {CCNumber = "58"},
                new CCMapping() {CCNumber = "59"},
                new CCMapping() {CCNumber = "60"},
                new CCMapping() {CCNumber = "61"},
                new CCMapping() {CCNumber = "62"},
                new CCMapping() {CCNumber = "16"},
                new CCMapping() {CCNumber = "17"},
                new CCMapping() {CCNumber = "18"},
                new CCMapping() {CCNumber = "19"},
                new CCMapping() {CCNumber = "20"},
                new CCMapping() {CCNumber = "21"},
                new CCMapping() {CCNumber = "22"},
                new CCMapping() {CCNumber = "23"},
                new CCMapping() {CCNumber = "32"},
                new CCMapping() {CCNumber = "48"},
                new CCMapping() {CCNumber = "64"},
                new CCMapping() {CCNumber = "33"},
                new CCMapping() {CCNumber = "49"},
                new CCMapping() {CCNumber = "65"},
                new CCMapping() {CCNumber = "34"},
                new CCMapping() {CCNumber = "50"},
                new CCMapping() {CCNumber = "66"},
                new CCMapping() {CCNumber = "35"},
                new CCMapping() {CCNumber = "51"},
                new CCMapping() {CCNumber = "67"},
                new CCMapping() {CCNumber = "36"},
                new CCMapping() {CCNumber = "52"},
                new CCMapping() {CCNumber = "68"},
                new CCMapping() {CCNumber = "37"},
                new CCMapping() {CCNumber = "53"},
                new CCMapping() {CCNumber = "69"},
                new CCMapping() {CCNumber = "38"},
                new CCMapping() {CCNumber = "54"},
                new CCMapping() {CCNumber = "70"},
                new CCMapping() {CCNumber = "39"},
                new CCMapping() {CCNumber = "55"},
                new CCMapping() {CCNumber = "71"}
            };
            ccItems.Sort((a,b) => int.Parse(a.CCNumber).CompareTo(int.Parse(b.CCNumber)));
        }
        
        private void InitializeHashes()
        {
            ccItems.ForEach(mapping => mappingHash.Add(mapping.CCNumber, mapping));
        }
        
        private void InitializeControls()
        {
            listBox.ItemsSource = ccItems;
            _midiManager.GetInputDevices().ToList().ForEach(s => comboInputDevice.Items.Add(s));
            _midiManager.GetOutputDevices().ToList().ForEach(s => comboOutputDevice.Items.Add(s));
            comboInputDevice.SelectedIndex = 0;
            comboOutputDevice.SelectedIndex = 0;
        }

        private void InitializeButtons()
        {
            foreach (var button in parentGrid.Children.OfType<Button>())
            {
                if (UnselectedBrush == null)
                {
                    UnselectedBrush = button.Background;
                }
                ccButtonsHash.Add(button.Content.ToString(), button);
            }
        }

        private void inputSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _midiManager.SetInputDevice(e.AddedItems[0].ToString());
        }

        private void outputSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _midiManager.SetOutputDevice(e.AddedItems[0].ToString());
        }

        private void ButtonMapTo_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentSelectionItem == null) return;
            var window = new WindowCCRemap(currentSelectionItem) {Owner = this};
            window.ShowDialog();
            if (window.Cancelled) return;
            var newItem = window.CcMapping;
            for (var i = 0; i < ccItems.Count; i++)
            {
                if (ccItems[i].CCNumber != newItem.CCNumber) continue;
                ccItems[i] = newItem;
                break;
            }

            mappingHash[newItem.CCNumber] = newItem;
            listBox.Items.Refresh();
            SelectCCByButton(newItem.CCNumber);
        }
    }
}