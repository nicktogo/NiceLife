﻿using NiceLife.Weather.Database;
using NiceLife.Weather.Util;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NiceLife.Weather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherInfo : Page
    {
        private const string FORECAST_SOURCE = "http://wthrcdn.etouch.cn/WeatherApi?city={0}";
        private County selectedCounty;
        public WeatherInfo()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            long CountyId = (long)e.Parameter;
            selectedCounty = CountyHelper.GetHelper().SelectSingleItemById(CountyId);

            string address = String.Format(FORECAST_SOURCE, selectedCounty.Name);
            HttpUtil.SendHttpRequest(address, new Listener(this));
            
        }

        private class Listener : HttpCallbackListener
        {
            private WeatherInfo page;
            public  Listener(WeatherInfo page)
            {
                this.page = page;
            }
            public void OnError(Exception e)
            {
                throw e;
            }

            public void OnFinished(string response)
            {
                DataUtility.handleWeatherResponse(response, page.selectedCounty.Id);
            }
        }
    }
}
