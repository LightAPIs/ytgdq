using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace WindowsFormsApplication2
{
    public partial class FormTheme : Form
    {
        private readonly Form1 frm;
        private Font fo1, fo2;
        /// <summary>
        /// 主题资源目录名称
        /// </summary>
        public static string ThemeFolderName = Path.Combine(Application.StartupPath, "Theme");

        public FormTheme(Form1 frm1)
        {
            frm = frm1;
            InitializeComponent();
        }

        private void FormTheme_Load(object sender, EventArgs e)
        {
            //初始化
            this.SwitchB1.Checked = Theme.IsBackBmp;
            this.SwitchB1.Invalidate();
            this.lblSelectPIC.Enabled = Theme.IsBackBmp;
            this.newButton4.Enabled = Theme.IsBackBmp;
            if (Theme.IsBackBmp)
            { // 图片背景
                if (Theme.ThemeBackBmp.Length == 0)
                {
                    this.lblBGPath.Text = "程序默认";
                }
                else
                {
                    this.lblBGPath.Text = Theme.ThemeBackBmp;
                }
            }
            else
            {
                this.lblBGPath.Text = "纯色";
            }

            this.lblSelectBG.Enabled = !Theme.IsBackBmp;
            this.newButton3.Enabled = !Theme.IsBackBmp;
            this.lblPicShow.BackColor = Theme.ThemeBG; // 纯色背景

            this.lblThemeBGShow.BackColor = Theme.ThemeColorBG; // 背景色
            this.lblThemeFCShow.BackColor = Theme.ThemeColorFC; // 前景色

            // 载入字体
            fo1 = Theme.Font_1;
            fo2 = Theme.Font_2;
            SetFont1();
            SetFont2();

            this.R1BackLabel.BackColor = Theme.R1Back;
            this.R2BackLabel.BackColor = Theme.R2Back;

            SetR1Color(Theme.R1Color);
            SetR2Color(Theme.R2Color);

            this.RightBGColorLabel.BackColor = Theme.RightBGColor;
            this.FalseBGColorLabel.BackColor = Theme.FalseBGColor;

            this.BackChangeColorLabel.BackColor = Theme.BackChangeColor;
            this.TimeLongColorLabel.BackColor = Theme.TimeLongColor;

            this.Words0ColorLabel.BackColor = Theme.Words0Color;
            this.Words1ColorLabel.BackColor = Theme.Words1Color;
            this.Words2ColorLabel.BackColor = Theme.Words2Color;
            this.Words3ColorLabel.BackColor = Theme.Words3Color;

            this.TestMarkColorLabel.BackColor = Theme.TestMarkColor;
        }

        private void SetR1Color(Color c)
        {
            this.R1ColorLabel.BackColor = c;
            this.R1BackLabel.ForeColor = c;
            this.RightBGColorLabel.ForeColor = c;
            this.FalseBGColorLabel.ForeColor = c;
            this.TimeLongColorLabel.ForeColor = c;
            this.TestMarkColorLabel.ForeColor = c;
        }

        private void SetR2Color(Color c)
        {
            this.R2ColorLabel.BackColor = c;
            this.R2BackLabel.ForeColor = c;
        }

        private void SetFont1()
        {
            FontConverter fc = new FontConverter();
            this.Font1Label.Text = fc.ConvertToInvariantString(fo1);
            this.toolTip1.SetToolTip(this.Font1Label, this.Font1Label.Text);

            Font exFo1 = new Font(fo1.FontFamily, 9f, fo1.Style);
            this.R1BackLabel.Font = exFo1;
            this.RightBGColorLabel.Font = exFo1;
            this.FalseBGColorLabel.Font = exFo1;
            this.TimeLongColorLabel.Font = exFo1;
            this.TestMarkColorLabel.Font = exFo1;
        }

        private void SetFont2()
        {
            FontConverter fc = new FontConverter();
            this.Font2Label.Text = fc.ConvertToInvariantString(fo2);
            this.toolTip1.SetToolTip(this.Font2Label, this.Font2Label.Text);

            Font exFo2 = new Font(fo2.FontFamily, 9f, fo2.Style);
            this.R2BackLabel.Font = exFo2;
        }

        #region 关闭按钮
        private void lblcls_MouseEnter(object sender, EventArgs e)
        {
            this.lblcls.BackColor = Color.FromArgb(199, 12, 52);
        }

        private void lblcls_MouseLeave(object sender, EventArgs e)
        {
            this.lblcls.BackColor = Color.Gray;
        }

        private void lblcls_MouseDown(object sender, MouseEventArgs e)
        {
            this.lblcls.TextAlign = ContentAlignment.BottomCenter;
        }

        private void lblcls_MouseUp(object sender, MouseEventArgs e)
        {
            this.lblcls.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void lblcls_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(System.IntPtr ptr, int wMsg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void FormTheme_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        #region 主题设置
        /// <summary>
        /// 更改背景模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchB1_CChange(object sender, EventArgs e)
        {
            if (this.SwitchB1.Checked)
            {
                this.lblSelectPIC.Enabled = true;
                this.newButton4.Enabled = true;
                this.lblBGPath.Text = "程序默认";
                this.lblSelectBG.Enabled = false;
                this.newButton3.Enabled = false;
            }
            else
            {
                this.lblSelectPIC.Enabled = false;
                this.newButton4.Enabled = false;
                this.lblBGPath.Text = "纯色";
                this.lblSelectBG.Enabled = true;
                this.newButton3.Enabled = true;
            }

            ReView();
        }

        /// <summary>
        /// 选择纯色背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSelectBG_Click(object sender, EventArgs e)
        {

            this.colorDialog1.Color = Theme.ThemeBG;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.lblPicShow.BackColor = this.colorDialog1.Color;
                ReView();
            }
        }

        /// <summary>
        /// 选择图片背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "图片|*.jpg;*.jpeg;*.bmp;*.png;*.gif",
                FileName = Application.StartupPath
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string sourceFileName = openFileDialog1.FileName;
                if (!string.IsNullOrEmpty(sourceFileName))
                {
                    if (!Directory.Exists(ThemeFolderName))
                    {
                        Directory.CreateDirectory(ThemeFolderName);
                    }

                    string filename = Path.GetFileName(sourceFileName);
                    string notExtFilename = Path.GetFileNameWithoutExtension(sourceFileName);
                    string extName = Path.GetExtension(sourceFileName); //? 文件后缀是带 "." 的
                    string themeFilename = Path.Combine(ThemeFolderName, filename);
                    int fileIndex = 0;

                    while (File.Exists(themeFilename))
                    {
                        fileIndex++;
                        themeFilename = Path.Combine(ThemeFolderName, string.Format("{0}_{1}{2}", notExtFilename, fileIndex.ToString(), extName));
                    }
                    File.Copy(sourceFileName, themeFilename);

                    this.lblBGPath.Text = Path.GetFileName(themeFilename);
                    ReView();
                }
            }
        }

        /// <summary>
        /// 主题背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSelectColor_Click(object sender, EventArgs e)
        {

            this.colorDialog1.Color = Theme.ThemeColorBG;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.lblThemeBGShow.BackColor = this.colorDialog1.Color;
                ReView();
            }
        }

        /// <summary>
        /// 主题前景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSelectFCColor_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.ThemeColorFC;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.lblThemeFCShow.BackColor = this.colorDialog1.Color;
                ReView();
            }
        }

        private void newButton5_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.R1Back;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.R1BackLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton6_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.R1Color;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                SetR1Color(this.colorDialog1.Color);
            }
        }

        private void newButton7_Click(object sender, EventArgs e)
        {
            this.fontDialog1.ShowEffects = false;
            this.fontDialog1.Font = fo1;
            if (this.fontDialog1.ShowDialog(this) == DialogResult.OK)
            {
                fo1 = fontDialog1.Font;
                SetFont1();
            }
        }

        private void newButton8_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.R2Back;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.R2BackLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton9_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.R2Color;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                SetR2Color(this.colorDialog1.Color);
            }
        }

        private void newButton10_Click(object sender, EventArgs e)
        {
            this.fontDialog1.ShowEffects = false;
            this.fontDialog1.Font = fo2;
            if (this.fontDialog1.ShowDialog(this) == DialogResult.OK)
            {
                fo2 = fontDialog1.Font;
                SetFont2();
            }
        }

        private void newButton11_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.RightBGColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.RightBGColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton12_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.FalseBGColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.FalseBGColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton13_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.BackChangeColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.BackChangeColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton14_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.TimeLongColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.TimeLongColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton15_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.Words0Color;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.Words0ColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton16_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.Words1Color;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.Words1ColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton17_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.Words2Color;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.Words2ColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton18_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.Words3Color;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.Words3ColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        private void newButton19_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Theme.TestMarkColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.TestMarkColorLabel.BackColor = this.colorDialog1.Color;
            }
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            Ini ini = new Ini("default.theme");

            //* 主题
            ini.IniWriteValue("主题", "是否应用主题背景", this.SwitchB1.Checked.ToString());
            ini.IniWriteValue("主题", "背景路径", this.lblBGPath.Text);
            ini.IniWriteValue("主题", "纯色", this.lblPicShow.BackColor.ToArgb().ToString());
            ini.IniWriteValue("主题", "主题颜色", this.lblThemeBGShow.BackColor.ToArgb().ToString());
            ini.IniWriteValue("主题", "字体颜色", this.lblThemeFCShow.BackColor.ToArgb().ToString());
            Theme.IsBackBmp = this.SwitchB1.Checked;
            Theme.ThemeBackBmp = this.lblBGPath.Text;
            Theme.ThemeBG = this.lblPicShow.BackColor;
            Theme.ThemeColorBG = this.lblThemeBGShow.BackColor;
            Theme.ThemeColorFC = this.lblThemeFCShow.BackColor;

            //* 颜色设置
            ini.IniWriteValue("外观", "对照区颜色", this.R1BackLabel.BackColor.ToArgb().ToString());
            Theme.R1Back = this.R1BackLabel.BackColor;
            frm.richTextBox1.BackColor = Theme.R1Back;
            ini.IniWriteValue("外观", "跟打区颜色", this.R2BackLabel.BackColor.ToArgb().ToString());
            Theme.R2Back = this.R2BackLabel.BackColor;
            frm.textBoxEx1.BackColor = Theme.R2Back;
            ini.IniWriteValue("外观", "对照区文字色", this.R1ColorLabel.BackColor.ToArgb().ToString());
            Theme.R1Color = this.R1ColorLabel.BackColor;
            frm.richTextBox1.ForeColor = Theme.R1Color;
            ini.IniWriteValue("外观", "跟打区文字色", this.R2ColorLabel.BackColor.ToArgb().ToString());
            Theme.R2Color = this.R2ColorLabel.BackColor;
            frm.textBoxEx1.ForeColor = Theme.R2Color;

            ini.IniWriteValue("外观", "打对颜色", this.RightBGColorLabel.BackColor.ToArgb().ToString());
            Theme.RightBGColor = this.RightBGColorLabel.BackColor;
            ini.IniWriteValue("外观", "打错颜色", this.FalseBGColorLabel.BackColor.ToArgb().ToString());
            Theme.FalseBGColor = this.FalseBGColorLabel.BackColor;

            ini.IniWriteValue("外观", "回改颜色", this.BackChangeColorLabel.BackColor.ToArgb().ToString());
            Theme.BackChangeColor = this.BackChangeColorLabel.BackColor;
            ini.IniWriteValue("外观", "用时背景色", this.TimeLongColorLabel.BackColor.ToArgb().ToString());
            Theme.TimeLongColor = this.TimeLongColorLabel.BackColor;

            ini.IniWriteValue("外观", "词组0重色", this.Words0ColorLabel.BackColor.ToArgb().ToString());
            Theme.Words0Color = this.Words0ColorLabel.BackColor;
            ini.IniWriteValue("外观", "词组1重色", this.Words1ColorLabel.BackColor.ToArgb().ToString());
            Theme.Words1Color = this.Words1ColorLabel.BackColor;
            ini.IniWriteValue("外观", "词组2重色", this.Words2ColorLabel.BackColor.ToArgb().ToString());
            Theme.Words2Color = this.Words2ColorLabel.BackColor;
            ini.IniWriteValue("外观", "词组3重色", this.Words3ColorLabel.BackColor.ToArgb().ToString());
            Theme.Words3Color = this.Words3ColorLabel.BackColor;
            ini.IniWriteValue("外观", "测速点颜色", this.TestMarkColorLabel.BackColor.ToArgb().ToString());
            Theme.TestMarkColor = this.TestMarkColorLabel.BackColor;

            //* 字体设置
            FontConverter fc = new FontConverter();
            ini.IniWriteValue("外观", "对照区字体", fc.ConvertToInvariantString(fo1));
            ini.IniWriteValue("外观", "跟打区字体", fc.ConvertToInvariantString(fo2));
            Theme.Font_1 = fo1;
            Theme.Font_2 = fo2;
            frm.richTextBox1.Font = Theme.Font_1;
            frm.textBoxEx1.Font = Theme.Font_2;

            this.Close();
        }

        /// <summary>
        /// 另存为...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton35_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "主题文件|*.theme|所有文件|*.*",
                FilterIndex = 0,
                RestoreDirectory = true,
                Title = "选择保存主题文件路径",
                OverwritePrompt = true,
                FileName = "yt-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".theme"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Ini ini = new Ini(saveFileDialog.FileName, true);
                //* 主题
                ini.IniWriteValue("主题", "是否应用主题背景", this.SwitchB1.Checked.ToString());
                ini.IniWriteValue("主题", "背景路径", this.lblBGPath.Text);
                ini.IniWriteValue("主题", "纯色", this.lblPicShow.BackColor.ToArgb().ToString());
                ini.IniWriteValue("主题", "主题颜色", this.lblThemeBGShow.BackColor.ToArgb().ToString());
                ini.IniWriteValue("主题", "字体颜色", this.lblThemeFCShow.BackColor.ToArgb().ToString());
                //* 颜色设置
                ini.IniWriteValue("外观", "对照区颜色", this.R1BackLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "跟打区颜色", this.R2BackLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "对照区文字色", this.R1ColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "跟打区文字色", this.R2ColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "打对颜色", this.RightBGColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "打错颜色", this.FalseBGColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "回改颜色", this.BackChangeColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "用时背景色", this.TimeLongColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "词组0重色", this.Words0ColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "词组1重色", this.Words1ColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "词组2重色", this.Words2ColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "词组3重色", this.Words3ColorLabel.BackColor.ToArgb().ToString());
                ini.IniWriteValue("外观", "测速点颜色", this.TestMarkColorLabel.BackColor.ToArgb().ToString());
                //* 字体设置
                FontConverter fc = new FontConverter();
                ini.IniWriteValue("外观", "对照区字体", fc.ConvertToInvariantString(fo1));
                ini.IniWriteValue("外观", "跟打区字体", fc.ConvertToInvariantString(fo2));

                MessageBox.Show(this, "主题文件保存成功！", "保存提示");
            }
        }

        /// <summary>
        /// 载入...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton36_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "主题文件|*.theme|所有文件|*.*",
                FilterIndex = 0,
                RestoreDirectory = true,
                Title = "载入主题文件",
                CheckFileExists = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Ini themeConf = new Ini(ofd.FileName, true);

                //* 主题
                bool isBackBmp = bool.Parse(themeConf.IniReadValue("主题", "是否应用主题背景", "False"));
                string backBmp = themeConf.IniReadValue("主题", "背景路径", "程序默认");

                this.SwitchB1.Checked = isBackBmp;
                this.SwitchB1.Invalidate();
                this.lblSelectPIC.Enabled = isBackBmp;
                this.newButton4.Enabled = isBackBmp;
                if (isBackBmp)
                { // 图片背景
                    if (backBmp.Length == 0)
                    {
                        this.lblBGPath.Text = "程序默认";
                    }
                    else
                    {
                        this.lblBGPath.Text = backBmp;
                    }
                }
                else
                {
                    this.lblBGPath.Text = "纯色";
                }

                this.lblSelectBG.Enabled = !isBackBmp;
                this.newButton3.Enabled = !isBackBmp;
                this.lblPicShow.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("主题", "纯色", "-13089719"))); // #384449

                this.lblThemeBGShow.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("主题", "主题颜色", "-13089719"))); // #384449
                this.lblThemeFCShow.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("主题", "字体颜色", "-1"))); // #FFFFFF

                // 载入字体
                FontConverter fc = new FontConverter();
                fo1 = (Font)fc.ConvertFromString(themeConf.IniReadValue("外观", "对照区字体", "宋体, 21.75pt"));
                fo2 = (Font)fc.ConvertFromString(themeConf.IniReadValue("外观", "跟打区字体", "宋体, 12pt"));
                SetFont1();
                SetFont2();

                //* 颜色设置
                this.R1BackLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "对照区颜色", "-722948"))); // #F4F7FC
                this.R2BackLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "跟打区颜色", "-722948"))); // #F4F7FC
                SetR1Color(Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "对照区文字色", "-16777216"))));
                SetR2Color(Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "跟打区文字色", "-16777216"))));

                this.RightBGColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "打对颜色", "-8355712"))); // #808080
                this.FalseBGColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "打错颜色", "-38294"))); // #FF6A6A

                this.BackChangeColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "回改颜色", "-5374161"))); // #ADFF2F
                this.TimeLongColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "用时背景色", "-6632142"))); // #9ACD32

                this.Words0ColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "词组0重色", "-16776961"))); // #0000FF
                this.Words1ColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "词组1重色", "-65536"))); // #FF0000
                this.Words2ColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "词组2重色", "-8388480"))); // #800080
                this.Words3ColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "词组3重色", "-60269"))); // #FF1493

                this.TestMarkColorLabel.BackColor = Color.FromArgb(int.Parse(themeConf.IniReadValue("外观", "测速点颜色", "-2894893"))); // #D3D3D3

                ReView();
            }
        }

        /// <summary>
        /// 还原功能
        /// - 关闭时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTheme_FormClosed(object sender, FormClosedEventArgs e)
        {
            string path;
            if (Theme.IsBackBmp)
            {
                path = Path.Combine(FormTheme.ThemeFolderName, Theme.ThemeBackBmp);
                if (path != "程序默认" && !System.IO.File.Exists(path))
                {
                    path = "程序默认";
                }
            }
            else
            {
                path = "纯色";
            }

            frm.LoadTheme(path, Theme.ThemeColorBG, Theme.ThemeColorFC, Theme.ThemeBG);
        }

        /// <summary>
        /// 主题预览功能
        /// </summary>
        private void ReView()
        {
            string path;
            if (this.SwitchB1.Checked)
            {
                path = Path.Combine(FormTheme.ThemeFolderName, this.lblBGPath.Text);
                if (path != "程序默认" && !System.IO.File.Exists(path))
                {
                    path = "程序默认";
                }
            }
            else
            {
                path = "纯色";
            }

            frm.LoadTheme(path, this.lblThemeBGShow.BackColor, this.lblThemeFCShow.BackColor, this.lblPicShow.BackColor);
        }
        #endregion

        #region 默认按钮
        // 默认背景图片
        private void newButton4_Click(object sender, EventArgs e)
        {
            if (this.SwitchB1.Checked)
            {
                this.lblBGPath.Text = "程序默认";
                ReView();
            }
        }

        // 默认纯色
        private void newButton3_Click(object sender, EventArgs e)
        {
            if (!SwitchB1.Checked)
            {
                this.lblBGPath.Text = "纯色";
                this.lblPicShow.BackColor = Color.FromArgb(56, 68, 73);
                ReView();
            }
        }

        // 默认背景色
        private void newButton2_Click(object sender, EventArgs e)
        {
            this.lblThemeBGShow.BackColor = Color.FromArgb(56, 68, 73);
            ReView();
        }

        // 默认前景色
        private void newButton1_Click(object sender, EventArgs e)
        {
            this.lblThemeFCShow.BackColor = Color.White;
            ReView();
        }

        private void newButton20_Click(object sender, EventArgs e)
        {
            this.R1BackLabel.BackColor = Color.FromArgb(244, 247, 252);
        }

        private void newButton21_Click(object sender, EventArgs e)
        {
            SetR1Color(Color.Black);
        }

        private void newButton22_Click(object sender, EventArgs e)
        {
            FontConverter fc = new FontConverter();
            fo1 = (Font)fc.ConvertFromString("宋体, 21.75pt");
            SetFont1();
        }

        private void newButton23_Click(object sender, EventArgs e)
        {
            this.R2BackLabel.BackColor = Color.FromArgb(244, 247, 252);
        }

        private void newButton24_Click(object sender, EventArgs e)
        {
            SetR2Color(Color.Black);
        }

        private void newButton25_Click(object sender, EventArgs e)
        {
            FontConverter fc = new FontConverter();
            fo2 = (Font)fc.ConvertFromString("宋体, 12pt");
        }

        private void newButton26_Click(object sender, EventArgs e)
        {
            this.RightBGColorLabel.BackColor = Color.Gray;
        }

        private void newButton27_Click(object sender, EventArgs e)
        {
            this.FalseBGColorLabel.BackColor = Color.FromArgb(255, 106, 106);
        }

        private void newButton28_Click(object sender, EventArgs e)
        {
            this.BackChangeColorLabel.BackColor = Color.GreenYellow;
        }

        private void newButton29_Click(object sender, EventArgs e)
        {
            this.TimeLongColorLabel.BackColor = Color.YellowGreen;
        }

        private void newButton30_Click(object sender, EventArgs e)
        {
            this.Words0ColorLabel.BackColor = Color.Blue;
        }

        private void newButton31_Click(object sender, EventArgs e)
        {
            this.Words1ColorLabel.BackColor = Color.Red;
        }

        private void newButton32_Click(object sender, EventArgs e)
        {
            this.Words2ColorLabel.BackColor = Color.Purple;
        }

        private void newButton33_Click(object sender, EventArgs e)
        {
            this.Words3ColorLabel.BackColor = Color.DeepPink;
        }

        private void newButton34_Click(object sender, EventArgs e)
        {
            this.TestMarkColorLabel.BackColor = Color.LightGray;
        }
        #endregion
    }
}
