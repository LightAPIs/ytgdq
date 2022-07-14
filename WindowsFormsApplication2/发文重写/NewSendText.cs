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
        public static bool 单字乱序;
        //乱序全段不重复
        public static bool 乱序全段不重复 = false;

        /// <summary>
        /// 每段发送的字数/词数
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
        /// 文章来源
        /// </summary>
        public static ArticleSourceValue ArticleSource;
        public static int 已发字数 = 0;
        public static bool 是否自动 = false;

        public static string 词组发送分隔符 = "，"; //用于词组的发送分隔，默认的为 逗号 ，此项目暂时不保存
        public static bool 词组乱序;
        public static List<string> 词组 = new List<string>();
        public static List<string> 词组全文 = new List<string>();

        /// <summary>
        /// 文章自动剔除空格
        /// </summary>
        public static bool trim;

        /// <summary>
        /// 发文配置保存 id
        /// </summary>
        public static long SentId = -1;

        /// <summary>
        /// 条件自动
        /// </summary>
        public static bool AutoCondition = false;
        /// <summary>
        /// 条件基准
        /// </summary>
        public static AutoKeyValue AutoKey = AutoKeyValue.Speed;
        /// <summary>
        /// 条件关系运算符
        /// </summary>
        public static AutoOperatorValue AutoOperator = AutoOperatorValue.DY;
        /// <summary>
        /// 条件数值
        /// </summary>
        public static double AutoNumber = 0;
        /// <summary>
        /// 条件否操作
        /// </summary>
        public static AutoNoValue AutoNo = AutoNoValue.None;

        /// <summary>
        /// 条件基准 Enum
        /// </summary>
        public enum AutoKeyValue
        {
            Speed = 0,
            Keystroke = 1,
            CodeLen = 2,
            AccuracyRate = 3,
            BackChange = 4,
            Error = 5,
            BackRate = 6,
            TypeWords = 7,
            WordsRate = 8,
            Effciency = 9,
            Grade = 10
        }
        /// <summary>
        /// 条件关系运算符 Enum
        /// </summary>
        public enum AutoOperatorValue
        {
            DY = 0,
            DYDY = 1,
            XY = 2,
            XYDY = 3
        }
        /// <summary>
        /// 条件否操作 Enum
        /// </summary>
        public enum AutoNoValue
        {
            None = 0,
            Retype = 1,
            Disorder = 2
        }

        public enum ArticleSourceValue
        {
            Internal = 0,
            Local = 1,
            Clipboard = 2,
            Web = 3,
            Stored = 4,
            Sent = 5
        }

        public enum ContentTypeValue
        {
            Single = 0,
            Article= 1,
            Phrase = 2
        }

        public enum PhraseSeparatorType
        {
            None = -1,
            Space = 0,
            NewLine = 1,
            Tab = 2,
            Other = 3
        }
    }
}
