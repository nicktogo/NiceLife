using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

using Windows.UI.Notifications;
using Windows.ApplicationModel.Background;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife.Schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DayPage : Page
    {
        DateTime current;
        List<Plan> list;
        private const string PLANTASK = "PlanTask";
        ObservableCollection<Alarm> UsefulAlarm = new ObservableCollection<Alarm>();
        public DayPage()
        {
            this.InitializeComponent();
            current = DateTime.Today;
            choose.Date = current.Date;
            
            showtime(current);
           
        }
  
        public void showtime(DateTime now)
        {
           
            
            current = now;
            PlanHelper ph = PlanHelper.GetHelper();
            ColorLable c;
            string color="White";
            list = ph.SelectByDate(now);
            for (int i = 0; i < list.Count(); i++)
            {
                ColorLableHelper clp = ColorLableHelper.GetHelper();
                c = clp.SelectSingleItemById(list.ElementAt(i).ColorId);
                if (c != null) color=c.Color;
                UsefulAlarm.Add(new Alarm(i, list.ElementAt(i).Title, list.ElementAt(i).Description, list.ElementAt(i).BeginDate, list.ElementAt(i).EndDate,color));
            }
            listView1.DataContext = UsefulAlarm;

        }
        



        private void choose_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            showtime(choose.Date.DateTime);
        }
    }
}
