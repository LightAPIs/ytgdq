using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication2.Category;

namespace WindowsFormsApplication2
{
    public partial class SpeedAn : Form
    {
        /// <summary>
        /// 跟打日期
        /// </summary>
        private readonly string scoreTime;
        /// <summary>
        /// 当前段号
        /// </summary>
        private readonly string nowCout;
        /// <summary>
        /// 传递的数据
        /// </summary>
        private readonly string getData;
        /// <summary>
        /// 文本类别
        /// </summary>
        private readonly Glob.CategoryValue textCate;
        /// <summary>
        /// 成绩版本
        /// </summary>
        private readonly string verInstration;
        private readonly Form1 frm;
        public SpeedAn(string score_time, string now_cout, string get_data, Glob.CategoryValue cate, string ver_instration, Form1 frm1)
        {
            scoreTime = score_time;
            nowCout = now_cout;
            getData = get_data;
            textCate = cate;
            verInstration = ver_instration;
            frm = frm1;
            InitializeComponent();
        }

        private void SpeedAn_Load(object sender, EventArgs e)
        {
            if (getData != "")
            {
                string[] data = getData.Split('|');//获取各项数据
                if (data.Length == 16)
                {
                    this.Text = "第" + nowCout + "段速度分析";
                    if (Glob.PicName.Length > 0)
                    {
                        this.Text += "<" + Glob.PicName + ">";
                    }

                    //准备画布
                    Bitmap bmp = new Bitmap(this.SpeedAnGet.Width + 2, this.SpeedAnGet.Height + 20);
                    Rectangle rect = new Rectangle(1, 1, this.SpeedAnGet.Width, this.SpeedAnGet.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    //g.Clear(Color.DimGray);
                    //准备数据

                    double perfect = double.TryParse(data[0], out perfect) ? perfect : 0; // 完美值
                    double theoretical = double.TryParse(data[2], out theoretical) ? theoretical : 0; // 理论值
                    double allTotalWidth = perfect > theoretical ? perfect : theoretical; // 总长
                    //double TotalWidth2 = double.TryParse(data[1], out TotalWidth2) ? TotalWidth2 : 0;//总长2
                    //double ImpactWidth = double.TryParse(data[2], out ImpactWidth) ? ImpactWidth : 0;//实际长
                    //double Hg = double.TryParse(data[3], out Hg) ? Hg : 0;
                    //double Bg = double.TryParse(data[4], out Bg) ? Bg : 0;
                    //double St = double.TryParse(data[5], out St) ? St : 0;
                    //double Cz = double.TryParse(data[6], out Cz) ? Cz : 0;
                    //double En = double.TryParse(data[7], out En) ? En : 0;
                    //开始画
                    //矩形1 总长1
                    int BmpWidth = this.SpeedAnGet.Width - 150; //矩形终点长度
                    int X = 15, Y = 8, width = 18; //整体坐标
                    //码长理论长度
                    for (int i = 0; i < data.Length; i += 2)
                    {
                        SolidBrush SB_TotalWidth;
                        Color Colour;
                        string MC_;
                        double TotalWidth = double.TryParse(data[i], out TotalWidth) ? TotalWidth < 0 ? 0 : TotalWidth : 0;// 长度
                        string showSpeed = TotalWidth.ToString("0.00");
                        if (CategoryHandler.IsEn(this.textCate))
                        {
                            showSpeed = (TotalWidth / 5).ToString("0.00");
                        }
                        switch (i)
                        {
                            case 0:
                                Colour = Color.FromArgb(10, 166, 146);
                                MC_ = "完美理论值：" + showSpeed;
                                break;
                            case 2:
                                Colour = Color.FromArgb(7, 153, 7);
                                MC_ = "码长理论值：" + showSpeed;
                                break;
                            case 4:
                                Colour = Color.FromArgb(195, 31, 89);
                                MC_ = "跟打实际值：" + showSpeed;
                                break;
                            case 6:
                                Colour = Color.FromArgb(150, 27, 181);
                                MC_ = "回改影响值：-" + showSpeed;
                                break;
                            case 8:
                                Colour = Color.FromArgb(202, 122, 36);
                                MC_ = "退格影响值：-" + showSpeed;
                                break;
                            case 10:
                                Colour = Color.FromArgb(222, 51, 51);
                                MC_ = "停留影响值：-" + showSpeed;
                                break;
                            case 12:
                                Colour = Color.FromArgb(110, 88, 242);
                                MC_ = "错字影响值：-" + showSpeed;
                                break;
                            case 14:
                                Colour = Color.FromArgb(164, 193, 65);
                                MC_ = "回车影响值：-" + showSpeed;
                                break;
                            default:
                                Colour = Color.FromArgb(7, 153, 7);
                                MC_ = "XXX" + TotalWidth;
                                break;
                        }
                        Font F = new Font("宋体", 9f);//画字字体
                        double Width = TotalWidth * BmpWidth / allTotalWidth;
                        if (int.Parse(data[3]) > 0 && i == 5) TotalWidth = 0; //有暂停时 ，停留不计
                        if (TotalWidth.ToString("0.00") == "0.00")
                        {
                            Width = 1;
                            Colour = Color.Gray;
                            F = new Font("宋体", 9f, FontStyle.Strikeout);//画字字体
                        }
                        else
                        {
                            F = new Font("宋体", 9f);//画字字体
                        }
                        SB_TotalWidth = new SolidBrush(Colour);
                        Rectangle rect_TotalWidth = new Rectangle(X, Y, (int)Math.Floor(Width), width);
                        g.FillRectangle(SB_TotalWidth, rect_TotalWidth); //画码长理论 TotalWidth
                        //线条
                        float Start = (float)X;
                        Color PLine_ = Color.LightGray;
                        int HeighP = 1;//偏移量
                        Pen PLine = new Pen(Color.Gray, 1);
                        Pen PLine2 = new Pen(Color.LightGray, 1);
                        PLine2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        if (i == 2)
                        {//第三行数据列
                            PLine_ = Color.FromArgb(61, 61, 61);
                            HeighP = 3;
                            Start = this.SpeedAnGet.Width - 120;
                            g.DrawLine(PLine2, (float)X, Y + width + HeighP, Start, Y + width + HeighP);
                            g.DrawLine(PLine, Start, Y + width + HeighP, bmp.Width - 9, Y + width + HeighP);
                        }

                        PLine = new Pen(PLine_, 1);
                        //线条终
                        if (i != 2)
                            g.DrawLine(PLine2, Start, Y + width + HeighP, bmp.Width - 9, Y + width + HeighP);
                        g.DrawString(MC_, F, SB_TotalWidth, this.SpeedAnGet.Width - 120, Y + 4);//画字
                        Y += 25;
                    }
                    /*
                    //完美值
                    Y += 30;
                    double Perfect_Width = TotalWidth2 * BmpWidth / TotalWidth;
                    Rectangle rect_PerfectWidth = new Rectangle(X, Y, (int)Perfect_Width, width);
                    SolidBrush SB_PerfectWidth = new SolidBrush(Color.FromArgb(7, 153, 7));
                    string PF_ = "完美理论值：" + TotalWidth2;
                    SizeF PF_Widht = g.MeasureString(PF_,F);
                    g.FillRectangle(SB_PerfectWidth,rect_PerfectWidth);
                    g.DrawString(PF_, F,Brushes.White,rect_TotalWidth.Width - PF_Widht.Width + 20, Y + 4);
                     */
                    //显示
                    this.SpeedAnGet.Image = bmp;
                }
                //MessageBox.Show(Data[0] + "\n" + data[0] + "\n" + data[data.Length - 1]);
            }
        }

        private string ConvertSpeed(string speedStr, bool sym = false)
        {
            if (CategoryHandler.IsEn(this.textCate))
            {
                double.TryParse(speedStr, out double speed);
                return (sym && speed > 0 ? "+" : "") + (speed / 5).ToString("0.00");
            }
            return speedStr;
        }

        /// <summary>
        /// 生成速度分析文本
        /// </summary>
        /// <returns></returns>
        private string CreateText()
        {
            if (getData != "")
            {
                string[] data = getData.Split('|');//获取各项数据
                if (data.Length == 16)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"     第{nowCout}段跟打分析：");
                    sb.AppendLine("+----------------------------------+");
                    sb.AppendLine($" 速度码长理论值：{ConvertSpeed(data[2])}");
                    sb.AppendLine($" 速度完美理论值：{ConvertSpeed(data[0])}({ConvertSpeed(data[1], true)})");
                    sb.AppendLine($" 速度跟打实际值：{ConvertSpeed(data[4])}({ConvertSpeed(data[5], true)})");
                    sb.AppendLine("+----------------------------------+");
                    sb.AppendLine($" 回改影响：-{ConvertSpeed(data[6])} 回改：{data[7]}s");
                    sb.AppendLine($" 退格影响：-{ConvertSpeed(data[8])} 退格：{data[9]}");
                    if (int.Parse(data[3]) == 0)
                    {
                        sb.AppendLine($" 停留影响：-{ConvertSpeed(data[10])} 停留：{data[11]}s");
                    }
                    sb.AppendLine($" 错字影响：-{ConvertSpeed(data[12])} 错字：{data[13]}");
                    sb.AppendLine($" 回车影响：-{ConvertSpeed(data[14])} 回车：{data[15]}");
                    sb.AppendLine("+----------------------------------+");
                    return sb.ToString();
                }
            }
            return "无";
        }


        //复制文本
        private void GetText_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(CreateText());
        }

        //截图类
        private void GetPic_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(GetPicMethod());
        }

        private Bitmap GetPicMethod()
        {
            Clipboard.Clear();
            Bitmap bmp = new Bitmap(this.SpeedAnGet.Width + 2, this.SpeedAnGet.Height + 40);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.DimGray);
            var rect = new Rectangle(1, 21, this.SpeedAnGet.Width, this.SpeedAnGet.Height);//定义矩形
            this.SpeedAnGet.DrawToBitmap(bmp, rect);
            Font F = new Font("宋体", 9f);
            string s = Glob.Form + "(" + verInstration.Trim() + ")";
            string ca = "文本类别：" + CategoryHandler.GetCategoryText(this.textCate);
            SizeF sF = g.MeasureString(s, F);
            SizeF caF = g.MeasureString(ca, F);
            g.DrawString(s, F, Brushes.White, this.SpeedAnGet.Width - sF.Width + 2, bmp.Height - 15);
            g.DrawString(ca, F, Brushes.White, this.SpeedAnGet.Width - caF.Width + 2, 4);
            g.DrawString(this.Text, F, Brushes.White, 3, 4);
            g.DrawString(scoreTime, F, Brushes.LightGray, 2, bmp.Height - 15);
            return bmp;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
