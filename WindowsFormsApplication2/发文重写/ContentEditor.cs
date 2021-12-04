using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication2.发文重写
{
    public partial class ContentEditor : Form
    {
        private string content;
        public string OutValue;
        public ContentEditor(string _content)
        {
            this.content = _content;
            InitializeComponent();
        }

        private void ContentEditor_Load(object sender, EventArgs e)
        {
            this.ContentRichTextBox.Text = content;
        }

        #region 按钮
        private void RemoveSpaceButton_Click(object sender, EventArgs e)
        {
            string text = this.ContentRichTextBox.Text;
            this.ContentRichTextBox.Text = TickBlock(text, "");
        }

        private void FillPunctuationButton_Click(object sender, EventArgs e)
        {
            string text = this.ContentRichTextBox.Text;
            this.ContentRichTextBox.Text = TickBlock(text, "，");
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            string text = this.ContentRichTextBox.Text.Trim();
            if (text != "")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.OutValue = this.ContentRichTextBox.Text.Trim();
        }

        /// <summary>
        /// 替换字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private string TickBlock(string text, string target)
        {
            return Regex.Replace(text, @"[\s\u3000]", target);
        }
    }
}
