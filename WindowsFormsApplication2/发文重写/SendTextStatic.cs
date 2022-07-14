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

            //* 主题颜色
            LoadTheme(Theme.ThemeColorBG, Theme.ThemeColorFC, Theme.SecondBG, Theme.SecondFC);

            FillData();
            frm.ShowFlowText("发文已开启...");
        }

        /// <summary>
        /// 载入主题颜色
        /// </summary>
        public void LoadTheme(Color BG, Color FC, Color SBG, Color SFC)
        {
            this.BackColor = Theme.GetTranColor(BG, 50);
            this.ForeColor = FC;
            this.gbstatic.ForeColor = FC;
            this.newButton1.BackColor = SBG;
            this.newButton1.ForeColor = SFC;
            this.newButton1.默认背景色 = SBG;
            this.newButton1.进入背景色 = Theme.GetReverseColor(SBG);
            this.panel1.BackColor = Theme.GetTranColor(BG, -50);
            this.btnFixNowTitle.BackColor = SBG;
            this.btnFixNowTitle.ForeColor = SFC;
            this.btnSendTime.BackColor = SBG;
            this.btnSendTime.ForeColor = SFC;
            this.AutoNumberButton.BackColor = SBG;
            this.AutoNumberButton.ForeColor = SFC;
            this.btnChangePreCout.BackColor = SBG;
            this.btnChangePreCout.ForeColor = SFC;
            this.btnSave.BackColor = SBG;
            this.btnSave.ForeColor = SFC;
            this.btnStop.BackColor = SBG;
            this.btnStop.ForeColor = Theme.GetTranColor(SFC, 50);
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private void FillData()
        {
            tbxTitle.Text = NewSendText.标题;
            lblTextSources.Text = 文章来源;
            lblTextStyle.Text = 文章类型;
            lblTotalCount.Text = NewSendText.文章全文.Length.ToString();
            if (NewSendText.类型 == "词组")
            {
                this.label4.Text = "已发词数";
                this.label6.Text = "总词数";
                this.label7.Text = "剩余词数";
                this.label8.Text = "词数|标记";
                lblTotalCount.Text = NewSendText.词组全文.Count.ToString();
            }
            lblSendCounted.Text = NewSendText.已发字数.ToString();//已发字数
            lblSendPCounted.Text = NewSendText.已发段数.ToString();
            tbxSendC.Text = NewSendText.字数.ToString();
            lblMarkCount.Text = NewSendText.标记.ToString();//当前标记
            tbxNowStartCount.Text = Glob.CurSegmentNum.ToString(); // 当前段号
            lblNowIni.Text = NewSendText.SentId > 0 ? NewSendText.SentId.ToString() : "无";
            
            if (NewSendText.是否周期)
            {
                this.CycleCheckBox.Checked = true;
                this.btnSendTime.Enabled = true;
                tbxSendTime.Text = NewSendText.周期.ToString();
                lblNowTime.Text = NewSendText.周期计数.ToString();
                this.checkBox1.Enabled = false;
            }
            else
            {
                this.CycleCheckBox.Checked = false;
                this.btnSendTime.Enabled = false;
                tbxSendTime.Text = "-";
                lblNowTime.Text = "无";
                this.checkBox1.Enabled = true;
            }

            if (NewSendText.是否自动)
            {
                this.checkBox1.Checked = true;
                this.checkBox2.Enabled = true;
                this.CycleCheckBox.Enabled = false;
            }
            else
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Enabled = false;
                this.CycleCheckBox.Enabled = true;
            }

            if (NewSendText.AutoCondition)
            {
                this.checkBox2.Checked = true;
                if (NewSendText.是否自动)
                {
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    this.comboBox3.Enabled = true;
                    this.AutoNumberButton.Enabled = true;
                }
            }
            else
            {
                this.checkBox2.Checked = false;
            }
        }

        private string 文章来源 {
            get {
                switch (NewSendText.ArticleSource)
                {
                    case NewSendText.ArticleSourceValue.Internal:
                        return "内置文章";
                    case NewSendText.ArticleSourceValue.Local:
                        return "本地文章";
                    case NewSendText.ArticleSourceValue.Clipboard:
                        return "来自剪贴板";
                    case NewSendText.ArticleSourceValue.Web:
                        return "网络文章";
                    case NewSendText.ArticleSourceValue.Stored:
                        return "保存的文章";
                    case NewSendText.ArticleSourceValue.Sent:
                        return "保存的发文";
                    default:
                        return "未知来源";
                }
            }
        }

        private string 文章类型 {
            get {
                string str;
                if (NewSendText.类型 == "单字")
                {
                    if (NewSendText.单字乱序)
                    {
                        str = "单字/乱序";
                    }
                    else
                    {
                        str = "单字/顺序";
                    }
                }
                else if (NewSendText.类型 == "词组")
                {
                    if (NewSendText.词组乱序)
                    {
                        str = "词组/乱序";
                    }
                    else
                    {
                        str = "词组/顺序";
                    }
                }
                else
                {
                    str = NewSendText.类型;
                }

                return str;
            }
        }

        private void lblMarkCount_TextChanged(object sender, EventArgs e)
        {
            if (NewSendText.类型 == "单字")
            {
                if (NewSendText.单字乱序)
                {
                    if (NewSendText.乱序全段不重复)
                    {
                        lblLeastCount.Text = NewSendText.发文全文.Length.ToString();
                    }
                    else
                    {
                        lblLeastCount.Text = "乱序无限";
                    }
                }
                else
                {
                    int total = int.Parse(lblTotalCount.Text);
                    int now = int.Parse(lblMarkCount.Text);
                    lblLeastCount.Text = (total - now).ToString();
                }
            }
            else if (NewSendText.类型 == "词组")
            {
                if (NewSendText.词组乱序)
                {
                    if (NewSendText.乱序全段不重复)
                    {
                        lblLeastCount.Text = NewSendText.词组.Count.ToString();
                    }
                    else
                    {
                        lblLeastCount.Text = "乱序无限";
                    }
                }
                else
                {
                    int total = int.Parse(lblTotalCount.Text);
                    int now = int.Parse(lblMarkCount.Text);
                    lblLeastCount.Text = (total - now).ToString();
                }
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
        /// <summary>
        /// 修改标题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 修改周期
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
                    int value = int.Parse(this.tbxSendTime.Text);
                    if (value >= 10 && value <= 1800)
                    {
                        this.tbxSendTime.ReadOnly = true;
                        this.tbxSendTime.BackColor = Color.DarkGray;
                        (sender as Button).Text = "修";
                        NewSendText.周期 = value;
                        NewSendText.周期计数 = value;
                    }
                    else
                    {
                        MessageBox.Show("定义超出总范围，请重设！");
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 修改自动条件值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoNumberButton_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "修")
            {
                this.AutoNumberTextBox.ReadOnly = false;
                this.AutoNumberTextBox.BackColor = Color.White;
                this.AutoNumberTextBox.Focus();
                (sender as Button).Text = "定";
            }
            else
            {
                try
                {
                    double.TryParse(this.AutoNumberTextBox.Text, out double value);
                    this.AutoNumberTextBox.ReadOnly = true;
                    this.AutoNumberTextBox.BackColor = Color.DarkGray;
                    (sender as Button).Text = "修";
                    NewSendText.AutoNumber = value;
                }
                catch { }
            }
        }

        /// <summary>
        /// 修改当前段号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePreCout_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "修")
            {
                this.tbxNowStartCount.ReadOnly = false;
                this.tbxNowStartCount.BackColor = Color.White;
                this.tbxNowStartCount.Focus();
                (sender as Button).Text = "定";
            }
            else
            {
                int value = int.Parse(this.tbxNowStartCount.Text);
                if (value > 0)
                {
                    this.tbxNowStartCount.ReadOnly = true;
                    this.tbxNowStartCount.BackColor = Color.DarkGray;
                    (sender as Button).Text = "修";
                    Glob.CurSegmentNum = value;
                    frm.lblDuan.Text = "第" + Glob.CurSegmentNum.ToString() + "段";
                }
                else
                {
                    MessageBox.Show("定义超出总范围，请重设！");
                }
            }
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

        #region 功能开关
        private void CycleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                if (!NewSendText.是否周期)
                {
                    NewSendText.是否周期 = true;
                    this.btnSendTime.Enabled = true;
                    this.tbxSendTime.Text = NewSendText.周期.ToString();
                    this.lblNowTime.Text = NewSendText.周期计数.ToString();
                    NewSendText.是否自动 = false;
                    this.checkBox1.Enabled = false;
                    this.checkBox2.Enabled = false;
                    this.AutoNumberButton.Enabled = false;
                    this.AutoNumberButton.Text = "修";
                    this.AutoNumberTextBox.ReadOnly = true;
                    this.AutoNumberTextBox.BackColor = Color.DarkGray;
                    this.comboBox1.Enabled = false;
                    this.comboBox2.Enabled = false;
                    this.comboBox3.Enabled = false;
                    frm.SendNextFun();
                }
            }
            else
            {
                NewSendText.是否周期 = false;
                this.btnSendTime.Enabled = false;
                this.btnSendTime.Text = "修";
                this.tbxSendTime.Text = "-";
                this.tbxSendTime.ReadOnly = true;
                this.tbxSendTime.BackColor = Color.DarkGray;
                this.lblNowTime.Text = "无";
                this.checkBox1.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                if (!NewSendText.是否自动)
                {
                    NewSendText.是否自动 = true;
                    this.CycleCheckBox.Enabled = false;
                    this.btnSendTime.Enabled = false;
                    this.btnSendTime.Text = "修";
                    this.tbxSendTime.Text = "-";
                    this.tbxSendTime.ReadOnly = true;
                    this.tbxSendTime.BackColor = Color.DarkGray;
                    this.lblNowTime.Text = "无";
                    this.checkBox2.Enabled = true;
                    if (this.checkBox2.Checked)
                    {
                        this.AutoNumberButton.Enabled = true;
                        this.comboBox1.Enabled = true;
                        this.comboBox2.Enabled = true;
                        this.comboBox3.Enabled = true;
                        this.AutoNumberTextBox.Text = NewSendText.AutoNumber.ToString();
                        this.comboBox1.SelectedIndex = (int)NewSendText.AutoKey;
                        this.comboBox2.SelectedIndex = (int)NewSendText.AutoOperator;
                        this.comboBox3.SelectedIndex = (int)NewSendText.AutoNo;
                    }
                }
            }
            else
            {
                NewSendText.是否自动 = false;
                this.CycleCheckBox.Enabled = true;
                this.checkBox2.Enabled = false;
                this.AutoNumberButton.Enabled = false;
                this.AutoNumberButton.Text = "修";
                this.AutoNumberTextBox.ReadOnly = true;
                this.AutoNumberTextBox.BackColor = Color.DarkGray;
                this.comboBox1.Enabled = false;
                this.comboBox2.Enabled = false;
                this.comboBox3.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked && this.checkBox1.Checked)
            {
                NewSendText.AutoCondition = true;
                this.AutoNumberButton.Enabled = true;
                this.comboBox1.Enabled = true;
                this.comboBox2.Enabled = true;
                this.comboBox3.Enabled = true;
                this.AutoNumberTextBox.Text = NewSendText.AutoNumber.ToString();
                this.comboBox1.SelectedIndex = (int)NewSendText.AutoKey;
                this.comboBox2.SelectedIndex = (int)NewSendText.AutoOperator;
                this.comboBox3.SelectedIndex = (int)NewSendText.AutoNo;
            }
            else
            {
                NewSendText.AutoCondition = false;
                this.AutoNumberButton.Enabled = false;
                this.AutoNumberButton.Text = "修";
                this.AutoNumberTextBox.ReadOnly = true;
                this.AutoNumberTextBox.BackColor = Color.DarkGray;
                this.comboBox1.Enabled = false;
                this.comboBox2.Enabled = false;
                this.comboBox3.Enabled = false;
            }
        }
        #endregion

        #region 输入限制
        private void tbxSendTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void AutoNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbxNowStartCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region 选择功能
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewSendText.AutoKey = (NewSendText.AutoKeyValue)(sender as ComboBox).SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewSendText.AutoOperator = (NewSendText.AutoOperatorValue)(sender as ComboBox).SelectedIndex;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewSendText.AutoNo = (NewSendText.AutoNoValue)(sender as ComboBox).SelectedIndex;
        }
        #endregion

        private void newButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
