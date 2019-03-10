using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
        /// 显示消息
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <param name="button">显示按钮</param>
        /// <param name="image">显示图片</param>
        /// <param name="msgKey">消息文本密钥</param>
        /// <param name="args">参数</param>
        /// <returns>按钮结果</returns>
        public static MessageBoxResult ShowSystemMessage(bool isShow, MessageBoxButton button, MessageBoxImage image, string msgKey, params object[] args)
        {
            string caption = Globals.CurrentLanguage[msgKey + "_Title"] as string;
            string msg = Globals.CurrentLanguage[msgKey + "_Message"] as string;
            msg = string.Format(msg, args);
            DisplayLogOnUI(msg);
            return MessageBox.Show(msg, caption, button, image);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="msg">基本消息</param>
        /// <returns>完整消息</returns>
        public static string ExceptiionMsg(string msg)
        {
            string info = msg;
            StackTrace st = new StackTrace(true);
            //得到当前的所以堆栈  
            StackFrame[] sf = st.GetFrames();
            string format = Globals.CurrentLanguage["ERR_CommonNewException"] as string;
            for (int i = 1; i < sf.Length; ++i)
            {
                info = string.Format(format, info, sf[i].GetFileName(), sf[i].GetMethod().DeclaringType.FullName, sf[i].GetMethod().Name, sf[i].GetFileLineNumber());
            }
            return info;
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="msg">消息</param>
        public static Exception NewException(string msg)
        {
            return new Exception(ExceptiionMsg(msg));
        }

        /// <summary>
        /// 测试代码
        /// </summary>
        /// <param name="val">测试表达式</param>
        public static void Assert(bool val)
        {
            if (val) return;

#if DEBUG
            throw NewException("Assert!");
#else
            //ShowSystemMessage(ExceptiionMsg("Assert!"));
#endif
        }

        #endregion
    }
}
