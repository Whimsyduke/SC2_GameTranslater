using System;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using System.Threading;
using System.Text;

namespace SC2_GameTranslater.Source
{
    using Globals = Class_Globals;
    using Threads = Class_Threads;
    using Log = Class_Log;

    #region 枚举声明

    /// <summary>
    /// 文本状态
    /// </summary>
    public enum EnumGameUseStatus
    {
        /// 过期
        /// </summary>
        Droped = 1,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 2,
        /// <summary>
        /// <summary>
        /// 新增
        /// </summary>
        New = 4,
        /// <summary>
        /// 全部
        /// </summary>
        All = 7,
    }

    /// <summary>
    /// 文本状态
    /// </summary>
    public enum EnumGameTextStatus
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty = 1,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 2,
        /// <summary>
        /// 已修改
        /// </summary>
        Modified = 4,
        /// <summary>
        /// 全部复选
        /// </summary>
        All = 7,
    }

    /// <summary>
    /// 文本文件
    /// </summary>
    public enum EnumGameTextFile
    {
        /// <summary>
        /// GameStrings.txt
        /// </summary>
        GameStrings = 1,
        /// <summary>
        /// ObjectStrings.txt
        /// </summary>
        ObjectStrings = 2,
        /// <summary>
        /// ObjectStrings.txt
        /// </summary>
        TriggerStrings = 4,
        /// <summary>
        /// 全部复选
        /// </summary>
        All = 7,
    }

    #endregion

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
        public static Regex Const_Regex_StringExternal = new Regex("(?<=StringExternal\\(\")[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*(?=\"\\))", RegexOptions.Compiled);
        public const int UnuseID = -1;
        public static string Const_String_GameTextMainPath = ".SC2Data\\LocalizedData\\";
        public static string Const_String_FileGameStringsPath = "GameStrings.txt";
        public static string Const_String_FileObjectStringsPath = "ObjectStrings.txt";
        public static string Const_String_FileTriggerStringsPath = "TriggerStrings.txt";

        #endregion

        #region 表常量声明

        #region 表名

        public const string TN_ProjectInfo = "Table_ProjectInfo";
        public const string TN_Language = "Table_Language";
        public const string TN_GalaxyFile = "Table_GalaxyFile";
        public const string TN_GalaxyLine = "Table_GalaxyLine";
        public const string TN_GalaxyLocation = "Table_GalaxyLocation";
        public const string TN_GameText = "Table_GameText";
        public const string TN_TextValue = "Table_TextValue";
        public const string TN_Log = "Table_Log";

        #endregion

        #region 列名

        public const string RN_ModInfo_CompontentsPath = "CompontentsPath";
        public const string RN_Language_ID = "ID";
        public const string RN_GalaxyFile_Path = "Path";
        public const string RN_GalaxyFile_Name = "Name";
        public const string RN_GalaxyFile_Count = "Count";
        public const string RN_GalaxyLine_Index = "Index";
        public const string RN_GalaxyLine_File = "File";
        public const string RN_GalaxyLine_Script = "Script";
        public const string RN_GalaxyLocation_Index = "Index";
        public const string RN_GalaxyLocation_Line = "Line";
        public const string RN_GalaxyLocation_TextID = "TextID";
        public const string RN_GalaxyLocation_Key = "Key";
        public const string RN_GameText_ID = "ID";
        public const string RN_GameText_Index = "Index";
        public const string RN_GameText_File = "File";
        public const string RN_GameText_TextStatus = "TextStatus";
        public const string RN_GameText_UseStatus = "UseStatuse";
        public const string RN_GameText_DropedText = "Droped";
        public const string RN_GameText_SourceText = "Source";
        public const string RN_GameText_EditedText = "Edited";

        #endregion

        #region 关系

        public const string RSN_GalaxyLine_GameLocation_Line = "Relation_GalaxyLine_GameLocation_Line";
        public const string RSN_GalaxyFile_GalaxyLine_File = "Relation_GalaxyFile_GalaxyLine_File";
        public const string RSN_GameText_GalaxyLocation_Key = "Relation_GameText_GalaxyLocation_Key";

        #endregion

        #region 其他

        public const string PATH_TempFolder = "Temp\\";
        public const string PATH_BackupFolder = "Backup\\";

        #endregion

        #endregion

        #endregion

        #region 属性字段

        /// <summary>
        /// 组件文件夹路径
        /// </summary>
        public FileInfo SC2Components
        {
            set
            {
                m_ComponentsPath = value;
                WriteCompontentsPath(value.FullName);
                Globals.MainWindow.TextBox_ComponentsPath.Text = value.FullName;
            }
            get
            {
                return m_ComponentsPath;
            }
        }
        private FileInfo m_ComponentsPath = null;

        /// <summary>
        /// 使用语言列表
        /// </summary>
        public List<EnumLanguage> LangaugeList { get; private set; }

        /// <summary>
        /// 项目信息行
        /// </summary>
        public DataRow ProjectInfoRow
        {
            get
            {
                if (Tables[TN_ProjectInfo].Rows.Count == 0)
                {
                    Tables[TN_ProjectInfo].Rows.Add(Tables[TN_ProjectInfo].NewRow());
                }
                return Tables[TN_ProjectInfo].Rows[0];
            }
        }

        #endregion

        #region 方法

        #region 重载方法

        public new void EndInit()
        {
            base.EndInit();
            foreach (EnumLanguage lang in Enum.GetValues(typeof(EnumLanguage)))
            {
                GenerateDataColumnForLanguage(lang);
            }
        }

        #endregion

        #region 数据

        /// <summary>
        /// 获取项目语言列表
        /// </summary>
        /// <returns></returns>
        public List<EnumLanguage> GetLanguageList()
        {
            return Tables[TN_Language].AsEnumerable().Select(r => (EnumLanguage)r[RN_Language_ID]).ToList();
        }

        /// <summary>
        /// 写入组件文件夹路径
        /// </summary>
        /// <param name="path">路径</param>
        private void WriteCompontentsPath(string path)
        {

            ProjectInfoRow[RN_ModInfo_CompontentsPath] = path;
        }

        #endregion

        #region 进程

        /// <summary>
        /// 委托设为当前Project
        /// </summary>
        /// <param name="count">当前计数</param>
        /// <param name="max">最大计数</param>
        private void SetToCurrentProject(double count, double max)
        {
            Globals.CurrentProject = this;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="argu"></param>
        public void ThreadInitialization(object argu)
        {
            FileInfo file = argu as FileInfo;
            CleanGalaxyTable();

            List<FileInfo> galaxyFiles = new List<FileInfo>();
            GetGalaxyFiles(file.Directory, ref galaxyFiles);
            int max = galaxyFiles.Count + GameTextFilesCount(file.Directory);
            Globals.MainWindow.ProgressBarInit(max, "", null);

            LoadGalaxyFile(galaxyFiles);
            LoadGameTextFile(file.Directory);

            Globals.MainWindow.ProgressBarClean(SetToCurrentProject);
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存当前项目
        /// </summary>
        /// <param name="path">保存路径</param>
        public void SaveProject(FileInfo path)
        {
            Globals.ObjectSerializerCompression(path, this);
        }

        #endregion

        #region 加载

        /// <summary>
        /// 从Mod初始化数据集
        /// </summary>
        /// <param name="path">文件路径</param>
        public void Initialization(FileInfo file)
        {
            Clear();
            Threads.StartThread(ThreadInitialization, file, true);
            SC2Components = file;
        }

        /// <summary>
        /// 加载项目
        /// </summary>
        /// <param name="path">路径</param>
        public static Data_GameText LoadProject(FileInfo path)
        {
            Data_GameText proj = Globals.ObjectDeserializeDecompress(path) as Data_GameText;
            proj.RefreshAttribute();
            return proj;
        }

        /// <summary>
        /// 刷新项目属性
        /// </summary>
        /// <returns>是否成功加载</returns>
        public bool RefreshAttribute()
        {
#if !DEBUG
            try
#endif
            {
                SC2Components = new FileInfo(ProjectInfoRow[RN_ModInfo_CompontentsPath] as string);
                LangaugeList = Tables[TN_Language].AsEnumerable().Select(r => (EnumLanguage)r[RN_Language_ID]).ToList();
                return true;
            }
#if !DEBUG
            catch
            {
                return false;
            }
#endif
        }

        #endregion

        #region 写入

        /// <summary>
        /// 写入到地图或Mod
        /// </summary>
        /// <returns>写入成功</returns>
        public bool WriteToComponents()
        {
            if (SC2Components != null && !SC2Components.Exists) return false;
            DirectoryInfo baseDir = SC2Components.Directory;
            List<FileInfo> backFiles = new List<FileInfo>();
            EnumerableRowCollection<DataRow> gameStringRows = GetGameTextRows(EnumGameTextFile.GameStrings);
            EnumerableRowCollection<DataRow> objectStringRows = GetGameTextRows(EnumGameTextFile.ObjectStrings);
            EnumerableRowCollection<DataRow> triggerStringRows = GetGameTextRows(EnumGameTextFile.TriggerStrings);
            foreach (EnumLanguage lang in Enum.GetValues(typeof(EnumLanguage)))
            {
                WriteToTextFile(baseDir, lang, EnumGameTextFile.GameStrings, ref backFiles, gameStringRows);
                WriteToTextFile(baseDir, lang, EnumGameTextFile.ObjectStrings, ref backFiles, objectStringRows);
                WriteToTextFile(baseDir, lang, EnumGameTextFile.TriggerStrings, ref backFiles, triggerStringRows);
            }
            return true;
        }

        /// <summary>
        /// 写入文本文件数据
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <param name="lang">语言</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="backFiles">备份文件列表</param>
        /// <param name="data">文件数据</param>
        /// <returns>写入成功</returns>
        private bool WriteToTextFile(DirectoryInfo baseDir, EnumLanguage lang, EnumGameTextFile fileTyperef, ref List<FileInfo> backFiles, EnumerableRowCollection<DataRow> data)
        {
#if !DEBUG
            try
#endif
            {
                string backPath = TextFilePath(lang, fileTyperef);
                string originpath = string.Format("{0}\\{1}", baseDir.FullName, backPath);
                if (!File.Exists(originpath)) return true;
                FileInfo backFile = new FileInfo(PATH_TempFolder + backPath);
                backFiles.Add(backFile);
                if (!backFile.Directory.Exists) backFile.Directory.Create();
                File.Copy(originpath, backFile.FullName, true);
                StreamWriter sw = new StreamWriter(originpath, false);
                string key = GetGameTextNameForLanguage(lang, RN_GameText_EditedText);
                foreach (DataRow row in data)
                {
                    sw.WriteLine(string.Format("{0}={1}", row[RN_GameText_ID], row[key]));
                }
                sw.Close();
                return true;
            }
#if !DEBUG
            cache
            {
                returen false;
            }
#endif
        }

        /// <summary>
        /// 获取文本文件对应的文本数据
        /// </summary>
        /// <param name="fileType">文本文件类型</param>
        /// <returns>数据列</returns>
        private EnumerableRowCollection<DataRow> GetGameTextRows(EnumGameTextFile fileType)
        {
            EnumerableRowCollection<DataRow> query = Tables[TN_GameText].AsEnumerable();
            string keyFile = RN_GameText_File;
            query = from row in query
                    where (EnumGameTextFile)row[keyFile] == fileType
                    orderby row[RN_GameText_ID]
                    select row;
            return query;
        }

        #endregion

        #region 文本功能

        #region 通用

        /// <summary>
        /// 获取语言对应列名
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="name">基本名称</param>
        /// <returns>列名</returns>
        public static string GetGameTextNameForLanguage(EnumLanguage lang, string name)
        {
            return string.Format("{0}_{1}", Enum.GetName(lang.GetType(), lang), name);
        }

        /// <summary>
        /// 获取枚举值对应的文本
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="value">枚举值</param>
        /// <returns>翻译名称</returns>
        public static string GetEnumNameInLanguage(EnumLanguage lang, Enum value)
        {
            string key = string.Format("ENUM_{0}_{1}", value.GetType().Name, Enum.GetName(value.GetType(), value));
            return Globals.DictUILanguages[lang][key] as string;
        }

        /// <summary>
        /// 生成语言对应的DataColumn配置
        /// </summary>
        /// <param name="lang">语言</param>
        private void GenerateDataColumnForLanguage(EnumLanguage lang)
        {
            string columnName;
            DataColumn column;
            DataTable table = Tables[TN_GameText];

            // Text Status
            columnName = GetGameTextNameForLanguage(lang, RN_GameText_TextStatus);
            column = new DataColumn(columnName, typeof(int), "", MappingType.Attribute)
            {
                Caption = columnName,
                DefaultValue = EnumGameTextStatus.Empty,
                AllowDBNull = false,
                Unique = false,
            };
            table.Columns.Add(column);

            // Use Status
            columnName = GetGameTextNameForLanguage(lang, RN_GameText_UseStatus);
            column = new DataColumn(columnName, typeof(int), "", MappingType.Attribute)
            {
                Caption = columnName,
                DefaultValue = EnumGameTextStatus.Empty,
                AllowDBNull = false,
                Unique = false,
            };
            table.Columns.Add(column);

            // Drop Text
            columnName = GetGameTextNameForLanguage(lang, RN_GameText_DropedText);
            column = new DataColumn(columnName, typeof(string), "", MappingType.Element)
            {
                Caption = columnName,
                DefaultValue = null,
                AllowDBNull = true,
                Unique = false,
            };
            table.Columns.Add(column);

            // Source Text
            columnName = GetGameTextNameForLanguage(lang, RN_GameText_SourceText);
            column = new DataColumn(columnName, typeof(string), "", MappingType.Element)
            {
                Caption = columnName,
                DefaultValue = null,
                AllowDBNull = true,
                Unique = false,
            };
            table.Columns.Add(column);

            // Edited Text
            columnName = GetGameTextNameForLanguage(lang, RN_GameText_EditedText);
            column = new DataColumn(columnName, typeof(string), "", MappingType.Element)
            {
                Caption = columnName,
                DefaultValue = null,
                AllowDBNull = true,
                Unique = false,
            };
            table.Columns.Add(column);
        }

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

        /// <summary>
        /// 游戏文本文件数量
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <returns>数量</returns>
        private int GameTextFilesCount(DirectoryInfo baseDir)
        {
            int count = 0;
            foreach (EnumLanguage lang in Enum.GetValues(typeof(EnumLanguage)))
            {
                string path;
                path = string.Format("{0}\\{1}", baseDir.FullName, TextFilePath(lang, EnumGameTextFile.GameStrings));
                if (File.Exists(path)) count++;
                path = string.Format("{0}\\{1}", baseDir.FullName, TextFilePath(lang, EnumGameTextFile.ObjectStrings));
                if (File.Exists(path)) count++;
                path = string.Format("{0}\\{1}", baseDir.FullName, TextFilePath(lang, EnumGameTextFile.TriggerStrings));
                if (File.Exists(path)) count++;
            }
            return count;
        }

        #endregion

        #region 加载

        /// <summary>
        /// 获取Mod的文本文件
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        public void LoadGameTextFile(DirectoryInfo baseDir)
        {
            LangaugeList = new List<EnumLanguage>();
            foreach (EnumLanguage lang in Enum.GetValues(typeof(EnumLanguage)))
            {
                if (ExistGameTextFileOfLanguage(baseDir, lang))
                {
                    GetLanguageRow(lang);
                    LangaugeList.Add(lang);
                }
            }
            foreach (EnumLanguage lang in LangaugeList)
            {
                LoadGameTextFile(baseDir, lang, EnumGameTextFile.GameStrings);
                LoadGameTextFile(baseDir, lang, EnumGameTextFile.ObjectStrings);
                LoadGameTextFile(baseDir, lang, EnumGameTextFile.TriggerStrings);
            }
        }

        /// <summary>
        /// 验证待翻译语言文件存在
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <param name="lang">语言</param>
        /// <returns>验证结果</returns>
        private bool ExistGameTextFileOfLanguage(DirectoryInfo baseDir, EnumLanguage lang)
        {
            string path;
            path = string.Format("{0}\\{1}", baseDir.FullName, TextFilePath(lang, EnumGameTextFile.GameStrings));
            if (File.Exists(path)) return true;
            path = string.Format("{0}\\{1}", baseDir.FullName, TextFilePath(lang, EnumGameTextFile.ObjectStrings));
            if (File.Exists(path)) return true;
            path = string.Format("{0}\\{1}", baseDir.FullName, TextFilePath(lang, EnumGameTextFile.TriggerStrings));
            if (File.Exists(path)) return true;
            return false;
        }

        /// <summary>
        /// 加载文本
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <param name="lang">语言</param>
        /// <param name="file">文件类型</param>
        private bool LoadGameTextFile(DirectoryInfo baseDir, EnumLanguage lang, EnumGameTextFile file)
        {
            string name = TextFilePath(lang, file);
            string path = string.Format("{0}\\{1}", baseDir.FullName, name);
            string line;
            string key;
            string value;
            int length;
            if (!File.Exists(path)) return false;
            Globals.MainWindow.ProgressBarUpadte(1, name, null);
            StreamReader sr = new StreamReader(path);
            DataRow rowValue;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.StartsWith("//")) continue;
                length = line.IndexOf("=");
                key = line.Substring(0, length++);
                value = line.Substring(length);
                rowValue = SetTextValue(lang, file, key, value);
            }

            sr.Close();
            return true;
        }

        #endregion

        #region 数据

        /// <summary>
        /// 设置语言数据
        /// </summary>
        /// <param name="lang">语言</param>
        /// <returns>语言对应的数据行</returns>
        public DataRow GetLanguageRow(EnumLanguage lang)
        {
            DataTable table = Tables[TN_Language];
            DataRow row = table.Rows.Find(lang);
            if (row == null)
            {
                row = table.NewRow();
                row[RN_Language_ID] = lang;
                table.Rows.Add(row);
            }
            return row;
        }

        /// <summary>
        /// 设置游戏文本数据
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public DataRow GetGameTextRow(EnumGameTextFile file, string key)
        {
            DataTable table = Tables[TN_GameText];
            DataRow row = table.Rows.Find(key);
            if (row == null)
            {
                row = table.NewRow();
                row[RN_GameText_ID] = key;
                row[RN_GameText_File] = file;
                table.Rows.Add(row);
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
        public DataRow SetTextValue(EnumLanguage lang, EnumGameTextFile file, string key, string value)
        {
            DataRow row = GetGameTextRow(file, key);
            row[GetGameTextNameForLanguage(lang, RN_GameText_TextStatus)] = EnumGameTextStatus.Normal;
            row[GetGameTextNameForLanguage(lang, RN_GameText_UseStatus)] = EnumGameUseStatus.Normal;
            row[GetGameTextNameForLanguage(lang, RN_GameText_DropedText)] = value;
            row[GetGameTextNameForLanguage(lang, RN_GameText_SourceText)] = value;
            row[GetGameTextNameForLanguage(lang, RN_GameText_EditedText)] = value;
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
            Tables[TN_GalaxyLocation].Rows.Clear();
            Tables[TN_GalaxyLine].Rows.Clear();
            Tables[TN_GalaxyFile].Rows.Clear();
        }

        /// <summary>
        /// 递归获取Mod的Galaxy文件
        /// </summary>
        /// <param name="parentDir">父级目录</param>
        /// <param name="files">Galaxy文件</param>
        private void GetGalaxyFiles(DirectoryInfo parentDir, ref List<FileInfo> files)
        {
            foreach (FileInfo file in parentDir.GetFiles())
            {
                if (file.Extension.ToLower() == ".galaxy")
                {
                    files.Add(file);
                }
            }
            foreach (DirectoryInfo select in parentDir.GetDirectories())
            {
                GetGalaxyFiles(select, ref files);
            }
        }

        /// <summary>
        /// 处理Galaxy文件
        /// </summary>
        /// <param name="files">Galaxy文件</param>
        private void LoadGalaxyFile(List<FileInfo> files)
        {
#if !DEBUG
            try
#endif
            {
                foreach (FileInfo file in files)
                {
                    Globals.MainWindow.ProgressBarUpadte(1, file.Name, null);
                    LoadGalaxyFile(file);
                }
            }
#if !DEBUG
            catch (Exception e)
            {
                //Log.DisplayLogOnUI(e.Message);
                MessageBox.Show(e.Message);
            }
#endif
        }

        /// <summary>
        /// 加载Galaxy文件
        /// </summary>
        /// <param name="file">Galaxy文件</param>
        private void LoadGalaxyFile(FileInfo file)
        {
            string path = file.FullName.Substring(SC2Components.DirectoryName.Length);
            DataRow row = Tables[TN_GalaxyFile].NewRow();
            row[RN_GalaxyFile_Path] = path;
            row[RN_GalaxyFile_Name] = file.Name;
            Tables[TN_GalaxyFile].Rows.Add(row);
            int count = 0;
            DataTable tableLine = Tables[TN_GalaxyLine];
            DataTable tableLocation = Tables[TN_GalaxyLocation];
            int indexLine = tableLine.Rows.Count;
            int indexLocation = tableLocation.Rows.Count;
            StreamReader sr = new StreamReader(file.FullName);
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
