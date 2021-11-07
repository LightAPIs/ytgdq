using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace WindowsFormsApplication2.Storage
{
    public class ScoreData : Database
    {
        public ScoreData(string _dbName) : base(_dbName) { }

        public override void Init()
        {
            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS score(score_time DATETIME PRIMARY KEY NOT NULL, segment_num INT, speed TEXT, keystroke DOUBLE, code_len DOUBLE, calc_len DOUBLE, back_change INT, backspace INT, enter INT, duplicate INT, error INT, back_rate DOUBLE, accuracy_rate DOUBLE, effciency INT, keys INT, count INT, type_words INT, words_rate DOUBLE, cost_time TEXT, segment_id INTEGER, article_title TEXT, version TEXT);";
            this.cmd.ExecuteNonQuery();
            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS advanced(score_time DATETIME PRIMARY KEY NOT NULL, curve TEXT, speed_analysis TEXT, type_analysis TEXT);";
            this.cmd.ExecuteNonQuery();
            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS segment(id INTEGER PRIMARY KEY AUTOINCREMENT, content TEXT, check_code VARCHAR(5));";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入成绩
        /// </summary>
        /// <param name="score_time"></param>
        /// <param name="segment_num"></param>
        /// <param name="speed"></param>
        /// <param name="keystroke"></param>
        /// <param name="code_len"></param>
        /// <param name="calc_len"></param>
        /// <param name="back_change"></param>
        /// <param name="backspace"></param>
        /// <param name="enter"></param>
        /// <param name="duplicate"></param>
        /// <param name="error"></param>
        /// <param name="back_rate"></param>
        /// <param name="accuracy_rate"></param>
        /// <param name="effciency"></param>
        /// <param name="keys"></param>
        /// <param name="count"></param>
        /// <param name="type_words"></param>
        /// <param name="words_rate"></param>
        /// <param name="cost_time"></param>
        /// <param name="segment_id"></param>
        /// <param name="article_title"></param>
        /// <param name="version"></param>
        public void InsertScore(string score_time, int segment_num, string speed, double keystroke, double code_len, double calc_len, int back_change, int backspace, int enter, int duplicate, int error, double back_rate, double accuracy_rate, int effciency, int keys, int count, int type_words, double words_rate, string cost_time, int segment_id, string article_title, string version)
        {
            this.cmd.CommandText = $"INSERT INTO score VALUES('{score_time}',{segment_num},'{speed}',{keystroke},{code_len},{calc_len},{back_change},{backspace},{enter},{duplicate},{error},{back_rate},{accuracy_rate},{effciency},{keys},{count},{type_words},{words_rate},'{cost_time}',{segment_id},'{this.ConvertText(article_title)}','{version}');";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入高阶统计
        /// </summary>
        /// <param name="score_time"></param>
        /// <param name="curve"></param>
        /// <param name="speed_analysis"></param>
        /// <param name="type_analysis"></param>
        public void InsertAdvanced(string score_time, string curve, string speed_analysis, string type_analysis)
        {
            this.cmd.CommandText = $"INSERT INTO advanced VALUES('{score_time}','{curve}','{speed_analysis}','{type_analysis}');";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入文段
        /// </summary>
        /// <param name="content"></param>
        /// <param name="check_code"></param>
        /// <returns></returns>
        public int InsertSegment(string content, string check_code)
        {
            string newContent = this.ConvertText(content);
            this.cmd.CommandText = $"SELECT * FROM segment WHERE check_code='{check_code}' AND content='{newContent}';";
            object readId = this.cmd.ExecuteScalar();

            if (readId == null)
            {
                this.cmd.CommandText = $"INSERT INTO segment VALUES(NULL,'{newContent}','{check_code}'); SELECT last_insert_rowid();";
                readId = this.cmd.ExecuteScalar();
            }

            return Convert.ToInt32(readId);
        }

        /// <summary>
        /// 根据日期获取成绩
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public StorageDataSet.ScoreDataTable GetScoreFromDate(DateTime date)
        {
            this.cmd.CommandText = $"SELECT * FROM score WHERE score_time LIKE '{date:d}%'";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ScoreDataTable myScore = new StorageDataSet.ScoreDataTable();
            adapter.Fill(myScore);
            return myScore;
        }

        /// <summary>
        /// 根据标题获取成绩
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public StorageDataSet.ScoreDataTable GetScoreFromTitle(string title)
        {
            this.cmd.CommandText = $"SELECT * FROM score WHERE article_title LIKE '%{this.ConvertText(title).Replace("%", "/%").Replace("_", "/_")}%'";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ScoreDataTable myScore = new StorageDataSet.ScoreDataTable();
            adapter.Fill(myScore);
            return myScore;
        }

        /// <summary>
        /// 根据文段 id 获取成绩
        /// </summary>
        /// <param name="segmentId"></param>
        /// <returns></returns>
        public StorageDataSet.ScoreDataTable GetScoreFromSegmentId(int segmentId)
        {
            this.cmd.CommandText = $"SELECT * FROM score WHERE segment_id={segmentId}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ScoreDataTable myScore = new StorageDataSet.ScoreDataTable();
            adapter.Fill(myScore);
            return myScore;
        }

        /// <summary>
        /// 根据指定时间获取高阶统计
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public StorageDataSet.AdvancedRow GetAdvancedRowFromTime(string time)
        {
            this.cmd.CommandText = $"SELECT * FROM advanced WHERE score_time='{time.Replace(" ", "T")}'";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.AdvancedDataTable myAdvanced = new StorageDataSet.AdvancedDataTable();
            adapter.Fill(myAdvanced);
            if (myAdvanced.Count > 0)
            {
                return myAdvanced[0];
            }
            return null;
        }

        /// <summary>
        /// 获取文段内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetContentFromSegmentId(int id)
        {
            this.cmd.CommandText = $"SELECT content FROM segment WHERE id={id}";
            object readContent = this.cmd.ExecuteScalar();
            return readContent == null ? "" : readContent.ToString();
        }
    }

    public class ArticleData : Database
    {
        public ArticleData(string _dbName) : base(_dbName) { }

        public override void Init()
        {
            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS article(id INTEGER PRIMARY KEY AUTOINCREMENT, content TEXT, md5 VARCHAR(32), title TEXT, type INT)";
            this.cmd.ExecuteNonQuery();
        }
    }
}
