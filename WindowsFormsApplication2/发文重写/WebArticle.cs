using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2.发文重写
{
    internal class WebArticle
    {
        public List<string> dir { get; set; }
        public List<TxtFile> txt { get; set; }

        public WebArticle()
        {
            this.dir = new List<string>();
            this.txt = new List<TxtFile>();
        }
    }

    internal class TxtFile
    {
        public string name { get; set; }
        public string size { get; set; }
        public int count { get; set; }
    }
}
