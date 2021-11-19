using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
namespace WindowsFormsApplication2
{
    public partial class SpeedCheckOut : Form
    {
        private readonly Form1 frm;
        /// <summary>
        /// 提取标记前的文字数量
        /// </summary>
        private readonly int Count = 2;
        /// <summary>
        /// 寻找标记的正则
        /// </summary>
        private Regex findIndex;
        /// <summary>
        /// 赛文常用分段名称
        /// </summary>
        private readonly Regex matchIndex = new Regex("单字|散文|小说|古文|新闻|政论|名言|笑话|短信|文章");
        /// <summary>
        /// 测速点坐标列表
        /// </summary>
        private readonly List<int> getIndex = new List<int>();
        public SpeedCheckOut(Form1 frm1)
        {
            frm = frm1;
            InitializeComponent();
        }

        private void SpeedCheckOut_Load(object sender, EventArgs e)
        {
            Get(false);
        }

        /// <summary>
        /// 寻找测速点方法
        /// </summary>
        /// <param name="r">用来控制是否显示提示</param>
        private void Get(bool r)
        {
            try
            {
                string getText = frm.richTextBox1.Text;
                findIndex = new Regex(this.tbxP.Text);
                MatchCollection m = findIndex.Matches(getText);
                if (m.Count > 0)
                {
                    getIndex.Clear();
                    this.checkedListBox1.Items.Clear();
                    for (int i = 0; i < m.Count; i++)
                    {
                        int index = (m[i].Index - Count < 0) ? 0 : (m[i].Index - Count);
                        string g = getText.Substring(index, Count); // 分段名
                        bool check = matchIndex.IsMatch(g);
                        getIndex.Add(index - 1);
                        this.checkedListBox1.Items.Add(g, check);
                    }
                }
                else
                {
                    if (r)
                    {
                        MessageBox.Show("没有找有测速点，请修改分段标记后重新获取！");
                    }
                }
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
        }

        private void btnReget_Click(object sender, EventArgs e)
        {
            Get(true);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int selectC = this.checkedListBox1.CheckedItems.Count; // 选择的测速点数量
            if (selectC > 0 && getIndex.Count > 0 && selectC <= 10)
            {
                int idx;
                int idc = 0;//设置了几个
                for (int i = 0; i < selectC; i++)
                {
                    idx = getIndex[this.checkedListBox1.CheckedIndices[i]];
                    if (idx > 0)
                    {
                        frm.SetSpeedPoint(idx);
                        idc++;
                    }
                }

                if (idc > 0)
                {
                    frm.ShowFlowText("共自动设置" + idc + "个测速点(注：起始测速点不计算在内；测速点以浅灰底色标识。)");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请至少选择一个测速点！");
                }

            }
            else
            {
                if (selectC > 10)
                {
                    MessageBox.Show("最多只能设置10个测速点！");
                }
                else
                {
                    MessageBox.Show("请至少选择一个测速点！");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(System.IntPtr ptr, int wMsg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        //移动窗口
        private void SpeedCheckOut_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}
