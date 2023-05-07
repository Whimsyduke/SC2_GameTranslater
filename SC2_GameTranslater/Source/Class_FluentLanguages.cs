using Fluent.Localization;

namespace SC2_GameTranslater.Source
{
    /// <summary>
    /// SC2本地化语言类型枚举
    /// </summary>
    [RibbonLocalization("Chinese Simplified", "zhCN")]
    public class RibbonLanguage_zhCN : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "自动";
        public override string BackstageButtonKeyTip { get; } = FallbackLocalization.BackstageButtonKeyTip;
        public override string BackstageButtonText { get; } = "文件";
        public override string CustomizeStatusBar { get; } = "自定义状态栏";
        public override string ExpandButtonScreenTipText { get; } = "始终显示功能区选项卡和命令。";
        public override string ExpandButtonScreenTipTitle { get; } = "展开功能区 (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "仅显示功能区上的选项卡名称。单击选项卡可显示命令。";
        public override string MinimizeButtonScreenTipTitle { get; } = "功能区最小化 (Ctrl + F1)";
        public override string MoreColors { get; } = "更多的颜色...";
        public override string NoColor { get; } = "没有颜色";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "自定义快速访问具栏";
        public override string QuickAccessToolBarMenuHeader { get; } = "自定义快速访问工具栏";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "在功能区上方显示";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "在功能区下方显示";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "其他命令";
        public override string RibbonContextMenuAddGallery { get; } = "在快速访问工具栏中添加样式";
        public override string RibbonContextMenuAddGroup { get; } = "在快速访问工具栏中添加组";
        public override string RibbonContextMenuAddItem { get; } = "添加到快速访问工具栏";
        public override string RibbonContextMenuAddMenu { get; } = "在快速访问工具栏中添加菜单";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "自定义快速访问工具栏...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "自定义功能区...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "功能区最小化";
        public override string RibbonContextMenuRemoveItem { get; } = "在快速访问工具栏中移除";
        public override string RibbonContextMenuShowAbove { get; } = "在功能区上方显示快速访问工具栏";
        public override string RibbonContextMenuShowBelow { get; } = "在功能区下方显示快速访问工具栏";
        public override string ScreenTipDisableReasonHeader { get; } = "此命令当前已被禁用。";
        public override string ScreenTipF1LabelHeader { get; } = "按下F1获得更多帮助。";
    }

    /// <summary>
    /// 繁体中文
    /// </summary>
    [RibbonLocalization("Chinese Traditional", "zhTW")]
    public class RibbonLanguage_zhTW : RibbonLocalizationBase
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
        public override string ScreenTipF1LabelHeader { get; } = "按下F1獲得更多幫助。";
    }

    /// <summary>
    /// 英语美国
    /// </summary>
    [RibbonLocalization("English", "enUS")]
    public class RibbonLanguage_enUS : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automatic";
        public override string BackstageButtonKeyTip { get; } = "F";
        public override string BackstageButtonText { get; } = "File";
        public override string CustomizeStatusBar { get; } = "Customize Status Bar";
        public override string ExpandButtonScreenTipText { get; } = "Like seeing the ribbon? Keep it open while you work.";
        public override string ExpandButtonScreenTipTitle { get; } = "Pin the Ribbon (Ctrl+F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Need a bit more space? Collapse the ribbon so only the tab names show.";
        public override string MinimizeButtonScreenTipTitle { get; } = "Collapse the Ribbon (Ctrl+F1)";
        public override string MoreColors { get; } = "More colors...";
        public override string NoColor { get; } = "No color";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Customize Quick Access Toolbar";
        public override string QuickAccessToolBarMenuHeader { get; } = "Customize Quick Access Toolbar";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Show Above the Ribbon";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Show Below the Ribbon";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "More controls";
        public override string RibbonContextMenuAddGallery { get; } = "Add Gallery to Quick Access Toolbar";
        public override string RibbonContextMenuAddGroup { get; } = "Add Group to Quick Access Toolbar";
        public override string RibbonContextMenuAddItem { get; } = "Add to Quick Access Toolbar";
        public override string RibbonContextMenuAddMenu { get; } = "Add Menu to Quick Access Toolbar";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Customize Quick Access Toolbar...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Customize the Ribbon...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Collapse the Ribbon";
        public override string RibbonContextMenuRemoveItem { get; } = "Remove from Quick Access Toolbar";
        public override string RibbonContextMenuShowAbove { get; } = "Show Quick Access Toolbar Above the Ribbon";
        public override string RibbonContextMenuShowBelow { get; } = "Show Quick Access Toolbar Below the Ribbon";
        public override string ScreenTipDisableReasonHeader { get; } = "This command is currently disabled.";
        public override string ScreenTipF1LabelHeader { get; } = "Press F1 for help";
    }

    /// <summary>
    /// 德语德国
    /// </summary>
    [RibbonLocalization("German", "deDE")]
    public class RibbonLanguage_deDE : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automatisch";
        public override string BackstageButtonKeyTip { get; } = "D";
        public override string BackstageButtonText { get; } = "Datei";
        public override string CustomizeStatusBar { get; } = "Statusleiste anpassen";
        public override string ExpandButtonScreenTipText { get; } = "Ist es Ihnen lieber, wenn Sie das Menüband sehen? Lassen Sie es während der Arbeit geöffnet.";
        public override string ExpandButtonScreenTipTitle { get; } = "Menüband erweitern (Strg + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Sie benötigen etwas mehr Platz? Reduzieren Sie das Menüband, sodass nur die Registerkartennamen angezeigt werden.";
        public override string MinimizeButtonScreenTipTitle { get; } = "Menüband minimieren (Strg + F1)";
        public override string MoreColors { get; } = "Weitere Farben...";
        public override string NoColor { get; } = "Keine Farbe";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Symbolleiste für den Schnellzugriff anpassen";
        public override string QuickAccessToolBarMenuHeader { get; } = "Symbolleiste für den Schnellzugriff anpassen...";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Über dem Menüband anzeigen";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Unter dem Menüband anzeigen";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Weitere Befehle";
        public override string RibbonContextMenuAddGallery { get; } = "Katalog zur Symbolleiste für den Schnellzugriff hinzufügen";
        public override string RibbonContextMenuAddGroup { get; } = "Gruppe zur Symbolleiste für den Schnellzugriff hinzufügen";
        public override string RibbonContextMenuAddItem { get; } = "Zur Symbolleiste für den Schnellzugriff hinzufügen";
        public override string RibbonContextMenuAddMenu { get; } = "Zur Symbolleiste für den Schnellzugriff hinzufügen";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Symbolleiste für den Schnellzugriff anpassen...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Menüband anpassen...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Menüband minimieren";
        public override string RibbonContextMenuRemoveItem { get; } = "Aus Symbolleiste für den Schnellzugriff entfernen";
        public override string RibbonContextMenuShowAbove { get; } = "Symbolleiste für den Schnellzugriff über dem Menüband anzeigen";
        public override string RibbonContextMenuShowBelow { get; } = "Symbolleiste für den Schnellzugriff unter dem Menüband anzeigen";
        public override string ScreenTipDisableReasonHeader { get; } = "Diese Funktion ist momentan deaktiviert.";
        public override string ScreenTipF1LabelHeader { get; } = "Drücken Sie F1 für die Hilfe";
    }

    /// <summary>
    /// 西班牙语墨西哥
    /// </summary>
    [RibbonLocalization("Spanish", "esMX")]
    public class RibbonLanguage_esMX : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automático";
        public override string BackstageButtonKeyTip { get; } = "A";
        public override string BackstageButtonText { get; } = "Archivo";
        public override string CustomizeStatusBar { get; } = "Personalizar barra de estado";
        public override string ExpandButtonScreenTipText { get; } = "Muestra u oculta la cinta\n\nCuando la cinta está oculta, sólo se muestran los nombres de las pestañas";
        public override string ExpandButtonScreenTipTitle { get; } = "Expandir la cinta (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Muestra u oculta la cinta\n\nCuando la cinta está oculta, sólo se muestran los nombres de las pestañas";
        public override string MinimizeButtonScreenTipTitle { get; } = "Minimizar la cinta (Ctrl + F1)";
        public override string MoreColors { get; } = "Más colores...";
        public override string NoColor { get; } = "No hay color";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Personalizar barra de herramientas de acceso rápido";
        public override string QuickAccessToolBarMenuHeader { get; } = "Personalizar barra de herramientas de acceso rápido";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Mostrar sobre la cinta";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Mostrar bajo la cinta";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Más controles";
        public override string RibbonContextMenuAddGallery { get; } = "Agregar galería a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuAddGroup { get; } = "Agregar grupo a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuAddItem { get; } = "Agregar a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuAddMenu { get; } = "Agregar menú a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Personalizar la barra de herramientas de acceso rápido...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Personalizar la cinta...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Minimizar la cinta";
        public override string RibbonContextMenuRemoveItem { get; } = "Quitar de la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuShowAbove { get; } = "Mostrar barra de herramientas de acceso rápido sobre la cinta";
        public override string RibbonContextMenuShowBelow { get; } = "Mostrar barra de herramientas de acceso rápido bajo la cinta";
        public override string ScreenTipDisableReasonHeader { get; } = "Este comando está desactivado actualmente";
        public override string ScreenTipF1LabelHeader { get; } = "Pulse F1 para obtener más ayuda";
    }

    /// <summary>
    /// 西班牙语西班牙
    /// </summary>
    [RibbonLocalization("Spanish", "esES")]
    public class RibbonLanguage_esES : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automático";
        public override string BackstageButtonKeyTip { get; } = "A";
        public override string BackstageButtonText { get; } = "Archivo";
        public override string CustomizeStatusBar { get; } = "Personalizar barra de estado";
        public override string ExpandButtonScreenTipText { get; } = "Muestra u oculta la cinta\n\nCuando la cinta está oculta, sólo se muestran los nombres de las pestañas";
        public override string ExpandButtonScreenTipTitle { get; } = "Expandir la cinta (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Muestra u oculta la cinta\n\nCuando la cinta está oculta, sólo se muestran los nombres de las pestañas";
        public override string MinimizeButtonScreenTipTitle { get; } = "Minimizar la cinta (Ctrl + F1)";
        public override string MoreColors { get; } = "Más colores...";
        public override string NoColor { get; } = "No hay color";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Personalizar barra de herramientas de acceso rápido";
        public override string QuickAccessToolBarMenuHeader { get; } = "Personalizar barra de herramientas de acceso rápido";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Mostrar sobre la cinta";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Mostrar bajo la cinta";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Más controles";
        public override string RibbonContextMenuAddGallery { get; } = "Agregar galería a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuAddGroup { get; } = "Agregar grupo a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuAddItem { get; } = "Agregar a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuAddMenu { get; } = "Agregar menú a la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Personalizar la barra de herramientas de acceso rápido...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Personalizar la cinta...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Minimizar la cinta";
        public override string RibbonContextMenuRemoveItem { get; } = "Quitar de la barra de herramientas de acceso rápido";
        public override string RibbonContextMenuShowAbove { get; } = "Mostrar barra de herramientas de acceso rápido sobre la cinta";
        public override string RibbonContextMenuShowBelow { get; } = "Mostrar barra de herramientas de acceso rápido bajo la cinta";
        public override string ScreenTipDisableReasonHeader { get; } = "Este comando está desactivado actualmente";
        public override string ScreenTipF1LabelHeader { get; } = "Pulse F1 para obtener más ayuda";
    }

    /// <summary>
    /// 法语法国
    /// </summary>
    [RibbonLocalization("French", "frFR")]
    public class RibbonLanguage_frFR : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automatique";
        public override string BackstageButtonKeyTip { get; } = FallbackLocalization.BackstageButtonKeyTip;
        public override string BackstageButtonText { get; } = "Fichier";
        public override string CustomizeStatusBar { get; } = "Personnaliser la barre de statut";
        public override string ExpandButtonScreenTipText { get; } = "Afficher ou masquer le Ruban \n\nQuand le Ruban est masqué, seul les noms sont affichés";
        public override string ExpandButtonScreenTipTitle { get; } = "Agrandir le Ruban (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Afficher ou masquer le Ruban \n\nQuand le Ruban est masqué, seul les noms sont affichés";
        public override string MinimizeButtonScreenTipTitle { get; } = "Minimiser le Ruban (Ctrl + F1)";
        public override string MoreColors { get; } = "Plus de couleurs...";
        public override string NoColor { get; } = "Pas de couleur";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Personnaliser la barre d'outils Accès Rapide";
        public override string QuickAccessToolBarMenuHeader { get; } = "Personnaliser la barre d'outil Accès Rapide";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Afficher au dessus du Ruban";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Afficher en dessous du Ruban";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Plus de contrôles";
        public override string RibbonContextMenuAddGallery { get; } = "Ajouter une galerie à la barre d'outils Accès Rapide";
        public override string RibbonContextMenuAddGroup { get; } = "Ajouter un groupe à la barre d'outils Accès Rapide";
        public override string RibbonContextMenuAddItem { get; } = "Ajouter un élément à la barre d'outils Accès Rapide";
        public override string RibbonContextMenuAddMenu { get; } = "Ajouter un menu à la barre d'outils Accès Rapide";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Personnaliser la barre d'outils Accès Rapide...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Personnaliser le Ruban...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Minimiser le Ruban";
        public override string RibbonContextMenuRemoveItem { get; } = "Supprimer de la barre d'outils Accès Rapide";
        public override string RibbonContextMenuShowAbove { get; } = "Afficher la barre d'outils Accès Rapide au dessus du Ruban";
        public override string RibbonContextMenuShowBelow { get; } = "Afficher la barre d'outils Accès Rapide en dessous du Ruban";
        public override string ScreenTipDisableReasonHeader { get; } = "Cette commande est actuellement désactivée.";
        public override string ScreenTipF1LabelHeader { get; } = FallbackLocalization.ScreenTipF1LabelHeader;
    }

    /// <summary>
    /// 意大利语意大利
    /// </summary>
    [RibbonLocalization("Italian", "itIT")]
    public class RibbonLanguage_itIT : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automatico";
        public override string BackstageButtonKeyTip { get; } = FallbackLocalization.BackstageButtonKeyTip;
        public override string BackstageButtonText { get; } = FallbackLocalization.BackstageButtonText;
        public override string CustomizeStatusBar { get; } = FallbackLocalization.CustomizeStatusBar;
        public override string ExpandButtonScreenTipText { get; } = "Visualizza la barra multifunzione in modo che rimanga sempre espansa, anche se l’utente ha fatto click su un comando.";
        public override string ExpandButtonScreenTipTitle { get; } = "Espandi la barra multifunzione (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Consente di visualizzare solo i nomi delle schede nella barra multifunzione.";
        public override string MinimizeButtonScreenTipTitle { get; } = "Riduci a icona barra multifunzione (Ctrl + F1)";
        public override string MoreColors { get; } = "Più colori...";
        public override string NoColor { get; } = "Nessun colore";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Personalizza barra di accesso rapido";
        public override string QuickAccessToolBarMenuHeader { get; } = "Personalizza barra di accesso rapido";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Mostra sopra la barra multifunzione";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Mostra sotto la barra multifunzione";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Altri comandi…";
        public override string RibbonContextMenuAddGallery { get; } = "Aggiungi raccolta alla barra di accesso rapido";
        public override string RibbonContextMenuAddGroup { get; } = "Aggiungi gruppo alla barra di accesso rapido";
        public override string RibbonContextMenuAddItem { get; } = "Aggiungi alla barra di accesso rapido";
        public override string RibbonContextMenuAddMenu { get; } = "Aggiungi menu alla barra di accesso rapido";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Personalizza barra di accesso rapido...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Personalizza barra multifunzione...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Riduci a icona barra multifunzione";
        public override string RibbonContextMenuRemoveItem { get; } = "Rimuovi dalla barra di accesso rapido";
        public override string RibbonContextMenuShowAbove { get; } = "Mostra la barra di accesso rapido sopra la barra multifunzione";
        public override string RibbonContextMenuShowBelow { get; } = "Mostra la barra di accesso rapido sotto la barra multifunzione";
        public override string ScreenTipDisableReasonHeader { get; } = "Questo commando è disattivato.";
        public override string ScreenTipF1LabelHeader { get; } = FallbackLocalization.ScreenTipF1LabelHeader;
    }

    /// <summary>
    /// 波兰语波兰
    /// </summary>
    [RibbonLocalization("Polish", "plPL")]
    public class RibbonLanguage_plPL : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automatyczne";
        public override string BackstageButtonKeyTip { get; } = "P";
        public override string BackstageButtonText { get; } = "Plik";
        public override string CustomizeStatusBar { get; } = FallbackLocalization.CustomizeStatusBar;
        public override string ExpandButtonScreenTipText { get; } = "Pokazuje lub ukrywa Wstążkę\n\nGdy Wstążka jest ukryta, tylko nazwy zakładek są widoczne";
        public override string ExpandButtonScreenTipTitle { get; } = "Rozwiń Wstążkę (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Pokazuje lub ukrywa Wstążkę\n\nGdy Wstążka jest ukryta, tylko nazwy zakładek są widoczne";
        public override string MinimizeButtonScreenTipTitle { get; } = "Minimalizuj Wstążkę (Ctrl + F1)";
        public override string MoreColors { get; } = "Więcej kolorów...";
        public override string NoColor { get; } = "Brak koloru";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Dostosuj pasek narzędzi Szybki dostęp";
        public override string QuickAccessToolBarMenuHeader { get; } = "Dostosuj pasek narzędzi Szybki dostęp";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Pokaż powyżej Wstążki";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Pokaż poniżej Wstążki";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Więcej poleceń...";
        public override string RibbonContextMenuAddGallery { get; } = "Dodaj Galerię do paska narzędzi Szybki dostęp";
        public override string RibbonContextMenuAddGroup { get; } = "Dodaj Grupę do paska narzędzi Szybki dostęp";
        public override string RibbonContextMenuAddItem { get; } = "Dodaj do paska narzędzi Szybki dostęp";
        public override string RibbonContextMenuAddMenu { get; } = "Dodaj do paska narzędzi Szybki dostęp";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Dostosuj pasek narzędzi Szybki dostęp...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Dostosuj Wstążkę...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Minimalizuj Wstążkę";
        public override string RibbonContextMenuRemoveItem { get; } = "Usuń z paska narzędzi Szybki dostęp";
        public override string RibbonContextMenuShowAbove { get; } = "Pokaż pasek Szybki dostęp powyżej Wstążki";
        public override string RibbonContextMenuShowBelow { get; } = "Pokaż pasek Szybki dostęp poniżej Wstążki";
        public override string ScreenTipDisableReasonHeader { get; } = FallbackLocalization.ScreenTipDisableReasonHeader;
        public override string ScreenTipF1LabelHeader { get; } = FallbackLocalization.ScreenTipF1LabelHeader;
    }

    /// <summary>
    /// 葡萄牙语巴西
    /// </summary>
    [RibbonLocalization("Portuguese (Brazil)", "ptBR")]
    public class RibbonLanguage_ptBR : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Automático";
        public override string BackstageButtonKeyTip { get; } = "A";
        public override string BackstageButtonText { get; } = "Arquivo";
        public override string CustomizeStatusBar { get; } = FallbackLocalization.CustomizeStatusBar;
        public override string ExpandButtonScreenTipText { get; } = "Mostrar ou esconder o  Ribbon\n\nQuando o Ribbon estiver escondido, somente o nome das abas serão mostrados";
        public override string ExpandButtonScreenTipTitle { get; } = "Expandir o Ribbon (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Mostrar ou esconder o  Ribbon\n\nQuando o Ribbon estiver escondido, somente o nome das abas serão mostrados";
        public override string MinimizeButtonScreenTipTitle { get; } = "Minimizar o Ribbon (Ctrl + F1)";
        public override string MoreColors { get; } = "Mais cores...";
        public override string NoColor { get; } = "Nenhuma cor";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Customizar Barra de acesso rápido";
        public override string QuickAccessToolBarMenuHeader { get; } = " Customizar Barra de acesso rápido";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Mostrar acima do Ribbon";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Mostrar abaixo do Ribbon";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Mais controles";
        public override string RibbonContextMenuAddGallery { get; } = "Adicionar a galeria para Barra de acesso rápido";
        public override string RibbonContextMenuAddGroup { get; } = " Adicionar o grupo para Barra de acesso rápido";
        public override string RibbonContextMenuAddItem { get; } = "Adicionar para Barra de acesso rápido";
        public override string RibbonContextMenuAddMenu { get; } = " Adicionar o menu para Barra de acesso rápido";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Customizar Barra de acesso rápido...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Customizar o Ribbon...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Minimizar o Ribbon";
        public override string RibbonContextMenuRemoveItem { get; } = "Remover da Barra de acesso rápido";
        public override string RibbonContextMenuShowAbove { get; } = "Mostrar Barra de acesso rápido acima do Ribbon";
        public override string RibbonContextMenuShowBelow { get; } = "Mostrar Barra de acesso rápido abaixo do Ribbon";
        public override string ScreenTipDisableReasonHeader { get; } = "Este comando está desativado.";
        public override string ScreenTipF1LabelHeader { get; } = FallbackLocalization.ScreenTipF1LabelHeader;
    }

    /// <summary>
    /// 俄语俄罗斯
    /// </summary>
    [RibbonLocalization("Russian", "ruRU")]
    public class RibbonLanguage_ruRU : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "Автоматически";
        public override string BackstageButtonKeyTip { get; } = "Ф";
        public override string BackstageButtonText { get; } = "Файл";
        public override string CustomizeStatusBar { get; } = "Настройка строки состояния";
        public override string ExpandButtonScreenTipText { get; } = "Отображение или скрытие ленты\n\nКогда лента скрыта, отображаются только имена вкладок.";
        public override string ExpandButtonScreenTipTitle { get; } = "Развернуть ленту (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "Отображение или скрытие ленты\n\nКогда лента скрыта, отображаются только имена вкладок.";
        public override string MinimizeButtonScreenTipTitle { get; } = "Свернуть ленту (Ctrl + F1)";
        public override string MoreColors { get; } = "Больше цветов...";
        public override string NoColor { get; } = "Без цвета";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "Настройка панели быстрого доступа";
        public override string QuickAccessToolBarMenuHeader { get; } = "Настройка панели быстрого доступа";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "Разместить над лентой";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "Разместить под лентой";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "Другие элементы";
        public override string RibbonContextMenuAddGallery { get; } = "Добавить коллекцию на панель быстрого доступа";
        public override string RibbonContextMenuAddGroup { get; } = "Добавить группу на панель быстрого доступа";
        public override string RibbonContextMenuAddItem { get; } = "Добавить на панель быстрого доступа";
        public override string RibbonContextMenuAddMenu { get; } = "Добавить меню на панель быстрого доступа";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "Настройка панели быстрого доступа...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "Настройка ленты...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "Свернуть ленту";
        public override string RibbonContextMenuRemoveItem { get; } = "Удалить с панели быстрого доступа";
        public override string RibbonContextMenuShowAbove { get; } = "Разместить панель быстрого доступа над лентой";
        public override string RibbonContextMenuShowBelow { get; } = "Разместить панель быстрого доступа под лентой";
        public override string ScreenTipDisableReasonHeader { get; } = "В настоящее время эта команда отключена.";
        public override string ScreenTipF1LabelHeader { get; } = "Нажмите F1 для получения справки";
    }

    /// <summary>
    /// 韩语韩国
    /// </summary>
    [RibbonLocalization("Korean", "koKR")]
    public class RibbonLanguage_koKR : RibbonLocalizationBase
    {
        public override string Automatic { get; } = "자동";
        public override string BackstageButtonKeyTip { get; } = FallbackLocalization.BackstageButtonKeyTip;
        public override string BackstageButtonText { get; } = "파일";
        public override string CustomizeStatusBar { get; } = "상태 표시줄 사용자 지정";
        public override string ExpandButtonScreenTipText { get; } = "리본 메뉴를 표시하거나 숨깁니다\n\n리본 메뉴가 숨김 상태일때만,\n탭이름이 보여집니다";
        public override string ExpandButtonScreenTipTitle { get; } = "리본 메뉴를 표시합니다 (Ctrl + F1)";
        public override string MinimizeButtonScreenTipText { get; } = "리본 메뉴를 표시하거나 숨깁니다\n\n리본 메뉴가 숨김 상태일때만,\n탭이름이 보여집니다";
        public override string MinimizeButtonScreenTipTitle { get; } = "리본 메뉴를 최소화 합니다 (Ctrl + F1)";
        public override string MoreColors { get; } = "더 많은 색상...";
        public override string NoColor { get; } = "색 없음";
        public override string QuickAccessToolBarDropDownButtonTooltip { get; } = "빠른 실행 도구 모음 사용자 지정";
        public override string QuickAccessToolBarMenuHeader { get; } = "빠른 실행 도구 모음 사용자 지정";
        public override string QuickAccessToolBarMenuShowAbove { get; } = "리본 메뉴 위에 표시";
        public override string QuickAccessToolBarMenuShowBelow { get; } = "리본 메뉴 아래에 표시";
        public override string QuickAccessToolBarMoreControlsButtonTooltip { get; } = "기타 컨트롤들";
        public override string RibbonContextMenuAddGallery { get; } = "갤러리를 빠른 실행 도구 모음에 추가";
        public override string RibbonContextMenuAddGroup { get; } = "그룹을 빠른 실행 도구 모음에 추가";
        public override string RibbonContextMenuAddItem { get; } = "빠른 실행 도구 모음에 추가";
        public override string RibbonContextMenuAddMenu { get; } = "메뉴를 빠른 실행 도구 모음에 추가";
        public override string RibbonContextMenuCustomizeQuickAccessToolBar { get; } = "빠른 실행 도구 모음 사용자 지정...";
        public override string RibbonContextMenuCustomizeRibbon { get; } = "리본 메뉴 사용자 지정...";
        public override string RibbonContextMenuMinimizeRibbon { get; } = "리본 메뉴 최소화";
        public override string RibbonContextMenuRemoveItem { get; } = "빠른 실행 도구 모음에서 단추 제거";
        public override string RibbonContextMenuShowAbove { get; } = "리본 메뉴 위에 빠른 실행 도구 모음 표시";
        public override string RibbonContextMenuShowBelow { get; } = "리본 메뉴 아래에 빠른 실행 도구 모음 표시";
        public override string ScreenTipDisableReasonHeader { get; } = "이 명령은 현재 사용할 수 없습니다.";
        public override string ScreenTipF1LabelHeader { get; } = FallbackLocalization.ScreenTipF1LabelHeader;
    }
}
