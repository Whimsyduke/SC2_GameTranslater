using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SC2_GameTranslater.Source
{
    #region Converter

    /// <summary>
    /// 自动序号Converter
    /// </summary>
    public class AutoNumberValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值数组</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = values[0];
            var items = values[1] as ItemCollection;

            var index = items.IndexOf(item);
            return (index + 1).ToString();
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
    /// 枚举值翻译Converter
    /// </summary>
    public class EnumNameInLanguage_TextFileConverter : IMultiValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="value">值数组</param>
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

    #endregion
}
