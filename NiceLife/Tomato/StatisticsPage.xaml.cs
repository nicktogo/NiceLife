using NiceLife.Tomato.Database;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            this.InitializeComponent();
        }

        public void freshOne(int comNum, int sumNum)
        {
            float comRate;
            if (sumNum == 0)
            {
                comRate = 0;
            }
            else comRate = (float)comNum / sumNum;
            warnning.Visibility = (Visibility)0;
            if (comRate < 0.5)
                warnning.Text = "";
            else
                warnning.Text = "";
            double length = 310.86 * comRate;
            Eone.StrokeDashArray.Clear();
            Eone.StrokeDashArray.Add(length);
            Eone.StrokeDashArray.Add(628);
            rateone.Visibility = (Visibility)0;
            comRate = comRate * 100;
            String rate = comRate.ToString("0.0") + "%";
            rateone.Text = rate;

            rectcom1.Visibility = (Visibility)0;
            rectsum1.Visibility = (Visibility)0;
            textcom1.Visibility = (Visibility)0;
            textsum1.Visibility = (Visibility)0;
            line1.Visibility = (Visibility)0;
            namecom1.Visibility = (Visibility)0;
            namesum1.Visibility = (Visibility)0;
            if (sumNum == 0)
            {
                rectsum1.Height = 0;
            }
            rectcom1.Height = 2.75 * comRate;
            textsum1.Text = sumNum.ToString();
            textcom1.Text = comNum.ToString();

            namecom1.Text = "Done";
            namesum1.Text = "Total";
        }

        public void freshTwo(int comNum, int sumNum)
        {
            float comRate;
            if (sumNum == 0)
            {
                comRate = 0;
            }
            else comRate = (float)comNum / sumNum;
            warnning.Visibility = (Visibility)0;
            if (comRate < 0.5)
                warnning.Text = "";
            else
                warnning.Text = "";
            double length = 310.86 * comRate;
            Etwo.StrokeDashArray.Clear();
            Etwo.StrokeDashArray.Add(length);
            Etwo.StrokeDashArray.Add(628);
            ratetwo.Visibility = (Visibility)0;
            comRate = comRate * 100;
            String rate = comRate.ToString("0.0") + "%";
            ratetwo.Text = rate;

            rectcom2.Visibility = (Visibility)0;
            rectsum2.Visibility = (Visibility)0;
            textcom2.Visibility = (Visibility)0;
            textsum2.Visibility = (Visibility)0;
            line2.Visibility = (Visibility)0;
            namecom2.Visibility = (Visibility)0;
            namesum2.Visibility = (Visibility)0;

            if (sumNum == 0)
            {
                rectsum1.Height = 0;
            }
            rectcom2.Height = 2.75 * comRate;
            textsum2.Text = sumNum.ToString();
            textcom2.Text = comNum.ToString();

            namecom2.Text = "Done";
            namesum2.Text = "Total";
        }



        private void task_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            if (li.Content.ToString().Equals("Today's task"))
            {
                int comNumToday;
                int sumNumToday;
                sumNumToday = TaskHelper.GetHelper().numOfAllTaskByDate(DateTime.Now);
                comNumToday = TaskHelper.GetHelper().numOfDoneTaskByDate(DateTime.Now);
                freshOne(comNumToday, sumNumToday);
            }
            else
            {
                int comNumAll;
                int sumNumAll;
                sumNumAll = TaskHelper.GetHelper().numOfAllTask();
                comNumAll = TaskHelper.GetHelper().numOfDoneTask();
                freshOne(comNumAll, sumNumAll);
            }

        }

        private void tomato_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            if (li.Content.ToString().Equals("Today's tomato"))
            {
                int comNumToday = TaskHelper.GetHelper().numOfDoneTomatoByDate(DateTime.Now);
                int sumNumToday = TaskHelper.GetHelper().numOfAllTomatoByDate(DateTime.Now);
                freshTwo(comNumToday, sumNumToday);
            }
            else
            {
                int comNumAll = TaskHelper.GetHelper().numOfDoneTomato();
                int sumNumAll = TaskHelper.GetHelper().numOfAllTomato();
                freshTwo(comNumAll, sumNumAll);
            }

        }


    }
}
