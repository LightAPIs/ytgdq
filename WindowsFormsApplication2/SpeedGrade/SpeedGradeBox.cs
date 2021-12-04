using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2.SpeedGrade
{
    public partial class SpeedGradeBox : Form
    {
        public SpeedGradeBox()
        {
            InitializeComponent();
        }

        private void SpeedGradeBox_Load(object sender, EventArgs e)
        {
            if (Glob.SpeedGradeCount > 0)
            {
                this.GradeLabel.Text = Glob.SpeedGrade.ToString("0.00");
                if (Glob.SpeedGrade >= 720)
                {
                    this.GradeLabel.ForeColor = Color.FromArgb(255, 112, 67);
                    this.GradeTextLabel.ForeColor = Color.FromArgb(255, 112, 67);
                    this.GradeTextLabel.Text = "举世无双";
                }
                else if (Glob.SpeedGrade >= 600)
                {
                    this.GradeLabel.ForeColor = Color.FromArgb(255, 167, 38);
                    this.GradeTextLabel.ForeColor = Color.FromArgb(255, 167, 38);
                    if (Glob.SpeedGrade >= 660)
                    {
                        this.GradeTextLabel.Text = "超凡入圣";
                    }
                    else
                    {
                        this.GradeTextLabel.Text = "出神入化";
                    }
                }
                else if (Glob.SpeedGrade >= 480)
                {
                    this.GradeLabel.ForeColor = Color.FromArgb(255, 68, 38);
                    this.GradeTextLabel.ForeColor = Color.FromArgb(255, 68, 38);
                    if (Glob.SpeedGrade >= 540)
                    {
                        this.GradeTextLabel.Text = "鹤立鸡群";
                    }
                    else
                    {
                        this.GradeTextLabel.Text = "超群绝伦";
                    }
                }
                else if (Glob.SpeedGrade >= 360)
                {
                    this.GradeLabel.ForeColor = Color.FromArgb(255, 175, 228);
                    this.GradeTextLabel.ForeColor = Color.FromArgb(255, 175, 228);
                    if (Glob.SpeedGrade >= 420)
                    {
                        this.GradeTextLabel.Text = "炉火纯青";
                    }
                    else
                    {
                        this.GradeTextLabel.Text = "出类拔萃";
                    }
                }
                else if (Glob.SpeedGrade >= 240)
                {
                    this.GradeLabel.ForeColor = Color.FromArgb(72, 178, 51);
                    this.GradeTextLabel.ForeColor = Color.FromArgb(72, 178, 51);
                    if (Glob.SpeedGrade >= 300)
                    {
                        this.GradeTextLabel.Text = "渐入佳境";
                    }
                    else
                    {
                        this.GradeTextLabel.Text = "小有所成";
                    }
                }
                else if (Glob.SpeedGrade >= 120)
                {
                    this.GradeLabel.ForeColor = Color.FromArgb(0, 0, 0);
                    this.GradeTextLabel.ForeColor = Color.FromArgb(0, 0, 0);
                    if (Glob.SpeedGrade >= 180)
                    {
                        this.GradeTextLabel.Text = "竿头日进";
                    }
                    else
                    {
                        this.GradeTextLabel.Text = "力争上游";
                    }
                }
                else
                {
                    this.GradeLabel.ForeColor = Color.FromArgb(128, 128, 128);
                    this.GradeTextLabel.ForeColor = Color.FromArgb(128, 128, 128);
                    if (Glob.SpeedGrade >= 60)
                    {
                        this.GradeTextLabel.Text = "逆水行舟";
                    }
                    else
                    {
                        this.GradeTextLabel.Text = "听天由命";
                    }
                }

                this.SegCountLabel.Text = Glob.SpeedGradeCount.ToString();
                this.AvgSpeedLabel.Text = (Glob.SpeedGradeSpeed / Glob.SpeedGradeCount).ToString("0.00");
                this.AvgDiffLabel.Text = (Glob.SpeedGradeDiff / Glob.SpeedGradeCount).ToString("0.00");
            }
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
