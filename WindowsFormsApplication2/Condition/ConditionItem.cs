using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2.Condition
{
    public class ConditionItem
    {
        /// <summary>
        /// 条件类别
        /// </summary>
        public NewSendText.AutoKeyValue Key { get; set; }
        /// <summary>
        /// 关系运算符
        /// </summary>
        public NewSendText.AutoOperatorValue Operator { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public double Value { get; set; }

        public ConditionItem(NewSendText.AutoKeyValue key = NewSendText.AutoKeyValue.Speed, NewSendText.AutoOperatorValue op = NewSendText.AutoOperatorValue.DY, double value = 0)
        {
            Key = key;
            Operator = op;
            Value = value;
        }

        #region 公有方法
        public void Parse(string str)
        {
            string[] cond = str.Split(',');
            if (cond.Length >= 3)
            {
                int.TryParse(cond[0], out int keyIndex);
                this.Key = (NewSendText.AutoKeyValue)keyIndex;
                int.TryParse(cond[1], out int operatorIndex);
                this.Operator = (NewSendText.AutoOperatorValue)operatorIndex;
                double.TryParse(cond[2], out double val);
                this.Value = val;
            }
            else
            {
                this.Key = NewSendText.AutoKeyValue.Speed;
                this.Operator = NewSendText.AutoOperatorValue.DY;
                this.Value = 0;
            }
        }

        public override string ToString()
        {
            return $"{(int)Key},{(int)Operator},{Value}";
        }
        #endregion
    }
}
