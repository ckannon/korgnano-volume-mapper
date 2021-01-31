namespace KorgVolumeMapper
{
    public enum ControlType
    {
        None,
        Momentary,
        Toggle,
        Range
    }
    
    public enum MixerFunction
    {
        None,
        Mute,
        Volume
    }
    
    public class CCMapping
    {
        public string CCNumber { get; set; }
        public Mapping Mapping { get; set; }

        public CCMapping()
        {
            Mapping = new Mapping {MixerFunction = MixerFunction.None, MixerName = "N/A"};
        }

        public CCMapping(string ccNumber, Mapping mapping)
        {
            CCNumber = ccNumber;
            Mapping = mapping;
        }

        public CCMapping(string ccNumber, MixerFunction func, string mixer)
        {
            CCNumber = ccNumber;
            Mapping = new Mapping {MixerFunction = func, MixerName = mixer};
        }
    }

    public class Mapping
    {
        public ControlType CCType { get; set; }
        public string MixerName { get; set; }
        
        public MixerFunction MixerFunction { get; set; }

        public Mapping()
        {
        }

        public Mapping(ControlType ccType, string mixerName, MixerFunction mixerFunction)
        {
            CCType = ccType;
            MixerName = mixerName;
            MixerFunction = mixerFunction;
        }
    }
}