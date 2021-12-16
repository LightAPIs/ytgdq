namespace WindowsFormsApplication2.发文重写
{
    partial class ContentEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ContentRichTextBox = new System.Windows.Forms.RichTextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.FillPunctuationButton = new System.Windows.Forms.Button();
            this.RemoveSpaceButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ContentRichTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OKButton);
            this.splitContainer1.Panel2.Controls.Add(this.FillPunctuationButton);
            this.splitContainer1.Panel2.Controls.Add(this.RemoveSpaceButton);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 407;
            this.splitContainer1.TabIndex = 0;
            // 
            // ContentRichTextBox
            // 
            this.ContentRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.ContentRichTextBox.Name = "ContentRichTextBox";
            this.ContentRichTextBox.Size = new System.Drawing.Size(800, 407);
            this.ContentRichTextBox.TabIndex = 0;
            this.ContentRichTextBox.Text = "";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(713, 9);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.TabStop = false;
            this.OKButton.Text = "确定";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // FillPunctuationButton
            // 
            this.FillPunctuationButton.Location = new System.Drawing.Point(93, 9);
            this.FillPunctuationButton.Name = "FillPunctuationButton";
            this.FillPunctuationButton.Size = new System.Drawing.Size(75, 23);
            this.FillPunctuationButton.TabIndex = 2;
            this.FillPunctuationButton.TabStop = false;
            this.FillPunctuationButton.Text = "标点填充";
            this.toolTip1.SetToolTip(this.FillPunctuationButton, "用标点填充内容中的空格及空行");
            this.FillPunctuationButton.UseVisualStyleBackColor = true;
            this.FillPunctuationButton.Click += new System.EventHandler(this.FillPunctuationButton_Click);
            // 
            // RemoveSpaceButton
            // 
            this.RemoveSpaceButton.Location = new System.Drawing.Point(12, 9);
            this.RemoveSpaceButton.Name = "RemoveSpaceButton";
            this.RemoveSpaceButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveSpaceButton.TabIndex = 2;
            this.RemoveSpaceButton.TabStop = false;
            this.RemoveSpaceButton.Text = "去除空格";
            this.toolTip1.SetToolTip(this.RemoveSpaceButton, "移除内容中的空格和换行");
            this.RemoveSpaceButton.UseVisualStyleBackColor = true;
            this.RemoveSpaceButton.Click += new System.EventHandler(this.RemoveSpaceButton_Click);
            // 
            // ContentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContentEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "内容编辑器";
            this.Load += new System.EventHandler(this.ContentEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox ContentRichTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button FillPunctuationButton;
        private System.Windows.Forms.Button RemoveSpaceButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}