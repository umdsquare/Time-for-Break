using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for logout.xaml
    /// </summary>
    public partial class logout : Window
    {
        public logout()
        {
            InitializeComponent();
        }

        private void stay_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings[settingName] != null)
            {
                config.AppSettings.Settings.Remove(settingName);
            }
            config.AppSettings.Settings.Add(settingName, valueName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        private void quit_Click(object sender, RoutedEventArgs e)
        {
            user.online = false;
            user.username = "";
            UpdateSettingString("userName", "");
            UpdateSettingString("password", "");
            UpdateSettingString("isRemind", "false");


            foreach (Window w in Application.Current.Windows)
            {
                if ((w is Notiwin1) || (w is Notiwin2) || (w is NotiWindow) || (w is Breakmode) || (w is Settings))
                {
                    w.Close();
                }
            }

            if (user._timer != null)
            {
                user._timer.Stop();
            }
            Setup setup = new Setup();
            setup.Show();
            this.Close();
        }
    }
}
