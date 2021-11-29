using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WindowsFormsApplication2.检查更新
{
    public class UpgradeModel
    {
        #region 事件

        public delegate void hasUp(object sender, bool b);

        public event hasUp HasUpdated;

        protected virtual void OnHasUpdated(bool b)
        {
            HasUpdated?.Invoke(this, b);
        }

        #endregion
        #region 属性

        private readonly string _url = "https://cdn.jsdelivr.net/gh/LightAPIs/ytgdq@main/updates.json";
        public List<VersionObject> VersionList { get; set; }
        public List<VersionObject> NewVersionList { get; set; }

        public string VersionValue { get; set; }
        public string DateValue { get; set; }
        public string InstraValue { get; set; }
        public string ContentValue { get; set; }
        public string OtherValue { get; set; }

        public bool IsUpdate = false;
        public bool IsError = false;

        /// <summary>
        /// 版本格式
        /// </summary>
        public int[] Standard = new int[3] { 8, 4, 2 };

        #endregion

        public UpgradeModel()
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("最新版本：" + VersionValue + " (" + DateValue + ")");
            if (!string.IsNullOrEmpty(InstraValue)) {
                sb.AppendLine("更新说明：\n" + InstraValue);
            }
            sb.AppendLine("更新内容：\n" + ContentValue);
            if (!string.IsNullOrEmpty(OtherValue))
            {
                sb.AppendLine("其它信息：\n" + OtherValue);
            }
            sb.AppendLine("");

            if (NewVersionList != null && NewVersionList.Count > 1)
            {
                for (int i = 1; i < NewVersionList.Count; i++)
                {
                    sb.AppendLine("过往版本：" + NewVersionList[i].Version + " (" + NewVersionList[i].Date + ")");
                    if (!string.IsNullOrEmpty(NewVersionList[i].Instra))
                    {
                        sb.AppendLine("更新说明：\n" + NewVersionList[i].Instra);
                    }
                    sb.AppendLine("更新内容：\n" + NewVersionList[i].Content);
                    if (!string.IsNullOrEmpty(NewVersionList[i].Other))
                    {
                        sb.AppendLine("其它信息：\n" + NewVersionList[i].Other);
                    }
                    sb.AppendLine("");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取源数据
        /// </summary>
        public void GetWebRequest()
        {
            IsError = false;
            YTWebRequest ytReq = new YTWebRequest(_url);
            string strJson = ytReq.Request();
            if (!string.IsNullOrEmpty(strJson))
            {
                VersionList = JsonConvert.DeserializeObject<List<VersionObject>>(strJson);

                var localVersion = Glob.Ver;
                VersionValue = localVersion;
                DateValue = "";
                InstraValue = "";
                ContentValue = "";
                OtherValue = "";
                NewVersionList = new List<VersionObject>();
                IsUpdate = false;
                foreach (VersionObject vo in VersionList)
                {
                    // 对版本进行判断
                    if (CompareVer(localVersion, vo.Version))
                    {
                        IsUpdate = true;
                        NewVersionList.Add(vo);
                    }
                }

                if (NewVersionList.Count > 0)
                {
                    NewVersionList.Sort(delegate (VersionObject x, VersionObject y)
                    {
                        if (CompareVer(x.Version, y.Version))
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    });

                    VersionValue = NewVersionList[0].Version;
                    DateValue = NewVersionList[0].Date;
                    InstraValue = NewVersionList[0].Instra;
                    ContentValue = NewVersionList[0].Content;
                    OtherValue = NewVersionList[0].Other;
                }
            }
            else
            {
                IsError = true;
                IsUpdate = false;
            }

            OnHasUpdated(IsUpdate);
        }

        /// <summary>
        /// 比较版本是否有更新
        /// </summary>
        /// <param name="vL">本地版本</param>
        /// <param name="vN">网络版本</param>
        /// <returns></returns>
        private bool CompareVer(string vL, string vN)
        {
            var vLoc = SpliteVer(vL);
            var vNet = SpliteVer(vN);
            if (vNet[0] == -1) return false;
            int compare = 0;
            for (int index = 0; index < vLoc.Length; index++)
            {
                if (vNet[index] > vLoc[index])
                {
                    compare += Standard[index];
                }
                else if (vNet[index] < vLoc[index])
                {
                    compare -= Standard[index];
                }
            }
            return compare > 0;
        }

        /// <summary>
        /// 获取各个小版本号
        /// </summary>
        /// <param name="ver"></param>
        /// <returns></returns>
        private int[] SpliteVer(string ver)
        {
            var sp = ver.Split('.');
            if (sp.Length < 3) {
                return new int[3] { 1, 0, 0 };
            }
            var sv = new int[3];
            int.TryParse(sp[0], out sv[0]);
            int.TryParse(sp[1], out sv[1]);
            int.TryParse(sp[2], out sv[2]);
            return sv;
        }
    }
}
