using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    /// <summary>
    /// 剪贴板操作
    /// </summary>
    public class ClipboardHandler
    {
        /// <summary>
        /// 是否可以从操作系统剪切板获得文本
        /// </summary>
        /// <returns>true 可以从操作系统剪切板获得文本；false 不可以</returns>
        public static bool CanGetText()
        {
            // Clipboard.GetDataObject may throw an exception...
            try
            {
                System.Windows.Forms.IDataObject data = System.Windows.Forms.Clipboard.GetDataObject();
                return data != null && data.GetDataPresent(System.Windows.Forms.DataFormats.Text);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 向操作系统剪切板设置文本数据
        /// </summary>
        /// <param name="strText">文本数据</param>
        /// <returns>操作是否成功</returns>
        public static bool SetTextToClipboard(string strText)
        {
            if (strText != null && strText.Length > 0)
            {
                try
                {
                    Clipboard.Clear();
                    var dataObject = new DataObject();
                    dataObject.SetData(DataFormats.UnicodeText, true, strText);
                    Clipboard.SetDataObject(dataObject, true);
                    return true;
                }
                catch
                {

                }
            }
            return false;
        }
    }
}
