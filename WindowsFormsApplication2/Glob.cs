﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; //正则
using System.Collections;
using System.Globalization;
using System.Reflection;
using WindowsFormsApplication2.编码提示;
using WindowsFormsApplication2.Storage;

namespace WindowsFormsApplication2
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class Glob
    {
        public static string Ver = "1.0.0";

        public static string Form = "雨天跟打器v" + Ver;
        /// <summary>
        /// 成绩版本
        /// </summary>
        public static string Instration = "s03";
        ///// <summary>
        ///// 编码码表
        ///// </summary>
        //public static List<List<string>> BmTips = new List<List<string>>();

        //控制类
        /// <summary>
        /// 显示实时数据
        /// </summary>
        public static bool ShowRealTimeData = false;
        /// <summary>
        /// 自动复制成绩
        /// </summary>
        public static bool AutoCopy = false;
        /// <summary>
        /// 使用符号选重
        /// </summary>
        public static bool useSymbolSelect = false;

        /// <summary>
        /// 当前段号
        /// </summary>
        public static int CurSegmentNum = 1225;
        
        public static int Time = 0;
        public static string Text;//跟打文字*****
        /// <summary>
        /// 跟打的文段内容
        /// </summary>
        public static string TypeText;
        /// <summary>
        /// 已跟打字数
        /// </summary>
        public static int TypeTextCount = 0;
        public static double TextSpeed; //以下为上次成绩
        public static double Textjj;
        public static double Textmc;
        /// <summary>
        /// 上一次文段校验码
        /// 用于判定是否为重打
        /// </summary>
        public static string TextPreCout = "";
        public static bool ReTypePD = false;//重打判断
        public static int TextLen; //总字数
        /// <summary>
        /// 需要减去的数量
        /// </summary>
        public static int TextJc = 0;
        public static int TextCz = 0; //错字
        public static int TextiCz = 0; //正字计数（用来获取错字数量）
        public static int TextJs = 0; //键数

        public static int TextMc = 0;  //码长完美计数
        public static int TextMcc = 0; //完美计数总量

        public static int TextJj = 0; //击键
        public static ArrayList TextHgPlace = new ArrayList(); //显示回改地点
        public static int TextHgPlace_Skip = 0; //点击跳转的标记
        public static int TextBg = 0;//退格 + 回改量
        public static int 回车 = 0;
        public static int 选重 = 0;
        public static bool 是否选重 = true;

        /// <summary>
        /// 记录着本次按下的键
        /// - 用于处理中文后接数字时会被判定为选重的情况
        /// - 因为键盘钩子的触发是在跟打区 KeyDown 事件之前，所以需要将值先记录下来，放到实际处理 Glob.是否选重 的布尔值后再进行选重判定
        /// </summary>
        public static int TheKeyValue = -1;

        /// <summary>
        /// 真为中文；假为英文
        /// </summary>
        public static bool 文段类型 = true;
        public static int 撤销 = 0;
        public static int 撤销用量 = 0;
        
        //检查过程是否一直持续
        public static DateTime nowStart;
        /// <summary>
        /// 载文的段数
        /// </summary>
        public static Match getDuan;
        /// <summary>
        /// 获取载文段号用的正则表达式
        /// </summary>
        public static Regex regexCout;

        /// <summary>
        /// 对照区正确字的背景色
        /// - Right
        /// </summary>
        public static Color Right;
        /// <summary>
        /// 颜色
        /// - False
        /// </summary>
        public static Color False;

        public static Color r1Back;
        //峰值
        public static double MaxSpeed = 0;
        public static double MaxJj = 0;
        public static double MaxMc = 10; //码长


        public static int TextHg = 0; //回改

        /// <summary>
        /// 回改用时
        /// </summary>
        public static double hgAllUse;

        /// <summary>
        /// 记录总回改
        /// 用于计算记录字数
        /// </summary>
        public static int TextHgAll = 0;
        /// <summary>
        /// 总计跟打数
        /// 不包括回改
        /// </summary>
        public static int TextLenAll;
        
        /// <summary>
        /// 记录天数
        /// </summary>
        public static int TextRecDays = 0;
        /// <summary>
        /// 今日时间    
        /// </summary>
        public static string TodayDate;
        public static double TextHg_ = 0;//回改率
        public static double TextDc_ = 0; // 打词率

        public static int LoadCount = 0;//载入次数 暂时是用来确定是否开启输入法
        /// <summary>
        /// 跟打用时
        /// </summary>
        public static double TypeUseTime;
        /// <summary>
        /// 已跟打段数
        /// </summary>
        public static int HaveTypeCount = 0;
        public static int HaveTypeCount_ = 0;//实际跟打段数
        public static double TotalUse = 0;//总用时

        /// <summary>
        /// 个性签名
        /// </summary>
        public static string InstraPre = "";
        /// <summary>
        /// 是否使用个性签名
        /// </summary>
        public static string InstraPre_ = "";

        /// <summary>
        /// 输入法签名
        /// </summary>
        public static string InstraSrf = "";
        /// <summary>
        /// 是否启用输入法签名
        /// </summary>
        public static string InstraSrf_ = "";

        /// <summary>
        /// 打词次数
        /// </summary>
        public static int aTypeWords = 0;
        /// <summary>
        /// 打词的字数量
        /// </summary>
        public static int aTypeWordsCount = 0;

        public static Font font_1; //对照区字体大小
        public static Font font_2; //跟打区字体大小

        public static bool binput = true;
        /// <summary>
        /// 一行高度
        /// </summary>
        public static int oneH;
        /// <summary>
        /// 重打次数
        /// </summary>
        public static int reTypeCount = 0;

        /// <summary>
        /// 跟打效率
        /// </summary>
        public static int 效率 = 0;
        //发送的控制
        public static string sortSend = "ABCVDTSEFULGNORQ";
        public static int LastInput = 0;//末字错时不发送 可以继续跟打

        //跟打历史
        public static int TypeCount = 0;//跟打次数

        //发文标记
        public static int SendNow = 0;

        /// <summary>
        /// 前导符
        /// </summary>
        public static string PreText;
        /// <summary>
        /// 段标
        /// </summary>
        public static string PreDuan;
        /// <summary>
        /// 是否开启自定义前导符
        /// </summary>
        public static bool isZdy;

        public static string getName = "";//发文配置的名称

        //图表速度传递
        public static double chartSpeedTo = 0;
        public static bool chartShow = false;

        //表传递
        public static int Count = 0;

        //平均所有
        public static double Per_Speed = 0;//平均速度
        public static double Per_Jj = 0;//平均击键
        public static double Per_Mc = 0;//平均码长
        public static int Total_Type = 0;//跟打总字数

        //今日已跟打
        public static int todayTyping = 0;
        /// <summary>
        /// 是否为赛文
        /// </summary>
        public static bool isMatch = false;

        /// <summary>
        /// 上一次跟打成绩
        /// </summary>
        public static string theLastGoal = "";
        
        //随机段数
        public static int AZpre = 88;
        //错次
        public static int FalseCount = 0;
        public static ArrayList FWords = new ArrayList();
        public static int FWordsSkip = 0;//错字跳转标记

        //拖动条
        public static int p1;
        public static int p2;

        /// <summary>
        /// 曲线界面
        /// 默认显示
        /// </summary>
        public static bool isShowSpline = false;
        /// <summary>
        /// 停止用时
        /// </summary>
        public static int StopUse = 1;
        /// <summary>
        /// 曲线极值
        /// </summary>
        public static double MinSplite = 500;
        /// <summary>
        /// 极简模式
        /// </summary>
        public static bool simpleMoudle = false;
        /// <summary>
        /// 极简模式的分隔符
        /// </summary>
        public static string simpleSplite = "|";
        /// <summary>
        /// 自动替换英转中
        /// </summary>
        public static bool autoReplaceBiaodian = false;

        /// <summary>
        /// 暂停次数
        /// </summary>
        public static int PauseTimes = 0;
        /// <summary>
        /// 击键比例
        /// </summary>
        public static int[] jjPer = new int[9];
        /// <summary>
        /// 总跟打段数
        /// </summary>
        public static int jjAllC = 0;
        
        //跟打地图
        public static Graphics Type_Map;
        public static Color Type_Map_Color = Color.Green;
        public static Color Type_map_C_1 = Color.FromArgb(220,220,220);
        public static int Type_Map_C = 200;
        public static int 地图长度 = 0;
        public static bool Type_Map_Level = true;//优先级

        /// <summary>
        /// 打开标记
        /// - 这是一个记录，真正使用的是 this.tsb标注.Checked
        /// </summary>
        public static bool IsPointIt = false;

        //分析
        public static bool Use分析 = false;

        /// <summary>
        /// 测速点控制
        /// 注：存的是 index
        /// </summary>
        public static int[] SpeedPoint_ = new int[10];
        /// <summary>
        /// 测速点时间控制
        /// </summary>
        public static double[] SpeedTime = new double[10];
        /// <summary>
        /// 键数
        /// </summary>
        public static int[] SpeedJs = new int[10];
        /// <summary>
        /// 回改
        /// </summary>
        public static int[] SpeedHg = new int[10];
        /// <summary>
        /// 测速点数量控制
        /// </summary>
        public static int SpeedPointCount = 0;
        public static int SpeedControl = 0;

        /// <summary>
        /// 跟打报告
        /// </summary>
        public static List<TypeDate> TypeReport = new List<TypeDate>();

        /// <summary>
        /// 图片成绩发送昵称
        /// </summary>
        public static string PicName = "";

        public static DateTime TextTime;

        /// <summary>
        /// 是否开启智能测词
        /// </summary>
        public static bool 是否智能测词 = false;
        /// <summary>
        /// 编码记录
        /// </summary>
        public static List<BmAll> BmAlls = new List<BmAll>();
        public static double 词库理论码长 = 0;
        public static string 词组编码 = "";
        

        /// <summary>
        /// 是否正在测词中
        /// </summary>
        public static bool IsChecking = false;

        /// <summary>
        /// 禁止保存高阶统计
        /// </summary>
        public static bool DisableSaveAdvanced = false;

        /// <summary>
        /// 快捷键列表
        /// </summary>
        public static List<HotKey> HotKeyList = new List<HotKey> {
            new HotKey("设置", "F1"),
            new HotKey("发文", "F2"),
            new HotKey("重打", "F3"),
            new HotKey("暂停", "F4"),
            new HotKey("复制当前文段", "F5"),
            new HotKey("复制上次成绩", "F6"),
            new HotKey("复制图片成绩", "Ctrl+T"),
            new HotKey("发上一段", "Ctrl+P"),
            new HotKey("发下一段", "Ctrl+N"),
            new HotKey("速度分析", "Ctrl+G"),
            new HotKey("跟打报告", "Ctrl+J"),
            new HotKey("按键统计", "Ctrl+K"),
            new HotKey("理论按键统计", "Ctrl+M"),
            new HotKey("历史记录", "Ctrl+H"),
            new HotKey("保存发文配置", "Ctrl+S"),
            new HotKey("乱序重打", "Ctrl+L"),
            new HotKey("停止发文", "Ctrl+W"),
            new HotKey("查询当前编码", "Ctrl+F"),
            new HotKey("打开练习", "Ctrl+O"),
            new HotKey("测速数据", "Ctrl+I"),
            new HotKey("窗口复位", "Ctrl+D"),
            new HotKey("检验真伪", "Alt+D"),
            new HotKey("直接载文", "Alt+E"),
            new HotKey("格式载文", "Alt+S"),
            new HotKey("老板键", "Alt+Q"),
        };

        /// <summary>
        /// 保存曲线各点速度值
        /// </summary>
        public static List<double> ChartSpeedArr = new List<double> ();

        /// <summary>
        /// 按键统计
        /// </summary>
        public static int[] KeysTotal = new int[50];

        /// <summary>
        /// 理论按键统计
        /// </summary>
        public static int[] CalcKeysTotal = new int[50];

        /// <summary>
        /// 历史总按键统计
        /// </summary>
        public static int[] HistoryKeysTotal = new int[50];

        /// <summary>
        /// 临时文段保存器
        /// 主要用于实现"发上一段"的功能
        /// </summary>
        public static List<string> TempSegmentRecord = new List<string> ();

        /// <summary>
        /// 发文游标
        /// 主要用于"发上一段"的功能
        /// </summary>
        public static int SendCursor = 0;

        /// <summary>
        /// 历史成绩
        /// </summary>
        public static ScoreData ScoreHistory;
        /// <summary>
        /// 历史文章
        /// </summary>
        public static ArticleData ArticleHistory;
        /// <summary>
        /// 历史发文配置
        /// </summary>
        public static SentData SentHistory;
        /// <summary>
        /// 码表历史记录
        /// </summary>
        public static CodeData CodeHistory;

        /// <summary>
        /// 所使用的码表
        /// </summary>
        public static string UsedTableIndex;

        /// <summary>
        /// 单字字典
        /// - 字索引
        /// </summary>
        public static Dictionary<string, string> SingleWordDic = new Dictionary<string, string>();

        /// <summary>
        /// 单字字典
        /// - 编码索引
        /// </summary>
        public static Dictionary<string, string> SingleCodeDic = new Dictionary<string, string>();

        /// <summary>
        /// 全字字典
        /// - 字索引
        /// </summary>
        public static Dictionary<string, string> AllWordDic = new Dictionary<string, string>();

        /// <summary>
        /// 全字字典
        /// - 编码索引
        /// </summary>
        public static Dictionary<string, string> AllCodeDic = new Dictionary<string, string>();

        /// <summary>
        /// 码表词条最大长度
        /// - 限定小于等于 10
        /// </summary>
        public static int WordMaxLen = 1;

        /// <summary>
        /// 码表词条各长度分布
        /// </summary>
        public static int[] WordLenType = new int[10];
    }

    /// <summary>
    /// 跟打报告
    /// </summary>
    public class TypeDate {
        /// <summary>
        /// 序
        /// </summary>
        public int Index { set; get; }
        /// <summary>
        /// 跟打起点
        /// </summary>
        public int Start { set; get; }
        /// <summary>
        /// 跟打终点
        /// </summary>
        public int End { set; get; }
        /// <summary>
        /// 跟打长度
        /// </summary>
        public int Length { set; get; }
        /// <summary>
        /// 当前时间
        /// </summary>
        public double NowTime { set; get; }
        /// <summary>
        /// 总时间
        /// </summary>
        public double TotalTime { set; get; }
        /// <summary>
        /// 当前击键
        /// </summary>
        public int Tick { set; get; }
        /// <summary>
        /// 总击键
        /// </summary>
        public int TotalTick { set; get; }
    }
}
