using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MonthPage : Page
    {
        DateTime current;
        public MonthPage()
        {
            this.InitializeComponent();
            current = DateTime.Now;
            showdate(current);

        }

        public void showdate(DateTime today)
        {
          //  choose.Date = current;
            calview.Children.Clear();
            Month.Text = Convert.ToString(today.Month);
            Year.Text = Convert.ToString(today.Year);
            DateTime day1 = new DateTime(today.Year, today.Month, 1);
            int week1 = (int)day1.DayOfWeek;
            int lastday = day1.AddMonths(1).AddDays(-1).Day;
            int lastmon; ;
            if (today.Month == 1 || today.Month == 3 || today.Month == 5 || today.Month == 7 ||
                 today.Month == 8 || today.Month == 10 || today.Month == 12)
            {
                lastmon = 31;
            }
            else
            {
                if (today.Month == 2)
                {
                    if (today.Year % 4 == 0)
                    {
                        lastmon = 29;
                    }
                    else
                    {
                        lastmon = 28;
                    }
                }
                else
                {
                    lastmon = 30;
                }
            }
            if (week1 > 0)
            {
                if (week1 != 7)
                {
                    for (int i = week1 - 1; i >= 0; i--)
                    {
                        //   Foreground = "#FF3AAFC9" />

                        Button b = new Button();
                        
                        b.FontSize = 52;
                        b.Foreground = new SolidColorBrush(Colors.DarkGray);
                        b.Background = null;
                        b.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                        b.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                        b.Content = Convert.ToString(lastmon);
                        lastmon--;
                        calview.Children.Add(b);
                        Grid.SetRow(b, 1);
                        Grid.SetColumn(b, i);


                    }
                }
            }
            int k = 1, kk = 1;
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if ((i == 1) && (j < week1)) continue;
                    if (k <= lastday)
                    {
                       
                        Button t = new Button();
                        t.Background = null;
                        t.FontSize = 52;
                        if (k == today.Day)
                        {
                            t.Foreground =new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            t.Foreground = new SolidColorBrush(Colors.Black);
                        }

                        t.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                        t.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                        t.Content = Convert.ToString(k);
                        calview.Children.Add(t);
                        Grid.SetRow(t, i);
                        Grid.SetColumn(t, j);
                        k++;
                    }
                    else
                    {
                        TextBlock t = new TextBlock();
                        t.FontSize = 52;

                        t.Foreground = new SolidColorBrush(Colors.DarkGray);
                        t.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                        t.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                        t.Text = Convert.ToString(kk);
                        calview.Children.Add(t);
                        Grid.SetRow(t, i);
                        Grid.SetColumn(t, j);
                        kk++;
                    }
                }

            }

        }

        private void appBarButton_last_Click(object sender, RoutedEventArgs e)
        {
            current = current.AddMonths(-1);
            showdate(current);
        }

        private void appBarButton_next_Click(object sender, RoutedEventArgs e)
        {
            current = current.AddMonths(1);
            showdate(current);
        }

        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            current = choose.Date.Value.UtcDateTime;
            showdate(current);
        }
    }
}
