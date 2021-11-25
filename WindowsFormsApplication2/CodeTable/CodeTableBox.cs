using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using WindowsFormsApplication2.Storage;

namespace WindowsFormsApplication2.CodeTable
{
    public partial class CodeTableBox : Form
    {
        /// <summary>
        /// 所有码表信息
        /// </summary>
        private StorageDataSet.CodeTableInfoDataTable tableData;

        private readonly Regex ValidChar = new Regex(@"^[a-z0-9;',./]+$");

        private readonly Regex InvalidChar = new Regex(@"\s|[a-zA-Z0-9!！`~@#$￥%^…&*()（）\-_—=+[\]{}'‘’""“”\\、\|·;；:：,，.。<>《》?？/]");

        public CodeTableBox()
        {
            InitializeComponent();
        }

        private void CodeTableBox_Load(object sender, EventArgs e)
        {
            ReadData();

            long usedId = Glob.CodeHistory.GetUsedId();
            if (usedId > 0)
            {
                this.UsedIdLabel.Text = usedId.ToString();
            }
            this.SearchResultLabel.Text = "";
        }

        private void ReadData()
        {
            this.tableData = Glob.CodeHistory.GetCodeTableInfo();
            this.listView1.Items.Clear();
            foreach (var dataRow in this.tableData)
            {
                listView1.Items.Add(new ListViewItem(new string[] { dataRow["id"].ToString(), dataRow["name"].ToString(), dataRow["count"].ToString(), dataRow["create_time"].ToString() }));
            }
        }

        #region 按钮
        private void AddButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "文本文件|*.txt|所有文件|*.*",
                FilterIndex = 0,
                Title = "打开码表文件",
                CheckFileExists = true,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
                {
                    string readTxt = sr.ReadToEnd();
                    sr.Close();
                    string[] readLines = Array.ConvertAll(readTxt.Split('\n'), s => s.Trim());
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    int maxLen = 1;
                    int[] lenType = new int[10];
                    foreach (string line in readLines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            //! 码表采用多多格式
                            string[] l = Array.ConvertAll(line.Split('\t'), s => s.Trim());
                            if (l.Length == 2 && !InvalidChar.IsMatch(l[0]) && ValidChar.IsMatch(l[1]) && l[0].Length <= 10)
                            {
                                int wordLen = l[0].Length;
                                if (map.ContainsKey(l[0]))
                                {
                                    if (l[1].Length < map[l[0]].Length)
                                    { //? 取编码少的
                                        map[l[0]] = l[1];
                                    }
                                }
                                else
                                {
                                    map[l[0]] = l[1];
                                    if (maxLen < 10 && wordLen > maxLen)
                                    {
                                        maxLen = wordLen;
                                    }

                                    if (lenType[wordLen - 1] != 1)
                                    {
                                        lenType[wordLen - 1] = 1;
                                    }
                                }
                            }
                        }
                    }

                    if (map.Count > 0)
                    {
                        string txtName = Path.GetFileNameWithoutExtension(filePath);
                        long seq = Glob.CodeHistory.GetSeq();
                        seq++;
                        Glob.CodeHistory.CreateCodeTable(seq.ToString(), map);
                        Glob.CodeHistory.UpdateSeq(seq);
                        Glob.CodeHistory.InsertCodeTableInfo(txtName, map.Count, DateTime.Now.ToString("s"), seq.ToString(), maxLen, string.Join("|", lenType));
                        ReadData();
                    }

                    readLines = null;
                    map.Clear();
                    map = null;
                }
            }
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                long id = long.Parse(listView1.SelectedItems[0].SubItems[0].Text);
                StorageDataSet.CodeTableInfoRow sd = StorageDataSet.GetCodeTableInfoRowFromId(this.tableData, id);
                if (sd != null)
                {
                    switch (MessageBox.Show("确认删除ID为" + id.ToString() + "的码表吗？", "删除询问", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes:
                            if (id == Glob.CodeHistory.GetUsedId())
                            {
                                CancelDefault();
                            }
                            Glob.CodeHistory.DropCodeTable(id, sd["table_index"].ToString());
                            ReadData();
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
            }
        }

        private void CancelTableButton_Click(object sender, EventArgs e)
        {
            CancelDefault();
        }

        private void CancelDefault()
        {
            if (Glob.CodeHistory.GetUsedId() > 0)
            {
                Glob.SingleWordDic.Clear();
                Glob.SingleCodeDic.Clear();
                Glob.AllWordDic.Clear();
                Glob.AllCodeDic.Clear();
                Glob.WordMaxLen = 1;
                Glob.WordLenType = new int[10];

                Glob.CodeHistory.UpdateCodeTableIndex(Glob.WordMaxLen, "0|0|0|0|0|0|0|0|0|0", -1, "");
                this.UsedIdLabel.Text = "无";
            }
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                long id = long.Parse(this.listView1.SelectedItems[0].SubItems[0].Text);
                if (id == Glob.CodeHistory.GetUsedId())
                {
                    return;
                }

                StorageDataSet.CodeTableInfoRow sd = StorageDataSet.GetCodeTableInfoRowFromId(this.tableData, id);
                if (sd != null)
                {
                    this.UsedIdLabel.Text = "设定中...";

                    string tableIndex = sd["table_index"].ToString();
                    DefaultCodeTableHandler(tableIndex);

                    //* 写入到数据库中
                    Glob.UsedTableIndex = sd["table_index"].ToString();
                    Glob.WordMaxLen = (int)sd["max_len"];
                    Glob.WordLenType = Array.ConvertAll(sd["len_type"].ToString().Split('|'), s => int.Parse(s));
                    Glob.CodeHistory.UpdateCodeTableIndex(Glob.WordMaxLen, sd["len_type"].ToString(), id, Glob.UsedTableIndex);
                    this.UsedIdLabel.Text = id.ToString();
                }
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string sText = this.SearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(sText))
            {
                if (!string.IsNullOrEmpty(Glob.UsedTableIndex))
                {
                    if (Glob.AllWordDic.ContainsKey(sText))
                    {
                        string bm = Glob.AllWordDic[sText];
                        int findit = 1;
                        if (Glob.AllCodeDic.ContainsKey(bm) && Glob.AllCodeDic[bm] != sText)
                        {
                            int tempIndex = 2;
                            while (Glob.AllCodeDic.ContainsKey(bm + tempIndex.ToString()) && Glob.AllCodeDic[bm + tempIndex.ToString()] != sText)
                            {
                                tempIndex++;
                            }
                            findit = tempIndex;
                        }

                        this.SearchResultLabel.Text = $"{bm} 【{findit - 1}重】";
                    }
                    else
                    {
                        this.SearchResultLabel.Text = "没有找到相应编码";
                    }
                }
                else
                {
                    this.SearchResultLabel.Text = "没有设定默认码表";
                }
            }
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.SearchButton.PerformClick();
            }
        }
        #endregion

        /// <summary>
        /// 默认码表处理器
        /// </summary>
        /// <param name="tableIndex"></param>
        public static void DefaultCodeTableHandler(string tableIndex)
        {
            StorageDataSet.CodeDataTable getCodeTable = Glob.CodeHistory.GetCodeTableFromTableName(tableIndex);
            Glob.SingleWordDic.Clear();
            Glob.SingleCodeDic.Clear();
            Glob.AllWordDic.Clear();
            Glob.AllCodeDic.Clear();

            foreach (var cdr in getCodeTable)
            {
                string rWord = cdr["word"].ToString();
                string rCode = cdr["coding"].ToString();

                if (rWord.Length == 1)
                {
                    Glob.SingleWordDic[rWord] = rCode;
                    if (Glob.SingleCodeDic.ContainsKey(rCode))
                    {
                        int tempIndex = 2;
                        while (Glob.SingleCodeDic.ContainsKey(rCode + tempIndex.ToString()))
                        {
                            tempIndex++;
                        }
                        Glob.SingleCodeDic[rCode + tempIndex.ToString()] = rWord;
                    }
                    else
                    {
                        Glob.SingleCodeDic[rCode] = rWord;
                    }
                }

                Glob.AllWordDic[rWord] = rCode;
                if (Glob.AllCodeDic.ContainsKey(rCode))
                {
                    int tempIndex = 2;
                    while (Glob.AllCodeDic.ContainsKey(rCode + tempIndex.ToString()))
                    {
                        tempIndex++;
                    }
                    Glob.AllCodeDic[rCode + tempIndex.ToString()] = rWord;
                }
                else
                {
                    Glob.AllCodeDic[rCode] = rWord;
                }
            }
        }
    }
}
