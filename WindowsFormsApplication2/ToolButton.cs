using System;
//using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    public class ToolButton : ToolStripButton
    {
        private bool mouseHover = false;
        private bool checkedChange = false;
        public ToolButton()
        {
            ForeColor = Color.DimGray;
            BackColor = Color.Transparent;
            MouseEnter += new EventHandler(OnMouseEnter);
            MouseLeave += new EventHandler(OnMouseLeave);
            CheckedChanged += new EventHandler(OnCheckedChanged);
        }

        private Color _BorderColor = Color.FromArgb(36,143,108);
        /// <summary>
        /// 底线颜色
        /// </summary>
        public Color BorderColor {
            get {
                return _BorderColor;
            }
            set { _BorderColor = value; }
        }

        private int _WdithAdjust = 0;//底线宽度调整
        /// <summary>
        /// 底线宽度调整
        /// </summary>
        public int WdithAdjust {
            get {
                return _WdithAdjust;
            }
            set { _WdithAdjust = value; }
        }
        private void OnMouseEnter(object sender, EventArgs e)
        {
            mouseHover = true;
            //OnPaint(new PaintEventArgs(CreateGraphics(), ClientRectangle));
            if (mouseHover) this.Invalidate();
            //base.OnMouseEnter(e);
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            mouseHover = false;
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            if (Checked) { checkedChange = true; this.Invalidate(); }
            else checkedChange = false;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //pevent.Graphics.FillRectangle(new SolidBrush(Parent.BackColor), pevent.ClipRectangle);
            if (checkedChange) DrawCheckedButton(pevent.Graphics);
            else DrawCheckedButton_(pevent.Graphics);
            WriteText(pevent.Graphics);
        }

        /// <summary>
        /// CHECKED时下横线颜色
        /// </summary>
        /// <param name="g"></param>
        private void DrawCheckedButton(Graphics g)
        {
            Pen p = new Pen(_BorderColor, 2);
            //g.DrawLine(p, 1, 1, 1, Height - 2); //左边线
            //g.DrawLine(p, 1, 1, Width - 1, 1);//顶线
            //g.DrawLine(p, Width - 1, 1, Width - 1, Height - 2);//右边线
            g.DrawLine(p, 3, Height - 1, Width - 3 - _WdithAdjust, Height - 1);
            // PaintBack(g, SystemColors.ControlLightLight);
        }

        /// <summary>
        /// 未CHECKED时下划线
        /// </summary>
        /// <param name="g"></param>
        private void DrawCheckedButton_(Graphics g)
        {
            if (mouseHover)
            {
                Pen p = new Pen(Color.White, 2);
                //g.DrawLine(p, 1, 1, 1, Height - 2); //左边线
                //g.DrawLine(p, 1, 1, Width - 1, 1);//顶线
                //g.DrawLine(p, Width - 1, 1, Width - 1, Height - 2);//右边线
                g.DrawLine(p, 3, Height - 1, Width - 3 - _WdithAdjust, Height - 1);
                // PaintBack(g, SystemColors.ControlLightLight);
            }
        }

        private void PaintBack(Graphics g)
        {
            Color C = Color.Black;
            if (Checked) C = Color.AliceBlue;
            Pen p = new Pen(C, 1);
            g.DrawLine(p, 2, 2, 2, Height - 4); //左边线
            g.DrawLine(p, 2, 2, Width - 4, 2);//顶线
            g.DrawLine(p, Width - 4, 2, Width - 4, Height - 4);//右边线
            g.DrawLine(p, 2, Height - 4, Width - 4, Height - 4);
          //g.FillRectangle(new SolidBrush(c), 3, 3, Width - 6, Height - 6);
        }

        private void WriteText(Graphics g)
        {
            Size s = g.MeasureString(Text, Font).ToSize();
            int x = (Width - s.Width) / 2;
            int y = (Height - s.Height) / 2;
            Font BFont = new Font(Font,FontStyle.Bold);
            Brush BColor = new SolidBrush(BorderColor);
            if (Enabled)
            {
                if (Checked)
                {
                    if (mouseHover)
                    {
                        g.DrawString(Text, BFont, BColor, x, y);
                    }
                    else
                    {
                        Brush _b = new SolidBrush(Theme.tempToolButtonFc);
                        g.DrawString(Text, Font, _b, x, y);
                    }
                }
                else
                {
                    if (mouseHover)
                    {
                        g.DrawString(Text, BFont, BColor, x, y);
                    }
                    else
                    {
                        Color _c = Color.FromArgb(ColorLow(Theme.tempToolButtonFc.R), ColorLow(Theme.tempToolButtonFc.G), ColorLow(Theme.tempToolButtonFc.B));
                        Brush _b = new SolidBrush(_c);
                        g.DrawString(Text, Font, _b, x, y);
                    }
                }
            }
            else
            {
                g.DrawString(Text, Font, Brushes.DimGray, x, y);
            }
        }

        private int ColorLow(int c)
        {
            return c - 50 < 0 ? 0 : c - 50;
        }
    }
}
