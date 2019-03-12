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

        #region 属性

        /// <summary>
        /// 重载语言列表依赖项
        /// </summary>
        public static DependencyProperty ReloadLanguageListProperty = DependencyProperty.Register(nameof(ReloadLanguageList), typeof(List<EnumLanguage>), typeof(SC2_GameTranslater_Reload), new PropertyMetadata(new List<EnumLanguage>()));

        /// <summary>
        /// 重载语言列表依赖项属性
        /// </summary>
        public List<EnumLanguage> ReloadLanguageList { set => SetValue(ReloadLanguageListProperty, value); get => (List<EnumLanguage>)GetValue(ReloadLanguageListProperty); }

        #endregion

        #region 字段

        /// <summary>
        /// 接受回调
        /// </summary>
        private Delegate_CallBackReloadTranslateConfig mCallbackApprove = null;

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
                CheckBox checkBox = new CheckBox()
                {
                    IsChecked = true,
                    Content = Globals.CurrentLanguage["TEXT_" + langName],
                    Tag = language,
                    Name = langName,
                };
                checkBox.SetValue(Grid.ColumnProperty, x++);
                checkBox.SetValue(Grid.RowProperty, y++);
                checkBox.Checked += CheckBox_ReloadLanguge_CheckEvent;
                checkBox.Unchecked += CheckBox_ReloadLanguge_CheckEvent;

                ReloadLanguageCheckBoxEnableConverter converter = new ReloadLanguageCheckBoxEnableConverter();
                MultiBinding multiBinding = new MultiBinding()
                {
                    Converter = converter,
                };
                Binding binding = new Binding();
                binding.Path = new PropertyPath("IsEnable");
                binding.ElementName = langName;
                multiBinding.Bindings.Add(binding);
                binding = new Binding();
                binding.Path = new PropertyPath("ReloadLanguageList");
                binding.ElementName = "Window_ReloadConfig";
                multiBinding.Bindings.Add(binding);

                checkBox.SetBinding(IsEnabledProperty, multiBinding);

                ReloadLanguageList.Add(language);
                Grid_TranslateLanguage.Children.Add(checkBox);
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
            mCallbackApprove?.Invoke(ReloadLanguageList, CheckBox_ReloadOnlyModify.IsChecked == true);
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
            if (sender is CheckBox box)
            {
                EnumLanguage language = (EnumLanguage)box.Tag;
                if (box.IsChecked == true)
                {
                    ReloadLanguageList.Add(language);
                }
                else
                {
                    ReloadLanguageList.Remove(language);
                }
                e.Handled = true;
            }
        }

        #endregion
    }
}
