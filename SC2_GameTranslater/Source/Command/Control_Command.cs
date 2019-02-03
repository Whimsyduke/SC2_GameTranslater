using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

using Globals = SC2_GameTranslater.Source.Class_Globals;
using EnumLanguage = SC2_GameTranslater.Source.EnumLanguage;

namespace SC2_GameTranslater.Source.Command
{
    /// <summary>
    /// Command 通用
    /// </summary>
    public class Control_Command
    {
        #region 文件管理

        /// <summary>
        /// 打开项目命令执行函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public void Executed_OpenProject(object sender, ExecutedRoutedEventArgs e)
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
            fileDialog.Filter = Globals.CurrentLanguage["COM_SC2Components"] as string + "|ComponentList.SC2Components|" + Globals.CurrentLanguage["COM_ProjectFile"] as string + "|*.SC2GameTran";
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Title = Globals.CurrentLanguage["UI_OpenFileDialog_Open_Title"] as string;
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Globals.MainWindow.OnOpen(fileDialog.FileName);
            }

            e.Handled = true;
        }
        /// <summary>
        /// 打开项目命令判断函数
        /// </summary>
        /// <param name="sender">命令来源</param>
        /// <param name="e">路由事件参数</param>
        public void CanExecuted_OpenProject(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        #endregion

    }
}
