using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ListTrayBarInfo;
using TyDll;
namespace WindowsFormsApplication2
{
    public class NewForm:FormBase
    {
        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public NewForm()
            :base()
        {
            
        }

        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle MaxRect
        {
            get { return new Rectangle((int)(this.Width - this.CloseRect.Width - 28 * DpiScaleFactor), (int)(-1 * DpiScaleFactor), (int)(28 * DpiScaleFactor), (int)(20 * DpiScaleFactor)); }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle MiniRect
        {
            get
            {
                int x = (int)(this.Width - this.CloseRect.Width - this.MaxRect.Width - 28 * DpiScaleFactor);
                Rectangle rect = new Rectangle(x, (int)(-1 * DpiScaleFactor), (int)(28 * DpiScaleFactor), (int)(20 * DpiScaleFactor));
                return rect;
                //return new Rectangle(this.Width - this.CloseRect.Width - 28, -1, 28, 20);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle SysBtnRect
        {
            get
            {
                if (base._sysButton == ESysButton.Normal)
                    return new Rectangle((int)(this.Width - 28 * DpiScaleFactor * 2 - 39 * DpiScaleFactor), 0, (int)(39 * DpiScaleFactor + 28 * DpiScaleFactor + 28 * DpiScaleFactor), (int)(20 * DpiScaleFactor));
                else if (base._sysButton == ESysButton.Close_Mini)
                    return new Rectangle((int)(this.Width - 28 * DpiScaleFactor - 39 * DpiScaleFactor), 0, (int)(39 * DpiScaleFactor + 28 * DpiScaleFactor), (int)(20 * DpiScaleFactor));
                else
                    return this.CloseRect;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override Rectangle CloseRect
        {
            get { return new Rectangle((int)(this.Width - 39 * DpiScaleFactor), (int)(-1 * DpiScaleFactor), (int)(39 * DpiScaleFactor), (int)(20 * DpiScaleFactor)); }
        }
        
        #endregion

        #region 方法
        /// <summary>
        /// 绘画按钮
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="mouseState">鼠标状态</param>
        /// <param name="rect">按钮区域</param>
        /// <param name="str">图片字符串</param>
        private void DrawButton(Graphics g, EMouseState mouseState, Rectangle rect, string str)
        {
            switch (mouseState)
            {
                case EMouseState.Normal:
                    g.DrawImage(GetResources.GetImage("Resources.QQ.SysButton.btn_" + str + "_normal.png"), rect);
                    break;
                case EMouseState.Move:
                case EMouseState.Up:
                    g.DrawImage(GetResources.GetImage("Resources.QQ.SysButton.btn_" + str + "_highlight.png"), rect);
                    break;
                case EMouseState.Down:
                    g.DrawImage(GetResources.GetImage("Resources.QQ.SysButton.btn_" + str + "_down.png"), rect);
                    break;
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //绘画系统控制按钮
            switch (base.SysButton)
            {
                case ESysButton.Normal:
                    this.DrawButton(g, base.MinState, this.MiniRect, "mini");
                    this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                    if (this.WindowState == FormWindowState.Maximized)
                        this.DrawButton(g, base.MaxState, this.MaxRect, "restore");
                    else
                        this.DrawButton(g, base.MaxState, this.MaxRect, "max");
                    break;
                case ESysButton.Close:
                    this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                    break;
                case ESysButton.Close_Mini:
                    this.DrawButton(g, base.MinState, this.MiniRect, "mini");
                    this.DrawButton(g, base.CloseState, this.CloseRect, "close");
                    break;
            }



            base.OnPaint(e);
        }
        #endregion

    }
}
