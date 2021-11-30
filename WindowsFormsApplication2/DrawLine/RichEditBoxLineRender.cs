using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using WindowsFormsApplication2.DrawLine;
using WindowsFormsApplication2.编码提示;

namespace WindowsFormsApplication2
{
    public partial class RichEditBoxLineRender : RichTextBox
    {
        private readonly List<Label> floatLines = new List<Label>();
        private readonly List<LineInfo> lineInfos = new List<LineInfo>();
        private int floatIndex = 0;
        /// <summary>
        /// 当前跟打的位置
        /// - 防止画线时已开始跟打导致颜色异常
        /// </summary>
        private int currentIndex = 0;
        /// <summary>
        /// 对照区正确字的背景色
        /// </summary>
        private Color bkRightColor = Color.White;
        public SizeF charSize = new SizeF();

        public enum LineMode
        {
            Start = 0,
            Middle = 1,
            End = 2,
            Single = 3
        }

        public RichEditBoxLineRender()
        {
            InitializeComponent();
        }

        public RichEditBoxLineRender(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// 移除线条
        /// </summary>
        public void ClearLines()
        {
            this.lineInfos.Clear();
            foreach (Label l in floatLines)
            {
                l.Visible = false;
            }
        }

        /// <summary>
        /// 绘制标注线条
        /// </summary>
        /// <param name="bmAlls"></param>
        /// <param name="_rightColor"></param>
        public void Render(List<BmAll> bmAlls, Color _rightColor)
        {
            this.bkRightColor = _rightColor;

            using (Graphics g = this.CreateGraphics())
            {
                this.charSize = g.MeasureString("测", this.Font);
            }

            this.ClearLines();

            int index = 0;
            foreach (var bm in bmAlls)
            {
                if (bm.查询的字.Length > 1)
                { //* 标记词组
                    AddLineForOneWord(index, index + bm.查询的字.Length - 1, GetColor(bm.重数));
                    index += bm.查询的字.Length;
                }
                else
                {
                    if (bm.重数 > 0)
                    { //* 标记单字中的重码
                        AddLineForOneChar(index, LineMode.Single, GetColor(bm.重数));
                    }
                    index++;
                }

                if (index >= Glob.TypeText.Length)
                {
                    break;
                }
            }

            this.ShowLines();
        }

        /// <summary>
        /// 根据重码数获取对应的颜色
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Color GetColor(int n)
        {
            switch (n)
            {
                case 0:
                    return Glob.Words0Color;
                case 1:
                    return Glob.Words1Color;
                case 2:
                    return Glob.Words2Color;
                default:
                    return Glob.Words3Color;
            }
        }

        public void SetCurIndex(int cur)
        {
            this.currentIndex = cur;
            for (int i = 0; i < this.floatIndex; i++)
            {
                this.floatLines[i].Refresh(); // 重画
            }
        }

        /// <summary>
        /// 为词组添加线条信息
        /// - 注：包括 endIndex 上的字
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="color"></param>
        private void AddLineForOneWord(int startIndex, int endIndex, Color color)
        {
            if (endIndex > startIndex)
            {
                AddLineForOneChar(startIndex, LineMode.Start, color);
                for (int i = startIndex + 1; i < endIndex; i++)
                {
                    AddLineForOneChar(i, LineMode.Middle, color);
                }
                AddLineForOneChar(endIndex, LineMode.End, color);
            }
        }

        private void AddLineForOneChar(int index, LineMode mode, Color color)
        {
            try 
            {
                Point pt = this.GetPositionFromCharIndex(index); // 当前字的位置
                if (pt.X < 0 || pt.Y < 0 || (pt.Y + charSize.Height) > this.Height)
                {
                    return;
                }

                int left = pt.X;
                int top = pt.Y + (int)charSize.Height + 1;
                int width = Math.Min((int)charSize.Width, this.ClientRectangle.Width - left);

                int space = width / 5;
                switch (mode)
                {
                    case LineMode.Start:
                        left += space;
                        width -= space;
                        break;
                    case LineMode.End:
                        width -= space;
                        break;
                    case LineMode.Single:
                        left += space;
                        width -= 2 * space;
                        break;
                }

                LineInfo line = new LineInfo
                {
                    Index = index,
                    Color = color,
                    Left = left,
                    Top = top,
                    Width = width
                };
                this.lineInfos.Add(line);
            }
            catch { }
        }

        private void ShowLines()
        {
            if (lineInfos.Count > 0)
            {
                this.floatIndex = 0;

                foreach (int top in lineInfos.Select(x => x.Top).Distinct())
                {
                    Label floatLine = GetFreeFloatLine();
                    floatLine.Tag = lineInfos.Where(o => o.Top == top).ToList(); // 将这条线上所有需要的线段信息放至 Tag

                    //* 绘制这行线
                    floatLine.Top = top;
                    floatLine.Left = 0;
                    floatLine.Height = 1;
                    floatLine.Width = this.ClientSize.Width;
                    floatLine.BackColor = this.BackColor;
                }

                for (int i = 0; i < this.floatIndex; i++)
                {
                    this.floatLines[i].Visible = true;
                }

                this.SendToBack(); // 确保对照区文本框本身始终在控件底层
            }
        }

        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctl"></param>
        /// <param name="lInfos"></param>
        private void PaintLinesBG(PaintEventArgs e, Label ctl, List<LineInfo> lInfos)
        {
            int ctlLine = this.GetLineFromCharIndex(lInfos[0].Index);
            int curLine = this.GetLineFromCharIndex(this.currentIndex);

            if (ctlLine < curLine)
            { // 该行都已跟打过
                using (Pen p = new Pen(this.bkRightColor, 1))
                {
                    e.Graphics.DrawLine(p, 1, 0, ctl.Width, 0);
                }
            }
            else if (ctlLine > curLine)
            { // 该行都未跟打
                using (Pen p = new Pen(this.BackColor, 1))
                {
                    e.Graphics.DrawLine(p, 1, 0, ctl.Width, 0);
                }
            }
            else
            { // 正在跟打该行
                Point pt = this.GetPositionFromCharIndex(this.currentIndex);

                using (Pen p = new Pen(this.bkRightColor, 2))
                {
                    e.Graphics.DrawLine(p, 1, 0, pt.X, 0);
                }

                using (Pen p = new Pen(this.BackColor, 2))
                {
                    e.Graphics.DrawLine(p, pt.X, 0, ctl.Width, 0);
                }
            }
        }

        /// <summary>
        /// 绘制线条的方法
        /// - sender.Tag 为 List<LineInfo>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloatLine_Paint(object sender, PaintEventArgs e)
        {
            Label ctl = sender as Label;
            try
            {
                List<LineInfo> lInfos = ctl.Tag as List<LineInfo>;
                if (lInfos.Count == 0)
                {
                    return;
                }

                // 绘制背景
                PaintLinesBG(e, ctl, lInfos);

                int placeFixed = (int)charSize.Width / 7; // 因为坐标 left 会存在向右偏移，用部分字宽来修正
                // 绘制字符
                foreach (LineInfo li in lInfos)
                {
                    using (Pen p = new Pen(li.Color, 2))
                    {
                        e.Graphics.DrawLine(p, li.Left - placeFixed, 0, li.Left + li.Width - placeFixed, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        /// <summary>
        /// 获取新的浮动控件
        /// - 如果池子中已经有空闲的，那就直接使用，否则创建
        /// </summary>
        /// <returns></returns>
        private Label GetFreeFloatLine()
        {
            Label floatLine;

            if (this.floatIndex >= this.floatLines.Count)
            {
                floatLine = new Label();
                this.floatLines.Add(floatLine);
                this.Controls.Add(floatLine);
                floatLine.Paint += new PaintEventHandler(FloatLine_Paint);
            }
            else
            {
                floatLine = this.floatLines[this.floatIndex];
            }

            this.floatIndex++;

            return floatLine;
        }
    }
}
