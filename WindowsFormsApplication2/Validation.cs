using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace WindowsFormsApplication2
{
    /// <summary>
    /// 校验类
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// 计算字符串MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string input)
        {
            if (input == null)
            {
                return "";
            }
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 校验函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string Validat(string a)
        {
            byte[] result = Encoding.Default.GetBytes(a);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string get = BitConverter.ToInt64(output, 0).ToString();
            return get.Substring(get.Length - 6, 5);
        }

        /// <summary>
        /// 精五验证
        /// </summary>
        /// <param name="speed">速度</param>
        /// <param name="pressSpeed">击键</param>
        /// <param name="perLen">码长</param>
        /// <param name="totTime">用时</param>
        /// <param name="qq"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string JingWuValidat(int speed, int pressSpeed, int perLen, int totTime, string qq, string text)
        {
            string str = "";
            str += (((speed + perLen) + pressSpeed) % 10);
            int num = 4;
            int actLen = text.Length;
            if (actLen < 5) //总字数
            {
                num = 0;
            }
            int num2 = (text[num] + (speed / 100)) % 10;
            int num3 = (Convert.ToInt32((int)(qq[qq.Length - 1] - '0')) + actLen) % 10;
            return ((str + num2.ToString()) + num3.ToString() + string.Format("{0:000}", (((totTime % 0x3e8) * actLen) + actLen) % 0x3e5));
        }
    }
}
