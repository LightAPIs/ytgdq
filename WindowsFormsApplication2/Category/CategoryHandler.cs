using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2.Category
{
    public class CategoryHandler
    {
        /// <summary>
        /// 判定字符串是否为纯英文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEnglishStr(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            //? 通过查看字符串转换为 UFT-8 编码后长度是否一致判定
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return bytes.Length == str.Length;
        }

        public static Glob.CategoryValue GetCategoryValue(string str)
        {
            return IsEnglishStr(str) ? Glob.CategoryValue.English : Glob.CategoryValue.Chinese;
        }

        public static string GetCategoryText(Glob.CategoryValue val)
        {
            switch (val)
            {
                case Glob.CategoryValue.English:
                    return "英文";
                case Glob.CategoryValue.Chinese:
                    return "中文";
                case Glob.CategoryValue.Unknow:
                default:
                    return "未知";
            }
        }

        public static bool IsEn(Glob.CategoryValue val)
        {
            return val == Glob.CategoryValue.English;
        }
    }
}
