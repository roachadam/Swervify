using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Swervify.Web
{
    class HttpClient
    {
        private string _userAgent;
        private CookieContainer _cookieContainer;
        private NameValueCollection _headers;

        public HttpClient()
        {
            _userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
            _cookieContainer = new CookieContainer();
            _headers = new NameValueCollection();
        }

        public void SetHeader(string name, string value)
        {
            if(_headers[name] == null)
                _headers.Add(name, value);
            else
            {
                _headers[name] = value;
            }
        }

        public string Get(string url)
        {
            for (int i = 0; i < 3; i++)
            {
                string get = DoGet(url);
                if (get != null)
                    return get;
                Thread.Sleep(1000);
            }
            return null;
        }

        private string DoGet(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.CookieContainer = _cookieContainer;
                req.Headers.Add(_headers);
                req.Method = "GET";
                req.Timeout = 12000;
                req.ReadWriteTimeout = 12000;
                req.ServicePoint.MaxIdleTime = 12000;
                req.UserAgent = _userAgent;
                req.Accept = "*/*";
                //req.Proxy = null;
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                req.KeepAlive = true;

                using (var response = req.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
