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
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SC2_GameTranslater_Reload()
        {
            InitializeComponent();
            ResourceDictionary_WindowLanguage = Globals.CurrentLanguage;
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
            //Globals.MainWindow.CallBackReloadTranslateConfig();
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

        #endregion
    }
}
