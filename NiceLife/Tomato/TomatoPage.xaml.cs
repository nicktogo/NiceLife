using NiceLife.Tomato.Database;
using SQLitePCL;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NiceLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TomatoPage : Page
    {
        public void fresh()
        {
            List<Task> taskList = TaskHelper.GetHelper().SelectGroupItemsByDate(DateTime.Now);

            int count = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                {
                    if (count < taskList.Count && count != 7)
                    {
                        Grid g_TaskContent = new Grid();
                        g_TaskContent.Background = new SolidColorBrush(Colors.Gray);
                        g_TaskContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                        g_TaskContent.VerticalAlignment = VerticalAlignment.Stretch;
                        g_TaskContent.Margin = new Thickness(5, 5, 5, 5);
                        g_TaskContent.RowDefinitions.Add(new RowDefinition());
                        g_TaskContent.RowDefinitions.Add(new RowDefinition());
                        g_TaskContent.RowDefinitions.Add(new RowDefinition());
                        g_TaskContent.ColumnDefinitions.Add(new ColumnDefinition());
                        g_TaskContent.ColumnDefinitions.Add(new ColumnDefinition());
                        g_TodayTasksContent.Children.Add(g_TaskContent);
                        Grid.SetRow(g_TaskContent, i);
                        Grid.SetColumn(g_TaskContent, j);

                        TextBlock tb_TaskContentTitle = new TextBlock();
                        tb_TaskContentTitle.Text = taskList.ElementAt(count).Title;
                        tb_TaskContentTitle.FontSize = 30;
                        tb_TaskContentTitle.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentTitle);
                        Grid.SetRow(tb_TaskContentTitle, 0);
                        Grid.SetColumnSpan(tb_TaskContentTitle, 2);
                        Grid.SetColumn(tb_TaskContentTitle, 0);

                        TextBlock tb_TaskContentTotalTomato = new TextBlock();
                        tb_TaskContentTotalTomato.Text = "Total🍅:" + taskList.ElementAt(count).TotalTomato;
                        tb_TaskContentTotalTomato.FontSize = 15;
                        tb_TaskContentTotalTomato.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentTotalTomato);
                        Grid.SetRow(tb_TaskContentTotalTomato, 1);
                        Grid.SetColumn(tb_TaskContentTotalTomato, 0);
                        Grid.SetColumnSpan(tb_TaskContentTotalTomato, 2);

                        TextBlock tb_TaskContentDoneTomato = new TextBlock();
                        tb_TaskContentDoneTomato.Text = "Done🍅:" + taskList.ElementAt(count).DoneTomato;
                        tb_TaskContentDoneTomato.FontSize = 10;
                        tb_TaskContentDoneTomato.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentDoneTomato);
                        Grid.SetRow(tb_TaskContentDoneTomato, 2);
                        Grid.SetColumn(tb_TaskContentDoneTomato, 0);

                        TextBlock tb_TaskContentUndoneTomato = new TextBlock();
                        tb_TaskContentUndoneTomato.Text = "Undone🍅:" + (taskList.ElementAt(count).TotalTomato - taskList.ElementAt(count).DoneTomato);
                        tb_TaskContentUndoneTomato.FontSize = 10;
                        tb_TaskContentUndoneTomato.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentUndoneTomato);
                        Grid.SetRow(tb_TaskContentUndoneTomato, 2);
                        Grid.SetColumn(tb_TaskContentUndoneTomato, 1);

                        g_TaskContent.Tapped += G_TaskContent_Tapped;

                        count++;
                    }
                    else if (taskList.Count > 7 && count == 7)
                    {
                        Grid g_TaskContent = new Grid();
                        g_TaskContent.Background = new SolidColorBrush(Colors.Gray);
                        g_TaskContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                        g_TaskContent.VerticalAlignment = VerticalAlignment.Stretch;
                        g_TaskContent.Margin = new Thickness(5, 5, 5, 5);
                        g_TodayTasksContent.Children.Add(g_TaskContent);
                        Grid.SetRow(g_TaskContent, i);
                        Grid.SetColumn(g_TaskContent, j);

                        TextBlock tb_MoreTasks = new TextBlock();
                        tb_MoreTasks.Text = "...";
                        tb_MoreTasks.FontSize = 50;
                        tb_MoreTasks.HorizontalAlignment = HorizontalAlignment.Center;
                        tb_MoreTasks.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_MoreTasks);

                        g_TaskContent.Tapped += G_TaskContent_Tapped_More;
                    }
                    else break;
                }
        }

        public TomatoPage()
        {
            this.InitializeComponent();

            fresh();
        }

        private void G_TaskContent_Tapped_More(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AllTasksPage));
        }

        private void G_TaskContent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DoTaskPage));
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
        private void NewTaskConfirm_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task();
            task.Title = tb_Title.Text;
            task.Description = tb_Description.Text;
            task.Date = dp_Date.Date.DateTime;
            task.Type = "Normal";
            task.Status = "Undone";
            task.TotalTomato = cb_Tomato.SelectedIndex+1;
            task.DoneTomato = 0;
            TaskHelper.GetHelper().InsertSingleItem(task);

            tb_Title.Text = "";
            tb_Description.Text = "";
            dp_Date.Date = DateTime.Now;
            cb_Tomato.SelectedIndex = 0;

            fresh();

            fo_NewTask.Hide();
        }        
    }
}
