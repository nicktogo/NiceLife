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
        DateTime now = DateTime.Today;
        string choose="";
        public ListPage()
        {
            this.InitializeComponent();
            time.Date = DateTime.Today;
           
           
        }

        private void appBarButton_Click(object sender, RoutedEventArgs e)
        {
            UsefulAlarm.Clear();
            choose = "doing";
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByDoing(now);
            for (int i = 0; i < list.Count(); i++) {
                UsefulAlarm.Add(new Alarm(i,list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate));
                    }
            listView1.DataContext = UsefulAlarm;
        }

        private void appBarButton1_Click(object sender, RoutedEventArgs e)
        {
            UsefulAlarm.Clear();
            choose = "done";
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByAgo(now);
            for (int i = 0; i < list.Count(); i++)
            {
                UsefulAlarm.Add(new Alarm(i+1,list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate));
            }
            listView1.DataContext = UsefulAlarm;
        }
        public void reflash()
        {
            PlanHelper ph = PlanHelper.GetHelper();
            
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
                UsefulAlarm.Add(new Alarm(i+1, list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate));
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
                    cp.DeleteSingleItemById(list.ElementAt(listView1.SelectedIndex).RemindId);
                }
                ph.DeleteSingleItemById(list.ElementAt(listView1.SelectedIndex).Id);
                list.RemoveAt(listView1.SelectedIndex);
                reflash();
            }
        }


        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {/*
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(listView1.SelectedIndex.ToString()));
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        */
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
            UsefulAlarm.Clear();
            choose = "undo";
            PlanHelper ph = PlanHelper.GetHelper();
            list = ph.SelectByFurture(now);
            for (int i = 0; i < list.Count(); i++)
            {
                UsefulAlarm.Add(new Alarm(i + 1, list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate));
            }
            listView1.DataContext = UsefulAlarm;
        }
    }
}
