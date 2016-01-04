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
        List<Plan> infor;
        public MainPage()
        {
            this.InitializeComponent();
           // check();
        }
        public void check()
        {
            while (true)
            {
                CallHelper callp =CallHelper.GetHelper();
                call = callp.SelectlistItems(DateTime.Now);
                for(int i = 0; i < call.Count; i++)
                {
                    PlanHelper ph = PlanHelper.GetHelper();
                    infor = ph.SelectGroupItems(call.ElementAt(i).Id);
                }
                for (int i = 0; i < call.Count; i++)
                {
                    if (call.ElementAt(i).Date == DateTime.Now)
                    {
                        ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                        XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                        XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                        toastTextElements[0].AppendChild(toastXml.CreateTextNode(infor.ElementAt(i).Title+":"+ infor.ElementAt(i).Description));
                        ToastNotification toast = new ToastNotification(toastXml);
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
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
            bool b = counties.Count > 0 ? MainFrame.Navigate(typeof(Weather.WeatherFlipView)) : MainFrame.Navigate(typeof(WeatherPage));
        }
    }
}
