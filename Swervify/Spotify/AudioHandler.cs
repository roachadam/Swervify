using System;
using NAudio.CoreAudioApi;

namespace Swervify.Spotify
{
    public class AudioHandler
    {
        private float _previousLevel = -1;

        public void MuteSpotify()
        {
            using (MMDeviceEnumerator mde = new MMDeviceEnumerator())
            {
                using (MMDevice device = mde.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
                {
                    AudioSessionManager asm = device.AudioSessionManager;
                    SessionCollection sc = asm.Sessions;

                    for (int i = 0; i < sc.Count; i++)
                    {
                        string name = sc[i].GetSessionIdentifier;
                        if (name.IndexOf("spotify.exe", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            sc[i].SimpleAudioVolume.Mute = true;
                            _previousLevel = sc[i].SimpleAudioVolume.Volume;
                            //sc[i].SimpleAudioVolume.Volume = 0f;
                            break;
                        }
                    }
                }
            }
        }

        public void UnmuteSpotify()
        {
            using (MMDeviceEnumerator mde = new MMDeviceEnumerator())
            {
                using (MMDevice device = mde.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
                {
                    AudioSessionManager asm = device.AudioSessionManager;
                    SessionCollection sc = asm.Sessions;

                    for (int i = 0; i < sc.Count; i++)
                    {
                        string name = sc[i].GetSessionIdentifier;
                        if (name.IndexOf("spotify.exe", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            if (sc[i].SimpleAudioVolume.Mute)
                                sc[i].SimpleAudioVolume.Mute = false;

                            _previousLevel = sc[i].SimpleAudioVolume.Volume;
                            sc[i].SimpleAudioVolume.Volume = _previousLevel == -1 ? 1.0f : _previousLevel;
                            break;
                        }
                    }
                }
            }
        }
    }
}
