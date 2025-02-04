﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Fluent;
using Fluent.Localization;
using SC2_GameTranslater.Source;
using Binding = System.Windows.Data.Binding;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Orientation = System.Windows.Controls.Orientation;
using TextBox = System.Windows.Controls.TextBox;

namespace SC2_GameTranslater
{
    using Globals = Class_Globals;
    using Preference = Class_Preference;
    using Log = Class_Log;

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
        /// 滚动表格到指定项委托
        /// </summary>
        /// <param name="row">滚动到的行</param>
        private delegate void Delegate_ScrollItemToFirstRow(DataRow row, Delegate_ScrollItemToFirstRow_Callback callback);

        /// <summary>
        /// 滚动表格到指定项委托回调
        /// </summary>
        private delegate void Delegate_ScrollItemToFirstRow_Callback();

        /// <summary>
        /// 翻译语言控件
        /// </summary>
        public class TranslatedLanguageControls
        {
            #region 字段

            /// <summary>
            /// 当前翻译源语言选择按钮
            /// </summary>
            public Fluent.Button ButtonTranslateSource { get; }

            /// <summary>
            /// 当前翻译目标语言选择按钮
            /// </summary>
            public Fluent.Button ButtonTranslateTarget { get; }

            /// <summary>
            /// 复制源语言选择按钮
            /// </summary>
            public Fluent.Button ButtonCopySource { get; }

            /// <summary>
            /// 复制目标语言选择按钮
            /// </summary>
            public ToggleButton ButtonCopyTarget { get; }

            /// <summary>
            /// 详情显示列表项
            /// </summary>
            public ListBoxItem ListItemDetail { get; }

            /// <summary>
            /// 详情显示列表项文本
            /// </summary>
            public TextBlock ListItemTextDetail { get; }

            #endregion

            #region 构造函数

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="language">语言</param>
            public TranslatedLanguageControls(EnumLanguage language)
            {
                string langName = Globals.GetEnumLanguageName(language);
                ButtonTranslateSource = NewLanguageButton(language);
                ButtonTranslateTarget = NewLanguageButton(language);
                ButtonCopySource = NewLanguageButton(language);

                ButtonCopyTarget = new ToggleButton
                {
                    Tag = language,
                };
                SetRubbonButtonHeaderAndIcon(ButtonCopyTarget, "Use", langName);
                ButtonCopyTarget.Checked += ToggleButton_ButtonCopyTarget_CheckedEvent;
                ButtonCopyTarget.Unchecked += ToggleButton_ButtonCopyTarget_CheckedEvent;

                ListItemTextDetail = new TextBlock();
                ListItemDetail = new ListBoxItem
                {
                    Tag = language,
                    Content = ListItemTextDetail,
                };
                SetListItemHeaderAndIcon(ListItemDetail, ListItemTextDetail, "Use", langName);
            }

            #region 方法

            /// <summary>
            /// 新建语言按钮
            /// </summary>
            /// <param name="language">语言</param>
            /// <returns>按钮</returns>
            public static Fluent.Button NewLanguageButton(EnumLanguage language)
            {
                string langName = Globals.GetEnumLanguageName(language);
                Fluent.Button button = new Fluent.Button
                {
                    Tag = language,
                    IsHitTestVisible = false,
                };
                SetRubbonButtonHeaderAndIcon(button, "Use", langName);
                return button;
            }

            /// <summary>
            /// 获取语言对应ToolTip
            /// </summary>
            /// <param name="status">状态</param>
            /// <param name="langName">语言</param>
            /// <returns>Tooltip</returns>
            public static Fluent.ScreenTip GetEnumLanguageToolTip(string status, string langName)
            {
                CultureInfo culture = Globals.DictCultureInfo[langName];
                Fluent.ScreenTip tip = new Fluent.ScreenTip()
                {
                    Width = 300,
                    HelpTopic = Class_ConstantAndEnum.Link_HelpTopic,
                    IsRibbonAligned = false,
                    Text = culture.NativeName,
                };
                tip.SetResourceReference(Fluent.ScreenTip.TitleProperty, $"TEXT_{status}Language");
                tip.SetBinding(Fluent.ScreenTip.ImageProperty, $"IMAGE_{status}{langName}");
                return tip;
            }

            /// <summary>
            /// 设置按钮文本和图标(Fluent.Button)
            /// </summary>
            /// <param name="item">列表项</param>
            /// <param name="status">使用状态</param>
            /// <param name="langName">使用语言</param>
            public static void SetListItemHeaderAndIcon(ListBoxItem item, TextBlock block, string status, string langName)
            {
                block.SetResourceReference(TextBlock.TextProperty, $"TEXT_{status}{langName}");
                item.ToolTip = GetEnumLanguageToolTip(status, langName);
            }

            /// <summary>
            /// 设置按钮文本和图标(Fluent.Button)
            /// </summary>
            /// <param name="button">按钮</param>
            /// <param name="status">使用状态</param>
            /// <param name="langName">使用语言</param>
            public static void SetRubbonButtonHeaderAndIcon(Fluent.Button button, string status, string langName)
            {
                button.SetResourceReference(Fluent.Button.HeaderProperty, $"TEXT_{status}{langName}");
                button.SetResourceReference(Fluent.Button.IconProperty, $"IMAGE_{status}{langName}");
                button.SetResourceReference(Fluent.Button.LargeIconProperty, $"IMAGE_{status}{langName}");
                button.ToolTip = GetEnumLanguageToolTip(status, langName);
            }

            /// <summary>
            /// 设置按钮文本和图标(ToggleButton)
            /// </summary>
            /// <param name="button">按钮</param>
            /// <param name="status">使用状态</param>
            /// <param name="langName">使用语言</param>
            public static void SetRubbonButtonHeaderAndIcon(ToggleButton button, string status, string langName)
            {
                button.SetResourceReference(ToggleButton.HeaderProperty, $"TEXT_{status}{langName}");
                button.SetResourceReference(ToggleButton.IconProperty, $"IMAGE_{status}{langName}");
                button.SetResourceReference(ToggleButton.LargeIconProperty, $"IMAGE_{status}{langName}");
                button.ToolTip = GetEnumLanguageToolTip(status, langName);
            }

            /// <summary>
            /// 复制目标语言选中事件
            /// </summary>
            /// <param name="sender">事件控件</param>
            /// <param name="e">响应参数</param>
            private void ToggleButton_ButtonCopyTarget_CheckedEvent(object sender, RoutedEventArgs e)
            {
                bool isEnable = false;
                foreach (object select in Globals.MainWindow.InRibbonGallery_CopyTargets.Items)
                {
                    if (select is ToggleButton button && button.IsChecked == true && button.IsEnabled)
                    {
                        isEnable = true;
                        break;
                    }
                }
                Globals.MainWindow.RibbonButton_DoCopy.IsEnabled = isEnable;
            }

            #endregion

            #endregion
        }

        #endregion

        #region 属性字段

        #region 依赖项属性
        
        #region 其它

        /// <summary>
        /// 当前语言依赖项属性
        /// </summary>
        public static DependencyProperty EnumCurrentLanguageProperty = DependencyProperty.Register(nameof(EnumCurrentLanguage), typeof(EnumLanguage), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前语言依赖项
        /// </summary>
        public EnumLanguage EnumCurrentLanguage
        {
            set
            {
                if (value == EnumCurrentLanguage) return;
                App.Current.Resources.MergedDictionaries[0].MergedDictionaries.Clear();
                Globals.CurrentLanguage = Globals.DictUILanguages[value];
                App.Current.Resources.MergedDictionaries[0].MergedDictionaries.Add(Globals.CurrentLanguage);
                RibbonLocalization.Current.Localization = Globals.FluentLocalizationMap[value];
                SetValue(EnumCurrentLanguageProperty, value);
            }
            get => ((EnumLanguage)GetValue(EnumCurrentLanguageProperty));
        }

        /// <summary>
        /// 当前翻译语言文本数据依赖项属性
        /// </summary>>
        public static DependencyProperty EnumCurrentTranLangSourceProperty = DependencyProperty.Register(nameof(EnumCurrentTranLangSource), typeof(EnumLanguage), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前翻译语言文本数据依赖项(源)
        /// </summary>
        public EnumLanguage EnumCurrentTranLangSource
        {
            set
            {
                if (value == EnumCurrentTranLangSource) return;
                SetValue(EnumCurrentTranLangSourceProperty, value);
                RefreshEnumCurrentTranLangSource(value);
            }
            get => (EnumLanguage)GetValue(EnumCurrentTranLangSourceProperty);
        }

        /// <summary>
        /// 当前翻译语言文本数据依赖项属性
        /// </summary>>
        public static DependencyProperty EnumCurrentTranLangTargetProperty = DependencyProperty.Register(nameof(EnumCurrentTranLangTarget), typeof(EnumLanguage), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前翻译语言文本数据依赖项（目标）
        /// </summary>
        public EnumLanguage EnumCurrentTranLangTarget
        {
            set
            {
                if (value == EnumCurrentTranLangTarget) return;
                mLastEditedCell = null; // 后面会有一次刷新所以不需要调用属性
                SetValue(EnumCurrentTranLangTargetProperty, value);
                RefreshEnumCurrentTranLangTarget(value);
            }
            get => (EnumLanguage)GetValue(EnumCurrentTranLangTargetProperty);
        }

        /// <summary>
        /// 当前文本数据依赖项属性
        /// </summary>
        public static DependencyProperty CurrentTextDataProperty = DependencyProperty.Register(nameof(CurrentTextData), typeof(DataTable), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前文本数据依赖项
        /// </summary>
        public DataTable CurrentTextData { get => GetValue(CurrentTextDataProperty) as DataTable; set => SetValue(CurrentTextDataProperty, value); }

        /// <summary>
        /// 当前项目名称依赖项属性
        /// </summary>
        public static DependencyProperty CurrentProjectNameProperty = DependencyProperty.Register(nameof(CurrentProjectName), typeof(string), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前项目名称数据依赖项
        /// </summary>
        public string CurrentProjectName { get => GetValue(CurrentProjectNeedSaveProperty) as string; set => SetValue(CurrentProjectNameProperty, value); }

        /// <summary>
        /// 当前项目需要保存依赖项属性
        /// </summary>
        public static DependencyProperty CurrentProjectNeedSaveProperty = DependencyProperty.Register(nameof(CurrentProjectNeedSave), typeof(bool), typeof(SC2_GameTranslater_Window));

        /// <summary>
        /// 当前项目需要保存数据依赖项
        /// </summary>
        public bool CurrentProjectNeedSave { get => (bool)GetValue(CurrentProjectNeedSaveProperty); set => SetValue(CurrentProjectNeedSaveProperty, value); }

        #endregion

        #endregion

        #region 属性

        #region 命令

        /// <summary>
        /// 新建命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandNew { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 打开命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandOpen { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 保存命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandSave { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 另存为命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandSaveAs { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 关闭命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandClose { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 应用命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandAccept { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 重载命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandReloadTranslation { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 重载(地图/Mod)命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandReloadSourceText { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 预览文本命令依赖项属性
        /// </summary>
        public static RoutedUICommand CommandPreviewText { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 选择Mod/Map依赖项属性
        /// </summary>
        public static RoutedUICommand CommandComponentsPath { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 近期项目依赖项属性
        /// </summary>
        public static RoutedUICommand CommandRecentProjects { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 搜索点击依赖项属性
        /// </summary>
        public static RoutedUICommand SearchClick { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 上一记录依赖项属性
        /// </summary>
        public static RoutedUICommand FilterRecordPrev { set; get; } = new RoutedUICommand();

        /// <summary>
        /// 下一记录依赖项属性
        /// </summary>
        public static RoutedUICommand FilterRecordNext { set; get; } = new RoutedUICommand();

        #endregion

        #region 其他

        /// <summary>
        /// 翻译语言
        /// </summary>
        public Dictionary<EnumLanguage, TranslatedLanguageControls> DictTranslateAndSearchLanguage { set; get; } = NewTranslateAndSerachLanguageButton();

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
        public static bool CanRefreshTranslatedText { set; get; }

        /// <summary>
        /// 搜索位置委托字典
        /// </summary>
        private Dictionary<EnumSearchTextLocation, Delegate_IsInSearchResult> DictTextSearchLocationFunc { get; } = new Dictionary<EnumSearchTextLocation, Delegate_IsInSearchResult>
        {
            { EnumSearchTextLocation.All, IsInSearchResult_MatchAll},
            { EnumSearchTextLocation.Left, IsInSearchResult_MatchLeft},
            { EnumSearchTextLocation.Right, IsInSearchResult_MatchRight}
        };

        /// <summary>
        /// 当前筛选结果视图
        /// </summary>
        public DataView CurrentFilterResultView { private set; get; }

        /// <summary>
        /// 筛选器记录列表
        /// </summary>
        public static List<Class_SearchConfig> ListFilterRecord { set; get; } = new List<Class_SearchConfig>();

        /// <summary>
        /// 筛选器记录指针
        /// </summary>
        public static int ListFilterRecordPointer { set; get; } = -1;

        /// <summary>
        /// 允许保存记录
        /// </summary>
        public static bool CanSaveRecord { set; get; } = true;

        /// <summary>
        /// 上一次选择的数据
        /// </summary>
        public DataRowView LastSelectedCell { private set; get; }

        /// <summary>
        /// 上一次编辑的数据
        /// </summary>
        public DataRowView LastEditedCell
        {
            set
            {
                OnEditedTextChange(mLastEditedCell, TextBox_EditedBox.Text);
                mLastEditedCell = value;
                ResetEditedTextBinding();
            }
            get => mLastEditedCell;
        }
        private DataRowView mLastEditedCell;

        /// <summary>
        /// Galaxy按钮列表
        /// </summary>
        public List<ToggleButton> GalaxyButtons { get; } = new List<ToggleButton>();

        /// <summary>
        /// 当前滚动到的行号
        /// </summary>
        public int ScrollRowIndex { set; get; }

        #endregion

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
            foreach (EnumLanguage language in Globals.AllLanguage)
            {
                string langName = Globals.GetEnumLanguageName(language);
                CultureInfo culture = Globals.DictCultureInfo[langName];
                string fileName = "Language/" + langName + ".xaml";
                FileInfo file = new FileInfo(fileName);
                ResourceDictionary dictLanguage = new ResourceDictionary();
                if (file.Exists)
                {
                    dictLanguage.Source = new Uri(file.FullName);
                }
                else
                {
                    switch (language)
                    {
                        case EnumLanguage.zhCN:
                            dictLanguage.Source = new Uri("pack://application:,,,/" + fileName);
                            break;
                        case EnumLanguage.enUS:
                            dictLanguage.Source = new Uri("pack://application:,,,/" + fileName);
                            break;
                        case EnumLanguage.ruRU:
                            dictLanguage.Source = new Uri("pack://application:,,,/" + fileName);
                            break;
                        default:
                            continue;
                    }
                }
                string itemName = culture.NativeName;
                Globals.DictComboBoxItemLanguage.Add(itemName, language);
                Globals.DictUILanguages.Add(language, dictLanguage);
                Globals.FluentLocalizationMap[language] =
                    assembly.CreateInstance("SC2_GameTranslater.Source.RibbonLanguage_" +
                                            Globals.GetEnumLanguageName(language)) as RibbonLocalizationBase;
                ComboBox_Language.Items.Add(itemName);
                if (CultureInfo.CurrentCulture.LCID == (int)language)
                {
                    ComboBox_Language.SelectedItem = itemName;
                    Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
                    useDefault = false;
                }
            }
            if (useDefault)
            {
                ComboBox_Language.SelectedItem = Globals.DictCultureInfo[Globals.GetEnumLanguageName(EnumLanguage.enUS)].NativeName;
            }
            #endregion

            #region 指令配置

            CommandBinding commandBinding = new CommandBinding(CommandNew, Executed_New, CanExecuted_New);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandOpen, Executed_Open, CanExecuted_Open);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandSave, Executed_Save, CanExecuted_Save);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandSaveAs, Executed_SaveAs, CanExecuted_SaveAs);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandClose, Executed_Close, CanExecuted_Close);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandAccept, Executed_Accept, CanExecuted_Accept);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandReloadTranslation, Executed_ReloadTranslation, CanExecuted_ReloadTranslation);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandReloadSourceText, Executed_ReloadSourceText, CanExecuted_ReloadSourceText);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandPreviewText, Executed_PreviewText, CanExecuted_PreviewText);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandComponentsPath, Executed_ComponentsPath, CanExecuted_ComponentsPath);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(CommandRecentProjects, Executed_RecentProjects, CanExecuted_RecentProjects);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(SearchClick, Executed_SearchClick, CanExecuted_SearchClick);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(FilterRecordPrev, Executed_FilterRecordPrev, CanExecuted_FilterRecordPrev);
            Globals.MainWindow.CommandBindings.Add(commandBinding);
            commandBinding = new CommandBinding(FilterRecordNext, Executed_FilterRecordNext, CanExecuted_FilterRecordNext);
            Globals.MainWindow.CommandBindings.Add(commandBinding);

            #endregion

            #region ToolTip 服务
            
            Fluent.ToolTipService.Attach(typeof(SearchButton));
            Fluent.ToolTipService.Attach(typeof(TextBox));
            Fluent.ToolTipService.Attach(typeof(System.Windows.Controls.GroupBox));

            #endregion

            #region 其他
            Globals.Preference.LoadPreference();
            Globals.EventProjectChange += OnProjectChangeRefresh;
            OnProjectChangeRefresh(null, null);
            ScreenTip.HelpPressed += ScreenTip_HelpPressed;

            #endregion
        }

        #endregion

        #region 方法
        
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
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate
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
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate
                {
                    ProgressBar_Loading.Value += count;
                    TextBlock_ProgressMsg.Text =
                        $"({ProgressBar_Loading.Value}/{ProgressBar_Loading.Maximum}) {Globals.GetStringFromCurrentLanguage("UI_TextBlock_ProgressMsg_Text")} {msg}";
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
            Dispatcher.BeginInvoke(priority: DispatcherPriority.Normal,
                method: (ThreadStart)delegate
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
            string baseFolder = Globals.Preference.LastFolderPath;
            string filter = Globals.GetStringFromCurrentLanguage("TEXT_SC2File") + "|" + Class_ConstantAndEnum.FileName_SC2Components;
            string title = Globals.GetStringFromCurrentLanguage("UI_OpenFileDialog_New_Title");
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.MainWindow.CurrentProjectNeedSave = false;
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
            string baseFolder = Globals.Preference.LastFolderPath;
            string filter = Globals.GetStringFromCurrentLanguage("TEXT_ProjectFile") + "|*" + Class_ConstantAndEnum.Extension_SC2GameTran;
            string title = Globals.GetStringFromCurrentLanguage("UI_OpenFileDialog_Open_Title");
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.MainWindow.CurrentProjectNeedSave = false;
                Globals.Preference.LastFolderPath = file.DirectoryName;
                ProjectOpen(file);

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
            Log.Assert(Globals.CurrentProject != null, "Globals.CurrentProject != null");
            if (!Globals.CurrentProject?.SC2Components?.Exists == true)
            {
                if (!SetComponentsPath())
                {
                    e.Handled = true;
                    return;
                }
            }
            Globals.CurrentProject?.WriteToComponents();
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
        public static void Executed_ReloadTranslation(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Assert(Globals.CurrentProject != null, "Globals.CurrentProject != null");
            string baseFolder = Globals.Preference.LastFolderPath;
            string filter = Globals.GetStringFromCurrentLanguage("TEXT_ProjectFile") + "|*" + Class_ConstantAndEnum.Extension_SC2GameTran;
            string title = Globals.GetStringFromCurrentLanguage("UI_OpenFileDialog_Reload_Title");
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.Preference.LastFolderPath = file.DirectoryName;
                Data_GameText project = GetProjectDataFile(file);
                SC2_GameTranslater_Reload dialog = new SC2_GameTranslater_Reload(project.LangaugeList)
                {
                    Owner = Globals.MainWindow
                };
                if (!dialog.ShowDialog() == true) return;
                List<EnumLanguage> languages = dialog.DirtLanguageCheckBox.Where(r => r.Value.IsChecked == true).Select(r => r.Key).ToList();
                Globals.CurrentProject?.ReloadTranslationdText(project, languages, dialog.CheckBox_ReloadOnlyModify.IsChecked == true);
                Globals.MainWindow.RefreshTranslatedText(Globals.CurrentProject);
            }
            e.Handled = true;
        }

        /// <summary>
        /// 重载项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_ReloadTranslation(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.ComponentsPathValid;
            e.Handled = true;
        }

        /// <summary>
        /// 重载(地图/Mod)项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_ReloadSourceText(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Assert(Globals.CurrentProject != null, "Globals.CurrentProject != null");
            string baseFolder = Globals.Preference.LastFolderPath;
            string filter = Globals.GetStringFromCurrentLanguage("TEXT_SC2File") + "|" + Class_ConstantAndEnum.FileName_SC2Components + "|" + Globals.GetStringFromCurrentLanguage("TEXT_ProjectFile") + "|*" + Class_ConstantAndEnum.Extension_SC2GameTran;
            string title = Globals.GetStringFromCurrentLanguage("UI_OpenFileDialog_Reload_Title");
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                if (fileDialog.FilterIndex == 1)
                {
                    Globals.Preference.LastFolderPath = file.DirectoryName;
                    ProjectReloadSourceText(file);
                }
                else
                {
                    Data_GameText project = GetProjectDataFile(file);
                    ProjectReload(project);
                }
            }
            e.Handled = true;
        }

        /// <summary>
        /// 重载(地图/Mod)项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_ReloadSourceText(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Globals.ComponentsPathValid;
            e.Handled = true;
        }

        /// <summary>
        /// 预览文本命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public void Executed_PreviewText(object sender, ExecutedRoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("HelpPanelAddTip(PlayerGroupAll(), StringToText(\"Translated\"), (StringToText(\"\")"); 
            for (int i = 0; i < CurrentFilterResultView.Count; i++)
            {
                if ((CurrentFilterResultView[i].Row[Data_GameText.RN_GameText_AddToPreview] is bool needAdd) && needAdd && CurrentFilterResultView[i].Row[Data_GameText.RN_GameText_ID] is string id)
                {
                    builder.Append(" + StringExternal(\"");
                    builder.Append(id);
                    builder.Append("\") + StringToText(\"\\r\\n\")");
                }
            }
            builder.Append("), StringToText(\"\"), \"Assets\\\\Textures\\\\ui_btn_generic_exclemation_red.dds\");\r\nHelpPanelDisplayPage(PlayerGroupAll(), c_helpPanelPageTips);");
            System.Windows.Clipboard.SetText(builder.ToString());
            Log.ShowSystemMessage(true, MessageBoxButton.OK, MessageBoxImage.Information, "MSG_CopyPreviewTextToClipboard");
            e.Handled = true;
        }

        /// <summary>
        /// 预览文本命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_PreviewText(object sender, CanExecuteRoutedEventArgs e)
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
            if (e.Parameter is string path)
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    ProjectOpen(file);
                    Globals.MainWindow.Backstage_Menu.IsOpen = false;
                }
                else
                {
                    if (Log.ShowSystemMessage(true, MessageBoxButton.YesNo, MessageBoxImage.None, "MSG_CanNotFindProjectFile", file.FullName) == MessageBoxResult.Yes)
                    {
                        RemoveRecentProject(file);
                    }
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

        /// <summary>
        /// 浏览Mod/Map执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_SearchClick(object sender, ExecutedRoutedEventArgs e)
        {
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 浏览Mod/Map判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_SearchClick(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CheckCurrentProjectExist();
            e.Handled = true;
        }

        /// <summary>
        /// 上一记录执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_FilterRecordPrev(object sender, ExecutedRoutedEventArgs e)
        {
            ListFilterRecordPrev();
            e.Handled = true;
        }

        /// <summary>
        /// 上一记录判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_FilterRecordPrev(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ListFilterRecordCanPrev();
            e.Handled = true;
        }

        /// <summary>
        /// 下一记录执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void Executed_FilterRecordNext(object sender, ExecutedRoutedEventArgs e)
        {
            ListFilterRecordNext();
            e.Handled = true;
        }

        /// <summary>
        /// 下一记录判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public static void CanExecuted_FilterRecordNext(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ListFilterRecordCanNext();
            e.Handled = true;
        }

        #endregion

        #region 筛选项

        #region 翻译语言筛选项

        /// <summary>
        /// 设置翻译语言控件资源引用
        /// </summary>
        /// <param name="controls">使用控件</param>
        /// <param name="status">使用状态</param>
        /// <param name="language">使用语言</param>
        private static void SetLanguageControlsResource(TranslatedLanguageControls controls, string status, EnumLanguage language)
        {
            string languageName = Globals.GetEnumLanguageName(language);
            TranslatedLanguageControls.SetRubbonButtonHeaderAndIcon(controls.ButtonTranslateSource, status, languageName);
            TranslatedLanguageControls.SetRubbonButtonHeaderAndIcon(controls.ButtonTranslateTarget, status, languageName);
            TranslatedLanguageControls.SetRubbonButtonHeaderAndIcon(controls.ButtonCopySource, status, languageName);
            TranslatedLanguageControls.SetRubbonButtonHeaderAndIcon(controls.ButtonCopyTarget, status, languageName);
            TranslatedLanguageControls.SetListItemHeaderAndIcon(controls.ListItemDetail, controls.ListItemTextDetail, status, languageName);
        }

        /// <summary>
        /// 刷新翻译选项
        /// </summary>
        /// <param name="project">项目</param>
        public void RefreshTranslateAndSearchLanguageButtons(Data_GameText project)
        {
            CanRefreshTranslatedText = false;
            ResetTranslateAndSearchLanguageButtons();
            if (project == null || !project.LangaugeRowList.Any())
            {
                InRibbonGallery_TranslatedLanguageSource.IsEnabled = false;
                InRibbonGallery_TranslatedLanguageTarget.IsEnabled = false;
                InRibbonGallery_CopySource.IsEnabled = false;
                InRibbonGallery_CopyTargets.IsEnabled = false;
                RibbonButton_DoCopy.IsEnabled = false;
            }
            else
            {
                EnumLanguage selectLanguage = EnumLanguage.Other;
                foreach (DataRow row in project.LangaugeRowList)
                {
                    EnumLanguage language = (EnumLanguage)row[Data_GameText.RN_Language_ID];
                    TranslatedLanguageControls controls = DictTranslateAndSearchLanguage[language];
                    InRibbonGallery_TranslatedLanguageSource.Items.Add(controls.ButtonTranslateSource);
                    InRibbonGallery_TranslatedLanguageTarget.Items.Add(controls.ButtonTranslateTarget);
                    InRibbonGallery_CopySource.Items.Add(controls.ButtonCopySource);
                    InRibbonGallery_CopyTargets.Items.Add(controls.ButtonCopyTarget);
                    Binding binding = new Binding("Tag")
                    {
                        ElementName = "InRibbonGallery_CopySource",
                        Converter = new CopyTargetLanguageButtonEnableConverter(),
                        ConverterParameter = controls.ButtonCopyTarget,
                    };
                    controls.ButtonCopyTarget.SetBinding(IsEnabledProperty, binding);
                    ListBox_GameTextShowLanguage.Items.Add(controls.ListItemDetail);
                    switch ((EnumGameUseStatus)row[Data_GameText.RN_Language_Status])
                    {
                        case EnumGameUseStatus.Droped:
                            SetLanguageControlsResource(controls, "Remove", language);
                            break;
                        case EnumGameUseStatus.Added:
                            SetLanguageControlsResource(controls, "Add", language);
                            break;
                        default:
                            SetLanguageControlsResource(controls, "Use", language);
                            break;
                    }
                    if (selectLanguage == EnumLanguage.Other || (int)language == CultureInfo.CurrentCulture.LCID)
                    {
                        selectLanguage = language;
                    }
                }
                if (selectLanguage != EnumLanguage.Other)
                {
                    InRibbonGallery_TranslatedLanguageSource.SelectedItem = DictTranslateAndSearchLanguage[selectLanguage].ButtonTranslateSource;
                    InRibbonGallery_TranslatedLanguageTarget.SelectedItem = DictTranslateAndSearchLanguage[selectLanguage].ButtonTranslateTarget;
                    InRibbonGallery_CopySource.SelectedItem = DictTranslateAndSearchLanguage[selectLanguage].ButtonCopySource;
                }
                InRibbonGallery_TranslatedLanguageSource.IsEnabled = true;
                InRibbonGallery_TranslatedLanguageTarget.IsEnabled = true;
                InRibbonGallery_CopySource.IsEnabled = true;
                InRibbonGallery_CopyTargets.IsEnabled = true;
                CanRefreshTranslatedText = true;
            }
        }

        /// <summary>
        /// 重置翻译和搜索语言项
        /// </summary>
        private void ResetTranslateAndSearchLanguageButtons()
        {
            InRibbonGallery_TranslatedLanguageSource.Items.Clear();
            InRibbonGallery_TranslatedLanguageTarget.Items.Clear();
            InRibbonGallery_CopyTargets.Items.Clear();
            InRibbonGallery_CopySource.Items.Clear();
            ListBox_GameTextShowLanguage.Items.Clear();

            foreach (EnumLanguage language in Globals.AllLanguage)
            {
                TranslatedLanguageControls controls = DictTranslateAndSearchLanguage[language];
                SetLanguageControlsResource(controls, "Use", language);
                controls.ButtonCopyTarget.IsChecked = false;
                controls.ListItemDetail.IsSelected = true;
            }
        }

        /// <summary>
        /// 新建语言切换按钮
        /// </summary>
        /// <returns>按钮</returns>
        private static Dictionary<EnumLanguage, TranslatedLanguageControls> NewTranslateAndSerachLanguageButton()
        {
            Dictionary<EnumLanguage, TranslatedLanguageControls> list = new Dictionary<EnumLanguage, TranslatedLanguageControls>();

            EnumLanguage[] array = Globals.AllLanguage.Cast<EnumLanguage>().ToArray();
            Array.Sort(array, (p1, p2) => string.Compare(Globals.GetEnumLanguageName(p1), Globals.GetEnumLanguageName(p2), StringComparison.Ordinal));
            foreach (EnumLanguage language in array)
            {
                list.Add(language, new TranslatedLanguageControls(language));
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
            foreach (ToggleButton button in GalaxyButtons)
            {
                InRibbonGallery_GalaxyFileFilter.Items.Remove(button);
            }
            GalaxyButtons.Clear();
            if (project != null)
            {
                foreach (DataRow row in project.Tables[Data_GameText.TN_GalaxyFile].Rows)
                {
                    ToggleButton button = NewGalaxyTextFileFilterButton(row);
                    GalaxyButtons.Add(button);
                    button.ToolTip = GetGalaxyFileToolTip(row);
                    InRibbonGallery_GalaxyFileFilter.Items.Insert(InRibbonGallery_GalaxyFileFilter.Items.Count - 1, button);
                }
                ToggleButton_GalaxyFileFilterNone.IsChecked = true;
                ToggleButton_GalaxyFileFilterNone.IsEnabled = true;
            }
            else
            {
                ToggleButton_GalaxyFileFilterNone.IsChecked = false;
                ToggleButton_GalaxyFileFilterNone.IsEnabled = false;
            }
            InRibbonGallery_GalaxyFileFilter.SelectedItem = null;
        }

        /// <summary>
        /// 获取Galaxy文件对应ToolTip
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>Tooltip</returns>
        public Fluent.ScreenTip GetGalaxyFileToolTip(DataRow row)
        {
            Fluent.ScreenTip tip = new Fluent.ScreenTip()
            {
                Width = 300,
                HelpTopic = Class_ConstantAndEnum.Link_HelpTopic,
                IsRibbonAligned = false,
            };
            tip.SetResourceReference(Fluent.ScreenTip.TitleProperty, "TP_RibbonButton_GalaxyFIle_Title");
            tip.SetResourceReference(Fluent.ScreenTip.DisableReasonProperty, "TP_RibbonButton_GalaxyFIle_DisableReason");
            List<object> list = new List<object>()
            {
                "TP_RibbonButton_GalaxyFIle_Text",
                row,
                Data_GameText.RN_GalaxyFile_Name,
                Data_GameText.RN_GalaxyFile_Path,
                Data_GameText.RN_GalaxyFile_Count,
            };
            IValueConverter converter = App.Current.FindResource("Converter_FormatText") as IValueConverter;
            Binding binding = new Binding("EnumCurrentLanguage")
            {
                Converter = converter,
                ConverterParameter = list,
                Source = this,
            };
            tip.SetBinding(Fluent.ScreenTip.TextProperty, binding);

            return tip;
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
                SizeDefinition = new RibbonControlSizeDefinition(RibbonControlSize.Middle, RibbonControlSize.Middle, RibbonControlSize.Middle),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                IsChecked = true,
                Height = ToggleButton_GalaxyFileFilterNone.Height,
                FontSize = ToggleButton_GalaxyFileFilterNone.FontSize,
                VerticalContentAlignment = VerticalAlignment.Center
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
            ComboBox_SearchScope.IsEnabled = isEnable;
            ComboBox_SearchScope.SelectedIndex = 0;
            ComboBox_SearchLocation.SelectedIndex = 0;
            RadioButton_SearchNull.IsEnabled = isEnable;
            RadioButton_SearchNull.IsChecked = false;
            RadioButton_SearchRegex.IsEnabled = isEnable;
            RadioButton_SearchRegex.IsChecked = false;
            RadioButton_SearchNormal.IsEnabled = isEnable;
            RadioButton_SearchNormal.IsChecked = true;
            CheckBox_SearchCase.IsEnabled = isEnable;
            CheckBox_SearchCase.IsChecked = false;
            TextBox_SearchKeyword.IsEnabled = isEnable;
            TextBox_SearchKeyword.Text = "";
        }

        /// <summary>
        /// 刷新搜索相关控件状态
        /// </summary>
        /// <param name="isEnable">项目数据</param>
        private void ResetSearchControlToDefault(bool isEnable)
        {
            ComboBox_SearchScope.IsEnabled = isEnable;
            ComboBox_SearchScope.SelectedIndex = 0;
            ComboBox_SearchLocation.SelectedIndex = 0;
            RadioButton_SearchNull.IsEnabled = isEnable;
            RadioButton_SearchNull.IsChecked = false;
            RadioButton_SearchRegex.IsEnabled = isEnable;
            RadioButton_SearchRegex.IsChecked = false;
            RadioButton_SearchNormal.IsEnabled = isEnable;
            RadioButton_SearchNormal.IsChecked = true;
            CheckBox_SearchCase.IsEnabled = isEnable;
            CheckBox_SearchCase.IsChecked = false;
            TextBox_SearchKeyword.IsEnabled = isEnable;
            TextBox_SearchKeyword.Text = "";
            foreach (ToggleButton button in InRibbonGallery_GalaxyFileFilter.Items)
            {
                button.IsChecked = true;
            }
            foreach (ToggleButton button in InRibbonGallery_TextFileFilter.Items)
            {
                button.IsChecked = true;
            }
            foreach (ToggleButton button in InRibbonGallery_TextStatusFilter.Items)
            {
                button.IsChecked = true;
            }
            foreach (ToggleButton button in InRibbonGallery_UseStatusFilter.Items)
            {
                button.IsChecked = true;
            }
        }
        #endregion

        #region 筛选项数据

        #region 搜索翻译语言

        /// <summary>
        /// 获取搜索翻译语言(源)
        /// </summary>
        /// <returns>搜索翻译语言</returns>
        public EnumLanguage GetFileterTranslatedLanguageSource()
        {
            return EnumCurrentTranLangSource;
        }

        /// <summary>
        /// 设置搜索翻译语言(源)
        /// </summary>
        /// <param name="value">搜索翻译语言</param>
        public void SetFileterTranslatedLanguageSource(EnumLanguage value)
        {
            foreach (object select in InRibbonGallery_TranslatedLanguageSource.Items)
            {
                if (select is ToggleButton button && button.Tag != null)
                {
                    if ((EnumLanguage)button.Tag == value)
                    {
                        button.IsChecked = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取搜索翻译语言（目标）
        /// </summary>
        /// <returns>搜索翻译语言</returns>
        public EnumLanguage GetFileterTranslatedLanguageTarget()
        {
            return EnumCurrentTranLangTarget;
        }

        /// <summary>
        /// 设置搜索翻译语言（目标）
        /// </summary>
        /// <param name="value">搜索翻译语言</param>
        public void SetFileterTranslatedLanguageTarget(EnumLanguage value)
        {
            foreach (object select in InRibbonGallery_TranslatedLanguageTarget.Items)
            {
                if (select is ToggleButton button && button.Tag != null)
                {
                    if ((EnumLanguage)button.Tag == value)
                    {
                        button.IsChecked = true;
                        break;
                    }
                }
            }
        }


        #endregion

        #region 搜索类型

        /// <summary>
        /// 获取搜索类型
        /// </summary>
        /// <returns>搜索类型</returns>
        public EnumSearchTextType GetFileterSearchTextType()
        {
            if (ComboBox_SearchScope.SelectedItem != null && ComboBox_SearchScope.SelectedItem is ComboBoxItem item)
            {
                return (EnumSearchTextType?) item.Tag ?? EnumSearchTextType.All;
            }
            else
            {
                return EnumSearchTextType.All;
            }
        }

        /// <summary>
        /// 设置搜索类型
        /// </summary>
        /// <param name="value">搜索类型</param>
        public void SetFileterSearchTextType(EnumSearchTextType value)
        {
            foreach (object select in ComboBox_SearchScope.Items)
            {
                if (select is ComboBoxItem item && item.Tag != null)
                {
                    if ((EnumSearchTextType)item.Tag == value)
                    {
                        ComboBox_SearchScope.SelectedItem = item;
                    }
                }
            }
        }

        #endregion
        
        #region 搜索位置

        /// <summary>
        /// 获取搜索位置
        /// </summary>
        /// <returns>搜索位置</returns>
        public EnumSearchTextLocation GetFileterSearchLocation()
        {
            if (ComboBox_SearchLocation.SelectedItem != null && ComboBox_SearchLocation.SelectedItem is ComboBoxItem item)
            {
                return (EnumSearchTextLocation?) item.Tag ?? EnumSearchTextLocation.All;
            }
            else
            {
                return EnumSearchTextLocation.All;
            }
        }

        /// <summary>
        /// 设置搜索位置
        /// </summary>
        /// <param name="value">搜索位置</param>
        public void SetFileterSearchLocation(EnumSearchTextLocation value)
        {
            foreach (object select in ComboBox_SearchLocation.Items)
            {
                if (select is ComboBoxItem item && item.Tag != null)
                {
                    if ((EnumSearchTextLocation)item.Tag == value)
                    {
                        ComboBox_SearchLocation.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        #endregion

        #region 搜索方式

        /// <summary>
        /// 获取搜索方式
        /// </summary>
        /// <returns>搜索方式</returns>
        public EnumSearchWay GetFileterSearchWay()
        {
            if (RadioButton_SearchNull.IsChecked == true)
            {
                return EnumSearchWay.Null;
            }
            else if (RadioButton_SearchRegex.IsChecked == true)
            {
                return EnumSearchWay.Regex;
            }
            else
            {
                return EnumSearchWay.Text;
            }
        }

        /// <summary>
        /// 设置搜索位置
        /// </summary>
        /// <param name="value">搜索方式</param>
        public void SetFileterSearchWay(EnumSearchWay value)
        {
            switch (value)
            {
                case EnumSearchWay.Null:
                    RadioButton_SearchNull.IsChecked = true;
                    break;
                case EnumSearchWay.Regex:
                    RadioButton_SearchRegex.IsChecked = true;
                    break;
                case EnumSearchWay.Text:
                    RadioButton_SearchNormal.IsChecked = true;
                    break;
            }
        }

        #endregion

        #region 搜索大小写


        /// <summary>
        /// 获取搜索大小写
        /// </summary>
        /// <returns>搜索大小写</returns>
        public bool GetFileterSearchCase()
        {
            return CheckBox_SearchCase.IsChecked == true;
        }

        /// <summary>
        /// 设置搜索大小写
        /// </summary>
        /// <param name="value">搜索大小写</param>
        public void SetFileterSearchCase(bool value)
        {
            CheckBox_SearchCase.IsChecked = value;
        }

        #endregion

        #region 搜索Galaxy
        
        /// <summary>
        /// 获取搜索方式
        /// </summary>
        /// <returns>搜索方式</returns>
        public string[] GetFileterGalaxyFile()
        {
            List<string> files = new List<string>();
            if (ToggleButton_GalaxyFileFilterNone.IsChecked == true) files.Add("");
            foreach (ToggleButton select in GalaxyButtons)
            {
                if (select.Tag is DataRow row && row[Data_GameText.RN_GalaxyFile_Path] is string path)
                {
                    if (select.IsChecked == true) files.Add(path);
                }
            }
            return files.ToArray();
        }

        /// <summary>
        /// 设置搜索位置
        /// </summary>
        /// <param name="files">搜索方式</param>
        public void SetFileterGalaxyFile(string[] files)
        {
            ToggleButton_GalaxyFileFilterNone.IsChecked = files.Contains("");
            List<ToggleButton> buttons = GalaxyButtons.Where(r => r.Tag is DataRow row && row[Data_GameText.RN_GalaxyFile_Path] is string path && files.Contains(path)).Select(r => r).ToList();
            foreach (ToggleButton button in GalaxyButtons)
            {
                button.IsChecked = buttons.Contains(button);
            }
        }

        #endregion

        #region 搜索文本文件


        /// <summary>
        /// 获取搜索文本文件
        /// </summary>
        /// <returns>搜索文本文件</returns>
        public EnumGameTextFile GetFileterTextFile()
        {
            return TextFileFilter;
        }

        /// <summary>
        /// 设置搜索文本文件
        /// </summary>
        /// <param name="value">搜索文本文件</param>
        public void SetFileterTextFile(EnumGameTextFile value)
        {
            foreach (object select in InRibbonGallery_TextFileFilter.Items)
            {
                if (select is ToggleButton button && button.Tag != null)
                {
                    button.IsChecked = value.HasFlag((EnumGameTextFile)button.Tag);
                }
            }
        }


        #endregion

        #region 搜索文本状态
        
        /// <summary>
        /// 获取搜索文本状态
        /// </summary>
        /// <returns>搜索文本状态</returns>
        public EnumGameTextStatus GetFileterTextStatus()
        {
            return TextStatusFilter;
        }

        /// <summary>
        /// 设置搜索文本状态
        /// </summary>
        /// <param name="value">搜索文本状态</param>
        public void SetFileterTextStatus(EnumGameTextStatus value)
        {
            foreach (object select in InRibbonGallery_TextStatusFilter.Items)
            {
                if (select is ToggleButton button && button.Tag != null)
                {
                    button.IsChecked = value.HasFlag((EnumGameTextStatus)button.Tag);
                }
            }
        }

        #endregion

        #region 搜索使用状态

        /// <summary>
        /// 获取搜索使用状态
        /// </summary>
        /// <returns>搜索使用状态</returns>
        public EnumGameUseStatus GetFileterUseStatus()
        {
            return UseStatusFilter;
        }

        /// <summary>
        /// 设置搜索使用状态
        /// </summary>
        /// <param name="value">搜索使用状态</param>
        public void SetFileterUseStatus(EnumGameUseStatus value)
        {
            foreach (object select in InRibbonGallery_UseStatusFilter.Items)
            {
                if (select is ToggleButton button && button.Tag != null)
                {
                    button.IsChecked = value.HasFlag((EnumGameUseStatus)button.Tag);
                }
            }
        }

        #endregion

        #region 滚动位置


        /// <summary>
        /// 获取搜索使用状态
        /// </summary>
        /// <returns>搜索使用状态</returns>
        public int GetFileterScrollRowIndex()
        {
            return ScrollRowIndex;
        }

        /// <summary>
        /// 设置搜索使用状态
        /// </summary>
        /// <param name="value">搜索使用状态</param>
        public void SetFileterScrollRowIndex(int value)
        {
            ScrollToItemByIndex(value);
        }

        #endregion

        #endregion

        #region 筛选记录

        /// <summary>
        /// 清理筛选记录
        /// </summary>
        public static void ListFilterRecordClear()
        {
            ListFilterRecordPointer = -1;
            ListFilterRecord.Clear();
        }

        /// <summary>
        /// 新建 筛选记录
        /// </summary>
        public static void ListFilterRecordNew()
        {
            if (!CanSaveRecord || !CanRefreshTranslatedText) return;
            Class_SearchConfig config = Class_SearchConfig.NewSearchConfig();
            if (ListFilterRecordPointer >= 0 && config.Equals(ListFilterRecord[ListFilterRecordPointer]) && config.ScrollRowIndex == ListFilterRecord[ListFilterRecordPointer].ScrollRowIndex) return;
            ListFilterRecordPointer++;
            ListFilterRecord = ListFilterRecord.Take(ListFilterRecordPointer).Select(r=>r).ToList();
            ListFilterRecord.Add(config);
            Globals.MainWindow.CurrentProjectNeedSave = true;
        }

        /// <summary>
        /// 是否可以应用下一条记录
        /// </summary>
        /// <returns>结果</returns>
        public static bool ListFilterRecordCanNext()
        {
            return ListFilterRecordPointer < ListFilterRecord.Count - 1;
        }

        /// <summary>
        /// 应用下一条记录
        /// </summary>
        public static void ListFilterRecordNext()
        {
            bool isRefrshText = !ListFilterRecord[ListFilterRecordPointer].Equals(ListFilterRecord[ListFilterRecordPointer + 1]);
            if (ListFilterRecordCanNext()) ListFilterRecord[++ListFilterRecordPointer].ApplyToUI(isRefrshText);
        }

        /// <summary>
        /// 是否可以应用上一条记录
        /// </summary>
        /// <returns>结果</returns>
        public static bool ListFilterRecordCanPrev()
        {
            return ListFilterRecordPointer > 0;
        }

        /// <summary>
        /// 应用上一条记录
        /// </summary>
        public static void ListFilterRecordPrev()
        {
            bool isRefrshText = !ListFilterRecord[ListFilterRecordPointer].Equals(ListFilterRecord[ListFilterRecordPointer - 1]);
            if (ListFilterRecordCanPrev()) ListFilterRecord[--ListFilterRecordPointer].ApplyToUI(isRefrshText);
        }

        #endregion

        #endregion

        #region DataGrid

        #region Scroll

        /// <summary>
        /// 设置滚动到指定项第一行
        /// </summary>
        /// <param name="row">滚动到的第一行</param>
        /// <param name="callback">回调</param>
        private void ThreadScrollItemToFirstRow(DataRow row, Delegate_ScrollItemToFirstRow_Callback callback)
        {
            if (CurrentFilterResultView != null && CurrentFilterResultView.Count != 0)
            {
                object selectItem = CurrentFilterResultView[0];
                ScrollRowIndex = 0;
                for (int i = 0; i < CurrentFilterResultView.Count; i++)
                {
                    if (CurrentFilterResultView[i].Row == row)
                    {
                        selectItem = CurrentFilterResultView[i];
                        ScrollRowIndex = i;
                        break;
                    }
                }
                CanSaveRecord = false;
                DataGrid_TranslatedTexts.UnselectAllCells();
                DataGrid_TranslatedTexts.SelectedItem = selectItem;
                DataGrid_TranslatedTexts.CurrentItem = selectItem;
                DataGrid_TranslatedTexts.ScrollIntoView(CurrentFilterResultView.Cast<DataRowView>().Last());
                DataGrid_TranslatedTexts.ScrollIntoView(selectItem);
                CanSaveRecord = true;
            }
            callback?.Invoke();
        }

        /// <summary>
        /// 设置滚动到指定项第一行
        /// </summary>
        /// <param name="row">滚动到的第一行</param>
        /// <param name="callback">回调</param>
        private void ScrollItemToFirstRow(DataRow row, Delegate_ScrollItemToFirstRow_Callback callback)
        {
            IsEnabled = false;
            Delegate_ScrollItemToFirstRow scroll = ThreadScrollItemToFirstRow;
            DataGrid_TranslatedTexts.Dispatcher.BeginInvoke(scroll, DispatcherPriority.Background, row, callback);
        }

        /// <summary>
        /// 设置表格数据并滚动到指定行回调
        /// </summary>
        private void SetViewAndScollTranslatedText_CallBack()
        {
            IsEnabled = true;
            DataGrid_TranslatedTexts.Focus();
        }

        /// <summary>
        /// 设置表格数据并滚动到指定行
        /// </summary>
        /// <param name="view">表格数据</param>
        /// <param name="row">行</param>
        public void SetViewAndScollTranslatedText(DataView view , DataRow row)
        {
            DataGrid_TranslatedTexts.ItemsSource = view;
            ScrollItemToFirstRow(row, SetViewAndScollTranslatedText_CallBack);
        }

        /// <summary>
        /// 通过Key滚动到目标项
        /// </summary>
        /// <param name="id">滚动到的ID</param>
        /// <param name="callback">回调</param>
        /// <returns>是否找到滚动目标</returns>
        private bool? ScrollToItemByID(string id, Delegate_ScrollItemToFirstRow_Callback callback)
        {
            DataRow row = CurrentTextData.Rows.Find(id);
            if (row == null) return null;
            object selectItem = null;
            for (int i = 0; i < CurrentFilterResultView.Count; i++)
            {
                if (CurrentFilterResultView[i].Row == row)
                {
                    selectItem = CurrentFilterResultView[i];
                    break;
                }
            }
            if (selectItem == null) return false;
            ScrollItemToFirstRow(row, callback);
            return true;
        }

        /// <summary>
        /// 通过Key滚动到目标项
        /// </summary>
        /// <param name="id">滚动到的ID</param>
        private void ScrollToItemByID(string id)
        {
            switch (ScrollToItemByID(id, SetViewAndScollTranslatedText_CallBack))
            {
                case false:
                    break;
                case null:
                    Log.Assert(false, "Error ID");
                    break;
                default:
                    return;
            }
            if (Log.ShowSystemMessage(true, MessageBoxButton.YesNo, MessageBoxImage.None, "MSG_SrollToItemFindOutID", id) == MessageBoxResult.Yes)
            {
                CanRefreshTranslatedText = false;
                ResetSearchControlToDefault(true);
                CanRefreshTranslatedText = true;
                RefreshTranslatedText();
                ScrollToItemByID(id, SetViewAndScollTranslatedText_CallBack);
            }
        }

        /// <summary>
        /// 通过Key滚动到目标项
        /// </summary>
        /// <param name="index">滚动到的Index</param>
        private void ScrollToItemByIndex(int index)
        {
            if (CurrentFilterResultView == null) return;
            string id = CurrentFilterResultView[index].Row[Data_GameText.RN_GameText_ID] as string;
            switch (ScrollToItemByID(id, SetViewAndScollTranslatedText_CallBack))
            {
                case false:
                    break;
                case null:
                    Log.Assert(false, "Error ID");
                    break;
                default:
                    return;
            }
            if (Log.ShowSystemMessage(true, MessageBoxButton.YesNo, MessageBoxImage.None, "MSG_SrollToItemFindOutID", id) == MessageBoxResult.Yes)
            {
                CanRefreshTranslatedText = false;
                ResetSearchControlToDefault(true);
                CanRefreshTranslatedText = true;
                RefreshTranslatedText();
                ScrollToItemByID(id, SetViewAndScollTranslatedText_CallBack);
            }
        }

        #endregion

        #region Refresh

        /// <summary>
        /// 刷新翻译文本
        /// </summary>
        /// <param name="project">项目数据</param>
        public void RefreshTranslatedText(Data_GameText project)
        {
            if (project == null)
            {
                CurrentTextData = null;
                CurrentFilterResultView = null;
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
            DataRow selectDataRow = null;
            if (DataGrid_TranslatedTexts.SelectedCells.Count > 0)
            {
                object info = DataGrid_TranslatedTexts.SelectedCells[0].Item;
                if (info is DataRowView rowView)
                {
                    selectDataRow = rowView.Row;
                }
            }
            EnumerableRowCollection<DataRow> query = CurrentTextData.AsEnumerable();
            if (TextFileFilter != EnumGameTextFile.All)
            {
                string keyFile = Data_GameText.RN_GameText_File;
                query = from row in query
                        where TextFileFilter.HasFlag((EnumGameTextFile)row[keyFile])
                        select row;
            }
            if (TextStatusFilter != EnumGameTextStatus.All)
            {
                string keyStatus = Data_GameText.GetRowNameForLanguage(EnumCurrentLanguage, Data_GameText.RN_GameText_TextStatus);
                query = from row in query
                        where (TextStatusFilter).HasFlag((EnumGameTextStatus)row[keyStatus])
                        select row;
            }
            if (UseStatusFilter != EnumGameUseStatus.All)
            {
                string keyStatus = Data_GameText.GetRowNameForLanguage(EnumCurrentLanguage, Data_GameText.RN_GameText_UseStatus);
                query = from row in query
                        where (UseStatusFilter).HasFlag((EnumGameUseStatus)row[keyStatus])
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
            DataView view = query.AsDataView();
            view.Sort = Data_GameText.RN_GameText_Index + " ASC";
            CurrentFilterResultView = view;
            SetViewAndScollTranslatedText(view, selectDataRow);
            ListFilterRecordNew();
        }

        /// <summary>
        /// 获取需要搜索的Key的列表
        /// </summary>
        /// <returns></returns>
        private List<string> GetSearchKeyList()
        {
            List<string> keyList = new List<string>();
            EnumSearchTextType type = (EnumSearchTextType)((ComboBoxItem)ComboBox_SearchScope.SelectedItem).Tag;
            // ID
            if (type.HasFlag(EnumSearchTextType.ID))
            {
                keyList.Add(Data_GameText.RN_GameText_ID);
            }

            // Droped Text
            if (type.HasFlag(EnumSearchTextType.Droped))
            {
                keyList.Add(Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_DropedText));
            }

            // Source Text
            if (type.HasFlag(EnumSearchTextType.Source))
            {
                keyList.Add(Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangSource, Data_GameText.RN_GameText_SourceText));
            }

            // Edited Text
            if (type.HasFlag(EnumSearchTextType.Edited))
            {
                keyList.Add(Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_EditedText));
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
            string match = TextBox_SearchKeyword.Text;
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
            string match = TextBox_SearchKeyword.Text;
            if (String.IsNullOrEmpty(match)) return;
            EnumSearchTextLocation location = (EnumSearchTextLocation)((ComboBoxItem) ComboBox_SearchLocation.SelectedItem).Tag;
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
            if (!locations.Any())
            {
                return GalaxyFilter.Contains(Globals.Const_NoUseInGalaxy);
            }

            return locations.Count(r => GalaxyFilter.Contains(r.GetParentRow(Data_GameText.RSN_GalaxyLine_GameLocation_Line)[Data_GameText.RN_GalaxyLine_File])) != 0;
        }

        /// <summary>
        /// 在搜索结果中（空文本）
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="keyList">数据列名列表</param>
        /// <returns>判断结果</returns>
        private static bool IsInSearchResult_NullLangauge(DataRow row, List<string> keyList)
        {
            foreach (string key in keyList)
            {
                if (row[key] == DBNull.Value || string.IsNullOrEmpty(row[key] as string))
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
                if (row[key] is string value && match.IsMatch(value))
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
            if (ignoreCase && row[key] is string value)
            {
                value = value.ToLower();
                return value.Contains(match);
            }
            else
            {
                return false;
            }
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
            if (ignoreCase && row[key] is string value)
            {
                value = value.ToLower();
                return value.StartsWith(match);
            }
            else
            {
                return false;
            }
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
            if (ignoreCase && row[key] is string value)
            {
                value = value.ToLower();
                return value.EndsWith(match);
            }
            else
            {
                return false;
            }
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

        #endregion

        #region Other

        /// <summary>
        /// 刷新游戏文本单语言数据表
        /// </summary>
        /// <param name="refreshItem">仅筛选</param>
        private void RefreshGameTextDetails(bool refreshItem)
        {
            if (refreshItem && LastSelectedCell != null)
            {
                DataRow row = LastSelectedCell.Row;
                List<EnumLanguage> langList = new List<EnumLanguage>();
                foreach (ListBoxItem item in ListBox_GameTextShowLanguage.Items)
                {
                    langList.Add((EnumLanguage)item.Tag);
                }
                Data_GameText.RefreshGameTextForLanguageTable(row, langList);
            }
            List<EnumLanguage> showLanguages = ListBox_GameTextShowLanguage.SelectedItems.Cast<ListBoxItem>().Select(r => (EnumLanguage)r.Tag).ToList();
            EnumerableRowCollection<DataRow> query = Data_GameText.GameTextForLanguageTable.AsEnumerable();

            query = from row in query
                    where showLanguages.Contains((EnumLanguage)row[Data_GameText.RN_GameTextForLanguage_Language])
                    select row;

            DataGrid_GameTextForLanguage.ItemsSource = query.AsDataView();
        }

        /// <summary>
        /// 刷新Galaxy中文本
        /// </summary>
        private void RefreshInGalaxyTextDetails()
        {
            if (Globals.CurrentProject != null && LastSelectedCell != null)
            {
                DataView view = Globals.CurrentProject.GetRelateGalaxyLineRows(LastSelectedCell.Row);
                DataGrid_GameTextInGalaxy.ItemsSource = view;
                TextBlock_DetailsID.Text = Globals.GetStringFromCurrentLanguage("UI_TextBlock_DetailsID_Text", LastSelectedCell.Row[Data_GameText.RN_GameText_ID]);
                TextBlock_DetailsIndex.Text = Globals.GetStringFromCurrentLanguage("UI_TextBlock_DetailsIndex_Text", LastSelectedCell.Row[Data_GameText.RN_GameText_Index]);
            }
            else
            {
                DataGrid_GameTextInGalaxy.ItemsSource = null;
                TextBlock_DetailsID.Text = Globals.GetStringFromCurrentLanguage("UI_TextBlock_DetailsID_Text", "");
                TextBlock_DetailsIndex.Text = Globals.GetStringFromCurrentLanguage("UI_TextBlock_DetailsIndex_Text", "");
            }
        }

        /// <summary>
        /// 刷新详情控件
        /// </summary>
        /// <param name="project">项目数据</param>
        private void RefreshDetails(Data_GameText project)
        {
            bool isCheck = project != null;
            CheckBox_InGalaxyShowScript.IsEnabled = isCheck;
        }

        /// <summary>
        /// 编辑文本变化
        /// </summary>
        /// <returns>取消变化</returns>
        private void OnEditedTextChange(DataRowView view, string newValue)
        {
            if (view == null) return;
            DataRow row = view.Row;
            string keyStatus = Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_TextStatus);
            string keySource = Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangSource, Data_GameText.RN_GameText_SourceText);
            string value = newValue;
            switch ((EnumGameTextStatus)row[keyStatus])
            {
                case EnumGameTextStatus.Empty:
                    if (!string.IsNullOrEmpty(value))
                    {
                        row[keyStatus] = EnumGameTextStatus.Modified;
                        CurrentProjectNeedSave = true;
                    }
                    break;
                case EnumGameTextStatus.Normal:
                    if (value != row[keySource] as string)
                    {
                        row[keyStatus] = EnumGameTextStatus.Modified;
                        CurrentProjectNeedSave = true;
                    }
                    break;
            }
        }

        #endregion
        
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

            Globals.CurrentProjectPath = null;
            return false;
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="file">保存路径</param>
        public static void ProjectSave(FileInfo file)
        {
            Log.Assert(Globals.CurrentProject != null, "Globals.CurrentProject == null");
            Globals.MainWindow.CurrentProjectName = file.Name;
            Globals.MainWindow.CurrentProjectNeedSave = false;
            Globals.CurrentProject?.SaveProject(file);
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="isSaveAs">是否另存为</param>
        /// <return>是否保存</return>
        public static bool ProjectSave(bool isSaveAs)
        {
            Log.Assert(Globals.CurrentProject != null, "Globals.CurrentProject == null");
            if (!isSaveAs && Globals.CurrentProjectPath != null)
            {
                ProjectSave(Globals.CurrentProjectPath);
            }
            else
            {
                string baseFolder = Globals.Preference.LastFolderPath;
                string filter = Globals.GetStringFromCurrentLanguage("TEXT_ProjectFile") + "|*" + Class_ConstantAndEnum.Extension_SC2GameTran;
                string key = isSaveAs ? "UI_OpenFileDialog_SaveAs_Title" : "UI_OpenFileDialog_Save_Title";
                string title = Globals.GetStringFromCurrentLanguage(key);
                if (Globals.SaveFilePathDialog(baseFolder, filter, title, out SaveFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
                {
                    FileInfo file = new FileInfo(fileDialog.FileName);
                    Globals.Preference.LastFolderPath = file.DirectoryName;
                    Globals.CurrentProjectPath = file;
                    ProjectSave(file);
                    AddRecentProject(file);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 询问保存
        /// </summary>
        public static void AskForSave()
        {
            while(true)
            {
                if (Log.ShowSystemMessage(true, MessageBoxButton.YesNo, MessageBoxImage.None, "MSG_AskForSave") != MessageBoxResult.Yes || ProjectSave(false))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public static void ProjectClose()
        {
            if (Globals.CurrentProject != null)
            {
                if (Globals.MainWindow.CurrentProjectNeedSave)
                {
                    AskForSave();
                }
                Globals.CurrentProject = null;
                Globals.CurrentProjectPath = null;
                Globals.MainWindow.CurrentProjectName = null;
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
        public static void ProjectReloadSourceText(FileInfo file)
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
            string baseFolder = Globals.MainWindow.TextBox_ComponentsPath.Text;
            string filter = Globals.GetStringFromCurrentLanguage("TEXT_SC2File") + "|" + Class_ConstantAndEnum.FileName_SC2Components;
            string title = Globals.GetStringFromCurrentLanguage("UI_OpenFileDialog_CompontentsPath_Title");
            if (Globals.OpenFilePathDialog(baseFolder, filter, title, out OpenFileDialog fileDialog) == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                Globals.CurrentProject.SC2Components = file;
                return true;
            }

            return false;
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
            LastEditedCell = null;
            RefreshTranslateAndSearchLanguageButtons(newPro);
            RefreshGalaxyTextFileFilterButton(newPro);
            RefreshTextFileFilterButton(newPro);
            RefreshTextStatusFilterButton(newPro);
            RefreshUseStatusFilterButton(newPro);
            RefreshSearchControl(newPro);
            newPro?.UseSerachConfigData();
            ListFilterRecordClear();
            CanRefreshTranslatedText = true;
            RefreshTranslatedText(newPro);
            RefreshInGalaxyTextDetails();
            RefreshDetails(newPro);
            if (newPro == null)
            {
                CurrentProjectName = null;
            }
            else if (Globals.CurrentProjectPath == null)
            {
                CurrentProjectName = "TEXT_WindowTitleProjectUnsaved";
            }
            else
            {
                CurrentProjectName = Globals.CurrentProjectPath.Name;
            }
            CurrentProjectNeedSave = false;
        }

        /// <summary>
        /// 获取翻译语言对应的绑定
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="name">基本名称</param>
        /// <returns>绑定</returns>
        private Binding GetRowBinding(EnumLanguage language, string name)
        {
            Binding binding = new Binding(Data_GameText.GetRowNameForLanguage(language, name))
            {
                Mode = BindingMode.TwoWay,
                Converter = new TestValueConverter(),
            };
            return binding;
        }

        /// <summary>
        /// 刷新当前翻译语言(源)
        /// </summary>
        /// <param name="language">翻译语言</param>
        private void RefreshEnumCurrentTranLangSource(EnumLanguage language)
        {
            if (language == 0)
            {
            }
        }

        /// <summary>
        /// 刷新当前翻译语言(目标)
        /// </summary>
        /// <param name="language">翻译语言</param>
        private void RefreshEnumCurrentTranLangTarget(EnumLanguage language)
        {
            DataGridColumn_TranslatedEditedText.Binding = language == 0 ? null : GetRowBinding(language, Data_GameText.RN_GameText_EditedText);

            RefreshInGalaxyTextDetails();
            ResetEditedTextBinding();
            RefreshTranslatedText();
        }

        /// <summary>
        /// 重置编辑文本绑定
        /// </summary>
        private void ResetEditedTextBinding()
        {
            if (LastEditedCell != null)
            {
                Binding binding = new Binding(Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_EditedText))
                {
                    Source = LastEditedCell,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,                   
                };
                TextBox_EditedBox.IsEnabled = true;
                TextBox_EditedBox.SetBinding(TextBox.TextProperty, binding);
            }
            else
            {
                TextBox_EditedBox.IsEnabled = false;
                BindingOperations.ClearBinding(TextBox_EditedBox, TextBox.TextProperty);
                TextBox_EditedBox.Text = "";
            }
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
            BackstageTabItem_RecentProjects.IsEnabled = ItemsControl_RecentFiles.Items.Count != 0;
        }

        /// <summary>
        /// 新建最近文件按钮
        /// </summary>
        /// <param name="path">路径</param>
        private void NewRecentProejct(string path)
        {
            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/Image/ui-editoricon-triggereditor_newcomment.png"));
            Image image = new Image
            {
                Source = bitmap,
                Margin = new Thickness(3),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            TextBlock text = new TextBlock
            {
                Text = path,
                Margin = new Thickness(3),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(3),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            ScreenTip tooltip = new ScreenTip()
            {
                HelpTopic = Class_ConstantAndEnum.Link_HelpTopic,
                Width = 500,
                Text = path,
            };
            tooltip.SetResourceReference(ScreenTip.TitleProperty, "TP_BackstageTabItem_RecentProjects_Title");
            tooltip.SetResourceReference(ScreenTip.ImageProperty, "IMAGE_ProjectFile");
            panel.Children.Add(image);
            panel.Children.Add(text);
            Button button = new Button
            {
                Content = panel,
                Command = CommandRecentProjects,
                CommandParameter = path,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                BorderThickness = new Thickness(0),
                ToolTip = tooltip,
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
            if (!(ComboBox_Language.SelectedItem is string itemName)) return;
            EnumCurrentLanguage = Globals.DictComboBoxItemLanguage[itemName];
            RefreshInGalaxyTextDetails();
            e.Handled = true;
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void RibbonWindow_Main_Closing(object sender, CancelEventArgs e)
        {
            ProjectClose();
            Globals.Preference.SavePreference();
        }

        /// <summary>
        /// Galaxy文件筛选全选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_GalaxyFileFilterSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            ToggleButton_GalaxyFileFilterNone.IsChecked = true;
            foreach (ToggleButton button in GalaxyButtons)
            {
                button.IsChecked = true;
            }
            CanRefreshTranslatedText = true;
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// Galaxy文件筛选全不选
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_GalaxyFileFilterSelectNone_Click(object sender, RoutedEventArgs e)
        {
            CanRefreshTranslatedText = false;
            ToggleButton_GalaxyFileFilterNone.IsChecked = false;
            foreach (ToggleButton button in GalaxyButtons)
            {
                button.IsChecked = false;
            }
            CanRefreshTranslatedText = true;
            Globals.MainWindow.RefreshTranslatedText();
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
            IsSelectAllGalaxyFilter = ToggleButton_GalaxyFileFilterNone.IsChecked == true;
            if (IsSelectAllGalaxyFilter) GalaxyFilter.Add(Globals.Const_NoUseInGalaxy);
            foreach (ToggleButton button in GalaxyButtons)
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
            Globals.MainWindow.RefreshTranslatedText();
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
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 文本文件筛选文件点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ToggleButton_TextFileFilterButton_CheckEvent(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton button)) return;
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
            Globals.MainWindow.RefreshTranslatedText();
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
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选文件点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ToggleButton_TextStatusFilterButton_CheckEvent(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton button)) return;
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
            Globals.MainWindow.RefreshTranslatedText();
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
            Globals.MainWindow.RefreshTranslatedText();
            e.Handled = true;
        }

        /// <summary>
        /// 文本状态筛选文件点击
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void ToggleButton_UseStatusFilterButton_CheckEvent(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton button)) return;
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
            if (Globals.CurrentProject == null || DataGrid_TranslatedTexts.CurrentItem is DataRowView)
            {
                LastSelectedCell = DataGrid_TranslatedTexts.CurrentItem as DataRowView;
            }
            RefreshGameTextDetails(true);
            RefreshInGalaxyTextDetails();
            if (CurrentFilterResultView != null && DataGrid_TranslatedTexts.CurrentItem is DataRowView rowView)
            {
                ScrollRowIndex = 0;
                for (int i = 0; i < CurrentFilterResultView.Count; i++)
                {
                    if (CurrentFilterResultView[i].Row[Data_GameText.RN_GameText_ID] == rowView.Row[Data_GameText.RN_GameText_ID])
                    {
                        ScrollRowIndex = i;
                        break;
                    }
                }
                ListFilterRecordNew();
            }
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

        /// <summary>
        /// Galaxy文本点击事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        public static void SC2_GameTranslater_MenuItemJumptToText_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.MenuItem item && item.Tag is Run run && run.Tag is string id)
            {
                Globals.MainWindow.ScrollToItemByID(id);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 表格数据准备编辑
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void DataGrid_TranslatedTexts_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {

            if (e.Column == DataGridColumn_TranslatedEditedText)
            {
                if (e.Row.Item is DataRowView view) LastEditedCell = view;
            }
        }

        /// <summary>
        /// 表格数据结束编辑
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void DataGrid_TranslatedTexts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column == DataGridColumn_TranslatedEditedText)
            {
                if (!(e.Row.Item is DataRowView view)) return;
                OnEditedTextChange(view, ((TextBox)e.EditingElement).Text);
            }
        }

        /// <summary>
        /// 输入完成事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void TextBox_EditedBox_LostFocus(object sender, RoutedEventArgs e)
        {
            OnEditedTextChange(LastEditedCell, TextBox_EditedBox.Text);
        }

        /// <summary>
        /// 清空当前文本
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_TranslateEmptyEditedText_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid_TranslatedTexts.CurrentItem is DataRowView view)
            {
                
                view.Row[Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_EditedText)] = DBNull.Value;
                view.Row[Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_TextStatus)] = EnumGameTextStatus.Empty;
            }
        }

        /// <summary>
        /// 重置当前文本
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_TranslateResetEditedText_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid_TranslatedTexts.CurrentItem is DataRowView view)
            {
                object source = view.Row[Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangSource, Data_GameText.RN_GameText_SourceText)];
                view.Row[Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_EditedText)] = source;
                view.Row[Data_GameText.GetRowNameForLanguage(EnumCurrentTranLangTarget, Data_GameText.RN_GameText_TextStatus)] = source == DBNull.Value ? EnumGameTextStatus.Empty : EnumGameTextStatus.Normal;
            }
        }

        /// <summary>
        /// 跳至指定编号事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void MenuItem_TranslateGoToDataIndex_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTextData == null) return;
            SC2_GameTranslater_GoToIndex dialog = new SC2_GameTranslater_GoToIndex(CurrentTextData.Rows.Count);
            if (!dialog.ShowDialog() == true) return;
            string id = Data_GameText.FindIDByDataIndex(CurrentTextData, dialog.GoToIndex);
            ScrollToItemByID(id);
        }

        /// <summary>
        /// 进行复制翻译文本点击事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void RibbonButton_DoCopy_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.CurrentProject == null) return;
            if (!(InRibbonGallery_CopySource.Tag is EnumLanguage sourceLanguage)) return;
            string srcLangName = Globals.GetEnumLanguageName(sourceLanguage);
            string srcLangText = Globals.GetStringFromCurrentLanguage($"TEXT_Use{srcLangName}");
            List<EnumLanguage> targetLanguages = new List<EnumLanguage>();
            string tarLangText = "";
            foreach (object select in InRibbonGallery_CopyTargets.Items)
            {
                if (select is ToggleButton button && button.IsEnabled && button.IsChecked == true && button.Tag is EnumLanguage targetLanguage)
                {
                    targetLanguages.Add(targetLanguage);
                    string tarLangName = Globals.GetEnumLanguageName(targetLanguage);
                    tarLangText += Globals.GetCommaStringFromCurrentLanguage(Globals.GetStringFromCurrentLanguage(
                        $"TEXT_{tarLangName}"));
                }
            }
            tarLangText = tarLangText.Substring(2);
            if (Log.ShowSystemMessage(true, MessageBoxButton.YesNo, MessageBoxImage.None, "MSG_DoCopyTranslateText", srcLangText, tarLangText) != MessageBoxResult.Yes) return;
            ListFilterRecordClear();
            foreach (EnumLanguage language in targetLanguages)
            {
                Globals.CurrentProject.CopyTranslateText(sourceLanguage, language);
            }
            RefreshTranslatedText();
            CurrentProjectNeedSave = true;
        }

        /// <summary>
        /// 翻译语言选择变化事件(源)
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void InRibbonGallery_TranslatedLanguageSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InRibbonGallery_TranslatedLanguageSource.SelectedItem is Fluent.Button button && button.Tag is EnumLanguage language)
            {
                EnumCurrentTranLangSource = language;
            }
            else
            {
                EnumCurrentTranLangSource = EnumLanguage.Other;
            }
        }

        /// <summary>
        /// 翻译语言选择变化事件（目标）
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void InRibbonGallery_TranslatedLanguageTarget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InRibbonGallery_TranslatedLanguageTarget.SelectedItem is Fluent.Button button && button.Tag is EnumLanguage language)
            {
                EnumCurrentTranLangTarget = language;
            }
            else
            {
                EnumCurrentTranLangTarget = EnumLanguage.Other;
            }
        }

        /// <summary>
        /// 批量修改加入测试事件
        /// </summary>
        /// <param name="sender">事件控件</param>
        /// <param name="e">响应参数</param>
        private void CheckBox_AddToPreview_Checked(object sender, RoutedEventArgs e)
        {
            bool val = CheckBox_AddToPreview.IsChecked == true;
            for (int i = 0; i < CurrentFilterResultView.Count; i++)
            {
                CurrentFilterResultView[i].Row[Data_GameText.RN_GameText_AddToPreview] = val;
            }
        }

        /// <summary>
        /// 按下帮助F1键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenTip_HelpPressed(object sender, ScreenTipHelpEventArgs e)
        {
            if (e.HelpTopic is string topic)
            {
                if (topic.StartsWith("https://"))
                {
                    System.Diagnostics.Process.Start(topic);
                }
                else
                {
                    System.Diagnostics.Process.Start("https://github.com/Whimsyduke/SC2_GameTranslater/blob/Help/" + Globals.GetEnumLanguageName(EnumCurrentLanguage) + "/" + topic);
                }
            }
        }

        /// <summary>
        /// 按下帮助按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonButton_Help_Click(object sender, RoutedEventArgs e)
        {
            SC2_GameTranslater_About window = new SC2_GameTranslater_About();
            window.ShowDialog();
        }

        #endregion

    }
}
