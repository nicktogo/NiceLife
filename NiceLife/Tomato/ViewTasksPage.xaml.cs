﻿using NiceLife.Tomato.Database;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ViewTasksPage : Page
    {
        public List<Task> taskList = null;
        public DateTime date;
        Grid g_ViewTasksContent;

        public void fresh()
        {
            if(g_ViewTasksContent != null)
                g_ViewTasksContent.Children.Clear();
            if (taskList != null)
                taskList.Clear();
            
            taskList = TaskHelper.GetHelper().SelectGroupItemsByDate(dp_ViewDate.Date.DateTime);

            if (dp_ViewDate.Date.DateTime.ToString("yyyy-MM-dd") == DateTime.Now.Date.ToString("yyyy-MM-dd"))
                tb_IsToday.Visibility = Visibility.Visible;
            else
                tb_IsToday.Visibility = Visibility.Collapsed;

            g_ViewTasksContent = new Grid();
            for (int rowCount = 0; rowCount < taskList.Count/6+1; rowCount++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = new GridLength(120);
                g_ViewTasksContent.RowDefinitions.Add(rowdef);
            }
            for (int colCount = 0; colCount < 6; colCount++)
            {
                g_ViewTasksContent.ColumnDefinitions.Add(new ColumnDefinition());
            }
            sv_TasksContent.Content = g_ViewTasksContent;

            int count = 0;
            int i = 0;
            while (count < taskList.Count)
            {
                for (int j =0; j < 6; j++)
                {
                    Grid g_TaskContent = new Grid();
                    if (taskList.ElementAt(count).Status == "Done")
                        g_TaskContent.Background = new SolidColorBrush(Colors.LightSeaGreen);
                    else
                    {
                        if(taskList.ElementAt(count).Date.Date.CompareTo(DateTime.Now.Date) < 0)
                            g_TaskContent.Background = new SolidColorBrush(Colors.Gray);
                        else
                        {
                            g_TaskContent.Background = new SolidColorBrush(Colors.LightPink);
                            g_TaskContent.RightTapped += G_TaskContent_RightTapped;
                        }
                    } 
                    g_TaskContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                    g_TaskContent.VerticalAlignment = VerticalAlignment.Stretch;
                    g_TaskContent.Margin = new Thickness(5, 5, 5, 5);
                    g_TaskContent.RowDefinitions.Add(new RowDefinition());
                    g_TaskContent.RowDefinitions.Add(new RowDefinition());
                    g_TaskContent.RowDefinitions.Add(new RowDefinition());
                    g_TaskContent.ColumnDefinitions.Add(new ColumnDefinition());
                    g_TaskContent.ColumnDefinitions.Add(new ColumnDefinition());
                    g_ViewTasksContent.Children.Add(g_TaskContent);
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

                    g_TaskContent.Tag = count;
                    g_TaskContent.Tapped += G_TaskContent_Tapped;

                    count++;
                    if (count < taskList.Count)
                        continue;
                    else
                        break;
                }
                i++;
            }
        }

        private void G_TaskContent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Task task = taskList.ElementAt((int)((Grid)sender).Tag);
            this.Frame.Navigate(typeof(DoTaskPage), task);
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
            fresh();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            date = (DateTime)e.Parameter;
            dp_ViewDate.Date = date;
            fresh();
        }

        public ViewTasksPage()
        {
            this.InitializeComponent();

            fresh();
        }

        private void dp_ViewDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            fresh();
        }
    }
}
