using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HappyClinic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //if (true)
            //{
            //    MainWindow mainWindow = new MainWindow();
            //    mainWindow.Show();
            //}
            //else
            //{
                LogInWindow login = new LogInWindow();
                login.Show();
            //}
        }
    }
}
