namespace WindowsFormsApplication2.SpeedGrade
{
    partial class SpeedGradeBox
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.AvgDiffLabel = new System.Windows.Forms.Label();
            this.AvgSpeedLabel = new System.Windows.Forms.Label();
            this.GradeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SegCountLabel = new System.Windows.Forms.Label();
            this.GradeTextLabel = new System.Windows.Forms.Label();
            this.QuitButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GradeTextLabel);
            this.splitContainer1.Panel2.Controls.Add(this.QuitButton);
            this.splitContainer1.Size = new System.Drawing.Size(416, 211);
            this.splitContainer1.SplitterDistance = 179;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.AvgDiffLabel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.AvgSpeedLabel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.GradeLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.SegCountLabel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(412, 175);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // AvgDiffLabel
            // 
            this.AvgDiffLabel.AutoSize = true;
            this.AvgDiffLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvgDiffLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AvgDiffLabel.ForeColor = System.Drawing.Color.OliveDrab;
            this.AvgDiffLabel.Location = new System.Drawing.Point(277, 152);
            this.AvgDiffLabel.Name = "AvgDiffLabel";
            this.AvgDiffLabel.Size = new System.Drawing.Size(132, 23);
            this.AvgDiffLabel.TabIndex = 6;
            this.AvgDiffLabel.Text = "0.00";
            this.AvgDiffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AvgSpeedLabel
            // 
            this.AvgSpeedLabel.AutoSize = true;
            this.AvgSpeedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvgSpeedLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AvgSpeedLabel.ForeColor = System.Drawing.Color.MediumOrchid;
            this.AvgSpeedLabel.Location = new System.Drawing.Point(140, 152);
            this.AvgSpeedLabel.Name = "AvgSpeedLabel";
            this.AvgSpeedLabel.Size = new System.Drawing.Size(131, 23);
            this.AvgSpeedLabel.TabIndex = 5;
            this.AvgSpeedLabel.Text = "0.00";
            this.AvgSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GradeLabel
            // 
            this.GradeLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.GradeLabel, 3);
            this.GradeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GradeLabel.Font = new System.Drawing.Font("Segoe UI", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GradeLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.GradeLabel.Location = new System.Drawing.Point(3, 0);
            this.GradeLabel.Name = "GradeLabel";
            this.GradeLabel.Size = new System.Drawing.Size(406, 131);
            this.GradeLabel.TabIndex = 0;
            this.GradeLabel.Text = "0.00";
            this.GradeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.label2.Location = new System.Drawing.Point(3, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "统计段数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.MediumOrchid;
            this.label3.Location = new System.Drawing.Point(140, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "平均速度";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.OliveDrab;
            this.label4.Location = new System.Drawing.Point(277, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "平均难度";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SegCountLabel
            // 
            this.SegCountLabel.AutoSize = true;
            this.SegCountLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SegCountLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SegCountLabel.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.SegCountLabel.Location = new System.Drawing.Point(3, 152);
            this.SegCountLabel.Name = "SegCountLabel";
            this.SegCountLabel.Size = new System.Drawing.Size(131, 23);
            this.SegCountLabel.TabIndex = 4;
            this.SegCountLabel.Text = "0";
            this.SegCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GradeTextLabel
            // 
            this.GradeTextLabel.AutoSize = true;
            this.GradeTextLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GradeTextLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.GradeTextLabel.Location = new System.Drawing.Point(10, 5);
            this.GradeTextLabel.Name = "GradeTextLabel";
            this.GradeTextLabel.Padding = new System.Windows.Forms.Padding(1);
            this.GradeTextLabel.Size = new System.Drawing.Size(33, 16);
            this.GradeTextLabel.TabIndex = 1;
            this.GradeTextLabel.Text = "评级";
            // 
            // QuitButton
            // 
            this.QuitButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.QuitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.QuitButton.Location = new System.Drawing.Point(334, 1);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 0;
            this.QuitButton.Text = "关闭";
            this.QuitButton.UseVisualStyleBackColor = false;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // SpeedGradeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CancelButton = this.QuitButton;
            this.ClientSize = new System.Drawing.Size(416, 211);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpeedGradeBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "速度评级";
            this.Load += new System.EventHandler(this.SpeedGradeBox_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label AvgDiffLabel;
        private System.Windows.Forms.Label AvgSpeedLabel;
        private System.Windows.Forms.Label GradeLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SegCountLabel;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Label GradeTextLabel;
    }
}