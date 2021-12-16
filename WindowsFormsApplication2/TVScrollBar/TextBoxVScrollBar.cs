using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2.TVScrollBar
{
    public partial class TextBoxVScrollBar : UserControl
    {
        #region 基础变量
        /// <summary>
        /// 轨道颜色
        /// - 默认为烟白色
        /// </summary>
        protected Color tChannelColor = Color.WhiteSmoke;

        /// <summary>
        /// 向上箭头
        /// </summary>
        protected Image tUpArrowImage = null;

        /// <summary>
        /// 向下箭头
        /// </summary>
        protected Image tDownArrowImage = null;

        /// <summary>
        /// 箭头背景色
        /// - 默认为烟白色
        /// </summary>
        protected Color tArrowBackColor = Color.FromArgb(235, 235, 235);

        /// <summary>
        /// 拇指滑块颜色
        /// - 默认为银白色
        /// </summary>
        protected Color tThumbColor = Color.Silver;

        protected int tLargeChange = 10;
        protected int tSmallChange = 1;
        protected int tMinimum = 0;
        protected int tMaximum = 0;
        protected int tValue = 0;
        #endregion

        #region 内部变量
        private enum hoverRect
        {
            None = 0,
            Up = 1,
            Thumb = 2,
            Down = 3
        }
        /// <summary>
        /// 鼠标点击位置
        /// - 与滑块顶部间的距离
        /// </summary>
        private int clickPoint = 0;
        private bool thumbVisible = false;
        private int thumbTop = 0;
        /// <summary>
        /// 滑块高度
        /// </summary>
        private int thumbHeight = 0;
        private bool thumbMouseDown = false;
        private bool thumbDragging = false;
        private hoverRect hover = hoverRect.None;

        /// <summary>
        /// 轨道高度
        /// - 不包括上下箭头
        /// </summary>
        private int trackHeight
        {
            get
            {
                return this.Height - this.Width * 2;
            }
        }

        /// <summary>
        /// 操作区域的真实高度范围
        /// </summary>
        private int realRange
        {
            get
            {
                return this.tMaximum - this.tMinimum;
            }
        }

        /// <summary>
        /// 拇指滑块可移动范围
        /// - 即减去箭头高度和滑块高度
        /// </summary>
        private int pixelRange
        {
            get
            {
                return this.trackHeight - this.thumbHeight;
            }
        }
        #endregion

        #region 公开事件
        /// <summary>
        /// 滚动处理事件
        /// </summary>
        public new event EventHandler VScroll = null;
        /// <summary>
        /// 滚动条值变动处理事件
        /// </summary>
        public event EventHandler ValueChanged = null;
        #endregion

        #region 公开属性
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("行为"), Description("获取或设置大移矩")]
        public int LargeChange
        {
            get { return tLargeChange; }
            set { tLargeChange = value; Invalidate(); }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("行为"), Description("获取或设置小移矩")]
        public int SmallChange
        {
            get { return tSmallChange; }
            set { tSmallChange = value; Invalidate(); }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("行为"), Description("获取或设置控制区最小值")]
        public int Minimum
        {
            get { return tMinimum; }
            set { tMinimum = value; Invalidate(); }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("行为"), Description("获取或设置控制区最大值")]
        public int Maximum
        {
            get { return tMaximum; }
            set
            {
                tMaximum = value > 0 ? value : 0;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("行为"), Description("获取或设置滚动条的值")]
        public int Value
        {
            get { return tValue; }
            set
            {
                tValue = value;
                this.thumbTop = (int)Math.Ceiling((double)tValue / tMaximum * this.pixelRange);
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("外观"), Description("获取或设置轨道颜色")]
        public Color ChannelColor
        {
            get { return tChannelColor; }
            set { tChannelColor = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("外观"), Description("获取或设置向上箭头图片")]
        public Image UpArrowImage
        {
            get { return tUpArrowImage; }
            set { tUpArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("外观"), Description("获取或设置向下箭头图片")]
        public Image DownArrowImage
        {
            get { return tDownArrowImage; }
            set { tDownArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("外观"), Description("获取或设置向上箭头背景色")]
        public Color ArrowBackColor
        {
            get { return tArrowBackColor; }
            set { tArrowBackColor = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("外观"), Description("获取或设置拇指滑块颜色")]
        public Color ThumbColor
        {
            get { return tThumbColor; }
            set { tThumbColor = value; }
        }
        #endregion

        public TextBoxVScrollBar()
        {
            InitializeComponent();
        }

        public TextBoxVScrollBar(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            Brush upArrowBackBrush = new SolidBrush(this.hover == hoverRect.Up ? Theme.GetReverseColor(this.tArrowBackColor) : this.tArrowBackColor);
            Brush downArrowBackBrush = new SolidBrush(this.hover == hoverRect.Down ? Theme.GetReverseColor(this.tArrowBackColor) : this.tArrowBackColor);
            g.FillRectangle(upArrowBackBrush, new Rectangle(new Point(0, 0), new Size(this.Width, this.Width)));
            g.FillRectangle(downArrowBackBrush, new Rectangle(new Point(0, this.Height - this.Width), new Size(this.Width, this.Width)));

            if (UpArrowImage != null)
            {
                g.DrawImage(UpArrowImage, new Rectangle(new Point(0, 0), new Size(this.Width, this.Width)));
            }
            else
            {
                Brush upArrowBrush = new SolidBrush(this.hover == hoverRect.Up ? this.tArrowBackColor : this.tThumbColor);
                Point upt1 = new Point(9, 5);
                Point upt2 = new Point(3, 11);
                Point upt3 = new Point(15, 11);
                Point[] uptArr = { upt1 , upt2 , upt3 };
                g.FillPolygon(upArrowBrush, uptArr);
            }
            if (DownArrowImage != null)
            {
                g.DrawImage(DownArrowImage, new Rectangle(new Point(0, this.Height - this.Width), new Size(this.Width, this.Width)));
            }
            else
            {
                Brush downArrowBrush = new SolidBrush(this.hover == hoverRect.Down ? this.tArrowBackColor : this.tThumbColor);
                Point dpt1 = new Point(9, this.Height - this.Width + 12);
                Point dpt2 = new Point(4, this.Height - this.Width + 7);
                Point dpt3 = new Point(14, this.Height - this.Width + 7);
                Point[] dptArr = { dpt1, dpt2, dpt3 };
                g.FillPolygon(downArrowBrush, dptArr);
            }

            Brush channelBrush = new SolidBrush(tChannelColor);
            g.FillRectangle(channelBrush, new Rectangle(new Point(0, this.Width), new Size(this.Width, this.trackHeight)));

            if (Maximum > 0 )
            {
                this.thumbHeight = this.Height * this.trackHeight / (Maximum + this.Height); // 当 Maxinum > 0 以后，thumbHeight 恒小于 trackHeight
                this.thumbVisible = true;
                if (this.thumbHeight < SmallChange)
                { //? 设定一个滑块的最小高度
                    this.thumbHeight = SmallChange;
                }

                if (this.thumbVisible)
                { // 滑块可见时
                    Brush thumbBrush = new SolidBrush(this.hover == hoverRect.Thumb ? Theme.GetReverseColor(this.tThumbColor) : this.tThumbColor);
                    g.FillRectangle(thumbBrush, new Rectangle(new Point(1, this.Width + this.thumbTop), new Size(this.Width - 2, this.thumbHeight)));
                }
            }
        }

        private void CustomMouseDown(object sender, MouseEventArgs e)
        {
            if (this.thumbVisible && e.Button == MouseButtons.Left)
            {
                Point pt = this.PointToClient(Cursor.Position);

                Rectangle thumbRect = new Rectangle(new Point(1, this.Width + this.thumbTop), new Size(this.Width - 2, this.thumbHeight));
                if (thumbRect.Contains(pt))
                { //* 点击了滑块
                    this.clickPoint = pt.Y - this.thumbTop;
                    this.thumbMouseDown = true;
                }
                else if (this.realRange > 0 && this.pixelRange > 0)
                {
                    Rectangle upArrowRect = new Rectangle(new Point(0, 0), new Size(this.Width, this.Width));
                    if (upArrowRect.Contains(pt))
                    { //* 点击了向上箭头
                        if (this.thumbTop > 0)
                        {
                            this.thumbTop = this.thumbTop - SmallChange < 0 ? 0 : this.thumbTop - SmallChange;
                            float uPrec = (float)this.thumbTop / this.pixelRange;
                            this.tValue = (int)(uPrec * this.realRange);
                            ValueChanged?.Invoke(this, new EventArgs());
                            VScroll?.Invoke(this, new EventArgs());
                            Invalidate();
                        }
                    }
                    else
                    {
                        Rectangle downArrowRect = new Rectangle(new Point(0, this.Height - this.Width), new Size(this.Width, this.Width));
                        if (downArrowRect.Contains(pt))
                        { //* 点击了向下箭头
                            if (this.thumbTop < this.pixelRange)
                            {
                                this.thumbTop = this.thumbTop + SmallChange > this.pixelRange ? this.pixelRange : this.thumbTop + SmallChange;
                                float dPrec = (float)this.thumbTop / this.pixelRange;
                                this.tValue = (int)(dPrec * this.realRange);
                                ValueChanged?.Invoke(this, new EventArgs());
                                VScroll?.Invoke(this, new EventArgs());
                                Invalidate();
                            }
                        }
                        else
                        { //* 点击了轨道
                            if (pt.Y < this.thumbTop)
                            {
                                this.thumbTop = this.thumbTop - LargeChange < 0 ? 0 : this.thumbTop - LargeChange;
                            }
                            else
                            {
                                this.thumbTop = this.thumbTop + LargeChange > this.pixelRange ? this.pixelRange : this.thumbTop + LargeChange;
                            }
                            float tPrec = (float)this.thumbTop / this.pixelRange;
                            this.tValue = (int)(tPrec * this.realRange);
                            ValueChanged?.Invoke(this, new EventArgs());
                            VScroll?.Invoke(this, new EventArgs());
                            Invalidate();
                        }
                    }
                }
            }
        }

        private void CustomMouseUp(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.thumbMouseDown = false;
                this.thumbDragging = false;
            }
        }

        private void MoveThumb(int y)
        {
            if (this.thumbMouseDown && this.thumbDragging && this.realRange > 0 && this.pixelRange > 0)
            {
                int span = y - (this.thumbTop + this.clickPoint);
                if (span < 0)
                {
                    this.thumbTop = this.thumbTop + span < 0 ? 0 : this.thumbTop + span;
                }
                else
                {
                    this.thumbTop = this.thumbTop + span > this.pixelRange ? this.pixelRange : this.thumbTop + span;
                }
                float tPrec = (float)this.thumbTop / this.pixelRange;
                this.tValue = (int)(tPrec * this.realRange);
                Application.DoEvents();
                Invalidate();
            }
        }

        private void CustomMouseMove(object sender, MouseEventArgs e)
        {
            //* 处理高亮
            if (this.thumbVisible)
            {
                Point pt = this.PointToClient(Cursor.Position);

                Rectangle thumbRect = new Rectangle(new Point(1, this.Width + this.thumbTop), new Size(this.Width - 2, this.thumbHeight));
                if (thumbRect.Contains(pt))
                { //* 滑块的上方
                    if (this.hover != hoverRect.Thumb)
                    {
                        this.hover = hoverRect.Thumb;
                        Invalidate();
                    }
                }
                else
                {
                    Rectangle upArrowRect = new Rectangle(new Point(0, 0), new Size(this.Width, this.Width));
                    if (upArrowRect.Contains(pt))
                    { //* 向上箭头的上方
                        if (this.hover != hoverRect.Up)
                        {
                            this.hover = hoverRect.Up;
                            Invalidate();
                        }
                    }
                    else
                    {
                        Rectangle downArrowRect = new Rectangle(new Point(0, this.Height - this.Width), new Size(this.Width, this.Width));
                        if (downArrowRect.Contains(pt))
                        { //* 向下箭头的上方
                            if (this.hover != hoverRect.Down)
                            {
                                this.hover = hoverRect.Down;
                                Invalidate();
                            }
                        }
                        else
                        {
                            if (this.hover != hoverRect.None)
                            {
                                this.hover = hoverRect.None;
                                Invalidate();
                            }
                        }
                    }
                }
            }

            if (e.Button == MouseButtons.Left)
            { //* 处理拖动
                if (this.thumbMouseDown)
                {
                    this.thumbDragging = true;
                }

                if (this.thumbDragging)
                {
                    MoveThumb(e.Y);
                    ValueChanged?.Invoke(this, new EventArgs());
                    VScroll?.Invoke(this, new EventArgs());
                }
            }
        }

        private void CustomMouseWhell(object sender, MouseEventArgs e)
        {
            if (this.thumbVisible)
            {
                if (e.Delta > 0)
                { // 向上
                    if (this.thumbTop > 0)
                    {
                        this.thumbTop = this.thumbTop - SmallChange < 0 ? 0 : this.thumbTop - SmallChange;
                        float uPrec = (float)this.thumbTop / this.pixelRange;
                        this.tValue = (int)(uPrec * this.realRange);
                        ValueChanged?.Invoke(this, new EventArgs());
                        VScroll?.Invoke(this, new EventArgs());
                        Invalidate();
                    }
                }
                else if (e.Delta < 0)
                { // 向下
                    if (this.thumbTop < this.pixelRange)
                    {
                        this.thumbTop = this.thumbTop + SmallChange > this.pixelRange ? this.pixelRange : this.thumbTop + SmallChange;
                        float dPrec = (float)this.thumbTop / this.pixelRange;
                        this.tValue = (int)(dPrec * this.realRange);
                        ValueChanged?.Invoke(this, new EventArgs());
                        VScroll?.Invoke(this, new EventArgs());
                        Invalidate();
                    }
                }
            }
        }

        private void CustomMouseLeave(object sender, EventArgs e)
        {
            if (this.thumbVisible && this.hover != hoverRect.None)
            {
                this.hover = hoverRect.None;
                Invalidate();
            }
        }
    }
}
