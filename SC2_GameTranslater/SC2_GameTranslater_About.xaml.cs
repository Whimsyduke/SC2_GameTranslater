using Fluent;
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
using SC2_GameTranslater.Source;

namespace SC2_GameTranslater
{
    using Globals = Class_Globals;

    /// <summary>
    /// Interaction logic for SC2_GameTranslater_About.xaml
    /// </summary>
    public partial class SC2_GameTranslater_About : RibbonWindow
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SC2_GameTranslater_About()
        {
            InitializeComponent();
            ResourceDictionary_WindowLanguage.MergedDictionaries.Clear();
            ResourceDictionary_WindowLanguage.MergedDictionaries.Add(Globals.CurrentLanguage);
            if (Globals.MainWindow.EnumCurrentLanguage != EnumLanguage.zhCN)
            {
                Image_Alipay.Visibility = Visibility.Hidden;
                Image_Paypal.Visibility = Visibility.Visible;
                TextBlock_Alipay.Visibility = Visibility.Hidden;
                Button_Donate.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region 控件方法

        /// <summary>
        /// 点击OK事件
        /// </summary>
        /// <param name="sender">响应控件</param>
        /// <param name="e">响应参数</param>
        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        /// <summary>
        /// 点击联系邮箱
        /// </summary>
        /// <param name="sender">响应控件</param>
        /// <param name="e">响应参数</param>
        private void Hyperlink_EmailClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:whimsyduke@163.com?subject= &body= ");
        }

        /// <summary>
        /// 点击捐赠按钮(PayPal)
        /// </summary>
        /// <param name="sender">响应控件</param>
        /// <param name="e">响应参数</param>
        private void Button_DonatePayPal_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.me/Froggyandcatty");
        }

        /// <summary>
        /// 点击更新页地址
        /// </summary>
        /// <param name="sender">响应控件</param>
        /// <param name="e">响应参数</param>
        private void Hyperlink_Update_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Whimsyduke/SC2_GameTranslater/releases");
        }

        /// <summary>
        /// 点击源码页地址
        /// </summary>
        /// <param name="sender">响应控件</param>
        /// <param name="e">响应参数</param>
        private void Hyperlink_Source_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Whimsyduke/SC2_GameTranslater");
        }
        #endregion
    }
}
