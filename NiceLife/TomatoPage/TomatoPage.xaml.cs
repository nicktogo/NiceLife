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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NiceLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TomatoPage : Page
    {
        public TomatoPage()
        {
            this.InitializeComponent();
        }

        private void TodayTasks_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TodayTasksPage));
        }
        private void AllTasks_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AllTasksPage));
        }
        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StatisticsPage));
        }
        private void History_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HistoryPage));
        }
    }
}
