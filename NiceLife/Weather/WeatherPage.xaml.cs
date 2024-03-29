﻿using NiceLife.Weather;
using NiceLife.Weather.Database;
using NiceLife.Weather.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NiceLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherPage : Page
    {
        public const int TYPE_PROVINCE = 0x00;
        public const int TYPE_CITY = 0x01;
        public const int TYPE_COUNTY = 0x02;
        public const int TYPE_FORECAST = 0x03;



        private Province selectedProvince;
        private City selectedCity;
        private County selectedCounty;

        ObservableCollection<Province> OProvinces = new ObservableCollection<Province>();
        ObservableCollection<City> OCities = new ObservableCollection<City>();
        ObservableCollection<County> OCounties = new ObservableCollection<County>();

        public WeatherPage()
        {
            this.InitializeComponent();
            GetProvinces();
        }

        private void GetProvinces()
        {
            ProvinceHelper helper = ProvinceHelper.GetHelper();
            List<Province> LProvinces = helper.SelectGroupItems(-1);
            if (LProvinces.Count() > 0)
            {
                OProvinces.Clear();
                foreach (Province p in LProvinces)
                {
                    OProvinces.Add(p);
                }
            }
            else
            {
                GetFromServer(null, TYPE_PROVINCE);
            }
        }

        private void GetCities()
        {
            CityHelper helper = CityHelper.GetHelper();
            List<City> LCities = helper.SelectGroupItems(selectedProvince.Id);
            if (LCities.Count() > 0)
            {
                OCities.Clear();
                foreach (City c in LCities)
                {
                    OCities.Add(c);
                }
            }
            else
            {
                GetFromServer(selectedProvince.Code, TYPE_CITY);
            }
        }

        private void GetCounties()
        {
            CountyHelper helper = CountyHelper.GetHelper();
            List<County> LCounties = helper.SelectGroupItems(selectedCity.Id);
            if (LCounties.Count() > 0)
            {
                OCounties.Clear();
                foreach (County c in LCounties)
                {
                    OCounties.Add(c);
                }
            }
            else
            {
                GetFromServer(selectedCity.Code, TYPE_COUNTY);
            }
        }

        private void GetFromServer(string code, int type)
        {
            string address = null;
            switch (type)
            {
                case TYPE_PROVINCE:
                    address = HttpUtil.PROVINCE_SOURCE;
                    break;
                case TYPE_COUNTY:
                case TYPE_CITY:
                    address = String.Format(HttpUtil.CITY_COUNTY_SOURCE, code);
                    break;
                case TYPE_FORECAST:
                    address = String.Format(HttpUtil.FORECAST_SOURCE, code);
                    break;
            }
            if (address != null)
            {
                HttpUtil.SendHttpRequest(address, new Listener(this, type));
            }
        }

        private class Listener : HttpCallbackListener
        {
            private WeatherPage page;
            private int type;
            public Listener(Page page, int type)
            {
                this.page = (WeatherPage)page;
                this.type = type;
            }
            public void OnError(Exception e)
            {
                throw e;
            }

            public async void OnFinished(string response)
            {
                bool result = false;
                switch (type)
                {
                    case TYPE_PROVINCE:
                        result = DataUtility.handleProvincesResponse(response);
                        break;
                    case TYPE_COUNTY:
                        result = DataUtility.handleCountiesResponse(response, page.selectedCity.Id);
                        break;
                    case TYPE_CITY:
                        result = DataUtility.handleCitiesResponse(response, page.selectedProvince.Id);
                        break;
                    case TYPE_FORECAST:
                        result = DataUtility.handleForecastResponse(response, page.selectedCounty.Id);
                        break;
                }
                if (result)
                {
                    // close progress dialog
                    switch (type)
                    {
                        case TYPE_PROVINCE:
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                             {
                                 page.GetProvinces();
                             });
                            break;
                        case TYPE_COUNTY:
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                page.GetCounties();
                            });
                            break;
                        case TYPE_CITY:
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                page.GetCities();
                            });
                            break;
                        case TYPE_FORECAST:
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                page.Frame.Navigate(typeof(WeatherFlipView), WeatherFlipView.NAVIGATED_FROM_WEATHERPAGE);
                            });
                            break;
                    }

                }
            }
        }

        private void Province_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = Province.SelectedIndex;
            if (index != -1)
            {
                selectedProvince = OProvinces[index];
                GetCities();
            }
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = City.SelectedIndex;
            if (index != -1)
            {
                selectedCity = OCities[index];
                GetCounties(); 
            }
        }

        private void County_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = County.SelectedIndex;
            if (index != -1)
            {
                selectedCounty = OCounties[index];
                selectedCounty.CountySelect = Weather.Database.County.COUNT_SELECTED;
                CountyHelper countyHelper = CountyHelper.GetHelper();
                countyHelper.UpdateSingleItem(selectedCounty);
                // get forecast for the selected county
                GetFromServer(selectedCounty.Name, TYPE_FORECAST);
            }
        }
    }
}
