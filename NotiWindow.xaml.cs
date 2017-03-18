using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.Net;
using System.Media;
using System.Linq;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for NotiWindow.xaml
    /// </summary>

    public partial class NotiWindow : Window
    {
        public object ActiveWindow { get; private set; }
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        DispatcherTimer _timer;
        TimeSpan _time;

        string unit = "minutes.";

        private void WindowIgnored()
        {
            user.t1 = DateTime.Now;
            user.time_ = user.t1.ToString("yyyy-MM-dd HH:mm:ss");
            user.secondsDiff01 = (int)((user.t1 - user.t0).TotalSeconds);
            Trace.WriteLine("diiff01" + user.secondsDiff01);
            //user.time_ = DateTime.Now.ToString();
            user.title_ = GetActiveWindowTitle();
            user.response = -1;
            user.reason = "0";

            MySqlConnection con = new MySqlConnection();
            try
            {
                //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                con.Open();
                Trace.WriteLine("Noti_Connected");

                string cmdStr = "Insert into tblUsage(username, work_time,  time0, active_window0, time1, secondsDiff01, active_window1, response) VALUES(@username, @work_time, @time0, @active_window0, @time1, @secondsDiff01, @active_window1, @response)";
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

                    cmd.ExecuteNonQuery();
                    Trace.WriteLine("inserted_Noti" + user.username + user.time_ + user.title_ + user.response);
                }
            }
            catch (Exception e1)
            {
                Trace.WriteLine(e1.ToString());
            }
            con.Close();
            // user._timer.Start();
        }

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
        //    if (wasCodeClosed)
        //    {
        //        //closed by this.close
                
        //    }
        //    else
        //    {
        //        user._timer.Start();
        //    }

        //}


        public NotiWindow()
        {
            InitializeComponent();
            WPlaySound.PlaySoundEvent("isNotification.Default");
            // user._timer.Stop();
            this.Topmost = true;
            this.Activate();
            label5.Content = "";

            // Clear previous cookies
            //Trace.WriteLine("start: " + user.response);
            //user.response = -1;
            //user.reason = "";
            //user.comment = "";
            //Trace.WriteLine("after cleaning: " + user.response);

            if (user.work_time / 60 == 1)
            {
                unit = "minute.";
            }

            label2_copy.Content = (user.work_time / 60).ToString() + " " + unit;

            //var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            //this.Left = desktopWorkingArea.Right - this.Width;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
            //this.Activate();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>

            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                this.Left = workingArea.Right - this.Width;
                this.Top = workingArea.Bottom - this.Height;
                this.Activate();
            }));

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

                    //MainWindow main = new MainWindow();
                    //main.Show();                  
                    user.count_mins = 0;
                    user.count_seconds = 0;
                    user._timer.Start();
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


        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                if (Buff.ToString().Contains(" - "))
                {
                    string temp = Buff.ToString();
                    int index = temp.LastIndexOf(" - ");
                    int index1 = (temp.Length) - index;
                    Trace.WriteLine("index: " + index + ", Length: " + temp.Length + ", last-index: " + index1);
                    string temp1 = temp.Substring(index + 3, temp.Length - 3 - index);
                    return temp1;
                }
                else
                {
                    return Buff.ToString();
                }

            }
            return null;
        }


        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            user.response = 1;
            user.reason = "";
            user.comment = "";

            label5.Content = "";
            Notiwin1 noti1 = new Notiwin1();
            noti1.Show();
            this.Close();
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            user.response = 0;
            label5.Content = "";
            Notiwin2 noti2 = new Notiwin2();
            noti2.Show();
            this.Close();

        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            WindowIgnored();
            //MainWindow main = new MainWindow();
            //main.Show();
            user.count_mins = 0;
            user.count_seconds = 0;
            user._timer.Start();

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
