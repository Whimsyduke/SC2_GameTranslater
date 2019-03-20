using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SC2_GameTranslater
{
    using Globals = Source.Class_Globals;
    using Preference = Source.Class_Preference;
    using Log = Source.Class_Log;
    using EnumLanguage = Source.EnumLanguage;

    /// <summary>
    /// Interaction logic for SC2_GameTranslater_Reload.xaml
    /// </summary>
    public partial class SC2_GameTranslater_Reload
    {
        #region 属性字段

        #region 属性

        /// <summary>
        /// CheckBox字典
        /// </summary>
        public Dictionary<EnumLanguage, CheckBox> DirtLanguageCheckBox { set; get; } = new Dictionary<EnumLanguage, CheckBox>();

        #endregion

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SC2_GameTranslater_Reload(List<EnumLanguage> listLang)
        {
            InitializeComponent();
            ResourceDictionary_WindowLanguage.MergedDictionaries.Clear();
            ResourceDictionary_WindowLanguage.MergedDictionaries.Add(Globals.CurrentLanguage);
            int x = 0;
            int y = 0;
            foreach (EnumLanguage language in listLang)
            {
                if (x == 4)
                {
                    x = 0;
                    y++;
                    RowDefinition rowDef = new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    };
                    Grid_TranslatedLanguage.RowDefinitions.Add(rowDef);
                }
                string langName = Enum.GetName(language.GetType(), language);
                if (ResourceDictionary_MainGrid["CheckBox_" + langName] is CheckBox checkBox)
                {
                    checkBox.SetValue(Grid.ColumnProperty, x++);
                    checkBox.SetValue(Grid.RowProperty, y++);
                    DirtLanguageCheckBox.Add(language, checkBox);
                    Grid_TranslatedLanguage.Children.Add(checkBox);
                }
            }
        }

        /// <summary>
        /// 刷新重载语言CheckBox
        /// </summary>
        private void RefreshReloadLanguageCheckBox()
        {
            int count = 0;
            CheckBox enableCheckBox = null;
            foreach (KeyValuePair<EnumLanguage, CheckBox> item in DirtLanguageCheckBox)
            {
                if (item.Value.IsChecked == true)
                {
                    count++;
                    enableCheckBox = item.Value;
                }
                item.Value.IsEnabled = true;
            }
            if (count == 1)
            {
                enableCheckBox.IsEnabled = false;
            }
        }

        #endregion

        #region 控件方法

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// 重载语言选择事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void CheckBox_ReloadLanguge_CheckEvent(object sender, RoutedEventArgs e)
        {
            RefreshReloadLanguageCheckBox();
        }

        #endregion
    }
}
