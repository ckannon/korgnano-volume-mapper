# korgnano-volume-mapper
Allows mapping of Midi CC controls to the Windows Volume Mixer. For example if you have a Midi CC controller that has a volume slider, this allows you to map the slider 
to the System Volume, or to the volume of a specific application name. Also, momentary switches or toggles can be bound to various functions like Mute on an application, or
mute on a microphone.

Currently uses an image of the Korg Nanokontrol 2 for mapping purposes.. cuz that's what this was written for. 

WIP. 

## Current features
- catches CC midi input from any bound device
- if CC 0 is adjusted (0, 127 range) it will modify System Volume
- supports application PIDs, but no way yet to map CC numbers to them

## Next
- allow remapping of CC range inputs to a application volume
- allow mapping of a CC toggle input to an application mute/unmute
