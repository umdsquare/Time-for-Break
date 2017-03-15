using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Timers;
using System.Threading;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;
using Microsoft.Win32;
using System.Security.Permissions;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using System.Drawing;
using System.Net;
using System.Reflection;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
   
    public partial class MainWindow : Window
    {
        static string minStr = "";
        bool exist = false;
       
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        System.Windows.Forms.ContextMenu contextMenu1 = new System.Windows.Forms.ContextMenu();
        System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem();

        public object ActiveWindow { get; private set; }
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        //DispatcherTimer _timer;
        [HostProtectionAttribute(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]

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


        //public static NotiWindow notiWin = new NotiWindow();
        private static void OpenNoti()
        {
            user.t0 = DateTime.Now;
            user.time = user.t0.ToString("yyyy-MM-dd HH:mm:ss");
            user.title = GetActiveWindowTitle();
            Trace.WriteLine(user.title);

            NotiWindow notiWin = new NotiWindow();
            notiWin.Show();
            //System.Windows.Threading.Dispatcher.Run();


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

        //private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        //{
        //    switch (e.Mode)
        //    {
        //        case PowerModes.Resume:
        //            if (user._timer != null)
        //                user._timer.Stop();
        //            Trace.WriteLine("system resume...");
        //            if (userdata_init())
        //            {
        //                user.work_time = user.numbers[0];
        //                settimer();
        //                Trace.WriteLine("set timer");
        //            }
        //            else
        //            {
        //                if (user._timer != null)
        //                {
        //                    user._timer.Stop();
        //                }

        //                bool isWindowOpen = false;

        //                foreach (Window w in Application.Current.Windows)
        //                {
        //                    if (w is Settings)
        //                    {
        //                        isWindowOpen = true;
        //                        w.Activate();

        //                    }
        //                }


        //                if (!isWindowOpen)
        //                {
        //                    if (user._timer != null)
        //                        user._timer.Stop();
        //                    this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        //                    //Settings setting = new Settings();
        //                    //setting.ShowDialog();
        //                }

        //                Trace.WriteLine("reset...");
        //            }
        //            break;
        //        case PowerModes.Suspend:
        //            Trace.WriteLine("system sleeping...");
        //            if (user._timer != null)
        //                user._timer.Stop();
        //            break;
        //    }
        //}

        public void settimer()
        {
            user.count_mins = 0;
            user.count_seconds = 0;

            Trace.WriteLine("set timer..");
            ni.Text = "...";
            string min_unit = " mins ";
            string sec_unit = " secs ";
            user._time = TimeSpan.FromSeconds(user.work_time);
            user.remindContent = "You have been working for ";
            if (user._timer != null)
            {
                user._timer.Stop();
            }
            user._timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                // minStr = user._time.ToString("c");
                if (user.count_mins == 1)
                { min_unit = " min "; }
                else
                { min_unit = " mins "; }
                if (user.count_seconds == 1)
                { sec_unit = " sec "; }
                else
                { sec_unit = " secs "; }
                minStr = (user.count_mins.ToString()) + min_unit + (user.count_seconds.ToString()) + sec_unit;
               

                // timer.ToolTip = user.remindContent + minStr;
                ni.Text = user.remindContent + minStr;
                //Trace.WriteLine(user.remindContent + minStr);

                if (user._time == TimeSpan.Zero)
                {

                    if (IsAnyKeyDown())
                    {
                        user._timer.Stop();
                        Trace.WriteLine("Keyboard is active");
                        user._time = TimeSpan.FromSeconds(10);
                        user._timer.Start();
                    }
                    else
                    {
                        user._timer.Stop();
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            OpenNoti();
                            Trace.WriteLine("main process is running");

                        }

                    ));
                        user._time = TimeSpan.FromSeconds(user.work_time);

                    }

                }

                user.count_seconds++;
                if (user.count_seconds == 60)
                {
                    user.count_seconds = 0;
                    user.count_mins++;
                }

                user._time = user._time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (user.onBreak)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Breakmode)
                    {

                        w.Topmost = false;
                        break;
                    }
                }

            }

            else
            {

                switch (e.Reason)
                {
                    case SessionSwitchReason.SessionLock:
                        Trace.WriteLine("system locked...");
                        user.t1 = DateTime.Now;
                        user.time_ = user.t1.ToString("yyyy-MM-dd HH:mm:ss");
                        user.work_min = user.count_mins;
                        if (user._timer != null)
                        {
                            user._timer.Stop();
                            ni.Text = "Time for Break";
                        }
                        break;
                   
                    case SessionSwitchReason.SessionUnlock:
                        
                        Trace.WriteLine("system unlocked...");
                        if (userdata_init())
                        {
                            user.work_time = user.numbers[0];
                            settimer();
                            Trace.WriteLine("set timer from lock screen..");
                            LockDataInsert();
                        }
                        else
                        {
                            bool isWindowOpen = false;

                            foreach (Window w in Application.Current.Windows)
                            {
                                if (w is Settings)
                                {
                                    isWindowOpen = true;
                                    w.Activate();

                                }
                            }
                            if (!isWindowOpen)
                            {
                                Settings childWindow = new Settings();
                                //childWindow.MyEvent += new EventHandler(childWindow_MyEvent);

                                childWindow.Show();
                                // this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
   
                            }
                            Trace.WriteLine("reset from screen lock...");
                        }
                   
                        break;
                }
            }
        }

      

        public MainWindow()
        {
            InitializeComponent();
            ShowInTaskbar = false;
        
            menu1.Text = "Settings";
            menu1.Click += new System.EventHandler(this.menuItem1_Click);
            contextMenu1.MenuItems.Add(menu1);
            string BaseDir = System.AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                ni.Icon = new Icon(BaseDir+"\\logo.ico");
                //ni.Icon = new Icon("timer.ico");
                ni.ContextMenu = contextMenu1;
                ni.Text = "Time For Break";
                //Shell_NotifyIcon(DWORD dwMessage, PNOTIFYICONDATA lpdata);
                ni.Visible = true;
                //Trace.WriteLine("Added trayicon..");
            }
            catch (Exception e)
            {
                MessageBox.Show("2. " + e.Message);
            }

            //SystemEvents.PowerModeChanged += OnPowerChange;
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            //this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

            //ni.DoubleClick +=
            //    delegate (object sender, EventArgs args)
            //    {
            //        this.Show();
            //        this.WindowState = WindowState.Normal;
            //    };

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;



            if (user.online)
            {

                // text.Visibility = Visibility.Visible;
                //get intervals and break duration for this particular user
                if (userdata_init())
                {
                    user.work_time = user.numbers[0];
                    Trace.WriteLine("Timespan = " + user.work_time);

                    settimer();

                }
                else
                {
                    if (user._timer != null)
                    {
                        user._timer.Stop();
                        ni.Text = "Time for Break";
                    }
                    Trace.WriteLine("setting??");
                    bool isWindowOpen = false;

                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is Settings)
                        {
                            isWindowOpen = true;
                            w.Activate();
                        }
                    }

                    if (!isWindowOpen)
                    {
                        if (user._timer != null)
                        { user._timer.Stop();
                            ni.Text = "Time for Break";
                        }
                         this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
                        //Settings childWindow = new Settings();
                        //childWindow.Show();
                    }

                }
            }
            else
            {
                if (user._timer != null)
                {   user._timer.Stop();
                    ni.Text = "Time for Break";
                }
                this.Close();
            }

            //Application.Current.MainWindow.Closed += (s, a) =>
            //{
            //    Trace.WriteLine("Appication.current: is this the final closing?");
            //    ni.Visible = false;
            //};

        }

      

        public void checkonline()
        {
           
            if (user.online)
            {
                if (userdata_init())
                {
                    user.update = true;
                    user.work_time = user.numbers[0];
                    Trace.WriteLine("Timespan = " + user.work_time);

                    settimer();

                }
                else
                {
                    user.update = false;
                    Trace.WriteLine("setting??");

                    if (user._timer != null)
                    { user._timer.Stop();
                      ni.Text = "Time for Break";
                    }

                    bool isWindowOpen = false;

                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is Settings)
                        {
                            isWindowOpen = true;
                            w.Activate();

                        }
                    }
                    if (!isWindowOpen)
                    {
                        Settings childWindow = new Settings();
                        //childWindow.MyEvent += new EventHandler(childWindow_MyEvent);

                        childWindow.Show();
                        childWindow.Activate();
                      // this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
                    }

                }
            }
            else
            {

                if (user._timer != null)
                {
                    user._timer.Stop();
                    ni.Text = "Time for Break";
                }

                this.Close();
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Settings childWindow = new Settings();
            childWindow.MyEvent += new EventHandler(childWindow_MyEvent);

            childWindow.ShowDialog();
        }

        void childWindow_MyEvent(object sender, EventArgs e)
        {
            // handle event
            Trace.WriteLine("Event handle..");
            settimer();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            bool isWindowOpen = false;

            foreach (Window w in Application.Current.Windows)
            {
                if (w is Settings)
                {
                    isWindowOpen = true;
                    w.Activate();
                }
            }

            if (!isWindowOpen)
            {

                if (user._timer != null)
                {
                    user._timer.Stop();
                    ni.Text = "Time for Break";
                }
                //this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
                Settings settings = new Settings();
                settings.Show();
                //this.Close();
            }
        }

        public bool userdata_init()
        {
            MySqlConnection con = new MySqlConnection();
            DateTime t = DateTime.Now;
            user.today = t.ToString("yyyy-MM-dd");
            Trace.WriteLine("today is " + user.today);
            try
            {
                //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                con.Open();
                Trace.WriteLine("Initial connection established");

                MySqlCommand cmd = new MySqlCommand("Select * from tblSettings where username = @username order by dateChanged DESC Limit 1;", con);
                cmd.Parameters.AddWithValue("@username", user.username);
                using (MySqlDataReader oReader = cmd.ExecuteReader())
                {

                    while (oReader.Read())
                    {
                        user.mins[0] = (int)(oReader["Random1"]);

                        user._today = ((DateTime)(oReader["dateChanged"])).ToString("yyyy-MM-dd");
                        Trace.WriteLine("__today: " + user._today);
                        user.disppear_min = (int)(oReader["Disappear"]);
                        user.numbers[0] = user.mins[0] * 60;

                        user.disppear = user.disppear_min * 60;
                        user.update = true;

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
            con.Close();

            if ((!exist) || (user.today != user._today))
            {
                exist = false;
                user.update = false;
            }

            Trace.WriteLine("exist is " + exist);
            return exist;

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Trace.WriteLine("is this the final closing?");
            //ni.Visible = false;

            ni.Dispose();
            if (user._timer != null)
            {
                user._timer.Stop();

            }

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


        private void LockDataInsert()
        {
            user.t2 = DateTime.Now;
            user.time2 = user.t2.ToString("yyyy-MM-dd HH:mm:ss");
            user.secondsDiff12 = (int)((user.t2 - user.t1).TotalSeconds);
            Trace.WriteLine("diiff12" + user.secondsDiff12);
            //user.time_ = DateTime.Now.ToStringl
            user.title_ = GetActiveWindowTitle();
            user.response = -2;

            //if (!CheckConnection("www.google.com"))
            //{
            //    ni.ShowBalloonTip(3000, "Time For Break", "Please check you network connection.", System.Windows.Forms.ToolTipIcon.Info);
            //}

            MySqlConnection con = new MySqlConnection();
            try
            {
                //con.ConnectionString = "Server = dsquare.ist.psu.edu; database = timebreak; UID =timebreak; password =mFPS9mMEBCgD;";
                con.ConnectionString = "server = 107.180.40.19; Port = 3306; database = timebreak; UID=yuhan; Password=lab323;";
                //con.ConnectionString = "data source = YPL5142-LOANER\\MSSQLSERVER01; database = timebreak; integrated security = SSPI";
                con.Open();
                Trace.WriteLine("lockdata_Connected");

                string cmdStr = "Insert into tblUsage(username, work_time, time1, active_window1, response, time2, secondsDiff12) VALUES(@username, @work_time, @time1, @active_window1, @response, @time2, @secondsDiff12)";
                using (MySqlCommand cmd = new MySqlCommand(cmdStr, con))
                {
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@work_time", user.work_min);

                    cmd.Parameters.AddWithValue("@time1", user.time_);
                    cmd.Parameters.AddWithValue("@active_window1", user.title);

                    cmd.Parameters.AddWithValue("@response", user.response);

                    cmd.Parameters.AddWithValue("@time2", user.time2);
                    cmd.Parameters.AddWithValue("@secondsDiff12", user.secondsDiff12);

                    cmd.ExecuteNonQuery();
                    Trace.WriteLine("inserted_lockdata" + user.username + user.time_ + user.time2 + user.secondsDiff12 + user.title_ + user.response);
                }
            }
            catch (Exception e1)
            {
                Trace.WriteLine(e1.ToString());
            }
            con.Close();
            // user._timer.Start();
        }
    }
}
