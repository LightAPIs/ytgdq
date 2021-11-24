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
            this.cmd.CommandText = "PRAGMA foreign_keys = ON;";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS segment(id INTEGER PRIMARY KEY AUTOINCREMENT, content TEXT, check_code VARCHAR(5));";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS score(score_time DATETIME PRIMARY KEY NOT NULL, segment_num INT, speed TEXT, keystroke DOUBLE, code_len DOUBLE, calc_len DOUBLE, back_change INT, backspace INT, enter INT, duplicate INT, error INT, back_rate DOUBLE, accuracy_rate DOUBLE, effciency INT, keys INT, count INT, type_words INT, words_rate DOUBLE, cost_time TEXT, segment_id INTEGER NOT NULL, article_title TEXT, version TEXT, CONSTRAINT fk_segment_score FOREIGN KEY (segment_id) REFERENCES segment(id));";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE INDEX IF NOT EXISTS score_segment_id ON score (segment_id);";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE INDEX IF NOT EXISTS score_article_title ON score (article_title)";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE TRIGGER IF NOT EXISTS score_delete_before BEFORE DELETE ON score FOR EACH ROW BEGIN DELETE FROM advanced WHERE score_time=old.score_time; DELETE FROM calc WHERE score_time=old.score_time; END;";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE TRIGGER IF NOT EXISTS score_delete_after AFTER DELETE ON score FOR EACH ROW WHEN (SELECT COUNT(1) FROM score WHERE segment_id=old.segment_id) = 0 BEGIN DELETE FROM segment WHERE id=old.segment_id; END;";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS advanced(score_time DATETIME PRIMARY KEY NOT NULL, curve TEXT, speed_analysis TEXT, type_analysis TEXT, key_analysis TEXT, CONSTRAINT fk_score_advanced FOREIGN KEY (score_time) REFERENCES score(score_time));";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS calc(score_time DATETIME PRIMARY KEY NOT NULL, keys TEXT, CONSTRAINT fk_score_calc FOREIGN KEY (score_time) REFERENCES score(score_time));";
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
        public void InsertScore(string score_time, int segment_num, string speed, double keystroke, double code_len, double calc_len, int back_change, int backspace, int enter, int duplicate, int error, double back_rate, double accuracy_rate, int effciency, int keys, int count, int type_words, double words_rate, string cost_time, long segment_id, string article_title, string version)
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
        public void InsertAdvanced(string score_time, string curve, string speed_analysis, string type_analysis, string key_analysis)
        {
            this.cmd.CommandText = $"INSERT INTO advanced VALUES('{score_time}','{curve}','{speed_analysis}','{type_analysis}','{key_analysis}');";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入理论按键统计
        /// </summary>
        /// <param name="score_time"></param>
        /// <param name="keys"></param>
        public void InsertCalc(string score_time, string keys)
        {
            this.cmd.CommandText = $"INSERT INTO calc VALUES('{score_time}','{keys}');";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入文段
        /// </summary>
        /// <param name="content"></param>
        /// <param name="check_code"></param>
        /// <returns></returns>
        public long InsertSegment(string content, string check_code)
        {
            string newContent = this.ConvertText(content);
            this.cmd.CommandText = $"SELECT id FROM segment WHERE check_code='{check_code}' AND content='{newContent}';";
            object readId = this.cmd.ExecuteScalar();

            if (readId == null)
            {
                this.cmd.CommandText = $"INSERT INTO segment VALUES(NULL,'{newContent}','{check_code}'); SELECT last_insert_rowid();";
                readId = this.cmd.ExecuteScalar();
            }

            return Convert.ToInt64(readId);
        }

        /// <summary>
        /// 根据日期获取成绩
        /// </summary>
        /// <param name="date"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StorageDataSet.ScoreDataTable GetScoreFromDate(DateTime date, int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM score WHERE score_time LIKE '{date:d}%' LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ScoreDataTable myScore = new StorageDataSet.ScoreDataTable();
            adapter.Fill(myScore);
            return myScore;
        }

        /// <summary>
        /// 根据日期获取成绩数量
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetScoreCountFromDate(DateTime date)
        {
            this.cmd.CommandText = $"SELECT COUNT(1) FROM score WHERE score_time LIKE '{date:d}%'";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
        }

        /// <summary>
        /// 搜索包含指定标题文本的成绩
        /// </summary>
        /// <param name="title"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StorageDataSet.ScoreDataTable GetScoreFromSubTitle(string title, int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM score WHERE article_title LIKE '%{this.ConvertText(title).Replace("%", "/%").Replace("_", "/_")}%' LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ScoreDataTable myScore = new StorageDataSet.ScoreDataTable();
            adapter.Fill(myScore);
            return myScore;
        }

        /// <summary>
        /// 搜索包含指定标题文本的成绩数量
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int GetScoreCountFromSubTitle(string title)
        {
            this.cmd.CommandText = $"SELECT COUNT(1) FROM score WHERE article_title LIKE '%{this.ConvertText(title).Replace("%", "/%").Replace("_", "/_")}%'";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
        }

        /// <summary>
        /// 根据标题获取成绩
        /// </summary>
        /// <param name="title"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StorageDataSet.ScoreDataTable GetScoreFromTitle(string title, int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM score WHERE article_title='{this.ConvertText(title)}' LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ScoreDataTable myScore = new StorageDataSet.ScoreDataTable();
            adapter.Fill(myScore);
            return myScore;
        }

        /// <summary>
        /// 根据标题获取成绩数量
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int GetScoreCountFromTitle(string title)
        {
            this.cmd.CommandText = $"SELECT COUNT(1) FROM score WHERE article_title='{this.ConvertText(title)}'";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
        }

        /// <summary>
        /// 根据文段 id 获取成绩
        /// </summary>
        /// <param name="segmentId"></param>
        /// <param name="limit"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public StorageDataSet.ScoreDataTable GetScoreFromSegmentId(long segmentId, int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM score WHERE segment_id={segmentId} LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ScoreDataTable myScore = new StorageDataSet.ScoreDataTable();
            adapter.Fill(myScore);
            return myScore;
        }

        /// <summary>
        /// 根据文段 id 获取成绩数量
        /// </summary>
        /// <param name="segmentId"></param>
        /// <returns></returns>
        public int GetScoreCountFromSegmentId(long segmentId)
        {
            this.cmd.CommandText = $"SELECT COUNT(1) FROM score WHERE segment_id={segmentId}";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
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
        /// 根据时间获取指定高阶统计
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetAdvancedDataFromTime(string time, string dataType)
        {
            this.cmd.CommandText = $"SELECT {dataType} FROM advanced WHERE score_time='{time.Replace(" ", "T")}'";
            object readStr = this.cmd.ExecuteScalar();
            return readStr == null ? "" : readStr.ToString();
        }

        /// <summary>
        /// 根据时间获取理论按键数据
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetCalcDataFromTime(string time)
        {
            this.cmd.CommandText = $"SELECT keys FROM calc WHERE score_time='{time.Replace(" ", "T")}'";
            object readStr = this.cmd.ExecuteScalar();
            return readStr == null ? "" : readStr.ToString();
        }

        /// <summary>
        /// 获取文段内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetContentFromSegmentId(long id)
        {
            this.cmd.CommandText = $"SELECT content FROM segment WHERE id={id}";
            object readContent = this.cmd.ExecuteScalar();
            return readContent == null ? "" : readContent.ToString();
        }

        /// <summary>
        /// 根据指定时间删除记录
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool DeleteScoreItemByTime(string time)
        {
            this.cmd.CommandText = $"DELETE FROM score WHERE score_time='{time.Replace(" ", "T")}'";
            int rows = this.cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据 segment_id 删除记录
        /// </summary>
        /// <param name="segmentId"></param>
        public void DeleteScoreItemBySegmentId(long segmentId)
        {
            this.cmd.CommandText = $"DELETE FROM score WHERE segment_id={segmentId}";
            int count = this.cmd.ExecuteNonQuery();
            if (count > 20)
            {
                this.CleanDisk();
            }
        }

        /// <summary>
        /// 根据日期删除记录
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public void DeleteScoreItemByDate(string date)
        {
            this.cmd.CommandText = $"DELETE FROM score WHERE score_time LIKE '{date}%'";
            int count = this.cmd.ExecuteNonQuery();
            if (count > 20)
            {
                this.CleanDisk();
            }
        }

        /// <summary>
        /// 删除所有成绩数据
        /// </summary>
        public void DeleteAllScore()
        {
            this.cmd.CommandText = "DELETE FROM score;";
            this.cmd.ExecuteNonQuery();
            this.cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='segment';";
            this.cmd.ExecuteNonQuery();
            this.CleanDisk();
        }
    }

    public class ArticleData : Database
    {
        public ArticleData(string _dbName) : base(_dbName) { }

        public override void Init()
        {
            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS article(id INTEGER PRIMARY KEY AUTOINCREMENT, content TEXT, md5 VARCHAR(32), title TEXT, count INT, create_time DATETIME NOT NULL)";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入文章
        /// </summary>
        /// <param name="content"></param>
        /// <param name="md5"></param>
        /// <param name="title"></param>
        /// <param name="create_time"></param>
        public void InsertArticle(string content, string md5, string title, string create_time)
        {
            string newContent = this.ConvertText(content);
            this.cmd.CommandText = $"SELECT id FROM article WHERE md5='{md5}' AND content='{newContent}';";
            object readId = this.cmd.ExecuteScalar();

            if (readId == null)
            {
                int count = content.Length;
                this.cmd.CommandText = $"INSERT INTO article VALUES(NULL,'{newContent}','{md5}','{this.ConvertText(title)}',{count},'{create_time}');";
                this.cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StorageDataSet.ArticleDataTable GetArticle(int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM article LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ArticleDataTable myArticle = new StorageDataSet.ArticleDataTable();
            adapter.Fill(myArticle);
            return myArticle;
        }

        /// <summary>
        /// 获取文章总数量
        /// </summary>
        /// <returns></returns>
        public int GetArticleCount()
        {
            this.cmd.CommandText = "SELECT COUNT(1) FROM article";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
        }

        /// <summary>
        /// 搜索包含指定标题文本的文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StorageDataSet.ArticleDataTable GetArticleFromSubTitle(string title, int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM article WHERE title LIKE '%{this.ConvertText(title).Replace("%", "/%").Replace("_", "/_")}%' LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.ArticleDataTable myArticle = new StorageDataSet.ArticleDataTable();
            adapter.Fill(myArticle);
            return myArticle;
        }

        /// <summary>
        /// 搜索包含指定标题文本的文章数量
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int GetArticleCountFromSubTitle(string title)
        {
            this.cmd.CommandText = $"SELECT COUNT(1) FROM article WHERE title LIKE '%{this.ConvertText(title).Replace("%", "/%").Replace("_", "/_")}%'";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
        }

        /// <summary>
        /// 更新文章标题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public void UpdateArticleTitle(long id, string title)
        {
            this.cmd.CommandText = $"UPDATE article SET title='{this.ConvertText(title)}' WHERE id={id};";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新文章内容
        /// - 内部会自动更新字数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        public void UpdateArticleContent(long id, string content)
        {
            int count = content.Length;
            this.cmd.CommandText = $"UPDATE article SET content='{this.ConvertText(content)}', count={count} WHERE id={id};";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 根据 id 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteArticleItemById(long id)
        {
            this.cmd.CommandText = $"DELETE FROM article WHERE id={id}";
            int rows = this.cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除所有文章
        /// </summary>
        public void DeleteAllArticle()
        {
            this.cmd.CommandText = "DELETE FROM article;";
            this.cmd.ExecuteNonQuery();
            this.cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name ='article';";
            this.cmd.ExecuteNonQuery();
            this.CleanDisk();
        }
    }

    public class SentData : Database
    {
        public SentData(string _dbName) : base(_dbName) { }

        public override void Init()
        {
            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS sent(id INTEGER PRIMARY KEY AUTOINCREMENT, create_time DATETIME NOT NULL, article TEXT NOT NULL, full_text TEXT NOT NULL, title TEXT NOT NULL, phrases TEXT, separator TEXT, type INT DEFAULT 0, disorder INT DEFAULT 0, no_repeat INT DEFAULT 0, count INT, mark INT, segment_record TEXT NOT NULL, segment_cursor INT DEFAULT 0, cur_segment_num INT, sent_num INT, sent_count INT, cycle INT DEFAULT 0, cycle_value INT, auto INT DEFAULT 0, auto_condition INT DEFAULT 0, auto_key INT, auto_operator INT, auto_number DOUBLE, auto_no INT DEFAULT 0);";
            this.cmd.ExecuteNonQuery();
        }

        public long InsertSent(string create_time, string article, string full_text, string title, string phrases, string separator, int type, int disorder, int no_repeat, int count, int mark, string segment_record, int segment_cursor, int cur_segment_num, int sent_num, int sent_count, int cycle, int cycle_value, int auto, int auto_condition, int auto_key, int auto_operator, double auto_number, int auto_no)
        {
            this.cmd.CommandText = $"INSERT INTO sent VALUES(NULL,'{create_time}','{ConvertText(article)}','{ConvertText(full_text)}','{ConvertText(title)}','{ConvertText(phrases)}','{ConvertText(separator)}',{type},{disorder},{no_repeat},{count},{mark},'{ConvertText(segment_record)}',{segment_cursor},{cur_segment_num},{sent_num},{sent_count},{cycle},{cycle_value},{auto},{auto_condition},{auto_key},{auto_operator},{auto_number},{auto_no}); SELECT last_insert_rowid();";
            object readId = this.cmd.ExecuteScalar();
            return Convert.ToInt64(readId);
        }

        public void UpdateSent(long id, string full_text, string title, int mark, string segment_record, int segment_cursor, int cur_segment_num, int sent_num, int sent_count, int cycle, int cycle_value, int auto, int auto_condition, int auto_key, int auto_operator, double auto_number, int auto_no)
        {
            this.cmd.CommandText = $"UPDATE sent SET full_text='{ConvertText(full_text)}', title='{ConvertText(title)}', mark={mark}, segment_record='{ConvertText(segment_record)}', segment_cursor={segment_cursor}, cur_segment_num={cur_segment_num}, sent_num={sent_num}, sent_count={sent_count}, cycle={cycle}, cycle_value={cycle_value}, auto={auto}, auto_condition={auto_condition}, auto_key={auto_key}, auto_operator={auto_operator}, auto_number={auto_number}, auto_no={auto_no} WHERE id={id};";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新配置标题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public void UpdateSentTitle(long id, string title)
        {
            this.cmd.CommandText = $"UPDATE sent SET title='{this.ConvertText(title)}' WHERE id={id};";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获取发文配置
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StorageDataSet.SentDataTable GetSent(int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM sent LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.SentDataTable mySent = new StorageDataSet.SentDataTable();
            adapter.Fill(mySent);
            return mySent;
        }

        /// <summary>
        /// 获取配置总数量
        /// </summary>
        /// <returns></returns>
        public int GetSentCount()
        {
            this.cmd.CommandText = "SELECT COUNT(1) FROM sent";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
        }

        /// <summary>
        /// 搜索包含指定标题文本的配置
        /// </summary>
        /// <param name="title"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StorageDataSet.SentDataTable GetSentFromSubTitle(string title, int start, int limit)
        {
            this.cmd.CommandText = $"SELECT * FROM sent WHERE title LIKE '%{this.ConvertText(title).Replace("%", "/%").Replace("_", "/_")}%' LIMIT {limit} OFFSET {start}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.SentDataTable mySent = new StorageDataSet.SentDataTable();
            adapter.Fill(mySent);
            return mySent;
        }

        /// <summary>
        /// 搜索包含指定标题文本的配置数量
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int GetSentCountFromSubTitle(string title)
        {
            this.cmd.CommandText = $"SELECT COUNT(1) FROM sent WHERE title LIKE '%{this.ConvertText(title).Replace("%", "/%").Replace("_", "/_")}%'";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return 0;
            }
            return Convert.ToInt32(readNum);
        }

        /// <summary>
        /// 查找是否存在 id 值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool FindIdInSent(long id)
        {
            this.cmd.CommandText = $"SELECT COUNT(1) FROM sent WHERE id={id}";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null)
            {
                return false;
            }
            if (Convert.ToInt32(readNum) == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据 id 删除配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteSentItemById(long id)
        {
            this.cmd.CommandText = $"DELETE FROM sent WHERE id={id}";
            int rows = this.cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        public void DeleteAllSent()
        {
            this.cmd.CommandText = "DELETE FROM sent;";
            this.cmd.ExecuteNonQuery();
            this.cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='sent';";
            this.cmd.ExecuteNonQuery();
            this.CleanDisk();
        }
    }

    public class CodeData : Database
    {
        public CodeData(string _dbName) : base(_dbName) { }

        public override void Init()
        {
            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS code_table_info(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, count INT, create_time DATETIME NOT NULL, table_index TEXT NOT NULL UNIQUE, max_len INT DEFAULT 1, len_type TEXT);";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "CREATE TABLE IF NOT EXISTS code_table_index(name TEXT PRIMARY KEY, seq INTEGER NOT NULL, max_len INT DEFAULT 1, len_type TEXT, used_id INTEGER DEFAULT -1, used_table_index TEXT);";
            this.cmd.ExecuteNonQuery();

            this.cmd.CommandText = "SELECT COUNT(1) FROM code_table_index WHERE name='home';";
            object readNum = this.cmd.ExecuteScalar();

            if (readNum == null || Convert.ToInt32(readNum) == 0)
            {
                this.cmd.CommandText = "INSERT INTO code_table_index VALUES('home',0,1,'1|0|0|0|0|0|0|0|0|0',-1,'')";
                this.cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCodeTableIndex(int max_len, string len_type, long used_id, string used_table_index)
        {
            this.cmd.CommandText = $"UPDATE code_table_index SET max_len={max_len}, len_type='{len_type}', used_id={used_id}, used_table_index='{used_table_index}' WHERE name='home';";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获取前表单索引值
        /// </summary>
        /// <returns></returns>
        public long GetSeq()
        {
            this.cmd.CommandText = "SELECT seq FROM code_table_index WHERE name='home'";
            object readSeq = this.cmd.ExecuteScalar();

            if (readSeq != null)
            {
                return Convert.ToInt64(readSeq);
            }
            return 0;
        }

        public void UpdateSeq(long seq)
        {
            this.cmd.CommandText = $"UPDATE code_table_index SET seq={seq} WHERE name='home';";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获取正在使用的码表
        /// </summary>
        /// <returns></returns>
        public string GetUsedTableIndex()
        {
            this.cmd.CommandText = "SELECT used_table_index FROM code_table_index WHERE name='home'";
            object readUsedTableIndex = this.cmd.ExecuteScalar();

            if (readUsedTableIndex != null)
            {
                return readUsedTableIndex.ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取正在使用的 CodeTableInfo 的 id
        /// </summary>
        /// <returns></returns>
        public long GetUsedId()
        {
            this.cmd.CommandText = "SELECT used_id FROM code_table_index WHERE name='home'";
            object readUsedId = this.cmd.ExecuteScalar();

            if (readUsedId != null)
            {
                return Convert.ToInt64(readUsedId);
            }
            return -1;
        }

        /// <summary>
        /// 获取码表中的最大词条长度
        /// - 注：最大值限定为 10 
        /// </summary>
        /// <returns></returns>
        public int GetMaxLen()
        {
            this.cmd.CommandText = "SELECT max_len FROM code_table_index WHERE name='home'";
            object maxLen = this.cmd.ExecuteScalar();

            if (maxLen != null)
            {
                int len = Convert.ToInt32(maxLen);
                return  len <= 10 ? len : 10;
            }
            return 1;
        }

        public int[] GetLenType()
        {
            this.cmd.CommandText = "SELECT len_type FROM code_table_index WHERE name='home'";
            object readLenType = this.cmd.ExecuteScalar();

            if (readLenType != null && !string.IsNullOrEmpty(readLenType.ToString()))
            {
                return Array.ConvertAll(readLenType.ToString().Split('|'), s => int.Parse(s));
            }
            return new int[10];
        }

        /// <summary>
        /// 获取所有保存表单的信息
        /// </summary>
        /// <returns></returns>
        public StorageDataSet.CodeTableInfoDataTable GetCodeTableInfo()
        {
            this.cmd.CommandText = "SELECT * FROM code_table_info;";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.CodeTableInfoDataTable myCodeTableInfo = new StorageDataSet.CodeTableInfoDataTable();
            adapter.Fill(myCodeTableInfo);
            return myCodeTableInfo;
        }

        public void InsertCodeTableInfo(string name, int count, string create_time, string table_index, int max_len, string len_type)
        {
            this.cmd.CommandText = $"INSERT INTO code_table_info VALUES(NULL,'{ConvertText(name)}',{count},'{create_time}','{table_index}',{max_len},'{len_type}');";
            this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 创建新的码表
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="allWordDic"></param>
        public void CreateCodeTable(string table_name, Dictionary<string, string> allWordDic)
        {
            string name = "code_table_" + ConvertText(table_name);
            this.cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {name} (id INTEGER PRIMARY KEY AUTOINCREMENT, word TEXT NOT NULL, coding TEXT NOT NULL);";
            this.cmd.ExecuteNonQuery();

            this.InsertDicToWord(name, allWordDic);
        }

        /// <summary>
        /// 删除码表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="table_name"></param>
        public void DropCodeTable(long id, string table_name)
        {
            this.cmd.CommandText = $"DELETE FROM code_table_info WHERE id={id}";
            this.cmd.ExecuteNonQuery();

            string name = "code_table_" + ConvertText(table_name);
            this.cmd.CommandText = $"DROP TABLE {name};";
            this.cmd.ExecuteNonQuery();

            this.CleanDisk();
        }

        public StorageDataSet.CodeDataTable GetCodeTableFromTableName(string table_name)
        {
            string name = "code_table_" + ConvertText(table_name);
            this.cmd.CommandText = $"SELECT * FROM {name}";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.cmd);
            StorageDataSet.CodeDataTable myCode = new StorageDataSet.CodeDataTable();
            adapter.Fill(myCode);
            return myCode;
        }

        private void InsertDicToWord(string name, Dictionary<string, string> dic)
        {
            this.cmd.CommandText = "PRAGMA synchronous = 0;";
            this.cmd.ExecuteNonQuery();

            using (SQLiteTransaction tran = this.cn.BeginTransaction())
            {
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand($"INSERT INTO {name}(word, coding) VALUES(@word, @coding)", this.cn))
                    {
                        foreach (var co in dic)
                        {
                            command.Parameters.Add(new SQLiteParameter("@word", co.Key));
                            command.Parameters.Add(new SQLiteParameter("@coding", co.Value));
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
