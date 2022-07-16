using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2.Condition
{
    public class ConditionItems
    {
        public List<ConditionItem> Items { get; set; }

        public ConditionItems() {
            Items = new List<ConditionItem>();
        }

        private int GetKeyIndexFromItem(ConditionItem item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                ConditionItem item2 = Items[i];
                if (item2.Key == item.Key)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetKeyIndexFromKey(NewSendText.AutoKeyValue key)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Key == key)
                {
                    return i;
                }
            }
            return -1;
        }

        #region 公有方法
        public void Parse(string str)
        {
            string[] strArr = str.Split('|');
            foreach (string strItem in strArr)
            {
                ConditionItem item = new ConditionItem();
                item.Parse(strItem);
                int index = GetKeyIndexFromItem(item);
                if (index == -1)
                {
                    Items.Add(item);
                }
                else
                {
                    Items[index] = item;
                }
            }

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void AddItem(ConditionItem item)
        {
            int index = GetKeyIndexFromItem(item);
            if (index == -1)
            {
                Items.Add(item);
            }
            else
            {
                Items[index] = item;
            }
        }

        public void AddItem(NewSendText.AutoKeyValue key, NewSendText.AutoOperatorValue op, double val)
        {
            ConditionItem item = new ConditionItem(key, op, val);
            int index = GetKeyIndexFromKey(key);
            if (index == -1)
            {
                Items.Add(item);
            }
            else
            {
                Items[index] = item;
            }
        }

        public bool RemoveItem(ConditionItem item)
        {
            int index = GetKeyIndexFromItem(item);
            if (index >= 0)
            {
                Items.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveItem(NewSendText.AutoKeyValue key)
        {
            int index = GetKeyIndexFromKey(key);
            if (index >= 0)
            {
                Items.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            Items.Clear();
        }
        #endregion
    }
}
