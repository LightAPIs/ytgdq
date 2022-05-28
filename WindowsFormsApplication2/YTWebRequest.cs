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
                //!? 由于 .NET 4.0 不支持 TLS 1.2，且 SecurityProtocolType 中没有 Tls12/Tls11 等枚举值，但若电脑上安装有 .NET 4.5 及以上框架时，还是可以通过手动赋值提供以支持
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)768 | SecurityProtocolType.Tls;
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

        static public List<string> SourceUrl = new List<string> { "https://fastly.jsdelivr.net/gh/{user}/{repo}@{branch}/", "https://gcore.jsdelivr.net/gh/{user}/{repo}@{branch}/", "https://testingcf.jsdelivr.net/gh/{user}/{repo}@{branch}/", "https://cdn.jsdelivr.net/gh/{user}/{repo}@{branch}/", "https://raw.fastgit.org/{user}/{repo}/{branch}/", "https://cdn.staticaly.com/gh/{user}/{repo}/{branch}/" };

        static public string GetSourceUrl(string user, string repo, string branch)
        {
            int index = (int)Glob.ArticleMirror;
            if (index < 0 || index >= SourceUrl.Count)
            {
                index = 0;
            }

            return SourceUrl[index].Replace("{user}", user).Replace("{repo}", repo).Replace("{branch}", branch);
        }
    }
}
