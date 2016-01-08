using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NiceLife.Tomato.Database;
using NiceLife.Tomato;

namespace NiceLife
{
    public partial class CountDown : UserControl
    {
        public DispatcherTimer timer;
        public Process pro;
        public Stopwatch sw = new Stopwatch();
        public int seconds;

        public Task task;
        int tomatoDoneCount = 0;
        string periodNow = "Tomato";
        string btnStatus = "Start";

        public void startTomato()
        {
            startCountDown(1500);//实际为1500
        }

        public void startShortRest()
        {
            startCountDown(300);//实际为300
        }

        public void startLongRest()
        {
            startCountDown(900);//实际为900
        }

        public void startCountDown(int seconds)
        {
            this.timer.Stop();
            this.sw.Reset();
            this.seconds = seconds;
            this.timer.Start();
            this.sw.Start();
        }

        public void periodDone()
        {
            
            if (this.pro.GetMinute() == "00" && this.pro.GetSecond() == "00")
            {
                if (periodNow == "Tomato")
                {

                    tomatoDoneCount++;
                    task.DoneTomato++;
                    TaskHelper.GetHelper().UpdateDoneTomato(task);
                    if(task.DoneTomato == task.TotalTomato)
                    {
                        task.Status = "Done";
                        TaskHelper.GetHelper().UpdateStatus(task);
                        timer.Stop();
                        sw.Stop();
                        sw.Reset();
                        SetTime(0);
                        btn_StartAndStop.Content = "任务完成";
                        btn_StartAndStop.IsEnabled = false;
                    }
                    else
                    {
                        if (tomatoDoneCount % 4 != 0)
                        {
                            startShortRest();
                            periodNow = "ShortRest";
                        }
                        else
                        {
                            startLongRest();
                            periodNow = "LongRest";
                        }
                    }
                    freshTomato();
                }
                else
                {
                    startTomato();
                    periodNow = "Tomato";
                }
            }
        }

        public void SetTime(int totalSecond)
        {
            MinuteArea.Text = string.Format("{0:D2}", (totalSecond / 60 - ((int)(totalSecond / 3600) * 60)));
            SecondArea.Text = string.Format("{0:D2}", totalSecond % 60);
        }

        public void freshTomato()
        {
            tb_DoneTomato.Text = "已完成🍅：" + task.DoneTomato.ToString();
            tb_UndoneTomato.Text = "未完成🍅：" + (task.TotalTomato - task.DoneTomato).ToString();
        }

        public CountDown(Task _task)
        {
            InitializeComponent();
            
            this.task = _task;
            pro = new Process();
            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;

            SetTime(1500);//实际为1500

            tb_TotalTomato.Text = "总🍅：" + task.TotalTomato.ToString();
            tb_DoneTomato.Text = "已完成🍅：" + task.DoneTomato.ToString();
            tb_UndoneTomato.Text = "未完成🍅：" + (task.TotalTomato - task.DoneTomato).ToString();

            if(task.Date.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                btn_StartAndStop.Content = "非今日任务";
                btn_StartAndStop.IsEnabled = false;
            }
            if(task.Status == "Done")
            {
                btn_StartAndStop.Content = "任务完成";
                btn_StartAndStop.IsEnabled = false;
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            if(periodNow == "ShortRest")
                Period.Text = "小憩时间";
            else if(periodNow == "LongRest")
                Period.Text = "休息时间";
            else
                Period.Text = "番茄时间";

            TimeSpan ts = new TimeSpan(0, 0, seconds);
            pro.totalSecond = (int)(ts - sw.Elapsed).TotalSeconds;
            if (pro.totalSecond > 0)
            {
                MinuteArea.Text = pro.GetMinute();
                SecondArea.Text = pro.GetSecond();
            }
            else
            {
                timer.Stop();
                sw.Stop();
                sw.Reset();
                SecondArea.Text = string.Format("{0:D2}", 0);
                periodDone();
            }
        }

        private void btn_StartAndStop_Click(object sender, RoutedEventArgs e)
        {

            if (btnStatus == "Start")
            {
                btn_StartAndStop.Content = "停止";
                btnStatus = "Stop";

                if (task.Status == "Undone")
                {
                    startTomato();
                }
            }
            else
            {
                btn_StartAndStop.Content = "开始";
                btnStatus = "Start";

                timer.Stop();
                sw.Reset();
                SetTime(1500);//实际为1500
                Period.Text = "番茄时间";
            }

        }
    }
    
    public class Process
    {
        public int totalSecond;


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
