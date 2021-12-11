namespace WindowsFormsApplication2
{
    partial class SendTextStatic
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
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblNowIni = new System.Windows.Forms.Label();
            this.btnChangePreCout = new System.Windows.Forms.Button();
            this.btnSendTime = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnFixNowTitle = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.CycleCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoNumberButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblNowTime = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.tbxTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTextSources = new System.Windows.Forms.Label();
            this.lblTextStyle = new System.Windows.Forms.Label();
            this.lblSendCounted = new System.Windows.Forms.Label();
            this.lblSendPCounted = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblLeastCount = new System.Windows.Forms.Label();
            this.tbxNowStartCount = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblMarkCount = new System.Windows.Forms.Label();
            this.tbxSendC = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbxSendTime = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.AutoNumberTextBox = new System.Windows.Forms.TextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.gbstatic = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.gbstatic.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.ForeColor = System.Drawing.Color.DarkRed;
            this.btnStop.Location = new System.Drawing.Point(153, 350);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止发文";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 350);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存为配置";
            this.toolTip1.SetToolTip(this.btnSave, "将当前的发文状态保存为配置");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // lblNowIni
            // 
            this.lblNowIni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNowIni.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblNowIni.Location = new System.Drawing.Point(153, 1);
            this.lblNowIni.Name = "lblNowIni";
            this.lblNowIni.Size = new System.Drawing.Size(80, 19);
            this.lblNowIni.TabIndex = 1;
            this.lblNowIni.Text = "无";
            this.lblNowIni.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblNowIni, "当前配置序列，如果保存则会覆盖");
            // 
            // btnChangePreCout
            // 
            this.btnChangePreCout.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnChangePreCout.Location = new System.Drawing.Point(202, 255);
            this.btnChangePreCout.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.btnChangePreCout.Name = "btnChangePreCout";
            this.btnChangePreCout.Size = new System.Drawing.Size(24, 22);
            this.btnChangePreCout.TabIndex = 30;
            this.btnChangePreCout.Text = "修";
            this.toolTip1.SetToolTip(this.btnChangePreCout, "修改当前的段号");
            this.btnChangePreCout.UseVisualStyleBackColor = true;
            this.btnChangePreCout.Click += new System.EventHandler(this.btnChangePreCout_Click);
            // 
            // btnSendTime
            // 
            this.btnSendTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSendTime.Enabled = false;
            this.btnSendTime.Location = new System.Drawing.Point(202, 186);
            this.btnSendTime.Margin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.btnSendTime.Name = "btnSendTime";
            this.btnSendTime.Size = new System.Drawing.Size(24, 21);
            this.btnSendTime.TabIndex = 24;
            this.btnSendTime.Text = "修";
            this.toolTip1.SetToolTip(this.btnSendTime, "修改当前周期，修改后重新计时");
            this.btnSendTime.UseVisualStyleBackColor = true;
            this.btnSendTime.Click += new System.EventHandler(this.btnSendTime_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.ForeColor = System.Drawing.Color.Green;
            this.label8.Location = new System.Drawing.Point(3, 164);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 20);
            this.label8.TabIndex = 8;
            this.label8.Text = "字数|标记";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label8, "每段字数|起始标记");
            // 
            // btnFixNowTitle
            // 
            this.btnFixNowTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnFixNowTitle.Location = new System.Drawing.Point(202, 2);
            this.btnFixNowTitle.Margin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.btnFixNowTitle.Name = "btnFixNowTitle";
            this.btnFixNowTitle.Size = new System.Drawing.Size(24, 21);
            this.btnFixNowTitle.TabIndex = 18;
            this.btnFixNowTitle.Text = "修";
            this.toolTip1.SetToolTip(this.btnFixNowTitle, "修改当前文章标题");
            this.btnFixNowTitle.UseVisualStyleBackColor = true;
            this.btnFixNowTitle.Click += new System.EventHandler(this.btnFixNowTitle_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 233);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 20);
            this.label11.TabIndex = 25;
            this.label11.Text = "数值|指令";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label11, "下一段条件的数值|不满足条件的指令");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 210);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 20);
            this.label10.TabIndex = 22;
            this.label10.Text = "条件|关系";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label10, "自动下一段的条件|关系运算符");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 187);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "周期|计数";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label9, "周期大小|周期计数");
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(157, 326);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 16);
            this.checkBox2.TabIndex = 33;
            this.checkBox2.Text = "设定条件";
            this.toolTip1.SetToolTip(this.checkBox2, "设定自动下一段条件");
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // CycleCheckBox
            // 
            this.CycleCheckBox.AutoSize = true;
            this.CycleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CycleCheckBox.Location = new System.Drawing.Point(7, 326);
            this.CycleCheckBox.Name = "CycleCheckBox";
            this.CycleCheckBox.Size = new System.Drawing.Size(69, 16);
            this.CycleCheckBox.TabIndex = 34;
            this.CycleCheckBox.Text = "周期发文";
            this.CycleCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.CycleCheckBox, "勾选后会直接发下一段\r\n同时开始启用周期发文");
            this.CycleCheckBox.UseVisualStyleBackColor = true;
            this.CycleCheckBox.CheckedChanged += new System.EventHandler(this.CycleCheckBox_CheckedChanged);
            // 
            // AutoNumberButton
            // 
            this.AutoNumberButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.AutoNumberButton.Enabled = false;
            this.AutoNumberButton.Location = new System.Drawing.Point(202, 232);
            this.AutoNumberButton.Margin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.AutoNumberButton.Name = "AutoNumberButton";
            this.AutoNumberButton.Size = new System.Drawing.Size(24, 21);
            this.AutoNumberButton.TabIndex = 37;
            this.AutoNumberButton.Text = "修";
            this.toolTip1.SetToolTip(this.AutoNumberButton, "修改自动条件的数值");
            this.AutoNumberButton.UseVisualStyleBackColor = true;
            this.AutoNumberButton.Click += new System.EventHandler(this.AutoNumberButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox1.Location = new System.Drawing.Point(82, 326);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 16);
            this.checkBox1.TabIndex = 31;
            this.checkBox1.Text = "自动发文";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBox1, "自动发下一段，可选设定条件");
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblNowTime
            // 
            this.lblNowTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNowTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNowTime.Font = new System.Drawing.Font("宋体", 9F);
            this.lblNowTime.Location = new System.Drawing.Point(66, 1);
            this.lblNowTime.Margin = new System.Windows.Forms.Padding(2, 1, 2, 0);
            this.lblNowTime.Name = "lblNowTime";
            this.lblNowTime.Size = new System.Drawing.Size(60, 22);
            this.lblNowTime.TabIndex = 26;
            this.lblNowTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.lblNowIni);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 21);
            this.panel1.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(0, 1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(153, 19);
            this.label13.TabIndex = 0;
            this.label13.Text = "当前发文配置ID：";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.btnFixNowTitle, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbxTitle, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.lblTextSources, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblTextStyle, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblSendCounted, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblSendPCounted, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalCount, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblLeastCount, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.tbxNowStartCount, 1, 11);
            this.tableLayoutPanel2.Controls.Add(this.btnChangePreCout, 2, 11);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.btnSendTime, 2, 8);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.AutoNumberButton, 2, 10);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 12;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(229, 279);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label12.Location = new System.Drawing.Point(3, 256);
            this.label12.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 23);
            this.label12.TabIndex = 28;
            this.label12.Text = "当前段号";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbxTitle
            // 
            this.tbxTitle.BackColor = System.Drawing.Color.DarkGray;
            this.tbxTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxTitle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxTitle.Location = new System.Drawing.Point(75, 2);
            this.tbxTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.tbxTitle.Name = "tbxTitle";
            this.tbxTitle.ReadOnly = true;
            this.tbxTitle.Size = new System.Drawing.Size(122, 21);
            this.tbxTitle.TabIndex = 12;
            this.tbxTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "文章标题";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "文章来源";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 49);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "文章类型";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "已发字数";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 95);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "已发段数";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 118);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "总字数";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 141);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "剩余字数";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTextSources
            // 
            this.lblTextSources.AutoSize = true;
            this.lblTextSources.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTextSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTextSources.Font = new System.Drawing.Font("宋体", 9F);
            this.lblTextSources.Location = new System.Drawing.Point(75, 26);
            this.lblTextSources.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.lblTextSources.Name = "lblTextSources";
            this.lblTextSources.Size = new System.Drawing.Size(122, 18);
            this.lblTextSources.TabIndex = 10;
            this.lblTextSources.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTextStyle
            // 
            this.lblTextStyle.AutoSize = true;
            this.lblTextStyle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTextStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTextStyle.Font = new System.Drawing.Font("宋体", 9F);
            this.lblTextStyle.Location = new System.Drawing.Point(75, 49);
            this.lblTextStyle.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.lblTextStyle.Name = "lblTextStyle";
            this.lblTextStyle.Size = new System.Drawing.Size(122, 18);
            this.lblTextStyle.TabIndex = 12;
            this.lblTextStyle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSendCounted
            // 
            this.lblSendCounted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSendCounted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSendCounted.Font = new System.Drawing.Font("宋体", 9F);
            this.lblSendCounted.Location = new System.Drawing.Point(75, 72);
            this.lblSendCounted.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.lblSendCounted.Name = "lblSendCounted";
            this.lblSendCounted.Size = new System.Drawing.Size(122, 18);
            this.lblSendCounted.TabIndex = 13;
            this.lblSendCounted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSendPCounted
            // 
            this.lblSendPCounted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSendPCounted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSendPCounted.Font = new System.Drawing.Font("宋体", 9F);
            this.lblSendPCounted.Location = new System.Drawing.Point(75, 95);
            this.lblSendPCounted.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.lblSendPCounted.Name = "lblSendPCounted";
            this.lblSendPCounted.Size = new System.Drawing.Size(122, 18);
            this.lblSendPCounted.TabIndex = 14;
            this.lblSendPCounted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalCount.Font = new System.Drawing.Font("宋体", 9F);
            this.lblTotalCount.Location = new System.Drawing.Point(75, 118);
            this.lblTotalCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(122, 18);
            this.lblTotalCount.TabIndex = 15;
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLeastCount
            // 
            this.lblLeastCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLeastCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLeastCount.Font = new System.Drawing.Font("宋体", 9F);
            this.lblLeastCount.Location = new System.Drawing.Point(75, 141);
            this.lblLeastCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.lblLeastCount.Name = "lblLeastCount";
            this.lblLeastCount.Size = new System.Drawing.Size(122, 18);
            this.lblLeastCount.TabIndex = 16;
            this.lblLeastCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbxNowStartCount
            // 
            this.tbxNowStartCount.BackColor = System.Drawing.Color.DarkGray;
            this.tbxNowStartCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxNowStartCount.Font = new System.Drawing.Font("宋体", 9F);
            this.tbxNowStartCount.Location = new System.Drawing.Point(75, 256);
            this.tbxNowStartCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tbxNowStartCount.Name = "tbxNowStartCount";
            this.tbxNowStartCount.ReadOnly = true;
            this.tbxNowStartCount.Size = new System.Drawing.Size(122, 21);
            this.tbxNowStartCount.TabIndex = 29;
            this.tbxNowStartCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxNowStartCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxNowStartCount_KeyPress);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.lblMarkCount, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbxSendC, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(72, 161);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(128, 23);
            this.tableLayoutPanel3.TabIndex = 34;
            // 
            // lblMarkCount
            // 
            this.lblMarkCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMarkCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarkCount.Location = new System.Drawing.Point(66, 1);
            this.lblMarkCount.Margin = new System.Windows.Forms.Padding(2, 1, 2, 0);
            this.lblMarkCount.Name = "lblMarkCount";
            this.lblMarkCount.Size = new System.Drawing.Size(60, 22);
            this.lblMarkCount.TabIndex = 31;
            this.lblMarkCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMarkCount.TextChanged += new System.EventHandler(this.lblMarkCount_TextChanged);
            // 
            // tbxSendC
            // 
            this.tbxSendC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxSendC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxSendC.Location = new System.Drawing.Point(2, 1);
            this.tbxSendC.Margin = new System.Windows.Forms.Padding(2, 1, 1, 0);
            this.tbxSendC.Name = "tbxSendC";
            this.tbxSendC.Size = new System.Drawing.Size(61, 22);
            this.tbxSendC.TabIndex = 32;
            this.tbxSendC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tbxSendTime, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNowTime, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(72, 184);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(128, 23);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // tbxSendTime
            // 
            this.tbxSendTime.BackColor = System.Drawing.Color.DarkGray;
            this.tbxSendTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxSendTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxSendTime.Font = new System.Drawing.Font("宋体", 9F);
            this.tbxSendTime.Location = new System.Drawing.Point(2, 1);
            this.tbxSendTime.Margin = new System.Windows.Forms.Padding(2, 1, 1, 0);
            this.tbxSendTime.Name = "tbxSendTime";
            this.tbxSendTime.ReadOnly = true;
            this.tbxSendTime.Size = new System.Drawing.Size(61, 21);
            this.tbxSendTime.TabIndex = 23;
            this.tbxSendTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxSendTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxSendTime_KeyPress);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.comboBox1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBox2, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(72, 207);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(128, 23);
            this.tableLayoutPanel4.TabIndex = 35;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "速度",
            "击键",
            "码长",
            "键准",
            "回改",
            "错字",
            "回改率",
            "打词",
            "打词率",
            "效率",
            "评级"});
            this.comboBox1.Location = new System.Drawing.Point(2, 2);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2, 2, 1, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(61, 20);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "大于",
            "大于等于",
            "小于",
            "不于等于"});
            this.comboBox2.Location = new System.Drawing.Point(66, 2);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2, 2, 1, 0);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(61, 20);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.AutoNumberTextBox, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.comboBox3, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(72, 230);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(128, 23);
            this.tableLayoutPanel5.TabIndex = 36;
            // 
            // AutoNumberTextBox
            // 
            this.AutoNumberTextBox.BackColor = System.Drawing.Color.DarkGray;
            this.AutoNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AutoNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoNumberTextBox.Location = new System.Drawing.Point(2, 1);
            this.AutoNumberTextBox.Margin = new System.Windows.Forms.Padding(2, 1, 1, 0);
            this.AutoNumberTextBox.Name = "AutoNumberTextBox";
            this.AutoNumberTextBox.ReadOnly = true;
            this.AutoNumberTextBox.Size = new System.Drawing.Size(61, 21);
            this.AutoNumberTextBox.TabIndex = 0;
            this.AutoNumberTextBox.Text = "0";
            this.AutoNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AutoNumberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AutoNumberTextBox_KeyPress);
            // 
            // comboBox3
            // 
            this.comboBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Enabled = false;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "无操作",
            "重打",
            "乱序"});
            this.comboBox3.Location = new System.Drawing.Point(66, 2);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(2, 2, 1, 0);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(61, 20);
            this.comboBox3.TabIndex = 1;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // gbstatic
            // 
            this.gbstatic.Controls.Add(this.tableLayoutPanel2);
            this.gbstatic.Location = new System.Drawing.Point(0, 24);
            this.gbstatic.Name = "gbstatic";
            this.gbstatic.Size = new System.Drawing.Size(235, 299);
            this.gbstatic.TabIndex = 0;
            this.gbstatic.TabStop = false;
            this.gbstatic.Text = "当前发文";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label15.Location = new System.Drawing.Point(6, 345);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(220, 1);
            this.label15.TabIndex = 32;
            // 
            // SendTextStatic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 373);
            this.Controls.Add(this.CycleCheckBox);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbstatic);
            this.Controls.Add(this.checkBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendTextStatic";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "雨天跟打器发文状态";
            this.Load += new System.EventHandler(this.SendTextStatic_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.gbstatic.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnFixNowTitle;
        private System.Windows.Forms.TextBox tbxTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblTextSources;
        private System.Windows.Forms.Label lblTextStyle;
        public System.Windows.Forms.Label lblSendCounted;
        public System.Windows.Forms.Label lblSendPCounted;
        private System.Windows.Forms.Label lblTotalCount;
        public System.Windows.Forms.Label lblLeastCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxSendTime;
        private System.Windows.Forms.Button btnSendTime;
        public System.Windows.Forms.Label lblNowTime;
        public System.Windows.Forms.TextBox tbxNowStartCount;
        private System.Windows.Forms.Button btnChangePreCout;
        private System.Windows.Forms.GroupBox gbstatic;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label lblMarkCount;
        public System.Windows.Forms.Label tbxSendC;
        public System.Windows.Forms.Label lblNowIni;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox CycleCheckBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox AutoNumberTextBox;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button AutoNumberButton;
    }
}