using System;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;

using Globals = SC2_GameTranslater.Source.Class_Globals;
using Log = SC2_GameTranslater.Source.Class_Log;
using EnumLanguage = SC2_GameTranslater.Source.EnumLanguage;
using System.Data;
using System.Collections.Generic;

namespace SC2_GameTranslater.Source
{
    /// <summary>
    /// 翻译文本数据表
    /// </summary>
    public partial class Data_GameText
    {
        #region 声明常量

        #region 枚举声明
        /// <summary>
        /// 文本状态
        /// </summary>
        public enum EnumGameTextType
        {
            /// <summary>
            /// 空
            /// </summary>
            Empty,
            /// <summary>
            /// 正常
            /// </summary>
            Normal,
            /// <summary>
            /// 已修改
            /// </summary>
            Modified,
            /// <summary>
            /// 覆盖
            /// </summary>
            Overwrite,
        }

        /// <summary>
        /// 数据状态
        /// </summary>
        public enum EnumDataValueStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal,
            /// <summary>
            /// 未使用
            /// </summary>
            New,
            /// <summary>
            /// 未使用
            /// </summary>
            Unused,
        }

        /// <summary>
        /// 文本文件
        /// </summary>
        public enum EnumGameTextFile
        {
            GameStrings,
            ObjectStrings,
            TriggerStrings,
        }

        #endregion

        #region 正则表达式常量
        /// <summary>
        /// Galaxy文本函数
        /// </summary>
        public static Regex Const_Regex_StringExternal = new Regex("(?<=StringExternal\\(\")[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*(?=\"\\))", RegexOptions.Compiled);
        public const int UnuseID = -1;
        public static string Const_String_GameTextMainPath = ".SC2Data\\LocalizedData\\";
        public static string Const_String_FileGameStringsPath = "GameStrings.txt";
        public static string Const_String_FileObjectStringsPath = "ObjectStrings.txt";
        public static string Const_String_FileTriggerStringsPath = "TriggerStrings.txt";

        #endregion

        #region 表常量声明

        #region 表名

        public const string TN_ModInfo = "Table_ModInfo";
        public const string TN_Language = "Table_Language";
        public const string TN_GalaxyFile = "Table_GalaxyFile";
        public const string TN_GalaxyLine = "Table_GalaxyLine";
        public const string TN_GalaxyLocation = "Table_GalaxyLocation";
        public const string TN_GameText = "Table_GameText";
        public const string TN_TextValue = "Table_TextValue";
        public const string TN_Log = "Table_Log";

        #endregion

        #region 列名

        public const string RN_ModInfo_FilePath = "FilePath";
        public const string RN_Language_ID = "ID";
        public const string RN_Language_Count = "Count";
        public const string RN_Language_Status = "Status";
        public const string RN_GalaxyFile_Path = "Path";
        public const string RN_GalaxyFile_Count = "Count";
        public const string RN_GalaxyLine_Index = "Index";
        public const string RN_GalaxyLine_File = "File";
        public const string RN_GalaxyLine_Script = "Script";
        public const string RN_GalaxyLocation_Index = "Index";
        public const string RN_GalaxyLocation_Line = "Line";
        public const string RN_GalaxyLocation_TextID = "TextID";
        public const string RN_GalaxyLocation_Key = "Key";
        public const string RN_GameText_ID = "ID";
        public const string RN_GameText_File = "File";
        public const string RN_GameText_Status = "Status";
        public const string RN_TextValue_Index = "Index";
        public const string RN_TextValue_TextID = "TextID";
        public const string RN_TextValue_LangID = "LangID";
        public const string RN_TextValue_Type = "Type";
        public const string RN_TextValue_Text = "Text";
        public const string RN_TextValue_TempText = "TempText";
        public const string RN_Log_ID = "ID";
        public const string RN_Log_Date = "Date";
        public const string RN_Log_Type = "Type";
        public const string RN_Log_Target = "Target";
        public const string RN_Log_OldValue = "OldValue";
        public const string RN_Log_NewValue = "NewValue";
        public const string RN_Log_Msg = "Msg";

        #endregion

        #region 关系

        public const string RSN_Language_TextValue_LangID = "Relation_Language_TextValue_LangID";
        public const string RSN_GameText_TextValue_TextID = "Relation_GameText_TextValue_TextID";
        public const string RSN_GalaxyLine_GameLocation_Line = "Relation_GalaxyLine_GameLocation_Line";
        public const string RSN_GalaxyFile_GalaxyLine_File = "Relation_GalaxyFile_GalaxyLine_File";
        public const string RSN_TextValue_Log_Target = "Relation_TextValue_Log_Target";
        public const string RSN_GameText_GalaxyLocation_Key = "Relation_GameText_GalaxyLocation_Key";

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
            GetGameTextFiles(file.Directory);
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
            SetDataValue(TN_Language, RN_Language_Status, EnumDataValueStatus.Unused);
            SetDataValue(TN_GameText, RN_GameText_Status, EnumDataValueStatus.Unused);
            GetGameTextFiles(file.Directory);
        }

        #endregion

        #region 文本功能

        #region 通用

        /// <summary>
        /// 生成文本文件路径
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="file">文件类型</param>
        /// <returns>路径</returns>
        public string TextFilePath(EnumLanguage lang, EnumGameTextFile file)
        {
            string path = Enum.GetName(lang.GetType(), lang) + Const_String_GameTextMainPath;
            switch (file)
            {
                case EnumGameTextFile.GameStrings:
                    path += Const_String_FileGameStringsPath;
                    break;
                case EnumGameTextFile.ObjectStrings:
                    path += Const_String_FileObjectStringsPath;
                    break;
                case EnumGameTextFile.TriggerStrings:
                    path += Const_String_FileTriggerStringsPath;
                    break;
                default:
                    Log.Assert(false);
                    break;
            }
            return path;
        }

        /// <summary>
        /// 批量设置值（行）
        /// </summary>
        /// <param name="rows">列表</param>
        /// <param name="column">行</param>
        /// <param name="value">值</param>
        private void SetDataValue(List<DataRow> rows, string column, object value)
        {
            foreach (DataRow select in rows)
            {
                select[column] = value;
            }
        }

        /// <summary>
        /// 批量设置值(整个表)
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="column">行</param>
        /// <param name="value">值</param>
        private void SetDataValue(string name, string column, object value)
        {
            foreach (DataRow select in Tables[name].Rows)
            {
                select[column] = value;
            }
        }

        #endregion

        #region 加载

        /// <summary>
        /// 获取Mod的文本文件
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        public void GetGameTextFiles(DirectoryInfo baseDir)
        {
            int count;
            foreach (EnumLanguage lang in Enum.GetValues(typeof(EnumLanguage)))
            {
                count = 0;
                DataRow row = SetLanguage(lang);
                foreach (EnumGameTextFile file in Enum.GetValues(typeof(EnumGameTextFile)))
                {
                    LoadGameTextFile(baseDir, lang, file, ref count);
                }
                row[RN_Language_Count] = count;
            }
        }

        /// <summary>
        /// 加载文本
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <param name="lang">语言</param>
        /// <param name="file">文件类型</param>
        private void LoadGameTextFile(DirectoryInfo baseDir, EnumLanguage lang, EnumGameTextFile file, ref int count)
        {
            string path = string.Format("{0}\\{1}", baseDir.FullName, TextFilePath(lang, file));
            string line;
            string key;
            string value;
            int length;
            StreamReader sr = new StreamReader(path);
            DataRow rowText;
            DataRow rowValue;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.StartsWith("//")) continue;
                length = line.IndexOf("=");
                key = line.Substring(0, length++);
                value = line.Substring(length);
                rowValue = SetTextValue(lang, file, key, value, out rowText);
                count++;
            }

            sr.Close();
        }

        #endregion

        #region 数据

        /// <summary>
        /// 设置语言数据
        /// </summary>
        /// <param name="lang">语言</param>
        /// <returns>语言对应的数据行</returns>
        public DataRow SetLanguage(EnumLanguage lang)
        {
            DataTable table = Tables[TN_Language];
            DataRow row = table.Rows.Find(lang);
            if (row == null)
            {
                row = table.NewRow();
                row[RN_Language_ID] = lang;
                row[RN_Language_Status] = EnumDataValueStatus.New;
                table.Rows.Add(row);
            }
            else
            {
                row[RN_Language_Status] = EnumDataValueStatus.Normal;
            }
            return row;
        }

        /// <summary>
        /// 设置游戏文本数据
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public DataRow SetGameText(EnumGameTextFile file, string key)
        {
            DataTable table = Tables[TN_GameText];
            DataRow row = table.Rows.Find(key);
            if (row == null)
            {
                row = table.NewRow();
                row[RN_GameText_ID] = key;
                row[RN_GameText_File] = file;
                row[RN_GameText_Status] = EnumDataValueStatus.New;
                table.Rows.Add(row);
            }
            else
            {
                row[RN_GameText_Status] = EnumDataValueStatus.Normal;
            }
            return row;
        }

        /// <summary>
        /// 设置文本值数据
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="file">文件</param>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="textRow">文本列</param>
        /// <returns></returns>
        public DataRow SetTextValue(EnumLanguage lang, EnumGameTextFile file, string key, string value, out DataRow textRow)
        {
            textRow = SetGameText(file, key);
            DataTable table = Tables[TN_TextValue];
            string filter = string.Format("{0}='{1}' and {2}={3:D}", RN_TextValue_TextID, key, RN_TextValue_LangID, lang);
            DataRow[] rows = table.Select(filter);
            DataRow row = null;
            switch (rows.Length)
            {
                case 0:
                    row = table.NewRow();
                    row[RN_TextValue_TextID] = key;
                    row[RN_TextValue_LangID] = lang;
                    row[RN_TextValue_Type] = EnumGameTextType.Overwrite;
                    row[RN_TextValue_Text] = value;
                    table.Rows.Add(row);
                    break;
                case 1:
                    row = rows[0];
                    row[RN_TextValue_Type] = EnumGameTextType.Overwrite;
                    break;
                default:
                    Log.Assert(false);
                    break;
            }
            return row;
        }

        #endregion

        #endregion

        #region 脚本功能

        /// <summary>
        /// 清理Galaxy相关的表
        /// </summary>
        private void CleanGalaxyTable()
        {
            Tables[TN_GalaxyLocation].Clear();
            Tables[TN_GalaxyLine].Clear();
            Tables[TN_GalaxyFile].Clear();
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
                MatchCollection matchs = Const_Regex_StringExternal.Matches(line);
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
