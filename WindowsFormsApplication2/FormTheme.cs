using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication2
{
    public partial class FormTheme : Form
    {
        private readonly Form1 frm;
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
            {
                // 图片背景
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
            ColorDialog cd = new ColorDialog();
            cd.Color = Theme.ThemeBG;
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.lblPicShow.BackColor = cd.Color;
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
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.lblBGPath.Text = openFileDialog1.FileName;
                ReView();
            }
        }

        /// <summary>
        /// 主题背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = Theme.ThemeColorBG;
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.lblThemeBGShow.BackColor = cd.Color;
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
            ColorDialog cd = new ColorDialog();
            cd.Color = Theme.ThemeColorFC;
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.lblThemeFCShow.BackColor = cd.Color;
                ReView();
            }
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            _Ini ini = new _Ini("config.ini");
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
            this.Close();
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
                path = Theme.ThemeBackBmp;
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
        /// 预览功能
        /// </summary>
        private void ReView()
        {
            string path;
            if (this.SwitchB1.Checked)
            {
                path = this.lblBGPath.Text;
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
        #endregion
    }
}
