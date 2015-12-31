using NiceLife.Tomato.Database;
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
    public sealed partial class AllTasksPage : Page
    {
        public List<Task> allTaskList = null;
        public List<DateTime> allDateList = null;

        public void fresh()
        {
            if (allTaskList != null)
                allTaskList.Clear();
            if (allDateList != null)
                allDateList.Clear();

            allTaskList = TaskHelper.GetHelper().SelectAllItems();
            allDateList = TaskHelper.GetHelper().SelectAllDate();

            Grid g_AllTasksContent = new Grid();
            for (int rowCount = 0; rowCount < (allTaskList.Count+allDateList.Count) / 8 + 1; rowCount++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = new GridLength(120);
                g_AllTasksContent.RowDefinitions.Add(rowdef);
            }
            for (int colCount = 0; colCount < 8; colCount++)
            {
                g_AllTasksContent.ColumnDefinitions.Add(new ColumnDefinition());
            }
            sv_AllTasksContent.Content = g_AllTasksContent;

            int totalCount = 0;
            int taskCount = 0;
            bool lastIsDate = false;
            int i = 0;
            while (totalCount < allTaskList.Count+allDateList.Count)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(totalCount == 0)
                    {
                        Grid g_DateContent = new Grid();
                        g_DateContent.Background = new SolidColorBrush(Colors.Gray);
                        g_DateContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                        g_DateContent.VerticalAlignment = VerticalAlignment.Stretch;
                        g_DateContent.Margin = new Thickness(5, 5, 5, 5);
                        g_AllTasksContent.Children.Add(g_DateContent);
                        Grid.SetRow(g_DateContent, i);
                        Grid.SetColumn(g_DateContent, j);

                        TextBlock tb_DateContent = new TextBlock();
                        tb_DateContent.Text = allTaskList.ElementAt(taskCount).Date.ToString("yyyy-MM-dd");
                        tb_DateContent.FontSize = 20;
                        tb_DateContent.VerticalAlignment = VerticalAlignment.Center;
                        tb_DateContent.HorizontalAlignment = HorizontalAlignment.Center;
                        g_DateContent.Children.Add(tb_DateContent);

                        g_DateContent.Tag = allTaskList.ElementAt(taskCount).Date;
                        g_DateContent.Tapped += G_DateContent_Tapped;

                        totalCount++;
                        continue;
                    }
                    else
                    {
                        if(taskCount != 0)
                        {
                            if (allTaskList.ElementAt(taskCount).Date.ToString("yyyy-MM-dd") != allTaskList.ElementAt(taskCount - 1).Date.ToString("yyyy-MM-dd") && lastIsDate == false)
                            {
                                Grid g_DateContent = new Grid();
                                g_DateContent.Background = new SolidColorBrush(Colors.Gray);
                                g_DateContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                                g_DateContent.VerticalAlignment = VerticalAlignment.Stretch;
                                g_DateContent.Margin = new Thickness(5, 5, 5, 5);
                                g_AllTasksContent.Children.Add(g_DateContent);
                                Grid.SetRow(g_DateContent, i);
                                Grid.SetColumn(g_DateContent, j);

                                TextBlock tb_DateContent = new TextBlock();
                                tb_DateContent.Text = allTaskList.ElementAt(taskCount).Date.ToString("yyyy-MM-dd");
                                tb_DateContent.FontSize = 20;
                                tb_DateContent.VerticalAlignment = VerticalAlignment.Center;
                                tb_DateContent.HorizontalAlignment = HorizontalAlignment.Center;
                                g_DateContent.Children.Add(tb_DateContent);

                                g_DateContent.Tag = allTaskList.ElementAt(taskCount).Date;
                                g_DateContent.Tapped += G_DateContent_Tapped;

                                lastIsDate = true;

                                totalCount++;
                                continue;
                            }
                        }
                    }
                    
                    
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
                    g_AllTasksContent.Children.Add(g_TaskContent);
                    Grid.SetRow(g_TaskContent, i);
                    Grid.SetColumn(g_TaskContent, j);

                    TextBlock tb_TaskContentTitle = new TextBlock();
                    tb_TaskContentTitle.Text = allTaskList.ElementAt(taskCount).Title;
                    tb_TaskContentTitle.FontSize = 20;
                    tb_TaskContentTitle.VerticalAlignment = VerticalAlignment.Center;
                    g_TaskContent.Children.Add(tb_TaskContentTitle);
                    Grid.SetRow(tb_TaskContentTitle, 0);
                    Grid.SetColumnSpan(tb_TaskContentTitle, 2);
                    Grid.SetColumn(tb_TaskContentTitle, 0);

                    TextBlock tb_TaskContentTotalTomato = new TextBlock();
                    tb_TaskContentTotalTomato.Text = "总🍅：" + allTaskList.ElementAt(taskCount).TotalTomato;
                    tb_TaskContentTotalTomato.FontSize = 15;
                    tb_TaskContentTotalTomato.VerticalAlignment = VerticalAlignment.Center;
                    g_TaskContent.Children.Add(tb_TaskContentTotalTomato);
                    Grid.SetRow(tb_TaskContentTotalTomato, 1);
                    Grid.SetColumn(tb_TaskContentTotalTomato, 0);
                    Grid.SetColumnSpan(tb_TaskContentTotalTomato, 2);

                    TextBlock tb_TaskContentDoneTomato = new TextBlock();
                    tb_TaskContentDoneTomato.Text = "已完成🍅：" + allTaskList.ElementAt(taskCount).DoneTomato;
                    tb_TaskContentDoneTomato.FontSize = 10;
                    tb_TaskContentDoneTomato.VerticalAlignment = VerticalAlignment.Center;
                    g_TaskContent.Children.Add(tb_TaskContentDoneTomato);
                    Grid.SetRow(tb_TaskContentDoneTomato, 2);
                    Grid.SetColumn(tb_TaskContentDoneTomato, 0);

                    TextBlock tb_TaskContentUndoneTomato = new TextBlock();
                    tb_TaskContentUndoneTomato.Text = "未完成🍅：" + (allTaskList.ElementAt(taskCount).TotalTomato - allTaskList.ElementAt(taskCount).DoneTomato);
                    tb_TaskContentUndoneTomato.FontSize = 10;
                    tb_TaskContentUndoneTomato.VerticalAlignment = VerticalAlignment.Center;
                    g_TaskContent.Children.Add(tb_TaskContentUndoneTomato);
                    Grid.SetRow(tb_TaskContentUndoneTomato, 2);
                    Grid.SetColumn(tb_TaskContentUndoneTomato, 1);

                    g_TaskContent.Tag = taskCount;
                    g_TaskContent.Tapped += G_TaskContent_Tapped;

                    lastIsDate = false;

                    totalCount++;
                    taskCount++;
                    if (totalCount < allTaskList.Count+allDateList.Count)
                        continue;
                    else
                        break;
                }
                i++;
            }
        }

        private void G_DateContent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DateTime date = (DateTime)((Grid)sender).Tag;
            this.Frame.Navigate(typeof(ViewTasksPage), date);
        }

        private void G_TaskContent_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Task task = allTaskList.ElementAt((int)((Grid)sender).Tag);
            this.Frame.Navigate(typeof(DoTaskPage), task);
        }

        public AllTasksPage()
        {
            this.InitializeComponent();

            fresh();
        }
    }
}
