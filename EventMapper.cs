using System;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;

namespace KorgVolumeMapper
{
    public class EventMapper
    {
        private readonly List<CCMapping> CCMappings;

        public EventMapper(List<CCMapping> mappings)
        {
            CCMappings = mappings;
        }
        
        public void MapMidiEvent(MidiEvent eEvent)
        {
            if (!(eEvent is ControlChangeEvent ccE)) return;
            HandleMapping(CCMappings.FirstOrDefault(mapping => int.Parse(mapping.CCNumber) == ccE.ControlNumber), ccE);
        }

        private static void HandleMapping(CCMapping mapping, ControlChangeEvent cc)
        {
            if (mapping == null) return;
            
            var newVol = (cc.ControlValue / 127f) * 100f;
            AudioManager.SetMasterVolume(newVol);
        }
    }
}