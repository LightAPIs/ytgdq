using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
namespace WindowsFormsApplication2
{
    public partial class SendTextStatic : Form
    {
        private Point MainPos;
        private Form1 frm;
        public SendTextStatic(Point pos, Form1 frm1)
        {
            MainPos = pos;
            frm = frm1;
            InitializeComponent();
        }

        private void SendTextStatic_Load(object sender, EventArgs e)
        {
            this.Location = new Point(MainPos.X - this.Width, MainPos.Y);
            //MessageBox.Show(MainPos + "\n" + this.Width);
            FillData();
            frm.ShowFlowText("发文已开启...");
        }

        //填充数据
        private void FillData()
        {
            tbxTitle.Text = NewSendText.标题;
            lblTextSources.Text = 文章来源;
            lblTextStyle.Text = 文章类型;
            lblSendCounted.Text = NewSendText.已发字数.ToString();//已发字数
            lblSendPCounted.Text = NewSendText.已发段数.ToString();
            tbxSendC.Text = NewSendText.字数.ToString();
            lblTotalCount.Text = NewSendText.文章全文.Length.ToString();
            lblMarkCount.Text = NewSendText.标记.ToString();//当前标记
            tbxNowStartCount.Text = Glob.CurSegmentNum.ToString(); // 当前段号
            lblNowIni.Text = NewSendText.SentId > 0 ? NewSendText.SentId.ToString() : "无";
            
            if (NewSendText.是否周期)
            {
                tbxSendTime.Text = NewSendText.周期.ToString();
            }
            else {
                tbxSendTime.Text = "-";
                lblNowTime.Text = "无周期手动";
                btnCancelTime.Text = "开";
            }

            if (NewSendText.是否自动)
            {
                this.checkBox1.Checked = true;
            }
            else
            {
                this.checkBox1.Checked = false;
            }
        }

        private string 文章来源 {
            get {
                int i = NewSendText.文章来源;
                switch (i) {
                    case 0: return "内置文章";
                    case 1: return "自定义文章";
                    case 2: return "来自剪贴板";
                    case 3: return "保存的文章";
                    case 4: return "保存的发文配置";
                    default: return "未知来源";
                }
            }
        }

        private string 文章类型 {
            get {
                string str = "";
                if (NewSendText.类型 == "单字")
                {
                    if (NewSendText.是否乱序) str = "单字/乱序";
                    else str = "单字/顺序";
                }
                else {
                    str = NewSendText.类型;
                }
                return str;
            }
        }

        private void lblMarkCount_TextChanged(object sender, EventArgs e)
        {
            if (NewSendText.类型 == "单字")
            {
                if (NewSendText.是否乱序)
                {
                    if (NewSendText.乱序全段不重复)
                        lblLeastCount.Text = NewSendText.发文全文.Length.ToString();
                    else
                        lblLeastCount.Text = "乱序无限";
                }
                else {
                    int total = int.Parse(lblTotalCount.Text);
                    int now = int.Parse(lblMarkCount.Text);
                    lblLeastCount.Text = (total - now).ToString();
                }
            }
            else if (NewSendText.类型 == "词组")
            {
                lblLeastCount.Text = "乱序无限";
            }
            else
            {
                int total = int.Parse(lblTotalCount.Text);
                int now = int.Parse(lblMarkCount.Text);
                lblLeastCount.Text = (total - now).ToString();
            }
        }

        //停止发文
        private void btnStop_Click(object sender, EventArgs e)
        {
            frm.StopSendFun();
        }

        #region 修改字段
        private void btnCancelTime_TextChanged(object sender, EventArgs e)
        {
            string text = (sender as Button).Text;
            if (text == "开")
                toolTip1.SetToolTip(btnCancelTime,"打开周期发文");
            else
                toolTip1.SetToolTip(btnCancelTime, "关闭周期发文");
        }
        //文章标题
        private void btnFixNowTitle_Click(object sender, EventArgs e)
        {
            string now = (sender as Button).Text;
            if (now == "修")
            {
                this.tbxTitle.ReadOnly = false;
                this.tbxTitle.BackColor = Color.White;
                this.tbxTitle.Focus();
                this.btnFixNowTitle.Text = "定";
            }
            else
            {
                string title = this.tbxTitle.Text.Trim();
                if (title.Length > 0)
                {
                    this.tbxTitle.ReadOnly = true;
                    this.tbxTitle.BackColor = Color.DarkGray;
                    this.btnFixNowTitle.Text = "修";
                    NewSendText.标题 = title;
                    frm.ResetArticleTitle(title);
                }
            }
        }

        /// <summary>
        /// 周期修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendTime_Click(object sender, EventArgs e)
        {
            string now = (sender as Button).Text;
            if (now == "修")
            {
                this.tbxSendTime.ReadOnly = false;
                this.tbxSendTime.BackColor = Color.White;
                this.tbxSendTime.Focus();
                (sender as Button).Text = "定";
            }
            else
            {
                try
                {
                    int get = int.Parse(this.tbxSendTime.Text);
                    if (get >= 10 && get <= 1800)
                    {
                        this.tbxSendTime.ReadOnly = true;
                        this.tbxSendTime.BackColor = Color.DarkGray;
                        (sender as Button).Text = "修";
                        NewSendText.周期 = get;
                        NewSendText.周期计数 = get;
                    }
                    else
                    {
                        MessageBox.Show("定义超出总范围，请重设！");
                    }
                }
                catch { }
            }
        }

        private void btnCancelTime_Click(object sender, EventArgs e)
        {
            if (NewSendText.是否周期)
            {
                NewSendText.是否周期 = false;
                lblNowTime.Text = "无周期手动";
                btnCancelTime.Text = "开";
            }
            else {
                NewSendText.是否周期 = true;
                tbxSendTime.Text = NewSendText.周期.ToString();
                lblNowTime.Text = NewSendText.周期计数.ToString();
                btnCancelTime.Text = "停";
                frm.SendNextFun();
            }
        }

        /// <summary>
        /// 当前段号修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePreCout_Click(object sender, EventArgs e)
        {
            string now = (sender as Button).Text;
            if (now == "修")
            {
                this.tbxNowStartCount.ReadOnly = false;
                this.tbxNowStartCount.BackColor = Color.White;
                this.tbxNowStartCount.Focus();
                (sender as Button).Text = "定";
            }
            else
            {
                int get = int.Parse(this.tbxNowStartCount.Text);
                if (get > 0)
                {
                    this.tbxNowStartCount.ReadOnly = true;
                    this.tbxNowStartCount.BackColor = Color.DarkGray;
                    (sender as Button).Text = "修";
                    Glob.CurSegmentNum = get;
                    frm.lblDuan.Text = "第" + Glob.CurSegmentNum.ToString() + "段";
                }
                else
                {
                    MessageBox.Show("定义超出总范围，请重设！");
                }
            }
        }

        private void lblNowTime_Click(object sender, EventArgs e)
        {
            NewSendText.周期计数--;
        }
        #endregion

        #region 保存配置
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            frm.SaveSendFun();
        }

        public void UpdateFromSave()
        {
            lblNowIni.Text = NewSendText.SentId.ToString();
            lblTextSources.Text = 文章来源;
        }
        #endregion

        #region 自动
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked && !NewSendText.是否自动)
            {
                NewSendText.是否自动 = true;
            }
            else if (!this.checkBox1.Checked && NewSendText.是否自动)
            {
                NewSendText.是否自动 = false;
            }
        }
        #endregion
    }
}
