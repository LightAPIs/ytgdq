using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApplication2.Storage;
using Newtonsoft.Json;

namespace WindowsFormsApplication2.History
{
    public partial class History : Form
    {
        /// <summary>
        /// 当前成绩数据
        /// </summary>
        private StorageDataSet.ScoreDataTable currentScoreData = new StorageDataSet.ScoreDataTable();

        /// <summary>
        /// 鼠标当前操作行
        /// </summary>
        private DataGridViewCellMouseEventArgs mouseLocation;

        private readonly Form1 frm;

        public History(Form1 frm1)
        {
            this.frm = frm1;
            InitializeComponent();
        }

        private void History_Load(object sender, EventArgs e)
        {
            ShowDataFromDate(DateTime.Now);
        }

        /// <summary>
        /// 展示数据
        /// </summary>
        private void ShowData()
        {
            int index = 0;
            int lastSegmentId = -1;
            foreach (var dataRow in this.currentScoreData)
            {
                string typeCountStr = "";
                string[] curSpeed = dataRow["speed"].ToString().Split('/');
                if ((int)dataRow["segment_id"] == lastSegmentId)
                {
                    //* 为重打数据
                    int rowCount = this.dataGridView1.Rows.Count - 1;
                    string[] oldSpeed = this.dataGridView1.Rows[rowCount].Cells[3].Value.ToString().Split('/');
                    double speedPlus = double.Parse(curSpeed[0]) - double.Parse(oldSpeed[0]);
                    double keystrokePlus = (double)dataRow["keystroke"] - double.Parse(this.dataGridView1.Rows[rowCount].Cells[4].Value.ToString());
                    double codeLenPlus = (double)dataRow["code_len"] - double.Parse(this.dataGridView1.Rows[rowCount].Cells[5].Value.ToString());
                    //! 添加对比行
                    this.dataGridView1.Rows.Add("", "", "", (speedPlus > 0 ? "+" : "") + speedPlus.ToString("0.00"), (keystrokePlus > 0 ? "+" : "") + keystrokePlus.ToString("0.00"), (codeLenPlus > 0 ? "+" : "") + codeLenPlus.ToString("0.00"));
                    rowCount++;
                    this.dataGridView1.Rows[rowCount].Height = 10;
                    this.dataGridView1.Rows[rowCount].DefaultCellStyle.Font = new Font("Arial", 6.8f);
                    this.dataGridView1.Rows[rowCount].DefaultCellStyle.ForeColor = Color.LightGray;
                    // 对比高亮
                    if (speedPlus > 0)
                    {
                        this.dataGridView1.Rows[rowCount].Cells[3].Style.ForeColor = Color.FromArgb(253, 108, 108);
                    }
                    if (keystrokePlus > 0)
                    {
                        this.dataGridView1.Rows[rowCount].Cells[4].Style.ForeColor = Color.FromArgb(255, 129, 233);
                    }
                    if (codeLenPlus < 0)
                    {
                        this.dataGridView1.Rows[rowCount].Cells[5].Style.ForeColor = Color.FromArgb(124, 222, 255);
                    }
                    for (int i = 0; i < 21; i++)
                    {
                        if (i == 3 || i == 4 || i == 5)
                        {
                            this.dataGridView1.Rows[rowCount].Cells[i].Style.BackColor = Color.FromArgb(90, 90, 90);
                        }
                    }
                }
                else
                {
                    index++;
                    typeCountStr = index.ToString();
                }

                this.dataGridView1.Rows.Add(typeCountStr, dataRow["score_time"], dataRow["segment_num"], dataRow["speed"], ((double)dataRow["keystroke"]).ToString("0.00"), ((double)dataRow["code_len"]).ToString("0.00"), ((double)dataRow["calc_len"]).ToString("0.00"), dataRow["back_change"], dataRow["backspace"], dataRow["enter"], dataRow["duplicate"], dataRow["error"], dataRow["back_rate"] + "%", dataRow["accuracy_rate"] + "%", dataRow["effciency"] + "%", dataRow["keys"], dataRow["count"], dataRow["type_words"], dataRow["words_rate"] + "%", dataRow["cost_time"], dataRow["article_title"]);
                this.dataGridView1.Rows[dataGridView1.RowCount - 1].ContextMenuStrip = this.HistoryContextMenuStrip;
                #region 单元格高亮
                CellHighlight.Speed(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[3], double.Parse(curSpeed[0]));
                CellHighlight.Keystroke(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[4], (double)dataRow["keystroke"]);
                CellHighlight.CodeLen(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[5], (double)dataRow["code_len"]);
                CellHighlight.Error(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[11], (int)dataRow["error"]);
                #endregion
                lastSegmentId = (int)dataRow["segment_id"];
            }

            this.dataGridView1.Enabled = true;
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        private void ClearData()
        {
            this.dataGridView1.Rows.Clear();
            this.currentScoreData.Clear();
            this.PreviewRichTextBox.Text = "";
            this.SpeedChart.Series[0].Points.Clear();
            this.dataGridView1.Enabled = false;
        }

        private void ShowDataFromDate(DateTime date)
        {
            this.ClearData();
            this.ResultLabel.Text = "日期：" + date.ToString("d");
            this.currentScoreData = Glob.ScoreHistory.GetScoreFromDate(date);
            this.ShowData();
        }

        private void ShowDataFromTitle(string title)
        {
            this.ClearData();
            this.ResultLabel.Text = "搜索标题：" + title;
            this.currentScoreData = Glob.ScoreHistory.GetScoreFromTitle(title);
            this.ShowData();
        }

        private void ShowDataFromSegment(int id)
        {
            this.ClearData();
            this.ResultLabel.Text = "文段：" + id.ToString();
            this.currentScoreData = Glob.ScoreHistory.GetScoreFromSegmentId(id);
            this.ShowData();
        }

        private void HistorySelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow curRow = (sender as DataGridView).CurrentRow;
            string scoreTime = curRow.Cells[1].Value.ToString();
            this.SpeedChart.Series[0].Points.Clear();
            if (string.IsNullOrEmpty(scoreTime))
            {
                //* 清空文段预览内容
                this.PreviewRichTextBox.Text = "";
            }
            else
            {
                StorageDataSet.AdvancedRow advRow = Glob.ScoreHistory.GetAdvancedRowFromTime(scoreTime);
                if (advRow != null)
                {
                    double[] advCurve = Array.ConvertAll(advRow["curve"].ToString().Split('|'), s => double.Parse(s));
                    foreach (double ce in advCurve)
                    {
                        this.SpeedChart.Series[0].Points.AddY(ce);
                    }
                    this.SpeedChart.ChartAreas[0].AxisY.Minimum = (int)(advCurve.Min() / 20) * 10;
                    this.SpeedChart.ChartAreas[0].AxisX.Interval = advCurve.Length / 5;
                }

                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(currentScoreData, scoreTime);
                if (sd != null)
                {
                    this.PreviewRichTextBox.Text = Glob.ScoreHistory.GetContentFromSegmentId((int)sd["segment_id"]);
                }
            }
        }

        private string MenuGetScoreTime()
        {
            DataGridViewRow curRow = this.dataGridView1.Rows[this.mouseLocation.RowIndex];
            if (curRow != null)
            {
                return curRow.Cells[1].Value.ToString();
            }
            return "";
        }

        private void History_CellMoseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.mouseLocation = e;
        }

        private void CopyScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow curRow = this.dataGridView1.Rows[this.mouseLocation.RowIndex];
            if (curRow != null)
            {
                string goal = "第" + curRow.Cells[2].Value + "段 ";
                for (int i = 3; i < this.dataGridView1.Columns.Count - 1; i++)
                {
                    goal += this.dataGridView1.Columns[i].Name + curRow.Cells[i].Value + " ";
                }
                goal += "校验:" + Validation.Validat(goal);
                goal += " v" + Glob.Ver + "(" + Glob.Instration + ") [复制成绩]";
                ClipboardHandler.SetTextToClipboard(goal);
            }
        }

        private void CopyPicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(currentScoreData, scoreTime);
                if (sd != null)
                {
                    using (PicGoal_Class pgc = new PicGoal_Class())
                    {
                        string[] speed = sd["speed"].ToString().Split('/');
                        Clipboard.SetImage(pgc.GetPic(sd["article_title"].ToString(), scoreTime, sd["cost_time"].ToString(), (double)sd["accuracy_rate"], (int)sd["effciency"], (int)sd["count"], (int)sd["back_change"], (int)sd["error"], (int)sd["keys"], (int)sd["backspace"], (int)sd["duplicate"], sd["segment_num"].ToString(), double.Parse(speed.Last()), (double)sd["keystroke"], (double)sd["code_len"], sd["version"].ToString()));
                        pgc.Dispose();
                    }
                }
            }
        }

        private void CopyContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(currentScoreData, scoreTime);
                if (sd != null)
                {
                    ClipboardHandler.SetTextToClipboard(Glob.ScoreHistory.GetContentFromSegmentId((int)sd["segment_id"]));
                }
            }
        }

        private void SpeedAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.AdvancedRow advRow = Glob.ScoreHistory.GetAdvancedRowFromTime(scoreTime);
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(currentScoreData, scoreTime);
                if (advRow != null && sd != null)
                {
                    string sn = sd["segment_num"].ToString();
                    string gd = advRow["speed_analysis"].ToString();
                    string vi = sd["version"].ToString();

                    SpeedAn sa = new SpeedAn(scoreTime, sn, gd, vi, this.frm);
                    sa.ShowDialog();
                }
                else
                {
                    MessageBox.Show("没有找到高阶统计数据！");
                }
            }
        }

        private void TypeAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.AdvancedRow advRow = Glob.ScoreHistory.GetAdvancedRowFromTime(scoreTime);
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(this.currentScoreData, scoreTime);
                if (advRow != null && sd != null)
                {
                    try
                    {
                        List<TypeDate> td = JsonConvert.DeserializeObject<List<TypeDate>>(advRow["type_analysis"].ToString());
                        string content = Glob.ScoreHistory.GetContentFromSegmentId((int)sd["segment_id"]);
                        string[] speed = sd["speed"].ToString().Split('/');
                        int bc = (int)sd["back_change"];
                        string vi = sd["version"].ToString();

                        WindowsFormsApplication2.跟打报告.TypeAnalysis tya = new 跟打报告.TypeAnalysis(scoreTime, td, content, speed.Last(), bc, vi);
                        tya.ShowDialog();
                    }
                    catch
                    {
                        MessageBox.Show("内部数据读取出错！");
                    }
                }
                else
                {
                    MessageBox.Show("没有找到高阶统计数据！");
                }
            }
        }

        private void SearchTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow curRow = this.dataGridView1.Rows[this.mouseLocation.RowIndex];
            if (curRow != null)
            {
                string articleTitle = curRow.Cells["标题"].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(articleTitle))
                {
                    this.ShowDataFromTitle(articleTitle);
                }
            }
        }

        private void SearchSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(this.currentScoreData, scoreTime);
                if (sd != null)
                {
                    int segmentId = (int)sd["segment_id"];
                    this.ShowDataFromSegment(segmentId);
                }
            }
        }

        private void RetypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(currentScoreData, scoreTime);
                if (sd != null)
                {
                    string content = Glob.ScoreHistory.GetContentFromSegmentId((int)sd["segment_id"]);
                    string segmentNum = sd["segment_num"].ToString();
                    string articleTitle = sd["article_title"].ToString();
                    this.frm.TypeContentDirectly(content, segmentNum, articleTitle);
                    this.Close();
                }
            }
        }

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            this.ShowDataFromDate(e.Start);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string sText = this.SearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(sText))
            {
                this.ShowDataFromTitle(sText);
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SearchButton.PerformClick();
            }
        }
    }
}
