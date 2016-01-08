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
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

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
        DateTime now = DateTime.Now;
        string choose="";
        public ListPage()
        {
            this.InitializeComponent();
            time.Date = DateTime.Today;
            appBarButton2.Visibility = Visibility.Collapsed;
            edit.Visibility = Visibility.Collapsed;
           
        }

        private void appBarButton_Click(object sender, RoutedEventArgs e)
        {
            UsefulAlarm.Clear();
            appBarButton2.Visibility = Visibility.Collapsed;
            edit.Visibility = Visibility.Collapsed;
            choose = "doing";
            ColorLable c;
            string color = "white";
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByDoing(now);
            for (int i = 0; i < list.Count(); i++) {
                ColorLableHelper clp = ColorLableHelper.GetHelper();
                c = clp.SelectSingleItemById(list.ElementAt(i).ColorId);
                if (c != null) color = c.Color;
                UsefulAlarm.Add(new Alarm(i+1,list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate,color));
                    }
            listView1.DataContext = UsefulAlarm;
        }

        private void appBarButton1_Click(object sender, RoutedEventArgs e)
        {
            UsefulAlarm.Clear();
            appBarButton2.Visibility = Visibility.Collapsed;
            edit.Visibility = Visibility.Collapsed;
            choose = "done";
            ColorLable c;
            string color = "white";
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByAgo(now);
            for (int i = 0; i < list.Count(); i++)
            {
                ColorLableHelper clp = ColorLableHelper.GetHelper();
                c = clp.SelectSingleItemById(list.ElementAt(i).ColorId);
                if (c != null) color = c.Color;
                UsefulAlarm.Add(new Alarm(i+1,list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate,color));
            }
            listView1.DataContext = UsefulAlarm;
        }
        public void reflash()
        {
            PlanHelper ph = PlanHelper.GetHelper();
            ColorLable c;
            string color = "white";
            UsefulAlarm.Clear();

            if (choose == "doing")
            {

                list = ph.SelectByDate(now);


            }else
            if(choose=="done")
            {

                list = ph.SelectByAgo(now);

            }
            else
            {
                list = ph.SelectByFurture(now);
            }
            for (int i = 0; i < list.Count(); i++)
            {
                ColorLableHelper clp = ColorLableHelper.GetHelper();
                c = clp.SelectSingleItemById(list.ElementAt(i).ColorId);
                if (c != null) color = c.Color;
                UsefulAlarm.Add(new Alarm(i+1, list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate,color));
            }
            listView1.DataContext = UsefulAlarm;
        }
        private void appBarButton2_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedIndex != -1)
            {
                PlanHelper ph = PlanHelper.GetHelper();
                if (list.ElementAt(listView1.SelectedIndex).IsRemind == 1)
                {
                    CallHelper cp = CallHelper.GetHelper();
                    cp.DeleteSingleItemById(list.ElementAt(listView1.SelectedIndex).Id);
                }
                ph.DeleteSingleItemById(list.ElementAt(listView1.SelectedIndex).Id);
                list.RemoveAt(listView1.SelectedIndex);
                reflash();
            }
        }


        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            }

        private void time_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            now = time.Date.Date;
            reflash();
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedIndex != -1)
            {

                Frame.Navigate(typeof(Add),list.ElementAt(listView1.SelectedIndex));
            }
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            appBarButton2.Visibility = Visibility.Visible;
            edit.Visibility = Visibility.Visible;
            UsefulAlarm.Clear();
            choose = "undo";
            ColorLable c;
            string color = "white";
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByFurture(now);
            for (int i = 0; i < list.Count(); i++)
            {
                ColorLableHelper clp = ColorLableHelper.GetHelper();
                c = clp.SelectSingleItemById(list.ElementAt(i).ColorId);
                if (c != null) color = c.Color;
                UsefulAlarm.Add(new Alarm(i + 1, list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate,color));
            }
            listView1.DataContext = UsefulAlarm;
        }
    }
}
