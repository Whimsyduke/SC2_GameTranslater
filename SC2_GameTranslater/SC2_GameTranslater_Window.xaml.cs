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
using System.Threading;
using SC2_GameTranslater.Source;
using System.Data;
using System.Text.RegularExpressions;

namespace SC2_GameTranslater
{
    using Globals = Source.Class_Globals;
    using Preference = Source.Class_Preference;
    using Log = Source.Class_Log;
    using EnumLanguage = Source.EnumLanguage;

    #region 声明

    /// <summary>
    /// 搜索文本类型
    /// </summary>
    public enum EnumSearchTextType
    {
        /// <summary>
        /// ID
        /// </summary>
        ID = 1,
        /// <summary>
        /// Index
        /// </summary>
        Index = 2,
        /// <summary>
        /// 原文本
        /// </summary>
        Source = 4,
        /// <summary>
        /// 旧文本
        /// </summary>
        Droped = 8,
        /// <summary>
        /// 修改文本
        /// </summary>
        Edited = 16,
        /// <summary>
        /// 全部文本
        /// </summary>
        AllText = 28,
        /// <summary>
        /// 全部（全部文本及ID）
        /// </summary>
        All = 29,
    }

    /// <summary>
    /// 搜索文本位置
    /// </summary>
    public enum EnumSearchTextLocation
    {
        /// <summary>
        /// 全部
        /// </summary>
        All,
        /// <summary>
        /// 左侧开始
        /// </summary>
        Left,
        /// <summary>
        /// 右侧开始
        /// </summary>
        Right
    }
    #endregion

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SC2_GameTranslater_Window : RibbonWindow
    {
        #region 声明

        /// <summary>
        /// 进度条函数委托
        /// </summary>
        /// <param name="count">当前计数</param>
        /// <param name="max">最大计数</param>
        public delegate void Delegate_ProgressEvent(double count, double max, object param);

        /// <summary>
        /// 在搜索结果委托
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="key">数据列名</param>
        /// <param name="match">匹配字符串</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>判断结果</returns>
        private delegate bool Delegate_IsInSearchResult(DataRow row, string key, string match, bool ignoreCase);

        /// <summary>
        /// 翻译语言控件
        /// </summary>
        public class TranslateLanguageControls
        {
            #region 字段
            /// <summary>
            /// 语言选择按钮
            /// </summary>
            public ToggleButton Button { private set; get; }

            /// <summary>
            /// 搜索列表项
            /// </summary>
            public ComboBoxItem ComboItem { private set; get; }

            /// <summary>
            /// 搜索列表项文本
            /// </summary>
            public TextBlock ComboItemText { private set; get; }

            /// <summary>
            /// 显示列表项
            /// </summary>
            public ListBoxItem ListItem { private set; get; }

            /// <summary>
            /// 显示列表项文本
            /// </summary>
            public TextBlock ListItemText { private set; get; }

            #endregion

            #region 构造函数

            /// <summary>
            /// 构造函数
            /// </summary>
            public TranslateLanguageControls()
            {
                Button = new ToggleButton
                {
                    Tag = null,
                    IsEnabled = false,
                };
                Button.SetResourceReference(ToggleButton.HeaderProperty, "TEXT_Null");
                Button.SetResourceReference(ToggleButton.IconProperty, "IMAGE_Null");
                Button.SetResourceReference(ToggleButton.LargeIconProperty, "IMAGE_Null");
                ComboItemText = new TextBlock();
                ComboItemText.SetResourceReference(TextBlock.TextProperty, "TEXT_All");
                ComboItem = new ComboBoxItem
                {
                    Tag = null,
                    Content = ComboItemText,
                };
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="language">语言</param>
            public TranslateLanguageControls(EnumLanguage language)
            {
                Button = new ToggleButton
                {
                    Tag = language,
                    GroupName = "TranslateLanguage",
                };
                Button.SetResourceReference(ToggleButton.HeaderProperty, string.Format("TEXT_{0}", language));
                Button.SetResourceReference(ToggleButton.IconProperty, string.Format("IMAGE_{0}", language));
                Button.SetResourceReference(ToggleButton.LargeIconProperty, string.Format("IMAGE_{0}", language));
                Button.Checked += Button_TranslateLanguage_Checked;

                ComboItemText = new TextBlock();
                ComboItemText.SetResourceReference(TextBlock.TextProperty, string.Format("TEXT_{0}", language));
                ComboItem = new ComboBoxItem
                {
                    Tag = language,
                    Content = ComboItemText,
                };

                ListItemText = new TextBlock();
                ListItemText.SetResourceReference(TextBlock.TextProperty, string.Format("TEXT_{0}", language));
                ListItem = new ListBoxItem
                {
                    Tag = language,
                    Content = ListItemText,
                };
            }
            #endregion
        }

        #endregion

        #region 命令

        /// <summary>
        /// 新建命令依赖项
        /// </summary>
        public static DependencyProperty CommandNewProperty = DependencyProperty.Register(nameof(CommandNew), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 新建命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandNew { set => SetValue(CommandNewProperty, value); get => (RoutedUICommand)GetValue(CommandNewProperty); }

        /// <summary>
        /// 打开命令依赖项
        /// </summary>
        public static DependencyProperty CommandOpenProperty = DependencyProperty.Register(nameof(CommandOpen), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 打开命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandOpen { set => SetValue(CommandOpenProperty, value); get => (RoutedUICommand)GetValue(CommandOpenProperty); }

        /// <summary>
        /// 保存命令依赖项
        /// </summary>
        public static DependencyProperty CommandSaveProperty = DependencyProperty.Register(nameof(CommandSave), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 保存命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandSave { set => SetValue(CommandSaveProperty, value); get => (RoutedUICommand)GetValue(CommandSaveProperty); }

        /// <summary>
        /// 另存为命令依赖项
        /// </summary>
        public static DependencyProperty CommandSaveAsProperty = DependencyProperty.Register(nameof(CommandSaveAs), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 另存为命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandSaveAs { set => SetValue(CommandSaveAsProperty, value); get => (RoutedUICommand)GetValue(CommandSaveAsProperty); }

        /// <summary>
        /// 关闭命令依赖项
        /// </summary>
        public static DependencyProperty CommandCloseProperty = DependencyProperty.Register(nameof(CommandClose), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 关闭命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandClose { set => SetValue(CommandCloseProperty, value); get => (RoutedUICommand)GetValue(CommandCloseProperty); }

        /// <summary>
        /// 应用命令依赖项
        /// </summary>
        public static DependencyProperty CommandAcceptProperty = DependencyProperty.Register(nameof(CommandAccept), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 应用命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandAccept { set => SetValue(CommandAcceptProperty, value); get => (RoutedUICommand)GetValue(CommandAcceptProperty); }

        /// <summary>
        /// 重载命令依赖项
        /// </summary>
        public static DependencyProperty CommandReloadSC2perty = DependencyProperty.Register(nameof(CommandReload), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 重载命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandReload { set => SetValue(CommandReloadSC2perty, value); get => (RoutedUICommand)GetValue(CommandReloadSC2perty); }

        /// <summary>
        /// 重载(地图/Mod)命令依赖项
        /// </summary>
        public static DependencyProperty CommandReloadSC2Property = DependencyProperty.Register(nameof(CommandReloadSC2), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 重载(地图/Mod)命令依赖项属性
        /// </summary>
        public RoutedUICommand CommandReloadSC2 { set => SetValue(CommandReloadSC2Property, value); get => (RoutedUICommand)GetValue(CommandReloadSC2Property); }

        /// <summary>
        /// 选择Mod/Map命令依赖项
        /// </summary>
        public static DependencyProperty CommandComponentsPathProperty = DependencyProperty.Register(nameof(CommandComponentsPath), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 选择Mod/Map依赖项属性
        /// </summary>
        public RoutedUICommand CommandComponentsPath { set => SetValue(CommandComponentsPathProperty, value); get => (RoutedUICommand)GetValue(CommandComponentsPathProperty); }

        /// <summary>
        /// 近期项目命令依赖项
        /// </summary>
        public static DependencyProperty CommandRecentProjectsProperty = DependencyProperty.Register(nameof(CommandRecentProjects), typeof(RoutedUICommand), typeof(SC2_GameTranslater_Window), new PropertyMetadata(new RoutedUICommand()));

        /// <summary>
        /// 近期项目依赖项属性
        /// </summary>
        public RoutedUICommand CommandRecentProjects { set => SetValue(CommandRecentProjectsProperty, value); get => (RoutedUICommand)GetValue(CommandRecentProjectsProperty); }


        #endregion

        #region 字段属性

        #region 属性

        /// <summary>
        /// 语言依赖项属性
        /// </summary>
        public static DependencyProperty EnumCurrentLanguageProperty = DependencyProperty.Register(nameof(EnumCurrentLanguage), typeof(EnumLanguage), typeof(SC2_GameTranslater_Window));

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

        /// <summary>
        /// 翻译语言
        /// </summary>
        public Dictionary<EnumLanguage, TranslateLanguageControls> TranslateAndSearchLanguage { set; get; } = NewTranslateAndSerachLanguageButton();

        /// <summary>
        /// 是否选择全部Galaxy文件筛选
        /// </summary>
        public bool IsSelectAllGalaxyFilter { private set; get; } = true;

        /// <summary>
        /// Galaxy筛选列表
        /// </summary>
        public List<string> GalaxyFilter { set; get; } = new List<string>();

        /// <summary>
        /// 文本所在文件筛选
        /// </summary>
        public EnumGameTextFile TextFileFilter { private set; get; } = EnumGameTextFile.All;

        /// <summary>
        /// 文本状态筛选
        /// </summary>
        public EnumGameTextStatus TextStatusFilter { private set; get; } = EnumGameTextStatus.All;

        /// <summary>
        /// 使用状态筛选
        /// </summary>
        public EnumGameUseStatus UseStatusFilter { private set; get; } = EnumGameUseStatus.All;

        /// <summary>
        /// 允许刷新翻译文本
        /// </summary>
        public bool CanRefreshTranslatedText { set; get; }

        /// <summary>
        /// 当前文本数据依赖项属性
        /// </summary>
        public static DependencyProperty CurrentTextDataProperty = DependencyProperty.Register(nameof(CurrentTextData), typeof(DataTable), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前文本数据依赖项
        /// </summary>
        public DataTable CurrentTextData { get => (DataTable)GetValue(CurrentTextDataProperty); set => SetValue(CurrentTextDataProperty, value); }

        private Dictionary<EnumSearchTextLocation, Delegate_IsInSearchResult> DictTextSearchLocationFunc { get; } = new Dictionary<EnumSearchTextLocation, Delegate_IsInSearchResult>
        {
            { EnumSearchTextLocation.All, IsInSearchResult_MatchAll},
            { EnumSearchTextLocation.Left, IsInSearchResult_MatchLeft},
            { EnumSearchTextLocation.Right, IsInSearchResult_MatchRight},
        };


        /// <summary>
        /// 当前翻译语言文本数据依赖项属性
        /// </summary>>
        public static DependencyProperty CurrentTranslateLanguageProperty = DependencyProperty.Register(nameof(CurrentTranslateLanguage), typeof(EnumLanguage), typeof(SC2_GameTranslater_Window));


        /// <summary>
        /// 当前翻译语言文本数据依赖项
        /// </summary>
        private EnumLanguage CurrentTranslateLanguage { set => SetValue(CurrentTranslateLanguageProperty, value); get => (EnumLanguage)GetValue(CurrentTranslateLanguageProperty); }

        #endregion

        #region 字段

        private List<ToggleButton> m_GalaxyButtons = new List<ToggleButton>();

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
            binding = new CommandBinding(CommandNew, Executed_New, CanExecuted_New);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandOpen, Executed_Open, CanExecuted_Open);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandSave, Executed_Save, CanExecuted_Save);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandSaveAs, Executed_SaveAs, CanExecuted_SaveAs);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandClose, Executed_Close, CanExecuted_Close);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandAccept, Executed_Accept, CanExecuted_Accept);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandReload, Executed_Reload, CanExecuted_Reload);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandReloadSC2, Executed_ReloadSC2, CanExecuted_ReloadSC2);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandComponentsPath, Executed_ComponentsPath, CanExecuted_ComponentsPath);
            Globals.MainWindow.CommandBindings.Add(binding);
            binding = new CommandBinding(CommandRecentProjects, Executed_RecentProjects, CanExecuted_RecentProjects);
            Globals.MainWindow.CommandBindings.Add(binding);

            #endregion

            #region 其他
            Globals.Preference.LoadPreference();
            Globals.EventProjectChange += OnProjectChangeRefresh;
            OnProjectChangeRefresh(null, null);

            #endregion
        }

        #endregion

        #region 方法

        #region 通用

        /// <summary>
        /// 打开文件浏览器选择文件
        /// </summary>
        /// <param name="pTextBox">设置文件地址的TextBox控件</param>
        /// <param name="pFilter">文件类型筛选字符串</param>
        /// <param name="pTitle">标题</param>
        /// <returns></returns>
        private FileInfo OpenFileDialogGetOpenFile(System.Windows.Controls.TextBox pTextBox, string pFilter, string pTitle)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            if (Directory.Exists(pTextBox.Text))
            {
                fileDialog.InitialDirectory = pTextBox.Text;
            }
            else
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            fileDialog.Filter = pFilter;
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Title = pTitle;
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pTextBox.Text = fileDialog.FileName;
                return new FileInfo(fileDialog.FileName);
            }
            else
            {
                pTextBox.Text = "";
                return null;
            }
        }

        #endregion

        #region 进度条

        /// <summary>
        /// 初始化进度条状态
        /// </summary>
        /// <param name="max">最大值</param>
        /// <param name="msg">初始消息</param>
        /// <param name="param">参数</param>
        /// <param name="func">调用委托</param>
        public void ProgressBarInit(int max, string msg, object param, Delegate_ProgressEvent func)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    Grid_Main.IsEnabled = false;
                    ProgressBar_Loading.Visibility = Visibility.Visible;
                    TextBlock_ProgressMsg.Visibility = Visibility.Visible;
                    ProgressBar_Loading.Maximum = max;
                    TextBlock_ProgressMsg.Text = msg;
                    func?.Invoke(0, max, param);
                });
        }

        /// <summary>
        /// 更新加载进度条
        /// </summary>
        /// <param name="count">计数</param>
        /// <param name="msg">消息</param>
        /// <param name="param">参数</param>
        /// <param name="func">调用委托</param>
        public void ProgressBarUpadte(int count, string msg, object param, Delegate_ProgressEvent func)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    ProgressBar_Loading.Value += count;
                    TextBlock_ProgressMsg.Text = string.Format("({0}/{1}) {2} {3}", ProgressBar_Loading.Value, ProgressBar_Loading.Maximum, Globals.CurrentLanguage["UI_TextBlock_ProgressMsg_Text"], msg);
                    func?.Invoke(ProgressBar_Loading.Value, ProgressBar_Loading.Maximum, param);
                });
        }

        /// <summary>
        /// 清理进度条状态
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="func">调用委托</param>
        public void ProgressBarClean(object param, Delegate_ProgressEvent func)
        {
            Dispatcher.BeginInvoke(priority: System.Windows.Threading.DispatcherPriority.Normal,
                method: (ThreadStart)delegate ()
                {
                    ProgressBar_Loading.Visibility = Visibility.Hidden;
                    TextBlock_ProgressMsg.Visibility = Visibility.Hidden;
                    ProgressBar_Loading.Value = 0;
                    TextBlock_ProgressMsg.Text = "";
                    Grid_Main.IsEnabled = true;
                    func?.Invoke(ProgressBar_Loading.Value, ProgressBar_Loading.Maximum, param);
                });
        }

        #endregion

        #region 命令

        /// <summary>
        /// 新建项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_New(object sender, ExecutedRoutedEventArgs e)
        {
            string baseFolder = Globals.Preference.LastFolderPath; ;
            string filter = Globals.CurrentLanguage["TEXT_SC2File"] as string + "|" + Globals.FileName_SC2Components;
            string title = Globals.CurrentLanguage["UI_OpenFileDialog_New_Title"] as string;
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out System.Windows.Forms.OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.Preference.LastFolderPath = file.DirectoryName;
                ProjectNew(file);
            }
            e.Handled = true;
        }

        /// <summary>
        /// 新建项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_New(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        /// <summary>
        /// 打开项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Open(object sender, ExecutedRoutedEventArgs e)
        {
            string baseFolder = Globals.Preference.LastFolderPath; ;
            string filter = Globals.CurrentLanguage["TEXT_ProjectFile"] as string + "|*" + Globals.Extension_SC2GameTran;
            string title = Globals.CurrentLanguage["UI_OpenFileDialog_Open_Title"] as string;
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out System.Windows.Forms.OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.Preference.LastFolderPath = file.DirectoryName;
                bool result = ProjectOpen(file);

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
        ///保存项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectSave(false);
            e.Handled = true;
        }

        /// <summary>
        /// 保存项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CheckCurrentProjectExist();
            e.Handled = true;
        }

        /// <summary>
        ///另存为项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectSave(true);
            e.Handled = true;
        }

        /// <summary>
        /// 另存为项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_SaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CheckCurrentProjectExist();
            e.Handled = true;
        }

        /// <summary>
        ///关闭项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectClose();
            e.Handled = true;
        }

        /// <summary>
        /// 关闭项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Close(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CheckCurrentProjectExist();
            e.Handled = true;
        }

        /// <summary>
        /// 应用项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Accept(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Assert(Globals.CurrentProject != null);
            if (!Globals.CurrentProject.SC2Components.Exists)
            {
                if (!SetComponentsPath())
                {
                    e.Handled = true;
                    return;
                }
            }
            Globals.CurrentProject.WriteToComponents();
            e.Handled = true;
        }

        /// <summary>
        /// 应用项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Accept(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.ComponentsPathValid;
            e.Handled = true;
        }

        /// <summary>
        /// 重载项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_Reload(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Assert(Globals.CurrentProject != null);
            string baseFolder = Globals.Preference.LastFolderPath; ;
            string filter = Globals.CurrentLanguage["TEXT_ProjectFile"] as string + "|*" + Globals.Extension_SC2GameTran;
            string title = Globals.CurrentLanguage["UI_OpenFileDialog_Reload_Title"] as string;
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out System.Windows.Forms.OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.Preference.LastFolderPath = file.DirectoryName;
                ProjectReload(GetProjectDataFile(file));
            }
            e.Handled = true;
        }

        /// <summary>
        /// 重载项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_Reload(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.ComponentsPathValid;
            e.Handled = true;
        }

        /// <summary>
        /// 重载(地图/Mod)项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_ReloadSC2(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Assert(Globals.CurrentProject != null);
            Log.Assert(Globals.CurrentProject != null);
            string baseFolder = Globals.Preference.LastFolderPath; ;
            string filter = Globals.CurrentLanguage["TEXT_SC2File"] as string + "|" + Globals.FileName_SC2Components;
            string title = Globals.CurrentLanguage["UI_OpenFileDialog_Reload_Title"] as string;
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out System.Windows.Forms.OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.Preference.LastFolderPath = file.DirectoryName;
                ProjectReloadSC2(file);
            }
            e.Handled = true;
        }

        /// <summary>
        /// 重载(地图/Mod)项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_ReloadSC2(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.ComponentsPathValid;
            e.Handled = true;
        }

        /// <summary>
        /// 浏览Mod/Map执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_ComponentsPath(object sender, ExecutedRoutedEventArgs e)
        {
            SetComponentsPath();
            e.Handled = true;
        }

        /// <summary>
        /// 浏览Mod/Map判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_ComponentsPath(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CheckCurrentProjectExist();
            e.Handled = true;
        }
        
        /// <summary>
        /// 浏览Mod/Map执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_RecentProjects(object sender, ExecutedRoutedEventArgs e)
        {
            FileInfo file = new FileInfo(e.Parameter as string);
            if (file.Exists)
            {
                bool result = ProjectOpen(file);
                Globals.MainWindow.Backstage_Menu.IsOpen = false;
            }
            else
            {
                if (Log.ShowSystemMessage(true, MessageBoxButton.YesNo, MessageBoxImage.None, "MSG_CanNotFindProjectFile", file.FullName) == MessageBoxResult.Yes)
                {
                    RemoveRecentProject(file);
                }
            }
            e.Handled = true;
        }

        /// <summary>
        /// 浏览Mod/Map判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_RecentProjects(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        #endregion

        #region 翻译语言筛选项

        /// <summary>
        /// 刷新翻译选项
        /// </summary>
        /// <param name="project">项目</param>
        public void RefreshTranslateAndSearchLanguageButtons(Data_GameText project)
        {
            ResetTranslateAndSearchLanguageButtons();
            if (project == null || project.LangaugeRowList.Count() == 0)
            {
                InRibbonGallery_TranslateLanguage.Items.Add(TranslateAndSearchLanguage[0].Button);
                TranslateAndSearchLanguage[0].Button.IsChecked = false;
                InRibbonGallery_TranslateLanguage.Selectable = false;
            }
            else
            {        
                ToggleButton selectButton = null;
                ToggleButton firstButton = null;
                foreach (DataRow row in project.LangaugeRowList)
                {
                    EnumLanguage lang = (EnumLanguage)row[Data_GameText.RN_Language_ID];
                    InRibbonGallery_TranslateLanguage.Items.Add(TranslateAndSearchLanguage[lang].Button);
                    ComboBox_SearchLanguage.Items.Add(TranslateAndSearchLanguage[lang].ComboItem);
                    ListBox_GameTextShowLanguage.Items.Add(TranslateAndSearchLanguage[lang].ListItem);
                    switch ((EnumGameUseStatus)row[Data_GameText.RN_Language_Status])
                    {
                        case EnumGameUseStatus.Droped:
                            TranslateAndSearchLanguage[lang].ComboItemText.TextDecorations = TextDecorations.Strikethrough;
                            TranslateAndSearchLanguage[lang].ListItemText.TextDecorations = TextDecorations.Strikethrough;
                            break;
                        case EnumGameUseStatus.Added:
                            TranslateAndSearchLanguage[lang].ComboItemText.FontWeight = FontWeights.Bold;
                            TranslateAndSearchLanguage[lang].ListItemText.FontWeight = FontWeights.Bold;
                            break;
                        default:
                            break;
                    }
                    if (selectButton == null || (int)lang == CultureInfo.CurrentCulture.LCID)
                    {
                        selectButton = TranslateAndSearchLanguage[lang].Button;
                    }
                    if (firstButton == null)
                    {
                        firstButton = TranslateAndSearchLanguage[lang].Button;
                    }
                }
                if (firstButton != null)
                {
                    if (selectButton == null) selectButton = firstButton;
                    selectButton.IsChecked = true;
                    CurrentTranslateLanguage = (EnumLanguage)selectButton.Tag;
                }
                InRibbonGallery_TranslateLanguage.Selectable = true;
            }
        }
        
        /// <summary>
        /// 重置翻译和搜索语言项
        /// </summary>
        private void ResetTranslateAndSearchLanguageButtons()
        {
            InRibbonGallery_TranslateLanguage.Items.Clear();
            ComboBox_SearchLanguage.Items.Clear();
            ComboBox_SearchLanguage.Items.Add(TranslateAndSearchLanguage[0].ComboItem);
            ListBox_GameTextShowLanguage.Items.Clear();
            foreach (EnumLanguage lang in Enum.GetValues(typeof(EnumLanguage)))
            {
                TranslateAndSearchLanguage[lang].Button.IsChecked = false;
                TranslateAndSearchLanguage[lang].ComboItem.IsSelected = false;
                TranslateAndSearchLanguage[lang].ComboItemText.TextDecorations = null;
                TranslateAndSearchLanguage[lang].ComboItemText.FontWeight = FontWeights.Normal;
                TranslateAndSearchLanguage[lang].ListItem.IsSelected = true;
                TranslateAndSearchLanguage[lang].ListItemText.TextDecorations = null;
                TranslateAndSearchLanguage[lang].ListItemText.FontWeight = FontWeights.Normal;
            }
        }

        /// <summary>
        /// 新建语言切换按钮
        /// </summary>
        /// <returns>按钮</returns>
        private static Dictionary<EnumLanguage, TranslateLanguageControls> NewTranslateAndSerachLanguageButton()
        {
            Dictionary<EnumLanguage, TranslateLanguageControls> list = new Dictionary<EnumLanguage, TranslateLanguageControls>
            {
                { 0, new TranslateLanguageControls() }
            };

            EnumLanguage[] array = Enum.GetValues(typeof(EnumLanguage)).Cast<EnumLanguage>().ToArray();
            Array.Sort(array, (p1, p2) => Enum.GetName(typeof(EnumLanguage), p1).CompareTo(Enum.GetName(typeof(EnumLanguage), p2)));
            foreach (EnumLanguage lang in array)
            {
                list.Add(lang, new TranslateLanguageControls(lang));
            }
            return list;
        }

        #endregion

        #region Galaxy筛选项


        /// <summary>
        /// 刷新Galaxy筛选按钮
        /// </summary>
        /// <param name="project">项目数据</param>
        private void RefreshGalaxyTextFileFilterButton(Data_GameText project)
        {
            foreach (ToggleButton button in m_GalaxyButtons)
            {
                InRibbonGallery_GalaxyFilter.Items.Remove(button);
            }
            m_GalaxyButtons.Clear();
            if (project != null)
            {
                ToggleButton button;
                foreach (DataRow row in project.Tables[Data_GameText.TN_GalaxyFile].Rows)
                {
                    button = NewGalaxyTextFileFilterButton(row);
                    m_GalaxyButtons.Add(button);
                    InRibbonGallery_GalaxyFilter.Items.Insert(InRibbonGallery_GalaxyFilter.Items.Count - 1, button);
                }
                ToggleButton_FilterGalaxyFileNone.IsChecked = true;
                ToggleButton_FilterGalaxyFileNone.IsEnabled = true;
            }
            else
            {
                ToggleButton_FilterGalaxyFileNone.IsChecked = false;
                ToggleButton_FilterGalaxyFileNone.IsEnabled = false;
            }
            InRibbonGallery_GalaxyFilter.SelectedItem = null;
        }

        /// <summary>
        /// 新建Galaxy文件筛选按钮
        /// </summary>
        /// <param name="fileRow">Galaxy数据</param>
        /// <returns>按钮</returns>
        private ToggleButton NewGalaxyTextFileFilterButton(DataRow fileRow)
        {
            ToggleButton button = new ToggleButton
            {
                Tag = fileRow,
                Header = fileRow[Data_GameText.RN_GalaxyFile_Name],
                ToolTip = fileRow[Data_GameText.RN_GalaxyFile_Path],
                SizeDefinition = new RibbonControlSizeDefinition(RibbonControlSize.Middle, RibbonControlSize.Middle, RibbonControlSize.Middle),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                IsChecked = true,
                Height = ToggleButton_FilterGalaxyFileNone.Height,
                FontSize = ToggleButton_FilterGalaxyFileNone.FontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            button.SetResourceReference(ToggleButton.IconProperty, "IMAGE_GalaxyFile");
            button.SetResourceReference(ToggleButton.LargeIconProperty, "IMAGE_GalaxyFile");
            button.Checked += ToggleButton_FilterGalaxyButton_CheckEvent;
            button.Unchecked += ToggleButton_FilterGalaxyButton_CheckEvent;
            return button;
        }

        #endregion

        #region 枚举筛选项

        /// <summary>
        /// 刷新文件筛选选项
        /// </summary>
        /// <param name="project">项目数据</param>
        private void RefreshTextFileFilterButton(Data_GameText project)
        {
            bool isCheck = project != null;
            foreach (ToggleButton button in InRibbonGallery_TextFileFilter.Items)
            {
                button.IsChecked = isCheck;
                button.IsEnabled = isCheck;
            }
        }

        /// <summary>
        /// 刷新文本状态选项
        /// </summary>
        /// <param name="project">项目数据</param>
        private void RefreshTextStatusFilterButton(Data_GameText project)
        {
            bool isCheck = project != null;
            foreach (ToggleButton button in InRibbonGallery_TextStatusFilter.Items)
            {
                button.IsChecked = isCheck;
                button.IsEnabled = isCheck;
            }
        }

        /// <summary>
        /// 刷新文本状态选项
        /// </summary>
        /// <param name="project">项目数据</param>
        private void RefreshUseStatusFilterButton(Data_GameText project)
        {
            bool isCheck = project != null;
            foreach (ToggleButton button in InRibbonGallery_UseStatusFilter.Items)
            {
                button.IsChecked = isCheck;
                button.IsEnabled = isCheck;
            }
        }
        #endregion

        #region 搜索相关

        /// <summary>
        /// 刷新搜索相关控件状态
        /// </summary>
        /// <param name="project">项目数据</param>
        private void RefreshSearchControl(Data_GameText project)
        {
            bool isEnable = project != null;
            ComboBox_SearchType.IsEnabled = isEnable;
            ComboBox_SearchType.SelectedIndex = 0;
            ComboBox_SearchLocation.SelectedIndex = 0;
            ComboBox_SearchLanguage.IsEnabled = isEnable;
            ComboBox_SearchLanguage.SelectedIndex = 0;
            RadioButton_SearchNull.IsEnabled = isEnable;
            RadioButton_SearchNull.IsChecked = false;
            RadioButton_SearchRegex.IsEnabled = isEnable;
            RadioButton_SearchRegex.IsChecked = false;
            RadioButton_SearchText.IsEnabled = isEnable;
            RadioButton_SearchText.IsChecked = true;
            CheckBox_SearchCase.IsEnabled = isEnable;
            CheckBox_SearchCase.IsChecked = false;
            TextBox_SearchText.IsEnabled = isEnable;
            TextBox_SearchText.Text = "";
        }

        #endregion

        #region DataGrid

        /// <summary>
        /// 刷新翻译文本
        /// </summary>
        /// <param name="project">项目数据</param>
        public void RefreshTranslatedText(Data_GameText project)
        {
            if (project == null)
            {
                CurrentTextData = null;
            }
            else
            {
                CurrentTextData = project.Tables[Data_GameText.TN_GameText];
            }
            RefreshTranslatedText();
        }

        /// <summary>
        /// 刷新翻译文本
        /// </summary>
        public void RefreshTranslatedText()
        {
            if (!CanRefreshTranslatedText) return;
            if (CurrentTextData == null)
            {
                DataGrid_TranslatedTexts.ItemsSource = null;
                return;
            }
            EnumerableRowCollection<DataRow> query = CurrentTextData.AsEnumerable();
            if (TextFileFilter != EnumGameTextFile.All)
            {
                string keyFile = Data_GameText.RN_GameText_File;
                query = from row in query
                        where ((int)row[keyFile] & (int)TextFileFilter) != 0
                        select row;
            }
            if (TextStatusFilter != EnumGameTextStatus.All)
            {
                string keyStatus = Data_GameText.GetGameRowNameForLanguage(EnumCurrentLanguage, Data_GameText.RN_GameText_TextStatus);
                query = from row in query
                        where ((EnumGameTextStatus)row[keyStatus]).HasFlag(TextStatusFilter)
                        select row;
            }
            if (UseStatusFilter != EnumGameUseStatus.All)
            {
                string keyStatus = Data_GameText.GetGameRowNameForLanguage(EnumCurrentLanguage, Data_GameText.RN_GameText_TextStatus);
                query = from row in query
                        where ((EnumGameUseStatus)row[keyStatus]).HasFlag(UseStatusFilter)
                        select row;
            }
            if (!IsSelectAllGalaxyFilter)
            {
                query = from row in query
                        where IsUseInGalaxyFiles(row)
                        select row;
            }
            if (RadioButton_SearchNull.IsChecked == true)
            {
                SereachText_NullMod(ref query);
            }
            else if (RadioButton_SearchRegex.IsChecked == true)
            {
                SereachText_RegexMod(ref query);
            }
            else
            {
                SereachText_MatchMod(ref query);
            }
            DataGrid_TranslatedTexts.ItemsSource = query.AsDataView();
        }

        /// <summary>
        /// 获取需要搜索的Key的列表
        /// </summary>
        /// <returns></returns>
        private List<string> GetSearchKeyList()
        {
            List<string> keyList = new List<string>();
            EnumSearchTextType type = (EnumSearchTextType)(ComboBox_SearchType.SelectedItem as ComboBoxItem).Tag;
            // ID
            if (type.HasFlag(EnumSearchTextType.ID))
            {
                keyList.Add(Data_GameText.RN_GameText_ID);
            }

            // Droped Text
            if (type.HasFlag(EnumSearchTextType.Droped))
            {
                ComboBoxItem item = ComboBox_SearchLanguage.SelectedItem as ComboBoxItem;
                List<EnumLanguage> languageList = (CurrentTextData.DataSet as Data_GameText).LangaugeList;
                if (item.Tag == null)
                {
                    keyList.AddRange(languageList.Select(r => Data_GameText.GetGameRowNameForLanguage(r, Data_GameText.RN_GameText_DropedText)).ToList());
                }
                else
                {
                    keyList.Add(Data_GameText.GetGameRowNameForLanguage((EnumLanguage)item.Tag, Data_GameText.RN_GameText_DropedText));
                }
            }

            // Source Text
            if (type.HasFlag(EnumSearchTextType.Source))
            {
                ComboBoxItem item = ComboBox_SearchLanguage.SelectedItem as ComboBoxItem;
                List<EnumLanguage> languageList = (CurrentTextData.DataSet as Data_GameText).LangaugeList;
                if (item.Tag == null)
                {
                    keyList.AddRange(languageList.Select(r => Data_GameText.GetGameRowNameForLanguage(r, Data_GameText.RN_GameText_SourceText)).ToList());
                }
                else
                {
                    keyList.Add(Data_GameText.GetGameRowNameForLanguage((EnumLanguage)item.Tag, Data_GameText.RN_GameText_SourceText));
                }
            }

            // Edited Text
            if (type.HasFlag(EnumSearchTextType.Edited))
            {
                ComboBoxItem item = ComboBox_SearchLanguage.SelectedItem as ComboBoxItem;
                List<EnumLanguage> languageList = (CurrentTextData.DataSet as Data_GameText).LangaugeList;
                if (item.Tag == null)
                {
                    keyList.AddRange(languageList.Select(r => Data_GameText.GetGameRowNameForLanguage(r, Data_GameText.RN_GameText_EditedText)).ToList());
                }
                else
                {
                    keyList.Add(Data_GameText.GetGameRowNameForLanguage((EnumLanguage)item.Tag, Data_GameText.RN_GameText_EditedText));
                }
            }
            return keyList;
        }

        /// <summary>
        /// 搜索文本（正则表达式模式）
        /// </summary>
        /// <param name="query">查询数据</param>
        private void SereachText_NullMod(ref EnumerableRowCollection<DataRow> query)
        {
            try
            {
                query = from row in query
                        where IsInSearchResult_NullLangauge(row, GetSearchKeyList())
                        select row;
            }
            catch
            {
                SereachText_MatchMod(ref query);
            }
        }

        /// <summary>
        /// 搜索文本（正则表达式模式）
        /// </summary>
        /// <param name="query">查询数据</param>
        private void SereachText_RegexMod(ref EnumerableRowCollection<DataRow> query)
        {
            string match = TextBox_SearchText.Text;
            if (string.IsNullOrEmpty(match)) return;
            RegexOptions options = CheckBox_SearchCase.IsChecked == true ? RegexOptions.Compiled : RegexOptions.Compiled | RegexOptions.IgnoreCase;
            try
            {
                Regex regex = new Regex(match, options);
                query = from row in query
                        where IsInSearchResult_RegexLangauge(row, GetSearchKeyList(), regex)
                        select row;
            }
            catch
            {
                SereachText_MatchMod(ref query);
            }
        }

        /// <summary>
        /// 搜索文本（匹配模式）
        /// </summary>
        /// <param name="query">查询数据</param>
        private void SereachText_MatchMod(ref EnumerableRowCollection<DataRow> query)
        {
            string match = TextBox_SearchText.Text;
            if (String.IsNullOrEmpty(match)) return;
            EnumSearchTextType type = (EnumSearchTextType)(ComboBox_SearchType.SelectedItem as ComboBoxItem).Tag;
            EnumSearchTextLocation location = (EnumSearchTextLocation)(ComboBox_SearchLocation.SelectedItem as ComboBoxItem).Tag;
            Delegate_IsInSearchResult searchMatchFunc = DictTextSearchLocationFunc[location];
            bool ignoreCase = CheckBox_SearchCase.IsChecked == false;
            if (ignoreCase) match = match.ToLower();
            query = from row in query
                    where IsInSearchResult_MatchLangauge(row, GetSearchKeyList(), match, ignoreCase, searchMatchFunc)
                    select row;
        }

        /// <summary>
        /// 是否在galaxy中使用
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>判断结果</returns>
        private bool IsUseInGalaxyFiles(DataRow row)
        {
            DataRow[] locations = row.GetChildRows(Data_GameText.RSN_GameText_GalaxyLocation_Key);
            if (locations.Count() == 0)
            {
                return GalaxyFilter.Contains(Globals.Const_NoUseInGalaxy);
            }
            else
            {
                return locations.Where(r => GalaxyFilter.Contains(r.GetParentRow(Data_GameText.RSN_GalaxyLine_GameLocation_Line)[Data_GameText.RN_GalaxyLine_File])).Count() != 0;
            }
        }

        /// <summary>
        /// 在搜索结果中（空文本）
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="keyList">数据列名列表</param>
        /// <param name="match">测试正则表达式</param>
        /// <returns>判断结果</returns>
        private static bool IsInSearchResult_NullLangauge(DataRow row, List<string> keyList)
        {
            foreach (string key in keyList)
            {
                if (row[key] == DBNull.Value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 在搜索结果中（正则表达式）
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="keyList">数据列名列表</param>
        /// <param name="match">测试正则表达式</param>
        /// <returns>判断结果</returns>
        private static bool IsInSearchResult_RegexLangauge(DataRow row, List<string> keyList, Regex match)
        {
            foreach (string key in keyList)
            {
                if (row[key] != DBNull.Value && match.IsMatch(row[key] as string))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 在搜索结果中（全部）
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="key">数据列名</param>
        /// <param name="match">匹配字符串</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>判断结果</returns>
        private static bool IsInSearchResult_MatchAll(DataRow row, string key, string match, bool ignoreCase)
        {
            if (row[key] == DBNull.Value) return false;
            string value = row[key] as string;
            if (ignoreCase) value = value.ToLower();
            return value.Contains(match);
        }

        /// <summary>
        /// 在搜索结果中（左起）
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="key">数据列名</param>
        /// <param name="match">匹配字符串</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>判断结果</returns>
        private static bool IsInSearchResult_MatchLeft(DataRow row, string key, string match, bool ignoreCase)
        {
            if (row[key] == DBNull.Value) return false;
            string value = row[key] as string;
            if (ignoreCase) value = value.ToLower();
            return value.StartsWith(match);
        }

        /// <summary>
        /// 在搜索结果中（右起）
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="key">数据列名</param>
        /// <param name="match">匹配字符串</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>判断结果</returns>
        private static bool IsInSearchResult_MatchRight(DataRow row, string key, string match, bool ignoreCase)
        {
            if (row[key] == DBNull.Value) return false;
            string value = row[key] as string;
            if (ignoreCase) value = value.ToLower();
            return value.EndsWith(match);
        }

        /// <summary>
        /// 在搜索结果中（文本匹配）
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="keyList">数据列名列表</param>
        /// <param name="match">匹配字符串</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <param name="func">匹配函数</param>
        /// <returns>判断结果</returns>
        private static bool IsInSearchResult_MatchLangauge(DataRow row, List<string> keyList, string match, bool ignoreCase, Delegate_IsInSearchResult func)
        {
            foreach (string key in keyList)
            {
                if (func(row, key, match, ignoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 刷新游戏文本单语言数据表
        /// </summary>
        /// <param name="refreshItem">仅筛选</param>
        private void RefreshGameTextDetails(bool refreshItem)
        {
            if (refreshItem && DataGrid_TranslatedTexts.CurrentItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                List<EnumLanguage> langList = new List<EnumLanguage>();
                foreach (ListBoxItem item in ListBox_GameTextShowLanguage.Items)
                {
                    langList.Add((EnumLanguage)item.Tag);
                }
                Data_GameText.RefreshGameTextForLanguageTable(row, langList);
            }
            List<EnumLanguage> showLanguages = ListBox_GameTextShowLanguage.SelectedItems.Cast<ListBoxItem>().Select(r=>(EnumLanguage)r.Tag).ToList();
            EnumerableRowCollection<DataRow> query = Data_GameText.GameTextForLanguageTable.AsEnumerable();

            query = from row in query
                    where showLanguages.Contains((EnumLanguage)row[Data_GameText.RN_GameTextForLanguage_Language])
                    select row;

            DataGrid_GameTextForLanguage.ItemsSource = query.AsDataView();
        }

        #endregion

        #region 功能

        /// <summary>
        /// 委托设为当前Project
        /// </summary>
        /// <param name="count">当前计数</param>
        /// <param name="max">最大计数</param>
        /// <param name="param">参数</param>
        public static void SetToCurrentProject(double count, double max, object param)
        {
            Globals.CurrentProject = param as Data_GameText;
        }

        /// <summary>
        /// 新建项目
        /// </summary>
        /// <param name="file">文件路径</param>
        public static void ProjectNew(FileInfo file)
        {
            ProjectClose();
            Data_GameText project = new Data_GameText();
            project.Initialization(file, SetToCurrentProject);
            Globals.MainWindow.RibbonGroupBox_File.UpdateLayout();
        }

        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>是否成功打开</returns>
        public static bool ProjectOpen(FileInfo file)
        {
            ProjectClose();
            Globals.CurrentProjectPath = file;
            Data_GameText project = Data_GameText.LoadProject(file);
            Globals.CurrentProject = project;
            Globals.MainWindow.RibbonGroupBox_File.UpdateLayout();
            if (project != null)
            {
                AddRecentProject(file);
                return true;
            }
            else
            {
                Globals.CurrentProjectPath = null;
                return false;
            }
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="file">保存路径</param>
        public static void ProjectSave(FileInfo file)
        {
            Log.Assert(Globals.CurrentProject != null);
            Globals.CurrentProject.SaveProject(file);
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="isSaveAs">是否另存为</param>
        public static void ProjectSave(bool isSaveAs)
        {
            Log.Assert(Globals.CurrentProject != null);
            if (!isSaveAs && Globals.CurrentProjectPath != null)
            {
                ProjectSave(Globals.CurrentProjectPath);
                return;
            }
            else
            {
                string baseFolder = Globals.Preference.LastFolderPath; ;
                string filter = Globals.CurrentLanguage["TEXT_ProjectFile"] as string + "|*" + Globals.Extension_SC2GameTran;
                string key = isSaveAs ? "UI_OpenFileDialog_SaveAs_Title" : "UI_OpenFileDialog_Save_Title";
                string title = Globals.CurrentLanguage[isSaveAs] as string;
                if (Globals.SaveFilePathDialog(baseFolder, filter, title, out System.Windows.Forms.SaveFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
                {
                    FileInfo file = new FileInfo(fileDialog.FileName);
                    Globals.Preference.LastFolderPath = file.DirectoryName;
                    Globals.CurrentProjectPath = file;
                    ProjectSave(file);
                    AddRecentProject(file);
                }
            }
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public static void ProjectClose()
        {
            ///To Do Save
            if (Globals.CurrentProject != null)
            {
                Globals.CurrentProject = null;
                Globals.CurrentProjectPath = null;
            }
        }

        /// <summary>
        /// 获取项目文件
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>项目数据</returns>
        public static Data_GameText GetProjectDataFile(FileInfo file)
        {
            return Data_GameText.LoadProject(file);
        }

        /// <summary>
        /// 委托设为当前Project
        /// </summary>
        /// <param name="count">当前计数</param>
        /// <param name="max">最大计数</param>
        /// <param name="param">参数</param>
        public static void ProjectReloadCallback(double count, double max, object param)
        {
            ProjectReload(param as Data_GameText);
        }

        /// <summary>
        /// 重载项目
        /// </summary>
        /// <param name="project">项目文件</param>
        public static void ProjectReload(Data_GameText project)
        {
            project.ReloadProjectData(Globals.CurrentProject);
            Globals.CurrentProject = project;
        }

        /// <summary>
        /// 重载项目(地图/Mod)
        /// </summary>
        public static void ProjectReloadSC2(FileInfo file)
        {
            Data_GameText project = new Data_GameText();
            project.Initialization(file, ProjectReloadCallback);
        }

        /// <summary>
        /// 设置组件文件夹路径
        /// </summary>
        /// <returns>设置成功</returns>
        public static bool SetComponentsPath()
        {
            string baseFolder = Globals.MainWindow.TextBox_ComponentsPath.Text; ;
            string filter = Globals.CurrentLanguage["TEXT_SC2File"] as string + "|" + Globals.FileName_SC2Components;
            string title = Globals.CurrentLanguage["UI_OpenFileDialog_CompontentsPath_Title"] as string;
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out System.Windows.Forms.OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.CurrentProject.SC2Components = file;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检测当前项目存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckCurrentProjectExist()
        {
            return Globals.CurrentProject != null;
        }

        /// <summary>
        /// 当前项目数据切换刷新
        /// </summary>
        /// <param name="oldPro">旧项目</param>
        /// <param name="newPro">新项目</param>
        private void OnProjectChangeRefresh(Data_GameText oldPro, Data_GameText newPro)
        {
            CanRefreshTranslatedText = false;
            RefreshTranslateAndSearchLanguageButtons(newPro);
            RefreshGalaxyTextFileFilterButton(newPro);
            RefreshTextFileFilterButton(newPro);
            RefreshTextStatusFilterButton(newPro);
            RefreshUseStatusFilterButton(newPro);
            RefreshSearchControl(newPro);
            CanRefreshTranslatedText = true;
            RefreshTranslatedText(newPro);
        }

#endregion

#region 最近打开项目

        /// <summary>
        /// 添加最近项目
        /// </summary>
        /// <param name="file">项目文件</param>
        private static void AddRecentProject(FileInfo file)
        {
            Globals.Preference.Preference_AddRecentRecord(file);
            Globals.MainWindow.RefreshRecentProjects();
        }

        /// <summary>
        /// 删除最近项目
        /// </summary>
        /// <param name="file">项目文件</param>
        private static void RemoveRecentProject(FileInfo file)
        {
            Globals.Preference.Preference_RemoveRecentRecord(file);
            Globals.MainWindow.RefreshRecentProjects();
        }

        /// <summary>
        /// 刷新最近打开的项目
        /// </summary>
        public void RefreshRecentProjects()
        {
            ItemsControl_RecentFiles.Items.Clear();
            foreach (string path in Globals.Preference.RecentProjectList)
            {
                if (path != null) NewRecentProejct(path);
            }
        }

        /// <summary>
        /// 新建最近文件按钮
        /// </summary>
        /// <param name="path">路径</param>
        private void NewRecentProejct(string path)
        {
            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/Image/ui-editoricon-triggereditor_newcomment.dds"));
            Image image = new Image
            {
                Source = bitmap,
                Margin = new Thickness(3),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            TextBlock text = new TextBlock
            {
                Text = path,
                Margin = new Thickness(3),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(3),
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };
            panel.Children.Add(image);
            panel.Children.Add(text);
            System.Windows.Controls.Button button = new System.Windows.Controls.Button
            {
                Content = panel,
                Command = CommandRecentProjects,
                CommandParameter = path,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                BorderThickness = new Thickness(0),
            };
            ItemsControl_RecentFiles.Items.Add(button);
        }

#endregion

#endregion

#region 控件事件

        /// <summary>
        /// 语言选择下拉列表选择事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ComboBox_Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemName = ComboBox_Language.SelectedItem as string;
            EnumCurrentLanguage = Globals.DictComboBoxItemLanguage[itemName];
            RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 翻译语言按钮点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private static void Button_TranslateLanguage_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton button)
            {
                Globals.MainWindow.InRibbonGallery_TranslateLanguage.SelectedItem = button;
                if (button.Tag != null)
                {
                    Binding binding = new Binding(Data_GameText.GetGameRowNameForLanguage((EnumLanguage) button.Tag, Data_GameText.RN_GameText_EditedText))
                    {
                        Mode = BindingMode.TwoWay,
                    };
                    Globals.MainWindow.DataGridColumn_TranslateEditedText.Binding = binding;
                }
                else
                {
                    Globals.MainWindow.DataGridColumn_TranslateEditedText.Binding = null;
                }
                Globals.MainWindow.CurrentTranslateLanguage = (EnumLanguage)button.Tag;
                Globals.MainWindow.RefreshTranslatedText();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void RibbonWindow_Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.Preference.SavePreference();
        }

        /// <summary>
        /// Galaxy文件筛选全选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_GalaxyFilterSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            ToggleButton_FilterGalaxyFileNone.IsChecked = true;
            foreach (ToggleButton button in m_GalaxyButtons)
            {
                button.IsChecked = true;
            }
            CanRefreshTranslatedText = true;
            e.Handled = true;
        }

        /// <summary>
        /// Galaxy文件筛选全不选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_GalaxyFilterSelectNone_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            ToggleButton_FilterGalaxyFileNone.IsChecked = false;
            foreach (ToggleButton button in m_GalaxyButtons)
            {
                button.IsChecked = false;
            }
            CanRefreshTranslatedText = true;
            e.Handled = true;
        }

        /// <summary>
        /// Galaxy筛选文件点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ToggleButton_FilterGalaxyButton_CheckEvent(object sender, RoutedEventArgs e)
        {
            GalaxyFilter.Clear();
            IsSelectAllGalaxyFilter = ToggleButton_FilterGalaxyFileNone.IsChecked == true;
            if (IsSelectAllGalaxyFilter) GalaxyFilter.Add(Globals.Const_NoUseInGalaxy);
            foreach (ToggleButton button in m_GalaxyButtons)
            {
                if (button.IsChecked == false)
                {
                    IsSelectAllGalaxyFilter = false;
                }
                else
                {
                    if (button.Tag is DataRow row)
                    {
                        GalaxyFilter.Add(row[Data_GameText.RN_GalaxyFile_Path] as string);
                    }
                    else
                    {
                        GalaxyFilter.Add(button.Tag as string);
                    }
                }
            }
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 文本文件筛选全选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_TextFileFilterSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            foreach (ToggleButton button in InRibbonGallery_TextFileFilter.Items)
            {
                button.IsChecked = true;
            }
            CanRefreshTranslatedText = true;
            e.Handled = true;
        }

        /// <summary>
        /// 文本文件筛选全不选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_TextFileFilterSelectNone_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            foreach (ToggleButton button in InRibbonGallery_TextFileFilter.Items)
            {
                button.IsChecked = false;
            }
            CanRefreshTranslatedText = true;
            e.Handled = true;
        }

        /// <summary>
        /// 文本文件筛选文件点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ToggleButton_TextFileFilterButton_CheckEvent(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            if (button.IsChecked == true)
            {
                TextFileFilter |= (EnumGameTextFile)button.Tag;
            }
            else
            {
                TextFileFilter &= ~(EnumGameTextFile)button.Tag;
            }
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选全选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_TextStatusFilterSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            foreach (ToggleButton button in InRibbonGallery_TextStatusFilter.Items)
            {
                button.IsChecked = true;
            }
            CanRefreshTranslatedText = true;
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选全不选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_TextStatusFilterSelectNone_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            foreach (ToggleButton button in InRibbonGallery_TextStatusFilter.Items)
            {
                button.IsChecked = false;
            }
            CanRefreshTranslatedText = false;
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选文件点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ToggleButton_TextStatusFilterButton_CheckEvent(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            if (button.IsChecked == true)
            {
                TextStatusFilter |= (EnumGameTextStatus)button.Tag;
            }
            else
            {
                TextStatusFilter &= ~(EnumGameTextStatus)button.Tag;
            }
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选全选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_UseStatusFilterSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            foreach (ToggleButton button in InRibbonGallery_UseStatusFilter.Items)
            {
                button.IsChecked = true;
            }
            CanRefreshTranslatedText = true;
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选全不选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_UseStatusFilterSelectNone_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            foreach (ToggleButton button in InRibbonGallery_UseStatusFilter.Items)
            {
                button.IsChecked = false;
            }
            CanRefreshTranslatedText = false;
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选文件点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ToggleButton_UseStatusFilterButton_CheckEvent(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            if (button.IsChecked == true)
            {
                UseStatusFilter |= (EnumGameUseStatus)button.Tag;
            }
            else
            {
                UseStatusFilter &= ~(EnumGameUseStatus)button.Tag;
            }
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }
        /// <summary>
        /// 翻译文本表选择事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void DataGrid_TranslatedTexts_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            RefreshGameTextDetails(true);
        }

        /// <summary>
        /// 搜索类型选择事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ComboBox_Search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 搜索正则表达式状态切换事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void CheckBox_SearchConfigure_CheckEvent(object sender, RoutedEventArgs e)
        {
            RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 文本变化换事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void TextBox_SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshTranslatedText();
        }

        /// <summary>
        /// 显示语言选择变化
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ListBox_GameTextShowLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshGameTextDetails(false);
        }

#endregion

    }
}
