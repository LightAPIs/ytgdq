using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions; //正则
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Drawing.Text;
//秒表
using IWshRuntimeLibrary;
using WindowsFormsApplication2.检查更新;
using WindowsFormsApplication2.编码提示;
using WindowsFormsApplication2.Storage;
using WindowsFormsApplication2.History;
using WindowsFormsApplication2.KeyAnalysis;
using WindowsFormsApplication2.CodeTable;
using WindowsFormsApplication2.Difficulty;
using WindowsFormsApplication2.SpeedGrade;
using WindowsFormsApplication2.DelayAction;
using Newtonsoft.Json;

namespace WindowsFormsApplication2
{
    public partial class Form1 : NewForm
    {
        /// <summary>
        /// 每次输入的字符数量
        /// </summary>
        public int[] HisSave = new int[2];
        public int[] HisLine = new int[2]; //调整滚动条

        /// <summary>
        /// 当前已输入的字数
        /// </summary>
        public int Sw = 0;
        /// <summary>
        /// 开关
        /// - 大于 0 时表示处于跟打中
        /// </summary>
        public int sw = 0;

        /// <summary>
        /// 开始跟打时间
        /// </summary>
        private DateTime startTime;
        /// <summary>
        /// 暂停结束后开始时间
        /// - 初始时与开始跟打时间一致
        /// - 若出现暂停，重新跟打后时间调整为重新启动时间
        /// </summary>
        private DateTime sTime;

        public double ts;
        private Series SeriesSpeed = new Series("速度");
        public ChartArea ChartArea1 = new ChartArea();
        public Title title1 = new Title();

        /// <summary>
        /// 键盘钩子
        /// </summary>
        private KeyBordHook KH = new KeyBordHook();

        /// <summary>
        /// 跟打时间的累计值
        /// </summary>
        private TimeSpan allUsedTime = new TimeSpan();
        /// <summary>
        /// 过往跟打时间的累加值
        /// - 用于处理跟打暂停的情况
        /// - 如果没有过暂停，那这个时间为空
        /// </summary>
        private TimeSpan recordUsedTime = new TimeSpan();

        private SendTextStatic 发文状态窗口;

        /// <summary>
        /// 测词委托
        /// </summary>
        /// <param name="_text"></param>
        private delegate void checkWordsDelegate();
        /// <summary>
        /// 开始测词委托
        /// </summary>
        private checkWordsDelegate 开始测词委托;

        /// <summary>
        /// 智能测词任务
        /// </summary>
        private Task checkWordTask;

        private DelayActionModel delayActionModel = new DelayActionModel();

        /// <summary>
        /// 判断是否为汉字或数字
        /// </summary>
        public static Regex IsCN = new Regex(@"[\u4e00-\u9fa5]|\d");

        /// <summary>
        /// 当前成绩数据
        /// </summary>
        private StorageDataSet.ScoreDataTable currentScoreData = new StorageDataSet.ScoreDataTable();

        /// <summary>
        /// 表格操作器
        /// </summary>
        private HistoryDataGridHandler gridHandler;

        /// <summary>
        /// 特殊字符替换字典
        /// </summary>
        private readonly static Dictionary<string, char> CharReDict = new Dictionary<string, char>
        {
            {
                "0x3008", '《'
            },
            {
                "0x3009", '》'
            },
            {
                "0xfe43", '『'
            },
            {
                "0xfe44", '』'
            },
            {
                "0xfe4f", '_'
            },
            {
                "0xffe5", '$'
            }
        };

        /// <summary>
        /// 难度计算字典
        /// </summary>
        public readonly DifficultyDict DiffDict = new DifficultyDict();

        /// <summary>
        /// 编码中的有效字符
        /// </summary>
        private readonly static string ValidChars = @"abcdefghizklmnopqrstuvwxyz0123456789;',./";

        private readonly static string SymbolChars = @"abcdefghizklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!！`~@#$￥%^…&*()（）-_—=+[]{}'‘’""“”\、|·;；:：,，.。<>《》?？/";

        public Form1()
        {
            InitializeComponent();
            int spX, spY;
            int spW, spH;

            spX = int.TryParse(IniRead("窗口位置", "横", "200"), out spX) ? spX < 0 ? 200 : spX : 200;
            spY = int.TryParse(IniRead("窗口位置", "纵", "200"), out spY) ? spY < 0 ? 200 : spY : 200;
            spW = int.TryParse(IniRead("窗口位置", "宽", "1280"), out spW) ? spW < 200 ? 520 : spW : 520;
            spH = int.TryParse(IniRead("窗口位置", "高", "480"), out spH) ? spH < 50 ? 480 : spH : 480;
            Point pos = new Point(spX, spY);
            this.Location = pos;
            this.Size = new Size(spW, spH);

            this.toolStripButton4.Checked = !bool.TryParse(IniRead("程序控制", "详细信息", "True"), out bool t4) || t4;
            int p11H = int.TryParse(IniRead("拖动条", "高1", "142"), out p11H) ? p11H : 142;
            int p31H = int.TryParse(IniRead("拖动条", "高2", "89"), out p31H) ? p31H : 89;
            this.splitContainer1.SplitterDistance = p11H;
            this.splitContainer3.SplitterDistance = p31H;

            //* 快捷键处理
            this.HotKeyHandler();

            this.UIThread(LoadSetup);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //改变窗口标题
            this.Text = Glob.Form;
            Glob.Text = richTextBox1.Text;

            this.textBoxEx1.Select();
            try
            {
                KH.Start();
                KH.OnKeyDownEvent += new KeyEventHandler(KH_OnKeyDownEvent);
            }
            catch { this.lbl键准.Text = "NA"; }
            this.textBoxEx1.LostFocus += new System.EventHandler(textBoxEx1_LostFocus);

            //载入主题
            GetTheme();
            if (Theme.IsBackBmp)
            { //* 启用主题背景图
                LoadTheme(Theme.ThemeBackBmp, Theme.ThemeColorBG, Theme.ThemeColorFC, Theme.ThemeBG);
            }
            else
            { //* 纯色
                LoadTheme("纯色", Theme.ThemeColorBG, Theme.ThemeColorFC, Theme.ThemeBG);
            }

            // 注册表格操作器
            this.gridHandler = new HistoryDataGridHandler(this.dataGridView1);

            //* 数据库初始化
            Glob.ScoreHistory = new ScoreData("score");
            Glob.ScoreHistory.Init();
            Glob.ArticleHistory = new ArticleData("article");
            Glob.ArticleHistory.Init();
            Glob.SentHistory = new SentData("sent");
            Glob.SentHistory.Init();
            Glob.CodeHistory = new CodeData("code");
            Glob.CodeHistory.Init();

            //* 码表处理
            Glob.UsedTableIndex = Glob.CodeHistory.GetUsedTableIndex();
            Glob.WordMaxLen = Glob.CodeHistory.GetMaxLen();
            Glob.WordLenType = Glob.CodeHistory.GetLenType();
            if (!string.IsNullOrEmpty(Glob.UsedTableIndex))
            { //! 此处读取 4 个基本码表字典
                Task t1 = new Task(() =>
                {
                    CodeTableBox.DefaultCodeTableHandler(Glob.UsedTableIndex);
                });
                t1.Start();
            }
        }


        /// <summary>
        /// 快捷键处理
        /// </summary>
        public void HotKeyHandler()
        {
            //* 读取快捷键列表
            for (int i = 0; i < Glob.HotKeyList.Count; i++)
            {
                string hk = IniRead("快捷键", Glob.HotKeyList[i].GetId(), Glob.HotKeyList[i].GetDefaultKeys());
                if (hk == "None")
                {
                    Glob.HotKeyList[i].SetKeys("None");
                }
                else
                {
                    bool dup = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (Glob.HotKeyList[j].GetKeys() == hk)
                        {
                            dup = true;
                            break;
                        }
                    }
                    if (!dup)
                    {
                        Glob.HotKeyList[i].SetKeys(hk);
                    }
                }

                Keys hotK = Glob.HotKeyList[i].TransKeyCode();
                switch (Glob.HotKeyList[i].GetId())
                {
                    case "设置":
                        this.设置ToolStripMenuItem1.ShortcutKeys = hotK;
                        break;
                    case "发文":
                        this.新发文ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "重打":
                        this.重打ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "暂停":
                        this.暂停ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "复制当前文段":
                        this.复制跟打的文段ToolStripMenuItem1.ShortcutKeys = hotK;
                        break;
                    case "复制上次成绩":
                        this.上一次成绩ToolStripMenuItem1.ShortcutKeys = hotK;
                        break;
                    case "复制图片成绩":
                        this.复制图片成绩ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "发上一段":
                        this.SendPreToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "发下一段":
                        this.发下一段ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "速度分析":
                        this.跟打分析ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "跟打报告":
                        this.跟打报告ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "按键统计":
                        this.KeyAnToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "理论按键统计":
                        this.CalcKeysToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "历史记录":
                        this.HistoryToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "保存发文配置":
                        this.SaveSendToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "乱序重打":
                        this.DisorderToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "停止发文":
                        this.停止发文ToolStripMenuItem1.ShortcutKeys = hotK;
                        break;
                    case "查询当前编码":
                        this.查询当前编码ToolStripMenuItem2.ShortcutKeys = hotK;
                        break;
                    case "打开练习":
                        this.DrillToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "测速数据":
                        this.测速数据ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "窗口复位":
                        this.窗口复位ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "检验真伪":
                        this.检验真伪ToolStripMenuItem.ShortcutKeys = hotK;
                        break;
                    case "直接载文":
                        this.从剪切板ToolStripMenuItem1.ShortcutKeys = hotK;
                        break;
                    case "格式载文":
                        this.FormatLoadToolStripMenuItem1.ShortcutKeys = hotK;
                        break;
                    case "老板键":
                        if (Glob.HotKeyList[i].GetKeys() == "None")
                        {
                            this.老板键ToolStripMenuItem1.ShortcutKeyDisplayString = "";
                        }
                        else
                        {
                            //！ 注册全局热键
                            this.老板键ToolStripMenuItem1.ShortcutKeyDisplayString = Glob.HotKeyList[i].GetKeys();
                            RegisterHotKey(this.Handle, 100, Glob.HotKeyList[i].TransKeyModifiers(), Glob.HotKeyList[i].TransOnlyKeyCode());
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// 重新注册全局老板键
        /// 用于手动关闭设置窗口时
        /// </summary>
        public void ReRegisterBossKey()
        {
            if (Glob.HotKeyList.Last().GetKeys() != "None")
            {
                RegisterHotKey(this.Handle, 100, Glob.HotKeyList.Last().TransKeyModifiers(), Glob.HotKeyList.Last().TransOnlyKeyCode());
            }
        }


        /// <summary>
        /// 全局热键处理
        /// </summary>
        /// <param name="m"></param>
        private void ProcessHotKey(int id)
        {
            string sid = id.ToString();
            switch (sid)
            {
                case "100": // 老板键
                    if (this.Visible)
                    {
                        this.Hide();
                        if (this.发文状态窗口 != null && !this.发文状态窗口.IsDisposed && this.发文状态窗口.Visible)
                        {
                            this.发文状态窗口.Hide();
                        }
                        this.notifyIcon1.Visible = false;
                    }
                    else
                    {
                        this.Show();
                        if (this.WindowState == FormWindowState.Minimized)
                        {
                            this.WindowState = FormWindowState.Normal;
                        }
                        if (this.发文状态窗口 != null && !this.发文状态窗口.IsDisposed)
                        {
                            this.发文状态窗口.Show(this);
                        }
                        this.notifyIcon1.Visible = true;
                        this.Activate();
                    }
                    break;
            }
        }

        /// <summary>
        /// 键盘钩子按键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KH_OnKeyDownEvent(object sender, KeyEventArgs e)
        { //! 键盘钩子的触发是在跟打区 KeyDown 事件之前
            if (sw != 0 && this.textBoxEx1.Focused)
            {
                Glob.TextMc++; //计数 用于计量回车及回车产生前的量

                int k = e.KeyValue;
                if (k == 8)
                {
                    Glob.TextBg++;
                }
                else if (k == 13)
                {
                    Glob.回车++;
                    //触发回车时 计算
                    跟打地图步进++;
                    Type_Map(Color.HotPink, 跟打地图步进, 1);
                    Glob.TextMcc += Glob.TextMc;
                    Glob.TextMc = 0;

                }

                //! 统计具体按键
                if (KeyObj.KeysDic.ContainsKey(e.KeyCode))
                {
                    int index = KeyObj.KeysDic[e.KeyCode];
                    Glob.KeysTotal[index]++;
                    Glob.HistoryKeysTotal[index]++;
                }
            }

            Glob.TheKeyValue = e.KeyValue;
        }

        /// <summary>
        /// 获取主题数据
        /// </summary>
        public void GetTheme()
        {
            _Ini ini = new _Ini("config.ini");
            Theme.IsBackBmp = bool.Parse(ini.IniReadValue("主题", "是否应用主题背景", "False"));
            Theme.ThemeBackBmp = ini.IniReadValue("主题", "背景路径", "程序默认");
            Theme.ThemeBG = Color.FromArgb(int.Parse(ini.IniReadValue("主题", "纯色", "-13089719"))); // #384449
            Theme.ThemeColorBG = Color.FromArgb(int.Parse(ini.IniReadValue("主题", "主题颜色", "-13089719"))); // #384449
            Theme.ThemeColorFC = Color.FromArgb(int.Parse(ini.IniReadValue("主题", "字体颜色", "-1"))); // #FFFFFF
        }

        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="BGround">背景设置的路径</param>
        /// <param name="BG">背景色</param>
        /// <param name="FC">前景色</param>
        /// <param name="BGr">纯色</param>
        public void LoadTheme(string BGround, Color BG, Color FC, Color BGr)
        {
            //载入图标或者颜色
            if (BGround != "")
            {
                if (BGround == "纯色")
                {
                    this.BackColor = BGr;
                    this.BackgroundImage = null;
                }
                else if (BGround == "程序默认")
                {
                    this.BackColor = Theme.ThemeBG;
                    try
                    {
                        this.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsFormsApplication2.Resources.bg.jpg"));
                    }
                    catch
                    {
                        this.BackgroundImage = null;
                        ShowFlowText("未知错误，已采用纯色设置!");
                    }
                }
                else
                { // 自定义图片背景
                    try
                    {
                        string picPath = Path.Combine(FormTheme.ThemeFolderName, BGround);
                        if (System.IO.File.Exists(picPath))
                        {
                            this.BackgroundImage = Image.FromFile(picPath);
                        }
                        else
                        {
                            this.BackgroundImage = null;
                        }
                    }
                    catch
                    {
                        this.BackgroundImage = null;
                    }
                }
            }
            else
            {
                this.BackColor = Theme.ThemeBG;
                try
                {
                    this.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsFormsApplication2.Resources.bg.jpg"));
                }
                catch
                {
                    this.BackgroundImage = null;
                }
            }

            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            // this.mS1.ThemeColor = BG;
            // this.mS1.BackColor = BG;
            this.mS1.ForeColor = FC;
            this.dataGridView2.GridColor = BG;
            //this.toolStrip1.BackColor = BG;
            this.splitContainer1.BackColor = BG;
            this.splitContainer2.BackColor = BG;
            this.splitContainer3.BackColor = BG;
            this.splitContainer4.BackColor = BG;
            this.tableLayoutPanel2.BackColor = BG;
            this.tableLayoutPanel1.BackColor = FC;
            this.lblDuan.BackColor = BG;
            this.lblTitle.BackColor = BG;
            this.lblCount.BackColor = BG;
            this.DifficultyLabel.BackColor = BG;
            this.lblSpeedText.BackColor = BG;
            this.lblJJText.BackColor = BG;
            this.lblMCText.BackColor = BG;
            this.lblMatchCount.BackColor = BG;

            this.dataGridView1.Rows[0].DefaultCellStyle.BackColor = BG;
            this.dataGridView1.Rows[0].DefaultCellStyle.ForeColor = FC;

            this.lblDuan.ForeColor = FC;
            this.lblTitle.ForeColor = FC;
            this.lblCount.ForeColor = FC;
            this.DifficultyLabel.ForeColor = FC;
            this.lblSpeedText.ForeColor = FC;
            this.lblJJText.ForeColor = FC;
            this.lblMCText.ForeColor = FC;
            this.lblMatchCount.ForeColor = FC;

            this.TSMI1.ForeColor = FC;
            this.TSMI3.ForeColor = FC;
            this.TSMI4.ForeColor = FC;
            this.TSMI5.ForeColor = FC;

            //击键评定
            this.labelJiCheck.ForeColor = FC;
            this.labelCheckUD.ForeColor = FC;
            this.labelmcing.ForeColor = FC;

            Color C = Color.FromArgb(ColorTran(BG.R), ColorTran(BG.G), ColorTran(BG.B));
            this.labelSpeeding.BackColor = C;
            this.labelJjing.BackColor = C;
            this.labelJiCheck.BackColor = C;
            this.labelCheckUD.BackColor = C;
            this.labelmcing.BackColor = C;
            this.ForeColor = FC;
            Rectangle rect = new Rectangle(0, 0, 220, 24);
            this.Invalidate(rect, true);

        }
        private int ColorTran(int c)
        {
            if (c + 20 > 255)
                return 255;
            else return c + 20;
        }
        public void LoadSetup()
        {
            //创建表头
            this.dataGridView1.Rows.Add("序", "时间", "段号", "速度", "击键", "码长", "理论", "难度", "评级", "回改", "退格", "回车", "选重", "错字", "回改率", "键准", "效率", "键数", "字数", "打词", "打词率", "用时", "标题");
            this.dataGridView1.Rows[0].Frozen = true;
            this.dataGridView1.Rows[0].DefaultCellStyle.Font = new Font("微软雅黑", 11f);
            this.dataGridView1.Rows[0].DefaultCellStyle.BackColor = Theme.ThemeColorBG;
            this.dataGridView1.Rows[0].DefaultCellStyle.ForeColor = Theme.ThemeColorFC;
            this.dataGridView1.Rows[0].Height = 20;
            //跟打地图
            Bitmap bmp_ = new Bitmap(this.picMap.ClientRectangle.Width, this.picMap.ClientRectangle.Height);
            this.picMap.Image = bmp_;
            //tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null);
            this.chartSpeed.ChartAreas.Add(ChartArea1);
            this.ChartArea1.BackColor = Color.FromArgb(150, 150, 150);
            this.ChartArea1.AxisX.LineColor = Color.White;
            this.ChartArea1.AxisX.MajorGrid.LineColor = Color.FromArgb(127, 127, 127);
            this.ChartArea1.AxisX.MajorTickMark.LineColor = Color.FromArgb(10, 10, 35);
            this.ChartArea1.AxisX.LabelStyle.ForeColor = Color.Black;
            this.ChartArea1.AxisX2.LineDashStyle = ChartDashStyle.Dash;

            this.ChartArea1.AxisY.MajorGrid.LineColor = Color.FromArgb(127, 127, 127);
            this.ChartArea1.AxisY.MajorTickMark.LineColor = Color.Black;
            this.ChartArea1.AxisY.LabelStyle.ForeColor = Color.Black;
            this.ChartArea1.AxisY.LineColor = Color.White;

            Type type = dataGridView1.GetType();
            PropertyInfo pi = type.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView1, true, null);

            tableLayoutPanel2.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel2, true, null);
            this.ChartArea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            this.ChartArea1.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            this.ChartArea1.AxisX.LabelAutoFitMaxFontSize = 7;
            this.ChartArea1.AxisY.LabelAutoFitMaxFontSize = 7;
            //? X 轴从 1 开始显示
            this.ChartArea1.AxisX.Minimum = 1.00D;

            this.chartSpeed.Titles.Add(title1);
            this.title1.ForeColor = Color.Black;
            this.title1.Font = new Font("Verdana", 8.25f);

            this.chartSpeed.Series.Add(SeriesSpeed); //增加图表
            this.SeriesSpeed.ChartType = SeriesChartType.SplineArea;
            this.SeriesSpeed.BorderWidth = 2;
            this.SeriesSpeed.Color = Color.White;
            this.SeriesSpeed.BackSecondaryColor = Color.Black;

            this.richTextBox1.ForeColor = Color.Black;

            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//列标题居中显示
            this.dataGridView1.ForeColor = Color.DarkSlateGray;
            for (int i = 0; i <= 14; i += 2)
            {
                this.dataGridView2.Rows[0].Cells[i].Value = 4 + i / 2;
                this.dataGridView2.Rows[0].Cells[i].Style.BackColor = Color.FromArgb(217, 217, 217);
            }
            for (int i = 0; i < 9; i++)
            {
                Glob.jjPer[i] = int.Parse(IniRead("记录", i.ToString(), "0"));
            }
            Glob.jjAllC = int.TryParse(IniRead("记录", "总数", "0"), out Glob.jjAllC) ? Glob.jjAllC : 0;
            jjPerCheck(0);
            this.dataGridView2.Rows[0].Cells[16].Value = "12+";
            this.dataGridView2.Rows[0].Cells[16].Style.BackColor = Color.FromArgb(219, 219, 219);

            //* 载入颜色设置
            Glob.R1Back = Color.FromArgb(int.Parse(IniRead("外观", "对照区颜色", "-722948"))); // #F4F7FC
            richTextBox1.BackColor = Glob.R1Back;
            Glob.R2Back = Color.FromArgb(int.Parse(IniRead("外观", "跟打区颜色", "-722948"))); // #F4F7FC
            textBoxEx1.BackColor = Glob.R2Back;

            Glob.R1Color = Color.FromArgb(int.Parse(IniRead("外观", "对照区文字色", "-16777216"))); // #000000
            richTextBox1.ForeColor = Glob.R1Color;
            Glob.R2Color = Color.FromArgb(int.Parse(IniRead("外观", "跟打区文字色", "-16777216"))); // #000000
            textBoxEx1.ForeColor = Glob.R2Color;

            Glob.RightBGColor = Color.FromArgb(int.Parse(IniRead("外观", "打对颜色", "-8355712"))); // #808080
            Glob.FalseBGColor = Color.FromArgb(int.Parse(IniRead("外观", "打错颜色", "-38294"))); // #FF6A6A
            Glob.BackChangeColor = Color.FromArgb(int.Parse(IniRead("外观", "回改颜色", "-5374161"))); // #ADFF2F
            Glob.TimeLongColor = Color.FromArgb(int.Parse(IniRead("外观", "用时背景色", "-6632142"))); // #9ACD32

            Glob.Words0Color = Color.FromArgb(int.Parse(IniRead("外观", "词组0重色", "-16776961"))); // #0000FF
            Glob.Words1Color = Color.FromArgb(int.Parse(IniRead("外观", "词组1重色", "-65536"))); // #FF0000
            Glob.Words2Color = Color.FromArgb(int.Parse(IniRead("外观", "词组2重色", "-8388480"))); // #800080
            Glob.Words3Color = Color.FromArgb(int.Parse(IniRead("外观", "词组3重色", "-60269"))); // #FF1493

            Glob.TestMarkColor = Color.FromArgb(int.Parse(IniRead("外观", "测速点颜色", "-2894893"))); // #D3D3D3

            //字体
            FontConverter fc = new FontConverter();
            Glob.Font_1 = (Font)fc.ConvertFromString(IniRead("外观", "对照区字体", "宋体, 21.75pt"));
            Glob.Font_2 = (Font)fc.ConvertFromString(IniRead("外观", "跟打区字体", "宋体, 12pt"));
            richTextBox1.ForeColor = Color.Black;
            textBoxEx1.Font = Glob.Font_2;
            this.richTextBox1.FontChanged += new EventHandler(richTextBox1_FontChanged);
            richTextBox1.Font = Glob.Font_1;

            //载入个签
            Glob.InstraPre = IniRead("个签", "签名", "");
            Glob.InstraPre_ = IniRead("个签", "标志", "0");
            //载入输入法签名
            Glob.InstraSrf = IniRead("输入法", "签名", "");
            Glob.InstraSrf_ = IniRead("输入法", "标志", "0");

            //获取发送成绩的排序顺序
            Glob.sortSend = IniRead("发送", "顺序", "ABCVDTSEFULGNORQ");

            //载入前导
            Glob.IsZdyPre = bool.Parse(IniRead("载入", "开启", "False"));
            if (Glob.IsZdyPre)
            {
                Glob.PreText = IniRead("载入", "前导", "-----");
                Glob.PreDuan = IniRead("载入", "段标", "第xx段");
            }
            else
            {
                Glob.PreText = "-----";
                Glob.PreDuan = "第xx段";
            }

            Glob.TextHgAll = int.Parse(IniRead("记录", "总回改", "0"));

            //今日跟打
            _Ini iniSetup = new _Ini("config.ini");
            // 记录天数
            Glob.TextRecDays = int.Parse(IniRead("记录", "记录天数", "1"));

            ArrayList a = ReadKeys("今日跟打");
            if (a.Count > 0)
            {
                Glob.TodayDate = a[0].ToString();
            }
            else
                Glob.TodayDate = DateTime.Now.ToShortDateString();

            if (Glob.TodayDate != DateTime.Now.ToShortDateString())
            {
                iniSetup.IniWriteValue("今日跟打", Glob.TodayDate, null);
                Glob.TodayDate = DateTime.Now.ToShortDateString();
                Glob.TextRecDays++;//记录天数自增
            }
            Glob.todayTyping = int.Parse(IniRead("今日跟打", DateTime.Today.ToShortDateString(), "0"));
            Glob.TextLenAll = int.Parse(IniRead("记录", "总字数", "0"));

            //* 记录开始时的总按键统计
            string readKeysTotalStr = IniRead("记录", "总按键", "");
            if (!string.IsNullOrEmpty(readKeysTotalStr))
            {
                try
                {
                    int[] readKeysTotal = Array.ConvertAll(readKeysTotalStr.Split('|'), s => int.Parse(s));
                    if (readKeysTotal.Length == 50)
                    {
                        Glob.HistoryKeysTotal = readKeysTotal;
                    }
                }
                catch
                {
                    Array.Clear(Glob.HistoryKeysTotal, 0, 50);
                }
            }

            lblMatchCount.Text = Validation.Validat(Validation.Validat(richTextBox1.Text));
            labelHaveTyping.Text = Glob.todayTyping + "/" + 字数格式化(Glob.TextLenAll) + "/" + Glob.TextRecDays + "天/" + 字数格式化(Glob.TextLenAll + Glob.TextHgAll);

            //曲线
            Glob.isShowSpline = bool.Parse(iniSetup.IniReadValue("拖动条", "曲线", "False"));
            this.splitContainer4.Panel1Collapsed = Glob.isShowSpline;
            this.tbnSpline.Checked = !Glob.isShowSpline;
            //停止用时
            int stopTime = int.Parse(IniRead("控制", "停止", "1"));
            if (stopTime < 1 || stopTime > 10)
            {
                stopTime = 1;
            }
            Glob.StopUseTime = stopTime;
            this.toolTip1.SetToolTip(this.lblAutoReType, "跟打发呆时间，超过" + Glob.StopUseTime + "分钟时自动重打");
            //极简设置
            Glob.SimpleMoudle = bool.Parse(IniRead("发送", "极简状态", "False"));
            Glob.SimpleSplite = IniRead("发送", "分隔符", "|");
            //自动替换
            Glob.autoReplaceBiaodian = bool.Parse(IniRead("程序控制", "自动替换", "False"));
            this.toolStripButton1.Checked = Glob.autoReplaceBiaodian;

            // 显示实时数据
            Glob.ShowRealTimeData = bool.Parse(IniRead("控制", "实时数据", "True"));
            this.RealTimeData.Checked = Glob.ShowRealTimeData;

            // 自动复制
            Glob.AutoCopy = bool.Parse(IniRead("控制", "自动复制", "False"));
            this.toolStripButton3.Checked = Glob.AutoCopy;
            //不保存高阶
            Glob.DisableSaveAdvanced = bool.Parse(IniRead("控制", "不保存高阶", "False"));
            // 顶功输入法
            Glob.UseDGInput = bool.Parse(IniRead("控制", "顶功输入", "False"));
            // 四码唯一自动上屏
            Glob.UseAutoInput = bool.Parse(IniRead("控制", "四码唯一", "False"));
            // 符号选重
            Glob.UseSymbolSelect = bool.Parse(IniRead("控制", "符号选重", "False"));
            // 词组不统计符号上屏
            Glob.NotSymbolInput = bool.Parse(IniRead("控制", "不统计符号", "False"));
            // Z 键复打
            Glob.UseZRetype = bool.Parse(IniRead("控制", "Z键复打", "False"));

            // 编码提示
            this.picBmTips.Checked = bool.Parse(IniRead("程序控制", "编码", "False"));
            // 智能测词
            Glob.是否智能测词 = bool.Parse(IniRead("程序控制", "智能测词", "False"));
            this.CheckWordToolButton.Checked = Glob.是否智能测词;
            // 标记功能
            Glob.IsPointIt = bool.Parse(IniRead("程序控制", "标记", "False"));
            this.tsb标注.Checked = Glob.IsPointIt;

            // 图片发送
            Glob.PicName = IniRead("发送", "昵称", "");

            // 速度评级部分
            Glob.SpeedGradeCount = int.Parse(IniRead("评级", "段数", "0"));
            Glob.SpeedGradeSpeed = double.Parse(IniRead("评级", "速度", "0"));
            Glob.SpeedGradeDiff = double.Parse(IniRead("评级", "难度", "0"));
            Glob.SpeedGrade = double.Parse(IniRead("评级", "结果", "0"));

            this.比赛时自动打开寻找测速点ToolStripMenuItem.Checked = bool.Parse(IniRead("程序控制", "自动打开寻找", "False"));
            LblHaveTypingChange();

            GetInfo(); //获取文段信息
        }

        /// <summary>
        /// 已跟打数据的改变
        /// </summary>
        private void LblHaveTypingChange()
        {
            this.UIThread(() => toolTip1.SetToolTip(labelHaveTyping, "今日跟打：" + Glob.todayTyping + "字\n" +
                                                                     "总计跟打：" + Glob.TextLenAll + "字\n" +
                                                                     "跟打段数：" + Glob.jjAllC + "段\n" +
                                                                     "记录天数：" + Glob.TextRecDays + "天\n" +
                                                                     "记录字数：" + (Glob.TextLenAll + Glob.TextHgAll).ToString() + "字\n" +
                                                                     "平均每天：" + ((Glob.TextLenAll + Glob.TextHgAll) / Glob.TextRecDays).ToString("0.00") + "字"));
        }


        void richTextBox1_FontChanged(object sender, EventArgs e)
        {
            Glob.oneH = (int)this.richTextBox1.Font.GetHeight() + 4;

            if (Glob.IsPointIt)
            {
                delayActionModel.Debounce(100, this, new Action(() =>
                {
                    this.richTextBox1.Render(Glob.BmAlls, Glob.RightBGColor);
                }));
            }
        }

        #region dll
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id); // 取消全局热键

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, Keys vk); // 注册全局热键

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(System.IntPtr ptr, int wMsg, int wParam, int lParam);
        #endregion

        #region HookKey
        public class KeyBordHook
        {
            private const int WM_KEYDOWN = 0x100;
            private const int WM_KEYUP = 0x101;
            private const int WM_SYSKEYDOWN = 0x104;
            private const int WM_SYSKEYUP = 0x105;

            //全局的事件 
            public event KeyEventHandler OnKeyDownEvent;
            public event KeyEventHandler OnKeyUpEvent;
            public event KeyPressEventHandler OnKeyPressEvent;
            static int hKeyboardHook = 0;   //键盘钩子句柄 
            //鼠标常量 
            public const int WH_KEYBOARD_LL = 13;   //keyboard   hook   constant   
            HookProc KeyboardHookProcedure;   //声明键盘钩子事件类型. 
            //声明键盘钩子的封送结构类型 
            [StructLayout(LayoutKind.Sequential)]
            public class KeyboardHookStruct
            {
                public int vkCode;   //表示一个在1到254间的虚似键盘码 
                public int scanCode;   //表示硬件扫描码 
                public int flags;
                public int time;
                public int dwExtraInfo;
            }
            //装置钩子的函数 
            [DllImport("user32.dll ", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
            //卸下钩子的函数 
            [DllImport("user32.dll ", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern bool UnhookWindowsHookEx(int idHook);

            //下一个钩挂的函数 
            [DllImport("user32.dll ", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);
            [DllImport("user32 ")]
            public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);
            [DllImport("user32 ")]
            public static extern int GetKeyboardState(byte[] pbKeyState);
            public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
            ///   <summary> 
            ///   默认的构造函数构造当前类的实例并自动的运行起来. 
            ///   </summary> 
            public KeyBordHook()
            {
                Start();
            }
            //析构函数. 
            ~KeyBordHook()
            {
                Stop();
            }
            public void Start()
            {
                //! 安装键盘钩子
                if (hKeyboardHook == 0)
                {
                    KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                    hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule), 0);
                    if (hKeyboardHook == 0)
                    {
                        Stop();
                        //throw new Exception("SetWindowsHookEx   ist   failed. ");
                    }
                }
            }
            public void Stop()
            {
                bool retKeyboard = true;

                if (hKeyboardHook != 0)
                {
                    retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                    hKeyboardHook = 0;
                }
                //如果卸下钩子失败 
                if (!(retKeyboard)) throw new Exception("UnhookWindowsHookEx   failed. ");
            }
            private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
            {
                if ((nCode >= 0) && (OnKeyDownEvent != null || OnKeyUpEvent != null || OnKeyPressEvent != null))
                {
                    KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                    //引发OnKeyDownEvent 
                    if (OnKeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                        KeyEventArgs e = new KeyEventArgs(keyData);
                        OnKeyDownEvent(this, e);
                    }

                    //引发OnKeyPressEvent 
                    if (OnKeyPressEvent != null && wParam == WM_KEYDOWN)
                    {
                        byte[] keyState = new byte[256];
                        GetKeyboardState(keyState);
                        byte[] inBuffer = new byte[2];
                        if (ToAscii(MyKeyboardHookStruct.vkCode,
                            MyKeyboardHookStruct.scanCode,
                            keyState,
                            inBuffer,
                            MyKeyboardHookStruct.flags) == 1)
                        {
                            KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                            OnKeyPressEvent(this, e);
                        }
                    }

                    //引发OnKeyUpEvent 
                    if (OnKeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                    {
                        Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                        KeyEventArgs e = new KeyEventArgs(keyData);
                        OnKeyUpEvent(this, e);
                    }
                }
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
            }
        }

        #endregion

        #region 新发文
        /// <summary>
        /// 正式发文前的数据清理
        /// </summary>
        /// <param name="textAll">跟打区的文本内容</param>
        private void CleanBeforeSending(string textAll)
        {
            this.textBoxEx1.Clear(); // 清空跟打区
            this.richTextBox1.SelectAll();
            this.richTextBox1.SelectionBackColor = Glob.R1Back;
            this.richTextBox1.Text = textAll;
            Initialize(1);
            Initialize(2);
            this.textBoxEx1.ReadOnly = false; // 激活跟打区
            this.textBoxEx1.Select();
            Glob.CurSegmentNum++;
            this.lblDuan.Text = "第" + Glob.CurSegmentNum.ToString() + "段";
            GetInfo(); // 获取信息
            NewSendText.已发段数++;
        }

        /// <summary>
        /// 倒序发文前的数据清理
        /// </summary>
        /// <param name="textAll"></param>
        private void CleanBeforeReverseSending(string textAll)
        {
            this.textBoxEx1.Clear(); // 清空跟打区
            this.richTextBox1.SelectAll();
            this.richTextBox1.SelectionBackColor = Glob.R1Back;
            this.richTextBox1.Text = textAll;
            Initialize(1);
            Initialize(2);
            this.textBoxEx1.ReadOnly = false; // 激活跟打区
            this.textBoxEx1.Select();
            Glob.CurSegmentNum--;
            if (Glob.CurSegmentNum < 1)
            {
                Glob.CurSegmentNum = 1;
            }
            this.lblDuan.Text = "第" + Glob.CurSegmentNum.ToString() + "段";
            GetInfo(); // 获取信息
            NewSendText.已发段数--;
        }

        /// <summary>
        /// 正常发文
        /// </summary>
        public void SendAOnce()
        {
            this.textBoxEx1.TextChanged -= new System.EventHandler(textBoxEx1_TextChanged);
            if (NewSendText.发文状态)
            {
                this.CleanSpeedPoints();
                this.cmsDuanList.Items.Clear();
                //输入法状态
                Glob.binput = true;
                string TextAll = ""; // 要发送的文字
                int TextLen = NewSendText.发文全文.Length;
                this.lblTitle.Text = NewSendText.标题;
                if (NewSendText.类型 == "单字")
                {
                    if (NewSendText.是否乱序)
                    { //* 单字乱序
                        //? 此时 TextLen 指示剩余字数
                        int[] numlist;
                        if (NewSendText.乱序全段不重复)
                        { //* 全段不重复
                            if (TextLen > 0)
                            { // 剩余字数大于零时
                                if (TextLen < NewSendText.字数)
                                { // 剩余字数小于需要发送的字数
                                    numlist = GetRandomUnrepeatArray(0, TextLen - 1, TextLen);
                                }
                                else
                                { // 剩余字数大于需要发送的字数时
                                    numlist = GetRandomUnrepeatArray(0, TextLen - 1, NewSendText.字数);
                                }

                                //? 由于 String.Replace 方法是替换所有字符，当原文中存在重复的文字时，会被一并替换掉
                                //? 所以换用 StringBuilder 对象来处理
                                StringBuilder tempSb = new StringBuilder(NewSendText.发文全文);
                                foreach (int item in numlist)
                                {
                                    TextAll += NewSendText.发文全文[item];
                                    tempSb.Replace(NewSendText.发文全文[item], ' ', item, 1); // 将已发送的单个文字从全文当中剔除
                                }

                                NewSendText.发文全文 = tempSb.ToString().Replace(" ", "");
                                NewSendText.标记 += numlist.Length;
                                CleanBeforeSending(TextAll);
                                //* 缓存文段内容
                                Glob.TempSegmentRecord.Add(TextAll);
                            }
                            else
                            { // 剩余字数为零时
                                ShowFlowText("文章全文已发送完毕，请重新换文！"); //* 仅弹出提示，不会自动停止发文
                                NewSendText.标记 = 0;
                                NewSendText.发文全文 = NewSendText.文章全文;
                            }
                        }
                        else
                        { //* 乱序无限
                            int sendNum = NewSendText.字数 > TextLen ? TextLen : NewSendText.字数;
                            numlist = GetRandomUnrepeatArray(0, TextLen - 1, sendNum);

                            foreach (int item in numlist)
                            {
                                TextAll += NewSendText.发文全文[item];
                            }
                            CleanBeforeSending(TextAll);
                            //* 缓存文段内容
                            Glob.TempSegmentRecord.Add(TextAll);
                        }
                    }
                    else
                    { //* 单字顺序
                        //? 此时 TextLen 指示总字数
                        if (NewSendText.标记 < TextLen)
                        {
                            int now = NewSendText.标记 + NewSendText.字数;
                            if (now < TextLen)
                            {
                                TextAll = NewSendText.发文全文.Substring(NewSendText.标记, NewSendText.字数);
                                NewSendText.标记 += NewSendText.字数;
                            }
                            else
                            {
                                TextAll = NewSendText.发文全文.Substring(NewSendText.标记, TextLen - NewSendText.标记);
                                NewSendText.标记 = TextLen;
                            }

                            CleanBeforeSending(TextAll);
                            //* 缓存文段内容
                            Glob.TempSegmentRecord.Add(TextAll);
                        }
                        else
                        {
                            ShowFlowText("文章全文已发送完毕，请重新换文！");
                            NewSendText.标记 = 0;
                        }
                    }
                }
                else if (NewSendText.类型 == "词组")
                {
                    int sendNum = NewSendText.字数 > NewSendText.词组.Length ? NewSendText.词组.Length : NewSendText.字数;
                    Random ro = new Random((int)DateTime.Now.Ticks);
                    for (int i = 0; i < sendNum; i++)
                    {
                        TextAll += NewSendText.词组[ro.Next(0, NewSendText.词组.Length - 1)] + NewSendText.词组发送分隔符;
                    }

                    if (NewSendText.词组发送分隔符.Length > 0)
                    { // 移除最末尾的一个分隔符
                        TextAll = TextAll.Remove(TextAll.Length - NewSendText.词组发送分隔符.Length,
                                      NewSendText.词组发送分隔符.Length);
                    }

                    CleanBeforeSending(TextAll);
                    //* 缓存文段内容
                    Glob.TempSegmentRecord.Add(TextAll);
                }
                else if (NewSendText.类型 == "文章")
                { //? 此时 TextLen 指示总字数
                    if (NewSendText.标记 < TextLen)
                    {  //标记必须小于长度
                        int now = NewSendText.标记 + NewSendText.字数;
                        if (now < TextLen && ((double)(TextLen - now) / NewSendText.字数) > 0.1)
                        { // 当前小于总字数，且距末尾的距离大于发文字数的 10%
                            int textlength = NewSendText.字数;
                            int findIndex = now - 1;
                            bool isLastFind = false;
                            bool isCurFind = false;
                            char[] textChars = NewSendText.文章全文.ToCharArray();

                            for (; findIndex < now + 50 && findIndex < TextLen; findIndex++)
                            { //* 寻找潜在的符号
                                string nowIt = textChars[findIndex].ToString();
                                isCurFind = !IsCN.IsMatch(nowIt);

                                if (isCurFind)
                                {
                                    if (nowIt == "“" || nowIt == "‘")
                                    { //? 不包括开引号
                                        textlength = findIndex - NewSendText.标记;
                                        break;
                                    }
                                    isLastFind = true;
                                }
                                else
                                { //? 一并处理连续符号
                                    if (isLastFind)
                                    {
                                        textlength = findIndex - NewSendText.标记;
                                        break;
                                    }
                                }
                            }

                            TextAll = NewSendText.文章全文.Substring(NewSendText.标记, textlength);
                            NewSendText.标记 += textlength;
                        }
                        else
                        {
                            TextAll = NewSendText.文章全文.Substring(NewSendText.标记, TextLen - NewSendText.标记);
                            NewSendText.标记 = TextLen;
                        }

                        CleanBeforeSending(TextAll);
                        //* 缓存文段内容
                        Glob.TempSegmentRecord.Add(TextAll);
                    }
                    else
                    {
                        ShowFlowText("文章全文已发送完毕，请重新换文！");
                        NewSendText.标记 = 0;
                    }
                }
                this.textBoxEx1.TextChanged += new System.EventHandler(textBoxEx1_TextChanged);
                NewSendText.已发字数 += TextAll.Length;
                发文状态后处理();
                this.Activate();
            }
        }

        /// <summary>
        /// 过去发文
        /// </summary>
        /// <param name="textAll">发文的内容</param>
        private void SendAPast(string textAll)
        {
            this.textBoxEx1.TextChanged -= new System.EventHandler(textBoxEx1_TextChanged);
            if (NewSendText.发文状态)
            {
                this.CleanSpeedPoints();
                this.cmsDuanList.Items.Clear();
                //输入法状态
                Glob.binput = true;
                int textMaxLen = NewSendText.文章全文.Length;
                int textlength = textAll.Length;
                this.lblTitle.Text = NewSendText.标题;
                if (NewSendText.类型 == "单字")
                {
                    if (NewSendText.是否乱序)
                    {
                        if (NewSendText.乱序全段不重复)
                        {
                            NewSendText.标记 += textlength;
                            if (NewSendText.标记 > textMaxLen)
                            {
                                NewSendText.标记 -= textMaxLen;
                            }
                        }
                    }
                    else
                    {
                        NewSendText.标记 += textlength;
                        if (NewSendText.标记 > textMaxLen)
                        {
                            NewSendText.标记 -= textMaxLen;
                        }
                    }
                }
                else if (NewSendText.类型 == "文章")
                {
                    NewSendText.标记 += textlength;
                    if (NewSendText.标记 > textMaxLen)
                    {
                        NewSendText.标记 -= textMaxLen;
                    }
                }

                CleanBeforeSending(textAll);
                this.textBoxEx1.TextChanged += new System.EventHandler(textBoxEx1_TextChanged);
                NewSendText.已发字数 += textlength;
                发文状态后处理();
                this.Activate();
            }
        }

        private void ReverseSendAPast(string textAll, int curTextLength)
        {
            this.textBoxEx1.TextChanged -= new System.EventHandler(textBoxEx1_TextChanged);
            if (NewSendText.发文状态)
            {
                this.CleanSpeedPoints();
                this.cmsDuanList.Items.Clear();
                //输入法状态
                Glob.binput = true;
                int textMaxLen = NewSendText.文章全文.Length;
                this.lblTitle.Text = NewSendText.标题;
                if (NewSendText.类型 == "单字")
                {
                    if (NewSendText.是否乱序)
                    {
                        if (NewSendText.乱序全段不重复)
                        {
                            NewSendText.标记 -= curTextLength;
                            if (NewSendText.标记 <= 0)
                            {
                                NewSendText.标记 += textMaxLen;
                            }
                        }
                    }
                    else
                    {
                        NewSendText.标记 -= curTextLength;
                        if (NewSendText.标记 <= 0)
                        {
                            NewSendText.标记 += textMaxLen;
                        }
                    }
                }
                else if (NewSendText.类型 == "文章")
                {
                    NewSendText.标记 -= curTextLength;
                    if (NewSendText.标记 <= 0)
                    {
                        NewSendText.标记 += textMaxLen;
                    }
                }

                CleanBeforeReverseSending(textAll);
                this.textBoxEx1.TextChanged += new System.EventHandler(textBoxEx1_TextChanged);
                NewSendText.已发字数 -= curTextLength;
                发文状态后处理();
                this.Activate();
            }
        }

        private void 发文状态后处理()
        {
            // MessageBox.Show("调用了发文状态后处理");
            if (发文状态窗口 != null && !发文状态窗口.IsDisposed)
            {
                发文状态窗口.lblSendCounted.Text = NewSendText.已发字数.ToString();//已发字数
                发文状态窗口.lblSendPCounted.Text = NewSendText.已发段数.ToString();//已发段数
                发文状态窗口.lblMarkCount.Text = NewSendText.标记.ToString();//当前标记
                发文状态窗口.tbxSendC.Text = NewSendText.字数.ToString();//一次发送字数
                发文状态窗口.tbxNowStartCount.Text = Glob.CurSegmentNum.ToString();//当前段号
                if (NewSendText.是否周期)
                {
                    发文状态窗口.lblNowTime.Text = NewSendText.周期计数.ToString();
                }
            }

            if (NewSendText.是否周期)
            {
                this.lblNowTime_.Text = NewSendText.周期计数.ToString();
            }
        }

        private void 发文状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //! 注：新发文同样是采用该事件来显示发文状态窗口
            if (!NewSendText.发文状态) return;
            // MessageBox.Show("起始段号：" + NewSendText.起始段号.ToString() + "\n Glob.Pre_Cout：" + Glob.Pre_Cout);
            if (发文状态窗口 != null)
            {
                if (发文状态窗口.IsDisposed)
                {
                    发文状态窗口 = new SendTextStatic(this.Location, this);
                    MagneticMagnager mm = new MagneticMagnager(this, 发文状态窗口, MagneticPosition.Left);
                    发文状态窗口.Show(this);
                }
            }
            else
            {
                发文状态窗口 = new SendTextStatic(this.Location, this);
                MagneticMagnager mm = new MagneticMagnager(this, 发文状态窗口, MagneticPosition.Left);
                发文状态窗口.Show(this);
                this.Focus();
            }
        }
        #endregion

        #region 周期发文
        /// <summary>
        /// 开始周期发文
        /// </summary>
        public void SendTTest()
        {
            NewSendText.周期计数 = NewSendText.周期;
            SendAOnce();
            timerTSend.Start();
        }

        private void timerTSend_Tick(object sender, EventArgs e)
        {
            if (NewSendText.是否周期)
            {
                NewSendText.周期计数 -= 1;
                if (发文状态窗口 != null && !发文状态窗口.IsDisposed)
                {
                    发文状态窗口.lblNowTime.Text = NewSendText.周期计数.ToString();
                }

                this.lblNowTime_.Text = NewSendText.周期计数.ToString();

                if (NewSendText.周期计数 <= 0)
                {
                    SendNextFun();
                }
            }
            else
            {
                timerTSend.Stop();
            }
        }
        #endregion

        /// <summary>
        /// ini的快捷读取
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public string IniRead(string section, string key, string def)
        {
            _Ini sing = new _Ini("config.ini");
            return sing.IniReadValue(section, key, def);
        }

        /// <summary>
        /// 初始化操作
        /// 1 数值初始化；2 显示初始化
        /// </summary>
        /// <param name="Contr"></param>
        public void Initialize(int Contr)
        {
            if (Contr == 1)
            { //数值初始化
                Glob.StartTextLen = 0;
                Glob.TextCz = 0; //错字
                Glob.TextJs = 0; //键数
                Glob.TextJj = 0; //击键
                Glob.TextHg = 0; //回改
                Glob.LastInput = 0; //最后输入 

                Sw = 0; //控制
                sw = 0;
                Glob.aTypeWords = 0;//打词
                Glob.aTypeWordsCount = 0;
                sTime = new DateTime();
                startTime = new DateTime();
                Glob.MaxSpeed = 0;
                Glob.MaxJj = 0;
                Glob.MaxMc = 10; //峰值
                Glob.MinSplite = 500;
                Glob.FWords.Clear();
                Glob.TextBg = 0;//退格
                Glob.回车 = 0;
                Glob.选重 = 0;
                Glob.FWordsSkip = 0;
                Glob.撤销 = 0;
                Glob.撤销用量 = 0;
                Glob.MinSplite = 500;
                Glob.TextMc = 0;
                Glob.TextMcc = 0;
                allUsedTime = new TimeSpan();
                recordUsedTime = new TimeSpan();
                Glob.PauseTimes = 0;
                Glob.Use分析 = false;
                Glob.Type_Map_C = 200;
                跟打地图步进 = 0;
                Glob.地图长度 = 0;
                Glob.Type_map_C_1 = Color.FromArgb(220, 220, 220);
                Glob.TextHgPlace.Clear();
                Glob.TypeReport.Clear();
                this.SeriesSpeed.Points.Clear();
                Glob.ChartSpeedArr.Clear();
                Array.Clear(Glob.KeysTotal, 0, 50);
            }
            else if (Contr == 2)
            {  //显示初始化
                richTextBox1.SelectionStart = 0;
                richTextBox1.ScrollToCaret();
                richTextBox1.SelectionLength = richTextBox1.TextLength;
                richTextBox1.SelectionBackColor = Glob.R1Back;
                richTextBox1.SelectionColor = Glob.R1Color;
                richTextBox1.SelectionFont = richTextBox1.Font;

                labelJjing.Text = "";
                labelmcing.Text = "";
                labelSpeeding.Text = "";
                labelTimeFlys.Text = "00:00.00";
                labelJsing.Text = "0";
                labelhgstatus.Text = "0";
                HisSave[0] = HisSave[1] = 0;
                HisLine[0] = HisLine[1] = 0;
                this.labelBM.Text = "-";
                if (isPause)
                {
                    EndPause();
                }
                //跟打地图
                Bitmap bmp_ = new Bitmap(this.picMap.ClientRectangle.Width, this.picMap.ClientRectangle.Height);
                this.picMap.Image = bmp_;
                this.lbl地图长度.Text = "";
                //跟打长度
                Bitmap bmp_1 = new Bitmap(this.picBar.ClientRectangle.Width, this.picBar.ClientRectangle.Height);
                this.picBar.Image = bmp_1;
                this.lbl回改显示.Text = "回改";
                this.lbl错字显示.Text = "错字";
                //编码显示归零
                PicSetBmTips("", "", 0);
                //拆分条进度
                using (Graphics g = this.splitContainer1.CreateGraphics())
                {
                    g.Clear(Theme.ThemeColorBG);
                }
                if (Glob.IsPointIt)
                {
                    this.richTextBox1.SetCurIndex(0);
                }
            }

            try
            {
                KH.Start();
            }
            catch { }
            System.GC.Collect();
        }

        #region 跟打过程
        /// <summary>
        /// 记录跟打总字数
        /// </summary>
        /// <param name="count"></param>
        private void RecTextTypeCount(int count)
        {
            if (Glob.TodayDate != DateTime.Now.ToShortDateString())
            {
                Glob.TextRecDays++;//记录天数自增
                Glob.todayTyping = 0;//今日跟打归零
                Glob.TodayDate = DateTime.Now.ToShortDateString();
            }
            Glob.todayTyping += count; //今日跟打
            Glob.TextLenAll += count;//总字数
            labelHaveTyping.Text = Glob.todayTyping + "/" + 字数格式化(Glob.TextLenAll) + "/" + Glob.TextRecDays + "天/" + 字数格式化(Glob.TextLenAll + Glob.TextHgAll);
        }

        private int 上次输入标记 = 1;
        private int 跟打地图步进 = 0;

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            int TextLen = Glob.TextLen;
            if (TextLen > 1) // 文段长度超过 2 个字符才统计
            {
                Application.DoEvents();

                //获取文章
                string TextAlticle = richTextBox1.Text;
                string TextType = richTextBox2.Text;
                int TextLenNow = TextType.Length;
                if (TextLenNow <= TextLen)
                {
                    //progressbar1.Size = new Size(TextLenNow * panel2.Size.Width / TextLen, 5);
                    int shengyu = TextLen - TextLenNow;
                    picBar_Draw((double)TextLenNow / TextLen, shengyu + "|" + shengyu * 100 / TextLen + "%");
                }
                //Application.DoEvents();
                //MessageBox.Show(TextType + "\n" + Sw.ToString());
                //再比较 当时的字符数量-1 就是原数组的序列
                if (TextLenNow >= 0 && TextLenNow <= TextLen)
                {
                    int getstart = richTextBox1.GetLineFromCharIndex(TextLenNow);
                    int getExend = richTextBox1.GetLineFromCharIndex(TextLen - 1);//获取最后一行的行号 也就是 总行号
                    HisLine[1] = getstart;
                    if (HisLine[1] != HisLine[0])
                    { //* 对滚动条的控制
                        this.richTextBox2.BeginInvoke(new MethodInvoker(delegate
                        {
                            int sizeH = richTextBox1.ClientSize.Height; //一屏高度
                            int onePHan = (int)Math.Ceiling((double)sizeH / Glob.oneH);//一屏行数
                            int sizeH_ = onePHan * Glob.oneH;
                            int nowHan = richTextBox1.GetPositionFromCharIndex(TextLenNow).Y; //当前
                            int allH = richTextBox1.GetPositionFromCharIndex(TextLen).Y + Glob.oneH; //末行像素
                            //MessageBox.Show("末行高度：" + allH.ToString() + "\n一屏高度：" + sizeH.ToString() + "\n一行高度：" + Glob.oneH + "\n当前高度：" + nowHan + "\n可见总数：" + onePHan + "\n第一像素：" + richTextBox1.GetPositionFromCharIndex(0).Y);
                            if (nowHan > 0)
                            {
                                if (allH > sizeH) //末行高度超出 一屏高度时才启用滚屏
                                {
                                    if (nowHan >= (sizeH_ - Glob.oneH * 2)) //走到倒数第二行时
                                    {
                                        this.richTextBox1.SelectionStart = TextLenNow - 上次输入标记;
                                        this.richTextBox1.ScrollToCaret();
                                    }
                                }
                            }
                            else
                            {
                                this.richTextBox1.SelectionStart = TextLenNow;
                                this.richTextBox1.ScrollToCaret();
                            }
                        }));
                        HisLine[0] = HisLine[1];
                    }

                    Sw += 1; // 每打一个字词就加 1
                    sw++;
                    if (Sw == 1)
                    { // 进入到 TextCahnged 事件后 Sw 至少是 1
                        startTime = DateTime.Now; // 开始跟打时间
                        sTime = startTime;
                        //! 由于可能会出现 timer1_Tick() 还未触发的情况，导致 Glob.typeUseTime 中的值还保留为上一段结束的值，
                        //! 从而造成跟打报告中第 2 项数据记录录异常的问题
                        Glob.TypeUseTime = 0;
                        timer1.Start(); //没有开启则
                        timer2.Start();
                        timer3.Start(); //图表
                        timer5.Start();
                        Point pmos = new Point(), rbox = new Point(), rbbbox = new Point(), toXY = new Point();
                        GetCursorPos(ref pmos);
                        rbox = this.richTextBox1.PointToClient(pmos);
                        Size rbbox = new Size();
                        rbbox = this.richTextBox1.ClientSize;
                        rbbbox = new Point(rbbox.Width, rbbox.Height);
                        toXY = this.richTextBox1.PointToScreen(rbbbox);
                        if (rbox.X < rbbox.Width && rbox.Y < rbbox.Height)
                            SetCursorPos(toXY.X + 30, toXY.Y);
                        if (richTextBox1.Text.Substring(0, 1) == richTextBox2.Text.Substring(0, 1))
                        {
                            RecTextTypeCount(richTextBox2.TextLength);
                        }
                        HisSave[1] = TextLenNow;
                        Glob.StartTextLen = TextLenNow;
                        Glob.StartKeyLen = Glob.TextJs; // 不置空 Glob.TextJs，而是记录，用于后续计算击键等减去
                        跟打地图步进 = 0;
                        //跟打报告
                        Glob.TypeReport.Add(new TypeDate { Index = Sw, Start = 0, End = TextLenNow, Length = HisSave[1] - HisSave[0], NowTime = 0, TotalTime = 0, Tick = Glob.StartKeyLen, TotalTick = Glob.StartKeyLen });
                    }
                    else
                    {
                        HisSave[0] = HisSave[1];
                        HisSave[1] = TextLenNow;
                        //跟打报告
                        Glob.TypeReport.Add(new TypeDate { Index = Sw, Start = HisSave[0], End = HisSave[1], Length = HisSave[1] - HisSave[0], NowTime = Math.Round(Glob.TypeUseTime, 4), TotalTime = Math.Round(Glob.TypeUseTime - Glob.TypeReport[Glob.TypeReport.Count - 1].NowTime, 4), Tick = Glob.TextJs, TotalTick = Glob.TextJs - Glob.TypeReport[Glob.TypeReport.Count - 1].Tick });
                        int last = HisSave[0];
                        跟打地图步进++;
                        Glob.地图长度++;
                        this.lbl地图长度.Text = (TextLen * 100 / sw).ToString("0") + "%";
                        if (跟打地图步进 > this.picMap.Width)
                        {
                            Glob.Type_Map_C -= 40;
                            if (Glob.Type_Map_C < 40)
                                Glob.Type_Map_C = 200;
                            Glob.Type_map_C_1 = Color.FromArgb(Glob.Type_Map_C, Glob.Type_Map_C, Glob.Type_Map_C);
                            跟打地图步进 = 0;
                        }
                        if (HisSave[1] > HisSave[0]) //非回改的情况下
                        {
                            int iPP = HisSave[1] - HisSave[0]; //长度
                            Glob.撤销用量 = iPP;
                            try
                            {
                                if (last >= richTextBox1.TextLength) { last = richTextBox1.TextLength - 1; }
                                string lastinput = richTextBox2.Text.Substring(last, iPP);
                                if (richTextBox1.Text.Substring(last, iPP) == lastinput) //所有的字非错的情况下
                                {
                                    RecTextTypeCount(iPP);
                                }
                            }
                            catch { }
                        }
                    }

                    int iP = HisSave[1] - HisSave[0];
                    if (iP > 0) //非退格情况往前打字
                    {
                        Glob.TextMc = 0;//完美计数
                        上次输入标记 = iP;
                        //Glob.TextCz = 0;//每次都归零
                        int Istart = textBoxEx1.SelectionStart; //在非回改情况下获取当前光标所在位置
                        int Glast = TextLenNow;//当前字数
                        if (Istart == Glast) //当前后面没有 字符的情况。
                        {
                            for (int i = HisSave[0]; i < HisSave[1]; i++)
                            {
                                if (TextType[i] == TextAlticle[i])
                                {
                                    richTextBox1.SelectionStart = i;
                                    richTextBox1.SelectionLength = 1;
                                    richTextBox1.SelectionBackColor = Glob.RightBGColor;
                                    if (Glob.FWords.Contains(i))//以标识来计算错误量
                                    {
                                        Glob.FWords.Remove(i);
                                    }
                                    Glob.Type_Map_Color = Glob.Type_map_C_1;
                                }
                                else
                                {
                                    richTextBox1.SelectionStart = i;
                                    richTextBox1.SelectionLength = 1;
                                    richTextBox1.SelectionBackColor = Glob.FalseBGColor;
                                    if (!Glob.FWords.Contains(i))//以标识来计算错误量
                                    {
                                        Glob.FWords.Add(i);
                                    }
                                    Glob.Type_Map_Color = Color.OrangeRed;

                                }
                            }

                            if (iP >= 2)
                            {//* 打词记录
                                string nowinput = richTextBox1.Text.Substring(HisSave[0], iP);

                                if (Glob.NotSymbolInput)
                                { //* 不统计符号上屏的字
                                    if (!string.IsNullOrEmpty(Glob.UsedTableIndex))
                                    { // 存在默认码表时
                                        if (Glob.AllWordDic.ContainsKey(nowinput))
                                        {
                                            Glob.aTypeWords++;
                                            Glob.aTypeWordsCount += iP;
                                        }
                                        else
                                        {
                                            if (iP > 2)
                                            {
                                                for (int c = iP - 1; c > 1; c--)
                                                {
                                                    string cInput = richTextBox1.Text.Substring(HisSave[0], c);
                                                    if (Glob.AllWordDic.ContainsKey(cInput))
                                                    {
                                                        Glob.aTypeWords++;
                                                        Glob.aTypeWordsCount += c;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    { // 不在在默认码表时
                                        for (int m = iP - 1; m > 0; m--)
                                        {
                                            if (!SymbolChars.Contains(nowinput[m]))
                                            {
                                                Glob.aTypeWords++;
                                                Glob.aTypeWordsCount += m + 1;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (iP == 2)
                                    {
                                        if (nowinput != "……" && nowinput != "——")
                                        { // 排除 2 位字宽的符号
                                            Glob.aTypeWords++;
                                            Glob.aTypeWordsCount += iP;
                                        }
                                    }
                                    else
                                    {
                                        Glob.aTypeWords++;
                                        Glob.aTypeWordsCount += iP;
                                    }
                                }
                            }
                        }
                        else
                        { //插入输入的情况
                            //MessageBox.Show("当前字数：" + Glast + "\n当前光标：" + Istart);
                            for (int i = Istart - iP; i < Glast; i++)
                            {
                                if (TextType[i] == TextAlticle[i])
                                {
                                    richTextBox1.SelectionStart = i;
                                    richTextBox1.SelectionLength = 1;
                                    richTextBox1.SelectionBackColor = Glob.RightBGColor;
                                    if (Glob.FWords.Contains(i))//以标识来计算错误量
                                    {
                                        Glob.FWords.Remove(i);
                                    }
                                }
                                else
                                {
                                    richTextBox1.SelectionStart = i;
                                    richTextBox1.SelectionLength = 1;
                                    richTextBox1.SelectionBackColor = Glob.FalseBGColor;
                                    if (!Glob.FWords.Contains(i))//以标识来计算错误量
                                    {
                                        Glob.FWords.Add(i);
                                    }
                                }
                            }
                        }

                        //测速点
                        if (Glob.SpeedPointCount > 0)
                        {
                            for (int i = 0; i < Glob.SpeedPointCount; i++)
                            {
                                for (int j = HisSave[0]; j < HisSave[1]; j++)
                                {
                                    if (Glob.SpeedPoint_[i] == j)
                                    {
                                        Glob.SpeedTime[i] = Glob.TypeUseTime;
                                        Glob.SpeedJs[i] = Glob.TextJs;
                                        Glob.SpeedHg[i] = Glob.TextHg;
                                        Glob.SpeedControl++;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        // 这是一种回改的情况
                        //Glob.TextCz = 0;//每次都归零
                        Glob.Type_Map_Color = Color.DeepSkyBlue;//回改橙色
                        int istart = textBoxEx1.SelectionStart; //获取当前光标所在的编号
                        int istep = Math.Abs(iP);//获取一次退格的 量
                        //MessageBox.Show(HisSave[1] + "\n" + HisSave[0]);
                        Glob.TextHgAll++; //? 和 Glob.TextHg 的计算方式结果基本是等效的
                        if (istep > 0)
                        {
                            richTextBox1.SelectionStart = HisSave[1];
                            richTextBox1.SelectionLength = istep;
                            richTextBox1.SelectionBackColor = Glob.R1Back;
                            for (int i = HisSave[1]; i <= HisSave[0]; i++)
                            {
                                if (Glob.FWords.Contains(i))//以标识来计算错误量
                                {
                                    Glob.FWords.Remove(i);
                                }
                            }
                        }
                        //else
                        //{
                        //MessageBox.Show(istart + "\n" + HisSave[1]);
                        for (int i = istart; i < HisSave[1]; i++) //从当前光标处再继续往后比较正错
                        {
                            if (TextType[i] == TextAlticle[i])
                            {
                                richTextBox1.SelectionStart = i;
                                richTextBox1.SelectionLength = 1;
                                richTextBox1.SelectionBackColor = Glob.RightBGColor;
                                if (Glob.FWords.Contains(i))//以标识来计算错误量
                                {
                                    Glob.FWords.Remove(i);
                                }
                            }
                            else
                            {
                                richTextBox1.SelectionStart = i;
                                richTextBox1.SelectionLength = 1;
                                richTextBox1.SelectionBackColor = Glob.FalseBGColor;
                                if (!Glob.FWords.Contains(i))//以标识来计算错误量
                                {
                                    Glob.FWords.Add(i);
                                }
                            }
                        }

                        //}
                    }
                    Type_Map(Glob.Type_Map_Color, 跟打地图步进, 1);
                    //更新错字
                    this.labelBM.Text = Glob.FWords.Count.ToString();
                    if (Glob.IsPointIt)
                    {
                        this.richTextBox1.SetCurIndex(TextLenNow);
                    }
                }

                if (TextLenNow == TextLen)
                {
                    for (int i = HisSave[0]; i < HisSave[1]; i++)
                    {
                        // 末尾最后三个输入的字符有错字时
                        if (TextAlticle[i] != TextType[i])
                        {
                            Glob.LastInput = 1;
                            break;
                        }

                        if (i >= 1 && (TextAlticle[i - 1] != TextType[i - 1]))
                        {
                            Glob.LastInput = 1;
                            break;
                        }

                        if (i >= 2 && (TextAlticle[i - 2] != TextType[i - 2]))
                        {
                            Glob.LastInput = 1;
                            break;
                        }
                    }

                    if (Glob.LastInput == 1)
                    {
                        Glob.LastInput = 0;
                        return;
                    }
                    timer1.Enabled = false; //关闭计时器
                    timer2.Enabled = false;
                    timer3.Enabled = false; //图表
                    timer5.Stop(); //长时间不跟打自动重打

                    this.lblAutoReType.Text = "0";
                    //已跟打赋值
                    Glob.HaveTypeCount++;//已跟打段数

                    #region 跟打结束

                    Glob.jjAllC++;//跟打总段数
                    LblHaveTypingChange();

                    textBoxEx1.ReadOnly = true;
                    ts = Glob.TypeUseTime;
                    Sw = 0; //初始化
                    Glob.TotalUse += Glob.TypeUseTime;
                    //处理数据
                    double speed = Math.Round((TextLen - Glob.StartTextLen) * 60 / ts, 2); // 不处理错字
                    double mc = Math.Round((double)Glob.TextJs / TextLen, 2);
                    double jj = Math.Round((Glob.TextJs - Glob.StartKeyLen) / ts, 2);
                    //以下三列数据为测速准备
                    Glob.TextSpeed = speed;
                    Glob.Textjj = jj;
                    Glob.Textmc = mc;
                    //击键数据排列
                    int j = (int)jj;
                    if (j > 3 && j < 12)
                    {
                        Glob.jjPer[j - 4]++;
                    }
                    else if (j >= 12)
                    {
                        Glob.jjPer[8]++;
                    }
                    //平均速度
                    Glob.TypeCount++; //跟打次数
                    //错情 与 错字
                    string RightAndFault = "", RFSplit = "|"; //分隔符
                    string fa = "";
                    Glob.TextCz = Glob.FWords.Count;
                    for (int i = 0; i < Glob.FWords.Count; i++)
                    {
                        int s = (int)Glob.FWords[i];
                        fa = TextType.Substring(s, 1);
                        if (fa == " ") { fa = "□"; }
                        RightAndFault += TextAlticle.Substring(s, 1) + "√ " + fa + "×";
                        if (Glob.TextCz > 0 && i < Glob.TextCz - 1) { RightAndFault += RFSplit; }
                    }
                    //MessageBox.Show(RightAndFault);
                    string Cz, Spsend; //错字和速度的输出
                    string FalutIns = "";//错情
                    double speed2 = Math.Round((double)((TextLen - Glob.StartTextLen - Glob.TextCz * 5) * 60) / ts, 2); // 惩罚错字后的真实速度，错一罚五
                    if (Glob.TextCz != 0)
                    {
                        if (speed2 < 0) { speed2 = 0.00; }
                        Spsend = speed2.ToString("0.00") + "/" + speed.ToString("0.00");
                        FalutIns = " 错情：[" + RightAndFault + "]";
                    }
                    else
                    {
                        Spsend = speed.ToString("0.00");
                    }
                    Cz = " 错字" + Glob.TextCz.ToString(); // 末尾描述

                    //* 速度评级
                    double nowValue = speed2 * Glob.Difficulty;
                    Glob.SpeedGradeSpeed += speed2;
                    Glob.SpeedGradeDiff += Glob.Difficulty;

                    if (Glob.SpeedGradeCount > 100)
                    {
                        if (nowValue > Glob.SpeedGrade)
                        {
                            Glob.SpeedGrade += Glob.Difficulty;
                        }
                        else if (nowValue < Glob.SpeedGrade)
                        {
                            Glob.SpeedGrade -= 1 / Glob.Difficulty;
                        }
                        Glob.SpeedGradeCount++;
                    }
                    else
                    {
                        double gradeValue = Glob.SpeedGrade * Glob.SpeedGradeCount + nowValue;
                        Glob.SpeedGradeCount++;
                        Glob.SpeedGrade = gradeValue / Glob.SpeedGradeCount;
                    }

                    //? 跟打用时小于 1s 以及错字超过 10% 时不处理
                    if (Glob.TypeUseTime >= 1 && Glob.TextCz <= (int)TextLen / 10)
                    {
                        //? 回改率和打词率的分母不需要减去起始字数量，因为这些数据和时间没有联系，起始的字数同样被统计进来
                        // Glob.TextHg_ = (double)Glob.TextHgAll * 100 / Glob.TextLenAll;   // 这计算的是总回改率
                        // 回改率
                        Glob.TextHg_ = Math.Round((double)Glob.TextHg * 100 / (Glob.TextLen + Glob.TextHg), 2);
                        // 打词率
                        Glob.TextDc_ = Math.Round((double)Glob.aTypeWordsCount * 100 / (Glob.TextLen + Glob.TextHg), 2);
                        // 跟打效率
                        Glob.效率 = TextLen * 100 / sw;
                        this.lbl地图长度.Text = Glob.效率 + "%"; // 显示跟打效率

                        //回改用时
                        Glob.hgAllUse = Glob.TypeReport.Where(o => o.Length < 0).Sum(o => o.TotalTime);
                        sw = 0;

                        //键准
                        this.lbl键准.Text = (UserJz == 0) ? "-" : UserJz + "%";

                        picBar_Draw(0.0, Glob.TextLen + ",100%");
                        labelSpeeding.Text = speed.ToString("0.00");
                        labelJjing.Text = jj.ToString("0.00");
                        labelmcing.Text = mc.ToString("0.00");

                        //平均数据
                        //this.SeriesSpeed.Points.AddXY(Glob.typeUseTime,speed2);
                        Glob.Per_Speed += speed2; // 用的惩罚错字后的速度
                        Glob.Per_Jj += jj;
                        Glob.Per_Mc += mc;

                        double touse = Glob.TotalUse;
                        if (dataGridView1.RowCount > 1)
                        {
                            dataGridView1.Rows.RemoveAt(dataGridView1.RowCount - 1);
                        }
                        Glob.TextTime = DateTime.Now;
                        //显示打完信息
                        //ShowFlowText("第" + Glob.Pre_Cout + "段" + " 速度" + Glob.TextSpeed + " 击键" + Glob.Textjj + " 码长" + Glob.Textmc + " 用时" + new DateTime().AddSeconds(Glob.typeUseTime).ToString("mm.ss.ff"));
                        string typeCountStr = "";
                        if (Glob.TextPreCout == this.lblMatchCount.Text)
                        { // 重打判断，为重打
                            Glob.reTypeCount++;
                            toolStripStatusLabelStatus.Text = Glob.reTypeCount.ToString();
                            toolStripStatusLabelStatus.ForeColor = Color.DarkGreen;

                            int RowCount = this.dataGridView1.Rows.Count - 1;
                            string[] oldSpeed = this.dataGridView1.Rows[RowCount].Cells[3].Value.ToString().Split('/');
                            double speed_Plus = speed2 - double.Parse(oldSpeed[0]);
                            double jj_Plus = Glob.Textjj - double.Parse(this.dataGridView1.Rows[RowCount].Cells[4].Value.ToString());
                            double mc_Plus = Glob.Textmc - double.Parse(this.dataGridView1.Rows[RowCount].Cells[5].Value.ToString());
                            dataGridView1.Rows.Add("", "", "",
                                //速度
                                (speed_Plus > 0 ? "+" : "") + speed_Plus.ToString("0.00"),
                                //击键
                                (jj_Plus > 0 ? "+" : "") + jj_Plus.ToString("0.00"),
                                //码长
                                (mc_Plus > 0 ? "+" : "") + mc_Plus.ToString("0.00")
                                );
                            RowCount++;
                            dataGridView1.Rows[RowCount].Height = 10;
                            dataGridView1.Rows[RowCount].DefaultCellStyle.Font = new Font("Arial", 6.8f);
                            dataGridView1.Rows[RowCount].DefaultCellStyle.ForeColor = Color.LightGray;
                            //各个项目样式 
                            if (speed_Plus > 0) dataGridView1.Rows[RowCount].Cells[3].Style.ForeColor = Color.FromArgb(253, 108, 108);
                            if (jj_Plus > 0) dataGridView1.Rows[RowCount].Cells[4].Style.ForeColor = Color.FromArgb(255, 129, 233);
                            if (mc_Plus < 0) dataGridView1.Rows[RowCount].Cells[5].Style.ForeColor = Color.FromArgb(124, 222, 255);
                            for (int i = 0; i < 23; i++)
                            {
                                if (i != 3 && i != 4 && i != 5)
                                {
                                    this.dataGridView1.Rows[RowCount].Cells[i].Style.BackColor = Color.FromArgb(61, 61, 61);
                                }
                            }
                            typeCountStr = "";
                        }
                        else
                        { // 此次和上次文章验证不相同，为新打
                            Glob.reTypeCount = 0;
                            toolStripStatusLabelStatus.Text = "-";
                            toolStripStatusLabelStatus.ForeColor = Color.Black;

                            Glob.HaveTypeCount_++; //实际跟打段数加一
                            typeCountStr = Glob.HaveTypeCount_.ToString();
                        }
                        //* 成绩栏添加新数据行
                        dataGridView1.Rows.Add(typeCountStr, Glob.TextTime.ToString("G"), Glob.CurSegmentNum.ToString(), Spsend, jj.ToString("0.00"), mc.ToString("0.00"), Glob.词库理论码长.ToString("0.00"), Glob.Difficulty.ToString("0.00"), (Glob.Difficulty * speed2).ToString("0.00"), Glob.TextHg.ToString(), UserTg, Glob.回车.ToString(), Glob.选重.ToString(), Glob.TextCz.ToString(), UserHgl, lbl键准.Text, Glob.效率 + "%", Glob.TextJs.ToString(), TextLen.ToString(), Glob.aTypeWords, UserDcl, UserTime, this.lblTitle.Text);
                        //* 绑定右键菜单
                        dataGridView1.Rows[dataGridView1.RowCount - 1].ContextMenuStrip = this.ScoreContextMenuStrip;
                        Glob.TextPreCout = this.lblMatchCount.Text; // 记录本文段校验码
                        //成绩信息底色黑
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(61, 61, 61);
                        #region 单元格高亮
                        // 使用会惩罚错字的速度进行判定
                        CellHighlight.Speed(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3], speed2, Glob.Difficulty);
                        CellHighlight.Keystroke(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4], jj);
                        CellHighlight.CodeLen(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5], mc);
                        CellHighlight.Error(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[13], Glob.TextCz);
                        #endregion
                        double jjPer_ = Glob.Per_Jj / Glob.HaveTypeCount;
                        Glob.Total_Type += Glob.TextLen;
                        string dis = "00:00:00";
                        if (Glob.HaveTypeCount >= 1)
                        {
                            DateTime dt = new DateTime().AddSeconds(touse);
                            dis = dt.ToString("HH:mm:ss");
                        }
                        // 成绩栏总计行
                        dataGridView1.Rows.Add("", dis, Glob.HaveTypeCount + "#", (Glob.Per_Speed / Glob.HaveTypeCount).ToString("0.00"), jjPer_.ToString("0.00"), (Glob.Per_Mc / Glob.HaveTypeCount).ToString("0.00"), "", "", "", "", "", "", "", "", "", "", "", "", (Glob.Total_Type / Glob.HaveTypeCount).ToString("0.00"), "", "", (touse / Glob.HaveTypeCount).ToString("0.00"), "");
                        dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                        dataGridView1.ClearSelection();
                        DataGridViewRow dgr = dataGridView1.Rows[dataGridView1.RowCount - 1];
                        //dgr.DefaultCellStyle.BackColor = Theme.ThemeColorBG;
                        dgr.DefaultCellStyle.ForeColor = Theme.ThemeColorFC;
                        jjPerCheck((int)jj);//击键占率
                        Show_Hg_Place();//显示回改地点
                        Glob.Use分析 = true;//F3时 分析不可用 只有在跟打结束后分析才可用
                        
                        try
                        {
                            KH.Stop();
                        }
                        catch { }

                        #region 成绩结果生成
                        //顺序及发送
                        string sortsend = "", qidayu = "";
                        string TotalSend = "";
                        if (!Glob.isMatch)
                        { // 非赛文
                            if (Glob.SimpleMoudle)
                            {  //极简模式
                                string string1 = Glob.CurSegmentNum.ToString() + Glob.SimpleSplite + speed2.ToString("0.00") + Glob.SimpleSplite + jj.ToString("0.00") + Glob.SimpleSplite + mc.ToString("0.00") + Glob.SimpleSplite + this.lblMatchCount.Text;
                                TotalSend += string1 + Glob.SimpleSplite;
                                if (Glob.InstraPre_ != "0")
                                {
                                    TotalSend += Glob.InstraPre + Glob.SimpleSplite;
                                }
                                TotalSend += Glob.Instration;
                            }
                            else
                            {
                                sortsend = Glob.sortSend;
                                qidayu = "";
                            }
                        }
                        else
                        { // 赛文全显示
                            sortsend = "ABDSTVCUEFGHIJKLWMNOQR";
                            qidayu = " 起打于" + startTime.ToLongTimeString();
                        }

                        if (sortsend.Length != 0)
                        { // 若顺序不为空时
                            //段号
                            string duanhao = "第" + Glob.CurSegmentNum.ToString() + "段";
                            if (!Glob.isMatch && duanhao == "段号")
                            {
                                duanhao = "自测";

                            }

                            //个签
                            string inistra = "";
                            if (Glob.InstraPre_ != "0")
                            {
                                inistra = " 个签:" + Glob.InstraPre;
                            }

                            //输入法签名
                            string inistraSrf = "";
                            if (Glob.InstraSrf_ != "0")
                            {
                                inistraSrf = " 输入法:" + Glob.InstraSrf;
                            }

                            //打词
                            string atypewords = " 打词" + Glob.aTypeWords;

                            //重打
                            string reTypeing;
                            if (Glob.reTypeCount > 0)
                            {
                                reTypeing = " 重打" + Glob.reTypeCount.ToString();
                            }
                            else
                            {
                                reTypeing = "";
                            }

                            //停留
                            string stay = "";
                            try
                            {
                                const string bd =
                                    @"，。“”！（）()~·#￥%&*_[]{}‘’/\<>,.《》？：；、—…1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ";
                                var findall = Glob.TypeReport.OrderByDescending(o => o.TotalTime);
                                foreach (
                                    var typeDate in
                                        from typeDate in findall
                                        let s = TextAlticle[typeDate.Start + typeDate.Length - 1]
                                        where !bd.Contains(s)
                                        select typeDate)
                                {
                                    stay = string.Format(" 停留[{0}]{1}",
                                                         TextAlticle.Substring(typeDate.Start, typeDate.Length),
                                                         typeDate.TotalTime.ToString("0.00") + "s");
                                    richTextBox1.SelectionStart = typeDate.Start;
                                    richTextBox1.SelectionLength = typeDate.Length;
                                    richTextBox1.SelectionBackColor = Glob.TimeLongColor;
                                    break;
                                }
                            }
                            catch
                            {
                                stay = "";
                            }

                            //跟打效率
                            string awordper;
                            if (sw != 0)
                            {
                                awordper = " 效率" + Glob.效率 + "%";
                            }
                            else
                            {
                                awordper = "";
                            }

                            char[] TSend = sortsend.ToArray();
                            TotalSend = duanhao;
                            for (int i = 0; i < TSend.Length; i++)
                            {
                                switch (TSend[i])
                                {
                                    case 'A': TotalSend += " 速度" + Spsend; break;
                                    case 'B':
                                        if (jj != 0)
                                        {
                                            if (!(jj <= 3.2 && mc <= 1.3))
                                            {
                                                TotalSend += " 击键" + jj.ToString("0.00");
                                            }
                                        }
                                        break;
                                    case 'C':
                                        if (mc != 0)
                                        {
                                            if (!(jj <= 3.2 && mc <= 1.3))
                                            {
                                                var v = "";
                                                if (Glob.词库理论码长 != 0) v = " 词库理论" + Glob.词库理论码长.ToString("0.00");
                                                TotalSend += " 码长" + mc.ToString("0.00") + v;
                                            }
                                        }
                                        break;
                                    case 'D':
                                        TotalSend += 回改量 + 连改;
                                        break;

                                    case 'S': TotalSend += " 退格" + UserTg; break;
                                    case 'T': TotalSend += UserHcText; break;
                                    case 'U': TotalSend += UserXcText; break;
                                    case 'V': TotalSend += " 键准" + lbl键准.Text; break;
                                    case 'E': TotalSend += Cz; break;
                                    case 'F': TotalSend += FalutIns; break;
                                    case 'G': TotalSend += " 字数" + TextLen; break;
                                    case 'H':
                                        if (Glob.TextJs != 0)
                                        {
                                            if (!(jj <= 3.2 && mc <= 1.3))
                                                TotalSend += " 键数" + Glob.TextJs;
                                        }
                                        break;
                                    case 'I': TotalSend += UserTime == "" ? " 用时" + UserTime : ""; break;
                                    case 'J': TotalSend += reTypeing; break;
                                    case 'K':
                                        //峰值
                                        string MaxValue = "";
                                        if (this.textBoxEx1.TextLength > 10 && jj > 3.2 && mc > 1.3)
                                        {
                                            if (speed2 > Glob.MaxSpeed) Glob.MaxSpeed = speed2;
                                            if (jj > Glob.MaxJj) Glob.MaxJj = jj;
                                            if (mc < Glob.MaxMc) Glob.MaxMc = mc;
                                            MaxValue = " 峰值" + Glob.MaxSpeed.ToString("0.00") + "/" + Glob.MaxJj.ToString("0.00") + "/" + Glob.MaxMc.ToString("0.00");
                                        }
                                        TotalSend += MaxValue; break;
                                    case 'L': TotalSend += atypewords; break;
                                    case 'M': TotalSend += " 回改率" + UserHgl; break;
                                    case 'N': TotalSend += stay; break;
                                    case 'O': TotalSend += awordper; break;

                                    case 'Q': TotalSend += 撤销; break;
                                    case 'R': TotalSend += 键法; break;
                                    case 'W': TotalSend += " 打词率" + UserDcl; break;
                                    default: break;
                                }
                            }
                            TotalSend += inistraSrf;
                            if (Glob.isMatch)
                            {
                                TotalSend += qidayu + " 赛文验证:" + lblMatchCount.Text;
                            }
                            TotalSend += " 校验:" + Validation.Validat(TotalSend);
                            TotalSend += inistra + 暂停 + 版本;
                            Glob.theLastGoal = TotalSend;
                        }
                        #endregion

                        #region 自动将统计结果复制到剪贴板
                        if (Glob.AutoCopy)
                        {
                            ClipboardHandler.SetTextToClipboard(TotalSend);
                            //* 闪烁按钮
                            timerSubFlash.Start();
                        }
                        #endregion

                        #region 保存成绩到数据库中
                        //* 保存文段
                        long databaseSegmentId = Glob.ScoreHistory.InsertSegment(Glob.TypeText, this.lblMatchCount.Text);
                        //* 保存成绩
                        Glob.ScoreHistory.InsertScore(Glob.TextTime.ToString("s"), Glob.CurSegmentNum, Spsend, jj, mc, Glob.词库理论码长, Glob.TextHg, Math.Abs(Glob.TextBg - Glob.TextHg), Glob.回车, Glob.选重, Glob.TextCz, Glob.TextHg_, UserJz, Glob.效率, Glob.TextJs, TextLen, Glob.aTypeWords, Glob.TextDc_, UserTime, databaseSegmentId, this.lblTitle.Text, Glob.Instration, Glob.Difficulty);
                        if (!Glob.DisableSaveAdvanced)
                        { // 保存高阶统计数据
                            string curveData = string.Join("|", Glob.ChartSpeedArr);
                            string speedAnalysisData = SpeedAnalysis();
                            string typeAnalysisData = JsonConvert.SerializeObject(Glob.TypeReport);
                            string keyAnalysisData = string.Join("|", Glob.KeysTotal);
                            Glob.ScoreHistory.InsertAdvanced(Glob.TextTime.ToString("s"), curveData, speedAnalysisData, typeAnalysisData, keyAnalysisData);
                            if (!string.IsNullOrEmpty(Glob.UsedTableIndex) && Glob.词库理论码长 > 0)
                            {
                                string calcKeys = string.Join("|", Glob.CalcKeysTotal);
                                Glob.ScoreHistory.InsertCalc(Glob.TextTime.ToString("s"), calcKeys);
                            }
                        }
                        #endregion

                        #region 记录当前数据
                        StorageDataSet.ScoreRow scoreRow = this.currentScoreData.NewScoreRow();
                        scoreRow["score_time"] = Glob.TextTime;
                        scoreRow["segment_num"] = Glob.CurSegmentNum;
                        scoreRow["speed"] = Spsend;
                        scoreRow["keystroke"] = jj;
                        scoreRow["code_len"] = mc;
                        scoreRow["calc_len"] = Glob.词库理论码长;
                        scoreRow["back_change"] = Glob.TextHg;
                        scoreRow["backspace"] = Math.Abs(Glob.TextBg - Glob.TextHg);
                        scoreRow["enter"] = Glob.回车;
                        scoreRow["duplicate"] = Glob.选重;
                        scoreRow["error"] = Glob.TextCz;
                        scoreRow["back_rate"] = Glob.TextHg_;
                        scoreRow["accuracy_rate"] = UserJz;
                        scoreRow["effciency"] = Glob.效率;
                        scoreRow["keys"] = Glob.TextJs;
                        scoreRow["count"] = TextLen;
                        scoreRow["type_words"] = Glob.aTypeWords;
                        scoreRow["words_rate"] = Glob.TextDc_;
                        scoreRow["cost_time"] = UserTime;
                        scoreRow["segment_id"] = databaseSegmentId;
                        scoreRow["article_title"] = this.lblTitle.Text;
                        scoreRow["version"] = Glob.Instration;
                        scoreRow["difficulty"] = Glob.Difficulty;
                        this.currentScoreData.AddScoreRow(scoreRow);
                        #endregion

                        #region 自动发文
                        if (NewSendText.发文状态)
                        {
                            if (NewSendText.是否自动)
                            { // 自动模式
                                if (NewSendText.AutoCondition)
                                {
                                    bool isNext = false;
                                    switch (NewSendText.AutoKey)
                                    {
                                        case NewSendText.AutoKeyValue.Speed:
                                            isNext = CompareAutoCondition(speed2, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.Keystroke:
                                            isNext = CompareAutoCondition(jj, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.CodeLen:
                                            isNext = CompareAutoCondition(mc, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.AccuracyRate:
                                            isNext = CompareAutoCondition(UserJz, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.BackChange:
                                            isNext = CompareAutoCondition(Glob.TextHg, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.Error:
                                            isNext = CompareAutoCondition(Glob.TextCz, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.BackRate:
                                            isNext = CompareAutoCondition(Glob.TextHg_, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.TypeWords:
                                            isNext = CompareAutoCondition(Glob.aTypeWords, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.WordsRate:
                                            isNext = CompareAutoCondition(Glob.TextDc_, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.Effciency:
                                            isNext = CompareAutoCondition(Glob.效率, NewSendText.AutoNumber);
                                            break;
                                        case NewSendText.AutoKeyValue.Grade:
                                            isNext = CompareAutoCondition(speed2 * Glob.Difficulty, NewSendText.AutoNumber);
                                            break;
                                        default:
                                            isNext = false;
                                            break;
                                    }

                                    if (isNext)
                                    {
                                        SendNextFun();
                                    }
                                    else
                                    {
                                        switch (NewSendText.AutoNo)
                                        {
                                            case NewSendText.AutoNoValue.Retype:
                                                GetInfo();
                                                F3();
                                                break;
                                            case NewSendText.AutoNoValue.Disorder:
                                                this.DisorderToolStripMenuItem.PerformClick();
                                                break;
                                            case NewSendText.AutoNoValue.None:
                                            default:
                                                break;
                                        }
                                    }
                                }
                                else
                                { // 无条件，直接发下一段
                                    SendNextFun();
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

            }
            else
            {
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                timer5.Stop();
                this.lblAutoReType.Text = "0";
                ShowFlowText("字数过少！");
            }
        }

        /// <summary>
        /// 比较自动的条件
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="setting"></param>
        private bool CompareAutoCondition(double cur, double setting)
        {
            switch (NewSendText.AutoOperator)
            {
                case NewSendText.AutoOperatorValue.DY:
                    return cur > setting;
                case NewSendText.AutoOperatorValue.DYDY:
                    return cur >= setting;
                case NewSendText.AutoOperatorValue.XY:
                    return cur < setting;
                case NewSendText.AutoOperatorValue.XYDY:
                    return cur <= setting;
                default:
                    return false;
            }
        }

        //显示回改地点
        private void Show_Hg_Place()
        {
            if (Glob.TextHgPlace.Count > 0)
            {
                foreach (int i in Glob.TextHgPlace)
                {
                    if (i < Glob.TextLen)
                    { //确定少于总字数
                        this.richTextBox1.SelectionStart = i - 1;
                        this.richTextBox1.SelectionLength = 1;
                        //Font font = new Font(this.richTextBox1.SelectionFont, FontStyle.Underline);
                        //this.richTextBox1.SelectionFont = font;

                        //? 显示回改地点的执行位于标识跟打用时最多的词条后
                        this.richTextBox1.SelectionColor = Glob.BackChangeColor;
                    }
                }
            }
        }

        //跟打地图
        private void Type_Map(Color C, float X, float W)
        {
            Bitmap bmp = new Bitmap(this.picMap.Image);//new Bitmap(this.pictureBox1.ClientRectangle.Width, this.pictureBox1.ClientRectangle.Height);
            Glob.Type_Map = Graphics.FromImage(bmp);
            Glob.Type_Map.DrawLine(new Pen(C, W), X, 0, X, bmp.Height);
            //Glob.Type_Map.DrawRectangle(new Pen(C, 1),X,0,W,bmp.Height);
            //Glob.Type_Map.FillRectangle(new (C, 1), X, 0, W, bmp.Height);
            this.picMap.Image = bmp;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SplitterBar(this.textBoxEx1.TextLength / this.richTextBox1.TextLength);
        }

        /// <summary>
        /// 图片进度重绘
        /// </summary>
        /// <param name="pro">百分比</param>
        /// <param name="text">绘制文字</param>
        private void picBar_Draw(double pro, string text)
        {
            Bitmap bmp = new Bitmap(this.picBar.Width, this.picBar.Height);
            Graphics g = Graphics.FromImage(bmp);
            double width = this.picBar.Width * pro;
            //MessageBox.Show("宽度：" + this.picBar.Width + "\n计算：" + width + "\n比例：" + pro);
            Color C;//进度线条
            float f = 1f;//进度宽度
            C = Theme.ThemeColorBG;

            //画进度
            Rectangle rect = new Rectangle(0, 0, (int)width, this.picBar.Height);
            g.FillRectangle(Brushes.GhostWhite, rect);
            g.DrawLine(new Pen(C, f), rect.Width - f + 1, 0, rect.Width - f + 1, (float)rect.Height);
            //画字
            Font F = new Font("宋体", 9f);
            SizeF s = g.MeasureString(text, F);
            int fontWidth = (int)Math.Ceiling(s.Width);
            //if (width >= fontWidth)
            //{
            //    g.DrawString(text, F, Brushes.Black, (float)(width - fontWidth + 2), 1.0f);
            //}
            //else
            g.DrawString(text, F, Brushes.Brown, (float)(this.picBar.Width / 2 - fontWidth / 2), this.picBar.Height / 2 - s.Height / 2);

            this.picBar.Image = bmp;
            SplitterBar(pro);
        }

        private void SplitterBar(double pro)
        {
            //测试 拆分条 的 绘图
            Graphics g_ = this.splitContainer1.CreateGraphics();
            Color Show = Color.DeepSkyBlue;//Color.FromArgb(255 - Theme.ThemeColorBG.R, 255 - Theme.ThemeColorBG.G, 255 - Theme.ThemeColorBG.B);
            g_.Clear(Theme.ThemeColorBG);
            using (SolidBrush sb = new SolidBrush(Show))
            {
                Rectangle r = this.splitContainer1.SplitterRectangle;
                Rectangle r_ = new Rectangle(r.X, r.Y, (int)(r.Width * pro), r.Height);
                g_.FillRectangle(sb, r_);
            }
            g_.Dispose();
        }
        //成绩分析
        private void 跟打分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Glob.Use分析)
            {
                SpeedAn sa = new SpeedAn(Glob.TextTime.ToString("G"), Glob.CurSegmentNum.ToString(), SpeedAnalysis(), Glob.Instration, this);
                sa.Show();
            }
        }

        /// <summary>
        /// 跟打成绩速度分析
        /// </summary>
        /// <returns></returns>
        private string SpeedAnalysis()
        {
            string speedAnGet = "";
            if (Glob.HaveTypeCount <= 0) return speedAnGet;
            double plus = 0;
            //回改影响速度值
            double jj_1 = (Glob.TextJs - Glob.StartKeyLen) / Glob.TypeUseTime;
            double mc_1 = (double)(Glob.TextJs - Glob.StartKeyLen) / (Glob.TextLen - Glob.StartTextLen);
            double speed_1 = (double)(Glob.TextLen - Glob.StartTextLen) * 60 / (Glob.TypeUseTime - Glob.hgAllUse);
            double speed_ = (double)(Glob.TextLen - Glob.StartTextLen) * 60 / Glob.TypeUseTime;
            double Hg_speed = speed_1 - speed_;
            //退格影响速度值
            double mc_2 = (double)(Glob.TextJs - Glob.StartKeyLen - Math.Abs(Glob.TextBg - Glob.TextHg)) / (Glob.TextLen - Glob.StartTextLen);
            double speed_3 = jj_1 * 60 / mc_2;
            double Bg_speed = speed_3 - speed_;
            //回车影响速度值
            double mc_3 = (double)(Glob.TextJs - Glob.StartKeyLen - Glob.回车) / (Glob.TextLen - Glob.StartTextLen);
            double speed_4 = jj_1 * 60 / mc_3;
            double En_speed = speed_4 - speed_;
            //停留影响速度值
            double 平均停留 = (double)Glob.TypeUseTime / (Glob.TextLen - Glob.StartTextLen);
            double 停留 = Glob.TypeReport.Where(o => o.Length > 0).Max(o => o.TotalTime) - 平均停留;
            if (停留 >= Glob.TypeUseTime)
            {
                停留 = 0;
            }
            double speed_5 = (double)(Glob.TextLen - Glob.StartTextLen) * 60 / (Glob.TypeUseTime - 停留);
            double St_speed = Math.Abs(speed_5 - speed_);
            //错字影响速度值
            double speed_6 = (double)(Glob.TextLen - Glob.StartTextLen - Glob.TextCz * 5) * 60 / Glob.TypeUseTime;
            double Cz_speed = speed_ - speed_6;
            //键准理论值
            int Low = Glob.TextJs - Glob.StartKeyLen - Math.Abs((Glob.TextBg - Glob.TextHg)) * 2 - Glob.TextMcc;
            double Jz_mc = (double)Low / (Glob.TextLen - Glob.StartTextLen);
            double Jz_speed = jj_1 * 60 / Jz_mc;
            if (Glob.PauseTimes > 0)
                plus = Hg_speed + Bg_speed + En_speed + Cz_speed;//有暂停时间，不显示停留
            else
                plus = Hg_speed + Bg_speed + En_speed + St_speed + Cz_speed;
            StringBuilder sb = new StringBuilder();
            string 码长理论 = Jz_speed.ToString("0.00");
            string 完美理论 = (speed_6 + plus).ToString("0.00");
            string 跟打实际 = speed_6.ToString("0.00");
            double 实际比较 = speed_6 - Jz_speed;
            double 完美比较 = speed_6 + plus - Jz_speed;
            string 实码比 = (实际比较 > 0 ? "+" : "") + 实际比较.ToString("0.00");
            string 完码比 = (完美比较 > 0 ? "+" : "") + 完美比较.ToString("0.00");
            speedAnGet = $"{完美理论}|{完码比}|{码长理论}|{Glob.PauseTimes}|{跟打实际}|{实码比}|{Hg_speed:0.00}|{Glob.hgAllUse:0.00}|{Bg_speed:0.00}|{Math.Abs(Glob.TextBg - Glob.TextHg):0.00}|{St_speed:0.00}|{停留:0.00}|{Cz_speed:0.00}|{Glob.TextCz}|{En_speed:0.00}|{Glob.回车}";
            return speedAnGet;
        }
        //击键占比
        private int GetMaxAndIndex(int[] pa)
        {
            int index = -1;//定义变量存最大值的索引
            int c = pa.Length;
            if (c != 0)
            {
                int Max = pa[0];
                for (int i = 0; i < c; i++)
                {
                    int nowP = pa[i];
                    if (Max < nowP)
                    {
                        index = i;
                        Max = nowP;
                    }
                }
            }
            return index;
        } //得到数组最大值索引

        private int thepre = -1;
        private Color theForeColor;
        private void jjPerCheck(int jP)
        {
            if (Glob.jjAllC <= 0) return;
            for (int i = 0; i <= 17; i += 2)
            {
                this.dataGridView2.Rows[0].Cells[i].Style.ForeColor = Color.FromArgb(127, 127, 127);
            }
            double 评定击键 = 0, 评定计数 = 0;
            for (int i = 0, j = 1; i < 9; i++, j += 2)
            {
                double jjP = Glob.jjPer[i] * 100.0 / Glob.jjAllC;
                string jj;
                if (jjP != 0)
                {
                    if (jjP >= 10) { 评定击键 += i + jjP / 100.0; 评定计数++; }
                    //this.dataGridView2.Rows[0].Cells[j].ToolTipText = Glob.jjPer[i] + "/" + Glob.jjAllC;
                    if (j >= 13) this.dataGridView2.Rows[0].Cells[j].Style.ForeColor = Color.Black;
                    if (jjP > 0 && jjP < 1)
                        jj = Math.Round(jjP, 1).ToString();
                    else
                        jj = ((int)jjP).ToString();

                    this.dataGridView2.Rows[0].Cells[j].Value = jj;//Math.Round(jjP, 2);
                    if (jjP >= 20 && jjP < 30)
                    {
                        this.dataGridView2.Rows[0].Cells[j - 1].Style.ForeColor = Color.FromArgb(63, 63, 63);
                        this.dataGridView2.Rows[0].Cells[j].Style.ForeColor = Color.FromArgb(223, 77, 85);
                    }
                    else if (jjP >= 30 && jjP < 50)
                    {
                        this.dataGridView2.Rows[0].Cells[j - 1].Style.ForeColor = Color.FromArgb(63, 63, 63);
                        this.dataGridView2.Rows[0].Cells[j].Style.ForeColor = Color.FromArgb(82, 0, 208);
                    }
                    else if (jjP >= 50 && jjP < 60)
                    {
                        this.dataGridView2.Rows[0].Cells[j - 1].Style.ForeColor = Color.FromArgb(63, 63, 63);
                        this.dataGridView2.Rows[0].Cells[j].Style.ForeColor = Color.FromArgb(255, 64, 0);
                    }
                    else if (jjP >= 60)
                    {
                        this.dataGridView2.Rows[0].Cells[j - 1].Style.ForeColor = Color.FromArgb(44, 44, 44);
                        this.dataGridView2.Rows[0].Cells[j].Style.ForeColor = Color.FromArgb(0, 150, 75);
                    }
                    else
                    {
                        if (j < 13)
                            this.dataGridView2.Rows[0].Cells[j].Style.ForeColor = Color.FromArgb(35, 35, 35); //普通击键颜色
                        this.dataGridView2.Rows[0].Cells[j - 1].Style.ForeColor = Color.FromArgb(127, 127, 127);
                    }
                    if (jP >= 4)
                    {
                        if (jP > 12) jP = 12;
                        if (thepre == -1)
                        {
                            thepre = 2 * jP - 8;
                            theForeColor = this.dataGridView2.Rows[0].Cells[thepre].Style.ForeColor;

                            this.dataGridView2.Rows[0].Cells[thepre].Style.BackColor = Color.Black;
                            this.dataGridView2.Rows[0].Cells[thepre].Style.ForeColor = Color.White;
                        }
                        else
                        {
                            this.dataGridView2.Rows[0].Cells[thepre].Style.BackColor = Color.FromArgb(217, 217, 217);
                            this.dataGridView2.Rows[0].Cells[thepre].Style.ForeColor = theForeColor;

                            thepre = 2 * jP - 8;
                            theForeColor = this.dataGridView2.Rows[0].Cells[thepre].Style.ForeColor;

                            this.dataGridView2.Rows[0].Cells[thepre].Style.BackColor = Color.Black;
                            this.dataGridView2.Rows[0].Cells[thepre].Style.ForeColor = Color.White;
                        }
                    }
                }
            }
            JjCheck(jP); //显示击键
            //this.dataGridView2.Rows[0].Cells[(jP - 4) * 2 + 1].Selected = true;
        }
        //表格模式判断八位是否
        private string check10(string sou)
        {
            if (sou.Length < 10)
            {
                for (int i = sou.Length; i <= 10; i++)
                {
                    sou += " ";
                }
            }
            return sou;
        }
        //各项属性(待建)
        private string 回改量
        {
            get
            {
                if (Glob.TextHg != 0)
                {
                    return " 回改" + Glob.TextHg + "(" + Glob.hgAllUse.ToString("0.00") + "s)";
                }
                else
                {
                    return " 回改" + Glob.TextHg;
                }
            }
        }
        /// <summary>
        /// 回改率
        /// 带百分号
        /// </summary>
        private string UserHgl
        {
            get { return Glob.TextHg_.ToString("0.00") + "%"; }
        }
        /// <summary>
        /// 打词率
        /// 带百分号
        /// </summary>
        private string UserDcl
        {
            get { return Glob.TextDc_.ToString("0.00") + "%"; }
        }
        /// <summary>
        /// 跟打用时
        /// 已转换好格式
        /// </summary>
        private string UserTime
        {
            get
            {
                if (Glob.TypeUseTime > 0)
                {
                    DateTime dt = new DateTime().AddSeconds(Glob.TypeUseTime);
                    if (dt.Hour == 0)
                    {
                        return dt.ToString("m:ss.fff");
                    }
                    else
                    {
                        return dt.ToString("hh:mm:ss.fff");
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        private string 键法
        {
            get
            {
                int[] lrKeys = KeyAn.GetLRKeysCount(Glob.KeysTotal);
                if (lrKeys[0] >= lrKeys[1])
                {
                    return $" [左{lrKeys[0]}:{lrKeys[1]}]";
                }
                else
                {
                    return $" [右{lrKeys[1]}:{lrKeys[0]}]";
                }
            }
        }
        private string UserHcText
        {
            get { return " 回车" + Glob.回车; }
        }
        /// <summary>
        /// 退格
        /// </summary>
        private string UserTg
        {
            get { return Math.Abs(Glob.TextBg - Glob.TextHg).ToString(); }
        }
        private string UserXcText
        {
            get { if (Glob.选重 > 0) { return " 选重" + Glob.选重; } else { return ""; } }
        }
        private string 撤销
        {
            get { if (Glob.撤销 > 0) { return " 撤销" + Glob.撤销; } else { return ""; } }
        }
        private string 暂停
        {
            get { if (Glob.PauseTimes > 0) return " 暂停" + Glob.PauseTimes + "次"; else return ""; }
        }
        /// <summary>
        /// 键准
        /// 键准度计算方法：退格一次，相当于两次
        /// </summary>
        private double UserJz
        {
            get
            {
                if (Glob.TextJs <= 0) return 0;
                int Low = Glob.TextJs - Math.Abs(Glob.TextBg - Glob.TextHg) * 2 - Glob.TextMcc;
                if (Low <= 0 || Glob.TextJs <= 0) return 0;
                double 键准度 = (double)Low * 100 / Glob.TextJs;
                if (键准度 > 0.00 && 键准度 <= 100)
                    return Math.Round(键准度, 2);
                else
                    return 0;
            }
        }

        /// <summary>
        /// 连续回改超过1次
        /// </summary>
        private string 连改
        {
            get
            {
                List<TypeDate> find = Glob.TypeReport.FindAll(o => o.Length < 0);
                int count = 0;
                int c_temp = 0;
                for (int i = 0; i < find.Count - 1; i++)
                {
                    for (int j = i + 1; j < find.Count - 2; j++)
                    {
                        if (find[i].Index != find[j].Index - (j - i))
                        {
                            break;
                        }
                        else
                        {
                            c_temp++;
                        }
                    }
                    if (c_temp > 0)
                    {
                        count++;//连改自增
                        i += c_temp;
                        if (i >= find.Count - 1) i = find.Count - 2;
                        c_temp = 0;
                    }
                }
                if (count != 0)
                    return " 连改" + count;//+ "/" + .Sum(o => o.Length)) + "/" + find.Sum(o => o.TotalTime).ToString("0.00") + "s";
                else
                    return "";
            }
        }

        /// <summary>
        /// 版本输出
        /// </summary>
        private string 版本
        {
            get
            {
                string ins = " v" + Glob.Ver + "(" + Glob.Instration + ")";
                if (Glob.TextCz > 0)
                {
                    ins += " [错1罚5]";
                }
                return ins;
            }
        }
        //属性END
        private void labelBM_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int g = 0;
                string lbm = this.labelBM.Text;
                if (!string.IsNullOrEmpty(lbm) && lbm != "-")
                {
                    g = int.Parse(lbm);
                }

                if (g > 0)
                {
                    this.labelBM.ForeColor = Color.IndianRed;
                }
                else
                {
                    this.labelBM.ForeColor = Color.FromArgb(63, 63, 63);
                }
            }
            catch
            {
                this.labelBM.Text = "0";
                this.labelBM.ForeColor = Color.FromArgb(63, 63, 63);
            }
        }

        private void labelhgstatus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int g = int.Parse((sender as Label).Text);
                if (g > 0)
                {
                    (sender as Label).ForeColor = Color.DarkRed;
                }
                else
                {
                    (sender as Label).ForeColor = Color.FromArgb(191, 191, 191);
                }
            }
            catch
            {
                (sender as Label).Text = "0";
                (sender as Label).ForeColor = Color.FromArgb(191, 191, 191);
            }
        }

        /// <summary>
        /// 停止时间计时
        /// - 超过设定的停止时间时自动重打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer5_Tick(object sender, EventArgs e)
        {
            if (sw > 0)
            {
                TimeSpan span = DateTime.Now - Glob.nowStart;
                int now = (int)span.TotalSeconds;
                this.lblAutoReType.Text = now.ToString();
                if (now > Glob.StopUseTime * 60)
                {
                    F3();

                    this.lblAutoReType.Text = "0";
                    MessageBox.Show("长时间未跟打，已自动重打！\n可在\"设置\"→\"程序控制\"→\"离开时间\"处调整。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public string 字数格式化(int 字数)
        {
            if (字数 >= 10000)
            {
                return Math.Round((double)字数 / 10000, 2).ToString("0.00") + "万";
            }
            else
            {
                return 字数 + "";
            }
        }

        private void 复制图片成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Glob.Use分析)
            {
                CopyPicGoal();
            }
        }

        /// <summary>
        /// 复制图片成绩到剪贴板中
        /// </summary>
        private void CopyPicGoal()
        {
            using (PicGoal_Class pgc = new PicGoal_Class())
            {
                Clipboard.SetImage(pgc.GetPic(this.lblTitle.Text, Glob.TextTime.ToString("G"), UserTime, UserJz, Glob.效率, Glob.TextLen, Glob.TextHg, Glob.TextCz, Glob.TextJs, Math.Abs(Glob.TextBg - Glob.TextHg), Glob.选重, Glob.CurSegmentNum.ToString(), Glob.TextSpeed, Glob.Textjj, Glob.Textmc, Glob.Instration));
                pgc.Dispose();
            }
        }

        private void textBoxEx1_KeyDown(object sender, KeyEventArgs e)
        { //! 键盘钩子的触发是在跟打区 KeyDown 事件之前
            if (e.Alt)
            {
                e.Handled = true;
            }

            //* 不统计 Ctrl 和 F区 键数
            if (e.KeyCode != Keys.ControlKey && (e.KeyCode < Keys.F1 || e.KeyCode > Keys.F12))
            {
                Glob.TextJs++;
                labelJsing.Text = Glob.TextJs.ToString();
                Glob.nowStart = DateTime.Now; //停止用时
            }

            //* 英文文章不存在选重
            if (e.KeyCode == Keys.ProcessKey)
            {
                Glob.是否选重 = true;
            }
            else
            {
                Glob.是否选重 = false;
            }

            if (Glob.文段类型 && Glob.是否选重 && Glob.TheKeyValue != -1)
            {
                //? 这种方法存在一定的风险，如果出现键盘钩子触发在本事件之后的情况时，会丢失统计
                //? 但如果在键盘钩子触发中判定，由于 Glob.是否选重 的变动置后，当在跟打区中错误地按下数字等键时将无法与提取的字符匹配，会被统计成选重
                //? 所以这种方法会更准确

                if (Glob.TheKeyValue == 186 || Glob.TheKeyValue == 222)
                {
                    if (Glob.UseSymbolSelect)
                    { // 符号选重的情况
                        Glob.选重++;
                    }
                }
                else if (Glob.TheKeyValue >= 48 && Glob.TheKeyValue <= 57)
                { // 数字选重的情况
                    Glob.选重++;
                }
            }

            if (e.KeyCode == Keys.Back)
            {
                Glob.TextHg++;
                //回改地点
                if (!Glob.TextHgPlace.Contains(this.textBoxEx1.TextLength))
                    Glob.TextHgPlace.Add(this.textBoxEx1.TextLength);

                Glob.TextMcc += Glob.TextMc; //在此回退的情况 键准处理
                labelhgstatus.Text = Glob.TextHg.ToString();//回改
                Glob.nowStart = DateTime.Now; //停止用时
                if (this.textBoxEx1.TextLength == 0)
                {
                    F3();
                }
            }

            if (isPause)
            { // 结束暂停，重新启动跟打
                sTime = DateTime.Now; //* 记录重启时间
                timer1.Start();
                timer2.Start();
                timer3.Start();
                timer5.Start();
                EndPause();
            }
        }

        private void textBoxEx1_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = textBoxEx1.Text; // 将跟打区内的文字传递至文本处理区(不可见)
            Glob.TypeTextCount = textBoxEx1.TextLength;

            if (this.picBmTips.Checked && !string.IsNullOrEmpty(Glob.UsedTableIndex))
            {
                QueryWordCode();
            }
        }
        #endregion

        [DllImport("User32")]
        public extern static bool GetCursorPos(ref Point cPoint); // 输出当前鼠标光标位置

        #region 全局快捷键设置
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            const int WM_NCHITTEST = 0x0084;


            int HTCLIENT = 1;
            int HTLEFT = 10;
            int HTRIGHT = 11;
            int HTTOP = 12;
            int HTTOPLEFT = 13;
            int HTTOPRIGHT = 14;
            int HTBOTTOM = 15;
            int HTBOTTOMLEFT = 16;
            int HTBOTTOMRIGHT = 17;

            int offset = 3;

            switch (m.Msg)
            {
                case WM_HOTKEY:
                    //* 现在只会处理老板键的全局热键
                    ProcessHotKey(m.WParam.ToInt32());
                    break;

                case WM_NCHITTEST:
                    int px = Form.MousePosition.X - this.Left;
                    int py = Form.MousePosition.Y - this.Top;

                    int temp;

                    if (px >= this.Width - offset)
                    {
                        if (py <= offset) temp = HTTOPRIGHT;
                        else if (py >= this.Height - offset) temp = HTBOTTOMRIGHT;
                        else temp = HTRIGHT;
                    }
                    else if (px <= offset)
                    {
                        if (py <= offset) temp = HTTOPLEFT;
                        else if (py >= this.Height - offset) temp = HTBOTTOMLEFT;
                        else temp = HTLEFT;
                    }
                    else
                    {
                        if (py <= offset) temp = HTTOP;
                        else if (py >= this.Height - offset) temp = HTBOTTOM;
                        else temp = HTCLIENT;
                    }
                    m.Result = (IntPtr)temp;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
            //base.WndProc(ref m);
        }

        private void richText2Event(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Z & e.Control)
            {
                Glob.撤销++;
                this.textBoxEx1.Undo();
                //MessageBox.Show(this.textBox1.CanUndo.ToString());
            }
        }

        /// <summary>
        /// 格式载文
        /// </summary>
        public void PutText()
        {
            Glob.Text = Clipboard.GetText().Trim(); //获取到跟打文字
            if (Glob.Text.Length == 0) { return; }
            //! 停止发文
            this.StopSendFun();

            string text_ = Glob.Text;
            //MessageBox.Show(text_);
            //SwitchToThisWindow(FindWindow(null, Glob.Form), true);
            string pretext, preduan;
            if (Glob.IsZdyPre)
            {
                pretext = Glob.PreText.Replace(@"\", @"\\");
                preduan = Glob.PreDuan.Replace("xx", @"\d+");
            }
            else
            {
                pretext = "-----";
                preduan = @"第\d+段";
            }

            Regex regexAll = new Regex(@".+\s.+\s" + pretext + preduan + ".*", RegexOptions.RightToLeft); //获取发送的全部信息
            Glob.getDuan = regexAll.Match(text_);
            if (Glob.getDuan.Length == 0) //为空
            {
                ShowFlowText("未找到文段");
                return;
            }

            if (Glob.IsZdyPre)
            {
                Glob.regexCout = new Regex(@"(?<=" + preduan.Substring(0, 1) + @")\d+(?=" + preduan.Substring(4, 1) + ")", RegexOptions.RightToLeft);
            }
            else
                Glob.regexCout = new Regex(@"(?<=第)\d+(?=段)", RegexOptions.RightToLeft);
            LoadText(pretext, preduan, Glob.regexCout, Glob.getDuan.ToString());
        }

        /// <summary>
        /// 载文功能
        /// </summary>
        /// <param name="pretext">前导符</param>
        /// <param name="preduan">段标</param>
        /// <param name="regexCout">获取段号的正则</param>
        /// <param name="getDuanAll">载文的所有段内容</param>
        public void LoadText(string pretext, string preduan, Regex regexCout, string getDuanAll)
        {
            Initialize(1);//数值初始化
            string PerText = richTextBox1.Text; //之前的文段
            Regex regexText, regexTitle;
            Match getText, getCout, getTitle;
            regexText = new Regex(@".+(?=\s" + pretext + ")");
            regexTitle = new Regex(@".+(?=\s)");
            getText = regexText.Match(getDuanAll); //获取文章
            string ExgetText = getText.ToString().Trim();
            getCout = regexCout.Match(getDuanAll);//获取段号
            getTitle = regexTitle.Match(getDuanAll); //获取标题
            //填入及初始化各项值
            timer1.Enabled = false;

            if (ExgetText != "")
            {
                if (ExgetText != PerText) //获取新文段
                {
                    richTextBox1.Text = ExgetText.ToString(); //填入文章

                    this.textBoxEx1.TextChanged -= new System.EventHandler(textBoxEx1_TextChanged);
                    textBoxEx1.Clear();
                    this.textBoxEx1.TextChanged += new System.EventHandler(textBoxEx1_TextChanged); //重新绑定
                    Initialize(2);//显示初始化
                    //处理文章
                    lblTitle.Text = getTitle.ToString().Trim(); //文段标题
                    toolTip1.SetToolTip(lblTitle, getTitle.ToString().Trim());
                    try
                    {
                        lblDuan.Text = preduan[0].ToString() + getCout + preduan[preduan.Length - 1];
                    }
                    catch
                    {
                        lblDuan.Text = "第" + getCout + "段";
                    }
                    this.title1.Text = lblDuan.Text + " -" + Glob.Instration;
                    Glob.CurSegmentNum = int.Parse(getCout.ToString());
                    textBoxEx1.ReadOnly = false;
                    //richTextBox2.MaxLength = richTextBox1.TextLength; //设置最大输入字符数量

                    if (Glob.SpeedControl > 0)
                    {   //* 找到新文段时关闭测速
                        Glob.SpeedPoint_ = new int[10];//测速点控制
                        Glob.SpeedTime = new double[10];//测速点时间控制
                        Glob.SpeedJs = new int[10];//键数
                        Glob.SpeedHg = new int[10];//回改
                        Glob.SpeedPointCount = 0;//测速点数量控制
                        Glob.SpeedControl = 0;
                        this.lblspeedcheck.Text = "时间";
                    }
                    if (getCout.ToString() == "999")
                    { // 比赛认证段
                        SetMatch(true);
                    }
                    else
                    {
                        SetMatch(false);
                    }

                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        ListDuan(getCout.ToString());//列出段号
                    }));

                }
                else
                {
                    if (textBoxEx1.Text != "")
                    {
                        ShowFlowText("未找到新文段");
                    }
                }
            }
            else
            {
                ShowFlowText("未找到文段");
            }
            GetInfo(); //获取文字信息
            richTextBox1.ForeColor = Color.Black;
            Clipboard.Clear(); // 清空剪贴板
            this.Activate();
        }

        /// <summary>
        /// 直接跟打内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="cout"></param>
        /// <param name="title"></param>
        public void TypeContentDirectly(string content, string cout, string title)
        {
            if (content != "")
            {
                //! 停止发文
                this.StopSendFun();

                //* 清理原测速点信息
                this.CleanSpeedPoints();
                this.cmsDuanList.Items.Clear();

                Glob.CurSegmentNum = int.Parse(cout);
                this.lblDuan.Text = "第" + Glob.CurSegmentNum.ToString() + "段";
                this.lblTitle.Text = title;
                Initialize(1);
                this.textBoxEx1.TextChanged -= new System.EventHandler(textBoxEx1_TextChanged);
                this.textBoxEx1.Clear();
                this.textBoxEx1.TextChanged += new System.EventHandler(textBoxEx1_TextChanged); //重新绑定
                this.richTextBox1.Text = content;
                GetInfo();
                F3();
                timer1.Stop();
                timer3.Stop();
            }
        }

        public void SetMatch(bool set)
        {
            if (set)
            {
                Glob.isMatch = true;
                lblDuan.Text = "比赛认证段";
                if (this.比赛时自动打开寻找测速点ToolStripMenuItem.Checked)
                {
                    this.自动寻找赛文标记ToolStripMenuItem.PerformClick();
                }
                ShowFlowText("当前为比赛认证段");
            }
            else
            {
                Glob.isMatch = false;
                lblDuan.Text = "第" + Glob.CurSegmentNum.ToString() + "段";
            }
        }

        private void 转换为比赛文段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!NewSendText.发文状态)
            {
                if (Glob.isMatch)
                {
                    SetMatch(false);
                }
                else
                {
                    SetMatch(true);
                }
            }
            else
            {
                ShowFlowText("仅载文模式下可以使用~");
            }
        }

        public void F3()
        {
            this.textBoxEx1.TextChanged -= new System.EventHandler(textBoxEx1_TextChanged);
            textBoxEx1.Clear();
            this.textBoxEx1.TextChanged += new System.EventHandler(textBoxEx1_TextChanged);

            timer1.Enabled = false;
            timer3.Enabled = false;
            this.SeriesSpeed.Points.Clear();
            Glob.ChartSpeedArr.Clear();
            textBoxEx1.ReadOnly = false;
            Initialize(1);//数值初始化
            Initialize(2);//显示初始化

            textBoxEx1.Select();
            timer5.Stop();
            Glob.nowStart = DateTime.Now;
            this.lblAutoReType.Text = "0";

            if (Glob.SpeedPointCount > 0)
            {
                for (int i = 0; i < Glob.SpeedPointCount; i++)
                {
                    this.richTextBox1.SelectionStart = Glob.SpeedPoint_[i];
                    this.richTextBox1.SelectionLength = 1;
                    this.richTextBox1.SelectionBackColor = Glob.TestMarkColor;
                }
                Array.Clear(Glob.SpeedTime, 0, Glob.SpeedTime.Length);
                Array.Clear(Glob.SpeedJs, 0, Glob.SpeedJs.Length);
                Array.Clear(Glob.SpeedHg, 0, Glob.SpeedHg.Length);
                Glob.SpeedControl = 0;
                this.lblspeedcheck.Text = "测速点:" + Glob.SpeedPointCount;
            }
            else
            {
                this.lblspeedcheck.Text = "时间";
            }
            //GC.Collect();
        }

        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        /// <summary>
        /// 获取信息
        /// - 自动替换英文标点在此处执行
        /// </summary>
        public void GetInfo()
        {
            if (Glob.autoReplaceBiaodian)
            { //* 标点替换
                this.richTextBox1.Text = ReText(this.richTextBox1.Text);
            }

            var tl = richTextBox1.TextLength;
            Glob.TextLen = tl;
            Glob.TypeText = richTextBox1.Text; // 存储跟打文字

            //* 计算难度
            Glob.Difficulty = DiffDict.Calc(Glob.TypeText);

            textBoxEx1.MaxLength = tl;
            lblCount.Text = tl.ToString() + "字";
            DifficultyLabel.Text = DiffDict.DiffText(Glob.Difficulty);
            lblMatchCount.Text = Validation.Validat(Validation.Validat(richTextBox1.Text));

            //? 为了能处理中途更换或停用码表等特殊情况，在手动按下重打时也会重新计算
            Glob.BmAlls.Clear();
            Array.Clear(Glob.CalcKeysTotal, 0, 50); // 清空按键统计
            Glob.词库理论码长 = 0;

            if (!string.IsNullOrEmpty(Glob.UsedTableIndex))
            {
                if (checkWordTask != null)
                {
                    checkWordTask.Dispose();
                    Glob.IsChecking = false;
                }

                if (Glob.是否智能测词)
                { //* 智能测词
                    if (开始测词委托 == null)
                    {
                        开始测词委托 = new checkWordsDelegate(SmartCheckWords);
                    }
                }
                else
                { //* 不测词，根据单字字典计算理论码长
                    if (开始测词委托 == null)
                    {
                        开始测词委托 = new checkWordsDelegate(CalcSingleCodeLen);
                    }
                }

                checkWordTask = Task.Factory.StartNew(() =>
                {
                    this.richTextBox1.BeginInvoke(开始测词委托);
                });
            }

            if (Glob.binput)
            {
                var chineseRegex = new Regex(@"[\u4E00-\u9FA5]");
                if (chineseRegex.IsMatch(Glob.TypeText))
                {
                    Glob.binput = false;
                    Glob.文段类型 = true;
                }
                else
                {
                    Glob.binput = true;
                    Glob.文段类型 = false;
                }
            }
        }
        #endregion

        #region 跟打过程中的控制

        #region 暂停处理
        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool isPause = false;

        private void 暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PauseType())
            {
                MessageBox.Show("暂停启动失败！或已暂停！");
            }
        }

        /// <summary>
        /// 暂停处理方法
        /// </summary>
        /// <returns></returns>
        public bool PauseType()
        {
            if (!isPause && sw > 0 && this.richTextBox1.Text.Length != this.textBoxEx1.Text.Length)
            {
                try
                {
                    timer1.Stop(); // 跟打计时停止
                    timer2.Stop(); // 实时速度等显示
                    timer3.Stop(); // 跟打曲线统计
                    timer5.Stop(); // 离开时间计时
                    isPause = true;
                    //? 由于暂停结束后重新启动跟打时，sTime 会被重新设定，
                    //? 则 timer 计时器的下一个触发点到达后,所记录的时间间隔会缺失掉上一个触发点到暂停点的时间间隔，需要自行手动弥补
                    TimeSpan span = DateTime.Now - sTime;
                    allUsedTime = span + recordUsedTime;
                    recordUsedTime = allUsedTime; //* 记录，因为重启跟打后 sTime 重新设定

                    // 显示跟打暂停的即时时间
                    DateTime showTime = new DateTime(allUsedTime.Ticks);
                    labelTimeFlys.Text = showTime.ToString("mm:ss.ff"); // 时间显示区显示时间
                    Glob.TypeUseTime = allUsedTime.TotalSeconds; //计算总秒数 小数点后两位

                    if (Glob.ShowRealTimeData)
                    {
                        this.labelSpeeding.Text = (this.textBoxEx1.Text.Length * 60 / Glob.TypeUseTime).ToString("0.00");
                    }
                    timerLblTime.Start(); // 暂停时跟打用时闪烁
                    this.Text += " [已暂停]";
                    Glob.PauseTimes++;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //失去焦点自动暂停
        private void textBoxEx1_LostFocus(object sender, EventArgs e)
        {
            PauseType();
        }

        #region 暂停时跟打用时闪烁
        private bool LblTimeFlash = true;
        private void timerLblTime_Tick(object sender, EventArgs e)
        {
            if (LblTimeFlash)
            {
                labelTimeFlys.ForeColor = Color.IndianRed;
                LblTimeFlash = false;
            }
            else
            {
                labelTimeFlys.ForeColor = Color.FromArgb(244, 244, 244);
                LblTimeFlash = true;
            }
        }

        /// <summary>
        /// 暂停结束处理
        /// </summary>
        private void EndPause()
        {
            timerLblTime.Stop();
            LblTimeFlash = false;
            labelTimeFlys.ForeColor = Color.Black;
            isPause = false;
            this.Text = Glob.Form;
        }
        #endregion

        private void labelTimeFlys_Click(object sender, EventArgs e)
        { // 点击时间显示区暂停
            PauseType();
        }
        #endregion

        /// <summary>
        /// 跟打时间计时
        /// - 100 ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan span = DateTime.Now - sTime;
            allUsedTime = span + recordUsedTime;
            DateTime showTime = new DateTime(allUsedTime.Ticks);
            labelTimeFlys.Text = showTime.ToString("mm:ss.ff"); // 时间显示区显示时间
            Glob.TypeUseTime = allUsedTime.TotalSeconds; //计算总秒数 小数点后两位
        }

        /// <summary>
        /// 实时数据计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            int inputL = textBoxEx1.TextLength;
            if (inputL > Glob.StartTextLen)
            {
                int len = richTextBox2.TextLength - Glob.StartTextLen;
                double speed2 = (double)len * 60 / Glob.TypeUseTime;
                if (speed2 > 999) { speed2 = 999; }
                Glob.chartSpeedTo = speed2;
                double mc = (double)Glob.TextJs / (inputL);
                double jj = (Glob.TextJs - Glob.StartKeyLen) / Glob.TypeUseTime;

                if (Glob.ShowRealTimeData)
                {
                    labelmcing.Text = mc.ToString("0.00");
                    labelSpeeding.Text = speed2.ToString("0.00");
                    labelJjing.Text = jj.ToString("0.00");
                }

                if (inputL > 10)
                {
                    if (speed2 > Glob.MaxSpeed)
                        Glob.MaxSpeed = speed2;
                    if (jj > Glob.MaxJj)
                        Glob.MaxJj = jj;
                    if (mc < Glob.MaxMc)
                        Glob.MaxMc = mc;
                }
            }
        }
        #endregion

        #region 关闭后的设置
        private void CloseTyping(object sender, FormClosedEventArgs e)
        {
            int tX = this.Location.X;//横坐标
            int tY = this.Location.Y;
            int tW = this.Size.Width;
            int tH = this.Size.Height;
            Rectangle rect = SystemInformation.WorkingArea;
            int width = rect.Width;
            int height = rect.Height;

            _Ini iniSetup = new _Ini("config.ini");
            iniSetup.IniWriteValue("窗口位置", "横", tX.ToString());
            iniSetup.IniWriteValue("窗口位置", "纵", tY.ToString());
            // 当宽度和高度均小于主监视器的工作区时才保存记录
            if (tW <= width && tH <= height)
            {
                iniSetup.IniWriteValue("窗口位置", "宽", tW.ToString());
                iniSetup.IniWriteValue("窗口位置", "高", tH.ToString());
            }

            if (!isShowAll)
            {
                if (tW <= width && tH <= height)
                {
                    int p11H = this.splitContainer1.Panel1.ClientSize.Height;
                    int p31H = this.splitContainer3.Panel1.ClientSize.Height;
                    //if (this.toolStripButton4.Checked)
                    //{
                    //    p11H = Glob.p1;
                    //    p31H = Glob.p2;
                    //}

                    iniSetup.IniWriteValue("拖动条", "高1", p11H.ToString());
                    iniSetup.IniWriteValue("拖动条", "高2", p31H.ToString());
                }
            }

            iniSetup.IniWriteValue("记录", "总字数", Glob.TextLenAll.ToString());
            iniSetup.IniWriteValue("记录", "总回改", Glob.TextHgAll.ToString());
            iniSetup.IniWriteValue("记录", "总按键", string.Join("|", Glob.HistoryKeysTotal));
            iniSetup.IniWriteValue("今日跟打", DateTime.Today.ToShortDateString(), Glob.todayTyping.ToString());
            iniSetup.IniWriteValue("记录", "记录天数", Glob.TextRecDays.ToString());

            for (int i = 0; i < 9; i++)
            {
                iniSetup.IniWriteValue("记录", i.ToString(), Glob.jjPer[i].ToString());
            }
            iniSetup.IniWriteValue("记录", "总数", Glob.jjAllC.ToString());

            iniSetup.IniWriteValue("评级", "段数", Glob.SpeedGradeCount.ToString());
            iniSetup.IniWriteValue("评级", "速度", Glob.SpeedGradeSpeed.ToString());
            iniSetup.IniWriteValue("评级", "难度", Glob.SpeedGradeDiff.ToString());
            iniSetup.IniWriteValue("评级", "结果", Glob.SpeedGrade.ToString());

            // 关闭数据库
            Glob.ScoreHistory.CloseDatabase();
            Glob.ArticleHistory.CloseDatabase();
            Glob.SentHistory.CloseDatabase();
            Glob.CodeHistory.CloseDatabase();
        }

        #endregion

        #region 历史
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileStringA(string segName, string keyName, string sDefault, byte[] buffer, int iLen, string fileName); // ANSI版本

        public ArrayList ReadKeys(string sectionName)
        {
            string str1 = Environment.CurrentDirectory;
            byte[] buffer = new byte[5120];
            int rel = GetPrivateProfileStringA(sectionName, null, "", buffer, buffer.GetUpperBound(0), str1 + "\\config.ini");

            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }
        #endregion

        #region 屏蔽输入
        private void maKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
            }
        }
        #endregion

        #region 即时图表 1000ms
        /// <summary>
        /// 统计跟打曲线数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (this.textBoxEx1.TextLength > Glob.StartTextLen)
            {
                try
                {
                    //? 手动使 X 轴坐标从 0 开始
                    //this.SeriesSpeed.Points.AddXY(this.SeriesSpeed.Points.Count, Glob.chartSpeedTo);
                    //* X 轴坐标从 1 开始
                    this.SeriesSpeed.Points.AddY(Glob.chartSpeedTo);
                    Glob.ChartSpeedArr.Add(Math.Round(Glob.chartSpeedTo, 1));
                    //System.Diagnostics.Debug.Write(Glob.chartSpeedTo.ToString() + ", ");
                    if (Glob.chartSpeedTo > 0)
                    {
                        if (Glob.chartSpeedTo < Glob.MinSplite)
                        {
                            Glob.MinSplite = Glob.chartSpeedTo;
                            this.ChartArea1.AxisY.Minimum = (int)(Glob.MinSplite / 20) * 10;
                        }
                        this.ChartArea1.AxisX.Interval = this.SeriesSpeed.Points.Count / 5;
                    }
                }
                catch { }
            }
        }
        #endregion

        #region 点击显示
        private void SkipToType(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            char g = richTextBox1.GetCharFromPosition(p);
            int idex = richTextBox1.GetCharIndexFromPosition(p);
            if (e.Button == MouseButtons.Left)
                if (textBoxEx1.TextLength != 0)
                {
                    textBoxEx1.SelectionStart = idex;
                    textBoxEx1.SelectionLength = 1;
                    textBoxEx1.ScrollToCaret();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    MessageBox.Show(g.ToString());
                }
        }
        #endregion

        #region 功能菜单
        private void 设置ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.TopMost) { this.TopMost = false; 保持窗口最前ToolStripMenuItem1.Checked = false; }

            //! 打开设置页面前禁用全局老板键
            if (Glob.HotKeyList.Last().GetKeys() != "None")
            {
                UnregisterHotKey(this.Handle, 100);
            }

            TSetup setupBox = new TSetup(this);
            setupBox.ShowDialog();
        }

        private void 新发文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TopMost) { this.TopMost = false; 保持窗口最前ToolStripMenuItem1.Checked = false; }
            if (NewSendText.发文状态)
            {
                switch (MessageBox.Show("正在发文中，请问你要做什么？\r\n选择是：停止当前发文，打开新发文\r\n选择否：打开当前发文状态", "发文询问", MessageBoxButtons.YesNoCancel))
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        NewSendText.发文状态 = false;
                        if (发文状态窗口 != null)
                        {
                            if (!发文状态窗口.IsDisposed)
                                发文状态窗口.Close();
                        }
                        新发文 NewSendTextForm = new 新发文(this);
                        if (NewSendTextForm.ShowDialog() == DialogResult.Cancel)
                        {
                            this.textBoxEx1.Focus();
                        }
                        break;
                    case System.Windows.Forms.DialogResult.No:
                        if (发文状态窗口 != null)
                        {
                            if (发文状态窗口.IsDisposed)
                            {
                                发文状态窗口 = new SendTextStatic(this.Location, this);
                                MagneticMagnager mm = new MagneticMagnager(this, 发文状态窗口, MagneticPosition.Left);
                            }
                            if (!发文状态窗口.Visible)
                            {
                                发文状态窗口.Show(this);
                            }
                            this.Focus();
                        }
                        else
                        { //? 从逻辑上来说似乎是触发不到这个位置的
                            发文状态窗口 = new SendTextStatic(this.Location, this);
                            发文状态窗口.Show(this);
                            this.Focus();
                        }
                        break;
                }
            }
            else
            {
                新发文 NewSendTextForm = new 新发文(this);
                if (NewSendTextForm.ShowDialog() == DialogResult.Cancel)
                { //! 用于处理跟打区会出现无法获取到输入焦点的问题。即：若打开发文设置前焦点在跟打区以外的控件时，关闭窗口后焦点也只会返还到原控件上。
                  //? 测试在发文设置窗口完全关闭前，在发文方法中给予跟打区焦点的方法都是无法生效的。
                  //? 因为这是一个 ShowDialog 窗口，所以在完全关闭前窗口的焦点仍旧只能位于最前端发文设置窗口中。
                    this.textBoxEx1.Focus();
                }
            }
        }

        private void 复制正在跟打的文段ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string text = richTextBox1.Text;
            if (text != "")
            {
                string textTitle = lblTitle.Text;
                string pre = "-----第" + Glob.CurSegmentNum.ToString() + "段";
                if (Glob.isMatch)
                {
                    pre += "-赛文验证:" + Validation.Validat(Validation.Validat(text));
                }
                string texttotal = textTitle + "\r\n" + text + "\r\n" + pre + "-" + Glob.Instration + "-分享发文";
                ClipboardHandler.SetTextToClipboard(texttotal);
            }
            else
            {
                MessageBox.Show("当前无文段！");
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        private void Form_Action(object sender, EventArgs e)
        {
            this.textBoxEx1.SelectionStart = this.textBoxEx1.TextLength;
        }

        private void 保持窗口最前ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!保持窗口最前ToolStripMenuItem1.Checked)
            {
                保持窗口最前ToolStripMenuItem1.Checked = true;
                this.TopMost = true;
            }
            else
            {
                保持窗口最前ToolStripMenuItem1.Checked = false;
                this.TopMost = false;
            }
        }

        private void 上一次成绩ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Glob.theLastGoal.Length != 0)
            {
                ClipboardHandler.SetTextToClipboard(Glob.theLastGoal + " *");
            }
        }

        #endregion

        #region 检查更新

        private void 检查更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var upgrade = new UpgradePro();
            upgrade.ShowDialog();
        }

        private void 打开下载地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://dogegg.ys168.com/");
        }

        private void 访问官方网站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/LightAPIs/ytgdq");
        }
        #endregion

        #region 编码查询

        /// <summary>
        /// 菜单栏重绘用于显示理论码长 及 词组的编码提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mS1_Paint(object sender, PaintEventArgs e)
        {
            if (!string.IsNullOrEmpty(Glob.UsedTableIndex) && picBmTips.Checked && Glob.词库理论码长 > 0)
            {
                var g = e.Graphics;
                var str = Glob.词组编码 + "理论：" + Glob.词库理论码长.ToString("0.00");
                var siz = g.MeasureString(str, mS1.Font);
                g.DrawString(str, mS1.Font, new SolidBrush(Theme.ThemeColorFC), mS1.Width - siz.Width,
                             mS1.Height - siz.Height);
            }
        }

        /// <summary>
        /// 智能测词
        /// </summary>
        private void SmartCheckWords()
        {
            string _text = Glob.TypeText;
            if (string.IsNullOrEmpty(Glob.UsedTableIndex))
            {
                return;
            }
            if (string.IsNullOrEmpty(_text))
            {
                return;
            }
            if (Glob.IsChecking)
            {
                return;
            }

            int textLen = _text.Length;
            DateTime startTime = DateTime.Now;

            for (int i = 0; i < textLen; i++)
            {
                string startStr = _text[i].ToString();
                if (!Glob.AllWordDic.ContainsKey(startStr))
                { // 不在码表中，可能是符号或未收录的字等
                    Glob.BmAlls.Add(new BmAll
                    {
                        查询的字 = startStr,
                        编码 = KeyObj.TransCode(startStr),
                        重数 = 0,
                        起点 = i,
                        终点 = i + 1
                    });
                    continue;
                }

                int end = i + 1;
                int tempSearch = 0; // 继续搜索的长度
                for (int j = i + 1; j < textLen; j++)
                {
                    if (tempSearch >= Glob.WordMaxLen - 1)
                    {
                        break;
                    }
                    string temp = _text[j].ToString();
                    if (!Glob.AllWordDic.ContainsKey(temp))
                    {
                        break;
                    }
                    tempSearch++;
                }
                end += tempSearch;

                if (end > textLen)
                { // 防止溢出
                    end = textLen;
                }

                for (int k = end; k > i; k--)
                {
                    if (k - i > Glob.WordMaxLen || Glob.WordLenType[k - i - 1] == 0)
                    { // 过长或码表中不存在该长度的词
                        continue;
                    }

                    string curStr = _text.Substring(i, k - i);
                    if (Glob.AllWordDic.ContainsKey(curStr))
                    { // 存在该词条
                        string bm = Glob.AllWordDic[curStr];
                        int findit = 1;
                        if (Glob.AllCodeDic.ContainsKey(bm) && Glob.AllCodeDic[bm] != curStr)
                        {
                            int tempIndex = 2;
                            while (Glob.AllCodeDic.ContainsKey(bm + tempIndex.ToString()) && Glob.AllCodeDic[bm + tempIndex.ToString()] != curStr)
                            {
                                tempIndex++;
                            }
                            findit = tempIndex;
                        }

                        Glob.BmAlls.Add(new BmAll
                        {
                            查询的字 = curStr,
                            编码 = bm,
                            重数 = findit - 1,
                            起点 = i,
                            终点 = k,
                        });
                        i = k - 1; //? 因为 for 循环中 i 会自加
                        break;
                    }
                }
            }

            if (Glob.BmAlls.Count == 0)
            {
                this.UIThread(() =>
                {
                    ShowFlowText("没有查询到任何编码！");
                    this.richTextBox1.ClearLines();
                });
                Glob.IsChecking = false;
                return;
            }

            if (Glob.IsPointIt)
            {
                delayActionModel.Debounce(100, this, new Action(() =>
                {
                    this.richTextBox1.Render(Glob.BmAlls, Glob.RightBGColor); //* 绘制标注线条
                }));
            }

            BeginInvoke(new MethodInvoker(() =>
            { // 计算理论码长
                string codeStr = CalcCodeString(Glob.BmAlls, Glob.AllWordDic, Glob.AllCodeDic);

                Glob.词库理论码长 = (double)codeStr.Length / textLen;

                for (int c = 0; c < codeStr.Length; c++)
                {
                    if (KeyObj.KeysStringDic.ContainsKey(codeStr[c].ToString()))
                    {
                        Glob.CalcKeysTotal[KeyObj.KeysStringDic[codeStr[c].ToString()]]++;
                    }
                }

                ShowFlowText(string.Format("第{0}段，计算码长为：{1}，用时：{2}秒", Glob.CurSegmentNum.ToString(), Glob.词库理论码长.ToString("0.00"), (DateTime.Now - startTime).TotalSeconds.ToString("0.000")));

                mS1.Invalidate();
                Glob.IsChecking = false;
            }));
        }

        /// <summary>
        /// 仅计算单字情况下码长
        /// - 未启用智能测词时
        /// </summary>
        private void CalcSingleCodeLen()
        {
            string _text = Glob.TypeText;
            if (string.IsNullOrEmpty(Glob.UsedTableIndex))
            {
                return;
            }
            if (string.IsNullOrEmpty(_text))
            {
                return;
            }
            if (Glob.IsChecking)
            {
                return;
            }

            int textLen = _text.Length;
            DateTime startTime = DateTime.Now;

            for (int i = 0; i < textLen; i++)
            {
                string singleStr = _text[i].ToString();
                if (Glob.SingleWordDic.ContainsKey(singleStr))
                { // 存在这个字
                    string bm = Glob.SingleWordDic[singleStr];
                    int findit = 1;
                    if (Glob.SingleCodeDic.ContainsKey(bm) && Glob.SingleCodeDic[bm] != singleStr)
                    {
                        int tempIndex = 2;
                        while (Glob.SingleCodeDic.ContainsKey(bm + tempIndex.ToString()) && Glob.SingleCodeDic[bm + tempIndex.ToString()] != singleStr)
                        {
                            tempIndex++;
                        }
                        findit = tempIndex;
                    }

                    Glob.BmAlls.Add(new BmAll
                    {
                        查询的字 = singleStr,
                        编码 = bm,
                        重数 = findit - 1,
                        起点 = i,
                        终点 = i + 1
                    });
                }
                else
                { // 不在码表中，可能是符号或未收录的字等
                    Glob.BmAlls.Add(new BmAll
                    {
                        查询的字 = singleStr,
                        编码 = KeyObj.TransCode(singleStr),
                        重数 = 0,
                        起点 = i,
                        终点 = i + 1
                    });
                }
            }

            if (Glob.BmAlls.Count == 0)
            {
                this.UIThread(() =>
                {
                    ShowFlowText("没有查询到任何编码！");
                    this.richTextBox1.ClearLines();
                });
                Glob.IsChecking = false;
                return;
            }

            if (Glob.IsPointIt)
            {
                delayActionModel.Debounce(100, this, new Action(() =>
                {
                    this.richTextBox1.Render(Glob.BmAlls, Glob.RightBGColor); //* 绘制标注线条
                }));
            }

            BeginInvoke(new MethodInvoker(() =>
            { // 计算理论码长
                string codeStr = CalcCodeString(Glob.BmAlls, Glob.SingleWordDic, Glob.SingleCodeDic);

                Glob.词库理论码长 = (double)codeStr.Length / textLen;

                for (int c = 0; c < codeStr.Length; c++)
                {
                    if (KeyObj.KeysStringDic.ContainsKey(codeStr[c].ToString()))
                    {
                        Glob.CalcKeysTotal[KeyObj.KeysStringDic[codeStr[c].ToString()]]++;
                    }
                }

                ShowFlowText(string.Format("第{0}段，计算码长为：{1}，用时：{2}秒", Glob.CurSegmentNum.ToString(), Glob.词库理论码长.ToString("0.00"), (DateTime.Now - startTime).TotalSeconds.ToString("0.000")));

                mS1.Invalidate();
                Glob.IsChecking = false;
            }));
        }

        /// <summary>
        /// 理论编码字符串计算
        /// </summary>
        /// <param name="wordDic"></param>
        /// <param name="CodeDic"></param>
        /// <returns></returns>
        private string CalcCodeString(List<BmAll> bmAlls, Dictionary<string, string> wordDic, Dictionary<string, string> codeDic)
        {
            string codeStr = "";
            for (int index = 0; index < bmAlls.Count; index++)
            {
                BmAll curBm = bmAlls[index];
                string cStr = curBm.编码;

                if (curBm.重数 == 0)
                { //* 0 重数可能会存在自动上屏或顶屏的情况
                    if (wordDic.ContainsKey(curBm.查询的字))
                    {
                        if (Glob.UseDGInput)
                        { //! 顶功输入法
                            if (index + 1 < bmAlls.Count)
                            { //* 非最后一组
                                BmAll nextBm = bmAlls[index + 1];
                                if (Glob.UseZRetype && curBm.查询的字 == nextBm.查询的字 && nextBm.编码.Length > 1)
                                {
                                    int reNum = 0;
                                    while (curBm.查询的字 == nextBm.查询的字 && index + 1 < bmAlls.Count)
                                    {
                                        reNum++;
                                        if (codeDic.ContainsKey(cStr + "z"))
                                        {
                                            if (codeDic.ContainsKey(cStr + nextBm.编码[0]))
                                            {
                                                cStr += " z ";

                                            }
                                            else
                                            {
                                                if (nextBm.编码.Length == 2)
                                                {
                                                    cStr += nextBm.编码;
                                                }
                                                else
                                                {
                                                    cStr += reNum > 0 ? "z " : " z ";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            cStr += "z ";
                                        }

                                        index++; //* 已处理过该字
                                        if (index + 1 < bmAlls.Count)
                                        { //* 判定接下去的一组字词，即连续出现重复
                                            nextBm = bmAlls[index + 1];
                                        }
                                    }
                                }
                                else
                                {
                                    if (codeDic.ContainsKey(cStr + nextBm.编码[0]))
                                    { //* 无法自动顶屏
                                        cStr += " ";
                                    }
                                    else if (Glob.UseSymbolSelect && ";'".Contains(KeyObj.TransSingleQuotation(nextBm.编码[0].ToString())))
                                    { //* 若输入方案有使用符号选重，需要判定
                                        for (int ci = 0; ci < ValidChars.Length; ci++)
                                        {
                                            if (codeDic.ContainsKey(cStr + ValidChars[ci]))
                                            { //* 不唯一，即不会自动上屏
                                                cStr += " ";
                                                break;
                                            }
                                        }
                                    }
                                    else if ("0123456789-=".Contains(nextBm.编码[0]))
                                    {
                                        for (int ci = 0; ci < ValidChars.Length; ci++)
                                        {
                                            if (codeDic.ContainsKey(cStr + ValidChars[ci]))
                                            { //* 不唯一，即不会自动上屏
                                                cStr += " ";
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            { // 最后一组
                                for (int ci = 0; ci < ValidChars.Length; ci++)
                                {
                                    if (codeDic.ContainsKey(cStr + ValidChars[ci]))
                                    { //* 不唯一，即不会自动上屏
                                        cStr += " ";
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        { //* 非顶功输入法
                            if (Glob.UseAutoInput)
                            {
                                if (cStr.Length < 4)
                                {
                                    cStr += " ";
                                }
                                else
                                {
                                    for (int ci = 0; ci < ValidChars.Length; ci++)
                                    {
                                        if (codeDic.ContainsKey(cStr + ValidChars[ci]))
                                        {
                                            cStr += " ";
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                cStr += " ";
                            }

                            if (Glob.UseZRetype && index + 1 < bmAlls.Count)
                            {
                                BmAll nextBm = bmAlls[index + 1];
                                if (curBm.查询的字 == nextBm.查询的字 && nextBm.查询的字.Length > 1)
                                {
                                    while (curBm.查询的字 == nextBm.查询的字 && index + 1 < bmAlls.Count)
                                    {
                                        cStr += "z ";
                                        index++;
                                        if (index + 1 < bmAlls.Count)
                                        { //* 判定接下去的一组字词，即连续出现重复
                                            nextBm = bmAlls[index + 1];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                { //? 当一个词条重数大于 0 时，那它一定是存在于字典当中的
                    int pageNum = (curBm.重数 + 1) / 10;
                    for (int n = 0; n < pageNum; n++)
                    {
                        cStr += "+";
                    }
                    int selectNum = (curBm.重数 + 1) % 10;

                    if (selectNum == 1)
                    {
                        cStr += " ";
                    }
                    else if (selectNum == 2)
                    {
                        if (Glob.UseSymbolSelect)
                        {
                            cStr += ";";
                        }
                        else
                        {
                            cStr += "2";
                        }
                    }
                    else if (selectNum == 3)
                    {
                        if (Glob.UseSymbolSelect)
                        {
                            cStr += "'";
                        }
                        else
                        {
                            cStr += "3";
                        }
                    }
                    else
                    {
                        cStr += selectNum.ToString();
                    }

                    if (Glob.UseZRetype && index + 1 < bmAlls.Count)
                    {
                        BmAll nextBm = bmAlls[index + 1];
                        if (curBm.查询的字 == nextBm.查询的字 && nextBm.编码.Length > 1)
                        {
                            while (curBm.查询的字 == nextBm.查询的字 && index + 1 < bmAlls.Count)
                            {
                                cStr += "z ";
                                index++; //* 已处理过该字
                                if (index + 1 < bmAlls.Count)
                                { //* 判定接下去的一组字词，即连续出现重复
                                    nextBm = bmAlls[index + 1];
                                }
                            }
                        }
                    }
                }

                codeStr += cStr;
            }

            return codeStr;
        }

        private void CheckWordToolButton_Click(object sender, EventArgs e)
        {
            var ini = new _Ini("config.ini");
            if (Glob.是否智能测词)
            {
                Glob.是否智能测词 = false;
                this.CheckWordToolButton.Checked = false;
                ini.IniWriteValue("程序控制", "智能测词", "False");
                开始测词委托 = null;
                this.richTextBox1.ClearLines();
            }
            else
            {
                Glob.是否智能测词 = true;
                this.CheckWordToolButton.Checked = true;
                ini.IniWriteValue("程序控制", "智能测词", "True");
                开始测词委托 = null;
            }
        }

        public delegate void BianMaCheck(string param, int flag);

        /// <summary>
        /// 编码检查
        /// </summary>
        /// <param name="word"></param>
        /// <param name="flag"></param>
        public void CodeCheck(string word, int flag)
        {
            string bm = word;
            int findit = 0;
            if (!Glob.AllWordDic.ContainsKey(word))
            { // 不在码表中，可能是符号或未收录的字等
                findit = 1;
                bm = "";
            }
            else
            {
                findit = 1;
                bm = Glob.AllWordDic[word];
                if (Glob.AllCodeDic.ContainsKey(bm) && Glob.AllCodeDic[bm] != word)
                {
                    int tempIndex = 2;
                    while (Glob.AllCodeDic.ContainsKey(bm + tempIndex.ToString()) && Glob.AllCodeDic[bm + tempIndex.ToString()] != word)
                    {
                        tempIndex++;
                    }
                    findit = tempIndex;
                }
            }

            if (flag == 0)
            {
                BeginInvoke(new MethodInvoker(() => ShowBmTips(word, bm, findit - 1)));
            }
            else if (flag == 1)
            {
                string s = "";
                if (findit > 0)
                {
                    s = string.Format("【{0}】 · 【{1}】 · 【{2}重】", word, bm, findit - 1);
                }
                else
                {
                    s = string.Format("【{0}】 的编码未找到。", word);
                }

                if (s.Length > 0)
                {
                    BeginInvoke(new MethodInvoker(() => ShowFlowText(s)));
                }
            }
        }

        private void ShowBmTips(string word, string s, int flag)
        {
            lblBmTips.Text = s;
            PicSetBmTips(word, s, flag);
        }

        /// <summary>
        /// 设置编码提示的显示
        /// </summary>
        /// <param name="zi">字</param>
        /// <param name="bm">编码</param>
        /// <param name="flag">选重</param>
        private void PicSetBmTips(string zi, string bm, int flag)
        {
            this.UIThread(() =>
                {
                    var bmp = new Bitmap(lblBmTips.Width, lblBmTips.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        int splitLineWidth = (bmp.Width - bmp.Height) * 2 / 5; //字起点
                        int splitLineWidth2 = (bmp.Width - splitLineWidth - bmp.Height) * 3 / 5;
                        var solidBrush = new SolidBrush(Color.FromArgb(99, 91, 91));
                        var ziFont = new Font("宋体", 9f);
                        var BasePen = new Pen(Theme.ThemeBG);
                        g.DrawLine(BasePen, bmp.Height + 3, 0, bmp.Height + 3, bmp.Height);
                        g.DrawLine(BasePen, splitLineWidth + bmp.Height, 0, splitLineWidth + bmp.Height, bmp.Height);
                        //画重
                        int radius = bmp.Height - 2;
                        g.FillPie(new SolidBrush(this.richTextBox1.GetColor(flag)), 2, 1, radius, radius, -360, 360);
                        g.FillRectangle(new SolidBrush(this.richTextBox1.GetColor(flag)), 1, 1, bmp.Height + 1, bmp.Height - 2);
                        //画字
                        SizeF ziSizeF = g.MeasureString(zi, ziFont);
                        g.DrawString(zi, ziFont, solidBrush, splitLineWidth / 2 - ziSizeF.Width / 2 + bmp.Height + 2,
                                     bmp.Height / 2 - ziSizeF.Height / 2 + 1);
                        //画编码
                        var bmFont = new Font("宋体", 9f);
                        SizeF bmSizeF = g.MeasureString(bm, bmFont);
                        if (flag != 0)
                        {
                            solidBrush = new SolidBrush(Color.DarkBlue);
                        }
                        g.DrawString(bm, bmFont, solidBrush,
                                     splitLineWidth2 / 2 - bmSizeF.Width / 2 + bmp.Height + splitLineWidth + 3,
                                     bmp.Height / 2 - bmSizeF.Height / 2 + 1);
                    }
                    lblBmTips.Image = bmp;

                    if (this.picBmTips.Checked && Glob.BmAlls.Count > 0)
                    {
                        int count = Glob.BmAlls.Count;
                        string str = "";
                        for (int index = 0; index < count; index++)
                        {
                            BmAll bmAll = Glob.BmAlls[index];
                            int now = this.textBoxEx1.TextLength;
                            if (now == bmAll.起点)
                            {
                                //显示当前
                                str = string.Format("【{0}】 {1} {2}重 ", Glob.BmAlls[index].查询的字, Glob.BmAlls[index].编码, Glob.BmAlls[index].重数);
                                break;
                            }

                            if (now == bmAll.终点)
                            {
                                //显示下一个
                                var ind = (index + 1 >= count) ? count - 1 : index + 1;
                                str = string.Format("【{0}】 {1} {2}重 ", Glob.BmAlls[ind].查询的字, Glob.BmAlls[ind].编码, Glob.BmAlls[ind].重数);
                                break;
                            }
                        }
                        Glob.词组编码 = str;
                        mS1.Invalidate();
                    }
                });
        }

        /// <summary>
        /// 查询文字编码
        /// </summary>
        private void QueryWordCode()
        {
            var bianMa = new BianMaCheck(CodeCheck);
            var s =
                Glob.TypeText[Glob.TypeTextCount == Glob.TextLen ? Glob.TypeTextCount - 1 : Glob.TypeTextCount].ToString();
            bianMa.BeginInvoke(s, 0, null, null);
        }

        private void 查询当前编码ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Glob.UsedTableIndex))
            {
                QueryWordCode();
            }
        }

        private void picBmTips_Click(object sender, EventArgs e)
        {
            var ini = new _Ini("config.ini");
            if (this.picBmTips.Checked)
            {
                //取消
                this.picBmTips.Checked = false;
                ini.IniWriteValue("程序控制", "编码", "False");

            }
            else
            {
                //开启
                this.picBmTips.Checked = true;
                ini.IniWriteValue("程序控制", "编码", "True");
            }
        }

        /// <summary>
        /// 查询选中文字的编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiFindSelectionBm_Click(object sender, EventArgs e)
        {
            var bianMa = new BianMaCheck(CodeCheck);
            var s = this.richTextBox1.SelectedText;
            bianMa.BeginInvoke(s, 1, null, null);
        }
        #endregion

        #region 按钮 快捷键

        /// <summary>
        /// 快捷键定义
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && e.Control)
            { // Ctrl + 右，载文下一段
                if (sw != 0) return;
                if (this.cmsDuanList.Items.Count <= 0) return;
                int index = this.cmsDuanList.Items.IndexOfKey(Glob.CurSegmentNum.ToString()); // 注：是获得索引值，而不是段号
                index++;
                if (index >= this.cmsDuanList.Items.Count)
                    index = 0;
                this.cmsDuanList.Items[index].PerformClick();
            }
            else if (e.KeyCode == Keys.Left && e.Control)
            { // Ctrl + 左，载文上一段
                if (sw != 0) return;
                if (this.cmsDuanList.Items.Count <= 0) return;
                int index = this.cmsDuanList.Items.IndexOfKey(Glob.CurSegmentNum.ToString());
                index--;
                if (index < 0)
                    index = this.cmsDuanList.Items.Count - 1;
                this.cmsDuanList.Items[index].PerformClick();
            }
        }

        /// <summary>
        /// 显示浮动的信息
        /// </summary>
        /// <param name="text">需要显示的信息</param>
        public void ShowFlowText(string text)
        {
            var sm = new ShowMessage(this.Size, this.Location, this);
            sm.Show(text);
        }

        private void 击键统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Glob.jjAllC >= 100)
            {
                JjCheck JjC = new JjCheck();
                JjC.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("记录段数小于100项，项目越多击键等级计算越准！", "提示");
            }
        }

        //击键评定 显示
        private void JjCheck(int jP)
        {
            if (Glob.jjAllC >= 100)
            {
                double jjP;
                int jjC = 0;
                double jjC_ = 0;
                for (int i = 0; i < 9; i++)
                {
                    jjP = (double)Glob.jjPer[i] / Glob.jjAllC;
                    if (jjP >= 0.1 && jjC == 0)
                        jjC = (i + 4) > 12 ? 12 : i + 4;

                    if (jjC != 0)
                    {
                        if ((i + 4) >= jjC)
                            jjC_ += jjP;
                    }
                }
                if (jjC != 0)
                {
                    string ud;
                    if (jjC < jP) ud = "↑";
                    else if (jjC > jP) ud = "↓";
                    else ud = "-";
                    this.labelCheckUD.Text = ud;
                    this.labelJiCheck.Text = (jjC + jjC_).ToString("0.000");
                }
                else
                {
                    this.labelJiCheck.Text = "-";
                    this.labelCheckUD.Text = "";
                }
            }
            else
            {
                this.labelJiCheck.Text = "-";
                this.labelCheckUD.Text = "";
            }
            toolTip1.SetToolTip(this.labelJiCheck, "击键等级评定\n已跟打" + Glob.jjAllC + "段");
        }
        #endregion

        #region 发文菜单部分
        /// <summary>
        /// 发上一段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendPreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewSendText.发文状态)
            {
                int totalCount = Glob.TempSegmentRecord.Count;
                if (Glob.SendCursor > 0)
                {
                    Glob.SendCursor = 0;
                }

                if (Glob.SendCursor + totalCount > 1)
                {
                    if (NewSendText.是否周期)
                    {
                        timerTSend.Stop();
                        NewSendText.周期计数 = NewSendText.周期;
                    }

                    int curTextLength = Glob.TempSegmentRecord[totalCount + Glob.SendCursor - 1].Length; //* 当前文段字数
                    Glob.SendCursor--;
                    ReverseSendAPast(Glob.TempSegmentRecord[totalCount + Glob.SendCursor - 1], curTextLength);

                    if (NewSendText.是否周期)
                    {
                        timerTSend.Start();
                    }

                    F3();
                }
                else
                {
                    Glob.SendCursor = 1 - totalCount;
                    ShowFlowText("已经没有上一段文章内容了哦~");
                }
            }
            else
            {
                ShowFlowText("仅处于发文状态中可以使用。");
            }
        }

        /// <summary>
        /// 发下一段
        /// </summary>
        public void SendNextFun()
        {
            if (NewSendText.发文状态)
            {
                if (NewSendText.是否周期)
                {
                    timerTSend.Stop();
                    NewSendText.周期计数 = NewSendText.周期;
                }

                if (Glob.SendCursor < 0)
                { // 还存在未发文的旧序列
                    int totalCount = Glob.TempSegmentRecord.Count;
                    if (totalCount + Glob.SendCursor < 1)
                    { // 防止出错
                        Glob.SendCursor = 1 - totalCount;
                    }

                    SendAPast(Glob.TempSegmentRecord[totalCount + Glob.SendCursor]);
                    Glob.SendCursor++;
                }
                else
                {
                    SendAOnce();
                }

                if (NewSendText.是否周期)
                {
                    timerTSend.Start();
                }

                F3();
            }
        }

        private void 发下一段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewSendText.发文状态)
            {
                SendNextFun();
            }
            else
            {
                ShowFlowText("仅处于发文状态中可以使用。");
            }
        }

        /// <summary>
        /// 保存发文配置
        /// </summary>
        public void SaveSendFun()
        {
            if (NewSendText.发文状态)
            {
                if (NewSendText.标记 >= NewSendText.文章全文.Length)
                {
                    ShowFlowText("无法在当前文章的尽头保存配置哦~");
                    return;
                }

                string phrases = (NewSendText.词组 != null && NewSendText.词组.Length > 0) ? JsonConvert.SerializeObject(NewSendText.词组) : "[]";
                int type = 0;
                switch (NewSendText.类型)
                {
                    case "单字":
                        type = 0;
                        break;
                    case "文章":
                        type = 1;
                        break;
                    case "词组":
                        type = 2;
                        break;
                }
                int disorder = NewSendText.是否乱序 ? 1 : 0;
                int no_repeat = NewSendText.乱序全段不重复 ? 1 : 0;
                string segmentRecord = JsonConvert.SerializeObject(Glob.TempSegmentRecord);
                int cycle = NewSendText.是否周期 ? 1 : 0;
                int auto = NewSendText.是否自动 ? 1 : 0;
                int autoCondition = NewSendText.AutoCondition ? 1 : 0;

                if (NewSendText.SentId > 0 && Glob.SentHistory.FindIdInSent(NewSendText.SentId)) // 这里需要加一个判定 id 是否存在，不存在则同样是保存新配置
                { //* 这是之前保存过的发文配置，更新配置
                    Glob.SentHistory.UpdateSent(NewSendText.SentId, NewSendText.发文全文, NewSendText.标题, NewSendText.标记, segmentRecord, Glob.SendCursor, Glob.CurSegmentNum, NewSendText.已发段数, NewSendText.已发字数, cycle, NewSendText.周期, auto, autoCondition, (int)NewSendText.AutoKey, (int)NewSendText.AutoOperator, NewSendText.AutoNumber, (int)NewSendText.AutoNo);

                    //* 提示更新
                    ShowFlowText("已更新配置" + NewSendText.SentId.ToString());
                }
                else
                { //* 保存新配置
                    NewSendText.SentId = Glob.SentHistory.InsertSent(DateTime.Now.ToString("s"), NewSendText.文章全文, NewSendText.发文全文, NewSendText.标题, phrases, NewSendText.词组发送分隔符, type, disorder, no_repeat, NewSendText.字数, NewSendText.标记, segmentRecord, Glob.SendCursor, Glob.CurSegmentNum, NewSendText.已发段数, NewSendText.已发字数, cycle, NewSendText.周期, auto, autoCondition, (int)NewSendText.AutoKey, (int)NewSendText.AutoOperator, NewSendText.AutoNumber, (int)NewSendText.AutoNo);

                    NewSendText.ArticleSource = NewSendText.ArticleSourceValue.Sent;
                    //* 提示保存
                    ShowFlowText("已将当前的发文保存为配置" + NewSendText.SentId.ToString());
                    if (this.发文状态窗口 != null && !this.发文状态窗口.IsDisposed)
                    {
                        this.发文状态窗口.UpdateFromSave();
                    }
                }
            }
        }

        private void SaveSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewSendText.发文状态)
            {
                SaveSendFun();
            }
            else
            {
                ShowFlowText("仅处于发文状态中可以使用。");
            }
        }

        /// <summary>
        /// 停止发文
        /// </summary>
        public void StopSendFun()
        {
            if (NewSendText.发文状态)
            {
                if (NewSendText.是否周期)
                {
                    NewSendText.是否周期 = false;
                    timerTSend.Stop();
                    this.lblNowTime_.Text = "";
                }
                NewSendText.发文状态 = false;
                NewSendText.已发段数 = 0;
                NewSendText.已发字数 = 0;
                NewSendText.标记 = 0;
                if (发文状态窗口 != null && !发文状态窗口.IsDisposed)
                {
                    发文状态窗口.Close();
                }

                //* 清空临时文段内容记录
                Glob.TempSegmentRecord.Clear();
                Glob.SendCursor = 0;
            }
        }

        private void 停止发文ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StopSendFun();
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="title"></param>
        public void ResetArticleTitle(string title)
        {
            this.lblTitle.Text = title;
        }
        #endregion

        #region 载文途径
        private void 从剪切板ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string get = Clipboard.GetText().Trim();//获取剪贴板内的文字
            if (get != "")
            {
                this.TypeContentDirectly(get, Glob.AZpre.ToString(), "来自剪贴板");
                Glob.AZpre++;
            }
        }

        private void FormatLoadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PutText();
        }
        #endregion

        #region 关闭详细信息
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            bool temp = (sender as ToolStripButton).Checked;
            if (temp)
            {
                this.toolStripButton4.Checked = false;
            }
            else
            {
                this.toolStripButton4.Checked = true;
            }
        }
        private void toolStripButton4_CheckedChanged(object sender, EventArgs e)
        {
            bool temp = (sender as ToolStripButton).Checked;
            CloseDetail(temp);
            _Ini inisetup = new _Ini("config.ini");
            inisetup.IniWriteValue("程序控制", "详细信息", temp.ToString());
        }

        private void CloseDetail(bool temp)
        {
            switch (temp)
            {
                case false:
                    Glob.p1 = this.splitContainer1.SplitterDistance;
                    Glob.p2 = this.splitContainer3.SplitterDistance;

                    this.splitContainer3.Panel2Collapsed = true;
                    //this.toolStripButton4.Checked = false;
                    this.splitContainer1.SplitterDistance = Glob.p1 + Glob.p2;
                    this.splitContainer3.SplitterDistance = 100;
                    break;
                case true:
                    this.splitContainer3.Panel2Collapsed = false;
                    //this.toolStripButton4.Checked = true;
                    this.splitContainer1.SplitterDistance = Glob.p1;
                    this.splitContainer3.SplitterDistance = 100;
                    break;
            }
        }
        #endregion

        #region 文章处理
        /// <summary>
        /// 获取范围随机数组
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            Random rnd = new Random();
            int length = maxValue - minValue + 1;
            byte[] keys = new byte[length];
            rnd.NextBytes(keys);
            int[] items = new int[length];
            for (int i = 0; i < length; i++)
            {
                items[i] = i + minValue;
            }
            Array.Sort(keys, items);
            int[] result = new int[count];
            Array.Copy(items, result, count);
            return result;
        }

        /// <summary>
        /// 乱序重打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sourceText = richTextBox1.Text;
            int textLen = sourceText.Length;
            if (textLen > 9)
            {
                this.CleanSpeedPoints(); // 清理测速点信息
                int[] numlist = GetRandomUnrepeatArray(0, textLen - 1, textLen);
                char[] textChars = sourceText.ToCharArray();
                StringBuilder sb = new StringBuilder();

                foreach (int item in numlist)
                {
                    sb.Append(textChars[item]);
                }
                richTextBox1.Text = sb.ToString();

                F3();
                GetInfo();
            }
            else
            {
                MessageBox.Show("字数过少！");
            }
        }


        /// <summary>
        /// 全角转半角的方法
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string ReFullAsHalf(string inputStr)
        {
            char[] c = inputStr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] > 0xff00 && c[i] < 0xff5f)
                {
                    c[i] = (char)(c[i] - 0xfee0);
                }
                else if (CharReDict.ContainsKey("0x" + Convert.ToString(c[i], 16)))
                {
                    c[i] = CharReDict["0x" + Convert.ToString(c[i], 16)];
                }
                else
                {
                    continue;
                }
            }

            return new string(c);
        }

        /// <summary>
        /// 标点替换方法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ReText(string text)
        {
            text = ReFullAsHalf(text); // 全角转半角

            string[] Ebiaodian = new string[] { ",", ";", ":", "?", "!", "｢", "｣", "(", ")", "<", ">", @"\(", @"\)" };
            string[] Cbiaodian = new string[] { "，", "；", "：", "？", "！", "「", "」", "（", "）", "《", "》", "（", "）" };
            for (int i = 0; i < Ebiaodian.Length; i++)
            {
                text = text.Replace(Ebiaodian[i], Cbiaodian[i]);
            }

            text = Regex.Replace(text, "，{2,}", "，");
            text = Regex.Replace(text, "—+|─+|-{2,}", "——");
            text = Regex.Replace(text, @"…+|\.{3,}", "……");
            text = text.Replace("..", "。");
            text = Regex.Replace(text, @"([^\w])\.", "$1。");

            return text;
        }

        private void 打开练习ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string filename = this.openFileDialog1.FileName;
                StreamReader openfile = new StreamReader(filename, UnicodeEncoding.Default);//读取
                FileInfo fileinfo = new FileInfo(filename); //获取文件信息
                string opentxt = openfile.ReadToEnd();//读取整个文件
                if (opentxt.Length != 0)
                {
                    this.richTextBox1.Text = opentxt;
                    GetInfo();
                }
                else
                {
                    MessageBox.Show("文件内容为空！", "雨天跟打器载文提示");
                }
            }
        }
        #endregion

        #region 下方工具条
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _Ini ini = new _Ini("config.ini");
            if (Glob.autoReplaceBiaodian)
            {
                Glob.autoReplaceBiaodian = false;
                this.toolStripButton1.Checked = false;
                ini.IniWriteValue("程序控制", "自动替换", "False");
            }
            else
            {
                Glob.autoReplaceBiaodian = true;
                this.toolStripButton1.Checked = true;
                ini.IniWriteValue("程序控制", "自动替换", "True");
            }
        }

        //* 自动复制成绩
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _Ini ini = new _Ini("config.ini");
            if (Glob.AutoCopy)
            {
                Glob.AutoCopy = false;
                this.toolStripButton3.Checked = false;
                ini.IniWriteValue("控制", "自动复制成绩", "False");
            }
            else
            {
                Glob.AutoCopy = true;
                this.toolStripButton3.Checked = true;
                ini.IniWriteValue("控制", "自动复制成绩", "True");
            }
        }

        private void ShowRealTimeData_Click(object sender, EventArgs e)
        {
            _Ini ini = new _Ini("config.ini");
            bool b = (sender as ToolButton).Checked;
            if (b)
            {
                this.RealTimeData.Checked = false;
                Glob.ShowRealTimeData = false;
                ini.IniWriteValue("控制", "实时数据", "False");
            }
            else
            {
                this.RealTimeData.Checked = true;
                Glob.ShowRealTimeData = true;
                ini.IniWriteValue("控制", "实时数据", "True");
            }
        }
        #endregion

        #region 老板键及双击全显
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                this.Activate();
            }
        }

        /// <summary>
        /// 老板键菜单项点击事件
        /// 注：老板键为全局注册热键，不会触发到这个事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 老板键ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Glob.HotKeyList.Last().GetKeys() == "None")
            {
                MessageBox.Show("请先在快捷键设置中为老板键绑定按键，否则在隐藏后将无法恢复窗口！");
            }
            else
            {
                if (this.发文状态窗口 != null && !this.发文状态窗口.IsDisposed && this.发文状态窗口.Visible)
                {
                    this.发文状态窗口.Hide();
                }
                this.Hide();
                this.notifyIcon1.Visible = false;
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private bool isShowAll = false;
        /// <summary>
        /// 成绩数据表格双击处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.splitContainer3.Panel1Collapsed)
            {
                isShowAll = false;
                if (!Glob.isShowSpline)
                    this.splitContainer4.Panel1Collapsed = false;
                this.splitContainer1.Panel1Collapsed = false;
                this.splitContainer3.Panel1Collapsed = false;
                if (Glob.HaveTypeCount > 0)
                    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            }
            else
            {
                isShowAll = true;
                this.splitContainer4.Panel1Collapsed = true;
                this.splitContainer1.Panel1Collapsed = true;
                this.splitContainer3.Panel1Collapsed = true;
            }
        }

        private void chartSpeed_DoubleClick(object sender, EventArgs e)
        {
            if (this.splitContainer4.Panel2Collapsed)
            {
                isShowAll = false;
                this.splitContainer4.Panel2Collapsed = false;
                this.splitContainer1.Panel1Collapsed = false;
                this.splitContainer3.Panel1Collapsed = false;
            }
            else
            {
                isShowAll = true;
                this.splitContainer4.Panel2Collapsed = true;
                this.splitContainer1.Panel1Collapsed = true;
                this.splitContainer3.Panel1Collapsed = true;
            }
        }

        #endregion

        #region 检验真伪
        /// <summary>
        /// 检验真伪
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        public string CheckTF(string goal)
        {
            string jg;
            Regex getit = new Regex(@".+(?= 校验)");
            string all = getit.Match(goal).ToString();
            Regex getID = new Regex(@"(?<= 校验:)\d+");
            string getid = getID.Match(goal).ToString();
            //计算ID
            string nowgetid = Validation.Validat(all);
            if (getid.Length != 0)
            {
                if (nowgetid == getid)
                {
                    jg = "真";
                }
                else
                {
                    jg = "假";
                }
            }
            else
            {
                jg = "缺少条件";
            }
            return jg;
        }

        private void 检验真伪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string getC = Clipboard.GetText();
                if (getC.Length != 0 && getC.Length <= 300)
                {
                    string g = CheckTF(getC);
                    MessageBox.Show(this, "成绩数据：" + getC + "\n" + "检验结果：" + g, "雨天跟打器成绩检验结果");
                }
                else
                {
                    MessageBox.Show(this, "请检查是否为成绩？", "雨天跟打器检验真伪提示");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\n" + "说明：此错误并非由跟打器本身所导致！");
            }
        }
        #endregion

        #region 字符寻找
        //错字单击
        private void labelBM_MouseClick(object sender, MouseEventArgs e)
        {
            if (Glob.FWords.Count != 0)
            {
                this.richTextBox1.SelectionStart = (int)Glob.FWords[Glob.FWordsSkip];
                this.richTextBox1.SelectionLength = 1;
                this.richTextBox1.ScrollToCaret();
                this.lbl错字显示.Text = this.richTextBox1.SelectedText;
                if (e.Button == System.Windows.Forms.MouseButtons.Left) //左键向下
                {
                    Glob.FWordsSkip++;
                    if (Glob.FWordsSkip >= Glob.FWords.Count) Glob.FWordsSkip = 0;
                } //右键向上
                else
                {
                    Glob.FWordsSkip--;
                    if (Glob.FWordsSkip < 0) Glob.FWordsSkip = (int)Glob.FWords.Count - 1;
                }
            }
        }

        //回改单击
        private void labelhgstatus_MouseClick(object sender, MouseEventArgs e)
        {
            if (Glob.TextHgPlace.Count > 0)
            {
                int now = (int)Glob.TextHgPlace[Glob.TextHgPlace_Skip];
                this.richTextBox1.SelectionStart = now;
                this.richTextBox1.SelectionLength = 1;
                this.richTextBox1.ScrollToCaret();
                this.lbl回改显示.Text = this.richTextBox1.SelectedText;
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Glob.TextHgPlace_Skip++;
                    if (Glob.TextHgPlace_Skip >= Glob.TextHgPlace.Count) Glob.TextHgPlace_Skip = 0;
                }
                else
                {
                    Glob.TextHgPlace_Skip--;
                    if (Glob.TextHgPlace_Skip < 0) Glob.TextHgPlace_Skip = (int)Glob.TextHgPlace.Count - 1;
                }
            }
        }
        #endregion

        #region 窗口复位
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int getW = this.dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.None) + 8;
            if (getW < 1280)
            {
                getW = 1280;
            }
            this.Size = new Size(getW, 480);

            this.splitContainer1.Panel2Collapsed = false;
            this.splitContainer1.SplitterDistance = 142;

            this.splitContainer3.Panel2Collapsed = false;
            this.splitContainer3.SplitterDistance = 89;

            this.splitContainer4.Panel2Collapsed = false;
            this.splitContainer4.SplitterDistance = 206;

            if (!Glob.isShowSpline)
                this.splitContainer4.Panel1Collapsed = false;
            this.splitContainer1.Panel1Collapsed = false;
            this.splitContainer3.Panel1Collapsed = false;

            this.textBoxEx1.Focus();
        }
        #endregion

        #region 闪烁
        //* 自动复制成绩的闪烁
        private static bool BtnSubFlash = true;
        private static int BtnSubFlashCount = 0;
        private void timerSubFlash_Tick(object sender, EventArgs e)
        {
            if (BtnSubFlashCount > 5)
            {
                timerSubFlash.Stop();
                BtnSubFlash = true;
                BtnSubFlashCount = 0;
                this.toolStripButton3.ForeColor = Color.White;
            }
            if (BtnSubFlash)
            {
                this.toolStripButton3.Enabled = true;
                BtnSubFlash = false;
            }
            else
            {
                this.toolStripButton3.Enabled = false;
                BtnSubFlash = true;
            }
            BtnSubFlashCount++;
        }
        #endregion

        #region 表格处理
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor; //选中的时候，单元格颜色不变
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor; //选中的时候，单元格颜色不变
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        }
        #endregion

        #region 测速点
        private void 比赛时自动打开寻找测速点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Ini ini = new _Ini("config.ini");
            ToolStripMenuItem ts = (sender as ToolStripMenuItem);
            if (ts.Checked)
            {
                ts.Checked = false;
                ini.IniWriteValue("程序控制", "自动打开寻找", "False");
            }
            else
            {
                ts.Checked = true;
                ini.IniWriteValue("程序控制", "自动打开寻找", "True");
            }
        }

        private void 添加测速点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sw != 0) { ShowFlowText("请勿在跟打时建立测速点！"); return; }
            if (Glob.TextLen <= 20) { ShowFlowText("文章字数过少，无法使用此功能！"); return; }
            int start = this.richTextBox1.SelectionStart - 1;
            int count = Glob.SpeedPointCount;
            if (count == 0)
            {
                if (start > 10 && start < Glob.TextLen - 10)
                {
                    SetSpeedPoint(start);
                }
                else
                {
                    MessageBox.Show("请先鼠标左键点击确认光标位置，再选择添加此处为测速点！" +
                        "\n测速点不要离开始点及结束点太近！\n（注：不能位于文章首尾的10个字符内）", "测速提示");
                }
            }
            else
            {
                int len = start - Glob.SpeedPoint_[Glob.SpeedPointCount - 1];
                if (len > 0)
                {
                    if (len <= 10)
                    {
                        MessageBox.Show("相邻两测速点距离太近，请重新选择！", "测速提示");
                    }
                    else
                    {
                        SetSpeedPoint(start);
                    }
                }
                else
                {
                    ShowFlowText("请按顺序安排测速点！");
                }
            }
        }

        /// <summary>
        /// 设置测速点
        /// </summary>
        /// <param name="start"></param>
        public void SetSpeedPoint(int start)
        {
            if (Glob.SpeedPointCount > 9)
            {
                MessageBox.Show("最多只能设置10个测速点！");
                return;
            }
            else
            {
                this.richTextBox1.SelectionStart = start;
                this.richTextBox1.SelectionLength = 1;
                this.richTextBox1.SelectionBackColor = Glob.TestMarkColor;
                Glob.SpeedPoint_[Glob.SpeedPointCount] = start;
                Glob.SpeedPointCount++;
                this.lblspeedcheck.Text = "测速点:" + Glob.SpeedPointCount.ToString();
            }
        }

        private void 测速数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Glob.SpeedPointCount > 0 && Glob.SpeedControl > 0)
            {
                if (sw > 0) { return; }
                SpeedCheckPoint scp = new SpeedCheckPoint(this);
                if (scp.ShowDialog() == DialogResult.Cancel)
                {
                    this.textBoxEx1.Focus();
                }
            }
            else
            {
                ShowFlowText("未找到测速信息！");
            }
        }

        private void 自动寻找赛文标记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Glob.isMatch)
            {
                if (Glob.SpeedPointCount == 0)
                {
                    SpeedCheckOut sco = new SpeedCheckOut(this);
                    sco.ShowDialog(this);
                }
                else
                {
                    ShowFlowText("测速点已存在！");
                }
            }
            else
            {
                ShowFlowText("非比赛认证段！无法寻找赛文标记！");
            }
        }

        /// <summary>
        /// 清理测速点的方法
        /// </summary>
        private void CleanSpeedPoints()
        {
            if (Glob.SpeedPointCount > 0)
            {
                this.richTextBox1.SelectAll();
                this.richTextBox1.SelectionBackColor = this.richTextBox1.BackColor;
                Glob.SpeedPoint_ = new int[10];//测速点控制
                Glob.SpeedTime = new double[10];//测速点时间控制
                Glob.SpeedJs = new int[10];//键数
                Glob.SpeedHg = new int[10];//回改
                Glob.SpeedPointCount = 0;//测速点数量控制
                Glob.SpeedControl = 0;
                this.lblspeedcheck.Text = "时间";
            }
        }

        private void 清除测速点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CleanSpeedPoints();
        }
        #endregion

        #region 新段号列表
        private void cmsDuanList_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
        }

        private void lblDuan_MouseClick(object sender, MouseEventArgs e)
        {
            if (cmsDuanList.Items.Count > 0)
            {
                cmsDuanList.Show((sender as Label), 0, (sender as Label).Height);
            }
        }

        private static Regex getDuanList
        {
            get
            {
                if (Glob.IsZdyPre)
                {
                    string pretext = Glob.PreText.Replace(@"\", @"\\");
                    string preduan = Glob.PreDuan.Replace("xx", @"\d+");
                    return new Regex(@"(?<=" + pretext + Glob.PreDuan.Replace("xx", @")\d+(?=") + ")");
                }
                else
                {
                    return new Regex(@"(?<=-----第)\d+(?=段)");
                }
            }
        }

        /// <summary>
        /// 将段列出来
        /// </summary>
        private void ListDuan(string duan)
        {
            if (Glob.Text.Length != 0)
            {
                int c = 0;//控制项目十项
                MatchCollection mc = getDuanList.Matches(Glob.Text);
                if (mc.Count > 0)
                {
                    this.cmsDuanList.Items.Clear();
                    for (int i = mc.Count - 1; i >= 0; i--)
                    {
                        if (c > 10) break;
                        this.cmsDuanList.Items.Add(mc[i].ToString(), null, new EventHandler(SelectDuan));
                        c++;
                        ToolStripItem tsi = (ToolStripItem)this.cmsDuanList.Items[this.cmsDuanList.Items.Count - 1];
                        tsi.Name = mc[i].ToString();
                        if (mc[i].ToString().Trim() == duan)
                            tsi.BackColor = Color.LightPink;
                        else
                            tsi.BackColor = Color.White;
                    }
                }
                else
                {
                    this.cmsDuanList.Items.Clear();
                }
            }
        }
        private void SelectDuan(object sender, EventArgs e)
        {
            string pretext, preduan;
            //System.Diagnostics.Debug.Write("sender.ToString() = " + sender.ToString() + "\n");
            //System.Diagnostics.Debug.Write("Glob.PreDuan = " + Glob.PreDuan + "\n");
            if (Glob.IsZdyPre)
            {
                pretext = Glob.PreText.Replace(@"\", @"\\");
                preduan = Glob.PreDuan.Replace("xx", sender.ToString());
            }
            else
            {
                pretext = "-----";
                preduan = "第" + sender.ToString() + "段";
            }
            //MessageBox.Show(preduan);
            Regex regexAll = new Regex(@".+\s.+\s" + pretext + preduan + ".*", RegexOptions.RightToLeft); //获取发送的全部信息
            Glob.getDuan = regexAll.Match(Glob.Text);
            if (Glob.getDuan.Length == 0) //为空
            {
                return;
            }

            if (Glob.IsZdyPre)
            {
                Glob.regexCout = new Regex(@"(?<=" + Glob.PreDuan.Replace("xx", @")" + sender.ToString() + "(?=") + ")", RegexOptions.RightToLeft);
            }
            else
            {
                Glob.regexCout = new Regex(@"(?<=第)" + sender.ToString() + "(?=段)", RegexOptions.RightToLeft);
            }
            LoadText(pretext, preduan, Glob.regexCout, Glob.getDuan.ToString());
        }
        #endregion

        #region 标记

        private void PointIt(object sender, EventArgs e)
        {
            _Ini ini = new _Ini("config.ini");
            if (Glob.IsPointIt)
            {
                Glob.IsPointIt = false;
                this.tsb标注.Checked = false;
                ini.IniWriteValue("程序控制", "标记", "False");
                this.richTextBox1.ClearLines();
            }
            else
            {
                Glob.IsPointIt = true;
                this.tsb标注.Checked = true;
                ini.IniWriteValue("程序控制", "标记", "True");
            }
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            if (Glob.IsPointIt)
            {
                delayActionModel.Debounce(100, this, new Action(() =>
                {
                    this.richTextBox1.Render(Glob.BmAlls, Glob.RightBGColor);
                }));
            }
        }

        private void richTextBox1_HScroll(object sender, EventArgs e)
        {
            if (Glob.IsPointIt)
            {
                delayActionModel.Debounce(100, this, new Action(() =>
                {
                    this.richTextBox1.Render(Glob.BmAlls, Glob.RightBGColor);
                }));
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Glob.IsPointIt)
            {
                delayActionModel.Debounce(100, this, new Action(() =>
                {
                    this.richTextBox1.Render(Glob.BmAlls, Glob.RightBGColor);
                }));
            }
        }
        #endregion

        #region 快捷键列表
        private void 重打ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetInfo();
            F3();
        }

        //发送到桌面的快捷方式
        private void 发送到桌面的快捷方式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shortcut();
        }

        /// <summary>
        /// 创建桌面快捷方式的方法
        /// </summary>
        private void Shortcut()
        {
            //获取当前系统用户启动目录
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            //获取当前系统用户桌面目录
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FileInfo fileStartup = new FileInfo(startupPath + "\\雨天跟打器.lnk");

            FileInfo fileDesktop = new FileInfo(desktopPath + "\\雨天跟打器.lnk");
            if (!fileDesktop.Exists)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(
                      Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +
                      "\\雨天跟打器.lnk");

                shortcut.TargetPath = Application.ExecutablePath; // 启动程序路径
                shortcut.WorkingDirectory = System.Environment.CurrentDirectory;
                shortcut.WindowStyle = 1;
                shortcut.Description = "雨天跟打器";
                shortcut.IconLocation = Application.ExecutablePath;
                shortcut.Save();
            }
            // 开机启动的方法
            /*
              if (!fileStartup.Exists)
               {
                    //获取可执行文件快捷方式的全部路径
                    string exeDir = desktopPath + "\\雨天跟打器.lnk";
                    //把程序快捷方式复制到启动目录
                     System.IO.File.Copy(exeDir, startupPath + "\\雨天跟打器.lnk", true);
               }*/
        }
        #endregion

        #region 透明背景处
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        /// <summary>
        /// 移动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mS1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        #endregion

        #region 主题的设置

        private void toolStrip1_Paint_1(object sender, PaintEventArgs e)
        {
            if ((sender as ToolStrip).RenderMode == ToolStripRenderMode.System)
            {
                Rectangle rect = new Rectangle(0, 0, this.toolStrip1.Width - 1, this.toolStrip1.Height - 2);
                e.Graphics.SetClip(rect);
            }
        }

        private void 外观ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTheme ft = new FormTheme(this);
            ft.ShowDialog();
        }
        #endregion

        #region 历史记录
        private void HistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            History.History his = new History.History(this);
            if (his.ShowDialog() == DialogResult.Cancel)
            { //* 处理重打文段时获取焦点
                this.textBoxEx1.Focus();
            }
        }
        #endregion

        #region 按键统计
        private void KeyAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyAn kan = new KeyAn(Glob.KeysTotal, Glob.TextTime.ToString("G"));
            kan.Show();
        }

        private void CalcKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Glob.UsedTableIndex) && Glob.词库理论码长 > 0)
            {
                KeyAn kan = new KeyAn(Glob.CalcKeysTotal, Glob.TextTime.ToString("G"), "理论按键统计");
                kan.Show();
            }
        }
        #endregion

        #region 历史按键热图
        private void HistoryKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyAn kan = new KeyAn(Glob.HistoryKeysTotal, "历史按键热图", "历史按键统计");
            kan.ShowDialog();
        }
        #endregion

        #region 表格右键菜单事件
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.gridHandler.SetMouseLocation(e);
            this.ItemToolStripTextBox.Text = this.gridHandler.MenuGetScoreTime();
        }

        private void CopyScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CopyScore(this.currentScoreData);
        }

        private void CopyPicScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CopyPicScore(this.currentScoreData);
        }

        private void CopyTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CopyTitle();
        }

        private void CopyCotentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CopyContent(this.currentScoreData);
        }

        private void GridSpeedAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.SpeedAn(this.currentScoreData, this);
        }

        private void GridTypeAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.TypeAn(this.currentScoreData);
        }

        private void GridKeyAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.KeyAn();
        }

        private void GridCalcKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CalcKeys();
        }

        private void GridRetypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] result = this.gridHandler.GetRetype(this.currentScoreData);
            if (result != null)
            {
                this.TypeContentDirectly(result[0], result[1], result[2]);
            }
        }
        #endregion

        #region 码表管理
        private void CodeTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeTableBox ctb = new CodeTableBox();
            ctb.ShowDialog();
        }
        #endregion

        #region 曲线显示的控制
        private void tbnSpline_Click(object sender, EventArgs e)
        {
            _Ini Setupini = new _Ini("config.ini");
            //是否显示曲线
            if (this.tbnSpline.Checked)
            {
                splitContainer4.Panel1Collapsed = true;
                Setupini.IniWriteValue("拖动条", "曲线", "True");
                this.tbnSpline.Checked = false;
            }
            else
            {
                splitContainer4.Panel1Collapsed = false;
                Setupini.IniWriteValue("拖动条", "曲线", "False");
                this.tbnSpline.Checked = true;
            }
        }
        #endregion

        #region 跟打报告
        private void 跟打报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowsFormsApplication2.跟打报告.TypeAnalysis tya = new 跟打报告.TypeAnalysis(Glob.TextTime.ToString("G"), Glob.TypeReport, Glob.TypeText, Glob.TextSpeed.ToString("0.00"), Glob.TextHg, Glob.Instration);
            tya.Show();
        }
        #endregion

        #region 速度评级
        private void SpeedGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpeedGradeBox sgb = new SpeedGradeBox();
            sgb.ShowDialog();
        }
        #endregion
    }
}