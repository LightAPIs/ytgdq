using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace WindowsFormsApplication2
{
    public class YTWebRequest
    {
        private readonly string webUrl;

        public YTWebRequest(string web_url)
        {
            this.webUrl = web_url;
        }

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <returns></returns>
        public string Request()
        {
            string result = "";
            try
            {
                Uri uri = new Uri(this.webUrl);
                WebRequest req = WebRequest.Create(uri);
                WebResponse resp = req.GetResponse();
                Stream receviceStream = resp.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, Encoding.UTF8);
                result = readerOfStream.ReadToEnd();

                readerOfStream.Close();
                receviceStream.Close();
                resp.Close();
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }
    }
}
