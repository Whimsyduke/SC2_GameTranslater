using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SC2_GameTranslater.Source
{
    using Globals = Class_Globals;

    #region 声明

    /// <summary>
    /// 搜索基本方式枚举
    /// </summary>
    public enum EnumSearchWay
    {
        /// <summary>
        /// 空文本
        /// </summary>
        Null,
        /// <summary>
        /// 正则表达式
        /// </summary>
        Regex,
        /// <summary>
        /// 匹配文本
        /// </summary>
        Text,
    }

    #endregion

    /// <summary>
    /// 搜索配置
    /// </summary>
    [XmlRoot(SearchConfig_ElementRoot)]
    public class Class_SearchConfig
    {
        #region 常量

        // 字段
        public const string SearchConfig_ElementRoot = "SearchConfig";
        public const string SearchConfig_AttributeTraslateLanguage = "TraslateLanguage";
        public const string SearchConfig_AttributeSearchType = "SearchType";
        public const string SearchConfig_AttributeSearchLanguage = "SearchLanguage";
        public const string SearchConfig_AttributeSearchLocation = "SearchLocation";
        public const string SearchConfig_AttributeSearchWay = "SearchWay";
        public const string SearchConfig_AttributeSearchCase = "SearchCase";
        public const string SearchConfig_AttributeSearchText = "SearchText";
        public const string SearchConfig_ArrayGalaxyFile = "GalaxyFile";
        public const string SearchConfig_AttributeTextFile = "TextFile";
        public const string SearchConfig_AttributeTextStatuse = "TextStatus";
        public const string SearchConfig_AttributeUseStatus = "UseStatus";

        #endregion

        #region 属性字段

        /// <summary>
        /// 翻译语言
        /// </summary>
        /// <remarks>特殊值0代表当前软件语言</remarks>
        [XmlAttribute(SearchConfig_AttributeTraslateLanguage)]
        public EnumLanguage TraslateLanguage { private set; get; } = 0;

        /// <summary>
        /// 搜索类型
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchType)]
        public EnumSearchTextType SearchType { private set; get; } = EnumSearchTextType.All;

        /// <summary>
        /// 搜索位置
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchLocation)]
        public EnumSearchTextLocation SearchLocation { private set; get; } = EnumSearchTextLocation.All;

        /// <summary>
        /// 搜索基本方式
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchWay)]
        public EnumSearchWay SearchWay { private set; get; } = EnumSearchWay.Text;

        /// <summary>
        /// 搜索匹配大小写
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchCase)]
        public bool SearchCase { private set; get; } = false;
        
        /// <summary>
        /// 搜索文本
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchText)]
        public string SearchText { private set; get; } = "";
        
        /// <summary>
        /// 搜索Galaxy文件
        /// </summary>
        /// <remarks>特殊值null代表全部选择，数组中值""代表不在Galaxy文件中的文本</remarks>
        [XmlArray(SearchConfig_ArrayGalaxyFile)]
        public string[] GalaxyFile { private set; get; } = null;
        
        /// <summary>
        /// 所属文本文件
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeTextFile)]
        public EnumGameTextFile TextFile { private set; get; } = EnumGameTextFile.All;

        /// <summary>
        /// 所属文本状态
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeTextStatuse)]
        public EnumGameTextStatus TextStatus { private set; get; } = EnumGameTextStatus.All;

        /// <summary>
        /// 所属使用状态
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeUseStatus)]
        public EnumGameUseStatus UseStatus { private set; get; } = EnumGameUseStatus.All;

        #endregion

        #region 构造函数

        #endregion

        #region 方法

        #region 生成

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <returns>筛选记录对象</returns>
        public static Class_SearchConfig NewSearchConfig()
        {
            Class_SearchConfig config = new Class_SearchConfig();
            config.LoadFromUI();
            return config;
        }

        #endregion

        #region 序列化

        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <returns>序列化数据值</returns>
        public static byte[] Serializer(Class_SearchConfig config)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Class_SearchConfig));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, config);
            byte[] data = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return data;
        }

        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <returns>序列化数据值</returns>
        public static Class_SearchConfig Deserialize(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            XmlSerializer serializer = new XmlSerializer(typeof(Class_SearchConfig));
            ms.Position = 0;
            Class_SearchConfig config = null;
            try
            {
                config = serializer.Deserialize(ms) as Class_SearchConfig;
            }
            catch
            {
                config = new Class_SearchConfig();
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }
            return config;
        }

        #endregion

        #region UI读写

        /// <summary>
        /// 加载数据自UI
        /// </summary>
        public void LoadFromUI()
        {
            TraslateLanguage = Globals.MainWindow.GetFileterTranslateLanguage();
            SearchType = Globals.MainWindow.GetFileterSearchTextType();
            SearchLocation = Globals.MainWindow.GetFileterSearchLocation();
            SearchWay = Globals.MainWindow.GetFileterSearchWay();
            SearchCase = Globals.MainWindow.GetFileterSearchCase();
            SearchText = Globals.MainWindow.TextBox_SearchText.Text;
            GalaxyFile = Globals.MainWindow.GetFileterGalaxyFile();
            TextFile = Globals.MainWindow.GetFileterTextFile();
            TextStatus = Globals.MainWindow.GetFileterTextStatus();
            UseStatus = Globals.MainWindow.GetFileterUseStatus();
        }

        /// <summary>
        /// 加载数据自UI
        /// </summary>
        public void ApplyToUI()
        {
            SC2_GameTranslater_Window.CanSaveRecord = false;
            Globals.MainWindow.SetFileterTranslateLanguage(TraslateLanguage);
            Globals.MainWindow.SetFileterSearchTextType(SearchType);
            Globals.MainWindow.SetFileterSearchLocation(SearchLocation);
            Globals.MainWindow.SetFileterSearchWay(SearchWay);
            Globals.MainWindow.SetFileterSearchCase(SearchCase);
            Globals.MainWindow.TextBox_SearchText.Text = SearchText;
            Globals.MainWindow.SetFileterGalaxyFile(GalaxyFile);
            Globals.MainWindow.SetFileterTextFile(TextFile);
            Globals.MainWindow.SetFileterTextStatus(TextStatus);
            Globals.MainWindow.SetFileterUseStatus(UseStatus);
            Globals.MainWindow.RefreshTranslatedText();
            SC2_GameTranslater_Window.CanSaveRecord = true;
        }

        #endregion

        #region 重载

        /// <summary>
        /// 重载等于运算
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>返回结果</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Class_SearchConfig config)) return false;
            if (TraslateLanguage != config.TraslateLanguage) return false;
            if (SearchType != config.SearchType) return false;
            if (SearchLocation != config.SearchLocation) return false;
            if (SearchWay != config.SearchWay) return false;
            if (SearchCase != config.SearchCase) return false;
            if (SearchText != config.SearchText) return false;
            if (TextFile != config.TextFile) return false;
            if (TextStatus != config.TextStatus) return false;
            if (UseStatus != config.UseStatus) return false;
            if (GalaxyFile != null && config.GalaxyFile != null)
            {
                if (GalaxyFile.Count() != config.GalaxyFile.Count())
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < GalaxyFile.Count(); i++)
                    {
                        if (GalaxyFile[i] != config.GalaxyFile[i]) return false;
                    }
                }
            }
            else if (GalaxyFile != config.GalaxyFile)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 当前对象的哈希代码
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return 0;
        }

        ///// <summary>
        ///// 重载等于运算符
        ///// </summary>
        ///// <param name="A">数据A</param>
        ///// <param name="B">数据B</param>
        ///// <returns>比较结果</returns>
        //public static  bool operator == (Class_SearchConfig A, Class_SearchConfig B)
        //{

        //}

        ///// <summary>
        ///// 重载不等于运算符
        ///// </summary>
        ///// <param name="A">数据A</param>
        ///// <param name="B">数据B</param>
        ///// <returns>比较结果</returns>
        //public static bool operator !=(Class_SearchConfig A, Class_SearchConfig B)
        //{

        //}

        #endregion

        #endregion
    }
}
