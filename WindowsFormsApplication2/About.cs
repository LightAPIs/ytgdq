using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace WindowsFormsApplication2
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.Text = string.Format("关于 {0}", AssemblyTitle);
            this.labelVersion.Text = "v" + Glob.VerInstance;
        }

        private void Jump(string str)
        {
            try
            {
                Process.Start(str);
            }
            catch (Exception)
            {

            }
        }

        private DateTime Start = new DateTime(2012, 3, 20);
        private void Start_Load(object sender, EventArgs e)
        {
            var ts = DateTime.Now - Start;
            lblInfo.Text = string.Format("添雨跟打器从{0}发布至今已过去{1}天", Start.ToShortDateString(),
            ts.TotalDays.ToString("0"));
        }

        #region 程序集特性访问器
        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        #endregion

        private void LinkLabel1Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Jump("https://github.com/taliove/tygdq");
        }

        private void LinkLabel2Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Jump("https://github.com/LightAPIs/tygdq");
        }

        private void OKClicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
