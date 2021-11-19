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
    }
}
