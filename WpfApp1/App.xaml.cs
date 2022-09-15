using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static bool IsStart = Convert.ToBoolean(ConfigurationManager.AppSettings["IsStart"]);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Mutex mutex = new Mutex(true, "WpfApp1", out bool isNewInstance);
            if (isNewInstance != true)
            {
                //MessageBox.Show("!");
                new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText("程序已启动")
                    .Show();
                Shutdown();
            }



        }

    }
}
