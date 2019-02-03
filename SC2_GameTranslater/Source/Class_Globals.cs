using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SC2_GameTranslater;
using Fluent;
using System.Reflection;
using Fluent.Localization;

namespace SC2_GameTranslater.Source
{
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
    static class Class_Globals
    {

        #region 属性字段

        #region 属性

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
        /// <summary>
        /// 获取命名控件类型类别
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="nameSpace">命名空间</param>
        /// <returns>类型列表</returns>
        private static IList<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToList();
        }

        #endregion
    }
}
