using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace WindowsFormsApplication2.Storage
{
    public class Database
    {
        protected string dbName;
        protected string dbPath;
        protected SQLiteConnection cn;
        protected SQLiteCommand cmd;

        public static string folderName = Path.Combine(System.Windows.Forms.Application.StartupPath, "Storage");

        public Database(string _dbName)
        {
            this.dbName = _dbName;

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            if (!string.IsNullOrEmpty(this.dbName))
            {
                this.dbPath = Path.Combine(folderName, this.dbName + ".db");
                this.cn = new SQLiteConnection("data source=" + this.dbPath);
                this.cn.Open();
                this.cmd = new SQLiteCommand();
                this.cmd.Connection = this.cn;
            }
        }

        public void CloseDatabase()
        {
            if (this.cn.State == System.Data.ConnectionState.Open)
            {
                this.cn.Close();
            }
        }

        /// <summary>
        /// 整理数据库
        /// </summary>
        public void CleanDisk()
        {
            this.cmd.CommandText = "VACUUM";
            this.cmd.ExecuteNonQuery();
        }

        public string ConvertText(string text)
        {
            return text.Replace("'", "''");
        }

        public virtual void Init() { }

    }
}
