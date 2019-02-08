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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fluent;
using Fluent.Localization;
using System.IO;
using System.Globalization;
using System.Reflection;

using Globals = SC2_GameTranslater.Source.Class_Globals;
using Log = SC2_GameTranslater.Source.Class_Log;
using EnumLanguage = SC2_GameTranslater.Source.EnumLanguage;

namespace SC2_GameTranslater
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SC2_GameTranslater_Window : RibbonWindow
    {
        #region 命令

        /// <summary>
        /// 打开命令依赖项
        /// </summary>
        public static DependencyProperty CommandOpenProperty = DependencyProperty.Register("CommandOpen", typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 打开命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandOpen { set => SetValue(CommandOpenProperty, value); get => (RoutedUICommand)GetValue(CommandOpenProperty); }
                
        /// <summary>
        /// 保存命令依赖项
        /// </summary>
        public static DependencyProperty CommandSaveProperty = DependencyProperty.Register("CommandSave", typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 保存命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandSave { set => SetValue(CommandSaveProperty, value); get => (RoutedUICommand)GetValue(CommandSaveProperty); }

        /// <summary>
        /// 另存为命令依赖项
        /// </summary>
        public static DependencyProperty CommandSaveAsProperty = DependencyProperty.Register("CommandSaveAs", typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 另存为命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandSaveAs { set => SetValue(CommandSaveAsProperty, value); get => (RoutedUICommand)GetValue(CommandSaveAsProperty); }

        /// <summary>
        /// 刷新命令依赖项
        /// </summary>
        public static DependencyProperty CommandReloadProperty = DependencyProperty.Register("CommandReload", typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 刷新命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandReload { set => SetValue(CommandReloadProperty, value); get => (RoutedUICommand)GetValue(CommandReloadProperty); }

        /// <summary>
        /// 应用命令依赖项
        /// </summary>
        public static DependencyProperty CommandAcceptProperty = DependencyProperty.Register("CommandAccept", typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 应用命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandAccept { set => SetValue(CommandAcceptProperty, value); get => (RoutedUICommand)GetValue(CommandAcceptProperty); }

        /// <summary>
        /// 关闭命令依赖项
        /// </summary>
        public static DependencyProperty CommandCloseProperty = DependencyProperty.Register("CommandClose", typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 关闭命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandClose { set => SetValue(CommandCloseProperty, value); get => (RoutedUICommand)GetValue(CommandCloseProperty); }

        #endregion

        #region 字段属性

        #region 属性

        /// <summary>
        /// 语言依赖项属性
        /// </summary>
        public static DependencyProperty EnumCurrentLanguageProperty = DependencyProperty.Register("EnumCurrentLanguage", typeof(EnumLanguage), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前语言依赖项
        /// </summary>
        public EnumLanguage EnumCurrentLanguage
        {
            set
            {
                SetValue(EnumCurrentLanguageProperty, value);
                ResourceDictionary_WindowLanguage.MergedDictionaries.Clear();
                Globals.CurrentLanguage = Globals.DictUILanguages[value];
                ResourceDictionary_WindowLanguage.MergedDictionaries.Add(Globals.CurrentLanguage);
                RibbonLocalization.Current.Localization = Globals.FluentLocalizationMap[value];
                Title = Globals.CurrentLanguage["TEXT_WindowTitleText"].ToString() + " V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            get
            {
                return ((EnumLanguage)GetValue(EnumCurrentLanguageProperty));
            }
        }
        #endregion

        #region 字段
        #endregion

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SC2_GameTranslater_Window()
        {
            Globals.MainWindow = this;
            InitializeComponent();
            
            #region 多语言配置
            bool useDefault = true;
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (EnumLanguage select in Enum.GetValues(typeof(EnumLanguage)))
            {
                string TEXT_LanguageName = Enum.GetName(typeof(EnumLanguage), select);
                string fileName = "Language/" + TEXT_LanguageName + ".xaml";
                FileInfo file = new FileInfo(fileName);
                ResourceDictionary language = new ResourceDictionary();
                if (file.Exists)
                {
                    language.Source = new Uri(file.FullName);
                }
                else
                {
                    switch (select)
                    {
                        case EnumLanguage.zhCN:
                            language.Source = new Uri("pack://application:,,,/" + fileName);
                            break;
                        case EnumLanguage.enUS:
                            language.Source = new Uri("pack://application:,,,/" + fileName);
                            break;
                        default:
                            continue;
                    }
                }
                string itemName = language["TEXT_LanguageName"] as string;
                Globals.DictComboBoxItemLanguage.Add(itemName, select);
                Globals.DictUILanguages.Add(select, language);
                Globals.FluentLocalizationMap[select] = assembly.CreateInstance("SC2_GameTranslater.Source.RibbonLanguage_" + Enum.GetName(typeof(EnumLanguage), select)) as RibbonLocalizationBase;
                ComboBox_Language.Items.Add(itemName);
                if (CultureInfo.CurrentCulture.LCID == (int)select)
                {
                    ComboBox_Language.SelectedItem = itemName;
                    useDefault = false;
                }
            }
            if (useDefault)
            {
                ComboBox_Language.SelectedItem = Globals.DictUILanguages[EnumLanguage.enUS]["TEXT_LanguageName"];
            }
            #endregion

            #region 指令配置
            CommandBinding binding;
            binding = new CommandBinding(CommandOpen, Executed_Open, CanExecuted_Open);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandSave, Executed_Save, CanExecuted_Save);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandSaveAs, Executed_SaveAs, CanExecuted_SaveAs);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandReload, Executed_Reload, CanExecuted_Reload);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandAccept, Executed_Accept, CanExecuted_Accept);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandClose, Executed_Close, CanExecuted_Close);
            #endregion
        }
        #endregion

        #region 方法

        #region 命令

        /// <summary>
        /// 打开项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Open(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            if (Directory.Exists(Globals.LastOpenPath))
            {
                fileDialog.InitialDirectory = Globals.LastOpenPath;
            }
            else
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
#if true

            fileDialog.Filter = Globals.CurrentLanguage["TEXT_ProjectFile"] as string + "|*" + Globals.Extension_SC2GameTran + "|" + Globals.CurrentLanguage["TEXT_SC2File"] as string + "|" + Globals.FileName_SC2Components;
#else            
            fileDialog.Filter = Globals.CurrentLanguage["TEXT_ProjectFile"] as string + "|*" + Globals.Extension_SC2GameTran + "|" + Globals.CurrentLanguage["TEXT_SC2File"] as string + "|*" + Globals.Extension_SC2Map + ";*" + Globals.Extension_SC2Mod + ";" + Globals.FileName_SC2Components;
#endif
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Title = Globals.CurrentLanguage["UI_OpenFileDialog_Open_Title"] as string;
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (fileDialog.FilterIndex == 1)
                {
                    Globals.MainWindow.ProjectOpen(new FileInfo(fileDialog.FileName));
                }
                else
                {
                    Globals.MainWindow.ProjectNew(new FileInfo(fileDialog.FileName));
                }
            }
            
            e.Handled = true;
        }

        /// <summary>
        /// 打开项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Open(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        /// <summary>
        ///接受项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Save");
            e.Handled = true;
        }

        /// <summary>
        /// 保存项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.MainWindow.CheckCurrentProjectExist();
            e.Handled = true;
        }
        
        /// <summary>
        ///另存为项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("SaveAs");
            e.Handled = true;
        }

        /// <summary>
        /// 另存为项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_SaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.MainWindow.CheckCurrentProjectExist();
            e.Handled = true;
        }

        /// <summary>
        /// 刷新项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Reload(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Reload");
            e.Handled = true;
        }

        /// <summary>
        /// 刷新项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Reload(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.MainWindow.CheckCurrentProjectExist();
            e.Handled = true;
        }

        /// <summary>
        /// 应用项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Accept(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Accept");
            e.Handled = true;
        }

        /// <summary>
        /// 应用项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Accept(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.MainWindow.CheckCurrentProjectExist();
            e.Handled = true;
        }

        /// <summary>
        ///关闭项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            Globals.MainWindow.ProjectClose();
            e.Handled = true;
        }

        /// <summary>
        /// 关闭项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Close(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.MainWindow.CheckCurrentProjectExist();
            e.Handled = true;
        }

        #endregion

        #region 功能

        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="file">文件路径</param>
        public void ProjectOpen(FileInfo file)
        {
            ProjectClose();
        }

        /// <summary>
        /// 新建项目
        /// </summary>
        /// <param name="file">文件路径</param>
        public void ProjectNew(FileInfo file)
        {
            ProjectClose();
            Globals.InitProjectData();
            Globals.CurrentProject.Initialization(file);
        }

        /// <summary>
        /// 重载数据
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public void ProjectReload(FileInfo file)
        {
            Log.Assert(Globals.CurrentProject != null);
            Globals.CurrentProject.ReloadFile(file);
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public void ProjectClose()
        {
            
        }

        /// <summary>
        /// 检测当前项目存在
        /// </summary>
        /// <returns></returns>
        public bool CheckCurrentProjectExist()
        {
            return Globals.CurrentProject != null;
        }
        #endregion

        #endregion

        #region 控件事件

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemName = ComboBox_Language.SelectedItem as string;
            EnumCurrentLanguage = Globals.DictComboBoxItemLanguage[itemName];
        }

        #endregion

    }
}
