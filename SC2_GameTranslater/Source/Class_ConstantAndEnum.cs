using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC2_GameTranslater.Source
{
    #region 声明

    #region 枚举

    /// <summary>
    /// SC2本地化语言类型枚举
    /// </summary>
    public enum EnumLanguage
    {
        /// <summary>
        /// 其它
        /// </summary>
        Other = 0,
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

    /// <summary>
    /// 搜索文本类型
    /// </summary>
    public enum EnumSearchTextType
    {
        /// <summary>
        /// ID
        /// </summary>
        ID = 1,
        /// <summary>
        /// 原文本
        /// </summary>
        Source = 2,
        /// <summary>
        /// 旧文本
        /// </summary>
        Droped = 4,
        /// <summary>
        /// 修改文本
        /// </summary>
        Edited = 8,
        /// <summary>
        /// 全部文本
        /// </summary>
        AllText = 14,
        /// <summary>
        /// 全部（全部文本及ID）
        /// </summary>
        All = 15
    }

    /// <summary>
    /// 搜索文本位置
    /// </summary>
    public enum EnumSearchTextLocation
    {
        /// <summary>
        /// 全部
        /// </summary>
        All,
        /// <summary>
        /// 左侧开始
        /// </summary>
        Left,
        /// <summary>
        /// 右侧开始
        /// </summary>
        Right
    }

    #endregion

    #endregion


    /// <summary>
    /// 常量
    /// </summary>
    public class Class_ConstantAndEnum
    {
        #region 常量

        #region 链接

        public const string Link_HelpTopic = "https://github.com/Whimsyduke/SC2_GameTranslater";

        #endregion

        #region 文件名

        public const string Extension_SC2GameTran = ".SC2GameTran";
        public const string Extension_SC2Map = ".SC2Map";
        public const string Extension_SC2Mod = ".SC2Mod";
        public const string Extension_Galaxy = ".Galaxy";
        public const string Extension_SC2Components = ".SC2Components";
        public const string FileName_SC2Components = "ComponentList.SC2Components";

        #endregion

        #region 路径

        public const string Path_TempFolder = "Temp\\";
        public const string Path_BackupFolder = "Backup\\";

        #endregion


        #endregion
    }
}
