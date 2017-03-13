using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Configuration;
using System.Linq;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for ResearchWindow.xaml
    /// </summary>
    public partial class Settings : Window
    {
        //public delegate void DataChangedEventHandler(object sender, EventArgs e);
        //public event DataChangedEventHandler DataChanged;
        public event EventHandler MyEvent;
        int n;
        bool workd_text = false;
        bool notid_text = false;
        public Settings()
        {
            InitializeComponent();
            this.Activate();
            textBox1_i.Focus();
            DateTime t1 = DateTime.Now;
            user.today = t1.ToString("yyyy-MM-dd");

            foreach (Window w in Application.Current.Windows)
            {
                if (w is MainWindow)
                {
                    this.Owner = w;
                    break;
                }
            }

            fetchData();

        }

        protected void OnMyEvent()
        {
            if (this.MyEvent != null)
            {
                this.MyEvent(this, EventArgs.Empty);
                Trace.WriteLine("on my event");
            }
            this.Close();
        }

        void Child_Loaded(object sender, RoutedEventArgs e)
        {
            // call event
            Trace.WriteLine("child loaded");
            this.OnMyEvent();
        }

        public bool fetchData()
        {

            MySqlConnection con = new MySqlConnection();
            bool exist = false;
            try
            {
                //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                con.Open();
                Trace.WriteLine("Settings connection established");

                MySqlCommand cmd = new MySqlCommand("Select * from tblSettings where username = @username order by dateChanged DESC Limit 1;", con);
                cmd.Parameters.AddWithValue("@username", user.username);
                using (MySqlDataReader oReader = cmd.ExecuteReader())
                {

                    while (oReader.Read())
                    {
                        user.mins[0] = (int)(oReader["Random1"]);
                        Trace.WriteLine("work duration: " + user.mins[0]);
                        user._today = ((DateTime)(oReader["dateChanged"])).ToString("yyyy-MM-dd");
                        // Trace.WriteLine("setting__today: " + user._today);
                        user.disppear_min = (int)(oReader["Disappear"]);

                        user.numbers[0] = user.mins[0] * 60;

                        user.disppear = user.disppear_min * 60;
                        user.update = true;

                        textBox1_i.Text = Convert.ToString(user.mins[0]);

                        textBox5.Text = Convert.ToString(user.disppear_min);

                        //pass = oReader["password"].ToString();

                        Trace.WriteLine("random1 " + 0 + user.numbers[0] + ", ");
                        exist = true;

                    }


                }
                Trace.WriteLine("disappear =" + user.disppear);
            }
            catch (Exception e1)
            {
                Trace.WriteLine(e1.ToString());

            }

            if ((!exist) || user._today != user.today)
            {
                exist = false;
                user.update = false;
                // label2.Content = "You haven't set up your desired work duration for today!";
            }
            con.Close();
            Trace.WriteLine("update = " + user.update);
            return exist;


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

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection();

            if (!CheckConnection("www.google.com"))
            {
                //spinner.Visibility = Visibility.Visible;
                label2.Content = "Please check your network connection.";
            }

            if ((!user.update) && (!(textBox1_i.Text.Equals(""))) && (!(textBox5.Text.Equals(""))))
            {
                label2.Content = "";

                user.mins[0] = Int32.Parse(textBox1_i.Text);

                user.disppear_min = Int32.Parse(textBox5.Text);

                user.numbers[0] = user.mins[0] * 60;

                user.disppear = user.disppear_min * 60;
                Trace.WriteLine("inserting.." + user.username + textBox1_i.Text + textBox5.Text + user.today);
                try
                {
                    con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                    con.Open();
                    Trace.WriteLine("Settings connection established");
                    string cmdStr = "Insert into tblSettings(username, Random1, Disappear, dateChanged) VALUES(@username, @duration, @disappear, @date)";
                    using (MySqlCommand cmd = new MySqlCommand(cmdStr, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@duration", textBox1_i.Text);
                        cmd.Parameters.AddWithValue("@disappear", textBox5.Text);
                        cmd.Parameters.AddWithValue("@date", user.today);
                        cmd.ExecuteNonQuery();
                        Trace.WriteLine("inserted" + user.username + textBox1_i.Text + textBox5.Text + user.today);
                        label2.Content = "Updated Successful!";
                    }

                    user.update = true;
                }
                catch (Exception e1)
                {
                    Trace.WriteLine(e1.ToString());
                }

                con.Close();
                  
                    // this.Loaded += new RoutedEventHandler(Child_Loaded);
                    user.work_time = user.numbers[0];
                ((MainWindow)this.Owner).settimer();
                this.Close();

                //this.OnMyEvent();
                Trace.WriteLine("on event here.., user.online = " + user.online );
            }

            else
            {

                if ((user.update))
                {
                    label2.Content = "you have alrealy updated your work duration today!";
                }


                if (((user.update)) && ((textBox1_i.Text.Equals("")) || (textBox5.Text.Equals(""))))
                {
                    textBox1_i.Text = Convert.ToString(user.mins[0]);
                    textBox5.Text = Convert.ToString(user.disppear_min);
                    label2.Content = "you have alrealy updated your work duration today!";
                }


                if (((!user.update)) && ((textBox1_i.Text.Equals("")) || (textBox5.Text.Equals(""))))
                {
                    label2.Content = "You haven't set up your desired work duration for today!";
                }

            }

        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            textBox1_i.Text = "";

            textBox5.Text = "";
            label2.Content = "";
            textBox1_i.Focus();
        }

        private void textBox1_i_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textBox1_i = sender as TextBox;
            textBox1_i.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#abadb3"));
            int min_temp = 0;

            if (!textBox1_i.Text.Equals(""))
            {

                bool isNumeric = int.TryParse((textBox1_i.Text), out n);
                if (!isNumeric)
                {

                    textBox1_i.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff5050"));
                    label2.Content = "Please input a number.";
                }
                else
                {
                    min_temp = Int32.Parse(textBox1_i.Text);
                    label2.Content = "";

                    if (min_temp > 120)
                    {
                        textBox1_i.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff5050"));
                        label2.Content = "The maximal length of a work duration is 120 minutes.";
                    }
                    else
                    {
                        label2.Content = "";
                        workd_text = true;
                    }
                }
            }

            else
            {
                workd_text = false;
            }

            if (workd_text && notid_text)
            {
                button_ok.IsDefault = true;
                button_ok.BorderThickness = new Thickness(2);
            }
            else
            {
                button_ok.IsDefault = false;
                button_ok.BorderThickness = new Thickness(1);
            }

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox5 = sender as TextBox;
            textBox5.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#abadb3"));
            int min_temp = 0;

            if (!textBox5.Text.Equals(""))
            {
                bool isNumeric = int.TryParse((textBox5.Text), out n);
                if (!isNumeric)
                {
                    textBox5.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff5050"));
                    label2.Content = "Please input a number.";
                }
                else
                {
                    min_temp = Int32.Parse(textBox5.Text);
                    if (min_temp > 5)
                    {
                        textBox5.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff5050"));
                        label2.Content = "The maximal length of the appearance of break reminder \n window is 5 minutes.";
                    }
                    else
                    {
                        label2.Content = "";
                        notid_text = true;
                    }
                }

            }

            else
            {
                notid_text = false;
            }

            if (workd_text && notid_text)
            {
                button_ok.IsDefault = true;
                button_ok.BorderThickness = new Thickness(2);
            }
            else
            {
                button_ok.IsDefault = false;
                button_ok.BorderThickness = new Thickness(1);
            }

        }

   
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (wasCodeClosed)
            {
                //closed by this.close
                Trace.WriteLine("was code closed");
            }
            else
            {
                Trace.WriteLine("was not code closed");

                if (!user.update)
                {
                    label2.Content = "You haven't set up your desired work duration for today!";
                    e.Cancel = true;
                }
                else
                {
                    if (user._timer != null)
                        user._timer.Start();
                    base.OnClosing(e);
                }

                Trace.WriteLine("Settings is closing..");
            }


        }



        private void MenuItemlogout_Click(object sender, RoutedEventArgs e)
        {
            logout logout1 = new logout();
            logout1.ShowDialog();
        }

        private void MenuItemexit_Click(object sender, RoutedEventArgs e)
        {
            exitwin logout1 = new exitwin();
            logout1.ShowDialog();
        }

    }
}
