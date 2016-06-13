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
        List<Task> taskList = null;
        List<DateTime> dateList = null;

        public void TodayTasksFresh()
        {
            g_TodayTasksContent.Children.Clear();
            if (taskList != null)
                taskList.Clear();
            taskList = TaskHelper.GetHelper().SelectGroupItemsByDate(DateTime.Now);

            int count = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                {
                    if (count < taskList.Count && count != 7)
                    {
                        Grid g_TaskContent = new Grid();

                        g_TaskContent.Tag = count;
                        g_TaskContent.Tapped += G_TaskContent_Tapped;

                        if (taskList.ElementAt(count).Status == "Done")
                            g_TaskContent.Background = new SolidColorBrush(Colors.LightSeaGreen);
                        else
                        {
                            g_TaskContent.Background = new SolidColorBrush(Colors.LightPink);
                            g_TaskContent.RightTapped += G_TaskContent_RightTapped;
                        }
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
                        tb_TaskContentTitle.FontSize = 20;
                        tb_TaskContentTitle.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentTitle);
                        Grid.SetRow(tb_TaskContentTitle, 0);
                        Grid.SetColumnSpan(tb_TaskContentTitle, 2);
                        Grid.SetColumn(tb_TaskContentTitle, 0);

                        TextBlock tb_TaskContentTotalTomato = new TextBlock();
                        tb_TaskContentTotalTomato.Text = "Total🍅：" + taskList.ElementAt(count).TotalTomato;
                        tb_TaskContentTotalTomato.FontSize = 15;
                        tb_TaskContentTotalTomato.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentTotalTomato);
                        Grid.SetRow(tb_TaskContentTotalTomato, 1);
                        Grid.SetColumn(tb_TaskContentTotalTomato, 0);
                        Grid.SetColumnSpan(tb_TaskContentTotalTomato, 2);

                        TextBlock tb_TaskContentDoneTomato = new TextBlock();
                        tb_TaskContentDoneTomato.Text = "Done🍅：" + taskList.ElementAt(count).DoneTomato;
                        tb_TaskContentDoneTomato.FontSize = 10;
                        tb_TaskContentDoneTomato.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentDoneTomato);
                        Grid.SetRow(tb_TaskContentDoneTomato, 2);
                        Grid.SetColumn(tb_TaskContentDoneTomato, 0);

                        TextBlock tb_TaskContentUndoneTomato = new TextBlock();
                        tb_TaskContentUndoneTomato.Text = "Undone🍅：" + (taskList.ElementAt(count).TotalTomato - taskList.ElementAt(count).DoneTomato);
                        tb_TaskContentUndoneTomato.FontSize = 10;
                        tb_TaskContentUndoneTomato.VerticalAlignment = VerticalAlignment.Center;
                        g_TaskContent.Children.Add(tb_TaskContentUndoneTomato);
                        Grid.SetRow(tb_TaskContentUndoneTomato, 2);
                        Grid.SetColumn(tb_TaskContentUndoneTomato, 1);

                        

                        count++;
                    }
                    else if (taskList.Count > 7 && count == 7)
                    {
                        Grid g_TaskContent = new Grid();
                        g_TaskContent.Background = new SolidColorBrush(Colors.LightPink);
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

        private void G_TaskContent_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            MenuFlyout mfo = new MenuFlyout();
            MenuFlyoutItem mfoItem = new MenuFlyoutItem();
            mfoItem.Text = "Cancel";
            mfoItem.Click += deleteTask;
            mfoItem.Tag = taskList.ElementAt((int)(((Grid)sender).Tag));
            mfo.Items.Add(mfoItem);
            mfo.ShowAt((Grid)sender);
        }

        private void deleteTask(object sender, RoutedEventArgs e)
        {
            TaskHelper.GetHelper().DeleteSingleItem((Task)(((MenuFlyoutItem)sender).Tag));
            TodayTasksFresh();
            AllTasksFresh();
        }

        public void AllTasksFresh()
        {
            g_AllTasksContent.Children.Clear();
            if (dateList != null)
                dateList.Clear();
            dateList = TaskHelper.GetHelper().SelectDate();

            int count = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                {
                    if (count < dateList.Count && count != 7)
                    {
                        Grid g_DateContent = new Grid();
                        g_DateContent.Background = new SolidColorBrush(Colors.LightPink);
                        g_DateContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                        g_DateContent.VerticalAlignment = VerticalAlignment.Stretch;
                        g_DateContent.Margin = new Thickness(5, 5, 5, 5);
                        g_AllTasksContent.Children.Add(g_DateContent);
                        Grid.SetRow(g_DateContent, i);
                        Grid.SetColumn(g_DateContent, j);

                        TextBlock tb_DateContent = new TextBlock();
                        tb_DateContent.Text = dateList.ElementAt(count).Date.ToString("yyyy-MM-dd");
                        tb_DateContent.FontSize = 20;
                        tb_DateContent.VerticalAlignment = VerticalAlignment.Center;
                        tb_DateContent.HorizontalAlignment = HorizontalAlignment.Center;
                        g_DateContent.Children.Add(tb_DateContent);

                        g_DateContent.Tag = dateList.ElementAt(count).Date;
                        g_DateContent.Tapped += G_DateContent_Tapped;

                        count++;
                    }
                    else if (dateList.Count > 7 && count == 7)
                    {
                        Grid g_DateContent = new Grid();
                        g_DateContent.Background = new SolidColorBrush(Colors.LightPink);
                        g_DateContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                        g_DateContent.VerticalAlignment = VerticalAlignment.Stretch;
                        g_DateContent.Margin = new Thickness(5, 5, 5, 5);
                        g_AllTasksContent.Children.Add(g_DateContent);
                        Grid.SetRow(g_DateContent, i);
                        Grid.SetColumn(g_DateContent, j);

                        TextBlock tb_MoreDate = new TextBlock();
                        tb_MoreDate.Text = "...";
                        tb_MoreDate.FontSize = 50;
                        tb_MoreDate.HorizontalAlignment = HorizontalAlignment.Center;
                        tb_MoreDate.VerticalAlignment = VerticalAlignment.Center;
                        g_DateContent.Children.Add(tb_MoreDate);

                        g_DateContent.Tapped += G_DateContent_Tapped_More;
                    }
                    else break;
                }
        }

        public TomatoPage()
        {
            this.InitializeComponent();

            TodayTasksFresh();
            AllTasksFresh();
            //tb_Date.Text = dp_Date.Date.ToString("yyyy-MM-dd");
            tb_Date.Text=DateTime.Today.ToString("yyyy-MM-dd");
            tb_Date.DataContextChanged += Tb_Date_DataContextChanged;
        }

        private void Tb_Date_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            //tb_Date.Text = dp_Date.Date.ToString("yyyy-MM-dd");
        }

        private void G_TaskContent_Tapped_More(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewTasksPage), DateTime.Now);
        }

        private void G_TaskContent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Task task = taskList.ElementAt((int)((Grid)sender).Tag);
            this.Frame.Navigate(typeof(DoTaskPage), task);
        }

        private void G_DateContent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DateTime date = (DateTime)((Grid)sender).Tag;
            this.Frame.Navigate(typeof(ViewTasksPage), date);
        }

        private void G_DateContent_Tapped_More(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AllTasksPage), dateList);
        }

        private void TodayTasks_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewTasksPage), DateTime.Now);
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
            //task.Date = dp_Date.Date.DateTime;
            task.Date = DateTime.Today;
            task.Type = "Normal";
            task.Status = "Undone";
            task.TotalTomato = cb_Tomato.SelectedIndex+1;
            task.DoneTomato = 0;
            TaskHelper.GetHelper().InsertSingleItem(task);

            tb_Title.Text = "";
            tb_Description.Text = "";
            //dp_Date.Date = DateTime.Now;
            cb_Tomato.SelectedIndex = 0;

            TodayTasksFresh();

            fo_NewTask.Hide();
        }        
    }
}
