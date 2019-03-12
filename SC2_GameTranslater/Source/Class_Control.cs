using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace SC2_GameTranslater.Source
{
    using Globals = Class_Globals;

    #region Converter

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
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == null || values[1] == null) return 0;
            var item = values[0];
            if (values[1] is ItemCollection items)
            {
                var index = items.IndexOf(item);
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
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
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
            EnumLanguage language = (EnumLanguage)value;
            string langName = Enum.GetName(language.GetType(), language);
            return Globals.CurrentLanguage[string.Format("TEXT_{0}", langName)];
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
    public class EnumTranslateLanguageDataConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataRowView rowView = values[0] as DataRowView;
            EnumLanguage language = (EnumLanguage)values[1];
            string column = parameter as string;
            string key = Data_GameText.GetGameRowNameForLanguage(language, column);
            object display = rowView.Row[key];
            if (display == DBNull.Value)
            {
                return Globals.CurrentLanguage["TEXT_NoText"];
            }
            else
            {
                return display as string;
            }
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 语言枚举值翻译Converter
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
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataRowView rowView = values[0] as DataRowView;
            EnumLanguage language = (EnumLanguage)values[1];
            var var = rowView.Row[Data_GameText.RN_GameText_File];
            EnumGameTextFile value = (EnumGameTextFile)Enum.ToObject(typeof(EnumGameTextFile), var);
            return Data_GameText.GetEnumNameInLanguage(language, value);
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 语言枚举值翻译Converter
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
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataRowView rowView = values[0] as DataRowView;
            EnumLanguage translateLanguage = (EnumLanguage)values[1];
            EnumLanguage softeareLanguage = (EnumLanguage)values[2];
            string column = parameter as string;
            string key;
            if (rowView.Row.Table == Data_GameText.GameTextForLanguageTable)
            {
                key = column;
            }
            else
            {
                key = Data_GameText.GetGameRowNameForLanguage(translateLanguage, column);
            }
            var var = rowView.Row[key];
            EnumGameTextStatus value = (EnumGameTextStatus)Enum.ToObject(typeof(EnumGameTextStatus), var);
            return Data_GameText.GetEnumNameInLanguage(softeareLanguage, value);
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }

    /// <summary>
    /// 语言枚举值翻译Converter
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
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataRowView rowView = values[0] as DataRowView;
            EnumLanguage translateLanguage = (EnumLanguage)values[1];
            EnumLanguage softeareLanguage = (EnumLanguage)values[2];
            string column = parameter as string;
            string key;
            if (rowView.Row.Table == Data_GameText.GameTextForLanguageTable)
            {
                key = column;
            }
            else
            {
                key = Data_GameText.GetGameRowNameForLanguage(translateLanguage, column);
            }
            var var = rowView.Row[key];
            EnumGameUseStatus value = (EnumGameUseStatus)Enum.ToObject(typeof(EnumGameUseStatus), var);
            return Data_GameText.GetEnumNameInLanguage(softeareLanguage, value);
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
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
            DataRowView view = value as DataRowView;
            DataRow rowFile = view.Row.GetParentRow(Data_GameText.RSN_GalaxyFile_GalaxyLine_File);
            return rowFile[Data_GameText.RN_GalaxyFile_Name];
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
    /// 空文本表格边框
    /// </summary>
    public class DataGridColumnNullTextBorderConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue) return ConsoleColor.Gray;
            DataRowView rowView = values[0] as DataRowView;
            EnumLanguage translateLanguage = (EnumLanguage)values[1];
            string column = parameter as string;
            string key;
            if (rowView.Row.Table == Data_GameText.GameTextForLanguageTable)
            {
                key = column;
            }
            else
            {
                key = Data_GameText.GetGameRowNameForLanguage(translateLanguage, column);
            }
            var var = rowView.Row[key];
            return var == System.DBNull.Value ? FontWeights.Bold: FontWeights.Normal;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
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
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataRowView rowView = values[0] as DataRowView;
            DataRow row = rowView.Row;
            string text = row[Data_GameText.RN_GalaxyLine_Script] as string;
            string[] texts = Data_GameText.Const_Regex_StringExternal.Split(text);
            FlowDocument doc = new FlowDocument();
            Paragraph paragraph = new Paragraph
            {
                FontSize = Globals.MainWindow.FontSize,
                FontWeight = FontWeights.Normal
            };
            DataTable table = Globals.CurrentProject.Tables[Data_GameText.TN_GameText];
            foreach (string select in texts)
            {
                Run r = new Run();
                DataRow textRow = table.Rows.Find(select);
                if (textRow != null)
                {
                    r.MouseLeftButtonDown += SC2_GameTranslater_Window.Run_MouseLeftButtonDown;
                    r.FontWeight = FontWeights.Bold;
                    r.ToolTip = select;
                    string key = Data_GameText.GetGameRowNameForLanguage((EnumLanguage)values[1], Data_GameText.RN_GameText_EditedText);
                    r.Text = textRow[key] as string;
                    r.Foreground = Brushes.Red;
                }
                else
                {
                    r.Text = select;
                }
                paragraph.Inlines.Add(r);
            }
            doc.Blocks.Add(paragraph);
            return doc;
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
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
    /// 布尔值可见性转换器
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
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
                    return Visibility.Visible;
                case false:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
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
            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return true; ;
                case Visibility.Hidden:
                case Visibility.Collapsed:
                default:
                    return false;
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
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)values[0] && !(bool)values[1];
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    /// <summary>
    /// 重载翻译启用Converter
    /// </summary>
    public class ReloadLanguageCheckBoxEnableConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="values">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue) return false;
            return !((bool)values[0] && (values[1] as List<EnumLanguage>).Count == 1);
        }

        /// <summary>
        /// 逆向转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }

    #endregion

    #region Control

    /// <summary>
    /// 富文本控件模板
    /// </summary>
    public class DataGridRichTextBoxTemplate : RichTextBox
    {
        #region 属性字段

        #region 属性

        /// <summary>
        /// 富文本内容依赖项属性
        /// </summary>
        public static readonly DependencyProperty DocumentTextProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(DataGridRichTextBoxTemplate), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDocumentChanged)));

        /// <summary>
        /// 富文本内容依赖项
        /// </summary>
        public new FlowDocument Document { set => SetValue(DocumentTextProperty, value); get => (FlowDocument)GetValue(DocumentTextProperty); }

        #endregion

        #region 字段

        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 文本变化回调
        /// </summary>
        /// <param name="dp">依赖项</param>
        /// <param name="args">参数</param>
        public static void OnDocumentChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            RichTextBox textBox = (RichTextBox)dp;
            textBox.Document = (FlowDocument)args.NewValue;
        }
        #endregion
    }

    #endregion
}
