using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NiceLife
{
    public partial class CountDown : UserControl
    {
        public DispatcherTimer timer;
        public Process pro;
        public Stopwatch sw = new Stopwatch();
        public int seconds;

        public CountDown()
        {
            InitializeComponent();
            pro = new Process();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            TimeSpan ts = new TimeSpan(0, 0, seconds);
            pro.totalSecond = (int)(ts - sw.Elapsed).TotalSeconds;
            if (pro.totalSecond > 0)
            {
                HourArea.Text = pro.GetHour();
                MinuteArea.Text = pro.GetMinute();
                SecondArea.Text = pro.GetSecond();
            }
            else
            {
                timer.Stop();
                sw.Stop();
                sw.Reset();
                SecondArea.Text = string.Format("{0:D2}", 0);
            }
        }

    }
    
    public class Process
    {
        public int totalSecond;

        //获取小时字符串
        public string GetHour()
        {
            return string.Format("{0:D2}", totalSecond / 3600);
        }

        //获取分钟字符串
        public string GetMinute()
        {
            return string.Format("{0:D2}", (totalSecond / 60 - ((int)(totalSecond / 3600) * 60)));
        }

        //获取秒字符串
        public string GetSecond()
        {
            return string.Format("{0:D2}", totalSecond % 60);
        }
    }
}
