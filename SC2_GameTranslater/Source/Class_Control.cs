﻿using Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SC2_GameTranslater.Source
{
    using Globals = Class_Globals;
    using Log = Class_Log;

    #region Converter

    /// <summary>
    /// 测试值转换器
    /// </summary>
    public class TestValueConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// 格式化文本Converter
    /// </summary>
    public class FormatTextConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is List<object> list)
            {
                if (list[0] is string key && list[1] is DataRow row)
                {
                    object[] args = list.Skip(2).Select(r => row[r as string]).ToArray();
                    return Globals.GetStringFromCurrentLanguage(key, args);
                }
            }
            return null;
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// 标题Converter
    /// </summary>
    public class TitleTextConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Globals.CurrentLanguage == null) return "";
            if(values[0] is string path)
            {
                if (path == "TEXT_WindowTitleProjectUnsaved")
                {
                    path = Globals.GetStringFromCurrentLanguage(path);
                }
                else
                {
                    if (values[1] is bool needSave && needSave)
                    {
                        path += "*";
                    }
                }
                return string.Format(Globals.GetStringFromCurrentLanguage("TEXT_WindowTitleText"), Globals.SoftwareVersion, Globals.GetStringFromCurrentLanguage("TEXT_WindowTitleProject"), path);
            }
            else
            {
                return string.Format(Globals.GetStringFromCurrentLanguage("TEXT_WindowTitleText"), Assembly.GetExecutingAssembly().GetName().Version, "", "");
            }
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 自动序号Converter
    /// </summary>
    public class AutoNumberValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null) return 0;
            if (values[1] is ItemCollection items)
            {
                int index = items.IndexOf(values[0]);
                return (index + 1).ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 布尔值可见性转换器
    /// </summary>
    public class EnumLanguageToTreslateNameConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (value != null)
            {
                EnumLanguage language = (EnumLanguage)value;
                string langName = Globals.GetEnumLanguageName(language);
                return Globals.GetStringFromCurrentLanguage($"TEXT_Use{langName}");
            }
            Log.Assert(true, "EnumLanguageToTreslateNameConverter");
            return Globals.GetStringFromCurrentLanguage("TEXT_Error");
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }

    /// <summary>
    /// 翻译语言对应数据Converter
    /// </summary>
    public class TranslatedDataTextConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] is EnumLanguage language && language != EnumLanguage.Other && values[0] is DataRowView rowView)
            {
                string column = parameter as string;
                string key = Data_GameText.GetRowNameForLanguage(language, column);
                object display = rowView.Row[key];
                if (display == DBNull.Value)
                {
                    return Globals.GetStringFromCurrentLanguage("TEXT_NoText");
                }
                else
                {
                    return display as string;
                }
            }
            else
            {
                Log.Assert(true, "EnumTranslatedDataTextConverter");
                return Globals.GetStringFromCurrentLanguage("TEXT_Error");
            }
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 翻译语言对应数据Converter
    /// </summary>
    public class EnumGameDataTextConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string text)
            {
                return text;
            }
            else if (values[0] == DBNull.Value || values[0] == DependencyProperty.UnsetValue)
            {
                return Globals.GetStringFromCurrentLanguage("TEXT_NoText");
            }
            else
            {
                Log.Assert(true, "EnumGameDataTextConverter");
                return Globals.GetStringFromCurrentLanguage("TEXT_Error");
            }
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 文本文件枚举值Converter
    /// </summary>
    public class EnumNameInLanguage_TextFileConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is DataRowView rowView)
            {
                object var = rowView.Row[Data_GameText.RN_GameText_File];
                EnumGameTextFile value = (EnumGameTextFile)Enum.ToObject(typeof(EnumGameTextFile), var);
                return Data_GameText.GetEnumNameInLanguage(value);
            }
            else
            {
                Log.Assert(true, "EnumNameInLanguage_TextFileConverter");
                return Globals.GetStringFromCurrentLanguage("TEXT_Error");
            }
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 文本状态枚举值Converter
    /// </summary>
    public class EnumValue_TextStatusConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is DataRowView view && values[1] is EnumLanguage language && language != EnumLanguage.Other)
            {
                string keyStatus = Data_GameText.GetRowNameForLanguage(language, Data_GameText.RN_GameText_TextStatus);
                return view.Row[keyStatus];
            }
            return EnumGameTextStatus.Normal;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 使用状态枚举值Converter
    /// </summary>
    public class EnumValue_UseStatusConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is DataRowView view && values[1] is EnumLanguage language && language != EnumLanguage.Other)
            {
                string keyStatus = Data_GameText.GetRowNameForLanguage(language, Data_GameText.RN_GameText_UseStatus);
                return view.Row[keyStatus];
            }
            return EnumGameUseStatus.Normal;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 文本状态枚举名称Converter
    /// </summary>
    public class EnumNameInLanguage_TextStatusConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue)
            {
                Log.Assert(true, "EnumNameInLanguage_TextStatusConverter");
                return Globals.GetStringFromCurrentLanguage("TEXT_Error");
            }

            EnumGameTextStatus status = (EnumGameTextStatus)Enum.ToObject(typeof(EnumGameTextStatus), values[0]);
            return Data_GameText.GetEnumNameInLanguage(status);
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 使用状态枚举名称Converter
    /// </summary>
    public class EnumNameInLanguage_UseStatusConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue)
            {
                Log.Assert(true, "EnumNameInLanguage_UseStatusConverter");
                return Globals.GetStringFromCurrentLanguage("TEXT_Error");
            }

            EnumGameUseStatus status = (EnumGameUseStatus)Enum.ToObject(typeof(EnumGameUseStatus), values[0]);
            return Data_GameText.GetEnumNameInLanguage(status);
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 文本状态枚举颜色Converter
    /// </summary>
    public class EnumColor_TextStatusConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null|| values[0] == DependencyProperty.UnsetValue) return Data_GameText.TextStatusColor[EnumGameTextStatus.Normal];
            EnumGameTextStatus status = (EnumGameTextStatus)Enum.ToObject(typeof(EnumGameTextStatus), values[0]);
            return Data_GameText.TextStatusColor[status];
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 使用状态枚举颜色Converter
    /// </summary>
    public class EnumColor_UseStatusConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue) return Data_GameText.UseStatusColor[EnumGameUseStatus.Normal];
            EnumGameUseStatus status = (EnumGameUseStatus)Enum.ToObject(typeof(EnumGameUseStatus), values[0]);
            return Data_GameText.UseStatusColor[status];
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 所在Galaxy文件名转换器Converter
    /// </summary>
    public class TextInGalaxyFileNameConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataRowView view)
            {
                DataRow rowFile = view.Row.GetParentRow(Data_GameText.RSN_GalaxyFile_GalaxyLine_File);
                return rowFile[Data_GameText.RN_GalaxyFile_Name];
            }
            else
            {
                Log.Assert(true, "TextInGalaxyFileNameConverter");
                return Globals.GetStringFromCurrentLanguage("TEXT_Error");
            }
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }

    /// <summary>
    /// 空文本文本加粗
    /// </summary>
    public class DataGridColumnNullTextFontWeightConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DataRowView rowView = values[0] as DataRowView;
            if (values[1] == DependencyProperty.UnsetValue) return FontWeights.Normal;
            EnumLanguage translatedLanguage = (EnumLanguage)Enum.ToObject(typeof(EnumGameUseStatus), values[1]);
            if (translatedLanguage == EnumLanguage.Other) return FontWeights.Normal;
            string column = parameter as string;
            string key = column;
            Log.Assert(rowView != null, nameof(rowView) + " != null");
            if (rowView?.Row.Table != Data_GameText.GameTextForLanguageTable)
            {
                key = Data_GameText.GetRowNameForLanguage(translatedLanguage, column);
            }
            object var = key != null ? rowView?.Row[key] : DBNull.Value;
            return var == DBNull.Value ? FontWeights.Bold : FontWeights.Normal;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 已修改文本加粗
    /// </summary>
    public class DataGridColumnModifyedTextFontWeightConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            DataRowView rowView = values[0] as DataRowView;
            if (values[1] == DependencyProperty.UnsetValue) return FontWeights.Normal;
            EnumLanguage translatedLanguage = (EnumLanguage)Enum.ToObject(typeof(EnumGameUseStatus), values[1]);
            if (translatedLanguage == EnumLanguage.Other) return FontWeights.Normal;
            string key = parameter as string;
            Log.Assert(rowView != null, nameof(rowView) + " != null");
            key = Data_GameText.GetRowNameForLanguage(translatedLanguage, key);
            if (rowView?.Row[key] == null) return FontWeights.Normal;
            EnumGameTextStatus status = (EnumGameTextStatus)Enum.ToObject(typeof(EnumGameTextStatus), rowView.Row[key]);
            return status == EnumGameTextStatus.Modified ? FontWeights.Bold : FontWeights.Normal;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// Galaxy使用文本转换器
    /// </summary>
    public class RichTextBoxInGalaxyTextConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string[] texts = new string[0];
            bool showScript = (bool?)values[2] == true;
            if (values[0] is DataRowView rowView && rowView.Row[Data_GameText.RN_GalaxyLine_Script] is string text && !string.IsNullOrEmpty(text))
            {
                texts = Data_GameText.Const_Regex_StringExternal.Split(text);
            }
            FlowDocument doc = new FlowDocument();
            Paragraph paragraph = new Paragraph
            {
                FontSize = Globals.MainWindow.FontSize,
                FontWeight = FontWeights.Normal
            };
            DataTable table = Globals.CurrentProject.Tables[Data_GameText.TN_GameText];
            Run lastSpace = null;
            foreach (string select in texts)
            {
                DataRow textRow = table.Rows.Find(select);
                if (textRow != null)
                {
                    Run run = new Run
                    {
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Black,
                        Background = Brushes.LightGray,
                        Tag = select,
                    };
                    if (textRow[Data_GameText.RN_GameText_Index] is int)
                    {
                        run.ToolTip = Globals.GetStringFromCurrentLanguage("TP_RichTextBoxInGalaxyTextConverter_ToolTip", select, textRow[Data_GameText.RN_GameText_Index]);
                    }
                    string keyEdited = Data_GameText.GetRowNameForLanguage((EnumLanguage)values[1], Data_GameText.RN_GameText_EditedText);
                    if (textRow[keyEdited] is string runText && !string.IsNullOrEmpty(runText))
                    {
                        run.Text = runText;
                    }
                    else
                    {
                        run.TextDecorations = TextDecorations.Strikethrough;
                        run.Text = Globals.GetStringFromCurrentLanguage("TEXT_NoText");
                    }
                    System.Windows.Controls.MenuItem item = new System.Windows.Controls.MenuItem()
                    {
                        Tag = run,
                        Header = Globals.GetStringFromCurrentLanguage("UI_MenuItem_JumpToText_Header"),
                    };
                    item.Click += SC2_GameTranslater_Window.SC2_GameTranslater_MenuItemJumptToText_Click;
                    System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu();
                    menu.Items.Add(item);
                    run.ContextMenu = menu;
                    paragraph.Inlines.Add(run);
                    if (!showScript)
                    {
                        lastSpace = new Run()
                        {
                            Text = " + ",
                        };
                        paragraph.Inlines.Add(lastSpace);
                    }
                }
                else
                {
                    if (showScript)
                    {
                        Run r = new Run
                        {
                            Text = select
                        };
                        paragraph.Inlines.Add(r);
                    }
                }
            }
            if (lastSpace != null) paragraph.Inlines.Remove(lastSpace);
            doc.Blocks.Add(paragraph);
            return doc;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 取反转换
    /// </summary>
    public class CommonNotValueConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((bool?)value)
            {
                case true:
                    return false;
                case false:
                    return true;
                default:
                    return value;
            }
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((bool?)value)
            {
                case true:
                    return false;
                case false:
                    return true;
                default:
                    return value;
            }
        }
    }

    /// <summary>
    /// 枚举值翻译Converter
    /// </summary>
    public class SerachLocationByRegexControlConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)values[0] && !(bool)values[1];
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 复制源Header可用性转换器
    /// </summary>
    public class LanguageButtonHeaderConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values[0] != null && values[0] is Fluent.Button button)
                {
                    return button.Header;
                }
                else
                {
                    return Globals.GetStringFromCurrentLanguage("TEXT_Null");
                }
            }
            catch
            {
                // For Designer
                return "";
            }
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 复制源Tag可用性转换器
    /// </summary>
    public class LanguageButtonTagConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Fluent.Button button)
            {
                return button.Tag;
            }
            return EnumLanguage.Other;
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 复制源Icon可用性转换器
    /// </summary>
    public class LanguageButtonIconConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Fluent.Button button)
            {
                return button.Icon;
            }
            return Application.Current.FindResource("IMAGE_Null");
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 复制源LargeIcon可用性转换器
    /// </summary>
    public class LanguageButtonLargeIconConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Fluent.Button button)
            {
                return button.LargeIcon;
            }
            return Application.Current.FindResource("IMAGE_Null");
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 复制目标语言可用性转换器
    /// </summary>
    public class CopyTargetLanguageButtonEnableConverter : IValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EnumLanguage sourceLanguage && parameter is ToggleButton targetButton && targetButton.Tag is EnumLanguage targetLanguage)
            {
                return targetLanguage != sourceLanguage;
            }
            return false;
        }

        /// <summary>
        /// 反向转回函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 跳转窗口确定可用性转换器
    /// </summary>
    public class GoToIndexConfirmButtonEnableConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string text && values[1] is int max)
            {

                int index = int.Parse(text);
                return index >= 0 && index < max;
            }
            return false;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetTypes">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    #endregion

    #region Control

    /// <summary>
    /// Ribbon标题
    /// </summary>
    public class RibbonLabel : Control
    {
        #region 属性字段

        #region 属性

        /// <summary>
        /// Header依赖项
        /// </summary>
        public static DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(RibbonLabel));

        /// <summary>
        /// Header依赖项属性
        /// </summary>
        public object Header { set => SetValue(HeaderProperty, value); get => GetValue(HeaderProperty); }

        /// <summary>
        /// Icon依赖项
        /// </summary>
        public static DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(object), typeof(RibbonLabel));

        /// <summary>
        /// Icon依赖项属性
        /// </summary>
        public object Icon { set => SetValue(IconProperty, value); get => (int)GetValue(IconProperty); }
               
        #endregion

        #endregion
    }

    public class SearchButton : System.Windows.Controls.Button
    {
        #region 属性

        #region New

        /// <summary>
        /// 通用图片依赖项属性
        /// </summary>
        public static readonly DependencyProperty NormalImageProperty = DependencyProperty.Register(nameof(NormalImage), typeof(BitmapImage), typeof(SearchButton));

        /// <summary>
        /// 通用图片依赖项
        /// </summary>
        public BitmapImage NormalImage
        {
            set { SetValue(NormalImageProperty, value); }
            get { return (BitmapImage)GetValue(NormalImageProperty); }
        }

        /// <summary>
        /// 填充模式依赖项属性
        /// </summary>
        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(nameof(ImageStretch), typeof(Stretch), typeof(SearchButton), new PropertyMetadata(Stretch.None));

        /// <summary>
        /// 填充模式依赖项
        /// </summary>
        public Stretch ImageStretch
        {
            set { SetValue(ImageStretchProperty, value); }
            get { return (Stretch)GetValue(ImageStretchProperty); }
        }

        #endregion

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        static SearchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchButton), new FrameworkPropertyMetadata(typeof(SearchButton)));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SearchButton() : base()
        {
            Style = Application.Current.Resources["SearchButtonStyle"] as Style;
        }

        #endregion

        #region 方法
        
        #endregion
    }

    /// <summary>
    /// Button 按钮
    /// </summary>
    public class ImageButton : System.Windows.Controls.Button
    {
        #region 属性
        /// <summary>
        /// 通用图片依赖项属性
        /// </summary>
        public static readonly DependencyProperty NormalImageProperty = DependencyProperty.Register(nameof(NormalImage), typeof(BitmapImage), typeof(ImageButton));

        /// <summary>
        /// 通用图片依赖项
        /// </summary>
        public BitmapImage NormalImage
        {
            set { SetValue(NormalImageProperty, value); }
            get { return (BitmapImage)GetValue(NormalImageProperty); }
        }

        /// <summary>
        /// 高亮图片依赖项属性
        /// </summary>
        public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register(nameof(HoverImage), typeof(BitmapImage), typeof(ImageButton));

        /// <summary>
        /// 高亮图片依赖项
        /// </summary>
        public BitmapImage HoverImage
        {
            set { SetValue(HoverImageProperty, value); }
            get { return (BitmapImage)GetValue(HoverImageProperty); }
        }

        /// <summary>
        /// 按下图片依赖项属性
        /// </summary>
        public static readonly DependencyProperty PressImageProperty = DependencyProperty.Register(nameof(PressImage), typeof(BitmapImage), typeof(ImageButton));

        /// <summary>
        /// 按下图片依赖项
        /// </summary>
        public BitmapImage PressImage
        {
            set { SetValue(PressImageProperty, value); }
            get { return (BitmapImage)GetValue(PressImageProperty); }
        }

        /// <summary>
        /// 禁用图片依赖项属性
        /// </summary>
        public static readonly DependencyProperty DisableImageProperty = DependencyProperty.Register(nameof(DisableImage), typeof(BitmapImage), typeof(ImageButton));

        /// <summary>
        /// 禁用图片依赖项
        /// </summary>
        public BitmapImage DisableImage
        {
            set { SetValue(DisableImageProperty, value); }
            get { return (BitmapImage)GetValue(DisableImageProperty); }
        }

        /// <summary>
        /// 选定图片依赖项属性
        /// </summary>
        public static readonly DependencyProperty CheckedImageProperty = DependencyProperty.Register(nameof(CheckedImage), typeof(BitmapImage), typeof(ImageButton));

        /// <summary>
        /// 选定图片依赖项
        /// </summary>
        public BitmapImage CheckedImage
        {
            set { SetValue(CheckedImageProperty, value); }
            get { return (BitmapImage)GetValue(CheckedImageProperty); }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImageButton() : base()
        {
            Style = Application.Current.Resources["ImageButtonStyle"] as Style;
        }

        #endregion

        #region 方法
        #endregion
    }

    #endregion
}
