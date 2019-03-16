using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SC2_GameTranslater.Source
{
    using Globals = SC2_GameTranslater.Source.Class_Globals;

    /// <summary>
    /// 配置文件类
    /// </summary>
    [XmlRoot(Preference_ElementConfig)]
    public class Class_Preference
    {
        #region 常量

        // 默认值
        public static readonly FileInfo Preference_ConfigFile = new FileInfo("Config.cfg");
        public static readonly Size Preference_DefaultWindowSize = new Size(1366, 768);
        public static readonly string Preference_DefaultLastFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly int Prefrence_MaxRecentProjectCount = 20;

        // 字段
        public const string Preference_ElementConfig = "Preference";
        public const string Preference_AttributeMajorVer = "Major";
        public const string Preference_AttributeMinorVer = "Minor";
        public const string Preference_AttributeBuildVer = "Build";
        public const string Preference_AttributeRevisedVer = "Revised";
        public const string Preference_AttributeWidth = "Width";
        public const string Preference_AttributeHeight = "Height";
        public const string Preference_AttributeLastFolderPath = "LastFolder";
        public const string Preference_ElementRecentProjectList = "RecentProjectList";
        public const string Preference_AttributeRecentProject = "RecentProject";
        public const string Preference_AttributeColumnVisiblility = "ColumnVisiblility";

        #endregion

        #region 属性字段

        #region 属性

        /// <summary>
        /// 主版本号
        /// </summary>
        [XmlAttribute(Preference_AttributeMajorVer)]
        public int MajorVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.Major;

        /// <summary>
        /// 次版本号
        /// </summary>
        [XmlAttribute(Preference_AttributeMinorVer)]
        public int MinorVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.Minor;

        /// <summary>
        /// 编译版本号
        /// </summary>
        [XmlAttribute(Preference_AttributeBuildVer)]
        public int BuildVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.Build;

        /// <summary>
        /// 修订本号
        /// </summary>
        [XmlAttribute(Preference_AttributeRevisedVer)]
        public int RevisedVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.Revision;

        /// <summary>
        /// 窗口宽度
        /// </summary>
        [XmlAttribute(Preference_AttributeWidth)]
        public double WindowWidth { get; set; } = Preference_DefaultWindowSize.Width;

        /// <summary>
        /// 窗口高度
        /// </summary>
        [XmlAttribute(Preference_AttributeHeight)]
        public double WindowHeight { get; set; } = Preference_DefaultWindowSize.Height;

        /// <summary>
        /// 最后打开保存文件路径
        /// </summary>
        [XmlAttribute(Preference_AttributeLastFolderPath)]
        public string LastFolderPath { get; set; } = Preference_DefaultLastFolderPath;

        /// <summary>
        /// 最近打开文件列表
        /// </summary>
        [XmlArray(Preference_ElementRecentProjectList), XmlArrayItem(Preference_AttributeRecentProject)]

        public string [] RecentProjectList { get; set; } = new string[Prefrence_MaxRecentProjectCount];

        /// <summary>
        /// 最后打开保存文件路径
        /// </summary>
        [XmlArray(Preference_AttributeColumnVisiblility)]
        public bool [] ColumnVisiblity { get; set; } =
        {
            #region DataGrid_TranslatedTexts

            true,  // MenuItem_TextID
            true,  // MenuItem_Index
            true,  // MenuItem_TextFile
            true,  // MenuItem_TextStatus
            false, // MenuItem_UseStatus
            false, // MenuItem_DropedText
            true,  // MenuItem_SourceText
            true,  // MenuItem_EditedText

            #endregion
            
            #region GroupBox_GameTextForLanguage

            true,  // MenuItem_Language
            true,  // MenuItem_TextStatus
            false,  // MenuItem_UseStatus
            false,  // MenuItem_DropedText
            true,  // MenuItem_SourceText
            true,  // MenuItem_EditedText

            #endregion
        };

        #endregion

        #endregion

        #region 方法

        #region 生成加载存储

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SavePreference()
        {
            Globals.Preference.Preference_SaveWindowSize();
            Globals.Preference.Preference_SaveColumnVisibility();
            SerializerCompression();

        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public void LoadPreference()
        {
            if (!Preference_ConfigFile.Exists) return;
#if !DEBUG
            try
#endif
            {
                DeserializeDecompress();
            }
#if !DEBUG
            catch
            {
                return ;
            }
            finally
#endif
            {
                Globals.Preference.Preference_LoadWindowSize();
                Globals.Preference.Preference_LoadColumnVisibility();
                Globals.MainWindow.RefreshRecentProjects();
            }
        }



        #endregion

        #region 配置数据

        #region 窗口大小

        /// <summary>
        /// 保存窗口大小
        /// </summary>
        private void Preference_SaveWindowSize()
        {
            WindowWidth = Globals.MainWindow.Width;
            WindowHeight = Globals.MainWindow.Height;
        }

        /// <summary>
        /// 加载窗口大小
        /// </summary>
        public void Preference_LoadWindowSize()
        {
            Globals.MainWindow.Width = WindowWidth;
            Globals.MainWindow.Height = WindowHeight;
        }

        #endregion

        #region 列可见性

        /// <summary>
        /// 保存列可见性
        /// </summary>
        private void Preference_SaveColumnVisibility()
        {
            foreach (System.Collections.DictionaryEntry select in Globals.MainWindow.DataGrid_TranslatedTexts.Resources)
            {
                if (select.Value is MenuItem item && item.Tag is string value)
                {
                    ColumnVisiblity[int.Parse(value)] = item.IsChecked;
                }
            }
            foreach (System.Collections.DictionaryEntry select in Globals.MainWindow.DataGrid_GameTextForLanguage.Resources)
            {
                if (select.Value is MenuItem item && item.Tag is string value)
                {
                    ColumnVisiblity[int.Parse(value)] = item.IsChecked;
                }
            }
        }

        /// <summary>
        /// 读取列可见性
        /// </summary>
        private void Preference_LoadColumnVisibility()
        {
            foreach (System.Collections.DictionaryEntry select in Globals.MainWindow.DataGrid_TranslatedTexts.Resources)
            {
                if (select.Value is MenuItem item && item.Tag is string value)
                {
                    item.IsChecked = ColumnVisiblity[int.Parse(value)];
                }
            }
            foreach (System.Collections.DictionaryEntry select in Globals.MainWindow.DataGrid_GameTextForLanguage.Resources)
            {
                if (select.Value is MenuItem item && item.Tag is string value)
                {
                    item.IsChecked = ColumnVisiblity[int.Parse(value)];
                }
            }
        }

        #endregion

        #region 最近项目

        /// <summary>
        /// 增加最近打开记录
        /// </summary>
        /// <param name="file">打开文件</param>
        public void Preference_AddRecentRecord(FileInfo file)
        {
            List<string> list = RecentProjectList.ToList();
            string path = file.FullName;
            if (list.Contains(path))
            {
                list.Remove(path);
            }
            list.Insert(0, path);
            while (list.Count > Prefrence_MaxRecentProjectCount)
            {
                list.RemoveAt(list.Count - 1);
            }
            RecentProjectList = list.ToArray();
        }

        /// <summary>
        /// 移除最近打开记录
        /// </summary>
        /// <param name="file">打开文件</param>
        public void Preference_RemoveRecentRecord(FileInfo file)
        {
            List<string> list = RecentProjectList.ToList();
            string path = file.FullName;
            if (list.Contains(path))
            {
                list.Remove(path);
            }
            RecentProjectList = list.ToArray();
        }

        #endregion

        #endregion

        #region 压缩文件

        /// <summary>      
        /// 序列化DataSet对象并压缩      
        /// </summary>      
        private static void SerializerCompression()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Class_Preference));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, Globals.Preference);
            byte[] buffer = ms.ToArray();
            ms.Close();
            ms.Dispose();
            if (Preference_ConfigFile.Directory != null && !Preference_ConfigFile.Directory.Exists)
            {
                Preference_ConfigFile.Directory.Create();
            }

            FileStream fs = Preference_ConfigFile.Create();
            GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true);
            gzipStream.Write(buffer, 0, buffer.Length);
            gzipStream.Close();
            gzipStream.Dispose();
            fs.Close();
            fs.Dispose();
        }

        /// <summary>   
        /// 反序列化压缩的DataSet   
        /// </summary>   
        private static void DeserializeDecompress()
        {
            FileStream fs = Preference_ConfigFile.OpenRead();
            fs.Position = 0;
            GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress);
            byte[] buffer = new byte[4096];
            int offset;
            MemoryStream ms = new MemoryStream();
            while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                ms.Write(buffer, 0, offset);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Class_Preference));
            ms.Position = 0;
            try
            {
                Globals.Preference = serializer.Deserialize(ms) as Class_Preference;
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }
            fs.Close();
            fs.Dispose();
            gzipStream.Close();
            gzipStream.Dispose();
        }

        #endregion

        #endregion

    }
}
