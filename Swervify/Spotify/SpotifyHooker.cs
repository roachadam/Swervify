using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Swervify.Web;

namespace Swervify.Spotify
{
    class SpotifyHooker
    {

        #region " Private Members "

        private HttpClient _client;
        private string _oAuthToken;
        private string _csrfToken;
        private Thread _hookThread;
        private Thread _monitorThread;
        private AudioHandler _audio;
        private bool _adJustPlayed;
        private bool _adFinished;
        private string _previousTrack;
        private bool _previousState; // true = playing, false = paused
        private bool _blockVideos;
        private bool _isSpotifyOpen;
        private bool _running;
        private bool _initializedTokens;
        private const string SPOTILOCAL = "http://swervify.spotilocal.com:4380";

        #endregion

        #region " Public Members "

        public bool BlockVideos
        {
            get => _blockVideos;
            set => _blockVideos = value;
        }

        #endregion

        public event TrackHandler Status = delegate { };
        public delegate void TrackHandler(TrackData data);

        public class TrackData
        {
            public bool IsAd;
            public bool Playing;
            public string Track;
            public string Artist;
            public bool SpotifyOpen = true;
        }
      
        public SpotifyHooker()
        {
            _client = new HttpClient();
            _audio = new AudioHandler();
            _audio.UnmuteSpotify();
        }

        #region " Get Tokens "

        private void GetOAuthToken()
        {
            string resp = _client.Get("https://open.spotify.com/token");

            if (resp == null)
            {
                MessageBox.Show("Failed to get OAuth token.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            var json = JsonConvert.DeserializeObject<dynamic>(resp);
            _oAuthToken = json.t;
            if (json.error != null)
            {
                _oAuthToken = null;
            }
        }

        private void GetCSRFToken()
        {
            _client.SetHeader("Origin", "https://open.spotify.com");
            string resp = _client.Get($"{SPOTILOCAL}/simplecsrf/token.json");

            if (resp == null)
            {
                MessageBox.Show("Failed to get OAuth token.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            var json = JsonConvert.DeserializeObject<dynamic>(resp);
            _csrfToken = json.token;
            if (json.error != null)
            {
                _csrfToken = null;
            }
        }

        #endregion

        #region " Hook "

        public void Hook()
        {
            _running = true;
            if (_monitorThread == null)
            {
                _monitorThread = new Thread(SpotifyMonitorThread);
                _monitorThread.IsBackground = true;
                _monitorThread.Start();
            }
            if (_hookThread == null)
            {
                _hookThread = new Thread(HookThread);
                _hookThread.IsBackground = true;
                _hookThread.Start();
            }
        }

        public void Unhook()
        {
            _running = false;
            
        }
        #endregion

        #region " Threads "

        private void HookThread()
        {
            while (_running)
            {
                try
                {
                    if (!_isSpotifyOpen)
                    {
                        Status(new TrackData
                        {
                            Track =  "None",
                            Artist = "None",
                            Playing = false,
                            SpotifyOpen = false
                        });
                        Thread.Sleep(10000);
                        continue;
                    }

                    if (!_initializedTokens)
                    {
                        GetOAuthToken();
                        GetCSRFToken();
                        _initializedTokens = true;
                    }
                    EnsureHelperRunning();

                    string response = _client.Get($"{SPOTILOCAL}/remote/status.json?oauth={_oAuthToken}&csrf={_csrfToken}");

                    Console.WriteLine(response);

                    var json = JsonConvert.DeserializeObject<dynamic>(response);

                    // Handle errors
                    if (json.error != null)
                    {
                        string errorMessage = json.error.message;

                        if (errorMessage.Contains("Invalid OAuth token"))
                            GetOAuthToken();
                        if (errorMessage.Contains("Expired OAuth token"))
                            GetOAuthToken();
                        if (errorMessage.Contains("Invalid Csrf token"))
                            GetCSRFToken();
                        if (errorMessage.Contains("No user logged in"))
                        {
                            _isSpotifyOpen = false;
                            Thread.Sleep(3000);
                            continue;
                        }
                        Console.WriteLine(errorMessage);
                    }

                    // If we can't skip, it's an ad
                    bool nextEnabled = json.next_enabled; // C# throws error if we dont assign/cast these dynamics for comparison
                    string trackType = json.track.track_type;
                    bool isAd = !nextEnabled || trackType == "ad";
                    if (isAd)
                    {
                        // Mute spotify
                        
                        _audio.MuteSpotify();
                        _adJustPlayed = true;

                        Status(new TrackData
                        {
                            IsAd = isAd,
                        });
                    }
                    else
                    {
                        _adFinished = true;
                    }
                    if (_adJustPlayed && _adFinished)
                    {
                        Thread.Sleep(750);
                        _audio.UnmuteSpotify();
                        _adJustPlayed = false;
                        _adFinished = false;
                    }

                    // Either a music video or dumb video spotlight thing
                    if (_blockVideos && !isAd && json.track.track_resource == null)
                    {
                        Skip();
                    }

                    // Send track data to event
                    if (json.track != null && json.track.track_resource != null)
                    {
                        string track = json.track.track_resource.name;

                        if (_previousTrack == null || !_previousTrack.Equals(track, StringComparison.OrdinalIgnoreCase) || json.playing != _previousState)
                        {
                            _previousTrack = track;
                            _previousState = json.playing;

                            // Fire that event, pow pow
                            Status(new TrackData
                            {
                                Track = track,
                                Artist = json.track.artist_resource.name,
                                IsAd = isAd,
                                Playing = json.playing
                            });
                        }
                    }

                    Thread.Sleep(500);
                }
                catch (RuntimeBinderException e)
                {
                    // Ignore, json error
                }
                catch (Exception e)
                {
                    MessageBox.Show($"An unexpected error has occured.\r\n\r\n{e}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
            }
        }

        private void SpotifyMonitorThread()
        {
            while (_running)
            {
                _isSpotifyOpen = IsSpotifyRunning();

                Thread.Sleep(325);
            }
        }

        #endregion

        #region " Helper Methods "

        private void Skip()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        private bool IsSpotifyRunning()
        {
            return Process.GetProcesses().Count(t => t.ProcessName.Equals("spotify", StringComparison.OrdinalIgnoreCase)) >= 3;
        }

        private void EnsureHelperRunning()
        {
            var helper = Process.GetProcesses().Where(t => t.ProcessName.Equals("spotifywebhelper", StringComparison.OrdinalIgnoreCase));

            if(!helper.Any())
                OpenWebHelper();
        }

        private void EnableWebHelper()
        {
            string prefsFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)  + @"\Spotify\prefs";

            if (File.Exists(prefsFile))
            {
                string contents = File.ReadAllText(prefsFile);
                if (contents.IndexOf("webhelper.enabled=false", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    contents = contents.Replace("webhelper.enabled=false", "webhelper.enabled=true");
                    File.WriteAllText(prefsFile, contents);
                }
            }
            else
            {
                MessageBox.Show("Unable to enable Spotify Web Helper. You must do so manually in order for Swervify to function properly. " +
                                "Configure this setting in Spotify under\r\nEdit -> Preferences -> Show Advanced Settings -> Allow Spotify to be opened from the web",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }
        }

        private void OpenWebHelper()
        {
            EnableWebHelper();

            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string locationOne = appdata + @"\Spotify\Data\SpotifyWebHelper.exe";
            string locationTwo = appdata + @"\Spotify\SpotifyWebHelper.exe";

            if (File.Exists(locationOne))
                Process.Start(locationOne);

            else if (File.Exists(locationTwo))
                Process.Start(locationTwo);
        }

        #endregion

        #region " Win32 "

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        private const int KEYEVENTF_EXTENTEDKEY = 1;
        private const int VK_MEDIA_NEXT_TRACK = 0xB0;

        #endregion
    }
}
