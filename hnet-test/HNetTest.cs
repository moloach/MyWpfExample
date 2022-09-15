using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace HNetTest
{
    public partial class HNetTest : Form
    {
        public static HNetTest m_Form;
        Thread thread_Update;
        bool isListen = false;
        public HNetTest()
        {
            InitializeComponent();
            m_Form = this;
        }

        private void ServerInit(int port)
        {
            int ret = NetDll.InitServer(port);
            if (ret < 0)
            {
                MessageBox.Show("初始化服务器失败");
                return;
            }
            NetDll.ServerRegist_CB();

            MsgBox.ReadOnly = true;
            MsgBox.Text = "初始化服务器成功，等待连接...\r\n";
        }


        private void ServerDestory()
        {
            NetDll.DeInitServer();
            MsgBox.Text = "服务器销毁成功！\r\n";
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            //获取发送数据
            string SendData = sendBox.Text;
            NetDll.Send(Marshal.StringToHGlobalAnsi(SendData), 6);
        }

        private void MsgBox_TextChanged(object sender, EventArgs e)
        {
            this.MsgBox.SelectionStart = this.MsgBox.Text.Length;
            this.MsgBox.SelectionLength = 0;
            this.MsgBox.ScrollToCaret();
        }

        void updata_Online()
        {
            while (true)
            {
                MsgBox.BeginInvoke((Action)(() =>
                {
                    if (NetDll.GetOnline() == 1)
                    {
                        MsgStatus.Text = $"客户端 在线 RTT: {NetDll.GetRtt()}ms";
                    }
                    else
                    {
                        MsgStatus.Text = "客户端离线";
                    }
                }));

                Thread.Sleep(500);
            }
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            if (!isListen)
            {

                int port = 60004;
                try
                {
                    port = Convert.ToInt32(portBox.Text);
                }
                catch (Exception) { }
                ServerInit(port);
                thread_Update = new Thread(updata_Online)
                {
                    IsBackground = true
                };
                thread_Update.Start();

                portBox.ReadOnly = true;
                listenButton.Text = "停止";
            }
            else
            {
                ServerDestory();
                thread_Update.Abort();
                listenButton.Text = "监听";
                portBox.ReadOnly = false;
                MsgStatus.Text = "";
            }
            isListen = !isListen;

        }
    }
}
