using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions; //正则

namespace WindowsFormsApplication2
{
    public partial class TSetup : Form
    {
        readonly Form1 frm;
        public Font fo1, fo2;

        /// <summary>
        /// 快捷键输入框
        /// </summary>
        private TextBox[] allTBox;

        /// <summary>
        /// 快捷键修改按钮
        /// </summary>
        private Button[] allModBtn;

        /// <summary>
        /// 格式控制选项
        /// </summary>
        private CheckBox[] allCheckBox;

        public TSetup(Form1 frm1)
        {
            frm = frm1;
            InitializeComponent();
        }

        /// <summary>
        /// 设置页面的读取配置方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setup_Load(object sender, EventArgs e)
        {
            string gq = IniRead("个签", "标志", "0");
            if (gq != "0") //个签初始化
            {
                this.checkBox1.Checked = true;
            }
            else
            {
                this.checkBox1.Checked = false;
            }
            this.textBox1.Text = IniRead("个签", "签名", "");

            //输入法签名初始化
            string srfsave = IniRead("输入法", "标志", "0");
            if (srfsave != "0")
            {
                this.checkBox3.Checked = true;
            }
            else
            {
                this.checkBox3.Checked = false;
            }
            this.textBox2.Text = IniRead("输入法", "签名", "");

            allCheckBox = new CheckBox[22]
            {
                this.checkBoxSpeed,
                this.checkBox4,
                this.checkBox5,
                this.checkBox6,
                this.checkBox7,
                this.checkBox8,
                this.checkBox9,
                this.checkBox10,
                this.checkBox11,
                this.checkBox12,
                this.checkBox2,
                this.checkBox15,
                this.checkBox14,
                this.checkBox17,
                this.checkBox18,
                this.checkBox20,
                this.checkBox24,
                this.checkBox25,
                this.checkBox26,
                this.checkBox27,
                this.checkBox29,
                this.checkBox31
            };

            //排序顺序初始化
            SortSend();

            //载入字体
            FontConverter fc = new FontConverter();
            fo1 = (Font)fc.ConvertFromString(IniRead("外观", "对照区字体", "宋体, 21.75pt"));
            fo2 = (Font)fc.ConvertFromString(IniRead("外观", "跟打区字体", "宋体, 12pt"));
            this.button3.Text = fo1.FontFamily.GetName(0) + " - " + fo1.Size;
            this.button4.Text = fo2.FontFamily.GetName(0) + " - " + fo2.Size;

            //各个外观配置初始化
            buttoncolor1.BackColor = frm.richTextBox1.BackColor; //对照区底色
            buttoncolor2.BackColor = frm.textBoxEx1.BackColor; //跟打区底色
            this.pictureBoxRight.BackColor = Glob.Right;
            this.pictureBoxFalse.BackColor = Glob.False;

            //载入初始化
            string pretext = IniRead("载入", "前导", "-----");
            string preduan = IniRead("载入", "段标", "第xx段");
            bool ison = bool.Parse(IniRead("载入", "开启", "False"));
            this.checkBox19.Checked = ison;
            this.textBoxPreText.Text = pretext;
            this.textBoxDuan.Text = preduan;

            // 昵称
            this.tbxName.Text = IniRead("发送", "昵称", this.tbxName.Text);
            //停止时间初始化
            int StopTime = int.Parse(IniRead("控制", "停止", "1"));
            if (StopTime < 1 || StopTime > 10)
            {
                StopTime = 1;
            }
            this.trackBar2.Value = StopTime;
            this.label17.Text = StopTime + "分";
            //极简设置
            this.SimpleCheckBox.Checked = Glob.simpleMoudle;
            this.SimpleTextBox.Text = Glob.simpleSplite;

            //* 使用顶功输入法
            this.DGCheckBox.Checked = Glob.UseDGInput;

            //* 四码唯一自动上屏
            this.AutoInputCheckBox.Checked = Glob.UseAutoInput;

            //* 使用 ;' 选重
            this.SymbolCheckBox.Checked = Glob.UseSymbolSelect;

            //* 使用 Z 键复打
            this.ZCheckBox.Checked = Glob.UseZRetype;

            //* 禁止保存高阶统计
            this.AdvancedCheckBox.Checked = Glob.DisableSaveAdvanced;

            //* 快捷键设置
            allTBox = new TextBox[25] {
                HotKeyTextBox0,
                HotKeyTextBox1,
                HotKeyTextBox2,
                HotKeyTextBox3,
                HotKeyTextBox4,
                HotKeyTextBox5,
                HotKeyTextBox6,
                HotKeyTextBox7,
                HotKeyTextBox8,
                HotKeyTextBox9,
                HotKeyTextBox10,
                HotKeyTextBox11,
                HotKeyTextBox12,
                HotKeyTextBox13,
                HotKeyTextBox14,
                HotKeyTextBox15,
                HotKeyTextBox16,
                HotKeyTextBox17,
                HotKeyTextBox18,
                HotKeyTextBox19,
                HotKeyTextBox20,
                HotKeyTextBox21,
                HotKeyTextBox22,
                HotKeyTextBox23,
                HotKeyTextBox24
            };
            allModBtn = new Button[25]
            {
                HotKeyModButton0,
                HotKeyModButton1,
                HotKeyModButton2,
                HotKeyModButton3,
                HotKeyModButton4,
                HotKeyModButton5,
                HotKeyModButton6,
                HotKeyModButton7,
                HotKeyModButton8,
                HotKeyModButton9,
                HotKeyModButton10,
                HotKeyModButton11,
                HotKeyModButton12,
                HotKeyModButton13,
                HotKeyModButton14,
                HotKeyModButton15,
                HotKeyModButton16,
                HotKeyModButton17,
                HotKeyModButton18,
                HotKeyModButton19,
                HotKeyModButton20,
                HotKeyModButton21,
                HotKeyModButton22,
                HotKeyModButton23,
                HotKeyModButton24
            };
            // 读取保存的快捷键
            for (int i = 0; i < allTBox.Length; i++)
            {
                allTBox[i].Text = IniRead("快捷键", Glob.HotKeyList[i].GetId(), Glob.HotKeyList[i].GetDefaultKeys());
                allTBox[i].Tag = allTBox[i].Text;
            }
        }

        public string IniRead(string section, string key, string def)
        { //ini的快捷读取
            _Ini sing = new _Ini("config.ini");
            return sing.IniReadValue(section, key, def);
        }

        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            _Ini Setupini = new _Ini("config.ini");

            gQ();//保存个签
            //保存输入法签名
            srfSave();
            //颜色设置
            Setupini.IniWriteValue("外观", "对照区颜色", buttoncolor1.BackColor.ToArgb().ToString());
            Glob.r1Back = buttoncolor1.BackColor;
            Setupini.IniWriteValue("外观", "跟打区颜色", buttoncolor2.BackColor.ToArgb().ToString());
            Setupini.IniWriteValue("外观", "打对颜色", this.pictureBoxRight.BackColor.ToArgb().ToString());
            Glob.Right = this.pictureBoxRight.BackColor;
            Setupini.IniWriteValue("外观", "打错颜色", this.pictureBoxFalse.BackColor.ToArgb().ToString());
            Glob.Right = this.pictureBoxFalse.BackColor;
            //字体设置
            FontConverter fc = new FontConverter();
            Setupini.IniWriteValue("外观", "对照区字体", fc.ConvertToInvariantString(fo1));
            Setupini.IniWriteValue("外观", "跟打区字体", fc.ConvertToInvariantString(fo2));
            //Point a1 = frm.richTextBox1.GetPositionFromCharIndex(1);
            //Point a2 = frm.richTextBox1.GetPositionFromCharIndex(frm.richTextBox1.GetFirstCharIndexFromLine(1));
            frm.richTextBox1.Font = fo1;
            frm.textBoxEx1.Font = fo2;
            //前导
            if (checkBox19.Checked)
            {
                if (this.textBoxDuan.Text.Contains("xx"))
                {
                    Setupini.IniWriteValue("载入", "前导", this.textBoxPreText.Text);
                    Setupini.IniWriteValue("载入", "段标", this.textBoxDuan.Text);
                    Setupini.IniWriteValue("载入", "开启", this.checkBox19.Checked.ToString());
                }
                else
                {
                    MessageBox.Show("段标输入错误！未保存！", "警告");
                    return;
                }
            }
            else
            {
                Setupini.IniWriteValue("载入", "开启", this.checkBox19.Checked.ToString());
            }

            //停止时间
            Setupini.IniWriteValue("控制", "停止", this.trackBar2.Value.ToString());
            Glob.StopUse = this.trackBar2.Value;
            frm.toolTip1.SetToolTip(frm.lblAutoReType, "跟打停止时间，大于" + this.trackBar2.Value + "分钟时自动停止跟打");

            //极简模式
            Setupini.IniWriteValue("发送", "极简状态", this.SimpleCheckBox.Checked.ToString());
            Setupini.IniWriteValue("发送", "分隔符", this.SimpleTextBox.Text);
            Glob.simpleMoudle = this.SimpleCheckBox.Checked;
            Glob.simpleSplite = this.SimpleTextBox.Text;

            if (!saveSort())
            {
                MessageBox.Show(this, "含有错误排序字符，请重新检查！", "雨天跟打器排序提示");
                return;
            }
            else
            {
                Setupini.IniWriteValue("发送", "顺序", textBox3.Text);
            }

            if (this.AdvancedCheckBox.Checked)
            {
                Setupini.IniWriteValue("控制", "不保存高阶", "True");
                Glob.DisableSaveAdvanced = true;
            }
            else
            {
                Setupini.IniWriteValue("控制", "不保存高阶", "False");
                Glob.DisableSaveAdvanced = false;
            }

            if (this.DGCheckBox.Checked)
            {
                Setupini.IniWriteValue("控制", "顶功输入", "True");
                Glob.UseDGInput = true;
            }
            else
            {
                Setupini.IniWriteValue("控制", "顶功输入", "False");
                Glob.UseDGInput = false;
            }

            if (this.AutoInputCheckBox.Checked)
            {
                Setupini.IniWriteValue("控制", "四码唯一", "True");
                Glob.UseAutoInput = true;
            }
            else
            {
                Setupini.IniWriteValue("控制", "四码唯一", "False");
                Glob.UseAutoInput = false;
            }

            if (this.SymbolCheckBox.Checked)
            {
                Setupini.IniWriteValue("控制", "符号选重", "True");
                Glob.UseSymbolSelect = true;
            }
            else
            {
                Setupini.IniWriteValue("控制", "符号选重", "False");
                Glob.UseSymbolSelect = false;
            }

            if (this.ZCheckBox.Checked)
            {
                Setupini.IniWriteValue("控制", "Z键复打", "True");
                Glob.UseZRetype = true;
            }
            else
            {
                Setupini.IniWriteValue("控制", "Z键复打", "False");
                Glob.UseZRetype = false;
            }

            if (this.checkBox1.Checked)
            {
                Glob.InstraPre_ = "1";
            }
            else
            {
                Glob.InstraPre_ = "0";
            }
            Glob.InstraPre = this.textBox1.Text; //个签
            Glob.InstraSrf = this.textBox2.Text; //输入法签名
            Glob.InstraSrf_ = IniRead("输入法", "标志", "0");
            Glob.binput = true;//输入法修改

            Glob.sortSend = this.textBox3.Text;
            Glob.Right = pictureBoxRight.BackColor;
            Glob.False = pictureBoxFalse.BackColor;
            frm.richTextBox1.BackColor = this.buttoncolor1.BackColor;
            frm.textBoxEx1.BackColor = this.buttoncolor2.BackColor;
            if (checkBox19.Checked)
            {
                Glob.PreText = this.textBoxPreText.Text;
                Glob.PreDuan = this.textBoxDuan.Text;
                Glob.isZdy = true;
            }
            else
            {
                Glob.PreText = "-----";
                Glob.PreDuan = "第xx段";
            }

            //图片成绩发送昵称
            Setupini.IniWriteValue("发送", "昵称", this.tbxName.Text);
            Glob.PicName = this.tbxName.Text;

            if (File.Exists("config.ini"))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("文件丢失！");
            }

            //* 保存快捷键设置
            for (int i = 0; i < allTBox.Length; i++)
            {
                Glob.HotKeyList[i].SetKeys(allTBox[i].Tag.ToString());
                Setupini.IniWriteValue("快捷键", Glob.HotKeyList[i].GetId(), Glob.HotKeyList[i].GetKeys());
            }
            //* 快捷键处理
            frm.HotKeyHandler();
        }

        public void gQ()
        {
            _Ini Setupini = new _Ini("config.ini");
            if (this.checkBox1.Checked)
            {
                if (this.textBox1.Text != "")
                {
                    Setupini.IniWriteValue("个签", "签名", this.textBox1.Text);
                    Setupini.IniWriteValue("个签", "标志", "1");
                }
                else
                {
                    Setupini.IniWriteValue("个签", "标志", "0"); // 0 表示未设置
                }
            }
            else
            {
                Setupini.IniWriteValue("个签", "标志", "0"); // 0 表示未设置
            }
        }//个签

        public void srfSave()
        {
            _Ini Setupini = new _Ini("config.ini");
            if (this.checkBox3.Checked)
            {
                if (this.textBox2.Text != "")
                {
                    Setupini.IniWriteValue("输入法", "签名", this.textBox2.Text);
                    Setupini.IniWriteValue("输入法", "标志", "1");
                }
                else
                {
                    Setupini.IniWriteValue("输入法", "标志", "0");
                }
            }
            else
            {
                Setupini.IniWriteValue("输入法", "标志", "0");
            }
        } //输入法
        //排序顺序
        public void SortSend()
        {
            string sort = IniRead("发送", "顺序", "ABCVDTSEFULGNORQ");
            textBox3.Text = sort;
            try
            {
                char[] g = sort.ToArray();
                CheckAllOut();//清空所有选择
                for (int i = 0; i < g.Length; i++)
                {
                    TestIt(g[i]); //根据当前输入 选中 或者取消选中
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //* 重新注册全局老板键
            if (Glob.HotKeyList[22].GetKeys() != "None")
            {
                frm.ReRegisterBossKey();
            }

            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.textBox1.ReadOnly = false;
                this.textBox1.BackColor = Color.White;
            }
            else
            {
                this.textBox1.ReadOnly = true;
                this.textBox1.BackColor = Color.Gray;
            }
        }

        private void textBoxQQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) //输入法签名开关
        {
            if (checkBox3.Checked)
            {
                textBox2.BackColor = Color.White;
                textBox2.ReadOnly = false;
            }
            else
            {
                textBox2.BackColor = Color.Gray;
                textBox2.ReadOnly = true;
            }
        }

        private void buttoncolor1_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = buttoncolor1.BackColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.buttoncolor1.BackColor = colorDialog1.Color;
            }
        }

        private void buttoncolor2_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = buttoncolor2.BackColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.buttoncolor2.BackColor = colorDialog1.Color;
            }
        }

        #region 数据排序
        private void textBox3Press(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3KDown(object sender, KeyEventArgs e)
        {

        }

        private void buttonGet_Click(object sender, EventArgs e)
        {
            string get = "", output = "";
            foreach (var item in this.panel9.Controls)
            {
                if (item is CheckBox)
                {
                    if ((item as CheckBox).Checked)
                    { //如果是选中状态
                        get = get + (item as CheckBox).Text.Substring(3, 1);
                    }
                }
            }
            for (int i = get.Length; i > 0; i--)
            {
                output += get.Substring(i - 1, 1);
            }
            textBox3.Text = output;
        }

        private bool saveSort()
        {
            string get = textBox3.Text.ToUpper();
            textBox3.Text = get;
            char[] g = get.ToArray();
            string get2 = "";
            if (get != "")
            {
                CheckAllOut();//清空所有选择
                for (int i = 0; i < g.Length; i++)
                {
                    get2 += GetIt(g[i]);
                    TestIt(g[i]); //根据当前输入 选中 或者取消选中
                }

                if (!get2.Contains("[错误]"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 清空所有选择
        /// </summary>
        private void CheckAllOut()
        {
            foreach (var item in this.allCheckBox)
            {
                if (item.Checked)
                {
                    item.Checked = false;
                }
            }
        }
        private void TestIt(char a)
        {
            foreach (var item in this.allCheckBox)
            {
                string checktext = CheckIt(a);
                if (checktext != "")
                {
                    if (item.Text == checktext)
                    {
                        if (!item.Checked)
                        {
                            item.Checked = true;
                        }
                        break;
                    }
                }
            }
        }
        private string CheckIt(char a)
        {
            switch (a)
            {
                case 'A': return "速度[A]";
                case 'B': return "击键[B]";
                case 'C': return "码长[C]";
                case 'D': return "回改[D]";
                case 'E': return "错字[E]";
                case 'F': return "错情[F]";
                case 'G': return "字数[G]";
                case 'H': return "键数[H]";
                case 'I': return "用时[I]";
                case 'J': return "重打[J]";
                case 'K': return "峰值[K]";
                case 'L': return "打词[L]";
                case 'M': return "回率[M]";
                case 'N': return "停留[N]";
                case 'O': return "效率[O]";
                case 'P': return "验证[P]";
                case 'Q': return "撤销[Q]";
                case 'R': return "键法[R]";
                case 'S': return "退格[S]";
                case 'T': return "回车[T]";
                case 'U': return "选重[U]";
                case 'V': return "键准[V]";
                case 'W': return "词率[W]";
                default: return "";
            }
        }
        private string GetIt(char a)
        {//返回成绩
            switch (a)
            {
                case 'A': return "速度161.53 ";
                case 'B': return "击键8.38 ";
                case 'C': return "码长3.11 ";
                case 'D': return "回改0 ";
                case 'E': return "错字0 ";
                case 'F': return "错情：无 ";
                case 'G': return "字数30 ";
                case 'H': return "键数00 ";
                case 'I': return "用时00秒 ";
                case 'J': return "重打2 ";
                case 'K': return "峰值 ";
                case 'L': return "打词2 ";
                case 'M': return "回改率0.00% ";
                case 'N': return "停留[字]XX秒";
                case 'O': return "效率100%";
                case 'P': return "校验:05555";
                case 'Q': return "撤销2";
                case 'R': return "键法";
                case 'S': return " 退格";
                case 'T': return " 回车";
                case 'U': return " 选重";
                case 'V': return " 键准";
                case 'W': return " 打词率0.00% ";
                default: return "[错误] ";
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e) //全选
        {
            foreach (var item in this.allCheckBox)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                }
            }
        }

        private void buttonCleanAll_Click(object sender, EventArgs e)
        {
            foreach (var item in this.allCheckBox)
            {
                if (item.Checked)
                {
                    item.Checked = false;
                }
            }
        }
        #endregion

        #region 字体
        private void button3_Click(object sender, EventArgs e)
        {
            this.fontDialog1.ShowEffects = false;
            this.fontDialog1.Font = fo1;
            if (this.fontDialog1.ShowDialog(this) == DialogResult.OK)
            {

                this.button3.Text = fontDialog1.Font.FontFamily.GetName(0) + " - " + fontDialog1.Font.Size;
                fo1 = fontDialog1.Font;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.fontDialog1.ShowEffects = false;
            this.fontDialog1.Font = fo2;
            if (this.fontDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.button4.Text = fontDialog1.Font.FontFamily.GetName(0) + " - " + fontDialog1.Font.Size;
                fo2 = fontDialog1.Font;
            }
        }
        #endregion

        #region 载入
        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked)
            {
                this.panelTextIn.Enabled = true;
                this.textBoxPreText.Focus();
            }
            else
            {
                this.panelTextIn.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.checkBox19.Checked = true;
            this.textBoxPreText.Text = "-----";
            this.textBoxDuan.Text = "第xx段";
        }
        #endregion

        #region 验证
        private void button7_Click(object sender, EventArgs e)
        {
            this.richTextBox2.Text = Clipboard.GetText();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            this.labelTF.Text = frm.CheckTF(richTextBox2.Text);
        }
        #endregion

        //颜色设置
        private void pictureBoxRight_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Glob.Right;
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.pictureBoxRight.BackColor = colorDialog1.Color;
            }
        }

        private void pictureBoxFalse_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = Glob.False;
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.pictureBoxFalse.BackColor = colorDialog1.Color;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.label17.Text = trackBar2.Value + "分";
        }

        #region 各项顺利控制
        //速度
        private void checkBoxSpeed_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBoxSpeed);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox4);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox5);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox6);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox7);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox8);
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox25);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox9);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox10);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox11);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox12);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox2);
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox15);
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox26);
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox14);
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox17);
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox18);
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox20);
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox24);
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox27);
        }


        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox29);
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            Change(checkBox31);
        }

        private void Change(CheckBox C)
        {
            string w = C.Text.Substring(3, 1);
            if (C.Checked)
            {
                if (!textBox3.Text.Contains(w))
                {
                    this.textBox3.Text = this.textBox3.Text.Insert(this.textBox3.TextLength, w);
                }
            }
            else
            {
                if (textBox3.Text.Contains(w))
                {
                    this.textBox3.Text = this.textBox3.Text.Replace(w, "");
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region 快捷键处理
        private void HotKeyModButtonClick(object sender, EventArgs e)
        {
            int index = int.Parse((sender as Button).Tag.ToString());
            if (index > -1 && index < allTBox.Length)
            {
                string btnText = (sender as Button).Text;
                if (btnText == "修改")
                {
                    allTBox[index].ReadOnly = false;
                    allTBox[index].BackColor = System.Drawing.SystemColors.ControlLightLight;
                    allTBox[index].Focus();
                    (sender as Button).Text = "确定";
                }
                else if (btnText == "确定")
                {
                    string newKeys = allTBox[index].Text;
                    allTBox[index].Tag = newKeys;
                    allTBox[index].ReadOnly = true;
                    allTBox[index].BackColor = System.Drawing.SystemColors.ControlDark;
                    (sender as Button).Text = "修改";

                    // 处理其它功能上可能的冲突按键
                    for (int i = 0; i < allTBox.Length; i++)
                    {
                        if (i != index)
                        {
                            if (allTBox[i].Tag.ToString() == newKeys)
                            {
                                allTBox[i].Tag = "None";
                                allTBox[i].Text = "None";
                                allTBox[i].ReadOnly = true;
                                allTBox[i].BackColor = System.Drawing.SystemColors.ControlDark;
                                allModBtn[i].Text = "修改";
                            }
                        }
                    }
                }
            }
        }

        private void HotKeyResetButtonClick(object sender, EventArgs e)
        {
            int index = int.Parse((sender as Button).Tag.ToString());
            if (index > -1 && index < allTBox.Length)
            {
                string rKeys = Glob.HotKeyList[index].GetDefaultKeys();
                allTBox[index].Tag = rKeys;
                allTBox[index].Text = rKeys;
                allTBox[index].ReadOnly = true;
                allTBox[index].BackColor = System.Drawing.SystemColors.ControlDark;
                allModBtn[index].Text = "修改";

                // 处理其它功能上可能的冲突按键
                for (int i = 0; i < allTBox.Length; i++)
                {
                    if (i != index)
                    {
                        if (allTBox[i].Tag.ToString() == rKeys)
                        {
                            allTBox[i].Tag = "None";
                            allTBox[i].Text = "None";
                            allTBox[i].ReadOnly = true;
                            allTBox[i].BackColor = System.Drawing.SystemColors.ControlDark;
                            allModBtn[i].Text = "修改";
                        }
                    }
                }
            }
        }

        private void HotKeyDisButtonClick(object sender, EventArgs e)
        {
            int index = int.Parse((sender as Button).Tag.ToString());
            if (index > -1 && index < allTBox.Length)
            {
                allTBox[index].Tag = "None";
                allTBox[index].Text = "None";
                allTBox[index].ReadOnly = true;
                allTBox[index].BackColor = System.Drawing.SystemColors.ControlDark;
                allModBtn[index].Text = "修改";
            }
        }

        private void HotKeyTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender as TextBox).ReadOnly)
            {
                (sender as TextBox).Text = "";
                if (e.Control)
                { // 优先处理按下 Ctrl 的情况
                    if ((e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12) || (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z))
                    {
                        (sender as TextBox).Text = "Ctrl+" + e.KeyCode.ToString();
                    }
                    else if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                    {
                        (sender as TextBox).Text = "Ctrl+" + e.KeyCode.ToString().Replace("D", "");
                    }
                    else
                    {
                        (sender as TextBox).Text = "None";
                    }
                }
                else if (e.Alt)
                {
                    if ((e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12) || (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z))
                    {
                        (sender as TextBox).Text = "Alt+" + e.KeyCode.ToString();
                    }
                    else if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                    {
                        (sender as TextBox).Text = "Alt+" + e.KeyCode.ToString().Replace("D", "");
                    }
                    else
                    {
                        (sender as TextBox).Text = "None";
                    }
                }
                else if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
                {
                    (sender as TextBox).Text = e.KeyCode.ToString();
                }
                else
                {
                    (sender as TextBox).Text = "None";
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(System.IntPtr ptr, int wMsg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void TSetup_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}
