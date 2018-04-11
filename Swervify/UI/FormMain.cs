using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Swervify.Core;
using Swervify.Spotify;

namespace Swervify.UI
{
    public partial class FormMain : Form
    {
        // check if spotify installed
        private SpotifyHooker _spotifyHooker;
        private NotifyIcon _notifyIcon;
        private bool _initialized;
        private bool _ballonShown;
        private bool _isAdmin = true;
        private bool _hide;
        private string _settingsLocation = Path.Combine(Application.StartupPath, "config.json");

        public FormMain(bool hide = false)
        {
            _hide = hide;
            _spotifyHooker = new SpotifyHooker();
            InitializeComponent();
            LoadSettings();

        }

        #region " Settings "

        private void cbStartup_CheckedChanged(object sender, EventArgs e)
        {
            using (var reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (reg == null) return;
                if (cbStartup.Checked)
                {
                    if(reg.GetValue("Swervify") == null)
                        reg.SetValue("Swervify", $"\"{Application.ExecutablePath}\" -startup");
                }                 
                else
                    reg.DeleteValue("Swervify");
            }
        }

        private void cbBanner_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBanner.Checked)
                HostsBlocker.Block();
            else
                HostsBlocker.Unblock();

            if (!_initialized) return;
            MessageBox.Show("This feature will require you to restart Spotify before changes can take effect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbVideos_CheckedChanged(object sender, EventArgs e)
        {
            _spotifyHooker.BlockVideos = cbVideos.Checked;
        }

        private void LoadSettings()
        {
            if (File.Exists(_settingsLocation))
            {
                Settings s = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_settingsLocation));
                cbStartup.Checked = s.Startup;
                cbBanner.Checked = s.BlockBanners;
                cbVideos.Checked = s.BlockVideos;
                _ballonShown = s.BalloonShown;
            }
            _initialized = true;
        }

        
        private void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(new Settings()
            {
                Startup = cbStartup.Checked,
                BlockBanners = cbBanner.Checked,
                BlockVideos = cbVideos.Checked,
                BalloonShown = _ballonShown,
            });
            File.WriteAllText(_settingsLocation, json);
        }

        #endregion

        #region " Form Handling "

        
        private void FormMain_Load(object sender, EventArgs e)
        {
            pbUAC.Image = SystemIcons.Shield.ToBitmap();
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                _isAdmin = false;
                cbBanner.ForeColor = SystemColors.ControlDarkDark;
            }
            AutoScaleMode = AutoScaleMode.Dpi;    
            _notifyIcon = CreateNotifyIcon();
            _spotifyHooker.Status += S_Status;
            _spotifyHooker.Hook();
            if (_hide)
            {
                this.WindowState = FormWindowState.Minimized;
                _hide = false;
            }         
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                Hide();
                if(!_ballonShown)
                    _notifyIcon.ShowBalloonTip(5000);
                _ballonShown = true;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                Show();
                _notifyIcon?.Dispose();
                _notifyIcon = CreateNotifyIcon();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
        #endregion

        private void S_Status(SpotifyHooker.TrackData data)
        {
            string status = data.Playing ? "Playing" : "Paused";
            if (!data.SpotifyOpen)
                status = "Spotify not active";

            if (data.IsAd)
            {
                status = "Muting advertisement...";
                data.Track = "None";
                data.Artist = "None";
            }

            if (InvokeRequired)
            {
                lTrack.Invoke(new Action(() => lTrack.Text = data.Track));
                lArtist.Invoke(new Action(() => lArtist.Text = data.Artist));
                lStatus.Invoke(new Action(() => lStatus.Text = status));
            }
            else
            {
                lTrack.Text = data.Track;
                lArtist.Text = data.Artist;
                lStatus.Text = status;
            }
        }

        private NotifyIcon CreateNotifyIcon()
        {
            try
            {
                if (components == null)
                {
                    components = new System.ComponentModel.Container();
                }

                NotifyIcon notifyIcon = new NotifyIcon(components)
                {
                    Icon = Icon,
                    BalloonTipTitle = Application.ProductName,
                    BalloonTipText = "Don't sweat it, Swervify is still running. Click on the icon in the system tray to bring it back.",
                    BalloonTipIcon = ToolTipIcon.Info,
                    Visible = true
                };

                notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
                return notifyIcon;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void cbBanner_Click(object sender, EventArgs e)
        {
            if (!_isAdmin)
            {
                MessageBox.Show("You must re-run Swervify as administrator to use this feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            cbBanner.Checked = !cbBanner.Checked;
        }
    }

}
