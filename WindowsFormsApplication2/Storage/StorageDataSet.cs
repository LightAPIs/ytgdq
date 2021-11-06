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
    }
}
