using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NiceLife.Schedule.db;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife.Schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListPage : Page
    {
        public
            ObservableCollection<Alarm> UsefulAlarm = new ObservableCollection<Alarm>();
        List<Plan> list;
        DateTime now = DateTime.Today;
        public ListPage()
        {
            this.InitializeComponent();
            time.Date = DateTime.Today;
           
           
        }

        private void appBarButton_Click(object sender, RoutedEventArgs e)
        {
            UsefulAlarm.Clear();
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByDate(now);
            for (int i = 0; i < list.Count(); i++) {
                UsefulAlarm.Add(new Alarm(list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate));
                    }
            listView1.DataContext = UsefulAlarm;
        }

        private void appBarButton1_Click(object sender, RoutedEventArgs e)
        {
            UsefulAlarm.Clear();
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByAgo(now);
            for (int i = 0; i < list.Count(); i++)
            {
                UsefulAlarm.Add(new Alarm(list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate));
            }
            listView1.DataContext = UsefulAlarm;
        }

        private void appBarButton2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void time_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            now = time.Date.DateTime;
        }
    }
}
