using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SC2_GameTranslater.Source
{
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
        public EnumLanguage TraslateLanguage { set; get; } = 0;

        /// <summary>
        /// 搜索类型
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchType)]
        public EnumSearchTextType SearchType { set; get; } = EnumSearchTextType.All;

        /// <summary>
        /// 搜索语言
        /// </summary>
        /// <remarks>特殊值0代表全部语言</remarks>
        [XmlAttribute(SearchConfig_AttributeSearchLanguage)]
        public EnumLanguage SearchLanguage { set; get; } = 0;

        /// <summary>
        /// 搜索位置
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchLocation)]
        public EnumSearchTextLocation SearchLocation { set; get; } = EnumSearchTextLocation.All;

        /// <summary>
        /// 搜索基本方式
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchWay)]
        public EnumSearchWay SearchWay { set; get; } = EnumSearchWay.Text;

        /// <summary>
        /// 搜索匹配大小写
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchCase)]
        public bool SearchCase { set; get; } = false;
        
        /// <summary>
        /// 搜索文本
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeSearchText)]
        public string SearchText { set; get; } = "";
        
        /// <summary>
        /// 搜索Galaxy文件
        /// </summary>
        /// <remarks>特殊值null代表全部选择，数组中值""代表不在Galaxy文件中的文本</remarks>
        [XmlArray(SearchConfig_ArrayGalaxyFile)]
        public string[] GalaxyFile { set; get; } = null;
        
        /// <summary>
        /// 所属文本文件
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeTextFile)]
        public EnumGameTextFile TextFile { set; get; } = EnumGameTextFile.All;

        /// <summary>
        /// 所属文本状态
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeTextStatuse)]
        public EnumGameTextStatus TextStatus { set; get; } = EnumGameTextStatus.All;

        /// <summary>
        /// 所属使用状态
        /// </summary>
        [XmlAttribute(SearchConfig_AttributeUseStatus)]
        public EnumGameUseStatus UseStatus { set; get; } = EnumGameUseStatus.All;

        #endregion

        #region 方法

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
    }
}
