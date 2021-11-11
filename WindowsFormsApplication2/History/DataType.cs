using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2.History
{
    internal class DataType
    {
        /// <summary>
        /// 当前类别
        /// </summary>
        public string Cur { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文段 id
        /// </summary>
        public int SegmentId { get; set; }
    }
}
