using Fluent.Localization;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Xml.Linq;

namespace SC2_GameTranslater.Source
{
    using Log = SC2_GameTranslater.Source.Class_Log;

    #region 声明

    /// <summary>
    /// SC2本地化语言类型枚举
    /// </summary>
    public enum EnumLanguage
    {
        /// <summary>
        /// 简体中文
        /// </summary>
        zhCN = 0x0804,
        /// <summary>
        /// 繁体中文
        /// </summary>
        zhTW = 0x0404,
        /// <summary>
        /// 英语美国
        /// </summary>
        enUS = 0x0409,
        /// <summary>
        /// 德语德国
        /// </summary>
        deDE = 0x0407,
        /// <summary>
        /// 西班牙语墨西哥
        /// </summary>
        esMX = 0x080A,
        /// <summary>
        /// 西班牙语西班牙
        /// </summary>
        esES = 0x0c0A,
        /// <summary>
        /// 法语法国
        /// </summary>
        frFR = 0x040C,
        /// <summary>
        /// 意大利语意大利
        /// </summary>
        itIT = 0x0410,
        /// <summary>
        /// 波兰语波兰
        /// </summary>
        plPL = 0x0415,
        /// <summary>
        /// 葡萄牙语巴西
        /// </summary>
        ptBR = 0x0416,
        /// <summary>
        /// 俄语俄罗斯
        /// </summary>
        ruRU = 0x0419,
        /// <summary>
        /// 韩语韩国
        /// </summary>
        koKR = 0x0412,
    }

    #endregion

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

        #region 文件名

        public const string Extension_SC2GameTran = ".SC2GameTran";
        public const string Extension_SC2Map = ".SC2Map";
        public const string Extension_SC2Mod = ".SC2Mod";
        public const string Extension_SC2Components = ".SC2Components";
        public const string FileName_SC2Components = "ComponentList.SC2Components";

        #endregion
        
        #endregion

        #endregion

        #region 属性字段

        #region 属性

        /// <summary>
        /// 允许在静态类中显示日志
        /// </summary>
        public static bool EnableShowLogInUI
        {
            set
            {
                m_EnableShowLogInUI = value;
                if (!value)
                {
                    MainWindow.AvalonTextEditor_Log.Text = "";
                }
            }
            get
            {
                return m_EnableShowLogInUI;
            }
        }
        private static bool m_EnableShowLogInUI = true;

        /// <summary>
        /// 当前处理的数据
        /// </summary>
        public static Data_GameText CurrentProject
        {
            set
            {
                _EventProjectChange(mCurrentProject, value);
                mCurrentProject = value;
            }
            get => mCurrentProject;
        }
        private static Data_GameText mCurrentProject = null;

        /// <summary>
        /// 当前处理数据切换事件
        /// </summary>
        public static event Delegate_CurrentProjectChange EventProjectChange
        {
            add { _EventProjectChange += value; }
            remove { _EventProjectChange -= value; }
        }
        private static event Delegate_CurrentProjectChange _EventProjectChange;

        /// <summary>
        /// 项目对应的Mod或Map路径
        /// </summary>
        public static bool ModPathValid
        {
            get
            {
                return CurrentProject == null ? false : File.Exists(CurrentProject.ModPath);
            }
        }

        /// <summary>
        /// 语言字典
        /// </summary>
        public static Dictionary<EnumLanguage, ResourceDictionary> DictUILanguages { set; get; } = new Dictionary<EnumLanguage, ResourceDictionary>();

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

        public static Dictionary<EnumLanguage, RibbonLocalizationBase> FluentLocalizationMap { set; get; } = new Dictionary<EnumLanguage, RibbonLocalizationBase>();

        /// <summary>
        /// 配置文件
        /// </summary>
        public static Class_Preference Preference { set; get; } = new Class_Preference();

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

        #region 通用

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
            if (!savePath.Directory.Exists)
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
            int offset = 0;//定义读取位置   
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
            catch
            {
                throw;
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
        /// 初始化新项目数据
        /// </summary>
        /// <param name="file">文件路径</param>
        public static void InitProjectData(FileInfo file)
        {
            Log.Assert(CurrentProject == null);
            Data_GameText project = new Data_GameText();
            project.Initialization(file);
        }

        #endregion

        #endregion
    }
}
