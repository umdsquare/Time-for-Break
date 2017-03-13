﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for exitwin.xaml
    /// </summary>
    public partial class exitwin : Window
    {
        public exitwin()
        {
            InitializeComponent();
        }

        private void stay_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            //Trace.WriteLine("exitwin: is this the final closing?");
            //foreach (Window w in Application.Current.Windows)
            //{
            //    if (w is MainWindow)
            //    {
            //        w.Close();

            //    }
            //}

            Application.Current.Shutdown();
        }
    }
}
