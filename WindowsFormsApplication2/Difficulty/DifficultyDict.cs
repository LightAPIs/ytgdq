using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TyDll;

namespace WindowsFormsApplication2.Difficulty
{
    public class DifficultyDict
    {
        private Dictionary<string, double> ranks = new Dictionary<string, double>();

        private readonly string symbloChars = @"abcdefghizklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!！`~@#$￥%^…&*()（）-_—=+[]{}'‘’""“”\、|·;；:：,，.。<>《》?？/";

        public DifficultyDict()
        {
            for (int i = 0; i < 10; i++)
            {
                string dicStr = TyDll.GetResources.GetText("Resources.DIC." + i.ToString() + ".txt");
                for (int j = 0; j < dicStr.Length; j++)
                {
                    double ra = 0.75;
                    switch (i)
                    {
                        case 0:
                            ra = 1;
                            break;
                        case 1:
                            ra = 1.25;
                            break;
                        case 2:
                            ra = 1.5;
                            break;
                        case 3:
                            ra = 1.75;
                            break;
                        case 4:
                            ra = 2;
                            break;
                        case 5:
                            ra = 2.5;
                            break;
                        case 6:
                            ra = 3;
                            break;
                        case 7:
                            ra = 4;
                            break;
                        case 8:
                            ra = 5;
                            break;
                        case 9:
                            ra = 7;
                            break;
                    }
                    this.ranks.Add(dicStr[j].ToString(), ra);
                }
            }
        }

        /// <summary>
        /// 难度计算器
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public double Calc(string text)
        {
            double accumulator = 0;
            int count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                string nowIt = text[i].ToString();
                if (!string.IsNullOrWhiteSpace(nowIt))
                {
                    if (this.ranks.ContainsKey(nowIt))
                    {
                        accumulator += this.ranks[nowIt];
                        count++;
                    }
                    else if (!symbloChars.Contains(nowIt))
                    { //* 不统计标点符号
                        accumulator += 9;
                        count++;
                    }
                }
            }

            return count > 0 ? accumulator / count : 0;
        }

        /// <summary>
        /// 难度等级标识
        /// </summary>
        /// <param name="diff"></param>
        /// <returns></returns>
        public string DiffText(double diff)
        {
            string diffText = "";
            if (diff == 0)
            {
                diffText = "无";
            }
            else if (diff <= 2)
            {
                diffText = "简单";
            }
            else if (diff <= 3)
            {
                diffText = "一般";
            }
            else if (diff <= 4)
            {
                diffText = "困难";
            }
            else if (diff <= 5)
            {
                diffText = "超难";
            }
            else if (diff <= 7)
            {
                diffText = "极难";
            }
            else
            {
                diffText = "地狱";
            }

            return diffText + "(" + diff.ToString("0.00") + ")";
        }
    }
}
