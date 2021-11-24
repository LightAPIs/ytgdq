using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2.KeyAnalysis
{
    public class KeyObj
    {
        /// <summary>
        /// 按键列表字典
        /// </summary>
        public static readonly Dictionary<Keys, int> KeysDic = new Dictionary<Keys, int>
        {
            {
                Keys.Oemtilde, 0
            },
            {
                Keys.D1, 1
            },
            {
                Keys.D2, 2
            },
            {
                Keys.D3, 3
            },
            {
                Keys.D4, 4
            },
            {
                Keys.D5, 5
            },
            {
                Keys.D6, 6
            },
            {
                Keys.D7, 7
            },
            {
                Keys.D8, 8
            },
            {
                Keys.D9, 9
            },
            {
                Keys.D0, 10
            },
            {
                Keys.OemMinus, 11
            },
            {
                Keys.Oemplus, 12
            },
            {
                Keys.Back, 13
            },
            {
                Keys.Q, 14
            },
            {
                Keys.W, 15
            },
            {
                Keys.E, 16
            },
            {
                Keys.R, 17
            },
            {
                Keys.T, 18
            },
            {
                Keys.Y, 19
            },
            {
                Keys.U, 20
            },
            {
                Keys.I, 21
            },
            {
                Keys.O, 22
            },
            {
                Keys.P, 23
            },
            {
                Keys.OemOpenBrackets, 24
            },
            {
                Keys.OemCloseBrackets, 25
            },
            {
                Keys.OemPipe, 26
            },
            {
                Keys.A, 27
            },
            {
                Keys.S, 28
            },
            {
                Keys.D, 29
            },
            {
                Keys.F, 30
            },
            {
                Keys.G, 31
            },
            {
                Keys.H, 32
            },
            {
                Keys.J, 33
            },
            {
                Keys.K, 34
            },
            {
                Keys.L, 35
            },
            {
                Keys.OemSemicolon, 36
            },
            {
                Keys.OemQuotes, 37
            },
            {
                Keys.Enter, 38
            },
            {
                Keys.Z, 39
            },
            {
                Keys.X, 40
            },
            {
                Keys.C, 41
            },
            {
                Keys.V, 42
            },
            {
                Keys.B, 43
            },
            {
                Keys.N, 44
            },
            {
                Keys.M, 45
            },
            {
                Keys.Oemcomma, 46
            },
            {
                Keys.OemPeriod, 47
            },
            {
                Keys.OemQuestion, 48
            },
            {
                Keys.Space, 49
            }
        };

        /// <summary>
        /// 按键字符串字典
        /// - 统计理论按键用
        /// </summary>
        public static readonly Dictionary<string, int> KeysStringDic = new Dictionary<string, int>
        {
            {
                "`", 0
            },
            {
                "~", 0
            },
            {
                "1", 1
            },
            {
                "!", 1
            },
            {
                "！", 1
            },
            {
                "2", 2
            },
            {
                "@", 2
            },
            {
                "3", 3
            },
            {
                "#", 3
            },
            {
                "4", 4
            },
            {
                "$", 4
            },
            {
                "￥", 4
            },
            {
                "5", 5
            },
            {
                "%", 5
            },
            {
                "6", 6
            },
            {
                "^", 6
            },
            {
                "…", 6
            },
            {
                "7", 7
            },
            {
                "&", 7
            },
            {
                "8", 8
            },
            {
                "*", 8
            },
            {
                "9", 9
            },
            {
                "(", 9
            },
            {
                "（", 9
            },
            {
                "0", 10
            },
            {
                ")", 10
            },
            {
                "）", 10
            },
            {
                "-", 11
            },
            {
                "_", 11
            },
            {
                "—", 11
            },
            {
                "=", 12
            },
            {
                "+", 12
            },
            {
                "\b", 13
            },
            {
                "q", 14
            },
            {
                "Q", 14
            },
            {
                "w", 15
            },
            {
                "W", 15
            },
            {
                "e", 16
            },
            {
                "E", 16
            },
            {
                "r", 17
            },
            {
                "R", 17
            },
            {
                "t", 18
            },
            {
                "T", 18
            },
            {
                "y", 19
            },
            {
                "Y", 19
            },
            {
                "u", 20
            },
            {
                "U", 20
            },
            {
                "i", 21
            },
            {
                "I", 21
            },
            {
                "o", 22
            },
            {
                "O", 22
            },
            {
                "p", 23
            },
            {
                "P", 23
            },
            {
                "[", 24
            },
            {
                "{", 24
            },
            {
                "]", 25
            },
            {
                "}", 25
            },
            {
                "\\", 26
            },
            {
                "|", 26
            },
            {
                "、", 26
            },
            {
                "·", 26
            },
            {
                "a", 27
            },
            {
                "A", 27
            },
            {
                "s", 28
            },
            {
                "S", 28
            },
            {
                "d", 29
            },
            {
                "D", 29
            },
            {
                "f", 30
            },
            {
                "F", 30
            },
            {
                "g", 31
            },
            {
                "G", 31
            },
            {
                "h", 32
            },
            {
                "H", 32
            },
            {
                "j", 33
            },
            {
                "J", 33
            },
            {
                "k", 34
            },
            {
                "K", 34
            },
            {
                "l", 35
            },
            {
                "L", 35
            },
            {
                ";", 36
            },
            {
                "；", 36
            },
            {
                ":", 36
            },
            {
                "：", 36
            },
            {
                "'", 37
            },
            {
                "\"", 37
            },
            {
                "‘", 37
            },
            {
                "’", 37
            },
            {
                "“", 37
            },
            {
                "”", 37
            },
            {
                "\n", 38
            },
            {
                "z", 39
            },
            {
                "Z", 39
            },
            {
                "x", 40
            },
            {
                "X", 40
            },
            {
                "c", 41
            },
            {
                "C", 41
            },
            {
                "v", 42
            },
            {
                "V", 42
            },
            {
                "b", 43
            },
            {
                "B", 43
            },
            {
                "n", 44
            },
            {
                "N", 44
            },
            {
                "m", 45
            },
            {
                "M", 45
            },
            {
                ",", 46
            },
            {
                "<", 46
            },
            {
                "，", 46
            },
            {
                "《", 46
            },
            {
                ".", 47
            },
            {
                ">", 47
            },
            {
                "。", 47
            },
            {
                "》", 47
            },
            {
                "/", 48
            },
            {
                "?", 48
            },
            {
                "？", 48
            },
            {
                " ", 49
            }
        };

        /// <summary>
        /// 转换某些特珠字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TransCode(string source)
        {
            string res = source;
            if (source.Length == 1 && KeysStringDic.ContainsKey(source))
            {
                switch (source)
                {
                    case "；":
                        res = ";";
                        break;
                    case "，":
                        res = ",";
                        break;
                    case "。":
                        res = ".";
                        break;
                }
            }
            return res;
        }
    }
}
