using System;
using Melanchall.DryWetMidi.Core;

namespace KorgVolumeMapper
{
    public class EventMapper
    {
        public static void MapMidiEvent(MidiEvent eEvent)
        {
            if (!(eEvent is ControlChangeEvent ccE)) return;
            if (ccE.ControlNumber != 0) return;
            var newVol = (ccE.ControlValue / 127f) * 100f;
            AudioManager.SetMasterVolume(newVol);
        }
    }
}