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
        private readonly Form1 frm;

        #region 网络文章数据
        /// <summary>
        /// 网络文章主地址
        /// </summary>
        private readonly string WebSourceRoot = "https://cdn.jsdelivr.net/gh/LightAPIs/article-storage@main/";

        /// <summary>
        /// 网络文章通用列表文件名称
        /// </summary>
        private readonly string WebListName = "list.json";

        /// <summary>
        /// 网络每页数量
        /// </summary>
        private readonly int WebPageSize = 30;

        private readonly Regex DirReg = new Regex(@"[^/]+/$");

        /// <summary>
        /// 网络项目总数
        /// </summary>
        private int totalWebCount = 0;

        /// <summary>
        /// 网络总页数
        /// </summary>
        private int WebTotalPage
        {
            get
            {
                return (int)Math.Ceiling((float)this.totalWebCount / this.WebPageSize);
            }
        }

        /// <summary>
        /// 当前网络页数
        /// </summary>
        private int currentWebPage = 0;

        /// <summary>
        /// 网络根目录
        /// </summary>
        private WebArticle webRoot = null;

        /// <summary>
        /// 所有网络文章列表内容
        /// </summary>
        private WebArticle allWebArticle = new WebArticle();

        /// <summary>
        /// 网络文章列表搜索结果
        /// </summary>
        private WebArticle searchWebArticle = new WebArticle();

        /// <summary>
        /// 当前网络文章列表内容
        /// </summary>
        private WebArticle currentWebArticle = new WebArticle();

        /// <summary>
        /// 当前网络目录
        /// </summary>
        private string currentDir = "src/";

        private string FullCurrentDirPath
        {
            get
            {
                return this.WebSourceRoot + this.currentDir + this.WebListName;
            }
        }

        /// <summary>
        /// 上级目录
        /// </summary>
        private string lastDir = "";

        private string FullLastDirPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.lastDir))
                {
                    return "";
                }
                else
                {
                    return this.WebSourceRoot + this.lastDir + this.WebListName;
                }
            }
        }

        /// <summary>
        /// 网络搜索文本
        /// </summary>
        private string webSearchText = "";
        #endregion

        #region 本地文章数据
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
        #endregion

        #region 发文配置数据
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
        #endregion

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
            AutoKeyComboBox.SelectedIndex = 0;
            AutoOperatorComboBox.SelectedIndex = 0;
            AutoNoComboBox.SelectedIndex = 0;
            ReadAll(Application.StartupPath);
            ReadSavedArticle();
            ReadSavedSent();

            _Ini t2 = new _Ini("config.ini");
            this.cbxTickOut.Checked = bool.Parse(t2.IniReadValue("发文面板配置", "自动剔除空格", "True"));
            this.EncodedComboBox.SelectedIndex = int.Parse(t2.IniReadValue("发文面板配置", "文件编码", "0"));

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
            else if (getid == 3)
            {
                this.ReadWebArticle();
            }

            if (getid == 5)
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

                double diff = frm.DiffDict.Calc(tickText);
                this.DiffcultyLabel.Text = frm.DiffDict.DiffText(diff);

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
            { //* 切换到词组
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

                        //* 难度文本不用处理，因为本质上难度不会变化
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
                DiffcultyLabel.Text = "难度";
                tbxSendStart.Text = "0";
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
                this.AutoConditionCheckBox.Enabled = true;
                if (AutoConditionCheckBox.Checked)
                {
                    this.AutoKeyComboBox.Enabled = true;
                    this.AutoOperatorComboBox.Enabled = true;
                    this.AutoNumberTextBox.Enabled = true;
                    this.AutoNoComboBox.Enabled = true;
                }
            }
            else
            {
                this.checkBox1.Enabled = true;
                this.AutoConditionCheckBox.Enabled = false;
                if (AutoConditionCheckBox.Checked)
                {
                    this.AutoKeyComboBox.Enabled = false;
                    this.AutoOperatorComboBox.Enabled = false;
                    this.AutoNumberTextBox.Enabled = false;
                    this.AutoNoComboBox.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 条件自动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoConditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                this.AutoKeyComboBox.Enabled = true;
                this.AutoOperatorComboBox.Enabled = true;
                this.AutoNumberTextBox.Enabled = true;
                this.AutoNoComboBox.Enabled = true;
            }
            else
            {
                this.AutoKeyComboBox.Enabled = false;
                this.AutoOperatorComboBox.Enabled = false;
                this.AutoNumberTextBox.Enabled = false;
                this.AutoNoComboBox.Enabled = false;
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

        #region 本地文章
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
        /// 本地文章读取方法
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
                    Encoding encoding = Encoding.Default;
                    switch (this.EncodedComboBox.SelectedIndex)
                    {
                        case 1:
                            encoding = Encoding.UTF8;
                            break;
                        case 2:
                            encoding = Encoding.GetEncoding("big5");
                            break;
                        case 3:
                            encoding = Encoding.BigEndianUnicode;
                            break;
                        case 4:
                            encoding = Encoding.Unicode;
                            break;
                        case 0:
                        default:
                            encoding = Encoding.Default;
                            break;
                    }

                    StreamReader fm = new StreamReader(path, encoding);
                    GetText = fm.ReadToEnd();
                    fm.Close();
                    FileTitleTextBox.Text = Path.GetFileNameWithoutExtension(path);
                    ComText();
                }
            }
        }

        private void lblFindTXTCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listViewFile.Items.Count > 0)
                {
                    this.listViewFile.TopItem = this.listViewFile.Items[0];
                }
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
            lblTitle.Text = (sender as TextBox).Text.Trim();
            if (lblTitle.Text.Length == 0)
            {
                lblTitle.Text = "来自剪贴板";
            }
        }

        private void FileTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            lblTitle.Text = (sender as TextBox).Text.Trim();
            if (lblTitle.Text.Length == 0)
            {
                lblTitle.Text = "本地文章";
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
                        string tickText = GetText;
                        if (this.cbxTickOut.Checked && this.tabControl2.SelectedIndex != 2)
                        {
                            tickText = TickBlock(GetText, "");
                        }
                        string showText = tickText.Substring(c, cou);
                        rtbShowText.Text = showText + "\r\n[...当前设置文段预览(非实际)]";
                        rtbShowText.ForeColor = Color.Black;
                        double diff = frm.DiffDict.Calc(showText);
                        DiffcultyLabel.Text = frm.DiffDict.DiffText(diff);
                        btnGoSend.Enabled = true;
                    }
                    catch
                    {
                        rtbShowText.Text = "标记起始点设置错误，因为设置数值超出总字数，请重设！";
                        rtbShowText.ForeColor = Color.IndianRed;
                        DiffcultyLabel.Text = "难度";
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
                                string tickText = GetText;
                                if (this.cbxTickOut.Checked && this.tabControl2.SelectedIndex != 2)
                                {
                                    tickText = TickBlock(GetText, "");
                                }
                                string showText = tickText.Substring(c, cou);
                                rtbShowText.Text = showText + "\r\n[...当前设置文段预览(非实际)]";
                                rtbShowText.ForeColor = Color.Black;
                                double diff = frm.DiffDict.Calc(showText);
                                DiffcultyLabel.Text = frm.DiffDict.DiffText(diff);
                                btnGoSend.Enabled = true;
                            }
                            catch
                            {
                                rtbShowText.Text = "在当前标起始点下，字数设置超出限制！";
                                rtbShowText.ForeColor = Color.IndianRed;
                                DiffcultyLabel.Text = "难度";
                                btnGoSend.Enabled = false;
                            }
                        }
                        else
                        {
                            rtbShowText.Text = "发送字数设置错误！";
                            rtbShowText.ForeColor = Color.IndianRed;
                            DiffcultyLabel.Text = "难度";
                            btnGoSend.Enabled = false;
                        }
                    }
                    else
                    {
                        rtbShowText.Text = "发送字数设置错误！";
                        rtbShowText.ForeColor = Color.IndianRed;
                        DiffcultyLabel.Text = "难度";
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

        private void AutoNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 网络文章
        /// <summary>
        /// 网络文章的读取方法
        /// </summary>
        private void ReadWebArticle()
        {
            if (webRoot == null)
            {
                this.CleanListView();
                this.panel4.Refresh();
                this.currentDir = "src/";
                this.lastDir = "";
                listViewWebArticle.Columns[0].Text = this.currentDir;
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    YTWebRequest ytReq = new YTWebRequest(FullCurrentDirPath);
                    string strJson = ytReq.Request();
                    if (!string.IsNullOrEmpty(strJson))
                    {
                        webRoot = JsonConvert.DeserializeObject<WebArticle>(strJson);
                        this.allWebArticle = webRoot;
                        this.totalWebCount = allWebArticle.dir.Count + allWebArticle.txt.Count;
                        this.WebCountLabel.Text = this.totalWebCount.ToString();
                        if (this.totalWebCount > 0)
                        {
                            this.currentWebPage = 1;
                        }
                        else
                        {
                            this.currentWebPage = 0;
                        }
                        ShowWebListView();
                    }
                    else
                    {
                        this.ErrorListView();
                    }
                }));
            }
        }

        private void ReadWebDir(string dirPath)
        {
            this.BeginInvoke(new MethodInvoker(() => {
                YTWebRequest ytReq = new YTWebRequest(dirPath);
                string strJson = ytReq.Request();
                if (!string.IsNullOrEmpty(strJson))
                {
                    // 清除搜索文本
                    this.webSearchText = "";
                    this.WebSearchTextBox.Text = "";

                    this.allWebArticle = JsonConvert.DeserializeObject<WebArticle>(strJson);
                    this.totalWebCount = allWebArticle.dir.Count + allWebArticle.txt.Count;
                    this.WebCountLabel.Text = this.totalWebCount.ToString();
                    if (this.totalWebCount > 0)
                    {
                        this.currentWebPage = 1;
                    }
                    else
                    {
                        this.currentWebPage = 0;
                    }
                    ShowWebListView();
                }
                else
                {
                    this.ErrorListView();
                }
            }));
        }

        private void CleanListView(bool isRefresh = false)
        {
            this.listViewWebArticle.Items.Clear();
            this.listViewWebArticle.Items.Add(new ListViewItem(new String[] {"正在从网络中加载...", "", "", ""}));
            if (isRefresh)
            {
                this.listViewWebArticle.Refresh();
            }
        }

        private void ErrorListView()
        {
            this.listViewWebArticle.Items.Clear();
            this.listViewWebArticle.Items.Add(new ListViewItem(new String[] { "网络出错！", "", "", "错误" }));
        }

        private void ShowWebListView(int selected = -1)
        {
            this.listViewWebArticle.Items.Clear();
            this.WebPageLabel.Text = this.currentWebPage.ToString();
            if (this.currentWebPage > 0)
            {
                if (string.IsNullOrEmpty(this.webSearchText))
                {
                    this.currentWebArticle = GetWebArticle(this.allWebArticle, this.currentWebPage - 1);
                }
                else
                {
                    this.currentWebArticle = GetWebArticle(this.searchWebArticle, this.currentWebPage - 1);
                }

                foreach (string dirRow in this.currentWebArticle.dir)
                {
                    listViewWebArticle.Items.Add(new ListViewItem(new string[] { dirRow, "", "", "目录" }));
                }
                foreach (TxtFile fileRow in this.currentWebArticle.txt)
                {
                    listViewWebArticle.Items.Add(new ListViewItem(new string[] { fileRow.name, fileRow.count.ToString() + "字", fileRow.size, "文本" }));
                }

                if (selected > -1 && selected < listViewWebArticle.Items.Count)
                {
                    try
                    {
                        this.listViewWebArticle.TopItem = this.listViewWebArticle.Items[selected];
                        this.listViewWebArticle.Items[selected].Selected = true;
                    }
                    catch { }
                    
                }
            }
        }

        private void listViewWebArticle_ItemActivate(object sender, EventArgs e)
        {
            if (this.listViewWebArticle.SelectedItems.Count > 0)
            {
                string type = this.listViewWebArticle.SelectedItems[0].SubItems[3].Text;
                if (!string.IsNullOrEmpty(type))
                {
                    string name = this.listViewWebArticle.SelectedItems[0].SubItems[0].Text;
                    if (type == "目录")
                    {
                        this.lastDir = this.currentDir;
                        this.currentDir += name + "/";
                        listViewWebArticle.Columns[0].Text = this.currentDir;
                        this.CleanListView(true);
                        ReadWebDir(FullCurrentDirPath);
                    }
                    else if (type == "文本")
                    {
                        string textFilename = this.WebSourceRoot + this.currentDir + name;
                        this.WebFileTitleTextBox.Text = name.Replace(".txt", "");
                        this.rtbShowText.Text = "正在努力加载文章当中...";
                        this.rtbShowText.Refresh();
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            YTWebRequest ytReq = new YTWebRequest(textFilename);
                            string content = ytReq.Request();

                            if (!string.IsNullOrEmpty(content))
                            {
                                GetText = content;
                                ComText();
                            }
                            else
                            {
                                this.rtbShowText.Text = "警告：没有获取到文章内容！";
                                this.DiffcultyLabel.Text = "难度";
                            }
                        }));
                    }
                    else if (type == "错误")
                    {
                        //! 这里执行刷新操作
                        this.CleanListView(true);
                        ReadWebDir(FullCurrentDirPath);
                    }
                }
            }
        }

        private WebArticle GetWebArticle(WebArticle all, int start)
        {
            WebArticle wa = new WebArticle();
            int startIndex = start * WebPageSize;
            int endIndex = (start + 1) * WebPageSize;
            int dirUsed = 0;
            for (int i = startIndex; i < endIndex && i < this.totalWebCount; i++)
            {
                int dirCount = all.dir.Count;
                if (i < dirCount)
                {
                    wa.dir.Add(all.dir[i]);
                    dirUsed++;
                }
                else
                {
                    int index = i - dirUsed;
                    wa.txt.Add(all.txt[index]);
                }
            }

            return wa;
        }

        private void WebArticleFirstButton_Click(object sender, EventArgs e)
        {
            if (this.currentWebPage > 1)
            {
                this.currentWebPage = 1;
                this.ShowWebListView();
            }
        }

        private void WebArticlePreButton_Click(object sender, EventArgs e)
        {
            if (this.currentWebPage > 1)
            {
                this.currentWebPage--;
                this.ShowWebListView();
            }
        }

        private void WebArticleNextButton_Click(object sender, EventArgs e)
        {
            if (this.currentWebPage < this.WebTotalPage)
            {
                this.currentWebPage++;
                this.ShowWebListView();
            }
        }

        private void WebArticleLastButton_Click(object sender, EventArgs e)
        {
            if (this.currentWebPage < this.WebTotalPage)
            {
                this.currentWebPage = this.WebTotalPage;
                this.ShowWebListView();
            }
        }

        private void WebArticleBackButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FullLastDirPath))
            {
                string rPath = FullLastDirPath;

                //! 调整路径
                this.currentDir = this.lastDir;
                this.lastDir = DirReg.Replace(this.lastDir, "");
                listViewWebArticle.Columns[0].Text = this.currentDir;
                this.CleanListView(true);

                ReadWebDir(rPath);
            }
        }

        private void WebArticleRootButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.lastDir))
            { // 当前不是根目录时
                this.CleanListView(true);
                this.lastDir = "";
                this.currentDir = "src/";
                listViewWebArticle.Columns[0].Text = this.currentDir;

                ReadWebDir(FullCurrentDirPath);
            }
        }

        private void WebFileTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            lblTitle.Text = (sender as TextBox).Text.Trim();
            if (lblTitle.Text.Length == 0)
            {
                lblTitle.Text = "网络文章";
            }
        }

        private WebArticle GetSearchWebArticle()
        {
            WebArticle wa = new WebArticle();
            if (!string.IsNullOrEmpty(this.webSearchText))
            {
                for (int i = 0; i < this.allWebArticle.dir.Count; i++)
                {
                    if (this.allWebArticle.dir[i].Contains(this.webSearchText))
                    {
                        wa.dir.Add(this.allWebArticle.dir[i]);
                    }
                }

                for (int j = 0; j < this.allWebArticle.txt.Count; j++)
                {
                    if (this.allWebArticle.txt[j].name.Contains(this.webSearchText))
                    {
                        wa.txt.Add(this.allWebArticle.txt[j]);
                    }
                }
            }
            return wa;
        }

        private void WebSearchButton_Click(object sender, EventArgs e)
        {
            this.webSearchText = this.WebSearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(this.webSearchText))
            {
                this.searchWebArticle = GetSearchWebArticle();

                this.totalWebCount = searchWebArticle.dir.Count + searchWebArticle.txt.Count;
            }
            else
            {
                this.webSearchText = "";
                this.WebSearchTextBox.Text = "";

                this.totalWebCount = allWebArticle.dir.Count + allWebArticle.txt.Count;
            }

            this.WebCountLabel.Text = this.totalWebCount.ToString();
            if (this.totalWebCount > 0)
            {
                this.currentWebPage = 1;
            }
            else
            {
                this.currentWebPage = 0;
            }
            ShowWebListView();
        }

        private void WebSearchTextBox_Click(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.WebSearchButton.PerformClick();
            }
        }

        private void WebRandomButton_Click(object sender, EventArgs e)
        {
            Random rd = new Random();
            int maxValue = this.allWebArticle.dir.Count + this.allWebArticle.txt.Count;
            int index = rd.Next(maxValue);

            // 清除搜索文本
            this.webSearchText = "";
            this.WebSearchTextBox.Text = "";

            if (index < this.allWebArticle.dir.Count)
            { // 目录
                string dirName = this.allWebArticle.dir[index];
                this.lastDir = this.currentDir;
                this.currentDir += dirName + "/";
                listViewWebArticle.Columns[0].Text = this.currentDir;
                this.CleanListView(true);
                ReadWebDir(FullCurrentDirPath);
            }
            else
            { // 文本
                this.currentWebPage = index / WebPageSize + 1;
                this.ShowWebListView(index % WebPageSize);
                this.listViewWebArticle.Refresh();

                string name = this.allWebArticle.txt[index - this.allWebArticle.dir.Count].name;
                string textFilename = this.WebSourceRoot + this.currentDir + name;
                this.WebFileTitleTextBox.Text = name.Replace(".txt", "");
                this.rtbShowText.Text = "正在努力加载文章当中...";
                this.rtbShowText.Refresh();
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    YTWebRequest ytReq = new YTWebRequest(textFilename);
                    string content = ytReq.Request();

                    if (!string.IsNullOrEmpty(content))
                    {
                        GetText = content;
                        ComText();
                    }
                    else
                    {
                        this.rtbShowText.Text = "警告：没有获取到文章内容！";
                        this.DiffcultyLabel.Text = "难度";
                    }
                }));
            }
        }

        private void WebPageLabel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listViewWebArticle.Items.Count > 0)
                {
                    this.listViewWebArticle.TopItem = this.listViewWebArticle.Items[0];
                }
            }
            catch { }
        }
        #endregion

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
            }
            else
            {
                this.currentArticlePage = 0;
            }
            ShowSaveArticle();
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
                    listViewArticle.Items.Add(new ListViewItem(new string[] { dataRow["id"].ToString(), dataRow["title"].ToString(), dataRow["count"].ToString() + "字", dataRow["create_time"].ToString() }));
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
                        this.listViewArticle.SelectedItems[0].SubItems[2].Text = result.Length.ToString() + "字";
                        Glob.ArticleHistory.UpdateArticleContent(id, result);
                    }
                }
            }
        }

        private void ArticleSearchButton_Click(object sender, EventArgs e)
        {
            this.articleSearchText = this.ArticleSearchTextBox.Text.Trim();
            this.ReadSavedArticle();
        }

        private void ArticleSearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.ArticleSearchButton.PerformClick();
            }
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

        private void ArticlePageLabel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listViewArticle.Items.Count > 0)
                {
                    this.listViewArticle.TopItem = this.listViewArticle.Items[0];
                }
            }
            catch { }
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
            }
            else
            {
                this.currentSentPage = 0;
            }
            ShowSaveSent();
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

                    double diff = frm.DiffDict.Calc(GetText);
                    this.DiffcultyLabel.Text = frm.DiffDict.DiffText(diff);

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

                    if ((int)sd["auto_condition"] == 1)
                    {
                        NewSendText.AutoCondition = true;
                        AutoConditionCheckBox.Checked = true;
                    }
                    else
                    {
                        NewSendText.AutoCondition = false;
                        AutoConditionCheckBox.Checked = false;
                    }

                    NewSendText.AutoKey = (NewSendText.AutoKeyValue)int.Parse(sd["auto_key"].ToString());
                    AutoKeyComboBox.SelectedIndex = (int)NewSendText.AutoKey;

                    NewSendText.AutoOperator = (NewSendText.AutoOperatorValue)int.Parse(sd["auto_operator"].ToString());
                    AutoOperatorComboBox.SelectedIndex = (int)NewSendText.AutoOperator;

                    NewSendText.AutoNumber = (double)sd["auto_number"];
                    AutoNumberTextBox.Text = NewSendText.AutoNumber.ToString();

                    NewSendText.AutoNo = (NewSendText.AutoNoValue)int.Parse(sd["auto_no"].ToString());
                    AutoNoComboBox.SelectedIndex = (int)NewSendText.AutoNo;
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
            this.sentSearchText = this.SentSearchTextBox.Text.Trim();
            this.ReadSavedSent();
        }

        private void SentSearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.SentSearchButton.PerformClick();
            }
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

        private void SentPageLabel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listViewSent.Items.Count > 0)
                {
                    this.listViewSent.TopItem = this.listViewSent.Items[0];
                }
            }
            catch { }
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

        private void WebContentEditButton_Click(object sender, EventArgs e)
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
                    DiffcultyLabel.Text = "难度";
                    MessageBox.Show("未获取到文章！");
                    return;
                }
                string theTitle = lblTitle.Text.Trim();
                if (theTitle.Length == 0)
                {
                    rtbShowText.Text = "标题为空！";
                    DiffcultyLabel.Text = "难度";
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
                        DiffcultyLabel.Text = "难度";
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
                NewSendText.AutoCondition = AutoConditionCheckBox.Checked;
                NewSendText.AutoKey = (NewSendText.AutoKeyValue)AutoKeyComboBox.SelectedIndex;
                NewSendText.AutoOperator = (NewSendText.AutoOperatorValue)AutoOperatorComboBox.SelectedIndex;
                if (NewSendText.是否自动 && NewSendText.AutoCondition)
                {
                    string autoNumberText = AutoNumberTextBox.Text;
                    if (autoNumberText.Length == 0)
                    {
                        NewSendText.AutoNumber = 0;
                    }
                    else
                    {
                        double.TryParse(autoNumberText, out double aNum);
                        NewSendText.AutoNumber = aNum;
                    }
                }
                else
                {
                    NewSendText.AutoNumber = 0;
                }
                NewSendText.AutoNo = (NewSendText.AutoNoValue)AutoNoComboBox.SelectedIndex;

                NewSendText.ArticleSource = (NewSendText.ArticleSourceValue)tabControl1.SelectedIndex;
                // 勾选"保存文章"
                if ((NewSendText.ArticleSource == NewSendText.ArticleSourceValue.Local && this.FileSaveCheckBox.Checked) || (NewSendText.ArticleSource == NewSendText.ArticleSourceValue.Clipboard && this.ClipboardSaveCheckBox.Checked) || (NewSendText.ArticleSource == NewSendText.ArticleSourceValue.Web && this.WebSaveCheckBox.Checked))
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
                NewSendText.ArticleSource = (NewSendText.ArticleSourceValue)tabControl1.SelectedIndex;
                NewSendText.发文状态 = true;
                frm.SetMatch(false);
                frm.SendNextFun();
            }

            frm.发文状态ToolStripMenuItem.PerformClick(); // 模拟点击"发文"→"发文状态"菜单项，用于显示发文状态窗口
            this.Close();
        }
        #endregion

        private void EncodedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Ini t2 = new _Ini("config.ini");
            int temp = (sender as ComboBox).SelectedIndex;
            t2.IniWriteValue("发文面板配置", "文件编码", temp.ToString());
        }
    }
}
