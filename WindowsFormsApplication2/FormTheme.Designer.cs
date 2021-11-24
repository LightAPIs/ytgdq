namespace WindowsFormsApplication2
{
    partial class FormTheme
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
            this.lblcls = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblThemeFCShow = new System.Windows.Forms.Label();
            this.lblThemeBGShow = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPicShow = new System.Windows.Forms.Label();
            this.lblBGPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.newButton3 = new WindowsFormsApplication2.NewButton();
            this.newButton1 = new WindowsFormsApplication2.NewButton();
            this.newButton2 = new WindowsFormsApplication2.NewButton();
            this.newButton4 = new WindowsFormsApplication2.NewButton();
            this.lblSelectFCColor = new WindowsFormsApplication2.NewButton();
            this.btnOk = new WindowsFormsApplication2.NewButton();
            this.lblSelectBGColor = new WindowsFormsApplication2.NewButton();
            this.lblSelectBG = new WindowsFormsApplication2.NewButton();
            this.SwitchB1 = new WindowsFormsApplication2.SwitchButton();
            this.lblSelectPIC = new WindowsFormsApplication2.NewButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblcls
            // 
            this.lblcls.BackColor = System.Drawing.Color.Gray;
            this.lblcls.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblcls.Location = new System.Drawing.Point(319, 1);
            this.lblcls.Name = "lblcls";
            this.lblcls.Size = new System.Drawing.Size(40, 20);
            this.lblcls.TabIndex = 0;
            this.lblcls.Text = "关闭";
            this.lblcls.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblcls.Click += new System.EventHandler(this.lblcls_Click);
            this.lblcls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblcls_MouseDown);
            this.lblcls.MouseEnter += new System.EventHandler(this.lblcls_MouseEnter);
            this.lblcls.MouseLeave += new System.EventHandler(this.lblcls_MouseLeave);
            this.lblcls.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblcls_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.newButton3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.newButton1);
            this.panel1.Controls.Add(this.newButton2);
            this.panel1.Controls.Add(this.newButton4);
            this.panel1.Controls.Add(this.lblSelectFCColor);
            this.panel1.Controls.Add(this.lblThemeFCShow);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.lblSelectBGColor);
            this.panel1.Controls.Add(this.lblThemeBGShow);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblSelectBG);
            this.panel1.Controls.Add(this.lblPicShow);
            this.panel1.Controls.Add(this.SwitchB1);
            this.panel1.Controls.Add(this.lblSelectPIC);
            this.panel1.Controls.Add(this.lblBGPath);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(11, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 174);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(7, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "前景色：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(7, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "背景色：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(7, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "主题模式：";
            // 
            // lblThemeFCShow
            // 
            this.lblThemeFCShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblThemeFCShow.BackColor = System.Drawing.Color.White;
            this.lblThemeFCShow.Location = new System.Drawing.Point(80, 130);
            this.lblThemeFCShow.Name = "lblThemeFCShow";
            this.lblThemeFCShow.Size = new System.Drawing.Size(175, 16);
            this.lblThemeFCShow.TabIndex = 11;
            this.toolTip1.SetToolTip(this.lblThemeFCShow, "前景色\r\n影响程序字体颜色包含标题");
            // 
            // lblThemeBGShow
            // 
            this.lblThemeBGShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblThemeBGShow.BackColor = System.Drawing.Color.Gray;
            this.lblThemeBGShow.Location = new System.Drawing.Point(80, 100);
            this.lblThemeBGShow.Name = "lblThemeBGShow";
            this.lblThemeBGShow.Size = new System.Drawing.Size(175, 16);
            this.lblThemeBGShow.TabIndex = 7;
            this.toolTip1.SetToolTip(this.lblThemeBGShow, "背景色");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "纯色背景：";
            // 
            // lblPicShow
            // 
            this.lblPicShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPicShow.BackColor = System.Drawing.Color.Gray;
            this.lblPicShow.Location = new System.Drawing.Point(80, 70);
            this.lblPicShow.Name = "lblPicShow";
            this.lblPicShow.Size = new System.Drawing.Size(175, 16);
            this.lblPicShow.TabIndex = 4;
            // 
            // lblBGPath
            // 
            this.lblBGPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBGPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBGPath.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBGPath.ForeColor = System.Drawing.Color.Gray;
            this.lblBGPath.Location = new System.Drawing.Point(80, 40);
            this.lblBGPath.Name = "lblBGPath";
            this.lblBGPath.Size = new System.Drawing.Size(175, 18);
            this.lblBGPath.TabIndex = 1;
            this.lblBGPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "图片背景：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "主题设置";
            // 
            // newButton3
            // 
            this.newButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton3.BackColor = System.Drawing.Color.DimGray;
            this.newButton3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.newButton3.Location = new System.Drawing.Point(304, 70);
            this.newButton3.Name = "newButton3";
            this.newButton3.Size = new System.Drawing.Size(40, 16);
            this.newButton3.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.newButton3.TabIndex = 20;
            this.newButton3.Text = "默认";
            this.newButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newButton3.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newButton3.默认背景色 = System.Drawing.Color.DimGray;
            this.newButton3.Click += new System.EventHandler(this.newButton3_Click);
            // 
            // newButton1
            // 
            this.newButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton1.BackColor = System.Drawing.Color.DimGray;
            this.newButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.newButton1.Location = new System.Drawing.Point(304, 130);
            this.newButton1.Name = "newButton1";
            this.newButton1.Size = new System.Drawing.Size(40, 16);
            this.newButton1.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.newButton1.TabIndex = 16;
            this.newButton1.Text = "默认";
            this.newButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newButton1.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newButton1.默认背景色 = System.Drawing.Color.DimGray;
            this.newButton1.Click += new System.EventHandler(this.newButton1_Click);
            // 
            // newButton2
            // 
            this.newButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton2.BackColor = System.Drawing.Color.DimGray;
            this.newButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.newButton2.Location = new System.Drawing.Point(304, 100);
            this.newButton2.Name = "newButton2";
            this.newButton2.Size = new System.Drawing.Size(40, 16);
            this.newButton2.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.newButton2.TabIndex = 15;
            this.newButton2.Text = "默认";
            this.newButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newButton2.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newButton2.默认背景色 = System.Drawing.Color.DimGray;
            this.newButton2.Click += new System.EventHandler(this.newButton2_Click);
            // 
            // newButton4
            // 
            this.newButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton4.BackColor = System.Drawing.Color.DimGray;
            this.newButton4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.newButton4.Location = new System.Drawing.Point(304, 40);
            this.newButton4.Name = "newButton4";
            this.newButton4.Size = new System.Drawing.Size(40, 16);
            this.newButton4.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.newButton4.TabIndex = 13;
            this.newButton4.Text = "默认";
            this.newButton4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newButton4.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newButton4.默认背景色 = System.Drawing.Color.DimGray;
            this.newButton4.Click += new System.EventHandler(this.newButton4_Click);
            // 
            // lblSelectFCColor
            // 
            this.lblSelectFCColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectFCColor.BackColor = System.Drawing.Color.DimGray;
            this.lblSelectFCColor.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectFCColor.Location = new System.Drawing.Point(258, 130);
            this.lblSelectFCColor.Name = "lblSelectFCColor";
            this.lblSelectFCColor.Size = new System.Drawing.Size(40, 16);
            this.lblSelectFCColor.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSelectFCColor.TabIndex = 12;
            this.lblSelectFCColor.Text = "选择";
            this.lblSelectFCColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblSelectFCColor, "选择前景色");
            this.lblSelectFCColor.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(199)))), ((int)(((byte)(192)))));
            this.lblSelectFCColor.默认背景色 = System.Drawing.Color.Gray;
            this.lblSelectFCColor.Click += new System.EventHandler(this.lblSelectFCColor_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.DimGray;
            this.btnOk.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.Location = new System.Drawing.Point(290, 156);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(58, 16);
            this.btnOk.SS = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "保存";
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOk.进入背景色 = System.Drawing.Color.DarkMagenta;
            this.btnOk.默认背景色 = System.Drawing.Color.DarkGray;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblSelectBGColor
            // 
            this.lblSelectBGColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectBGColor.BackColor = System.Drawing.Color.DimGray;
            this.lblSelectBGColor.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectBGColor.Location = new System.Drawing.Point(258, 100);
            this.lblSelectBGColor.Name = "lblSelectBGColor";
            this.lblSelectBGColor.Size = new System.Drawing.Size(40, 16);
            this.lblSelectBGColor.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSelectBGColor.TabIndex = 8;
            this.lblSelectBGColor.Text = "选择";
            this.lblSelectBGColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblSelectBGColor, "选择背景色");
            this.lblSelectBGColor.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(199)))), ((int)(((byte)(192)))));
            this.lblSelectBGColor.默认背景色 = System.Drawing.Color.Gray;
            this.lblSelectBGColor.Click += new System.EventHandler(this.lblSelectColor_Click);
            // 
            // lblSelectBG
            // 
            this.lblSelectBG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectBG.BackColor = System.Drawing.Color.DimGray;
            this.lblSelectBG.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectBG.Location = new System.Drawing.Point(258, 70);
            this.lblSelectBG.Name = "lblSelectBG";
            this.lblSelectBG.Size = new System.Drawing.Size(40, 16);
            this.lblSelectBG.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSelectBG.TabIndex = 5;
            this.lblSelectBG.Text = "选择";
            this.lblSelectBG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblSelectBG, "选择纯色背景");
            this.lblSelectBG.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(199)))), ((int)(((byte)(192)))));
            this.lblSelectBG.默认背景色 = System.Drawing.Color.Gray;
            this.lblSelectBG.Click += new System.EventHandler(this.lblSelectBG_Click);
            // 
            // SwitchB1
            // 
            this.SwitchB1.BackColor = System.Drawing.Color.DimGray;
            this.SwitchB1.BG = System.Drawing.Color.DimGray;
            this.SwitchB1.Checked = true;
            this.SwitchB1.FC = System.Drawing.Color.LightSeaGreen;
            this.SwitchB1.Location = new System.Drawing.Point(80, 10);
            this.SwitchB1.Name = "SwitchB1";
            this.SwitchB1.Size = new System.Drawing.Size(60, 16);
            this.SwitchB1.TabIndex = 3;
            this.SwitchB1.ValueA = "背景";
            this.SwitchB1.ValueB = "纯色";
            this.SwitchB1.CChange += new WindowsFormsApplication2.SwitchButton.CheckedChange(this.SwitchB1_CChange);
            // 
            // lblSelectPIC
            // 
            this.lblSelectPIC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectPIC.BackColor = System.Drawing.Color.DimGray;
            this.lblSelectPIC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectPIC.Location = new System.Drawing.Point(258, 40);
            this.lblSelectPIC.Name = "lblSelectPIC";
            this.lblSelectPIC.Size = new System.Drawing.Size(40, 16);
            this.lblSelectPIC.SS = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSelectPIC.TabIndex = 2;
            this.lblSelectPIC.Text = "选择";
            this.lblSelectPIC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSelectPIC.进入背景色 = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(199)))), ((int)(((byte)(192)))));
            this.lblSelectPIC.默认背景色 = System.Drawing.Color.Gray;
            this.lblSelectPIC.Click += new System.EventHandler(this.lblSelect_Click);
            // 
            // FormTheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(372, 210);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblcls);
            this.Controls.Add(this.label2);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTheme";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormTheme";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTheme_FormClosed);
            this.Load += new System.EventHandler(this.FormTheme_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormTheme_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblcls;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBGPath;
        private System.Windows.Forms.Label label1;
        private NewButton lblSelectPIC;
        private SwitchButton SwitchB1;
        private NewButton lblSelectBG;
        private System.Windows.Forms.Label lblPicShow;
        private System.Windows.Forms.Label label2;
        private NewButton btnOk;
        private NewButton lblSelectBGColor;
        private System.Windows.Forms.Label lblThemeBGShow;
        private System.Windows.Forms.Label label3;
        private NewButton lblSelectFCColor;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblThemeFCShow;
        private NewButton newButton1;
        private NewButton newButton2;
        private NewButton newButton4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private NewButton newButton3;
    }
}