using System;
using System.Threading;

namespace ShutdownTimer
{
    /// <summary>
    /// 
    /// </summary>
    public static class SingleInstance
    {
        public static readonly int sr_wm_showfirstinstance =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        private static Mutex s_mutex;

        public static bool Start()
        {
            bool only_instance;
            string mutex_name = string.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            s_mutex = new Mutex(true, mutex_name, out only_instance);
            return only_instance;
        }
        public static void ShowFirstInstance()
        {
            WinApi.PostMessage(
                (IntPtr)WinApi.hwnd_broadcast,
                sr_wm_showfirstinstance,
                IntPtr.Zero,
                IntPtr.Zero);
        }
        public static void Stop()
        {
            s_mutex.ReleaseMutex();
        }
    }
}
