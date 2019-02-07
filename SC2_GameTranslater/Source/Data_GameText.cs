using System;

namespace SC2_GameTranslater.Source
{
    /// <summary>
    /// 翻译文本数据表
    /// </summary>
    public partial class Data_GameText
    {
        partial class Table_ModInfoDataTable
        {
        }
        #region 声明常量

        #region 表常量声明

        #region 表名

        public const string TN_ModInfo = "Table_ModInfo";
        public const string TN_Language = "Table_Language";
        public const string TN_GalaxyFile = "Table_GalaxyFile";
        public const string TN_GalaxyLine = "Table_GalaxyLine";
        public const string TN_GameText = "Table_GameText";
        public const string TN_Log = "Table_Log";

        #endregion

        #region 列名

        public const string RN_ModInfo_FilePath = "FilePath";
        public const string RN_Language_ID = "ID";
        public const string RN_Language_Count = "Count";
        public const string RN_GalaxyFile_Path = "Path";
        public const string RN_GalaxyFile_Count = "Count";
        public const string RN_GalaxyLine_Index = "Index";
        public const string RN_GalaxyLine_File = "File";
        public const string RN_GalaxyLine_Script = "Script";
        public const string RN_GameText_Index = "Index";
        public const string RN_GameText_ID = "ID";
        public const string RN_GameText_LanguageID = "LanguageID";
        public const string RN_GameText_Type = "Type";
        public const string RN_GameText_FileLine = "FileLine";
        public const string RN_GameText_Text = "Text";
        public const string RN_GameText_TempText = "TempText";
        public const string RN_Log_ID = "ID";
        public const string RN_Log_Date = "Date";
        public const string RN_Log_Type = "Type";
        public const string RN_Log_Target = "Target";
        public const string RN_Log_OldValue = "OldValue";
        public const string RN_Log_NewValue = "NewValue";
        public const string RN_Log_Msg = "Msg";

        #endregion

        #region 关系

        public const string RSN_Language_GameText_LanguageID = "Relation_Language_GameText_LanguageID";
        public const string RSN_GalaxyFile_GalaxyLine_File = "Relation_GalaxyFile_GalaxyLine_File";
        public const string RSN_GalaxyFile_GameTextD_FileLine = "Relation_GalaxyFile_GameTextD_FileLine";
        public const string RSN_GameText_Log_Target = "Relation_GameText_Log_Target";

        #endregion

        #endregion

        #endregion

        #region 属性字段

        #endregion

        #region 构造函数

        #endregion
    }
}
