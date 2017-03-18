using System;
using System.Configuration;
using System.Windows;
using System.Diagnostics;
using System.IO;
using Microsoft.Shell;
using System.Collections.Generic;
using CustomizedClickOnce.Common;
using System.Reflection;
using Microsoft.Win32;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application, ISingleInstanceApp
    {
        // RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        //RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        // TODO: Make this unique!
        private const string Unique = "Time for Break";

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        #region ISingleInstanceApp Members
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // Bring window to foreground
            //if (this.MainWindow.WindowState == WindowState.Minimized)
            //{
            //    this.MainWindow.WindowState = WindowState.Normal;
            //}

            this.MainWindow.Activate();
            // Handle command line arguments of second instance
            return true;
        }
        #endregion

        public App()
        {
            //addIconSource();
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            //appShortcut("TimeforBreak");

            //ReadAllSettings();
            // AddToStartup();

            if (GetSettingString("userName") != "" && GetSettingString("password") != "")
            {
                Trace.WriteLine("get username: " + GetSettingString("userName"));
                Trace.WriteLine("get password: " + GetSettingString("password"));

                user.username = GetSettingString("userName");
                user.online = true;
            }
            else
            {
                Setup setup = new Setup();
                setup.Show();
            }
        }

        private void AutoTrayicon()
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explore", true);
                //Assembly curAssembly = Assembly.GetExecutingAssembly();
                if (key != null)  //must check for null key
                {
                    key.SetValue("EnableAutoTray", "0", RegistryValueKind.String);
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("2. " + e.Message);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //AutoTrayicon();

            try
            {
                var clickOnceHelper = new ClickOnceHelper(Globals.PublisherName, Globals.ProductName);
                clickOnceHelper.UpdateUninstallParameters();
                clickOnceHelper.AddShortcutToStartup();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("catch exception on start up"+ ex);
            }


            base.OnStartup(e);
        }

        private void appShortcut(string linkName)
        {
           
            var startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            if (!File.Exists((startup + @"\" + "Time For Break")))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(startup + "\\" + linkName + ".url"))
                    {
                        string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        writer.WriteLine("[InternetShortcut]");
                        writer.WriteLine("URL=file:///" + app);
                        writer.WriteLine("IconIndex=0");
                        string icon = app.Replace('\\', '/');
                        writer.WriteLine("IconFile=" + icon);
                        writer.Flush();
                    }
                }
                catch (Exception e1)
                {
                    Trace.WriteLine(e1.ToString());
                }
            }       
        }

        private void addIconSource()
        {

            var Cwinsystem = "C:\\WINDOWS\\system32";
            //string BaseDir = Assembly.GetEntryAssembly().Location;
            string BaseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            Trace.WriteLine("BaseDir = " + BaseDir);

            if (!File.Exists((Cwinsystem + @"\" + "logo.ico")))
            {
                try
                {
                    File.Copy(BaseDir + "logo.ico", Cwinsystem + @"\" + "logo.ico");
                }
                catch (Exception e1)
                {
                    Trace.WriteLine(e1.ToString());
                }
            }
        }

        //static void ReadAllSettings()
        //{
        //    try
        //    {
        //        var appSettings = ConfigurationManager.AppSettings;

        //        if (appSettings.Count == 0)
        //        {
        //            Trace.WriteLine("AppSettings is empty.");
        //        }
        //        else
        //        {
        //            foreach (var key in appSettings.AllKeys)
        //            {
        //                Trace.WriteLine("Key: {0} Value: {1}" + key + appSettings[key]);
        //            }
        //        }
        //    }
        //    catch (ConfigurationErrorsException)
        //    {
        //        Trace.WriteLine("Error reading app settings");
        //    }
        //}

        //public static void AddToStartup()
        //{
        //    var startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        //    Trace.WriteLine("startup = " + startup);
        //    string BaseDir = Assembly.GetEntryAssembly().Location;
        //    Trace.WriteLine("Basedir = " + BaseDir);

        //    if (!File.Exists((startup + @"\" + "Time For Break.exe")))
        //    {
        //        try
        //        {
        //            File.Copy(BaseDir, startup + @"\" + "Time For Break.exe");
        //        }
        //        catch (Exception e1)
        //        {
        //            Trace.WriteLine(e1.ToString());
        //        }
        //    }
        //}



        public static string GetSettingString(string settingName)
        {
            try
            {
                string settingString = ConfigurationManager.AppSettings[settingName].ToString();
                Trace.WriteLine("Settingstring= " + settingString);
                return settingString;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception" + e.Message);
                return null;
            }
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // string errorMessage = string.Format("An unhandled exception occurred: {0}" + "user.online = " + user.online, e.Exception.Message);
            //MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //Trace.WriteLine(errorMessage+"Error"+MessageBoxButton.OK+MessageBoxImage.Error);
            e.Handled = true;

        }


    }


}
