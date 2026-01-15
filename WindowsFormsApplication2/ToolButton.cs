using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    public class ToolButton : ToolStripButton
    {
        private static ToolTip _sharedToolTip;
        private bool _tooltipShown = false;
        private string _customToolTipText = string.Empty;

        public ToolButton()
        {
            ForeColor = Color.DimGray;
            BackColor = Color.Transparent;
            this.AutoToolTip = false;
        }

        /// <summary>
        /// Custom tooltip text property - stores text separately to prevent built-in tooltip
        /// </summary>
        public new string ToolTipText
        {
            get { return _customToolTipText; }
            set
            {
                _customToolTipText = value;
                // Keep base ToolTipText empty to prevent built-in tooltip
                base.ToolTipText = string.Empty;
            }
        }

        private static ToolTip SharedToolTip
        {
            get
            {
                if (_sharedToolTip == null)
                {
                    _sharedToolTip = new ToolTip();
                    _sharedToolTip.InitialDelay = 500;
                    _sharedToolTip.ReshowDelay = 100;
                }
                return _sharedToolTip;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ShowCustomToolTip();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HideCustomToolTip();
        }

        private void ShowCustomToolTip()
        {
            if (string.IsNullOrEmpty(_customToolTipText) || Parent == null || _tooltipShown)
                return;

            var ownerForm = Parent.FindForm();
            if (ownerForm == null)
                return;

            // Calculate tooltip height based on text content
            int tipHeight = CalculateTooltipHeight(_customToolTipText);

            // Position tooltip just above the toolbar
            var toolbarScreenPos = Parent.PointToScreen(new Point(0, 0));
            var buttonScreenPos = Parent.PointToScreen(new Point(Bounds.X, 0));

            // X position: align with button, Y position: above toolbar by tooltip height
            var tipPos = new Point(buttonScreenPos.X, toolbarScreenPos.Y - tipHeight - 2);

            _tooltipShown = true;
            SharedToolTip.Show(_customToolTipText, ownerForm, ownerForm.PointToClient(tipPos));
        }

        private int CalculateTooltipHeight(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 24;

            // Use TextRenderer to measure actual text size
            using (var font = SystemFonts.StatusFont ?? new Font("Segoe UI", 9f))
            {
                var size = TextRenderer.MeasureText(text, font);
                // Add padding for tooltip border and margins
                return size.Height + 8;
            }
        }

        private void HideCustomToolTip()
        {
            if (_tooltipShown && Parent != null)
            {
                var ownerForm = Parent.FindForm();
                if (ownerForm != null)
                {
                    SharedToolTip.Hide(ownerForm);
                }
                _tooltipShown = false;
            }
        }

        private Color _BorderColor = Color.FromArgb(36, 143, 108);
        /// <summary>
        /// 底线颜色
        /// </summary>
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }

        private int _WdithAdjust = 0;
        /// <summary>
        /// 底线宽度调整
        /// </summary>
        public int WdithAdjust
        {
            get { return _WdithAdjust; }
            set { _WdithAdjust = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Clear background
            if (Parent != null)
            {
                using (var brush = new SolidBrush(Parent.BackColor))
                {
                    g.FillRectangle(brush, 0, 0, Width, Height);
                }
            }

            // Draw underline based on state
            if (Checked)
            {
                // Checked state - colored underline
                using (var pen = new Pen(_BorderColor, 2))
                {
                    g.DrawLine(pen, 3, Height - 1, Width - 3 - _WdithAdjust, Height - 1);
                }
            }
            else if (Selected || Pressed)
            {
                // Hover/pressed state - white underline
                using (var pen = new Pen(Color.White, 2))
                {
                    g.DrawLine(pen, 3, Height - 1, Width - 3 - _WdithAdjust, Height - 1);
                }
            }

            // Draw text
            DrawButtonText(g);
        }

        private void DrawButtonText(Graphics g)
        {
            Size s = g.MeasureString(Text, Font).ToSize();
            int x = (Width - s.Width) / 2;
            int y = (Height - s.Height) / 2;

            if (!Enabled)
            {
                g.DrawString(Text, Font, Brushes.DimGray, x, y);
                return;
            }

            bool isHover = Selected || Pressed;

            if (isHover)
            {
                // Hover state - bold with border color
                using (var boldFont = new Font(Font, FontStyle.Bold))
                using (var brush = new SolidBrush(BorderColor))
                {
                    g.DrawString(Text, boldFont, brush, x, y);
                }
            }
            else if (Checked)
            {
                // Checked but not hover
                using (var brush = new SolidBrush(Theme.tempToolButtonFc))
                {
                    g.DrawString(Text, Font, brush, x, y);
                }
            }
            else
            {
                // Normal state
                Color c = Theme.GetTranColor(Theme.tempToolButtonFc, -50);
                using (var brush = new SolidBrush(c))
                {
                    g.DrawString(Text, Font, brush, x, y);
                }
            }
        }
    }
}
