using System.Windows;
using System.Windows.Data;
using Fluent;
using SC2_GameTranslater.Source;

namespace SC2_GameTranslater
{
    using Globals = Class_Globals;

    /// <summary>
    /// SC2_GameTranslater_GoToIndex.xaml 的交互逻辑
    /// </summary>
    public partial class SC2_GameTranslater_GoToIndex : RibbonWindow
    {

        #region 属性字段

        #region 依赖项属性

        /// <summary>
        /// 跳转序号值值依赖项属性
        /// </summary>
        public static DependencyProperty GoToIndexProperty = DependencyProperty.Register(nameof(GoToIndex), typeof(int), typeof(SC2_GameTranslater_GoToIndex), new PropertyMetadata(0));

        /// <summary>
        /// 跳转序号值值依赖项
        /// </summary>
        public int GoToIndex { get => (int)GetValue(GoToIndexProperty); set => SetValue(GoToIndexProperty, value); }

        /// <summary>
        /// 最大序号值值依赖项属性
        /// </summary>
        public static DependencyProperty MaxIndexProperty = DependencyProperty.Register(nameof(MaxIndex), typeof(int), typeof(SC2_GameTranslater_GoToIndex), new PropertyMetadata(1));

        /// <summary>
        /// 最大序号值值依赖项
        /// </summary>
        public int MaxIndex { get => (int)GetValue(MaxIndexProperty); set => SetValue(MaxIndexProperty, value); }

        #endregion

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SC2_GameTranslater_GoToIndex(int maxIndex)
        {
            InitializeComponent();
            GroupBox_GoToIndex.Header = Globals.GetStringFromCurrentLanguage("UI_GroupBox_GoToIndex_Header", maxIndex);
            MaxIndex = maxIndex;
            Binding binding = new Binding("Value")
            {
                ElementName = "IntegerUpDown_GoToIndex",
            };
            SetBinding(GoToIndexProperty, binding);
        }

        #endregion

        #region 控件事件

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GoToIndex = int.Parse(TextBox_GoToIndex.Text);
            }
            catch
            {
                GoToIndex = 0;
            }
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

        #endregion

    }
}
