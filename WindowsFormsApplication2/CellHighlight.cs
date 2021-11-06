using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    /// <summary>
    /// 单元格高亮
    /// </summary>
    public class CellHighlight
    {
        /// <summary>
        /// 速度高亮
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="sp"></param>
        public static void Speed(DataGridViewCell cell, double sp)
        {
            if (sp >= 360.00)
            {
                cell.Style.ForeColor = Color.FromArgb(255, 112, 67);
            }
            else if (sp >= 300.00)
            {
                cell.Style.ForeColor = Color.FromArgb(255, 167, 38);
            }
            else if (sp >= 240.00)
            {
                cell.Style.ForeColor = Color.FromArgb(255, 68, 38);
            }
            else if (sp >= 180.00)
            {
                cell.Style.ForeColor = Color.FromArgb(255, 175, 228);
            }
            else if (sp >= 120.00)
            {
                cell.Style.ForeColor = Color.FromArgb(72, 178, 51);
            }
        }

        /// <summary>
        /// 击键高亮
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="ke"></param>
        public static void Keystroke(DataGridViewCell cell, double ke)
        {
            if (ke > 11.9)
            {
                cell.Style.ForeColor = Color.FromArgb(233, 128, 255);
            }
            else if (ke > 9.9)
            {
                cell.Style.ForeColor = Color.FromArgb(255, 59, 48);
            }
            else if (ke > 7.9)
            {
                cell.Style.ForeColor = Color.FromArgb(97, 223, 255);
            }
            else if (ke > 5.9)
            {
                cell.Style.ForeColor = Color.FromArgb(188, 255, 3);
            }
        }

        /// <summary>
        /// 码长高亮
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cl"></param>
        public static void CodeLen(DataGridViewCell cell, double cl)
        {
            if (cl <= 1.80)
            {
                cell.Style.ForeColor = Color.FromArgb(194, 255, 121);
            }
            else if (cl <= 2.00)
            {
                cell.Style.ForeColor = Color.FromArgb(58, 232, 113);
            }
            else if (cl <= 2.20)
            {
                cell.Style.ForeColor = Color.FromArgb(133, 174, 187);
            }
            else if (cl <= 2.40)
            {
                cell.Style.ForeColor = Color.FromArgb(121, 134, 203);
            }
            else if (cl <= 2.60)
            {
                cell.Style.ForeColor = Color.FromArgb(149, 117, 205);
            }
            else if (cl > 5.00)
            {
                cell.Style.ForeColor = Color.FromArgb(238, 6, 238);
            }
        }

        /// <summary>
        /// 错字高亮
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="err"></param>
        public static void Error(DataGridViewCell cell, int err)
        {
            if (err > 0)
            {
                cell.Style.ForeColor = Color.IndianRed;
            }
        }
    }
}
