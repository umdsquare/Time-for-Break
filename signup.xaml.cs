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
    /// Interaction logic for signup.xaml
    /// </summary>
    public partial class signup : Window
    {

        bool uname_text = false;
        bool email_text = false;
        bool pass_text = false;
        bool passcon_text = false;
        public signup()
        {
            InitializeComponent();
            username.Focus();
            //Trace.WriteLine("start");
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

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            user.username = username.Text;
            if (!textBox.Text.Equals(""))
            {
                uname_text = true;
            }
            else
            {
                uname_text = false;
            }
            if (uname_text && email_text && passcon_text && passcon_text)
            {
                signup1.IsDefault = true;
                signup1.BorderThickness = new Thickness(2);
            }
            else
            {
                signup1.IsDefault = false;
                signup1.BorderThickness = new Thickness(1);
            }
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckConnection("www.google.com"))
            {
                label2.Content = "Please check your network connection.";
            }

            if (username.Text == "")
            {
                label2.Content = "Please type your username!";
            }
            else if (email.Text == "")
            {
                label2.Content = "Please type your email!";
            }
            else if (password.Password == "")
            {
                label2.Content = "Please type your password!";
            }
            else if (conpass.Password == "")
            {
                label2.Content = "Please confirm your password!";
            }
            else if (user.password != user.conpassword)
            {
                label2.Content = "Password doesn't match!";
                password.Clear();
                conpass.Clear();
            }
            else
            {
                MySqlConnection con = new MySqlConnection();
                bool existname = false;
                try
                {
                    //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                    //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                    con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";

                    con.Open();
                    Trace.WriteLine("Connected");
                    MySqlCommand cmd = new MySqlCommand("select * from tblUser where username = '" + username.Text + "'", con);
                    //SqlDataReader Dr = cmd.ExecuteReader();
                    using (MySqlDataReader oReader = cmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            label2.Content = "username has alrealy exist!";
                            existname = true;
                        }
                    }
                }
                catch (Exception e1)
                {
                    Trace.WriteLine(e1.ToString());
                }


                if (!existname)
                {
                    try
                    {
                        string cmdStr = "Insert into tblUser(username, email, password) VALUES(@username, @email, @password)";
                        using (MySqlCommand cmd = new MySqlCommand(cmdStr, con))
                        {
                            cmd.Parameters.AddWithValue("@username", username.Text);
                            cmd.Parameters.AddWithValue("@email", email.Text);
                            cmd.Parameters.AddWithValue("@password", password.Password);
                            cmd.ExecuteNonQuery();
                            Trace.WriteLine("inserted" + username.Text + email.Text + password.Password);
                            label2.Content = "Sign Up Successed!";
                            user.online = true;
                            UpdateSettingString("userName", user.username);
                            UpdateSettingString("password", user.password);
                            UpdateSettingString("isRemind", "true");
                            //Settings setting = new Settings();

                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.Close();
                                Trace.WriteLine("successfully closed login window!!");
                            }

                 ));

                            MainWindow winsignup = new MainWindow();
                            winsignup.Show();
                        
                            //setting.Show();
                            this.Close();
                        }
                    }
                    catch (Exception e1)
                    {
                        Trace.WriteLine(e1.ToString());
                    }
                    con.Close();
                }

            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            Setup setup = new Setup();
            setup.Show();
            this.Close();
        }

        private void Pwdbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as PasswordBox;
            user.password = textBox.Password;
            
            user.username = username.Text;
            if (!textBox.Password.Equals(""))
            {
                pass_text = true;
            }
            else
            {
                pass_text = false;
            }

            if (uname_text && email_text && pass_text && passcon_text)
            {
                signup1.IsDefault = true;
                signup1.BorderThickness = new Thickness(2);
            }
            else
            {
                signup1.IsDefault = false;
                signup1.BorderThickness = new Thickness(1);
            }
        }

        private void Pwdbox_conPasswordChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as PasswordBox;
            user.conpassword = textBox.Password;
            user.username = username.Text;
            if (!textBox.Password.Equals(""))
            {
                passcon_text = true;
            }
            else
            {
                passcon_text = false;
            }

            if (uname_text && email_text && passcon_text && passcon_text)
            {
                signup1.IsDefault = true;
                signup1.BorderThickness = new Thickness(2);
            }
            else
            {
                signup1.IsDefault = false;
                signup1.BorderThickness = new Thickness(1);
            }
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

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
        
            if (!textBox.Text.Equals(""))
            {
                email_text = true;
            }
            else
            {
                email_text = false;
            }
            if (uname_text && email_text && passcon_text && passcon_text)
            {
                signup1.IsDefault = true;
                signup1.BorderThickness = new Thickness(2);
            }
            else
            {
                signup1.IsDefault = false;
                signup1.BorderThickness = new Thickness(1);
            }
        }
    }
}
