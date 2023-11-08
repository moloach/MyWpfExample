using System;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace WpfApp1
{
    /// <summary>
    /// IPCExamples.xaml 的交互逻辑
    /// </summary>
    public partial class IPCExamples : Window
    {
        public IPCExamples()
        {
            InitializeComponent();
        }


        #region 使用共享内存
        //以下函数需要在两个进程中使用

        private void MmapRecvData()
        {
            var data = new byte[1920 * 1080 * 3 / 2];

            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("testmap", 1920 * 1080 * 3 / 2))
            {
                bool mutexCreated;
                Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(data);
                }
                mutex.ReleaseMutex();

                while (true)
                {

                    Console.WriteLine(DateTime.Now + " looping...");
                    mutex.WaitOne();
                    var sw = new Stopwatch();
                    sw.Start();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        var results = reader.ReadBytes(1920 * 1080 * 3 / 2);
                        Console.Write(results[0]);
                        Console.Write(" read results length:" + results.Length);
                    }
                    mutex.ReleaseMutex();
                    sw.Stop();
                    var readTime = sw.ElapsedMilliseconds;
                    Console.WriteLine(", read Time:" + readTime);

                    Thread.Sleep(500);
                }


            }
        }

        private void MMAPSendData()
        {
            var data = new byte[1920 * 1080 * 3 / 2];

            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("testmap", 1920 * 1080 * 3 / 2))
            {
                bool mutexCreated;
                Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(data);
                }
                mutex.ReleaseMutex();

                while (true)
                {

                    Console.WriteLine(DateTime.Now + " looping...");
                    mutex.WaitOne();
                    var sw = new Stopwatch();
                    sw.Start();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        var results = reader.ReadBytes(1920 * 1080 * 3 / 2);
                        Console.Write(results[0]);
                        Console.Write(" read results length:" + results.Length);
                    }
                    mutex.ReleaseMutex();
                    sw.Stop();
                    var readTime = sw.ElapsedMilliseconds;
                    Console.WriteLine(", read Time:" + readTime);

                    Thread.Sleep(500);
                }
            }
        }

        #endregion


        #region 使用函数最大大或者最小化
        private static void RemoteMonitorHide()
        {
            IntPtr handle = FindWindow(null, "RemoteMonitor");
            PostMessage(handle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
        }

        private static void RemoteMonitorShow()
        {
            IntPtr handle = FindWindow(null, "RemoteMonitor");

            if (handle == IntPtr.Zero)
            {
                Process mProcess = new Process();
                mProcess.StartInfo.FileName = string.Format(@"{0}\\{1}", Environment.CurrentDirectory, @"\RemoteMonitor.exe");
                mProcess.Start();

                System.Threading.Thread.Sleep(3000);
            }

            SetForegroundWindow(handle);
            ShowWindow(handle, SW_SHOWNORMAL);
            PostMessage(handle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
        }
        #endregion

        #region 找到窗口，发送字符串


        //此种方式找到对应窗口的标题 使用 SendMessage/PostMessage 发送给另外一个程序
        //适用于字符串
        private void SendMessageToTalkBack(string message)
        {
            try
            {
                SendMessage("talkback", message);
            }
            catch (Exception e)
            {
                //App.systemLog.Error("语音模块，发送出错，错误为：" + e.ToString());
            }
        }

        IntPtr handle = IntPtr.Zero;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
            {
                handle = hwndSource.Handle;

                //hook原来的内容
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
            }
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WM_COPYDATA:
                    CopyDataStruct cds = (CopyDataStruct)Marshal.PtrToStructure(lParam, typeof(CopyDataStruct));
                    //处理获取的数据串
                    //recv.Text = cds.lpData;
                    //Analysis(cds.lpData);
                    break;
                default:
                    break;
            }

            //发送给其他的程序
            SendMessage("RemoteDrive", "hello world");

            return hwnd;

        }





        #endregion

        #region WIN32 API

        //最小化相关变量
        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const uint WM_SYSCOMMAND2 = 0x0112;
        private const uint SC_MAXIMIZE2 = 0xF030;

        private const int SW_SHOWNORMAL = 1;

        private const int WM_COPYDATA = 0x004A;


        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);


        [DllImport("User32.dll ", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 发送消息给其他程序
        /// </summary>
        /// <param name="hWnd">目标窗体句柄</param>
        /// <param name="Msg">WM_COPYDATA（固定值）</param>
        /// <param name="wParam">自定义数值</param>
        /// <param name="lParam">结构体</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);


        [DllImport("user32")]
        public static extern bool ChangeWindowMessageFilter(uint msg, int flags);


        [StructLayout(LayoutKind.Sequential)]
        public struct CopyDataStruct
        {
            public IntPtr dwData;

            public int cbData;//字符串长度

            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;//字符串
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="windowName">window的title，建议加上GUID，不会重复</param>
        /// <param name="strMsg">要发送的字符串</param>
        public static void SendMessage(string windowName, string strMsg)
        {
            if (strMsg == null) return;
            IntPtr hwnd = FindWindow(null, windowName);
            if (hwnd != IntPtr.Zero)
            {
                CopyDataStruct cds;
                cds.dwData = IntPtr.Zero;
                cds.lpData = strMsg;
                //注意：长度为字节数
                cds.cbData = Encoding.Default.GetBytes(strMsg).Length + 1;
                // 消息来源窗体
                int fromWindowHandler = 0;
                SendMessage(hwnd, WM_COPYDATA, fromWindowHandler, ref cds);
            }
        }





        #endregion

    }
}
