using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Net;
using System.Configuration;
using System.Windows.Input;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for loginwindow.xaml
    /// </summary>
    public partial class loginwindow : Window
    {
        bool uname_text = false;
        bool pass_text = false;
        public loginwindow()
        {
            InitializeComponent();
            username.Focus();
        }

        private bool CheckConnection(String URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {

            MySqlConnection con = new MySqlConnection();

            if (!CheckConnection("www.google.com"))
            {
                label2.Content = "Please check your network connection.";
            }

            if (username.Text == "")
            {
                label2.Content = "Please type your username";
            }
            else if (password.Password == "")
            {
                label2.Content = "Please Type your password";
            }
            else
            {
                string pass = "";

                try
                {
                    //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                    //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                    con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                    con.Open();
                    Trace.WriteLine("Login Connected");
                    MySqlCommand cmd = new MySqlCommand("Select * from tblUser where username = '" + username.Text + "'", con);
                    //SqlDataReader Dr = cmd.ExecuteReader();
                    using (MySqlDataReader oReader = cmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            //username = oReader["username"].ToString();
                            pass = oReader["password"].ToString();
                            //Trace.WriteLine(username.Text + pass);
                        }
                    }

                    if (pass == password.Password)
                    {
                        Trace.WriteLine("Login Success!");
                        label2.Content = "Login Success!";
                        user.username = username.Text;
                        user.password = pass;
                        user.online = true;
                        UpdateSettingString("userName", user.username);
                        UpdateSettingString("password", pass);
                        UpdateSettingString("isRemind", "true");
                        //this.Visibility = Visibility.Hidden;
                      
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.Close();
                            Trace.WriteLine("successfully closed login window!!");
                        }

                   ));

                        MainWindow winsignup = new MainWindow();
                        winsignup.Show();
                        Trace.WriteLine("Open Mainwindow..");

                    }
                    else
                    {
                        label2.Content = "Username and password not match!";
                        Trace.WriteLine("Username and password not match!");
                    }

                }
                catch (Exception e1)
                {
                    Trace.WriteLine(e1.ToString());
                }
                con.Close();
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Setup winsignup = new Setup();
            winsignup.Show();
            this.Close();
        }


        private void Pwdbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as PasswordBox;
            user.password = textBox.Password;

            if (!textBox.Password.Equals(""))
            {
                pass_text = true;
            }
            else
            {
                pass_text = false;
            }

            if (uname_text && pass_text)
            {
                login1.IsDefault = true;
                login1.BorderThickness = new Thickness(2);
            }
            else
            {
                login1.IsDefault = false;
                login1.BorderThickness = new Thickness(1);
            }
        }

        public static void UpdateSettingString(string settingName, string valueName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            try
            {
                if (ConfigurationManager.AppSettings[settingName] == null)
                {
                    config.AppSettings.Settings.Add(settingName, valueName);
                    //Trace.WriteLine("appsetting config null");
                }
                else
                {
                    config.AppSettings.Settings[settingName].Value = valueName;
                    //Trace.WriteLine("appsetting config not null");
                }
                //config.AppSettings.Settings.Add(settingName, valueName);

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                //ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Trace.WriteLine("Error writing app settings");
            }
            //ReadAllSettings();
        }


        static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Trace.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Trace.WriteLine("Key: {0} Value: {1}" + key + appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Trace.WriteLine("Error reading app settings");
            }
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
           
            if (!textBox.Text.Equals(""))
            {
                uname_text = true;
            }
            else
            {
                uname_text = false;
            }

            if (uname_text && pass_text)
            {
                login1.IsDefault = true;
                login1.BorderThickness = new Thickness(2);
            }
            else
            {
                login1.IsDefault = false;
                login1.BorderThickness = new Thickness(1);
            }
        }
    }
}
