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
        class CCMapping
        {
            public string CCNumber { get; set; }
            public string Mapping { get; set; }
        }

        private readonly Dictionary<string, CCMapping> mappingHash = new Dictionary<string, CCMapping>();
        private readonly Dictionary<string, Button> ccButtonsHash = new Dictionary<string, Button>();
        private Button currentSelection;
        private readonly SolidColorBrush SelectionBrush = new SolidColorBrush(Color.FromRgb(0,50,50));
        private Brush UnselectedBrush;
        private EventMapper _eventMapper;
        private MidiManager _midiManager;
        
        public MainWindow()
        {
            InitializeComponent();
            initializeHashes();
            
            _eventMapper = new EventMapper();
            _midiManager = new MidiManager(_eventMapper);
            initializeButtons();
            initializeCombos();
        }

        private void cc_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button button)
            {
                SelectCC((string)button.Content);
            }
        }

        private void SelectCC(string cc)
        {
            if (!mappingHash.ContainsKey(cc)) return;
            listBox.SelectedItem = mappingHash[cc];
            listBox.ScrollIntoView(listBox.SelectedItem);
        }
        
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newItem = (CCMapping) e.AddedItems[0];

            if (!ccButtonsHash.ContainsKey(newItem.CCNumber)) return;

            if (currentSelection != null)
            {
                currentSelection.Background = UnselectedBrush;
            }
            currentSelection = ccButtonsHash[newItem.CCNumber];
            currentSelection.Background = SelectionBrush;
            textCCNumber.Text = newItem.CCNumber;
            textMapping.Text = newItem.Mapping;
        }

        private void initializeHashes()
        {
            var items = new List<CCMapping>
            {
                new CCMapping() {CCNumber = "0", Mapping = ""},
                new CCMapping() {CCNumber = "1", Mapping = ""},
                new CCMapping() {CCNumber = "2", Mapping = ""},
                new CCMapping() {CCNumber = "3", Mapping = ""},
                new CCMapping() {CCNumber = "4", Mapping = ""},
                new CCMapping() {CCNumber = "5", Mapping = ""},
                new CCMapping() {CCNumber = "6", Mapping = ""},
                new CCMapping() {CCNumber = "7", Mapping = ""},
                new CCMapping() {CCNumber = "45", Mapping = ""},
                new CCMapping() {CCNumber = "41", Mapping = ""},
                new CCMapping() {CCNumber = "42", Mapping = ""},
                new CCMapping() {CCNumber = "44", Mapping = ""},
                new CCMapping() {CCNumber = "43", Mapping = ""},
                new CCMapping() {CCNumber = "46", Mapping = ""},
                new CCMapping() {CCNumber = "58", Mapping = ""},
                new CCMapping() {CCNumber = "59", Mapping = ""},
                new CCMapping() {CCNumber = "60", Mapping = ""},
                new CCMapping() {CCNumber = "61", Mapping = ""},
                new CCMapping() {CCNumber = "62", Mapping = ""},
                new CCMapping() {CCNumber = "16", Mapping = ""},
                new CCMapping() {CCNumber = "17", Mapping = ""},
                new CCMapping() {CCNumber = "18", Mapping = ""},
                new CCMapping() {CCNumber = "19", Mapping = ""},
                new CCMapping() {CCNumber = "20", Mapping = ""},
                new CCMapping() {CCNumber = "21", Mapping = ""},
                new CCMapping() {CCNumber = "22", Mapping = ""},
                new CCMapping() {CCNumber = "23", Mapping = ""},
                new CCMapping() {CCNumber = "32", Mapping = ""},
                new CCMapping() {CCNumber = "48", Mapping = ""},
                new CCMapping() {CCNumber = "64", Mapping = ""},
                new CCMapping() {CCNumber = "33", Mapping = ""},
                new CCMapping() {CCNumber = "49", Mapping = ""},
                new CCMapping() {CCNumber = "65", Mapping = ""},
                new CCMapping() {CCNumber = "34", Mapping = ""},
                new CCMapping() {CCNumber = "50", Mapping = ""},
                new CCMapping() {CCNumber = "66", Mapping = ""},
                new CCMapping() {CCNumber = "35", Mapping = ""},
                new CCMapping() {CCNumber = "51", Mapping = ""},
                new CCMapping() {CCNumber = "67", Mapping = ""},
                new CCMapping() {CCNumber = "36", Mapping = ""},
                new CCMapping() {CCNumber = "52", Mapping = ""},
                new CCMapping() {CCNumber = "68", Mapping = ""},
                new CCMapping() {CCNumber = "37", Mapping = ""},
                new CCMapping() {CCNumber = "53", Mapping = ""},
                new CCMapping() {CCNumber = "69", Mapping = ""},
                new CCMapping() {CCNumber = "38", Mapping = ""},
                new CCMapping() {CCNumber = "54", Mapping = ""},
                new CCMapping() {CCNumber = "70", Mapping = ""},
                new CCMapping() {CCNumber = "39", Mapping = ""},
                new CCMapping() {CCNumber = "55", Mapping = ""},
                new CCMapping() {CCNumber = "71", Mapping = ""}
            };
            items.Sort((a,b) => int.Parse(a.CCNumber).CompareTo(int.Parse(b.CCNumber)));
            items.ForEach(mapping => mappingHash.Add(mapping.CCNumber, mapping));
            listBox.ItemsSource = items;
        }
        
        private void initializeCombos()
        {
            _midiManager.GetInputDevices().ToList().ForEach(s => comboInputDevice.Items.Add(s));
            _midiManager.GetOutputDevices().ToList().ForEach(s => comboOutputDevice.Items.Add(s));
        }

        private void initializeButtons()
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
            var window = new WindowCCRemap {Owner = this};
            var showDialog = window.ShowDialog();
        }

    }
}