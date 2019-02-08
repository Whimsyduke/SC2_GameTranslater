using System;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;

using Globals = SC2_GameTranslater.Source.Class_Globals;
using Log = SC2_GameTranslater.Source.Class_Log;
using EnumLanguage = SC2_GameTranslater.Source.EnumLanguage;
using System.Data;

namespace SC2_GameTranslater.Source
{
    /// <summary>
    /// 翻译文本数据表
    /// </summary>
    public partial class Data_GameText
    {
        #region 声明常量

        #region 正则表达式常量
        /// <summary>
        /// Galaxy文本函数
        /// </summary>
        public static Regex REGEX_STRINGEXTERNAL = new Regex("(?<=StringExternal\\(\")[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*(?=\"\\))", RegexOptions.Compiled);
        public const int UnuseID = -1;

        #endregion

        #region 表常量声明

        #region 表名

        public const string TN_ModInfo = "Table_ModInfo";
        public const string TN_Language = "Table_Language";
        public const string TN_GalaxyFile = "Table_GalaxyFile";
        public const string TN_GalaxyLine = "Table_GalaxyLine";
        public const string TN_GameText = "Table_GameText";
        public const string TN_GalaxyLocation = "Table_GalaxyLocation";
        public const string TN_Log = "Table_Log";

        #endregion

        #region 列名

        public const string RN_ModInfo_FilePath = "FilePath";
        public const string RN_Language_ID = "ID";
        public const string RN_Language_Count = "Count";
        public const string RN_GalaxyFile_Path = "Path";
        public const string RN_GalaxyFile_Count = "Count";
        public const string RN_GalaxyLine_Index = "Index";
        public const string RN_GalaxyLine_File = "File";
        public const string RN_GalaxyLine_Script = "Script";
        public const string RN_GameText_Index = "Index";
        public const string RN_GameText_ID = "ID";
        public const string RN_GameText_LanguageID = "LanguageID";
        public const string RN_GameText_Type = "Type";
        public const string RN_GameText_Text = "Text";
        public const string RN_GameText_TempText = "TempText";
        public const string RN_GalaxyLocation_Index = "Index";
        public const string RN_GalaxyLocation_Line = "Line";
        public const string RN_GalaxyLocation_TextID = "TextID";
        public const string RN_GalaxyLocation_Key = "Key";
        public const string RN_Log_ID = "ID";
        public const string RN_Log_Date = "Date";
        public const string RN_Log_Type = "Type";
        public const string RN_Log_Target = "Target";
        public const string RN_Log_OldValue = "OldValue";
        public const string RN_Log_NewValue = "NewValue";
        public const string RN_Log_Msg = "Msg";

        #endregion

        #region 关系

        public const string RSN_Language_GameText_LanguageID = "Relation_Language_GameText_LanguageID";
        public const string RSN_GalaxyFile_GalaxyLine_File = "Relation_GalaxyFile_GalaxyLine_File";
        public const string RSN_GalaxyLine_GameLocation_Line = "Relation_GalaxyLine_GameLocation_Line";
        public const string RSN_GameText_GalaxyLocation_TextID = "Relation_GameText_GalaxyLocation_TextID";
        public const string RSN_GameText_Log_Target = "Relation_GameText_Log_Target";

        #endregion

        #endregion

        #endregion

        #region 属性字段

        #endregion

        #region 构造函数

        #endregion

        #region 方法

        #region 加载

        /// <summary>
        /// 从Mod初始化数据集
        /// </summary>
        /// <param name="path">文件路径</param>
        public void Initialization(FileInfo file)
        {
            Clear();
            Tables[TN_ModInfo].Rows.Add(file.FullName);
            CleanGalaxyTable();
            GetGalaxyFiles(file.Directory);
        }

        /// <summary>
        /// 重载文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public void ReloadFile(FileInfo file)
        {
            Tables[TN_ModInfo].Rows.Add(file.FullName);
            CleanGalaxyTable();
            GetGalaxyFiles(file.Directory);
        }

        #endregion

        #region 文本相关

        #endregion

        #region 脚本相关

        /// <summary>
        /// 清理Galaxy相关的表
        /// </summary>
        private void CleanGalaxyTable()
        {
            Tables[TN_GalaxyFile].Clear();
            Tables[TN_GalaxyLine].Clear();
            Tables[TN_GalaxyLocation].Clear();
        }

        /// <summary>
        /// 递归获取Mod的Galaxy文件
        /// </summary>
        /// <param name="parentDir">父级目录</param>
        private void GetGalaxyFiles(DirectoryInfo parentDir)
        {
            try
            {
                foreach (FileInfo select in parentDir.GetFiles())
                {
                    if (select.Extension.ToLower() == ".galaxy")
                    {
                        LoadGalaxyFile(select);
                    }
                }
                foreach (DirectoryInfo select in parentDir.GetDirectories())
                {
                    GetGalaxyFiles(select);
                }
            }
            catch (Exception e)
            {
                Log.DisplayLogOnUI(e.Message);
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 加载Galaxy文件
        /// </summary>
        /// <param name="file">Galaxy文件</param>
        private void LoadGalaxyFile(FileInfo file)
        {
            string path = file.FullName;
            DataRow row = Tables[TN_GalaxyFile].NewRow();
            row[RN_GalaxyFile_Path] = path;
            Tables[TN_GalaxyFile].Rows.Add(row);
            int count = 0;
            DataTable tableLine = Tables[TN_GalaxyLine];
            DataTable tableLocation = Tables[TN_GalaxyLocation];
            int indexLine = tableLine.Rows.Count;
            int indexLocation = tableLocation.Rows.Count;
            StreamReader sr = new StreamReader(path);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                MatchCollection matchs = REGEX_STRINGEXTERNAL.Matches(line);
                if (matchs.Count == 0) continue;
                tableLine.Rows.Add(indexLine, path, line);
                foreach (var select in matchs)
                {
                    count++;
                    tableLocation.Rows.Add(indexLocation++, indexLine, UnuseID, select.ToString());
                }
                indexLine++;
            }
            sr.Close();
            row[RN_GalaxyFile_Count] = count;
        }

        #endregion

        #endregion
    }
}
