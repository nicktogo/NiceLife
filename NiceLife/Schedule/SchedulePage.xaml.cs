﻿using System;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NiceLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            this.InitializeComponent();
            CreateDb.LoadDatabase();
        }

        

        private void add_Click(object sender, RoutedEventArgs e)
        {
            ScheduleFrame.Navigate(typeof(Add));
        }

       

        private void select_Click(object sender, RoutedEventArgs e)
        {
            ScheduleFrame.Navigate(typeof(Select));
        }

        private void set_Click(object sender, RoutedEventArgs e)
        {
            ScheduleFrame.Navigate(typeof(Set));
        }
    }
}

