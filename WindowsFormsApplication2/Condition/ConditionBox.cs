using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2.Condition
{
    public partial class ConditionBox : Form
    {
        private CheckBox[] AllCheckBox;
        private ComboBox[] AllComboBox;
        private TextBox[] AllTextBox;

        public ConditionBox()
        {
            InitializeComponent();
        }

        private void AutoNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ConditionBox_Load(object sender, EventArgs e)
        {
            //* 初始化
            AllCheckBox = new CheckBox[11]
            {
                this.SpeedCheckBox,
                this.KeystrokeCheckBox,
                this.CodeLenCheckBox,
                this.AccuracyRateCheckBox,
                this.BackChangeCheckBox,
                this.ErrorCheckBox,
                this.BackRateCheckBox,
                this.TypeWordsCheckBox,
                this.WordsRateCheckBox,
                this.EffciencyCheckBox,
                this.GradeCheckBox
            };
            AllComboBox = new ComboBox[11]
            {
                this.SpeedComboBox,
                this.KeystrokeComboBox,
                this.CodeLenComboBox,
                this.AccuracyRateComboBox,
                this.BackChangeComboBox,
                this.ErrorComboBox,
                this.BackRateComboBox,
                this.TypeWordsComboBox,
                this.WordsRateComboBox,
                this.EffciencyComboBox,
                this.GradeComboBox
            };
            AllTextBox = new TextBox[11]
            {
                this.SpeedTextBox,
                this.KeystrokeTextBox,
                this.CodeLenTextBox,
                this.AccuracyRateTextBox,
                this.BackChangeTextBox,
                this.ErrorTextBox,
                this.BackRateTextBox,
                this.TypeWordsTextBox,
                this.WordsRateTextBox,
                this.EffciencyTextBox,
                this.GradeTextBox
            };

            foreach (ConditionItem item in NewSendText.ConditionValue.Items)
            {
                int keyIndex = (int)item.Key;
                this.AllCheckBox[keyIndex].Checked = true;
                this.AllComboBox[keyIndex].SelectedIndex = (int)item.Operator;
                this.AllTextBox[keyIndex].Text = item.Value.ToString();
            }
        }

        private void Box_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            int row = this.tableLayoutPanel1.GetRow(cb);
            int column = this.tableLayoutPanel1.GetColumn(cb);
            if (column == 0)
            {
                ComboBox comb = (ComboBox)this.tableLayoutPanel1.GetControlFromPosition(1, row);
                if (comb != null)
                {
                    comb.Enabled = cb.Checked;
                    if (comb.SelectedIndex == -1)
                    {
                        comb.SelectedIndex = 0;
                    }
                }
                TextBox tb = (TextBox)this.tableLayoutPanel1.GetControlFromPosition(2, row);
                if (tb != null)
                {
                    tb.Enabled = cb.Checked;
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            NewSendText.ConditionValue.Clear();
            for (int i = 0; i < AllCheckBox.Length; i++)
            {
                if (AllCheckBox[i].Checked)
                {
                    ConditionItem item = new ConditionItem();
                    item.Parse($"{i},{AllComboBox[i].SelectedIndex},{AllTextBox[i].Text}");
                    NewSendText.ConditionValue.AddItem(item);
                }
            }
            this.Close();
        }
    }
}
