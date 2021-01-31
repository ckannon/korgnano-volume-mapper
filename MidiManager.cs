using System;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Devices;

namespace KorgVolumeMapper
{
    public class MidiManager
    {
        private EventMapper _eventMapper;
        private IInputDevice SelectedInputDevice { get; set; }
        private IOutputDevice SelectedOutputDevice { get; set; }

        public MidiManager(EventMapper mapper)
        {
            _eventMapper = mapper;
        }

        public IEnumerable<string> GetInputDevices()
        {
            return InputDevice.GetAll().Select(d => d.Name).AsEnumerable();
        }

        public IEnumerable<string> GetOutputDevices()
        {
            return OutputDevice.GetAll().Select(d => d.Name).AsEnumerable();
        }

        public bool SetInputDevice(string deviceName)
        {
            if (SelectedInputDevice != null)
            {
                if (SelectedInputDevice.IsListeningForEvents)
                {
                    SelectedInputDevice.StopEventsListening();
                    SelectedInputDevice.EventReceived -= OnEventReceived;
                }
            }
            
            SelectedInputDevice = InputDevice.GetByName(deviceName);
            
            if (SelectedInputDevice == null) return false;
            
            SelectedInputDevice.EventReceived += OnEventReceived;
            SelectedInputDevice.StartEventsListening();
            
            return true;
        }

        private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            _eventMapper.MapMidiEvent(e.Event);
        }

        public bool SetOutputDevice(string deviceName)
        {
            SelectedOutputDevice = OutputDevice.GetByName(deviceName);
            return SelectedOutputDevice != null;
        }
    }
}