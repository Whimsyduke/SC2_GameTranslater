using Fluent.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace SC2_GameTranslater.Source
{
    using Log = Class_Log;

    /// <summary>
    /// 全局数据
    /// </summary>
    public static class Class_Globals
    {
        #region 声明常量

        #region 类型

        public delegate void Delegate_CurrentProjectChange(Data_GameText oldPro, Data_GameText newPro);

        #endregion

        #region 常量

        #region 默认值

        /// <summary>
        /// 未在Galaxy文件中使用
        /// </summary>
        public static string Const_NoUseInGalaxy { get;} = "-";
        public static string ProjectInfoVersion = "1.0.0";

        #endregion

        #endregion

        #endregion

        #region 属性字段

        #region 属性

        /// <summary>
        /// 当前处理的数据
        /// </summary>
        public static Data_GameText CurrentProject
        {
            set
            {
                PEventProjectChange?.Invoke(mCurrentProject, value);
                mCurrentProject = value;
            }
            get => mCurrentProject;
        }
        private static Data_GameText mCurrentProject;

        /// <summary>
        /// 当前处理数据切换事件
        /// </summary>
        public static event Delegate_CurrentProjectChange EventProjectChange
        {
            add { PEventProjectChange += value; }
            remove { PEventProjectChange -= value; }
        }
        private static event Delegate_CurrentProjectChange PEventProjectChange;

        /// <summary>
        /// 项目对应的Mod或Map路径
        /// </summary>
        public static bool ComponentsPathValid
        {
            get
            {
                return CurrentProject == null ? false : CurrentProject.SC2Components.Exists;
            }
        }

        /// <summary>
        /// 当前项目路径
        /// </summary>
        public static FileInfo CurrentProjectPath { set; get; } = null;

        /// <summary>
        /// 语言字典
        /// </summary>
        public static Dictionary<EnumLanguage, ResourceDictionary> DictUILanguages { set; get; } = new Dictionary<EnumLanguage, ResourceDictionary>();

        /// <summary>
        /// 全部语言
        /// </summary>
        public static List<EnumLanguage> AllLanguage { get; } = AllLanguageList();

        /// <summary>
        /// 本地化信息字典
        /// </summary>
        public static Dictionary<string, CultureInfo> DictCultureInfo { set; get; } = NewCultureInfoDict();

        /// <summary>
        /// 列表项对应语言
        /// </summary>
        public static Dictionary<object, EnumLanguage> DictComboBoxItemLanguage { set; get; } = new Dictionary<object, EnumLanguage>();

        /// <summary>
        /// 当前语言
        /// </summary>
        public static ResourceDictionary CurrentLanguage { set; get; }

        /// <summary>
        /// 主窗口
        /// </summary>
        public static SC2_GameTranslater_Window MainWindow { set; get; }

        /// <summary>
        /// Fluent语言字典
        /// </summary>
        public static Dictionary<EnumLanguage, RibbonLocalizationBase> FluentLocalizationMap { set; get; } = new Dictionary<EnumLanguage, RibbonLocalizationBase>();

        /// <summary>
        /// 配置文件
        /// </summary>
        public static Class_Preference Preference { set; get; } = new Class_Preference();

        /// <summary>
        /// 软件版本
        /// </summary>
        public static string SoftwareVersion { get; } = "V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        static Class_Globals()
        {
        }

        #endregion

        #region 方法

        #region 属性初始值

        /// <summary>
        /// 全部语言列表
        /// </summary>
        /// <returns>语言列表</returns>
        public static List<EnumLanguage> AllLanguageList()
        {
            List<EnumLanguage> languages = new List<EnumLanguage>((EnumLanguage[])Enum.GetValues(typeof(EnumLanguage)));
            languages.Remove(EnumLanguage.Other);
            return languages;
        }

        /// <summary>
        /// 新建本地化信息字典
        /// </summary>
        /// <returns>字典</returns>
        private static Dictionary<string, CultureInfo> NewCultureInfoDict()
        {
            Dictionary<string, CultureInfo> dictCulture = new Dictionary<string, CultureInfo>();
            foreach (EnumLanguage language in AllLanguage)
            {
                dictCulture[GetEnumLanguageName(language)] = new CultureInfo((int)language);
            }
            return dictCulture;
        }

        #endregion

        #region 打开保存窗口

        /// <summary>
        /// 打开文件路径窗口
        /// </summary>
        /// <param name="baseFolder">默认打开目录</param>
        /// <param name="filter">文件类型验证</param>
        /// <param name="title">标题</param>
        /// <param name="fileDialog">对话框</param>
        /// <returns>打开结果</returns>
        public static System.Windows.Forms.DialogResult OpenFilePathDialog(string baseFolder, string filter, string title, out System.Windows.Forms.OpenFileDialog fileDialog)
        {
            fileDialog = new System.Windows.Forms.OpenFileDialog();
            if (Directory.Exists(baseFolder))
            {
                fileDialog.InitialDirectory = baseFolder;
            }
            else
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            fileDialog.Filter = filter;
            fileDialog.Title = title;
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            return fileDialog.ShowDialog();
        }

        /// <summary>
        /// 保存文件路径窗口
        /// </summary>
        /// <param name="baseFolder">默认保存目录</param>
        /// <param name="filter">文件类型验证</param>
        /// <param name="title">标题</param>
        /// <param name="fileDialog">对话框</param>
        /// <returns>打开结果</returns>
        public static System.Windows.Forms.DialogResult SaveFilePathDialog(string baseFolder, string filter, string title, out System.Windows.Forms.SaveFileDialog fileDialog)
        {
            fileDialog = new System.Windows.Forms.SaveFileDialog();
            if (Directory.Exists(baseFolder))
            {
                fileDialog.InitialDirectory = baseFolder;
            }
            else
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            fileDialog.Filter = filter;
            fileDialog.Title = title;
            fileDialog.RestoreDirectory = true;
            return fileDialog.ShowDialog();
        }

        #endregion

        #region 压缩反压缩

        /// <summary>      
        /// 序列化对象并压缩      
        /// </summary>      
        /// <param name="savePath">保存路径</param>   
        /// <param name="obj">压缩对象</param>    
        public static void ObjectSerializerCompression(FileInfo savePath, object obj)
        {
            IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象   
            MemoryStream ms = new MemoryStream();//创建内存流对象   
            formatter.Serialize(ms, obj);//把DataSet对象序列化到内存流   
            byte[] buffer = ms.ToArray();//把内存流对象写入字节数组   
            ms.Close();//关闭内存流对象   
            ms.Dispose();//释放资源   
            if (savePath.Directory != null && !savePath.Directory.Exists)
            {
                savePath.Directory.Create();
            }

            FileStream fs = savePath.Create();//创建文件   
            GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true);//创建压缩对象   
            gzipStream.Write(buffer, 0, buffer.Length);//把压缩后的数据写入文件   
            gzipStream.Close();//关闭压缩流   
            gzipStream.Dispose();//释放对象   
            fs.Close();//关闭流   
            fs.Dispose();//释放对象   
        }

        /// <summary>   
        /// 反序列化压缩的对象 
        /// </summary>   
        /// <param name="filePath">读取路径</param>   
        /// <returns>序列化对象</returns>   
        public static object ObjectDeserializeDecompress(FileInfo filePath)
        {
            FileStream fs = filePath.OpenRead();//打开文件   
            fs.Position = 0;//设置文件流的位置   
            GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress);//创建解压对象   
            byte[] buffer = new byte[4096];//定义数据缓冲   
            int offset;//定义读取位置   
            MemoryStream ms = new MemoryStream();//定义内存流   
            while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                ms.Write(buffer, 0, offset);//解压后的数据写入内存流   
            }
            BinaryFormatter sfFormatter = new BinaryFormatter();//定义BinaryFormatter以反序列化DataSet对象   
            ms.Position = 0;//设置内存流的位置   
            object ds;
            try
            {
                ds = sfFormatter.Deserialize(ms);//反序列化   
            }
            finally
            {
                ms.Close();//关闭内存流   
                ms.Dispose();//释放资源   
            }
            fs.Close();//关闭文件流   
            fs.Dispose();//释放资源   
            gzipStream.Close();//关闭解压缩流   
            gzipStream.Dispose();//释放资源   
            return ds;
        }

        #endregion

        #region 项目文件

        /// <summary>
        /// 打开新项目数据
        /// </summary>
        /// <param name="file">文件路径</param>
        public static bool OpenProjectData(FileInfo file)
        {
            Log.Assert(CurrentProject != null, "Globals.CurrentProject == null");
            Data_GameText project = Data_GameText.LoadProject(file);
            CurrentProject = project;
            return project != null;
        }

        #endregion

        #region 通用

        /// <summary>
        /// 获取当前语言对应的文本内容
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>文本</returns>
        public static string GetStringFromCurrentLanguage(string key)
        {
            if (CurrentLanguage[key] is string text)
            {
                return text;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取当前语言对应逗格式文本内容
        /// </summary>
        /// <param name="origin">源文本</param>
        /// <returns>文本</returns>
        public static string GetCommaStringFromCurrentLanguage(string origin)
        {
            if (CurrentLanguage["TEXT_Comma"] is string text)
            {
                return string.Format(text, origin);
            }
            else
            {
                return "," + origin;
            }
        }

        /// <summary>
        /// 获取当前语言对应的格式化文本内容
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="args">参数</param>
        /// <returns>文本</returns>
        public static string GetStringFromCurrentLanguage(string key, params object[] args)
        {
            if (CurrentLanguage[key] is string text)
            {
                return string.Format(text, args);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取语言枚举对应的名称
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>名称</returns>
        public static string GetEnumLanguageName(EnumLanguage language)
        {
            return Enum.GetName(language.GetType(), language);
        }

        #endregion

        #endregion
    }
}
