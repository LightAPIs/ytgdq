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
        /// 主要背景色
        /// </summary>
        public static Color ThemeColorBG = Color.FromArgb(56, 68, 73);
        /// <summary>
        /// 主要前景色
        /// - 字体颜色
        /// </summary>
        public static Color ThemeColorFC = Color.White;

        /// <summary>
        /// 次要背景色
        /// </summary>
        public static Color SecondBG = Color.FromArgb(150, 150, 150);

        /// <summary>
        /// 次要前景色
        /// </summary>
        public static Color SecondFC = Color.Black;

        /// <summary>
        /// 对照区正确字的背景色
        /// </summary>
        public static Color RightBGColor = Color.Gray;
        /// <summary>
        /// 对照区错误字的背景色
        /// </summary>
        public static Color FalseBGColor = Color.FromArgb(255, 106, 106);

        /// <summary>
        /// 对照区背景色
        /// </summary>
        public static Color R1Back = Color.FromArgb(244, 247, 252);
        /// <summary>
        /// 跟打区背景色
        /// </summary>
        public static Color R2Back = Color.FromArgb(244, 247, 252);

        /// <summary>
        /// 对照区文字颜色
        /// </summary>
        public static Color R1Color = Color.Black;
        /// <summary>
        /// 跟打区文字颜色
        /// </summary>
        public static Color R2Color = Color.Black;

        /// <summary>
        /// 跟打结束后回改标记颜色
        /// </summary>
        public static Color BackChangeColor = Color.GreenYellow;
        /// <summary>
        /// 跟打结束后用时最多的标记背景色
        /// </summary>
        public static Color TimeLongColor = Color.YellowGreen;

        /// <summary>
        /// 词组标记 0 重颜色
        /// </summary>
        public static Color Words0Color = Color.Blue;
        /// <summary>
        /// 词组标记 1 重颜色
        /// </summary>
        public static Color Words1Color = Color.Red;
        /// <summary>
        /// 词组标记 2 重颜色
        /// </summary>
        public static Color Words2Color = Color.Purple;
        /// <summary>
        /// 词组标记 3 重颜色
        /// </summary>
        public static Color Words3Color = Color.DeepPink;

        /// <summary>
        /// 测速点标记颜色
        /// </summary>
        public static Color TestMarkColor = Color.LightGray;

        /// <summary>
        /// 对照区字体及大小
        /// </summary>
        public static Font Font_1;
        /// <summary>
        /// 跟打区字体及大小
        /// </summary>
        public static Font Font_2;

        /// <summary>
        /// 底部工具栏临时前景色
        /// - 用于实现主题预览功能
        /// </summary>
        public static Color tempToolButtonFc = Color.White;
    }

}
