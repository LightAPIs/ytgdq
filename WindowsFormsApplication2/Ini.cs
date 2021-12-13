﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication2
{
    public class Ini
    {
        /// <summary>
        /// INI 文件路径
        /// </summary>
        private readonly string path;
        public Ini(string inipath, bool isAbsolute = false)
        {
            if (isAbsolute)
            {
                this.path = inipath;
            }
            else
            {
                this.path = Path.Combine(Application.StartupPath, inipath);
            }
        }
        //声明读写INI文件的API函数    
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //写INI文件　　　    
        public void IniWriteValue(string section, string Key, string Value)
        {
            WritePrivateProfileString(section, Key, Value, this.path);
        }
        //读取INI文件指定
        public string IniReadValue(string section, string Key, string def)
        {
            try
            {
                StringBuilder temp = new StringBuilder(2048);
                int i = GetPrivateProfileString(section, Key, def, temp, 2048, this.path);

                return temp.ToString().Trim();
            }
            catch { return null; }
        }
    }
}
