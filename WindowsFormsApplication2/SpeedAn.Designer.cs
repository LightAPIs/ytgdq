namespace WindowsFormsApplication2
{
    partial class SpeedAn
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
            this.SpeedAnGet = new System.Windows.Forms.PictureBox();
            this.GetText = new System.Windows.Forms.Button();
            this.GetPic = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedAnGet)).BeginInit();
            this.SuspendLayout();
            // 
            // SpeedAnGet
            // 
            this.SpeedAnGet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeedAnGet.BackColor = System.Drawing.Color.WhiteSmoke;
            this.SpeedAnGet.Location = new System.Drawing.Point(0, 0);
            this.SpeedAnGet.Name = "SpeedAnGet";
            this.SpeedAnGet.Size = new System.Drawing.Size(403, 210);
            this.SpeedAnGet.TabIndex = 0;
            this.SpeedAnGet.TabStop = false;
            // 
            // GetText
            // 
            this.GetText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GetText.Location = new System.Drawing.Point(3, 214);
            this.GetText.Name = "GetText";
            this.GetText.Size = new System.Drawing.Size(72, 23);
            this.GetText.TabIndex = 1;
            this.GetText.Text = "复制文本";
            this.GetText.UseVisualStyleBackColor = true;
            this.GetText.Click += new System.EventHandler(this.GetText_Click);
            // 
            // GetPic
            // 
            this.GetPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GetPic.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.GetPic.ForeColor = System.Drawing.Color.DarkRed;
            this.GetPic.Location = new System.Drawing.Point(81, 214);
            this.GetPic.Name = "GetPic";
            this.GetPic.Size = new System.Drawing.Size(72, 23);
            this.GetPic.TabIndex = 3;
            this.GetPic.Text = "复制截图";
            this.GetPic.UseVisualStyleBackColor = true;
            this.GetPic.Click += new System.EventHandler(this.GetPic_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(321, 214);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 23);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "关闭";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // SpeedAn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(403, 241);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.GetPic);
            this.Controls.Add(this.GetText);
            this.Controls.Add(this.SpeedAnGet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpeedAn";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "速度分析";
            this.Load += new System.EventHandler(this.SpeedAn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpeedAnGet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SpeedAnGet;
        private System.Windows.Forms.Button GetText;
        private System.Windows.Forms.Button GetPic;
        private System.Windows.Forms.Button btnExit;
    }
}