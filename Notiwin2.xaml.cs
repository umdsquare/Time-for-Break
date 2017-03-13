using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.Net;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for NotiWindow.xaml
    /// </summary>

    public partial class Notiwin2 : Window
    {
        public object ActiveWindow { get; private set; }
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        DispatcherTimer _timer;
        TimeSpan _time;
        //bool reason_text = false;

        private void WindowIgnored()
        {
            DateTime t1 = DateTime.Now;
            user.time_ = t1.ToString("yyyy-MM-dd HH:mm:ss");
            //user.time_ = DateTime.Now.ToString();
            user.title_ = GetActiveWindowTitle();
            user.response = 0;
            //user.reason = "0";

            MySqlConnection con = new MySqlConnection();
            try
            {
                //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                con.Open();
                Trace.WriteLine("Notiwin_Connected");

                string cmdStr = "Insert into tblUsage(username, work_time, time0, active_window0, time1, active_window1, response, comment) VALUES(@username, @work_time, @time0, @active_window0, @time1, @active_window1, @response, @comment)";
                using (MySqlCommand cmd = new MySqlCommand(cmdStr, con))
                {
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@work_time", user.work_time / 60);

                    cmd.Parameters.AddWithValue("@time0", user.time);
                    cmd.Parameters.AddWithValue("@active_window0", user.title);

                    cmd.Parameters.AddWithValue("@time1", user.time_);
                    cmd.Parameters.AddWithValue("@active_window1", user.title_);
                    cmd.Parameters.AddWithValue("@response", user.response);

                    //cmd.Parameters.AddWithValue("@reason", user.reason);
                    cmd.Parameters.AddWithValue("@comment", user.comment);

                    cmd.ExecuteNonQuery();
                    Trace.WriteLine("inserted_Notiwin" + user.username + user.time_ + user.title_ + user.response);
                }
            }
            catch (Exception e1)
            {
                Trace.WriteLine(e1.ToString());
            }

            //MainWindow main = new MainWindow();
            //main.Show();
            user.count_mins = 0;
            user.count_seconds = 0;
            user._timer.Start();
            con.Close();

        }

        private void ChangeLayout0()
        {
            brd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffb3b3"));
            Trace.WriteLine("12 seconds left");
            //this.Background = System.Windows.Media.Brushes.MistyRose;
            this.Activate();
        }

        private void ChangeLayout1()
        {
            brd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffcccc"));
            Trace.WriteLine("11 seconds left");
            //this.Background = System.Windows.Media.Brushes.MistyRose;
            this.Activate();
        }

        private void ChangeLayout2()
        {
            brd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffe6e6"));
            Trace.WriteLine("10 seconds left");
            //this.Background = System.Windows.Media.Brushes.MistyRose;
            this.Activate();
        }

        private void ChangeLayout3()
        {
            brd.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f9ff"));
            Trace.WriteLine("9 seconds left");
            //this.Background = System.Windows.Media.Brushes.MistyRose;
            this.Activate();
        }



        public Notiwin2()
        {
            InitializeComponent();
            this.Topmost = true;
            textBox.Focus();
            textBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABADB3"));
            // Clear previous cookies
            Trace.WriteLine("start: " + user.response);
            user.response = -1;
            //user.reason = "";
            user.comment = "";
            Trace.WriteLine("after cleaning: " + user.response);

            SolidColorBrush defaultColor = new SolidColorBrush();
            defaultColor.Color = Color.FromRgb(171, 173, 179);

            SolidColorBrush highlightColor = new SolidColorBrush();
            highlightColor.Color = Color.FromRgb(255, 80, 80);


            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            _time = TimeSpan.FromSeconds(user.disppear);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {

                if (_time == TimeSpan.FromSeconds(12))
                {
                    ChangeLayout0();
                }

                if (_time == TimeSpan.FromSeconds(11))
                {
                    ChangeLayout1();
                }

                if (_time == TimeSpan.FromSeconds(10))
                {
                    ChangeLayout2();
                }

                if (_time == TimeSpan.FromSeconds(9))
                {
                    ChangeLayout3();
                }

                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    WindowIgnored();
                    Trace.WriteLine("countdown");
                    this.Close();
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
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


        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Current data response = " + user.response + "  comment = " + user.comment);
            label5.Content = "";

            if (user.comment.Equals(""))
            {
                label5.Content = "Please type your reason before submitting your answer.";
                //textBox.SelectAll();
                textBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff5050"));
            }
            else
            {
                textBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABADB3"));

                if (!CheckConnection("www.google.com"))
                {
                    label5.Content = "Please check your network connection.";
                }

                user.t1 = DateTime.Now;
                user.time_ = user.t1.ToString("yyyy-MM-dd HH:mm:ss");
                //user.time_ = DateTime.Now.ToString();
                user.secondsDiff01 = (int)((user.t1 - user.t0).TotalSeconds);
                Trace.WriteLine("diiff01" + user.secondsDiff01);
                user.title_ = GetActiveWindowTitle();

                user.response = 0;
                MySqlConnection con = new MySqlConnection();
                try
                {
                    //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                    con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                    //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                    con.Open();
                    Trace.WriteLine("Connected");

                    string cmdStr = "Insert into tblUsage(username, work_time, time0, active_window0, time1, secondsDiff01, active_window1, response, comment) VALUES(@username, @work_time, @time0, @active_window0, @time1, @secondsDiff01, @active_window1, @response, @comment)";
                    using (MySqlCommand cmd = new MySqlCommand(cmdStr, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@work_time", user.work_time / 60);

                        cmd.Parameters.AddWithValue("@time0", user.time);
                        cmd.Parameters.AddWithValue("@active_window0", user.title);

                        cmd.Parameters.AddWithValue("@time1", user.time_);
                        cmd.Parameters.AddWithValue("@secondsDiff01", user.secondsDiff01);
                        cmd.Parameters.AddWithValue("@active_window1", user.title_);
                        cmd.Parameters.AddWithValue("@response", user.response);

                        //cmd.Parameters.AddWithValue("@reason", user.reason);
                        cmd.Parameters.AddWithValue("@comment", user.comment);

                        cmd.ExecuteNonQuery();
                        Trace.WriteLine("inserted_noti" + user.username + user.time_ + user.title_ + user.response);
                    }
                }
                catch (Exception e1)
                {
                    Trace.WriteLine(e1.ToString());
                }

                con.Close();
                user.count_mins = 0;
                user.count_seconds = 0;
                user._timer.Start();
                //MainWindow main = new MainWindow();
                //main.Show();

                //Application.Current.Dispatcher.Invoke((Action)(() =>
                //{
                //    MainWindow main = new MainWindow();
                //    main.Show();

                //}));

                user.time = "";
                user.time_ = "";
                user.title = "";
                user.title_ = "";
                user.response = -1;
                _timer.Stop();
                this.Close();
            }
        }

 
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get control that raised this event.
            var textBox = sender as TextBox;
            textBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABADB3"));
            // Add comments.
            if (!(textBox.Text.Equals("")))
            {
                user.comment = textBox.Text;
                //reason_text = true;
                button_ok.IsDefault = true;
                button_ok.BorderThickness = new Thickness(2);
            }
            else
            {
                //reason_text = false;
                button_ok.IsDefault = false;
                button_ok.BorderThickness = new Thickness(1);
            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            WindowIgnored();
            _timer.Stop();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NotiWindow noti = new NotiWindow();
            noti.Show();

            _timer.Stop();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (_timer != null)
                _timer.Stop();
        }
    }
}
