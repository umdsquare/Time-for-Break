using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TimeforBreak
{
    public class user
    {
        public static string username;
        public static string email;
        public static string password;
        public static string conpassword;
        public static bool online = false;
        public static bool update = false;
        public static bool submitted = false;
        public static bool onBreak = false;
        public static bool existsetting;
        public static int work_time = 0;
        public static int _work_time = 0;
        public static int work_min = 0;
        public static int break_duration = 0;
        public static int min_duration = 0;
        public static int _min_duration = 0;
        public static string title = "No active window";
        public static string title_ = "No active window";

        public static DateTime t0;
        public static DateTime t1;
        public static DateTime t2;
        public static int count_seconds = 0;
        public static int count_mins = 0;

        public static string time = "";
        public static string time_ = "";
        public static string time2 = "";
        public static string today = "";
        public static string _today = "";

        public static int secondsDiff01 = -1;
        public static int secondsDiff12 = -1;

        public static int response = -1;
        public static string reason = "";
        public static string comment = "";
        public static int[] numbers = { 15 * 60, 20 * 60, 25 * 60, 30 * 60 };
        public static int[] mins = { 15, 20, 25, 30 };
        public static int[] mins_d = { 1, 2, 3, 4 };
        public static int[] numbers_d = { 1 * 60, 2 * 60, 3 * 60, 4 * 60 };
        public static int disppear = 5;
        public static int disppear_min = 5;
        public static DispatcherTimer _timer;
        public static System.Timers.Timer aTimer;
        public static TimeSpan _time;
        //public static Stopwatch timer = new Stopwatch();
        public static string remindContent = "";

        public static DispatcherTimer _timer_break;
        public static TimeSpan _time_break;

        public static DispatcherTimer _timer_buff;
        public static TimeSpan _time_buff;


    }


}