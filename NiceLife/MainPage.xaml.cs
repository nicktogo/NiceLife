using NiceLife.Weather.Database;
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
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NiceLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Call> call;
       
        public MainPage()
        {
           
            this.InitializeComponent();
            check();
        }
        public  void check()
        {
          //  Schedule.db.CreateDb.LoadDatabase();
                CallHelper callp =CallHelper.GetHelper();
                call = callp.SelectlistItems(DateTime.Now);
            for (int i = 0; i < call.Count; i++)
            {
                PlanHelper ph = PlanHelper.GetHelper();
                Plan plan = ph.SelectSingleItemById(call.ElementAt(i).PlanId);

                    DateTime toastTime;
                    try
                    {
                        toastTime =call.ElementAt(i).Date;
                        XmlDocument xdoc = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

                        var txtnodes = xdoc.GetElementsByTagName("text");
                        txtnodes[0].InnerText = plan.Title+":"+ plan.Description;
                       
                        ScheduledToastNotification toast3 = new ScheduledToastNotification(xdoc, toastTime);
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast3);
                    }
                    catch (Exception ex)
                    {
                       //
                       
                    }
                
            }
            
        }
        private void BackRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void HamburgerRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.SplitView.IsPaneOpen = !this.SplitView.IsPaneOpen;
        }

        private void KeepRadioButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(KeepPage));
        }

        private void ScheduleRadioButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(SchedulePage));
        }

        private void TomatoRadioButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(TomatoPage));
        }

        private void WeatherRadioButton_Click(object sender, RoutedEventArgs e)
        {
            CountyHelper countyHelper = CountyHelper.GetHelper();
            List<County> counties = countyHelper.GetSelectedItems();
            bool b = counties.Count > 0 ? MainFrame.Navigate(typeof(Weather.WeatherFlipView), Weather.WeatherFlipView.NAVIGATED_FROM_MAINPAGE) : MainFrame.Navigate(typeof(WeatherPage));
        }

        private void KeepRadioButton_Checked(System.Object sender, RoutedEventArgs e)
        {

        }

        private void KeepRadioButton_Checked(System.Object sender, RoutedEventArgs e)
        {

        }
    }
}
