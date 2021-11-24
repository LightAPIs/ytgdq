using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace WindowsFormsApplication2
{
    public class Theme
    {
        /// <summary>
        /// 是否应用背景图片
        /// </summary>
        public static bool IsBackBmp = false;
        /// <summary>
        /// 图片背景路径
        /// </summary>
        public static string ThemeBackBmp = "";

        /// <summary>
        /// 纯色背景
        /// </summary>
        public static Color ThemeBG = Color.FromArgb(56, 68, 73);

        /// <summary>
        /// 主题背景色
        /// </summary>
        public static Color ThemeColorBG = Color.FromArgb(56, 68, 73);
        /// <summary>
        /// 主题前景色
        /// - 字体颜色
        /// </summary>
        public static Color ThemeColorFC = Color.White;
    }

}
