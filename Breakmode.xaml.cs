using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for Breakmode.xaml
    /// </summary>

    public partial class Breakmode : Window
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        //Color myBlue = (Color)ColorConverter.ConvertFromString("#FF468499");
    
        double screenX = System.Windows.SystemParameters.WorkArea.Width;
        double screenY = System.Windows.SystemParameters.WorkArea.Height;

        // bool mouseclickout = false;

        public Breakmode()
        {
            InitializeComponent();
            this.Activate();
            this.Topmost = true;

            foreach (Window w in Application.Current.Windows)
            {
                if (w is MainWindow)
                {
                    this.Owner = w;
                    break;
                }
            }
            //typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(button, new object[] { true });

            label.Focus();

            //EventManager.RegisterClassHandler(typeof(Window), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDown));

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            user._time_break = TimeSpan.FromSeconds(user.response * 60);

            user._timer_break = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //this.Activate();
                text_remain.Content = user._time_break.ToString();

                if (user._time_break == TimeSpan.Zero)
                {
                    user._timer_break.Stop();

                    label.Content = "The break is over.";
                    text_remain.Content = "";
                    //button_go_back.Visibility = Visibility.Visible;
                    //button_close.Visibility = Visibility.Hidden;
                    Style style = this.FindResource("GreenButton") as Style;
                    button_close.Style = style;
                    button_close.Content = "Go Back to Work";
                    button_close.IsDefault = true;
                    button_close.IsCancel = false;
                    button_close.BorderThickness = new Thickness(2);
                    button_close.Background = new SolidColorBrush(Color.FromRgb(133, 193, 63));
                }
                if (IsAnyKeyDown())
                {
                    userdata_insert();
                    user._timer_break.Stop();
                    user.count_mins = 0;
                    user.count_seconds = 0;
                    user._timer.Start();
                    this.Close();
                }

                user._time_break = user._time_break.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
        }


        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
            user.onBreak = false;
            //var myObject = this.Owner as MainWindow;
            //myObject.checkonline();

            //((MainWindow)this.Owner).checkonline();
            //userdata_insert();
            user.count_mins = 0;
            user.count_seconds = 0;

            user._timer_break.Stop();
            this.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //userdata_insert();
            // user._timer.Start();

        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        //private void button_go_back_Click(object sender, RoutedEventArgs e)
        //{
        //    //closeExitwin();
        //    this.Topmost = false;
        //    //userdata_insert();
        //    //MainWindow main = new MainWindow();
        //    //main.Show();
        //    user.onBreak = false;
        //    user._timer_break.Stop();

        //    Trace.WriteLine("Green button.");
        //    var myObject = this.Owner as MainWindow;
        //    myObject.checkonline();

        //    user.count_mins = 0;
        //    user.count_seconds = 0;
        //    user._timer.Start();
        //    this.Close();
        //}

        public void userdata_insert()
        {
            MySqlConnection con = new MySqlConnection();
            label5.Content = "";

            if (!CheckConnection("www.google.com"))
            {
                label5.Content = "Please check your network connection.";
            }

            user.t2 = DateTime.Now;
            user.time2 = user.t2.ToString("yyyy-MM-dd HH:mm:ss");
            user.secondsDiff12 = (int)((user.t2 - user.t1).TotalSeconds);
            Trace.WriteLine("diiff12" + user.secondsDiff12);
            try
            {
                //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                con.Open();
                Trace.WriteLine("Breakmode Connected");

                string cmdStr = "Insert into tblUsage(username, work_time, time0, active_window0, time1, secondsDiff01, active_window1, response, time2, secondsDiff12) VALUES(@username, @work_time, @time0, @active_window0, @time1, @secondsDiff01, @active_window1, @response, @time2, @secondsDiff12)";
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

                    Trace.WriteLine("Response: " + user.response);

                    cmd.Parameters.AddWithValue("@time2", user.time2);
                    cmd.Parameters.AddWithValue("@secondsDiff12", user.secondsDiff12);

                    cmd.ExecuteNonQuery();
                    Trace.WriteLine("inserted_breakmode" + user.username + user.time_ + user.title_ + user.response + user.time2);
                }
            }
            catch (Exception e1)
            {
                Trace.WriteLine(e1.ToString());
            }

            con.Close();
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

        public static bool IsAnyKeyDown()
        {
            var values = Enum.GetValues(typeof(Key));

            foreach (var v in values)
            {
                if (((Key)v) != Key.None)
                {
                    if (Keyboard.IsKeyDown((Key)v))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

     

        void App_Deactivated(object sender, EventArgs e)
        {
            Trace.WriteLine("Clicked face!!");
            this.Topmost = false;
            //posX = GetMousePosition().X;
            //posY = GetMousePosition().Y;
            //Trace.WriteLine("screen width = " + screenX + "screen height = " + screenY);
            //Trace.WriteLine("window width = " + this.Width + "window height = " + this.Height);
            //Trace.WriteLine("posX = " + posX + "posY = " + posY);

            //Trace.WriteLine("not on in the loop");

            //if (((screenX - posX) < this.Width) || ((screenY - posY) < this.Height))
            //{
            //    Trace.WriteLine("clicked red or green button!");
            //    return;
            //    //mouseclickout = true;
            //}
            user.onBreak = false;
            userdata_insert();
            Trace.WriteLine("on the window");
            user._timer_break.Stop();

            ((MainWindow)this.Owner).checkonline();
            user.count_mins = 0;
            user.count_seconds = 0;
            if(user.update)
                user._timer.Start();
            this.Close();
        }



        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //closeExitwin();
            user._timer_break.Stop();
        }

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);

        //    // Begin dragging the window
        //    this.DragMove();
        //}

    }
}
