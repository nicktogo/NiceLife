using NiceLife.Tomato.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DoTaskPage : Page
    {
        CountDown CountDownClock;

        public DoTaskPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Task task = (Task)e.Parameter;
            tb_Title.Text = task.Title;
            tb_Description.Text = task.Description;

            CountDownClock = new CountDown(task);
            CountDownClock.HorizontalAlignment = HorizontalAlignment.Left;
            CountDownClock.VerticalAlignment = VerticalAlignment.Center;
            CountDownClock.Margin = new Thickness(50, 30, 0, 30);
            g_DoTask.Children.Add(CountDownClock);
            Grid.SetRow(CountDownClock, 2);

            if (task.Date.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd") || task.Status == "Done")
            {
                btn_NewInterrupt.IsEnabled = false;
            }
        }

        private void btn_NewInterruptConfirm_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task();
            task.Title = tb_InterruptTitle.Text;
            task.Description = tb_InterruptDescription.Text;
            task.Date = DateTime.Now;
            if (rb_Internal.IsChecked == true)
            {
                task.Type = "Internal";
            }
            else
            {
                task.Type = "External";
            }
            task.Status = "Undone";
            task.TotalTomato = cb_InterruptTomato.SelectedIndex + 1;
            task.DoneTomato = 0;
            TaskHelper.GetHelper().InsertSingleItem(task);

            tb_Title.Text = "";
            tb_Description.Text = "";
            rb_Internal.IsChecked = true;
            cb_InterruptTomato.SelectedIndex = 0;

            this.Frame.Navigate(typeof(DoTaskPage), task);
            //fo_NewInterrupt.Hide();
        }
    }
}
