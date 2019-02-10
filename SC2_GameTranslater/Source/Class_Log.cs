using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Globals = SC2_GameTranslater.Source.Class_Globals;

namespace SC2_GameTranslater.Source
{
    /// <summary>
    /// 全局日志支持
    /// </summary>
    public static class Class_Log
    {
        #region 方法

        /// <summary>
        /// UI上显示日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void DisplayLogOnUI(string msg)
        {
            if (!Globals.EnableShowLogInUI) return;
            if (Globals.MainWindow.AvalonTextEditor_Log.Text == "")
            {
                Globals.MainWindow.AvalonTextEditor_Log.Text = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss ") + (msg.Contains("\n") ? "\r\n" : "") + msg;
            }
            else
            {
                Globals.MainWindow.AvalonTextEditor_Log.Text = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss ") + (msg.Contains("\n") ? "\r\n" : "") + msg + "\r\n" + Globals.MainWindow.AvalonTextEditor_Log.Text;
            }
        }

        /// <summary>
        /// 测试代码
        /// </summary>
        /// <param name="val">测试表达式</param>
        public static void Assert(bool val)
        {
            if (val) return;
#if !DEBUG
            try
#endif
            {
                throw new Exception();
            }
#if !DEBUG
            catch (Exception err)
            {
                DisplayLogOnUI(err.Message);
            }
#endif
        }

#endregion
    }
}
