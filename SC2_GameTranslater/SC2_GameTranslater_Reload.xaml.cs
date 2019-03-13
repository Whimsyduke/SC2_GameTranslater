using SC2_GameTranslater.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SC2_GameTranslater
{
    using Globals = Source.Class_Globals;
    using Preference = Source.Class_Preference;
    using Log = Source.Class_Log;
    using EnumLanguage = Source.EnumLanguage;

    /// <summary>
    /// Interaction logic for SC2_GameTranslater_Reload.xaml
    /// </summary>
    public partial class SC2_GameTranslater_Reload : Window
    {
        #region 声明

        /// <summary>
        /// 重载翻译配置回调委托
        /// </summary>
        /// <param name="languages">重载语言列表</param>
        /// <param name="onlyModified">仅重载修改内容</param>
        public delegate void Delegate_CallBackReloadTranslateConfig(List<EnumLanguage> languages, bool onlyModified);

        #endregion

        #region 属性字段

        #region 字段

        /// <summary>
        /// 接受回调
        /// </summary>
        private Delegate_CallBackReloadTranslateConfig mCallbackApprove = null;
        private Dictionary<EnumLanguage, CheckBox> mDirtLanguageCheckBox = new Dictionary<EnumLanguage, CheckBox>();

        #endregion

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SC2_GameTranslater_Reload(List<EnumLanguage> listLang, Delegate_CallBackReloadTranslateConfig callback)
        {
            InitializeComponent();
            ResourceDictionary_WindowLanguage = Globals.CurrentLanguage;
            mCallbackApprove = callback;
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
                    Grid_TranslateLanguage.RowDefinitions.Add(rowDef);
                }
                string langName = Enum.GetName(language.GetType(), language);
                CheckBox checkBox = ResourceDictionary_MainGrid["CheckBox_" + langName] as CheckBox;
                checkBox.SetValue(Grid.ColumnProperty, x++);
                checkBox.SetValue(Grid.RowProperty, y++);
                mDirtLanguageCheckBox.Add(language, checkBox);
                Grid_TranslateLanguage.Children.Add(checkBox);
            }
        }

        /// <summary>
        /// 刷新重载语言CheckBox
        /// </summary>
        private void RefreshReloadLanguageCheckBox()
        {
            int count = 0;
            CheckBox enableCheckBox = null;
            foreach (KeyValuePair<EnumLanguage, CheckBox> item in mDirtLanguageCheckBox)
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
            Close();
            List<EnumLanguage> languages = new List<EnumLanguage>();
            foreach (KeyValuePair<EnumLanguage, CheckBox> item in mDirtLanguageCheckBox)
            {
                languages.Add(item.Key);
            }
            mCallbackApprove?.Invoke(languages, CheckBox_ReloadOnlyModify.IsChecked == true);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
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
