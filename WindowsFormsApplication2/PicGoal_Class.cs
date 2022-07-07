using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using WindowsFormsApplication2.Category;

namespace WindowsFormsApplication2
{

    /// <summary>
    /// 图片成绩的初始类
    /// </summary>
    public class PicGoal_Class:IDisposable
    {
        /// <summary>
        /// 需要显示的内容
        /// </summary>
        public Size PicSize = new Size(280, 220);
        public Bitmap Pic_Bmp;

        /// <summary>
        /// 初始化类
        /// </summary>
        public PicGoal_Class() {
            if (Pic_Bmp == null)
            {
                Pic_Bmp = new Bitmap(PicSize.Width, PicSize.Height);
            }
        }

        
        public Bitmap GetPic(string title, string time, string costTime, double accuracyRate, int effciency, int count, int back_change, int error, int keys, int backspace, int duplicate, string segmentNum, double speed, double keystroke, double codeLen, Glob.CategoryValue cate, string version){
            //取得画布
            Graphics g = Graphics.FromImage(Pic_Bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //填充颜色
            g.Clear(Color.FromArgb(240, 240, 240));//一种中蓝色
            //Font F = new Font("宋体",12f);
            int StartH = 20;
            //外框画笔
            Pen BorderP =new Pen(Brushes.DimGray);
            //画重打颜色 绿色为新打。红色为重打
            //以空格分隔需要显示的项目
            //段
            Font L_ = new Font("Verdana", 9f);
            //if (Glob.ReTypePD)
            //{ // 重打红
            //    g.FillPie(Brushes.IndianRed, Pic_Bmp.Width - 20, 0, 20, 20, -90, 360);
            //    g.DrawString("重", L_, Brushes.White, Pic_Bmp.Width - 20 + 2, 3);
            //}
            //else 
            //{ // 新打绿
            //    g.FillPie(Brushes.ForestGreen, Pic_Bmp.Width - 20, 0, 20, 20, -90, 360);
            //    g.DrawString("新", L_, Brushes.White, Pic_Bmp.Width - 20 + 2,3);
            //}
            //成绩列表X值
            int Left = 190;
            //画段号外框
            g.DrawRectangle(BorderP,Left - 1,StartH - 1,82,22);
            g.FillRectangle(Brushes.DimGray, Left, StartH, 80, 20);//画底色
            g.DrawString("第" + segmentNum + "段",L_,Brushes.White,Left + 1,StartH + 3);
            // 画文本类型
            StartH += 30;
            g.DrawRectangle(BorderP, Left - 1, StartH - 1, 82, 22);
            g.FillRectangle(Brushes.SaddleBrown, Left, StartH, 80, 20);
            g.DrawString("类别：" + CategoryHandler.GetCategoryText(cate), L_, Brushes.White, Left + 1, StartH + 3);
            //速度
            //以80为限
            //算出理论速度
            double theoreticalSpeed = keystroke * 60 / (keys * accuracyRate / 100 / count); // 理论速度
            if (CategoryHandler.IsEn(cate))
            {
                theoreticalSpeed /= 5;
            }
            //画出当前速度
            StartH += 30;
            int width = (int)(speed * 80 / theoreticalSpeed);
            g.DrawRectangle(BorderP, Left - 1, StartH - 1, 82, 22);//外框
            g.FillRectangle(Brushes.DimGray, Left, StartH, 80, 20);//画底色
            g.FillRectangle(Brushes.DarkGreen, Left, StartH, width, 20);
            g.DrawString("速度" + speed.ToString("0.00"), L_, (width < 40) ? Brushes.Black : Brushes.White, Left + 1, StartH + 3);//画当前速度
            //画出理论速度
            StartH += 30;
            g.DrawRectangle(BorderP,Left - 1,StartH - 1,82,22);
            g.FillRectangle(Brushes.DarkRed, Left, StartH, 80, 20);
            g.DrawString("理论" + theoreticalSpeed.ToString("0.00"),L_,Brushes.White,Left + 1,StartH + 3);

            //画出击键
            StartH += 30;
            g.DrawRectangle(BorderP, Left - 1, StartH - 1, 82, 22);
            g.FillRectangle(Brushes.DarkSlateBlue, Left, StartH, 80, 20);//画底色
            g.DrawString("击键" + keystroke.ToString("0.00"), L_, Brushes.White, Left + 1, StartH + 3);

            //画出码长
            StartH += 30;
            g.DrawRectangle(BorderP, Left - 1, StartH - 1, 82, 22);
            g.FillRectangle(Brushes.DarkCyan, Left, StartH, 80, 20);//画底色
            g.DrawString("码长" + codeLen.ToString("0.00"), L_, Brushes.White, Left + 1, StartH + 3);

            
            //饼形图为键准
            int Jz_Rect = 80;
            int Jz_X = 5,Jz_Y = 55;
            g.FillPie(Brushes.LightGray, Jz_X, Jz_Y, Jz_Rect, Jz_Rect, -90, 360);
            if (accuracyRate > 0)
            {
                g.FillPie(Brushes.DarkCyan, Jz_X, Jz_Y, Jz_Rect, Jz_Rect, -90, -(360f * (float)accuracyRate / 100f));
            }
            //显示键准
            Font JzNum_Font = new Font("Verdana",12,FontStyle.Bold);
            string JzNum_Text = (accuracyRate > 0) ? (accuracyRate == 100) ? "PERFECT" : accuracyRate + "%" : "Null";
            SizeF JzNum_Size = g.MeasureString(JzNum_Text,JzNum_Font);
            g.DrawString(JzNum_Text, JzNum_Font, Brushes.White, Jz_Rect / 2 + Jz_X - JzNum_Size.Width / 2, Jz_Rect / 2 + Jz_Y - JzNum_Size.Height / 2);
            g.DrawString("键准", L_, Brushes.White, Jz_X + 25, Jz_Y + Jz_Rect - 20);
            //效率 
            int Xl_Rect = 80;//效率半径
            int Xl_StartX = 100, Xl_StartY = 55;
            g.FillPie(Brushes.LightGray, Xl_StartX, Xl_StartY, Xl_Rect, Xl_Rect, -90, 360);//底色绘画
            if (effciency <= 100)
            {
                g.FillPie(new SolidBrush(Color.FromArgb(206, 97, 0)), Xl_StartX, Xl_StartY, Xl_Rect, Xl_Rect, -90, -(int)(360 * effciency / 100));//底色绘画
            }

            if (effciency > 100) //大于一百时
            {
                double Xl_Temp = effciency - 100;
                g.FillPie(Brushes.DarkSlateBlue, Xl_StartX, Xl_StartY, Xl_Rect, Xl_Rect, -90, 360);//底色绘画
                g.FillPie(Brushes.Tomato, Xl_StartX, Xl_StartY, Xl_Rect, Xl_Rect, -90, -(int)(360 * Xl_Temp/100));
            }
            string Xl_Text = effciency + "%";
            Font Xl_Font = new Font("Verdana",12f,FontStyle.Bold);
            SizeF Xl_Size = g.MeasureString(Xl_Text,Xl_Font);
            //写效率 
            g.DrawString(Xl_Text, Xl_Font, Brushes.White, Xl_StartX + Xl_Rect / 2 - Xl_Size.Width / 2 + 5, Xl_StartY + Xl_Rect / 2 - Xl_Size.Height / 2);

            g.DrawString("效率", L_, Brushes.White, Xl_StartX + 25 , Xl_StartY + Xl_Rect - 20);
            //图例
            //键准
            //g.DrawRectangle(BorderP, Left - 1, 9, 32, 12);
            //g.FillRectangle(Brushes.DarkCyan, Left, 10, 30, 10);
            //g.DrawString("键准",L_,Brushes.Black,Left + 33,8);
            //时间标 左上角
            Font text_Font = new Font("Verdana",8);

            //左下角
            //画出字数
            Pen B_Table = new Pen(Color.Gray);
            int ZiSH = Pic_Bmp.Height - 80;
            int ZiSX = 10;
            g.DrawString("字数",text_Font,Brushes.DimGray,ZiSX + 10,ZiSH);
            string zis = TypingFormat(count);
            g.DrawString(zis,text_Font,Brushes.DimGray, 85 - g.MeasureString(zis,text_Font).Width - 3,ZiSH);
            g.DrawLine(B_Table, ZiSX + 5, ZiSH + 16, 100, ZiSH + 16);
            //画出回改
            ZiSH += 18;
            g.DrawString("回改",text_Font,Brushes.DimGray,ZiSX + 10,ZiSH);
            g.DrawString(back_change.ToString(), text_Font, (back_change > 0) ? Brushes.OrangeRed : Brushes.DimGray, 85 - g.MeasureString(back_change.ToString(), text_Font).Width - 3, ZiSH);
            g.DrawLine(B_Table, ZiSX + 5, ZiSH + 16, 100, ZiSH + 16);
            //画出错字
            ZiSH += 18;
            g.DrawString("错字", text_Font, Brushes.DimGray, ZiSX + 10, ZiSH);
            g.DrawString(error.ToString(), text_Font, (error > 0) ? Brushes.DeepPink:Brushes.DimGray, 85 - g.MeasureString(error.ToString(), text_Font).Width - 3, ZiSH);
            g.DrawLine(B_Table, ZiSX + 5, ZiSH + 16, 100, ZiSH + 16);
            //键数
            ZiSH -= 36;
            ZiSX = 85;
            g.DrawString("键数", text_Font, Brushes.DimGray, ZiSX + 10, ZiSH);
            zis = TypingFormat(keys);
            g.DrawString(zis, text_Font, Brushes.DimGray, 165 - g.MeasureString(zis, text_Font).Width - 3, ZiSH);
            g.DrawLine(B_Table, ZiSX + 80, ZiSH + 16, 100, ZiSH + 16);
            //退格  
            ZiSH += 18;
            g.DrawString("退格", text_Font, Brushes.DimGray, ZiSX + 10, ZiSH);
            g.DrawString(backspace.ToString(), text_Font, Brushes.DimGray, 165 - g.MeasureString(backspace.ToString(), text_Font).Width - 3, ZiSH);
            g.DrawLine(B_Table, ZiSX + 80, ZiSH + 16, 100, ZiSH + 16);
            //选重
            ZiSH += 18;
            g.DrawString("选重", text_Font, Brushes.DimGray, ZiSX + 10, ZiSH);
            g.DrawString(duplicate.ToString(), text_Font, Brushes.DimGray, 165 - g.MeasureString(duplicate.ToString(), text_Font).Width - 3, ZiSH);
            //g.DrawLine(B_Table, ZiSX + 80, ZiSH + 16, 100, ZiSH + 16);

            //外边框
            g.DrawRectangle(B_Table, 15, Pic_Bmp.Height - 80,151,52);
            //标题处理
            //底色
            
            string T = title.Trim().Replace(":","").Replace("：","");
            Font title_Font = new Font("Verdana",8,FontStyle.Bold);
            //判断是否为标题
            System.Text.RegularExpressions.Regex isTitle = new System.Text.RegularExpressions.Regex(@"\(\d+\)|<.+>");
            SizeF label_title = g.MeasureString("文章标题:",text_Font);
            string T_ = !isTitle.IsMatch(T) ? ((T.Length > 15) ? T.Substring(0, 15) : T) : "无标题";
            //SizeF T_SizeF = g.MeasureString(T_,title_Font);
            //g.DrawRectangle(new Pen(Brushes.Tomato), 0, 0, label_title.Width + T_SizeF.Width + 5, 30);

            g.DrawString("文章标题: ", text_Font, Brushes.DimGray, 0, 1);
            g.DrawString(T_,title_Font,Brushes.PaleVioletRed,label_title.Width,1);
            if (Glob.PicName.Length > 0)
            {
                g.DrawString("跟打用户: " + Glob.PicName, text_Font, Brushes.DimGray, 0, label_title.Height + 3);
                g.DrawString("跟打日期: " + time, text_Font, Brushes.DimGray, 0, label_title.Height * 2 + 5);
            } 
            else
            {
                g.DrawString("跟打日期: " + time, text_Font, Brushes.DimGray, 0, label_title.Height + 3);
            }

            //* 跟打用时
            Pen borderCost = new Pen(Brushes.YellowGreen);
            string costStr = "用时: " + costTime;
            SizeF costSize = g.MeasureString(costStr, L_);
            int costY = (int)(Pic_Bmp.Height - costSize.Height);
            g.DrawRectangle(borderCost, 0, costY - 1, costSize.Width + 1, costSize.Height + 1);
            g.FillRectangle(Brushes.YellowGreen, 0, costY - 1, costSize.Width + 1, costSize.Height + 1);//画底色
            g.DrawString(costStr, L_, Brushes.White, 1, costY);

            //尾标
            string text_ = Glob.Form + "(" + version + ")";
            SizeF text_Size = g.MeasureString(text_, text_Font);
            int LastFlagHeight = (int)(Pic_Bmp.Height - text_Size.Height);
            g.DrawString(text_,text_Font,Brushes.DimGray,Pic_Bmp.Width - text_Size.Width,LastFlagHeight);
            ////用户
            //int user_Rect = 100;//半径
            //g.FillPie(Brushes.YellowGreen, -user_Rect / 2, Pic_Bmp.Height - user_Rect/2, user_Rect, user_Rect, -90, 360);
            //g.DrawString("跟打者", text_Font, Brushes.DimGray, 0, LastFlagHeight - text_Size.Height);
            //string user = (Glob.PicName.Length > 0) ? Glob.PicName : (Glob.QQnumber.Length > 0) ? Glob.QQnumber : "";
            //if (user.Length > 0)
            //    g.DrawString(user, new Font("Verdana", 8,FontStyle.Bold), Brushes.DimGray, 0, LastFlagHeight);
            //else
            //{
            //    g.DrawString("未设置", text_Font, Brushes.DimGray, 0, LastFlagHeight);
            //}

            //边框
            g.DrawRectangle(new Pen(Brushes.DimGray),0,0,Pic_Bmp.Width - 1,Pic_Bmp.Height - 1);
            return Pic_Bmp;
        }

        /// <summary>
        /// 字数格式
        /// </summary>
        /// <param name="zis"></param>
        /// <returns></returns>
        private string TypingFormat(int zis) {
            if (zis > 9999)
                return Math.Round((double)zis / 10000, 1) + "万";
            else
                return zis.ToString();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            //释放当前的图片资源
            Pic_Bmp = null;
        }
    }
}
