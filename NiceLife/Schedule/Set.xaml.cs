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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    
    public sealed partial class Set : Page
    {
        int page = 1;
        int cur_page = 1;
        int cur_count = 0;
        int all = 0;
        int change = 0;
        List<ColorLable> list;
        public Set()
        {
            this.InitializeComponent();
            appBarButton_save.Visibility = Visibility.Collapsed;
            ColorLableHelper ch = ColorLableHelper.GetHelper();
            list = ch.SelectlistItems();
            all = list.Count();
            page = (all / 12) + 1;
            cur_count = all % 12;
            showpage(1);
        }
        public void showpage(int p)
        {
             change = 0;
            color.Children.Clear();
            cur_page = p;
            int begin ;
            if (p == 1)
            {
                begin = 0;
                button_last.Visibility = Visibility.Collapsed;
            }
            else
            {
                begin = (p - 1) * 12;
                button_last.Visibility = Visibility.Visible;
            }
            if (p == page)
            {
                button_next.Visibility = Visibility.Collapsed;
            }
            else
            {
                button_next.Visibility = Visibility.Visible;
            }
            int k = 0;
            while (begin < all)
            {
                k++;
                ColorLable c = list.ElementAt(begin);
                RectangleGeometry rc = new RectangleGeometry();
                //color
                TextBlock t = new TextBlock();
                t.FontSize = 52;
                t.Margin = new Thickness(200, 0, 0, 0);
                t.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                t.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                t.Text =c.Mean;
                //can't edit t.text
                color.Children.Add(t);
                if (cur_count > 5)
                {
                    Grid.SetRow(t, cur_count - 6);
                    Grid.SetColumn(t, 3);
                }
                else
                {
                    Grid.SetRow(t, cur_count);
                    Grid.SetColumn(t, 0);
                }
                if (k >= 12) break;
            }
        }
       

        private void appBarButton_edit_Click(object sender, RoutedEventArgs e)
        {
            appBarButton_save.Visibility = Visibility.Visible;
            appBarButton_edit.Visibility = Visibility.Collapsed;
            change = 1;

        }

        private void appBarButton_add_Click(object sender, RoutedEventArgs e)
        {
             change = 1;
            cur_count++;
            all++;
            if (cur_count > 12)
            {
                page++;
                color.Children.Clear();

            }
            //RectangleGeometry rc = new RectangleGeometry();
            
            TextBlock t = new TextBlock();
            t.FontSize = 52;
            t.Margin = new Thickness(50, 0, 0, 0);
            t.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            t.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            t.Text = "set Lable";
            
            color.Children.Add(t);
            if (cur_count > 6)
            {
                Grid.SetRow(t, cur_count - 6-1);
                Grid.SetColumn(t, 3);
            }
            else
            {
                Grid.SetRow(t, cur_count-1);
                Grid.SetColumn(t, 0);
            }
        }

        private void appBarButton_save_Click(object sender, RoutedEventArgs e)
        {
            appBarButton_save.Visibility = Visibility.Collapsed;
            appBarButton_edit.Visibility = Visibility.Visible;
            change = 0;
        }

        private void button_last_Click(object sender, RoutedEventArgs e)
        {
            if (change == 1)
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("please save at first!"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            else {
                showpage(cur_page - 1);
            }
        }

        private void button_next_Click(object sender, RoutedEventArgs e)
        {
            if (change == 1)
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("please save at first!"));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            else {
                showpage(cur_page + 1);
            }
        }

        
    }
}
