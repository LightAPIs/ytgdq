using System;

namespace WindowsFormsApplication2.Storage
{
    public partial class StorageDataSet
    {
        /// <summary>
        /// 通过时间获取成绩行数据
        /// </summary>
        /// <param name="scoreData"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static ScoreRow GetScoreRowFromTime(ScoreDataTable scoreData, string time)
        {
            for (int i = 0; i < scoreData.Count; i++)
            {
                if (Convert.ToDateTime(scoreData[i]["score_time"]).ToString("yyyy-MM-dd HH:mm:ss").Equals(time))
                {
                    return scoreData[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 通过 id 值获取文章数据
        /// </summary>
        /// <param name="articleData"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ArticleRow GetArticleRowFromId(ArticleDataTable articleData, long id)
        {
            for (int i = 0; i < articleData.Count; i++)
            {
                if ((long)articleData[i]["id"] == id)
                {
                    return articleData[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 通过 id 值获取配置数据
        /// </summary>
        /// <param name="sentData"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SentRow GetSentRowFromId(SentDataTable sentData, long id)
        {
            for (int i = 0; i < sentData.Count; i++)
            {
                if ((long)sentData[i]["id"] == id)
                {
                    return sentData[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 通过 id 值获取码表表单信息
        /// </summary>
        /// <param name="infoData"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CodeTableInfoRow GetCodeTableInfoRowFromId(CodeTableInfoDataTable infoData, long id)
        {
            for (int i = 0; i <= infoData.Count; i++)
            {
                if ((long)infoData[i]["id"] == id)
                {
                    return infoData[i];
                }
            }
            return null;
        }
    }
}
