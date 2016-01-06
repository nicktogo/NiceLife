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
using System.Collections.ObjectModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace NiceLife
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    
    public sealed partial class Set : Page
    {

        public bool ReadOnly{ get; set; }
        public string Textcolor { get; set; }
        List <ColorLable> list;
        ObservableCollection<ColorLable> UsefulAlarm = new ObservableCollection<ColorLable>();
        public Set()
        {
            this.InitializeComponent();

            show();


        }
       
      public void show()
        {
            listView1.Items.Clear();
            ColorLableHelper cp = ColorLableHelper.GetHelper();
            list = cp.SelectlistItems();
            for (int i = 0; i < list.Count(); i++)
            {
                UsefulAlarm.Add(list.ElementAt(i));
            }
            ReadOnly = true;
            listView1.DataContext = UsefulAlarm;
            
        }
       

       

        private void appBarButton_add_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void appBarButton_save_Click(object sender, RoutedEventArgs e)
        {
           for(int i = 0; i < list.Count; i++)
            {
                ColorLableHelper cp = ColorLableHelper.GetHelper();
                cp.UpdateSingleItem(list.ElementAt(i));
            }
            show();

        }

       
        private void color_edit_Click(object sender, RoutedEventArgs e)
        {
            ReadOnly = false;
        }

        private void color_delete_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedIndex != -1)
            {
                ColorLableHelper cp = ColorLableHelper.GetHelper();
                cp.DeleteSingleItemById(list.ElementAt(listView1.SelectedIndex).Id);
                list.RemoveAt(listView1.SelectedIndex);
            }
            show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listView1.SelectedIndex != -1)
            {
                list.ElementAt(listView1.SelectedIndex).Mean = Textcolor;
            }
        }
    }
}
