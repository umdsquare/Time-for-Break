using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Window
    {
        public Setup()
        {
            InitializeComponent();
            CloseMainWindowNow();
        }

        public void CloseMainWindowNow()
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);
          
            if (mainWindow != null)
            {
                mainWindow.Close();
            }
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            signup sign = new signup();
            sign.Show();
            this.Close();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            loginwindow login = new loginwindow();
            login.Show();
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            foreach (Window w in Application.Current.Windows)
            {
                Trace.WriteLine("W..= " + w);
                if (w is MainWindow)
                {
                    w.Close();
                    Trace.WriteLine("Mainwindow close..");
                }
            }
        }
    }
}
