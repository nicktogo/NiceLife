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
    public sealed partial class HistoryPage : Page
    {
        public List<Task> taskList = null;
        public HistoryPage()
        {
            this.InitializeComponent();
            int totaltask = TaskHelper.GetHelper().numOfAllTask();
            int donetask = TaskHelper.GetHelper().numOfDoneTask();
            int undonetask = totaltask - donetask;

            int tomatototal = TaskHelper.GetHelper().numOfAllTomato();
            int tomatodone = TaskHelper.GetHelper().numOfDoneTomato();
            int tomatoundone = tomatototal - tomatodone;

            numofall.Text = totaltask.ToString();
            done.Text = donetask.ToString();
            undone.Text = undonetask.ToString();

            totaltomato.Text = tomatototal.ToString();
            donetomato.Text = tomatodone.ToString();
            undonetomato.Text = tomatoundone.ToString();
            fresh(DateTime.Now);


        }


        private void dp_ViewDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            fresh(dp_ViewDate.Date.DateTime);
        }

        public void fresh(DateTime date)
        {
            int alltoday = TaskHelper.GetHelper().numOfAllTaskByDate(date);
            int donetoday = TaskHelper.GetHelper().numOfDoneTaskByDate(date);
            int undonetoday = alltoday - donetoday;

            int alltomatotoday = TaskHelper.GetHelper().numOfAllTomatoByDate(date);
            int donetomatotoday = TaskHelper.GetHelper().numOfDoneTomatoByDate(date);
            int undonetomatotoday = alltomatotoday - donetomatotoday;

            todayall.Text = alltoday.ToString();
            todaydown.Text = donetoday.ToString();
            todayundone.Text = undonetoday.ToString();

            todayalltomato.Text = alltomatotoday.ToString();
            todaydonetomato.Text = donetomatotoday.ToString();
            todayundonetomato.Text = undonetomatotoday.ToString();
            int rate;
            if (donetomatotoday == 0)
            {
                rate = 0;
            }
            else rate = alltomatotoday / donetomatotoday;

            switch (rate)
            {
                case 1:
                    stars.Text = "🍅🍅🍅🍅🍅";
                    comments.Text = "Outstanding！";
                    break;
                case 2:
                    stars.Text = "🍅🍅🍅🍅";
                    comments.Text = "Excellent！";
                    break;
                case 3:
                    stars.Text = "🍅🍅🍅";
                    comments.Text = "Good！";
                    break;
                case 4:
                    stars.Text = "🍅🍅";
                    comments.Text = "Normal！";
                    break;
                case 5:
                    stars.Text = "🍅";
                    comments.Text = "Bad！";
                    break;
                default:
                    stars.Text = "";
                    comments.Text = "Terrible！";
                    break;
            }


            //小格子展示

            if (taskList != null)
                taskList.Clear();

            taskList = TaskHelper.GetHelper().SelectGroupItemsByDate(dp_ViewDate.Date.DateTime);
            Grid g_ViewTasksContent = new Grid();
            for (int rowCount = 0; rowCount < taskList.Count / 6 + 1; rowCount++)
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
                for (int j = 0; j < 6; j++)
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
                    tb_TaskContentDoneTomato.Text = "Down🍅：" + taskList.ElementAt(count).DoneTomato;
                    tb_TaskContentDoneTomato.FontSize = 10;
                    tb_TaskContentDoneTomato.VerticalAlignment = VerticalAlignment.Center;
                    g_TaskContent.Children.Add(tb_TaskContentDoneTomato);
                    Grid.SetRow(tb_TaskContentDoneTomato, 2);
                    Grid.SetColumn(tb_TaskContentDoneTomato, 0);

                    TextBlock tb_TaskContentUndoneTomato = new TextBlock();
                    tb_TaskContentUndoneTomato.Text = "Undown🍅：" + (taskList.ElementAt(count).TotalTomato - taskList.ElementAt(count).DoneTomato);
                    tb_TaskContentUndoneTomato.FontSize = 10;
                    tb_TaskContentUndoneTomato.VerticalAlignment = VerticalAlignment.Center;
                    g_TaskContent.Children.Add(tb_TaskContentUndoneTomato);
                    Grid.SetRow(tb_TaskContentUndoneTomato, 2);
                    Grid.SetColumn(tb_TaskContentUndoneTomato, 1);

                    g_TaskContent.Tag = count;

                    count++;
                    if (count < taskList.Count)
                        continue;
                    else
                        break;
                }
                i++;
            }


        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            // List<Task> copytotoday = null;
            List<Task> copy = new List<Task>();
            copy = TaskHelper.GetHelper().SelectGroupItemsByDate(dp_ViewDate.Date.DateTime);
            int number = TaskHelper.GetHelper().numOfAllTaskByDate(DateTime.Now);

            for (int i = 0; i < copy.Count; i++)
            {
                number = number + 1;
                copy.ElementAt(i).Id = number;
                copy.ElementAt(i).Date = DateTime.Now;
                copy.ElementAt(i).DoneTomato = 0;
                copy.ElementAt(i).Status = "Undone";

            }
            TaskHelper.GetHelper().InsertItems(copy);
        }
    }
}
