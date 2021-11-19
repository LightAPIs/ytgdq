using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2
{
    public class NewSendText
    {
        public static bool 发文状态;
        public static string 标题;
        public static string 文章全文 = "";
        public static string 发文全文 = "";
        public static string 类型;
        public static bool 是否乱序;
        //乱序全段不重复
        public static bool 乱序全段不重复 = false;

        /// <summary>
        /// 每段发送的字数
        /// </summary>
        public static int 字数;
        /// <summary>
        /// 标记从理论上始终大于 0 的
        /// </summary>
        public static int 标记;
        public static int 已发段数 = 0;

        public static bool 是否周期;
        public static int 周期;
        public static int 周期计数 = 0;

        /// <summary>
        /// 0 内置文章 1自定义文章 2剪贴板 3保存的文章 4保存的配置
        /// </summary>
        public static int 文章来源;
        public static int 已发字数 = 0;
        public static bool 是否自动 = false;

        public static string 词组发送分隔符 = "，"; //用于词组的发送分隔，默认的为 逗号 ，此项目暂时不保存
        public static string[] 词组;

        /// <summary>
        /// 发文配置保存 id
        /// </summary>
        public static long SentId = -1;

    }
}
