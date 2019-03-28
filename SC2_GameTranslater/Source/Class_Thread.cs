using System.Threading;

namespace SC2_GameTranslater.Source
{
    using Log = Class_Log;

    /// <summary>
    /// 线程功能
    /// </summary>
    public static class Class_Threads
    {
        #region 方法

        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="func">进程函数</param>
        /// <param name="argu">进程参数</param>
        /// <param name="isBack">后台运行</param>
        /// <returns>运行正常</returns>
        public static bool StartThread(ParameterizedThreadStart func, object argu, bool isBack)
        {
#if !DEBUG
            try
#endif
            {
                Thread thread = new Thread(func)
                {
                    IsBackground = isBack
                };
                thread.Start(argu);
            }
#if !DEBUG
            catch
            {
                return false;
            }
#endif
            return true;
        }

        #endregion
    }
}
