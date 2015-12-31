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

namespace NiceLife.Weather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherInfo : Page
    {
        private const string FORECAST_SOURCE = "http://wthrcdn.etouch.cn/WeatherApi?city={0}";
        private County selectedCounty;
        private RealtimeDetailViewModel realTimeDetail = new RealtimeDetailViewModel();
        private ObservableCollection<Forecast> forecasts = new ObservableCollection<Forecast>();
        public WeatherInfo()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            long CountyId = (long)e.Parameter;
            CountyHelper helper = CountyHelper.GetHelper();
            selectedCounty = helper.SelectSingleItemById(CountyId);
            selectedCounty.CountySelect = County.COUNT_SELECTED;
            helper.UpdateSingleItem(selectedCounty);
            GetForecast();
        }

        private void GetForecast()
        {
            RealTimeDetailHelper rHelper = RealTimeDetailHelper.GetHelper();
            realTimeDetail.DefaultRealtimeDetail = rHelper.SelectSingleItemById(selectedCounty.Id);
            ForecastHelper helper = ForecastHelper.GetHelper();
            List<Forecast> items = helper.SelectGroupItems(selectedCounty.Id);
            if (realTimeDetail != null && items.Count > 0)
            {
                foreach (Forecast f in items)
                {
                    forecasts.Add(f);
                }
            }
            else
            {
                GetFromServer();
            }
        }

        private void GetFromServer()
        {

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

            public async void OnFinished(string response)
            {
                if (DataUtility.handleForecastResponse(response, page.selectedCounty.Id))
                {
                    await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        page.GetForecast();
                    });
                }
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var forecast = (Forecast)e.ClickedItem;
            DayTpye.Text = forecast.dayType;
            DayWindDirection.Text = forecast.dayWindDirection;
            DayWindPower.Text = forecast.dayWindPower;
            NightTpye.Text = forecast.nightType;
            NightWindDirection.Text = forecast.nightWindDirection;
            NightWindPower.Text = forecast.nightWindPower;
        }
    }
}
