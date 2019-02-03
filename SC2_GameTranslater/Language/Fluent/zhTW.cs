#pragma warning disable // Localization

namespace Fluent.Localization.Languages
{
    [RibbonLocalization("Chinese Traditional", "zhTW")]
    public class Chinese : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "自動";
        public override string BackstageButtonKeyTip { get; } = FallbackLocalization.BackstageButtonKeyTip;
        public override string BackstageButtonText { get; } = "文件";
        public override string CustomizeStatusBar { get; } = "自訂狀態列";
        public override string ExpandButtonScreenTipText { get; } = "始終顯示功能區選項卡和命令。";
        public override string ExpandButtonScreenTipTitle { get; } = "展開功能區 (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "僅顯示功能區上的選項卡名稱。按一下選項卡可顯示命令。";
        public override string MinimizeButtonScreenTipTitle { get; } = "功能區最小化 (Ctrl + F1)";
        public override string MoreColors { get; } = "更多的顏色...";
        public override string NoColor { get; } = "沒有顏色";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "自訂快速訪問具欄";
        public override string QuickAccessToolBarMenuHeader { get; } = "自訂快速訪問工具列";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "在功能區上方顯示";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "在功能區下方顯示";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "其他命令";
        public override string RibbonContextMenuAddGallery { get; } = "在快速訪問工具列中添加樣式";
        public override string RibbonContextMenuAddGroup { get; } = "在快速訪問工具列中添加組";
        public override string RibbonContextMenuAddItem { get; } = "添加到快速訪問工具列";
        public override string RibbonContextMenuAddMenu { get; } = "在快速訪問工具列中添加菜單";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "自訂快速訪問工具列...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "自訂功能區...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "功能區最小化";
        public override string RibbonContextMenuRemoveItem { get; } = "在快速訪問工具列中移除";
        public override string RibbonContextMenuShowAbove { get; } = "在功能區上方顯示快速訪問工具列";
        public override string RibbonContextMenuShowBelow { get; } = "在功能區下方顯示快速訪問工具列";
        public override string ScreenTipDisableReasonHeader { get; } = "此命令當前已被禁用。";
        public override string ScreenTipF1LabelHeader { get; } = FallbackLocalization.ScreenTipF1LabelHeader;
    }
}
