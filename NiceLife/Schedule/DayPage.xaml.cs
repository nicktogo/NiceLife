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

namespace NiceLife.Schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DayPage : Page
    {
        DateTime current;
        public DayPage()
        {
            this.InitializeComponent();
            current = DateTime.Today;
            choose.Date = current;
            
            for(int i = 0; i < 12; i++)
            {
                TextBlock t = new TextBlock();
                t.FontSize = 36;


                t.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                t.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                t.Text = Convert.ToString(i);
                
                time.Children.Add(t);
                Grid.SetRow(t, i);
                Grid.SetColumn(t, 0);
                TextBlock t2 = new TextBlock();
                t2.FontSize = 36;


                t2.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                t2.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                t2.Text = Convert.ToString(i+12);

                time.Children.Add(t2);
                Grid.SetRow(t2, i);
                Grid.SetColumn(t2, 2);
            }
        }
    }
}
