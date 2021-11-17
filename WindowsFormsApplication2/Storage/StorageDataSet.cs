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
                if (scoreData[i]["score_time"].ToString().Equals(time))
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
    }
}
