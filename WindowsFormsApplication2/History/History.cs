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
using WindowsFormsApplication2.KeyAnalysis;
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
        /// 表格操作器
        /// </summary>
        private HistoryDataGridHandler gridHandler;

        private readonly Form1 frm;

        public History(Form1 frm1)
        {
            this.frm = frm1;
            InitializeComponent();
        }

        private void History_Load(object sender, EventArgs e)
        {
            this.gridHandler = new HistoryDataGridHandler(this.dataGridView1);
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

        #region 表格右键菜单事件
        private void History_CellMoseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.gridHandler.SetMouseLocation(e);
            this.ItemToolStripTextBox.Text = this.gridHandler.MenuGetScoreTime();
        }

        private void CopyScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CopyScore(this.currentScoreData);
        }

        private void CopyPicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CopyPicScore(this.currentScoreData);
        }

        private void CopyContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.CopyContent(this.currentScoreData);
        }

        private void SpeedAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.SpeedAn(this.currentScoreData, this.frm);
        }

        private void TypeAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.TypeAn(this.currentScoreData);
        }

        private void KeyAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gridHandler.KeyAn();
        }

        private void SearchTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string articleTitle = this.gridHandler.GetArticleTitle();
            if (!string.IsNullOrEmpty(articleTitle))
            {
                this.ShowDataFromTitle(articleTitle);
            }
        }

        private void SearchSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int segmentId = this.gridHandler.GetSegmentId(this.currentScoreData);
            if (segmentId != -1)
            {
                this.ShowDataFromSegment(segmentId);
            }
        }

        private void RetypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] result = this.gridHandler.GetRetype(this.currentScoreData);
            if (result != null)
            {
                this.frm.TypeContentDirectly(result[0], result[1], result[2]);
                this.Close();
            }
        }
        #endregion

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
