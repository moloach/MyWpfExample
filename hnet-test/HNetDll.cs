using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HNetTest
{
    class Receiver
    {
        public static int Data_CallBack(IntPtr pData, int DataLen, IntPtr pUser)
        {
            if (DataLen > 0)
            {
                string result = Marshal.PtrToStringAnsi(pData);

                HNetTest.m_Form.MsgBox.BeginInvoke((Action)(() =>
                {
                    HNetTest.m_Form.MsgBox.Text += "recv:[";
                    HNetTest.m_Form.MsgBox.Text += result;
                    HNetTest.m_Form.MsgBox.Text += "]\r\n";
                }));
            }

            return 0;
        }
    }

    class NetDll
    {
        //初始化服务器 端口
        [DllImport("Hudp.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitServer(int Port);

        //释放服务器
        [DllImport("Hudp.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DeInitServer();

        //设备绑定通道解码显示
        [DllImport("Hudp.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Send(IntPtr pData, int Len);

        //获取在线状态 0：离线 1：在线
        [DllImport("Hudp.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetOnline();

        //获取RTT延时时间 单位毫秒
        [DllImport("Hudp.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRtt();

        //根据DLL中的回调函数的原型声明一个委托类型
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DataCallback(IntPtr data, int dataLen, IntPtr pUser);

        //实例化这个委托。
        //CH 0
        static DataCallback data_cb = new DataCallback(Receiver.Data_CallBack);


        //取客户端消息回调函数设置
        [DllImport("Hudp.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static int Regist_CB(DataCallback data_cb);

        public static void ServerRegist_CB()
        {
            //执行DLL的函数，传入C#回调函数的指针。
            Regist_CB(data_cb);
        }
    }

}


