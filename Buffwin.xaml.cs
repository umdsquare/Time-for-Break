using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace TimeforBreak
{
    /// <summary>
    /// Interaction logic for Buffwin.xaml
    /// </summary>
    public partial class Buffwin : Window
    {
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
        public Buffwin()
        {
            InitializeComponent();
            this.Topmost = true;
            this.Activate();

            //Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            //{
            //    var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            //    var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            //    var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

            //    this.Left = (workingArea.Right/2) - (this.Width/2);
            //    this.Top = workingArea.Top;
            //    this.Activate();
            //}));

            user._time_buff = TimeSpan.FromSeconds(9);

            user._timer_buff = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //this.Activate();
                label2.Content = (user._time_buff.ToString()).Substring(7, 1);

                if ((user._time_buff.ToString()).Substring(7, 1).Equals("1"))
                {
                    label3.Content = "second to wrap up your work";
                }
                else
                {
                    label3.Content = "seconds to wrap up your work";
                }

                if (IsAnyKeyDown())
                {
                    Trace.WriteLine("Keyboard is active");

                }

                if (user._time_buff == TimeSpan.Zero)
                {
                    user._timer_buff.Stop();
                    Breakmode breakmodeWin = new Breakmode();
                    breakmodeWin.Show();

                    this.Close();
                }
                else
                {
                }

                user._time_buff = user._time_buff.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
        }
    }
}
