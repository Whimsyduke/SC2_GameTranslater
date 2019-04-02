using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SC2_GameTranslater.Source
{
    using Globals = Class_Globals;
    using Threads = Class_Threads;
    using Log = Class_Log;

    #region 枚举声明

    /// <summary>
    /// 文本状态
    /// </summary>
    [Flags]
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
        /// 已使用
        /// </summary>
        Useable = 6,
        /// <summary>
        /// 全部复选
        /// </summary>
        All = 7,
    }

    /// <summary>
    /// 使用状态
    /// </summary>
    [Flags]
    public enum EnumGameUseStatus
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 1,
        /// <summary>
        /// 过期
        /// </summary>
        Droped = 2,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 4,
        /// <summary>
        /// 新增
        /// </summary>
        Added = 8,
        /// <summary>
        /// 修改
        /// </summary>
        Modified = 16,
        /// <summary>
        /// 已使用
        /// </summary>
        Useable = 28,
        /// <summary>
        /// 全部
        /// </summary>
        All = 31,
    }


    /// <summary>
    /// 文本文件
    /// </summary>
    [Flags]
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

        #region 子类声明

        /// <summary>
        /// 游戏文本DataRow比较类
        /// </summary>
        /// <typeparam name="TRow">DataRow</typeparam>
        public class GameTextComparer<TRow> : IEqualityComparer<TRow>
        {
            #region 属性

            /// <summary>
            /// 默认比较器
            /// </summary>
            public static GameTextComparer<TRow> Default { get; } = new GameTextComparer<TRow>();

            #endregion

            #region 方法

            /// <summary>
            /// 比较函数
            /// </summary>
            /// <param name="x">行x</param>
            /// <param name="y">行y</param>
            /// <returns>比较结果</returns>
            bool IEqualityComparer<TRow>.Equals(TRow x, TRow y)
            {
                return (x as DataRow)?[RN_GameText_ID] as string == (y as DataRow)?[RN_GameText_ID] as string;
            }

            /// <summary>
            /// 获取哈希值
            /// </summary>
            /// <param name="obj">获取对象</param>
            /// <returns>哈希值</returns>
            int IEqualityComparer<TRow>.GetHashCode(TRow obj)
            {
                return 0;
            }

            #endregion

        }

        #endregion

        #region 正则表达式常量
        /// <summary>
        /// Galaxy文本函数
        /// </summary>
        public static Regex Const_Regex_StringExternal = new Regex("((?<=StringExternal\\(\")[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*(?=\"\\)))", RegexOptions.Compiled);
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
        public const string TN_GameTextForLanguage = "Table_GameTextForLanguage";

        #endregion

        #region 列名

        public const string RN_ModInfo_ProjectInfo = "ProjectInfo";
        public const string RN_ModInfo_CompontentsPath = "CompontentsPath";
        public const string RN_ModInfo_SearchConfig = "SearchConfig";
        public const string RN_Language_ID = "ID";
        public const string RN_Language_Name = "Name";
        public const string RN_Language_Status = "Status";
        public const string RN_GalaxyFile_Path = "Path";
        public const string RN_GalaxyFile_Name = "Name";
        public const string RN_GalaxyFile_Count = "Count";
        public const string RN_GalaxyLine_Index = "Index";
        public const string RN_GalaxyLine_File = "File";
        public const string RN_GalaxyLine_LineNumber = "LineNumber";
        public const string RN_GalaxyLine_Script = "Script";
        public const string RN_GalaxyLocation_Index = "Index";
        public const string RN_GalaxyLocation_Line = "Line";
        public const string RN_GalaxyLocation_LineIndex = "LineIndex";
        public const string RN_GalaxyLocation_Key = "Key";
        public const string RN_GameText_ID = "ID";
        public const string RN_GameText_Index = "Index";
        public const string RN_GameText_File = "File";
        public const string RN_GameText_TextStatus = "TextStatus";
        public const string RN_GameText_UseStatus = "UseStatus";
        public const string RN_GameText_DropedText = "Droped";
        public const string RN_GameText_SourceText = "Source";
        public const string RN_GameText_EditedText = "Edited";
        public const string RN_GameTextForLanguage_Language = "Language";
        public const string RN_GameTextForLanguage_TextStatus = "TextStatus";
        public const string RN_GameTextForLanguage_UseStatus = "UseStatus";
        public const string RN_GameTextForLanguage_DropedText = "Droped";
        public const string RN_GameTextForLanguage_SourceText = "Source";
        public const string RN_GameTextForLanguage_EditedText = "Edited";

        #endregion

        #region 关系

        public const string RSN_GalaxyLine_GameLocation_Line = "Relation_GalaxyLine_GameLocation_Line";
        public const string RSN_GalaxyFile_GalaxyLine_File = "Relation_GalaxyFile_GalaxyLine_File";
        public const string RSN_GameText_GalaxyLocation_Key = "Relation_GameText_GalaxyLocation_Key";

        #endregion


        #endregion

        #region 其他

        public const string PATH_TempFolder = "Temp\\";
        public const string PATH_BackupFolder = "Backup\\";

        #endregion

        #endregion

        #region 属性字段

        /// <summary>
        /// 文本状态对应颜色
        /// </summary>
        public static Dictionary<EnumGameTextStatus, Brush> TextStatusColor { get; } = new Dictionary<EnumGameTextStatus, Brush>()
        {
            { EnumGameTextStatus.Empty, Brushes.Red },
            { EnumGameTextStatus.Normal, Brushes.Green },
            { EnumGameTextStatus.Modified, Brushes.Yellow },
        };

        /// <summary>
        /// 使用状态对应颜色
        /// </summary>
        public static Dictionary<EnumGameUseStatus, Brush> UseStatusColor { get; } = new Dictionary<EnumGameUseStatus, Brush>()
        {
            { EnumGameUseStatus.None, Brushes.Red },
            { EnumGameUseStatus.Droped, Brushes.Magenta },
            { EnumGameUseStatus.Normal, Brushes.Green },
            { EnumGameUseStatus.Added, Brushes.Blue },
            { EnumGameUseStatus.Modified, Brushes.Yellow },
        };

        /// <summary>
        /// 组件文件夹路径
        /// </summary>
        public FileInfo SC2Components
        {
            set
            {
                mComponentsPath = value;
                WriteCompontentsPath(value.FullName);
                Globals.MainWindow.TextBox_ComponentsPath.Text = value.FullName;
            }
            get => mComponentsPath;
        }
        private FileInfo mComponentsPath;

        /// <summary>
        /// 使用语言列表
        /// </summary>
        public List<EnumLanguage> LangaugeList { get; private set; }

        /// <summary>
        /// 语言数据
        /// </summary>
        public EnumerableRowCollection<DataRow> LangaugeRowList => Tables[TN_Language].AsEnumerable();

        /// <summary>
        /// 项目信息行
        /// </summary>
        public DataRow ProjectInfoRow
        {
            get
            {
                if (Tables[TN_ProjectInfo].Rows.Count == 0)
                {
                    Tables[TN_ProjectInfo].Rows.Add(Globals.ProjectInfoVersion, "", null);
                }
                return Tables[TN_ProjectInfo].Rows[0];
            }
        }

        /// <summary>
        /// 游戏文本单语言数据表
        /// </summary>
        public static DataTable GameTextForLanguageTable { get; } = NewGameTextForLanguageTable();

        #endregion

        #region 方法

        #region 通用

        /// <summary>
        /// 获取语言对应列名
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="name">基本名称</param>
        /// <returns>列名</returns>
        public static string GetRowNameForLanguage(EnumLanguage language, string name)
        {
            return $"{Globals.GetEnumLanguageName(language)}_{name}";
        }

        /// <summary>
        /// 获取枚举值对应的文本
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>翻译名称</returns>
        public static string GetEnumNameInLanguage(Enum value)
        {
            string key = $"ENUM_{value.GetType().Name}_{Enum.GetName(value.GetType(), value)}";
            return Globals.GetStringFromCurrentLanguage(key);
        }

        /// <summary>
        /// 生成语言对应的DataColumn配置
        /// </summary>
        /// <param name="language">语言</param>
        private void GenerateDataColumnForLanguage(EnumLanguage language)
        {
            DataTable table = Tables[TN_GameText];

            // Text Status
            string columnName = GetRowNameForLanguage(language, RN_GameText_TextStatus);
            DataColumn column = new DataColumn(columnName, typeof(Int32), "", MappingType.Attribute)
            {
                Caption = columnName,
                DefaultValue = EnumGameTextStatus.Empty,
                AllowDBNull = false,
                Unique = false,
            };
            table.Columns.Add(column);

            // Use Status
            columnName = GetRowNameForLanguage(language, RN_GameText_UseStatus);
            column = new DataColumn(columnName, typeof(Int32), "", MappingType.Attribute)
            {
                Caption = columnName,
                DefaultValue = EnumGameUseStatus.None,
                AllowDBNull = false,
                Unique = false,
            };
            table.Columns.Add(column);

            // Drop Text
            columnName = GetRowNameForLanguage(language, RN_GameText_DropedText);
            column = new DataColumn(columnName, typeof(string), "", MappingType.Element)
            {
                Caption = columnName,
                DefaultValue = null,
                AllowDBNull = true,
                Unique = false,
            };
            table.Columns.Add(column);

            // Source Text
            columnName = GetRowNameForLanguage(language, RN_GameText_SourceText);
            column = new DataColumn(columnName, typeof(string), "", MappingType.Element)
            {
                Caption = columnName,
                DefaultValue = null,
                AllowDBNull = true,
                Unique = false,
            };
            table.Columns.Add(column);

            // Edited Text
            columnName = GetRowNameForLanguage(language, RN_GameText_EditedText);
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
        /// <param name="language">语言</param>
        /// <param name="file">文件类型</param>
        /// <returns>路径</returns>
        public string TextFilePath(EnumLanguage language, EnumGameTextFile file)
        {
            string path = Globals.GetEnumLanguageName(language) + Const_String_GameTextMainPath;
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
                    Log.Assert(Globals.CurrentProject != null, "");
                    break;
            }
            return path;
        }

        /// <summary>
        /// 游戏文本文件数量
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <returns>数量</returns>
        private int GameTextFilesCount(DirectoryInfo baseDir)
        {
            int count = 0;
            foreach (EnumLanguage language in Globals.AllLanguage)
            {
                string path = $"{baseDir.FullName}\\{TextFilePath(language, EnumGameTextFile.GameStrings)}";
                if (File.Exists(path)) count++;
                path = $"{baseDir.FullName}\\{TextFilePath(language, EnumGameTextFile.ObjectStrings)}";
                if (File.Exists(path)) count++;
                path = $"{baseDir.FullName}\\{TextFilePath(language, EnumGameTextFile.TriggerStrings)}";
                if (File.Exists(path)) count++;
            }
            return count;
        }

        /// <summary>
        /// 新建游戏文本单语言数据表
        /// </summary>
        /// <returns>数据表</returns>
        private static DataTable NewGameTextForLanguageTable()
        {
            DataTable table = new DataTable(TN_GameTextForLanguage);
            DataColumn column = new DataColumn(RN_GameTextForLanguage_Language, typeof(int));
            table.Columns.Add(column);
            column = new DataColumn(RN_GameTextForLanguage_TextStatus, typeof(int));
            table.Columns.Add(column);
            column = new DataColumn(RN_GameTextForLanguage_UseStatus, typeof(int));
            table.Columns.Add(column);
            column = new DataColumn(RN_GameTextForLanguage_DropedText, typeof(string));
            table.Columns.Add(column);
            column = new DataColumn(RN_GameTextForLanguage_SourceText, typeof(string));
            table.Columns.Add(column);
            column = new DataColumn(RN_GameTextForLanguage_EditedText, typeof(string));
            table.Columns.Add(column);
            return table;
        }

        /// <summary>
        /// 查找数据编号对应的ID
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="index">编号</param>
        /// <returns>ID</returns>
        public static string FindIDByDataIndex(DataTable table, int index)
        {
            List<DataRow> rows = table.AsEnumerable().Where(r => (int)r[RN_GameText_Index] == index).Select(r => r).ToList();
            if (rows.Count == 0)
            {
                return null;
            }
            else
            {
                return rows.First()[RN_GameText_ID] as string;
            }
        }

        #endregion

        #region 基类

        public new void EndInit()
        {
            base.EndInit();

            foreach (EnumLanguage language in Globals.AllLanguage)
            {
                GenerateDataColumnForLanguage(language);
            }
        }

        #endregion

        #region 数据

        /// <summary>
        /// 复制翻译文本
        /// </summary>
        /// <param name="from">复制来源</param>
        /// <param name="to">复制目标</param>
        public void CopyTranslateText(EnumLanguage from, EnumLanguage to)
        {
            string textStatusFromKey = GetRowNameForLanguage(from, RN_GameText_TextStatus);
            string textStatusToKey = GetRowNameForLanguage(to, RN_GameText_TextStatus);
            string useStatusFromKey = GetRowNameForLanguage(from, RN_GameText_UseStatus);
            string useStatusToKey = GetRowNameForLanguage(to, RN_GameText_UseStatus);
            string editedFromKey = GetRowNameForLanguage(from, RN_GameText_EditedText);
            string editedToKey = GetRowNameForLanguage(to, RN_GameText_EditedText);
            foreach (DataRow row in Tables[TN_GameText].Rows)
            {
                row[textStatusToKey] = row[textStatusFromKey];
                row[useStatusToKey] = row[useStatusFromKey];
                row[editedToKey] = row[editedFromKey];
            }
        }

        /// <summary>
        /// 重载语言配置
        /// </summary>
        /// <param name="oldProject">旧项目</param>
        private void ReloadLanguageStatus(Data_GameText oldProject)
        {
            EnumerableRowCollection<DataRow> newRows = Tables[TN_Language].AsEnumerable();
            EnumerableRowCollection<DataRow> oldRows = oldProject.Tables[TN_Language].AsEnumerable();
            IEnumerable<DataRow> dropRows = oldRows.Except(newRows, DataRowComparer.Default);
            IEnumerable<DataRow> addRows = newRows.Except(oldRows, DataRowComparer.Default);
            foreach (DataRow row in dropRows)
            {
                row[RN_Language_Status] = EnumGameUseStatus.Droped;
                Tables[TN_Language].ImportRow(row);
            }
            foreach (DataRow row in addRows)
            {
                row[RN_Language_Status] = EnumGameUseStatus.Added;
            }
        }

        /// <summary>
        /// 批量设置使用状态为新增
        /// </summary>
        /// <param name="oldProject">旧项目</param>
        private void ReloadSourceText(Data_GameText oldProject)
        {
            // GetRows
            DataTable table = Tables[TN_GameText];
            EnumerableRowCollection<DataRow> newRows = table.AsEnumerable();
            EnumerableRowCollection<DataRow> oldRows = oldProject.Tables[TN_GameText].AsEnumerable();
            List<DataRow> dropRows = oldRows.Except(newRows, GameTextComparer<DataRow>.Default).ToList();
            List<DataRow> addRows = newRows.Except(oldRows, GameTextComparer<DataRow>.Default).ToList();
            List<DataRow> normalRows = newRows.Except(addRows, GameTextComparer<DataRow>.Default).ToList();
            foreach (DataRow row in dropRows)
            {
                table.ImportRow(row);
            }
            dropRows = table.AsEnumerable().Except(normalRows, GameTextComparer<DataRow>.Default).Except(addRows, GameTextComparer<DataRow>.Default).ToList();

            // SetStatuse
            foreach (DataRow langRow in LangaugeRowList)
            {
                EnumLanguage language = (EnumLanguage)langRow[RN_Language_ID];
                string dropKey = GetRowNameForLanguage(language, RN_GameText_DropedText);
                string srcKey = GetRowNameForLanguage(language, RN_GameText_SourceText);
                string statusKey = GetRowNameForLanguage(language, RN_GameText_UseStatus);
                string text;
                switch ((EnumGameUseStatus)langRow[RN_Language_Status])
                {
                    case EnumGameUseStatus.Droped:
                        foreach (DataRow textRow in addRows)
                        {
                            textRow[statusKey] = EnumGameUseStatus.Droped;
                        }
                        foreach (DataRow textRow in normalRows)
                        {
                            DataRow oldRow = table.Rows.Find(textRow[RN_GameText_ID]);
                            text = oldRow[dropKey] as string;
                            textRow[dropKey] = text;
                            textRow[statusKey] = EnumGameUseStatus.Droped;
                        }
                        foreach (DataRow textRow in dropRows)
                        {
                            textRow[statusKey] = EnumGameUseStatus.Droped;
                        }
                        break;
                    case EnumGameUseStatus.Normal:
                        foreach (DataRow textRow in addRows)
                        {
                            textRow[statusKey] = EnumGameUseStatus.Added;
                        }
                        foreach (DataRow textRow in normalRows)
                        {
                            DataRow oldRow = table.Rows.Find(textRow[RN_GameText_ID]);
                            text = oldRow[dropKey] as string;
                            textRow[dropKey] = text;
                            textRow[statusKey] = (text == textRow[srcKey] as string) ? EnumGameUseStatus.Normal : EnumGameUseStatus.Modified;
                        }
                        foreach (DataRow textRow in dropRows)
                        {
                            textRow[statusKey] = EnumGameUseStatus.Droped;
                        }
                        break;
                    case EnumGameUseStatus.Added:
                        foreach (DataRow textRow in addRows)
                        {
                            textRow[statusKey] = EnumGameUseStatus.Added;
                        }
                        foreach (DataRow textRow in normalRows)
                        {
                            DataRow oldRow = table.Rows.Find(textRow[RN_GameText_ID]);
                            text = oldRow[dropKey] as string;
                            textRow[dropKey] = text;
                            textRow[statusKey] = EnumGameUseStatus.Added;
                        }
                        foreach (DataRow textRow in dropRows)
                        {
                            textRow[statusKey] = EnumGameUseStatus.Droped;
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// 重载项目数据
        /// </summary>
        /// <param name="oldProject">旧项目</param>
        public void ReloadProjectData(Data_GameText oldProject)
        {
            ReloadLanguageStatus(oldProject);
            ReloadSourceText(oldProject);
        }

        /// <summary>
        /// 重载翻译文本
        /// </summary>
        /// <param name="dataProject">数据项目</param>
        /// <param name="languages">翻译语言</param>
        /// <param name="onlyModified">仅修改内容</param>
        public void ReloadTranslationdText(Data_GameText dataProject, List<EnumLanguage> languages, bool onlyModified)
        {
            DataTable dataTable = dataProject.Tables[TN_GameText];
            DataTable targetTable = Tables[TN_GameText];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataRow targetRow = targetTable.Rows.Find(dataRow[RN_GameText_ID]);
                if (targetRow == null) continue;

                foreach (EnumLanguage language in languages)
                {
                    string keyStatus = GetRowNameForLanguage(language, RN_GameText_TextStatus);
                    if (onlyModified && (EnumGameTextStatus)dataRow[keyStatus] != EnumGameTextStatus.Modified) continue;
                    string keyEdited = GetRowNameForLanguage(language, RN_GameText_EditedText);
                    targetRow[keyStatus] = EnumGameTextStatus.Modified;
                    targetRow[keyEdited] = dataRow[keyEdited];
                }
            }
        }

        /// <summary>
        /// 写入组件文件夹路径
        /// </summary>
        /// <param name="path">路径</param>
        private void WriteCompontentsPath(string path)
        {
            ProjectInfoRow[RN_ModInfo_CompontentsPath] = path;
        }

        /// <summary>
        /// 刷新游戏文本单语言数据表
        /// </summary>
        /// <param name="textRow">语言数据</param>
        /// <param name="langList">语言列表</param>
        public static void RefreshGameTextForLanguageTable(DataRow textRow, List<EnumLanguage> langList)
        {
            GameTextForLanguageTable.Clear();
            if (textRow == null) return;
            foreach (EnumLanguage language in langList)
            {
                string key = GetRowNameForLanguage(language, RN_GameText_TextStatus);
                object dataTextStatus = textRow[key];
                key = GetRowNameForLanguage(language, RN_GameText_UseStatus);
                object dataUseStatus = textRow[key];
                key = GetRowNameForLanguage(language, RN_GameText_DropedText);
                object dataDropedText = textRow[key];
                key = GetRowNameForLanguage(language, RN_GameText_SourceText);
                object dataSourceText = textRow[key];
                key = GetRowNameForLanguage(language, RN_GameText_EditedText);
                object dataEditedText = textRow[key];
                GameTextForLanguageTable.Rows.Add(language, dataTextStatus, dataUseStatus, dataDropedText, dataSourceText, dataEditedText);
            }
        }

        #endregion

        #region 进程

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="argu"></param>
        public void ThreadInitialization(object argu)
        {
            KeyValuePair<FileInfo, SC2_GameTranslater_Window.Delegate_ProgressEvent> pair = (KeyValuePair<FileInfo, SC2_GameTranslater_Window.Delegate_ProgressEvent>)argu;
            FileInfo file = pair.Key;
            CleanGalaxyTable();

            List<FileInfo> galaxyFiles = new List<FileInfo>();
            GetGalaxyFiles(file.Directory, ref galaxyFiles);
            int max = galaxyFiles.Count + GameTextFilesCount(file.Directory);
            Globals.MainWindow.ProgressBarInit(max, "", this, null);

            LoadGalaxyFile(galaxyFiles);
            LoadGameTextFile(file.Directory);
            SortGameTextRow();

            Globals.MainWindow.ProgressBarClean(this, pair.Value);
        }

        /// <summary>
        /// 从Mod初始化数据集
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="func">结束回调</param>
        public void Initialization(FileInfo file, SC2_GameTranslater_Window.Delegate_ProgressEvent func)
        {
            Clear();
            Threads.StartThread(ThreadInitialization, new KeyValuePair<FileInfo, SC2_GameTranslater_Window.Delegate_ProgressEvent>(file, func), true);
            SC2Components = file;
        }

        #endregion

        #region 保存

        /// <summary>
        /// 生成搜索配置数据
        /// </summary>
        private void GenerateSearchConfigData()
        {
            byte[] data = Class_SearchConfig.Serializer(Class_SearchConfig.NewSearchConfig());
            ProjectInfoRow[RN_ModInfo_SearchConfig] = data;
        }

        /// <summary>
        /// 保存当前项目
        /// </summary>
        /// <param name="path">保存路径</param>
        public void SaveProject(FileInfo path)
        {
            GenerateSearchConfigData();
            Globals.ObjectSerializerCompression(path, this);
            Log.ShowSystemMessage(true, MessageBoxButton.OK, MessageBoxImage.None, "MSG_SaveProject", path.FullName);
        }

        #endregion

        #region 加载

        /// <summary>
        /// 获取项目文件版本号
        /// </summary>
        /// <returns></returns>
        public string GetProjectInfoVersion()
        {
            return ProjectInfoRow[RN_ModInfo_ProjectInfo] as string;
        }

        /// <summary>
        /// 使用搜索配置数据
        /// </summary>
        public void UseSerachConfigData()
        {
            if (ProjectInfoRow[RN_ModInfo_SearchConfig] is byte[] data)
            {
                Class_SearchConfig config = Class_SearchConfig.Deserialize(data);
                config.ApplyToUI(true);
            }
        }

        /// <summary>
        /// 加载项目
        /// </summary>
        /// <param name="path">路径</param>
        public static Data_GameText LoadProject(FileInfo path)
        {
            if (Globals.ObjectDeserializeDecompress(path) is Data_GameText proj)
            {
                string projectVer = proj.GetProjectInfoVersion();
                if (projectVer != Globals.ProjectInfoVersion)
                {
                    Log.ShowSystemMessage(true, MessageBoxButton.OK, MessageBoxImage.None, "MSG_ProjectFileVersionMismatched", projectVer, Globals.ProjectInfoVersion);
                }
                proj.RefreshAttribute();
                return proj;
            }
            return null;
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
                if (ProjectInfoRow[RN_ModInfo_CompontentsPath] is string path)
                {
                    SC2Components = new FileInfo(path);
                }

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
            if (SC2Components == null || !SC2Components.Exists)
            {
                Log.ShowSystemMessage(true, MessageBoxButton.OK, MessageBoxImage.None, "MSG_AcceptTranslateTextError");
                return false;
            }

            DirectoryInfo tempDir = new DirectoryInfo($"{PATH_TempFolder}{SC2Components.Directory.Name}\\");
            if (tempDir.Exists) tempDir.Delete(true);
            DirectoryInfo baseDir = SC2Components.Directory;
            List<FileInfo> backFiles = new List<FileInfo>();
            EnumerableRowCollection<DataRow> gameStringRows = GetGameTextRows(EnumGameTextFile.GameStrings);
            EnumerableRowCollection<DataRow> objectStringRows = GetGameTextRows(EnumGameTextFile.ObjectStrings);
            EnumerableRowCollection<DataRow> triggerStringRows = GetGameTextRows(EnumGameTextFile.TriggerStrings);
            foreach (DataRow row in LangaugeRowList)
            {
                EnumLanguage language = (EnumLanguage)row[RN_Language_ID];
                EnumGameUseStatus useStatus = (EnumGameUseStatus)row[RN_Language_Status];
                if (EnumGameUseStatus.Useable.HasFlag(useStatus))
                {
                    WriteToTextFile(baseDir, language, EnumGameTextFile.GameStrings, ref backFiles, gameStringRows);
                    WriteToTextFile(baseDir, language, EnumGameTextFile.ObjectStrings, ref backFiles, objectStringRows);
                    WriteToTextFile(baseDir, language, EnumGameTextFile.TriggerStrings, ref backFiles, triggerStringRows);
                }
            }
            Log.ShowSystemMessage(true, MessageBoxButton.OK, MessageBoxImage.None, "MSG_AcceptTranslateText", SC2Components.Directory, tempDir.FullName);
            return true;
        }

        /// <summary>
        /// 写入文本文件数据
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <param name="language">语言</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="backFiles">备份文件列表</param>
        /// <param name="data">文件数据</param>
        /// <returns>写入成功</returns>
        private bool WriteToTextFile(DirectoryInfo baseDir, EnumLanguage language, EnumGameTextFile fileType, ref List<FileInfo> backFiles, EnumerableRowCollection<DataRow> data)
        {
#if !DEBUG
            try
#endif
            {
                string backPath = TextFilePath(language, fileType);
                string originpath = $"{baseDir.FullName}\\{backPath}";
                if (!File.Exists(originpath)) return true;
                FileInfo backFile = new FileInfo($"{PATH_TempFolder}{SC2Components.Directory.Name}\\{backPath}");
                backFiles.Add(backFile);
                if (backFile.Directory != null && !backFile.Directory.Exists) backFile.Directory.Create();
                File.Copy(originpath, backFile.FullName, true);
                StreamWriter sw = new StreamWriter(originpath, false);
                string statusKey = GetRowNameForLanguage(language, RN_GameText_TextStatus);
                string useKey = GetRowNameForLanguage(language, RN_GameText_UseStatus);
                string editedKey = GetRowNameForLanguage(language, RN_GameText_EditedText);
                foreach (DataRow row in data)
                {
                    if (EnumGameUseStatus.Useable.HasFlag((EnumGameUseStatus)row[useKey]) && EnumGameTextStatus.Useable.HasFlag((EnumGameTextStatus)row[statusKey]))
                    {
                        sw.WriteLine("{0}={1}", row[RN_GameText_ID], row[editedKey]);
                    }
                }
                sw.Close();
                return true;
            }
#if !DEBUG
            catch
            {
                return false;
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

        #region 读取

        #region 文本

        #region 加载

        /// <summary>
        /// 获取Mod的文本文件
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        public void LoadGameTextFile(DirectoryInfo baseDir)
        {
            LangaugeList = new List<EnumLanguage>();
            foreach (EnumLanguage language in Globals.AllLanguage)
            {
                if (ExistGameTextFileOfLanguage(baseDir, language))
                {
                    GetLanguageRow(language);
                    LangaugeList.Add(language);
                }
            }
            foreach (EnumLanguage language in LangaugeList)
            {
                LoadGameTextFile(baseDir, language, EnumGameTextFile.GameStrings);
                LoadGameTextFile(baseDir, language, EnumGameTextFile.ObjectStrings);
                LoadGameTextFile(baseDir, language, EnumGameTextFile.TriggerStrings);
            }
        }

        /// <summary>
        /// 验证待翻译语言文件存在
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <param name="language">语言</param>
        /// <returns>验证结果</returns>
        private bool ExistGameTextFileOfLanguage(DirectoryInfo baseDir, EnumLanguage language)
        {
            string path = $"{baseDir.FullName}\\{TextFilePath(language, EnumGameTextFile.GameStrings)}";
            if (File.Exists(path)) return true;
            path = $"{baseDir.FullName}\\{TextFilePath(language, EnumGameTextFile.ObjectStrings)}";
            if (File.Exists(path)) return true;
            path = $"{baseDir.FullName}\\{TextFilePath(language, EnumGameTextFile.TriggerStrings)}";
            if (File.Exists(path)) return true;
            return false;
        }

        /// <summary>
        /// 加载文本
        /// </summary>
        /// <param name="baseDir">基础目录</param>
        /// <param name="language">语言</param>
        /// <param name="file">文件类型</param>
        private bool LoadGameTextFile(DirectoryInfo baseDir, EnumLanguage language, EnumGameTextFile file)
        {
            string name = TextFilePath(language, file);
            string path = $"{baseDir.FullName}\\{name}";
            if (!File.Exists(path)) return false;
            Globals.MainWindow.ProgressBarUpadte(1, name, this, null);
            StreamReader sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (string.IsNullOrEmpty(line) || line.StartsWith("//")) continue;
                int length = line.IndexOf("=", StringComparison.Ordinal);
                string key = line.Substring(0, length++);
                string value = line.Substring(length);
                SetTextValue(language, file, key, value);
            }

            sr.Close();
            return true;
        }

        #endregion

        #region 数据

        /// <summary>
        /// 设置语言数据
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>语言对应的数据行</returns>
        public DataRow GetLanguageRow(EnumLanguage language)
        {
            DataTable table = Tables[TN_Language];
            DataRow row = table.Rows.Find(language);
            if (row == null)
            {
                row = table.NewRow();
                row[RN_Language_ID] = language;
                row[RN_Language_Name] = Globals.GetEnumLanguageName(language);
                row[RN_Language_Status] = EnumGameUseStatus.Normal;
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
        /// <param name="language">语言</param>
        /// <param name="file">文件</param>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <returns>文本行</returns>
        public DataRow SetTextValue(EnumLanguage language, EnumGameTextFile file, string key, string value)
        {
            DataRow row = GetGameTextRow(file, key);
            row[GetRowNameForLanguage(language, RN_GameText_TextStatus)] = EnumGameTextStatus.Normal;
            row[GetRowNameForLanguage(language, RN_GameText_UseStatus)] = EnumGameUseStatus.Normal;
            row[GetRowNameForLanguage(language, RN_GameText_DropedText)] = value;
            row[GetRowNameForLanguage(language, RN_GameText_SourceText)] = value;
            row[GetRowNameForLanguage(language, RN_GameText_EditedText)] = value;
            return row;
        }

        #endregion

        #region 排序

        /// <summary>
        /// 排序游戏文本
        /// </summary>
        public void SortGameTextRow()
        {
            DataTable table = Tables[TN_GameText];
            EnumerableRowCollection<DataRow> rows = table.AsEnumerable();
            rows = rows.OrderBy(GetTextInGalaxyFileSortValue).ThenBy(GetTextInGalaxyLineNumberSortValue).ThenBy(GetTextInIndexSortValue).Select(r => r);
            int index = 0;
            foreach (DataRow row in rows)
            {
                row[RN_GameText_Index] = ++index;
            }
        }

        /// <summary>
        /// 获取Galaxy文件排序(文件名)
        /// </summary>
        /// <param name="row">被排序行</param>
        /// <returns>文件名</returns>
        private string GetTextInGalaxyFileSortValue(DataRow row)
        {
            DataRow[] locations = row.GetChildRows(RSN_GameText_GalaxyLocation_Key);
            if (!locations.Any()) return "";
            DataRow line = locations[0].GetParentRow(RSN_GalaxyLine_GameLocation_Line);
            return line[RN_GalaxyLine_File] as string;
        }

        /// <summary>
        /// 获取Galaxy文件排序(行号)
        /// </summary>
        /// <param name="row">被排序行</param>
        /// <returns>行号</returns>
        private int GetTextInGalaxyLineNumberSortValue(DataRow row)
        {
            DataRow[] locations = row.GetChildRows(RSN_GameText_GalaxyLocation_Key);
            if (!locations.Any()) return 0x7FFFFFFF;
            DataRow line = locations[0].GetParentRow(RSN_GalaxyLine_GameLocation_Line);
            return (int)line[RN_GalaxyLine_LineNumber];
        }

        /// <summary>
        /// 获取Galaxy文件排序（行内序号）
        /// </summary>
        /// <param name="row">被排序行</param>
        /// <returns>行号</returns>
        private int GetTextInIndexSortValue(DataRow row)
        {
            DataRow[] locations = row.GetChildRows(RSN_GameText_GalaxyLocation_Key);
            if (!locations.Any()) return 0x7FFFFFFF;
            return (int)locations[0][RN_GalaxyLocation_LineIndex];
        }
        #endregion

        #endregion

        #region 脚本

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
                    Globals.MainWindow.ProgressBarUpadte(1, file.Name, this, null);
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
            Log.Assert(SC2Components.DirectoryName != null, "SC2Components.DirectoryName != null");
            if (SC2Components.DirectoryName == null) return;
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
            int lineNumber = 0;
            while (!sr.EndOfStream)
            {
                lineNumber++;
                var line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    MatchCollection matchs = Const_Regex_StringExternal.Matches(line);
                    if (matchs.Count == 0) continue;
                    tableLine.Rows.Add(indexLine, path, lineNumber, line);
                    int lineIndex = 0;
                    foreach (var select in matchs)
                    {
                        count++;
                        tableLocation.Rows.Add(indexLocation++, indexLine, lineIndex++, select.ToString());
                    }
                    indexLine++;
                }
            }
            sr.Close();
            row[RN_GalaxyFile_Count] = count;
        }

        /// <summary>
        /// 获取应用的Galaxyline
        /// </summary>
        /// <param name="textRow">文本Row</param>
        /// <returns>相关row列表</returns>
        public DataView GetRelateGalaxyLineRows(DataRow textRow)
        {
            DataRow[] locations = textRow.GetChildRows(RSN_GameText_GalaxyLocation_Key);
            List<DataRow> lines = locations.Select(r => r.GetParentRow(RSN_GalaxyLine_GameLocation_Line)).Distinct().Select(r => r).ToList();
            EnumerableRowCollection<DataRow> query = Tables[TN_GalaxyLine].AsEnumerable();
            query = query.Where(r => lines.Contains(r)).Select(r => r);
            return query.AsDataView();
        }

        #endregion

        #endregion

        #endregion
    }
}
