using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication2.Storage;
using WindowsFormsApplication2.KeyAnalysis;
using Newtonsoft.Json;

namespace WindowsFormsApplication2.History
{
    public class HistoryDataGridHandler
    {
        /// <summary>
        /// 当前表格
        /// </summary>
        private readonly DataGridView historyDataGridView;

        /// <summary>
        /// 鼠标当前操作行
        /// </summary>
        private DataGridViewCellMouseEventArgs mouseLocation;

        public HistoryDataGridHandler(DataGridView data_grid_view)
        {
            this.historyDataGridView = data_grid_view;
        }

        public string MenuGetScoreTime()
        {
            if (this.mouseLocation.RowIndex >= 0)
            {
                DataGridViewRow curRow = this.historyDataGridView.Rows[this.mouseLocation.RowIndex];
                if (curRow != null)
                {
                    return curRow.Cells[1].Value.ToString();
                }
            }
            return "";
        }

        public void SetMouseLocation(DataGridViewCellMouseEventArgs e)
        {
            this.mouseLocation = e;
        }

        #region 表格右键菜单操作
        /// <summary>
        /// 复制成绩
        /// </summary>
        public void CopyScore(StorageDataSet.ScoreDataTable curData)
        {
            DataGridViewRow curRow = this.historyDataGridView.Rows[this.mouseLocation.RowIndex];
            StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(curData, curRow.Cells[1].Value.ToString());
            if (curRow != null && sd != null)
            {
                string goal = "第" + curRow.Cells[2].Value + "段";
                for (int i = 3; i < this.historyDataGridView.Columns.Count - 1; i++)
                {
                    goal += " " + this.historyDataGridView.Columns[i].Name + curRow.Cells[i].Value;
                }
                goal += " 校验:" + Validation.Validat(goal);
                goal += " v" + Glob.Ver + "(" + sd["version"].ToString() + ") [复制成绩]";
                ClipboardHandler.SetTextToClipboard(goal);
            }
        }

        /// <summary>
        /// 复制图片成绩
        /// </summary>
        /// <param name="curData"></param>
        public void CopyPicScore(StorageDataSet.ScoreDataTable curData)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(curData, scoreTime);
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

        /// <summary>
        /// 复制文段标题
        /// </summary>
        public void CopyTitle()
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                if (this.mouseLocation.RowIndex >= 0)
                {
                    DataGridViewRow curRow = this.historyDataGridView.Rows[this.mouseLocation.RowIndex];
                    if (curRow != null)
                    {
                        ClipboardHandler.SetTextToClipboard(curRow.Cells[20].Value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 复制文段内容
        /// </summary>
        /// <param name="curData"></param>
        public void CopyContent(StorageDataSet.ScoreDataTable curData)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(curData, scoreTime);
                if (sd != null)
                {
                    ClipboardHandler.SetTextToClipboard(Glob.ScoreHistory.GetContentFromSegmentId((long)sd["segment_id"]));
                }
            }
        }

        /// <summary>
        /// 速度分析
        /// </summary>
        /// <param name="curData"></param>
        /// <param name="frm"></param>
        public void SpeedAn(StorageDataSet.ScoreDataTable curData, Form1 frm)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                string adv = Glob.ScoreHistory.GetAdvancedDataFromTime(scoreTime, "speed_analysis");
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(curData, scoreTime);
                if (!string.IsNullOrEmpty(adv) && sd != null)
                {
                    string sn = sd["segment_num"].ToString();
                    string vi = sd["version"].ToString();

                    SpeedAn sa = new SpeedAn(scoreTime, sn, adv, vi, frm);
                    sa.ShowDialog();
                }
                else
                {
                    MessageBox.Show("没有找到高阶统计数据！");
                }
            }
        }

        /// <summary>
        /// 跟打报告
        /// </summary>
        /// <param name="curData"></param>
        public void TypeAn(StorageDataSet.ScoreDataTable curData)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                string adv = Glob.ScoreHistory.GetAdvancedDataFromTime(scoreTime, "type_analysis");
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(curData, scoreTime);
                if (!string.IsNullOrEmpty(adv) && sd != null)
                {
                    try
                    {
                        List<TypeDate> td = JsonConvert.DeserializeObject<List<TypeDate>>(adv);
                        string content = Glob.ScoreHistory.GetContentFromSegmentId((long)sd["segment_id"]);
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

        /// <summary>
        /// 按键统计
        /// </summary>
        public void KeyAn()
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                string adv = Glob.ScoreHistory.GetAdvancedDataFromTime(scoreTime, "key_analysis");
                if (!string.IsNullOrEmpty(adv))
                {
                    int[] keysData = Array.ConvertAll(adv.Split('|'), s => int.Parse(s));
                    KeyAn kan = new KeyAn(keysData, scoreTime);
                    kan.ShowDialog();
                }
                else
                {
                    MessageBox.Show("没有找到高阶统计数据！");
                }
            }
        }

        /// <summary>
        /// 获取文段标题
        /// </summary>
        /// <returns></returns>
        public string GetArticleTitle()
        {
            DataGridViewRow curRow = this.historyDataGridView.Rows[this.mouseLocation.RowIndex];
            if (curRow != null)
            {
                return curRow.Cells["标题"].Value.ToString().Trim();
            }
            return "";
        }

        /// <summary>
        /// 获取文段 id
        /// </summary>
        /// <param name="curData"></param>
        /// <returns></returns>
        public long GetSegmentId(StorageDataSet.ScoreDataTable curData)
        {
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(curData, scoreTime);
                if (sd != null)
                {
                    return (long)sd["segment_id"];
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取重打内容
        /// </summary>
        /// <param name="curData"></param>
        /// <returns></returns>
        public string[] GetRetype(StorageDataSet.ScoreDataTable curData)
        {
            string[] result = new string[3];
            string scoreTime = this.MenuGetScoreTime();
            if (!string.IsNullOrEmpty(scoreTime))
            {
                StorageDataSet.ScoreRow sd = StorageDataSet.GetScoreRowFromTime(curData, scoreTime);
                if (sd != null)
                {
                    result[0] = Glob.ScoreHistory.GetContentFromSegmentId((long)sd["segment_id"]);
                    result[1] = sd["segment_num"].ToString();
                    result[2] = sd["article_title"].ToString();
                    return result;
                }
            }
            return null;
        }
        #endregion
    }
}
