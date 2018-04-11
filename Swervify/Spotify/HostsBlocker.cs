using System;
using System.IO;
using System.Windows.Forms;

namespace Swervify.Spotify
{
    public static class HostsBlocker
    {
        private static string _hostsFile = Environment.SystemDirectory[0] + ":\\windows\\system32\\drivers\\etc\\hosts";

        private static string[] HOSTS = {
            "adclick.g.doubleclick.net",
            "pubads.g.doubleclick.net",
            "securepubads.g.doubleclick.net",
            "www.googletagservices.com",
            "gads.pubmatic.com",
            "ads.pubmatic.com",
            "pagead2.googlesyndication.com",
            "googlesyndication.com"
        };
        private static string _blockedString;
        private static string _hostsBlock;

        public static void Block()
        {
            try
            {
                SetBlockString();

                if (!IsHostsBlocked())
                {
                    if (!string.IsNullOrEmpty(_hostsBlock))
                    {
                        string hosts = File.ReadAllText(_hostsFile);
                        hosts = hosts.Replace(_hostsBlock, _blockedString);
                        File.WriteAllText(_hostsFile, hosts);
                    }
                    else
                        File.AppendAllText(_hostsFile, _blockedString);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while trying to block the hosts file.\r\n\r\n{e}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Unblock()
        {
            try
            {
                SetBlockString();

                string file = File.ReadAllText(_hostsFile);

                if (IsHostsBlocked())
                {
                    File.WriteAllText(_hostsFile, file.Replace(GetHostsBlock(file), string.Empty));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while trying to unblock the hosts file.\r\n\r\n{e}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsHostsBlocked()
        {
            try
            {
                SetBlockString();

                string file = File.ReadAllText(_hostsFile);
                _hostsBlock = GetHostsBlock(file);
                if (_hostsBlock == null) return false;
                return _hostsBlock.Equals(_blockedString, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets the old block of hosts to replace with updated list
        /// </summary>
        private static string GetHostsBlock(string hosts)
        {
            int start = hosts.IndexOf("# Swervify", StringComparison.Ordinal);
            int end = hosts.IndexOf("# End Swervify", StringComparison.Ordinal);

            if (start == -1 || end == -1)
                return null;
            return hosts.Substring(start, end - start +14);
        }

        private static void SetBlockString()
        {
            if (string.IsNullOrEmpty(_blockedString))
                _blockedString = ToBlockedString();
        }

        private static string ToBlockedString()
        {
            string result = "# Swervify\r\n";
            foreach (string host in HOSTS)
                result += $"0.0.0.0 {host}\r\n";

            result += "# End Swervify";
            return result;
        }
    }
}
