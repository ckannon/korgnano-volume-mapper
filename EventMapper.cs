using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Melanchall.DryWetMidi.Core;

namespace KorgVolumeMapper
{
    public class EventMapper
    {
        private readonly List<CCMapping> CCMappings;
        private volatile Process[] cachedProcesses;

        private Timer cachedProcessesTimer;

        public EventMapper(List<CCMapping> mappings)
        {
            CCMappings = mappings;
            cachedProcessesTimer = new Timer(state =>
            {
                Console.WriteLine("Getting new cached processes.");
                cachedProcesses = Process.GetProcesses();
            }, null, 0, 10000);
        }

        public void MapMidiEvent(MidiEvent eEvent)
        {
            if (!(eEvent is ControlChangeEvent ccE)) return;
            HandleMapping(CCMappings.FirstOrDefault(mapping => int.Parse(mapping.CCNumber) == ccE.ControlNumber), ccE);
        }

        private void HandleMapping(CCMapping mapping, ControlChangeEvent cc)
        {
            if (mapping == null) return;

            switch (mapping.Mapping.MixerMatchString)
            {
                case "System Volume":
                    ActionSystemVolume(mapping, cc);
                    break;
                case "System Mic Volume":
                    ActionSystemMicVolume(mapping, cc);
                    break;                
                case "Current Window":
                    var curWindowCaption = GetCaptionOfActiveWindow();
                    var curWindowPids = GetPID($"^{curWindowCaption}$");
                    ActionPID(curWindowPids, mapping, cc);
                    return;                
                case "N/A":
                    return;
                default:
                    var mappingPids = GetPID(mapping.Mapping.MixerMatchString);
                    ActionPID(mappingPids, mapping, cc);
                    break;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        private static string GetCaptionOfActiveWindow()
        {
            var strTitle = string.Empty;
            var handle = GetForegroundWindow();
            // Obtain the length of the text   
            var intLength = GetWindowTextLength(handle) + 1;
            var stringBuilder = new StringBuilder(intLength);
            if (GetWindowText(handle, stringBuilder, intLength) > 0)
            {
                strTitle = stringBuilder.ToString();
            }
            return strTitle;
        }
        
        private IEnumerable<int> GetPID(string appName)
        {
            if (cachedProcesses == null)
            {
                cachedProcesses = Process.GetProcesses();
            }
            var allProcesses = cachedProcesses.ToList(); 
            var r = new Regex(appName);
            var matches = allProcesses.Where(p => r.IsMatch(p.MainWindowTitle) | r.IsMatch(p.ProcessName)).Select(p => p.Id).ToList();
            return matches;
        }

        private static void ActionPID(IEnumerable<int> pids, CCMapping mapping, ControlChangeEvent cc)
        {
            foreach (var pid in pids)
            {
                if (mapping.Mapping.MixerFunction == MixerFunction.Mute)
                {
                    var isMuted = cc.ControlValue >= 1;
                    AudioManager.SetApplicationMute(pid, isMuted);
                    continue;
                }

                var newVol = (cc.ControlValue / 127f) * 100f;
                try
                {
                    AudioManager.SetApplicationVolume(pid, newVol);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error {e} occurred.");
                }
            }
        }

        private static void ActionSystemVolume(CCMapping mapping, ControlChangeEvent cc)
        {
            if (mapping.Mapping.MixerFunction == MixerFunction.Mute)
            {
                var isMuted = cc.ControlValue >= 1;
                AudioManager.SetMasterVolumeMute(isMuted);
                return;
            }

            var newVol = (cc.ControlValue / 127f) * 100f;
            AudioManager.SetMasterVolume(newVol);
        }
        
        private static void ActionSystemMicVolume(CCMapping mapping, ControlChangeEvent cc)
        {
            if (mapping.Mapping.MixerFunction == MixerFunction.Mute)
            {
                var isMuted = cc.ControlValue >= 1;
                AudioManager.SetMasterMicVolumeMute(isMuted);
                return;
            }

            var newVol = (cc.ControlValue / 127f) * 100f;
            AudioManager.SetMasterMicVolume(newVol);
        }
    }
}