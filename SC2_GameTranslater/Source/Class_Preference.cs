using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
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

        // 字段
        public const string Preference_ElementConfig = "Preference";
        public const string Preference_AttributeWidth = "Width";
        public const string Preference_AttributeHeight = "Height";
        public const string Preference_AttributeLastFolderPath = "LastFolder";
        public const string Preference_ElementRecentProjectList = "RecentProjectList";
        public const string Preference_AttributeRecentProject = "RecentProject";

        #endregion

        #region 属性字段

        #region 属性

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
        public List<string> RecentProjectList { get; set; } = new List<string>();

        #endregion

        #endregion

        #region 生成加载存储

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SavePreference()
        {
            Globals.Preference.Preference_SaveWindowSize();
            SerializerCompression();
        }

        /// <summary>
        /// 加载配置
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
            }
        }

        #endregion

        #region 配置数据

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
            if (!Preference_ConfigFile.Directory.Exists)
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
            int offset = 0;
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
            catch
            {
                throw;
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

    }
}
