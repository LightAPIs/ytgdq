using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions; //正则
using System.Collections;
using WindowsFormsApplication2.发文重写;
using WindowsFormsApplication2.Storage;
using Newtonsoft.Json;
using TyDll;

namespace WindowsFormsApplication2
{
    public partial class 新发文 : Form
    {
        Form1 frm;

        private int txtLocation = 0;

        /// <summary>
        /// 文章每页数量
        /// </summary>
        private readonly int ArticlePageSize = 30;

        /// <summary>
        /// 文章总数
        /// </summary>
        private int totalArticleCount = 0;

        /// <summary>
        /// 文章总页数
        /// </summary>
        private int ArticleTotalPage
        {
            get
            {
                return (int)Math.Ceiling((float)this.totalArticleCount / this.ArticlePageSize);
            }
        }

        /// <summary>
        /// 当前文章页数
        /// </summary>
        private int currentArticlePage = 0;

        /// <summary>
        /// 当前文章数据
        /// </summary>
        private StorageDataSet.ArticleDataTable currentArticleData = new StorageDataSet.ArticleDataTable();

        /// <summary>
        /// 文章搜索文本
        /// </summary>
        private string articleSearchText = "";

        /// <summary>
        /// 配置每页数量
        /// </summary>
        private readonly int SentPageSize = 30;

        /// <summary>
        /// 配置总数
        /// </summary>
        private int totalSentCount = 0;

        /// <summary>
        /// 配置总页数
        /// </summary>
        private int SentTotalPage
        {
            get
            {
                return (int)Math.Ceiling((float)this.totalSentCount / this.SentPageSize);
            }
        }

        /// <summary>
        /// 当前配置页数
        /// </summary>
        private int currentSentPage = 0;

        /// <summary>
        /// 当前配置数据
        /// </summary>
        private StorageDataSet.SentDataTable currentSentData = new StorageDataSet.SentDataTable();

        /// <summary>
        /// 配置搜索文本
        /// </summary>
        private string sentSearchText = "";

        public 新发文(Form1 frm1)
        {
            frm = frm1;
            InitializeComponent();
        }

        private void 新发文_Load(object sender, EventArgs e)
        {
            //DriveInfo[] Drives = DriveInfo.GetDrives();
            //HeaderFresh("我的电脑");
            //foreach (DriveInfo Dirs in Drives) {
            //    listViewFile.Items.Add(new ListViewItem(new string[] {Dirs.Name,"磁盘"}));
            //}

            NewSendText.SentId = -1;
            ReadAll(Application.StartupPath);
            ReadSavedArticle();
            ReadSavedSent();

            _Ini t2 = new _Ini("config.ini");
            this.cbxTickOut.Checked = bool.Parse(t2.IniReadValue("发文面板配置", "自动剔除空格", "True"));
            if (!File.Exists(Application.StartupPath + "\\TyDll.dll"))
            {
                this.lbxTextList.Items.Clear();
                this.lbxTextList.SelectedIndexChanged -= new EventHandler(lbxTextList_SelectedIndexChanged);
                MessageBox.Show("未找到TyDll.dll文件！");
            }
        }

        #region Tab页切换
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            int getid = (sender as TabControl).SelectedIndex;
            if (getid == 2)
            { //剪切板
                try
                {
                    rtbClipboard.Text = Clipboard.GetText();
                    tbxTitle.Text = "来自剪贴板";
                }
                catch (Exception err)
                {
                    rtbClipboard.Text = err.Message + "，请自行粘贴！";
                }
            }

            if (getid == 4)
            { // 保存的发文配置，需要锁定一些控件
                this.cbxTickOut.Enabled = false;
                this.cbx乱序全段不重复.Enabled = false;
                this.tabControl2.Enabled = false;
                this.groupBox1.Enabled = false;
            }
            else
            {
                NewSendText.SentId = -1;
                this.cbxTickOut.Enabled = true;
                this.cbx乱序全段不重复.Enabled = true;
                this.tabControl2.Enabled = true;
                this.groupBox1.Enabled = true;
            }
        }
        #endregion

        #region 文章处理
        /// <summary>
        /// 获取到的文章内容
        /// </summary>
        private string GetText = "";
        private void lbxTextList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex;
            if ((sender as ListBox).Text.Length != 0)
            {
                GetText = TyDll.GetResources.GetText("Resources.TXT." + (sender as ListBox).Text + ".txt");
                lblTitle.Text = (sender as ListBox).Text;

                ComText(); //确认文章信息
            }
            switch (index)
            {
                case 0: rtbInfo.Text = "选用标准常用字前一千五百的【前五百】个单字"; break;
                case 1: rtbInfo.Text = "选用标准常用字前一千五百的【中五百】个单字"; break;
                case 2: rtbInfo.Text = "选用标准常用字前一千五百的【后五百】个单字"; break;
                case 3: rtbInfo.Text = "选用标准常用字前一千五百个单字整体"; break;
                case 4: rtbInfo.Text = "选用标准常用词组前二百个词组 (以空格作为词组分隔符)"; break;
                case 5: rtbInfo.Text = "选【心的出口】现代文一篇"; break;
                case 6: rtbInfo.Text = "选【冰灯】现代文一篇"; break;
                case 7: rtbInfo.Text = "选【为人民服务】现代文节选一篇"; break;
                case 8: rtbInfo.Text = "选【从百草园到三味书屋】白话文一篇，收录于鲁迅作品《朝花夕拾》"; break;
                case 9: rtbInfo.Text = "选【岳阳楼记】古文一篇"; break;
                default: rtbInfo.Text = "没有定义的内容"; break;
            }
        }

        /// <summary>
        /// 自动处理并判定文段类型
        /// 预先判定类型，可后续手动更改；
        /// 只区分"文章"和"单字"；
        /// </summary>
        /// <param name="auto">是否自动确认文章类型</param>
        public void ComText(bool auto = true)
        {
            string tickText = GetText;
            // 注：只是采用去除空格和换行后的文本进行判定，但并没有修改原始的获取文本
            if (this.cbxTickOut.Checked)
            {
                tickText = TickBlock(GetText, "");
            }

            if (tickText.Length != 0)
            {
                if (tickText.Length > 300)
                {
                    rtbShowText.Text = tickText.Substring(0, 300) + "[......未完]";
                }
                else
                {
                    rtbShowText.Text = tickText + "[已完]";
                }

                if (tickText.Length > 25)
                {
                    this.tbxSendCount.Text = "25";
                }
                else
                {
                    this.tbxSendCount.Text = tickText.Length.ToString(); // 在 25 字以下时默认发送全文
                }

                if (auto)
                { // 确认文章类型
                    IsWords(tickText);
                }
                this.label2.Text = "总字数";
                this.label4.Text = "字数";
                lblTextCount.Text = tickText.Length.ToString();

                tbxSendCount.Select();
                tbxSendCount.MaxLength = lblTextCount.Text.Length;
                tbxSendStart.MaxLength = lblTextCount.Text.Length;
            }
        }

        /// <summary>
        /// 找到词组信息
        /// </summary>
        private void FindWords()
        {
            string[] getWords;
            if (cbxSplit.SelectedIndex == 1)
            {
                getWords = GetText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                getWords = GetText.Split(split);
            }

            if (getWords.Length > 1)
            {
                lblTextCount.Text = getWords.Length.ToString();
                ShowFlowText("找到" + getWords.Length + "个词组");

            }
            else
            {
                ShowFlowText("未找到词组，请确定您所选择的文件");
            }

            NewSendText.词组 = getWords;
        }
        /// <summary>
        /// 显示浮动的信息
        /// </summary>
        /// <param name="text">需要显示的信息</param>
        public void ShowFlowText(string text)
        {
            ShowMessage sm = new ShowMessage(this.Size, this.Location, this);
            sm.Show(text);
        }

        /// <summary>
        /// 确定文章类型
        /// </summary>
        /// <param name="text"></param>
        public void IsWords(string text)
        {
            Regex regexAll = new Regex(@"，|。|！|…|：|“|”|？");
            if (regexAll.IsMatch(text))
            {
                tabControl2.SelectedIndex = 1;
                this.lblStyle.Text = "文章";
            }
            else
            {
                tabControl2.SelectedIndex = 0;
                this.lblStyle.Text = "单字";
            }
        }

        /// <summary>
        /// 分隔的默认定义
        /// </summary>
        private char split = ' ';
        /// <summary>
        /// 词组分隔符确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSplit_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;
            if (index > -1)
            {
                switch (index)
                {
                    case 0: split = ' '; break;
                    case 1: split = '\n'; break;
                    case 2: split = '\t'; break;
                    case 3: split = (this.tbxsplit.TextLength > 0) ? this.tbxsplit.Text.ToCharArray()[0] : ' '; break;
                }
                FindWords();
            }
        }

        private void tbxsplit_TextChanged(object sender, EventArgs e)
        {
            string text = (sender as TextBox).Text;
            if (this.cbxSplit.SelectedIndex == 3)
            {
                split = text.Length > 0 ? text.ToCharArray()[0] : ' ';
                FindWords();
            }
        }

        /// <summary>
        /// 手动选择文段类型处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            int nowIndex = (sender as TabControl).SelectedIndex;
            if (nowIndex == 2 && this.lblStyle.Text != "词组")
            {
                this.cbxSplit.SelectedIndex = -1;
                this.label2.Text = "总词数";
                this.label4.Text = "词数";
                if (NewSendText.SentId < 0)
                {
                    //* 恢复显示原文，因为词组是不受自动移除空格换行影响的
                    if (GetText.Length != 0)
                    {
                        if (GetText.Length > 300)
                        {
                            this.rtbShowText.Text = GetText.Substring(0, 300) + "[......未完]";
                        }
                        else
                        {
                            this.rtbShowText.Text = GetText + "[已完]";
                        }
                    }
                    ShowFlowText("请选择词组分隔符来检索词组内容");
                }
            }
            else if ((nowIndex == 0 || nowIndex == 1) && this.lblStyle.Text == "词组")
            {
                ComText(false);
            }
            this.lblStyle.Text = (sender as TabControl).TabPages[this.tabControl2.SelectedIndex].Text;
        }

        private void lblStyle_TextChanged(object sender, EventArgs e)
        {
            string getit = (sender as Label).Text;
            if (getit == "单字")
            {
                (sender as Label).ForeColor = Color.DarkOliveGreen;
            }
            else if (getit == "文章")
            {
                (sender as Label).ForeColor = Color.IndianRed;
            }
            else if (getit == "词组")
            {
                (sender as Label).ForeColor = Color.DeepPink;
            }
            else
            {
                (sender as Label).ForeColor = Color.Black;
            }
        }
        #endregion

        #region 清除所有信息
        /// <summary>
        /// 清除所有信息
        /// </summary>
        private void ClearAll()
        {
            if (lblTitle.Text.Length != 0)
            {
                lblStyle.ResetText();
                lblTextCount.ResetText();
                lblTitle.ResetText();
                rtbInfo.ResetText();
                rtbShowText.ResetText();
                GetText = "";
                NewSendText.SentId = -1;
            }
        }
        #endregion

        #region 程序控制
        //? 自动与周期是不能同时开始的
        /// <summary>
        /// 周期开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                nudSendTimer.Enabled = true;
                speedfill();
                this.cbxAuto.Enabled = false;
            }
            else
            {
                nudSendTimer.Enabled = false;
                this.lblspeed.Text = "0";
                this.cbxAuto.Enabled = true;
            }
        }

        /// <summary>
        /// 自动开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxAuto_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                this.checkBox1.Enabled = false;
            }
            else
            {
                this.checkBox1.Enabled = true;
            }
        }

        private void btnAllText_Click(object sender, EventArgs e)
        {
            string allLen = this.lblTextCount.Text;
            if (!string.IsNullOrEmpty(allLen))
            {
                this.tbxSendStart.Text = "0";
                this.tbxSendCount.Text = this.lblTextCount.Text;
            }
        }

        //速度显示 由 周期
        private void nudSendTimer_ValueChanged(object sender, EventArgs e)
        {
            speedfill();
        }

        private void speedfill()
        {
            try
            {
                int textcount = int.Parse(this.tbxSendCount.Text);
                int nowtime = (int)this.nudSendTimer.Value;
                double speed;
                if (nowtime > 0)
                {
                    speed = (double)textcount * 60 / nowtime;
                    if (speed <= 400)
                        this.lblspeed.Text = speed.ToString("0");
                    else
                        this.lblspeed.Text = "起飞";
                }
            }
            catch { this.lblspeed.Text = "0"; }
        }
        #endregion

        #region 自定义文章
        private void listViewFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listViewFile.HitTest(e.X, e.Y); ;
            if (info.Item != null)
            {
                string Hearder = listViewFile.Columns[0].Text;
                if (info.Item.Text == "..")
                { // 返回上一层
                    string path = "";
                    try
                    {
                        if (Hearder.Length != 3 || !Hearder.Contains(@":\"))
                        {
                            path = Directory.GetParent(Hearder).FullName;//上一级目录
                        }
                    }
                    catch { }
                    if (path.Length != 0)
                    {
                        ReadAll(path);
                    }
                    else
                    {
                        HeaderFresh("我的电脑");
                        DriveInfo[] Drives = DriveInfo.GetDrives();
                        foreach (DriveInfo Dirs in Drives)
                        {
                            listViewFile.Items.Add(new ListViewItem(new string[] { Dirs.Name, "磁盘" }));
                        }
                    }
                }
                else
                {
                    if (Directory.Exists(info.Item.Text))
                    { // 当前工作存在该目录
                        ReadAll(info.Item.Text);
                    }
                    else
                    { // 不存在目录
                        string dir;
                        if (Hearder[Hearder.Length - 1] == '\\')
                        {
                            dir = Hearder + info.Item.Text;
                        }
                        else
                        {
                            dir = Hearder + "\\" + info.Item.Text;
                        }
                        if (dir.Length != 0)
                            ReadAll(dir); // 读取内容
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadAll(Application.StartupPath);
            this.listViewFile.Focus();
        }

        private void btnUP_Click(object sender, EventArgs e)
        {
            string Hearder = listViewFile.Columns[0].Text;
            string path = "";
            try
            {
                if (Hearder.Length != 3)
                    path = Directory.GetParent(Hearder).FullName;//上一级目录
            }
            catch { }
            if (path.Length != 0)
            {
                ReadAll(path);
            }
            else
            {
                HeaderFresh("我的电脑");
                DriveInfo[] Drives = DriveInfo.GetDrives();
                foreach (DriveInfo Dirs in Drives)
                {
                    listViewFile.Items.Add(new ListViewItem(new string[] { Dirs.Name, "磁盘" }));
                }
            }
            this.listViewFile.Focus();
        }

        /// <summary>
        /// 自定义文章读取方法
        /// </summary>
        /// <param name="path"></param>
        private void ReadAll(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    string[] dirs = Directory.GetDirectories(path);
                    HeaderFresh(path);
                    listViewFile.Items.Add(new ListViewItem(new string[] { "..", "文件夹" }));
                    foreach (string dir in dirs)
                    {
                        listViewFile.Items.Add(new ListViewItem(new string[] { Path.GetFileName(dir), "文件夹" }));
                    }
                    string[] files = Directory.GetFiles(path);
                    int findCount = 0;
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.Extension == ".txt")
                        {
                            ListViewItem item = new ListViewItem(new string[] { fi.Name, "文本" });
                            listViewFile.Items.Add(item);
                            item.ForeColor = Color.Green;
                            findCount++;
                            if (findCount == 1) txtLocation = listViewFile.Items.Count;
                        }
                    }
                    this.lblFindTXTCount.Text = findCount.ToString();//找到文章数量
                }
                catch (Exception err) { MessageBox.Show(err.Message, "跟打器提示！"); }
            }
            else
            { // 获取到文章
                if (File.Exists(path))
                {
                    StreamReader fm = new StreamReader(path, System.Text.Encoding.Default);
                    GetText = fm.ReadToEnd();
                    FileTitleTextBox.Text = Path.GetFileNameWithoutExtension(path);
                    ComText();
                }
            }
        }

        private void lblFindTXTCount_Click(object sender, EventArgs e)
        {
            try
            {
                this.listViewFile.TopItem = this.listViewFile.Items[txtLocation - 1];
            }
            catch { }
        }

        private void lblFindTXTCount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int get = int.Parse((sender as Label).Text);
                if (get > 0)
                {
                    this.lblFindTXTCount.ForeColor = Color.IndianRed;
                }
                else
                {
                    this.lblFindTXTCount.ForeColor = Color.Black;
                }
            }
            catch
            {
                this.lblFindTXTCount.ForeColor = Color.Black;
            }
        }

        private void HeaderFresh(string dir)
        {
            listViewFile.Items.Clear();
            listViewFile.Columns[0].Text = dir;
        }
        #endregion

        #region 剪贴板获取
        /// <summary>
        /// 重新获取剪贴板内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReGet_Click(object sender, EventArgs e)
        {
            try
            {
                rtbClipboard.Text = Clipboard.GetText();
            }
            catch (Exception err)
            {
                rtbClipboard.Text = err.Message + "，请自行编辑粘贴！";
            }
        }

        private void rtbClipboard_TextChanged(object sender, EventArgs e)
        {
            GetText = rtbClipboard.Text;
            lblTitle.Text = tbxTitle.Text;
            ComText();
        }
        #endregion

        #region 文章标题处理
        private void tbxTitle_TextChanged(object sender, EventArgs e)
        {
            lblTitle.Text = (sender as TextBox).Text;
            if (lblTitle.Text.Length == 0)
            {
                lblTitle.Text = "来自剪贴板";
            }
        }

        private void FileTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            lblTitle.Text = (sender as TextBox).Text;
            if (lblTitle.Text.Length == 0)
            {
                lblTitle.Text = "自定义文章";
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            tbxTitle.Text = "来自剪贴板";
        }
        #endregion

        #region 文章处理
        /// <summary>
        /// 替换字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private string TickBlock(string text, string target)
        {
            string s = text;
            s = s.Replace(" ", target);
            s = s.Replace("　", target);
            s = s.Replace("\r\n", target);
            s = s.Replace("\r", target);
            s = s.Replace("\n", target);
            return s;
        }
        #endregion

        #region 标记处理
        private void tbxSendStart_TextChanged(object sender, EventArgs e)
        {
            if (GetText.Length != 0)
            {
                string temp = (sender as TextBox).Text;
                if (temp.Length != 0)
                {
                    int c = int.Parse(temp);
                    int cou = int.Parse(tbxSendCount.Text);
                    try
                    {
                        rtbShowText.Text = GetText.Substring(c, cou) + "\r\n[...当前设置文段预览(非实际)]";
                        rtbShowText.ForeColor = Color.Black;
                        btnGoSend.Enabled = true;
                    }
                    catch
                    {
                        rtbShowText.Text = "标记起始点设置错误，因为设置数值超出总字数，请重设！";
                        rtbShowText.ForeColor = Color.IndianRed;
                        btnGoSend.Enabled = false;
                    }
                }
            }
        }

        private void tbxSendCount_TextChanged(object sender, EventArgs e)
        {
            Confrim();
            if (this.checkBox1.Checked) speedfill();
        }

        private void lblTextCount_TextChanged(object sender, EventArgs e)
        {
            string te = this.tbxSendCount.Text;
            if (te.Length != 0)
            {
                int cou = int.Parse(te);
                if (cou > 0 && tbxSendStart.Text != "")
                {
                    int c = int.Parse(tbxSendStart.Text);
                    if (c + cou > GetText.Length)
                    {
                        rtbShowText.Text = "发送字数设置错误！";
                        rtbShowText.ForeColor = Color.IndianRed;
                        btnGoSend.Enabled = false;
                    }
                    else
                    {
                        rtbShowText.ForeColor = Color.Black;
                        btnGoSend.Enabled = true;
                    }
                }
            }
        }

        private void Confrim()
        {
            if (GetText.Length != 0)
            {
                string te = this.tbxSendCount.Text;
                if (te.Length != 0)
                {
                    int cou = int.Parse(te);
                    if (cou > 0 && tbxSendStart.Text != "")
                    {
                        int c = int.Parse(tbxSendStart.Text);
                        if (c + cou <= GetText.Length)
                        {
                            try
                            {
                                rtbShowText.Text = GetText.Substring(c, cou) + "\r\n[...当前设置文段预览(非实际)]";
                                rtbShowText.ForeColor = Color.Black;
                                btnGoSend.Enabled = true;
                            }
                            catch
                            {
                                rtbShowText.Text = "在当前标起始点下，字数设置超出限制！";
                                rtbShowText.ForeColor = Color.IndianRed;
                                btnGoSend.Enabled = false;
                            }
                        }
                        else
                        {
                            rtbShowText.Text = "发送字数设置错误！";
                            rtbShowText.ForeColor = Color.IndianRed;
                            btnGoSend.Enabled = false;
                        }
                    }
                    else
                    {
                        rtbShowText.Text = "发送字数设置错误！";
                        rtbShowText.ForeColor = Color.IndianRed;
                        btnGoSend.Enabled = false;
                    }
                }
            }
        }

        private void tbxSendStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbxSendCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbxQisduan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 保存的文章
        /// <summary>
        /// 保存的文章读取方法
        /// </summary>
        private void ReadSavedArticle()
        {
            if (string.IsNullOrEmpty(this.articleSearchText))
            {
                this.totalArticleCount = Glob.ArticleHistory.GetArticleCount();
            }
            else
            {
                this.totalArticleCount = Glob.ArticleHistory.GetArticleCountFromSubTitle(this.articleSearchText);
            }
            this.ArticleCountLabel.Text = this.totalArticleCount.ToString();

            if (this.totalArticleCount > 0)
            {
                this.currentArticlePage = 1;
                ShowSaveArticle();
            }
            else
            {
                this.currentArticlePage = 0;
                this.ArticlePageLabel.Text = this.currentArticlePage.ToString();
                listViewArticle.Items.Clear();
            }
        }

        private void ShowSaveArticle()
        {
            listViewArticle.Items.Clear();
            this.ArticlePageLabel.Text = this.currentArticlePage.ToString();
            if (currentArticlePage > 0)
            {
                if (string.IsNullOrEmpty(this.articleSearchText))
                {
                    this.currentArticleData = Glob.ArticleHistory.GetArticle(this.currentArticlePage - 1, ArticlePageSize);
                }
                else
                {
                    this.currentArticleData = Glob.ArticleHistory.GetArticleFromSubTitle(this.articleSearchText, this.currentArticlePage - 1, ArticlePageSize);
                }

                foreach (var dataRow in this.currentArticleData)
                {
                    listViewArticle.Items.Add(new ListViewItem(new string[] { dataRow["id"].ToString(), dataRow["title"].ToString(), dataRow["create_time"].ToString() }));
                }
            }
        }

        private void ArticleFirstButton_Click(object sender, EventArgs e)
        {
            if (this.currentArticlePage > 1)
            {
                this.currentArticlePage = 1;
                this.ShowSaveArticle();
            }
        }

        private void ArticlePreButton_Click(object sender, EventArgs e)
        {
            if (this.currentArticlePage > 1)
            {
                this.currentArticlePage--;
                this.ShowSaveArticle();
            }
        }

        private void ArticleNextButton_Click(object sender, EventArgs e)
        {
            if (this.currentArticlePage < this.ArticleTotalPage)
            {
                this.currentArticlePage++;
                this.ShowSaveArticle();
            }
        }

        private void ArticleLastButton_Click(object sender, EventArgs e)
        {
            if (this.currentArticlePage < this.ArticleTotalPage)
            {
                this.currentArticlePage = this.ArticleTotalPage;
                this.ShowSaveArticle();
            }
        }

        private void listViewArticle_ItemActivate(object sender, EventArgs e)
        {
            if (this.listViewArticle.SelectedItems.Count > 0)
            {
                long id = long.Parse(this.listViewArticle.SelectedItems[0].SubItems[0].Text);
                StorageDataSet.ArticleRow sd = StorageDataSet.GetArticleRowFromId(this.currentArticleData, id);
                if (sd != null)
                {
                    lblTitle.Text = this.listViewArticle.SelectedItems[0].SubItems[1].Text;
                    GetText = sd["content"].ToString();
                    ComText();
                }
            }
        }

        private void ArticleEditTitleButton_Click(object sender, EventArgs e)
        {
            if (this.listViewArticle.SelectedItems.Count > 0)
            {
                string title = this.listViewArticle.SelectedItems[0].SubItems[1].Text;
                long id = long.Parse(this.listViewArticle.SelectedItems[0].SubItems[0].Text);
                ContentEditor cEditor = new ContentEditor(title);
                if (cEditor.ShowDialog() == DialogResult.OK)
                {
                    string result = cEditor.OutValue;
                    this.listViewArticle.SelectedItems[0].SubItems[1].Text = result;
                    Glob.ArticleHistory.UpdateArticleTitle(id, result);
                }
            }
        }

        private void ArticleEditContentButton_Click(object sender, EventArgs e)
        {
            if (this.listViewArticle.SelectedItems.Count > 0)
            {
                long id = long.Parse(this.listViewArticle.SelectedItems[0].SubItems[0].Text);
                StorageDataSet.ArticleRow sd = StorageDataSet.GetArticleRowFromId(this.currentArticleData, id);
                if (sd != null)
                {
                    string content = sd["content"].ToString();
                    ContentEditor cEditor = new ContentEditor(content);
                    if (cEditor.ShowDialog() == DialogResult.OK)
                    {
                        string result = cEditor.OutValue;
                        sd["content"] = result;
                        Glob.ArticleHistory.UpdateArticleContent(id, result);
                    }
                }
            }
        }

        private void ArticleSearchButton_Click(object sender, EventArgs e)
        {
            this.articleSearchText = this.ArticleSearchTextBox.Text;
            this.ReadSavedArticle();
        }

        private void ArticleDeleteItemButton_Click(object sender, EventArgs e)
        {
            if (this.listViewArticle.SelectedItems.Count > 0)
            {
                long id = long.Parse(this.listViewArticle.SelectedItems[0].SubItems[0].Text);
                StorageDataSet.ArticleRow sd = StorageDataSet.GetArticleRowFromId(this.currentArticleData, id);
                if (sd != null)
                {
                    switch (MessageBox.Show("确认删除ID=" + id.ToString() + "的文章吗？", "删除询问", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            if (Glob.ArticleHistory.DeleteArticleItemById(id))
                            {
                                this.totalArticleCount--;
                                this.ArticleCountLabel.Text = this.totalArticleCount.ToString();
                                if (this.totalArticleCount <= 0)
                                {
                                    this.totalArticleCount = 0;
                                    this.currentArticlePage = 0;
                                }
                                else if (this.currentArticlePage > this.ArticleTotalPage)
                                {
                                    this.currentArticlePage = this.ArticleTotalPage;
                                }
                                this.ShowSaveArticle();
                            }
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
            }
        }

        private void ArticleDeleteAllButton_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("确认删除所有保存的文章吗？", "删除询问", MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                    Glob.ArticleHistory.DeleteAllArticle();
                    this.ReadSavedArticle();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        #endregion

        #region 保存的配置
        /// <summary>
        /// 保存的发文配置读取方法
        /// </summary>
        private void ReadSavedSent()
        {
            if (string.IsNullOrEmpty(this.sentSearchText))
            {
                this.totalSentCount = Glob.SentHistory.GetSentCount();
            }
            else
            {
                this.totalSentCount = Glob.SentHistory.GetSentCountFromSubTitle(this.sentSearchText);
            }
            this.SentCountLabel.Text = this.totalSentCount.ToString();

            if (this.totalSentCount > 0)
            {
                this.currentSentPage = 1;
                ShowSaveSent();
            }
            else
            {
                this.currentSentPage = 0;
                this.SentPageLabel.Text = this.currentSentPage.ToString();
                listViewSent.Items.Clear();
            }
        }

        private void ShowSaveSent()
        {
            listViewSent.Items.Clear();
            this.SentPageLabel.Text = this.currentSentPage.ToString();
            if (currentSentPage > 0)
            {
                if (string.IsNullOrEmpty(this.sentSearchText))
                {
                    this.currentSentData = Glob.SentHistory.GetSent(this.currentSentPage - 1, SentPageSize);
                }
                else
                {
                    this.currentSentData = Glob.SentHistory.GetSentFromSubTitle(this.sentSearchText, this.currentSentPage - 1, SentPageSize);
                }

                foreach (var dataRow in this.currentSentData)
                {
                    listViewSent.Items.Add(new ListViewItem(new string[] { dataRow["id"].ToString(), dataRow["title"].ToString(), dataRow["create_time"].ToString() }));
                }
            }
        }

        private void SentFirstButton_Click(object sender, EventArgs e)
        {
            if (this.currentSentPage > 1)
            {
                this.currentSentPage = 1;
                this.ShowSaveSent();
            }
        }

        private void SentPreButton_Click(object sender, EventArgs e)
        {
            if (this.currentSentPage > 1)
            {
                this.currentSentPage--;
                this.ShowSaveSent();
            }
        }

        private void SentNextButton_Click(object sender, EventArgs e)
        {
            if (this.currentSentPage < this.SentTotalPage)
            {
                this.currentSentPage++;
                this.ShowSaveSent();
            }
        }

        private void SentLastButton_Click(object sender, EventArgs e)
        {
            if (this.currentSentPage < this.SentTotalPage)
            {
                this.currentSentPage = this.SentTotalPage;
                this.ShowSaveSent();
            }
        }

        private void listViewSent_ItemActivate(object sender, EventArgs e)
        {
            if (this.listViewSent.SelectedItems.Count > 0)
            {
                long id = long.Parse(this.listViewSent.SelectedItems[0].SubItems[0].Text);
                StorageDataSet.SentRow sd = StorageDataSet.GetSentRowFromId(this.currentSentData, id);
                if (sd != null)
                {
                    NewSendText.SentId = id;
                    NewSendText.文章全文 = sd["article"].ToString();
                    GetText = NewSendText.文章全文;
                    if (GetText.Length > 300)
                    {
                        rtbShowText.Text = GetText.Substring(0, 300) + "[......未完]";
                    }
                    else
                    {
                        rtbShowText.Text = GetText + "[已完]";
                    }

                    NewSendText.发文全文 = sd["full_text"].ToString();
                    NewSendText.标题 = this.listViewSent.SelectedItems[0].SubItems[1].Text.Trim();
                    lblTitle.Text = NewSendText.标题;

                    NewSendText.词组 = JsonConvert.DeserializeObject<string[]>(sd["phrases"].ToString());
                    NewSendText.词组发送分隔符 = sd["separator"].ToString();
                    if ((int)sd["disorder"] == 0)
                    {
                        NewSendText.是否乱序 = false;
                        this.rbninOrder.Checked = true;
                        this.rbnOutOrder.Checked = false;
                    }
                    else
                    {
                        NewSendText.是否乱序 = true;
                        this.rbninOrder.Checked = false;
                        this.rbnOutOrder.Checked = true;
                    }
                    if ((int)sd["no_repeat"] == 0)
                    {
                        NewSendText.乱序全段不重复 = false;
                        this.cbx乱序全段不重复.Checked = false;
                    }
                    else
                    {
                        NewSendText.乱序全段不重复 = true;
                        this.cbx乱序全段不重复.Checked = true;
                    }

                    switch ((int)sd["type"])
                    {
                        case 1:
                            NewSendText.类型 = "文章";
                            lblStyle.Text = "文章";
                            tabControl2.SelectedIndex = 1;
                            lblTextCount.Text = GetText.Length.ToString();
                            this.label2.Text = "总字数";
                            this.label4.Text = "字数";
                            break;
                        case 2:
                            NewSendText.类型 = "词组";
                            lblStyle.Text = "词组";
                            tabControl2.SelectedIndex = 2;
                            tbxSendSplit.Text = NewSendText.词组发送分隔符;
                            lblTextCount.Text = NewSendText.词组.Length.ToString();
                            this.label2.Text = "总词数";
                            this.label4.Text = "词数";
                            break;
                        case 0:
                        default:
                            NewSendText.类型 = "单字";
                            lblStyle.Text = "单字";
                            tabControl2.SelectedIndex = 0;
                            lblTextCount.Text = GetText.Length.ToString();
                            this.label2.Text = "总字数";
                            this.label4.Text = "字数";
                            break;
                    }

                    NewSendText.字数 = (int)sd["count"];
                    NewSendText.标记 = (int)sd["mark"];
                    tbxSendCount.Text = NewSendText.字数.ToString();
                    tbxSendStart.Text = NewSendText.标记.ToString();
                    tbxSendCount.MaxLength = lblTextCount.Text.Length;
                    tbxSendStart.MaxLength = lblTextCount.Text.Length;

                    Glob.TempSegmentRecord.Clear();
                    Glob.TempSegmentRecord = JsonConvert.DeserializeObject<List<string>>(sd["segment_record"].ToString());
                    Glob.SendCursor = (int)sd["segment_cursor"];

                    Glob.CurSegmentNum = (int)sd["cur_segment_num"];
                    tbxQisduan.Text = (Glob.CurSegmentNum + 1).ToString(); //* 起始段号，因为开始时是在发下一段

                    NewSendText.已发段数 = (int)sd["sent_num"];
                    NewSendText.已发字数 = (int)sd["sent_count"];

                    if ((int)sd["cycle"] == 1)
                    {
                        NewSendText.是否周期 = true;
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        NewSendText.是否周期 = false;
                        checkBox1.Checked = false;
                    }
                    NewSendText.周期 = (int)sd["cycle_value"];
                    nudSendTimer.Value = NewSendText.周期;

                    if ((int)sd["auto"] == 1)
                    {
                        NewSendText.是否自动 = true;
                        cbxAuto.Checked = true;
                    }
                    else
                    {
                        NewSendText.是否自动 = false;
                        cbxAuto.Checked = false;
                    }
                }
            }
        }

        private void SentEditTitleButton_Click(object sender, EventArgs e)
        {
            if (this.listViewSent.SelectedItems.Count > 0)
            {
                string title = this.listViewSent.SelectedItems[0].SubItems[1].Text;
                long id = long.Parse(this.listViewSent.SelectedItems[0].SubItems[0].Text);
                ContentEditor cEditor = new ContentEditor(title);
                if (cEditor.ShowDialog() == DialogResult.OK)
                {
                    string result = cEditor.OutValue;
                    this.listViewSent.SelectedItems[0].SubItems[1].Text = result;
                    Glob.SentHistory.UpdateSentTitle(id, result);
                }
            }
        }

        private void SentSearchButton_Click(object sender, EventArgs e)
        {
            this.sentSearchText = this.SentSearchTextBox.Text;
            this.ReadSavedSent();
        }

        private void SentDeleteButton_Click(object sender, EventArgs e)
        {
            if (this.listViewSent.SelectedItems.Count > 0)
            {
                long id = long.Parse(this.listViewSent.SelectedItems[0].SubItems[0].Text);
                StorageDataSet.SentRow sd = StorageDataSet.GetSentRowFromId(this.currentSentData, id);
                if (sd != null)
                {
                    switch (MessageBox.Show("确认删除ID=" + id.ToString() + "的发文配置吗？", "删除询问", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            if (Glob.SentHistory.DeleteSentItemById(id))
                            {
                                this.totalSentCount--;
                                this.SentCountLabel.Text = this.totalSentCount.ToString();
                                if (this.totalSentCount <= 0)
                                {
                                    this.totalSentCount = 0;
                                    this.currentSentPage = 0;
                                }
                                else if (this.currentSentPage > this.SentTotalPage)
                                {
                                    this.currentSentPage = this.SentTotalPage;
                                }
                                this.ShowSaveSent();
                            }
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
            }
        }

        private void SentDeleteAllButton_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("确认删除所有保存的发文配置吗？", "删除询问", MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                    Glob.SentHistory.DeleteAllSent();
                    this.ReadSavedSent();
                    break;
                case DialogResult.No:
                    break;
            }
        }
        #endregion

        #region 发文整体配置
        private void cbxTickOut_CheckedChanged(object sender, EventArgs e)
        {
            if (NewSendText.SentId < 0)
            {
                ComText(false);
            }

            bool temp = (sender as CheckBox).Checked;
            _Ini t2 = new _Ini("config.ini");
            if (temp)
            {
                t2.IniWriteValue("发文面板配置", "自动剔除空格", "True");
            }
            else
            {
                t2.IniWriteValue("发文面板配置", "自动剔除空格", "False");
            }
        }
        #endregion

        #region 编辑发文内容
        private void FileEditButton_Click(object sender, EventArgs e)
        {
            if (GetText != "")
            {
                ContentEditor cEditor = new ContentEditor(GetText);
                if (cEditor.ShowDialog() == DialogResult.OK)
                {
                    GetText = cEditor.OutValue;
                    ComText();
                }
            }
        }

        private void ClipboardEditButton_Click(object sender, EventArgs e)
        {
            if (GetText != "")
            {
                ContentEditor cEditor = new ContentEditor(GetText);
                if (cEditor.ShowDialog() == DialogResult.OK)
                {
                    this.rtbClipboard.Text = cEditor.OutValue;
                }
            }
        }
        #endregion

        #region 开始发文
        private void btnGoSend_Click(object sender, EventArgs e)
        {
            if (NewSendText.SentId < 0)
            {
                rtbShowText.Text = "处理中...";
                string sourceText = GetText; // 保留源文章内容
                if (this.cbxTickOut.Checked)
                { // 勾选"自动清除空格和换行时"
                    GetText = TickBlock(GetText, "");
                }
                if (GetText.Length == 0)
                {
                    rtbShowText.Text = "未获取到文章！";
                    MessageBox.Show("未获取到文章！");
                    return;
                }
                string theTitle = lblTitle.Text.Trim();
                if (theTitle.Length == 0)
                {
                    rtbShowText.Text = "标题为空！";
                    MessageBox.Show("标题为空！");
                    return;
                }

                Glob.TempSegmentRecord.Clear();
                Glob.SendCursor = 0;
                NewSendText.已发段数 = 0;
                NewSendText.已发字数 = 0;
                NewSendText.标题 = theTitle;
                NewSendText.文章全文 = GetText;
                NewSendText.发文全文 = NewSendText.文章全文;
                NewSendText.类型 = lblStyle.Text;
                if (NewSendText.类型 == "词组")
                {
                    if (NewSendText.词组.Length <= 1)
                    {
                        rtbShowText.Text = "未获取到词组！";
                        MessageBox.Show("未获取到词组！");
                        return;
                    }

                    NewSendText.词组发送分隔符 = this.tbxSendSplit.Text;
                }
                else
                { //! 非词组时清空，防止保存发文配置时存储冗余信息
                    NewSendText.词组 = new string[0];
                }

                NewSendText.是否乱序 = rbnOutOrder.Checked;
                NewSendText.乱序全段不重复 = this.cbx乱序全段不重复.Checked;
                try
                {
                    NewSendText.字数 = int.Parse(tbxSendCount.Text);
                    NewSendText.标记 = int.Parse(tbxSendStart.Text);
                    int startSegmentNum = int.Parse(this.tbxQisduan.Text);
                    Glob.CurSegmentNum = startSegmentNum - 1; // SendAOnce() 里会先自加一次，所以需要提前减一
                }
                catch { MessageBox.Show("请检查字数、标记或者起始段号是否设置错误？"); return; }
                NewSendText.是否周期 = checkBox1.Checked;
                NewSendText.周期 = (int)nudSendTimer.Value;
                NewSendText.是否自动 = cbxAuto.Checked;
                NewSendText.文章来源 = tabControl1.SelectedIndex;
                // 勾选"保存文章"
                if ((NewSendText.文章来源 == 1 && this.FileSaveCheckBox.Checked) || (NewSendText.文章来源 == 2 && this.ClipboardSaveCheckBox.Checked))
                {
                    Glob.ArticleHistory.InsertArticle(sourceText, Validation.GetMd5Hash(sourceText), NewSendText.标题, DateTime.Now.ToString("s"));
                }

                NewSendText.发文状态 = true;
                frm.SetMatch(false);
                if (NewSendText.是否周期)
                {
                    frm.SendTTest();
                }
                else
                {
                    frm.SendAOnce();
                }
            }
            else
            {
                NewSendText.文章来源 = tabControl1.SelectedIndex;
                NewSendText.发文状态 = true;
                frm.SetMatch(false);
                frm.SendNextFun();
            }

            frm.发文状态ToolStripMenuItem.PerformClick(); // 模拟点击"发文"→"发文状态"菜单项，用于显示发文状态窗口
            this.Close();
        }
        #endregion
    }
}
