using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2.KeyAnalysis
{
    public partial class KeyAn : Form
    {
        /// <summary>
        /// 按键信息
        /// </summary>
        private readonly int[] keysData;

        /// <summary>
        /// 按键列表
        /// </summary>
        private Button[] allKeyButton;

        public KeyAn(int[] keys_data)
        {
            this.keysData = keys_data;
            InitializeComponent();
        }

        private void KeyAn_Load(object sender, EventArgs e)
        {
            this.allKeyButton = new Button[50]
            {
                KeyOemtildeButton,
                KeyD1Button,
                KeyD2Button,
                KeyD3Button,
                KeyD4Button,
                KeyD5Button,
                KeyD6Button,
                KeyD7Button,
                KeyD8Button,
                KeyD9Button,
                KeyD0Button,
                KeyOemMinusButton,
                KeyOemplusButton,
                KeyBackButton,
                KeyQButton,
                KeyWButton,
                KeyEButton,
                KeyRButton,
                KeyTButton,
                KeyYButton,
                KeyUbutton,
                KeyIButton,
                KeyOButton,
                KeyPButton,
                KeyOemOpenBracketsButton,
                KeyOemCloseBracketsButton,
                KeyOemPipeButton,
                KeyAButton,
                KeySButton,
                KeyDButton,
                KeyFButton,
                KeyGButton,
                KeyHButton,
                KeyJButton,
                KeyKButton,
                KeyLButton,
                KeyOemSemicolonButton,
                KeyOemQuotesButton,
                KeyEnterButton,
                KeyZButton,
                KeyXButton,
                KeyCButton,
                KeyVButton,
                KeyBButton,
                KeyNButton,
                KeyMButton,
                KeyOemcommaButton,
                KeyOemPeriodButton,
                KeyOemQuestionButton,
                KeySpaceButton
            };

            if (this.keysData.Length == 50)
            {
                int sum = this.keysData.Sum();
                this.AllKeysLabel.Text = sum.ToString();
                for (int i = 0; i < keysData.Length; i++)
                {
                    int count = keysData[i];
                    double value = sum > 0 ? ((double)count / sum * 100) : 0;
                    Button btn = allKeyButton[i];
                    this.KeyToolTip.SetToolTip(btn, count.ToString() + "次");

                    if (value < 0.25)
                    {
                        btn.BackColor = Color.FromArgb(1, 87, 155);
                    }
                    else if (value < 0.5)
                    {
                        btn.BackColor = Color.FromArgb(2, 119, 189);
                    }
                    else if (value < 0.75)
                    {
                        btn.BackColor = Color.FromArgb(2, 136, 209);
                    }
                    else if (value < 1)
                    {
                        btn.BackColor = Color.FromArgb(3, 155, 229);
                    }
                    else if (value < 1.25)
                    {
                        btn.BackColor = Color.FromArgb(3, 169, 244);
                    }
                    else if (value < 1.5)
                    {
                        btn.BackColor = Color.FromArgb(41, 182, 246);
                    }
                    else if (value < 1.75)
                    {
                        btn.BackColor = Color.FromArgb(79, 195, 247);
                    }
                    else if (value < 2)
                    {
                        btn.BackColor = Color.FromArgb(129, 212, 250);
                    }
                    else if (value < 2.25)
                    {
                        btn.BackColor = Color.FromArgb(179, 229, 252);
                    }
                    else if (value < 2.5)
                    {
                        btn.BackColor = Color.FromArgb(225, 245, 254);
                    }
                    else if (value < 2.75)
                    {
                        btn.BackColor = Color.FromArgb(255, 255, 255);
                    }
                    else if (value < 3)
                    {
                        btn.BackColor = Color.FromArgb(255, 235, 238);
                    }
                    else if (value < 3.25)
                    {
                        btn.BackColor = Color.FromArgb(255, 205, 210);
                    }
                    else if (value < 3.5)
                    {
                        btn.BackColor = Color.FromArgb(239, 154, 154);
                    }
                    else if (value < 3.75)
                    {
                        btn.BackColor = Color.FromArgb(229, 115, 115);
                    }
                    else if (value < 4)
                    {
                        btn.BackColor = Color.FromArgb(239, 83, 80);
                    }
                    else if (value < 4.25)
                    {
                        btn.BackColor = Color.FromArgb(244, 67, 54);
                    }
                    else if (value < 4.5)
                    {
                        btn.BackColor = Color.FromArgb(229, 57, 53);
                    }
                    else if (value < 4.75)
                    {
                        btn.BackColor = Color.FromArgb(211, 47, 47);
                    }
                    else if (value < 5)
                    {
                        btn.BackColor = Color.FromArgb(198, 40, 40);
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(183, 28, 28);
                    }
                }
                int[] lrKeys = GetLRKeysCount(this.keysData);
                this.LKeysLabel.Text = lrKeys[0].ToString();
                this.RKeysLabel.Text = lrKeys[1].ToString();
                this.LSKeysLabel.Text = lrKeys[5].ToString();
                this.LZKeysLabel.Text = lrKeys[4].ToString();
                this.LWKeysLabel.Text = lrKeys[3].ToString();
                this.LXKeysLabel.Text = lrKeys[2].ToString();
                this.RSKeysLabel.Text = lrKeys[6].ToString();
                this.RZKeysLabel.Text = lrKeys[7].ToString();
                this.RWKeysLabel.Text = lrKeys[8].ToString();
                this.RXKeysLabel.Text = lrKeys[9].ToString();

                this.chart1.Series[0].Points[1].SetValueY(lrKeys[0]); // 左手
                this.chart1.Series[0].Points[0].SetValueY(lrKeys[1]); // 右手
                for (int j = 0; j < 2; j++)
                {
                    if (this.chart1.Series[0].Points[j].YValues[0] == 0)
                    {
                        this.chart1.Series[0].Points[j].IsValueShownAsLabel = false;
                        this.chart1.Series[0].Points[j].Label = String.Empty;
                    }
                }

                this.chart1.Series[1].Points[7].SetValueY(lrKeys[5]); // 左手食指
                this.chart1.Series[1].Points[6].SetValueY(lrKeys[4]); // 左手中指
                this.chart1.Series[1].Points[5].SetValueY(lrKeys[3]); // 左手无名指
                this.chart1.Series[1].Points[4].SetValueY(lrKeys[2]); // 左手小拇指
                this.chart1.Series[1].Points[0].SetValueY(lrKeys[6]); // 右手食指
                this.chart1.Series[1].Points[1].SetValueY(lrKeys[7]); // 右手中指
                this.chart1.Series[1].Points[2].SetValueY(lrKeys[8]); // 右手无名指
                this.chart1.Series[1].Points[3].SetValueY(lrKeys[9]); // 右手小拇指
                for (int k = 0; k < 8; k++)
                {
                    if (this.chart1.Series[1].Points[k].YValues[0] == 0)
                    {
                        this.chart1.Series[1].Points[k].IsValueShownAsLabel = false;
                        this.chart1.Series[1].Points[k].Label = String.Empty;
                    }
                }
            }
        }

        private void KeyAn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 统计左右手键法
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static int[] GetLRKeysCount(int[] keys)
        {
            int[] lrKeys = new int[10];
            if (keys.Length == 50)
            { //? 不统计 BackSpace、Enter、Space
                for (int i = 0; i< keys.Length; i++)
                {
                    if (keys[i] > 0)
                    {
                        //* 统计左右手
                        if (i <= 5 || (i >= 14 && i <= 18) || (i >= 27 && i <= 31) || (i >= 39 && i <= 43))
                        { // 左手
                            lrKeys[0] += keys[i];
                        }
                        else if ((i >= 6 && i <= 12) || (i >= 19 && i <= 26) || (i >= 32 && i <= 37) || (i >= 44 && i <= 48))
                        { // 右手
                            lrKeys[1] += keys[i];
                        }

                        //* 统计各手指
                        if (i <= 1 || i == 14 || i == 27 || i == 39)
                        { // 左手小拇指
                            lrKeys[2] += keys[i];
                        }
                        else if (i == 2 || i == 15 || i == 28 || i == 40)
                        { // 左手无名指
                            lrKeys[3] += keys[i];
                        }
                        else if (i == 3 || i == 16 || i == 29 || i == 41)
                        { // 左手中指
                            lrKeys[4] += keys[i];
                        }
                        else if (i == 4 || i == 5 || i == 17 || i == 18 || i == 30 || i == 31 || i == 42 || i == 43)
                        { // 左手食指
                            lrKeys[5] += keys[i];
                        }
                        else if (i == 6 || i == 7 || i == 19 || i == 20 || i == 32 || i == 33 || i == 44 || i == 45)
                        { // 右手食指
                            lrKeys[6] += keys[i];
                        }
                        else if (i == 8 || i == 21 || i == 34 || i == 46)
                        { // 右手中指
                            lrKeys[7] += keys[i];
                        }
                        else if (i == 9 || i == 22 || i == 35 || i == 47)
                        { // 右手无名指
                            lrKeys[8] += keys[i];
                        }
                        else if ((i >= 10 && i <= 12) || (i >= 23 && i <= 26) || i == 36 || i == 37 || i == 48)
                        { // 右手小拇指
                            lrKeys[9] += keys[i];
                        }
                    }
                }
            }

            return lrKeys;
        }
    }
}
