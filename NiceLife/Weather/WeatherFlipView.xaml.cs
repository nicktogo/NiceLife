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
    public sealed partial class WeatherFlipView : Page
    {
        private ObservableCollection<WeatherModel> weathers = new ObservableCollection<WeatherModel>();
        public WeatherFlipView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GetWeathers();
        }

        private void GetWeathers()
        {
            CountyHelper countyHelper = CountyHelper.GetHelper();
            List<County> counties = countyHelper.GetSelectedItems();

            RealTimeDetailHelper realTimeDetailHelper = RealTimeDetailHelper.GetHelper();
            ForecastHelper forecastHelper = ForecastHelper.GetHelper();
            foreach (County county in counties)
            {
                WeatherModel weatherModel = new WeatherModel();
                long countyId = county.Id;
                weatherModel.realtimeDetail = realTimeDetailHelper.SelectSingleItemById(county.Id);
                List<Forecast> forecasts = forecastHelper.SelectGroupItems(county.Id);
                foreach (Forecast f in forecasts)
                {
                    weatherModel.forecasts.Add(f);
                }
                weathers.Add(weatherModel);
            }
        }

        private void GetFromServer(County county)
        {

            string address = String.Format(HttpUtil.FORECAST_SOURCE, county.Name);
            HttpUtil.SendHttpRequest(address, new Listener(this, county.Id));
        }

        private class Listener : HttpCallbackListener
        {
            private WeatherFlipView page;
            private long countyId;
            public Listener(WeatherFlipView page, long countyId)
            {
                this.page = page;
                this.countyId = countyId;
            }
            public void OnError(Exception e)
            {
                throw e;
            }

            public async void OnFinished(string response)
            {
                if (DataUtility.handleForecastResponse(response, countyId))
                {
                    await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        page.GetWeathers();
                    });
                }
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var forecast = (Forecast)e.ClickedItem;
            var container = WeatherFlip.ItemContainerGenerator.ContainerFromItem(WeatherFlip.SelectedItem);
            var children = GetAllChildren(container);

            TextBlock textBlock;

            textBlock = GetTextBlock(children, "DayType");
            textBlock.Text = forecast.dayType;

            textBlock = GetTextBlock(children, "DayWindDirection");
            textBlock.Text = forecast.dayWindDirection;

            textBlock = GetTextBlock(children, "DayWindPower");
            textBlock.Text = forecast.dayWindPower;

            textBlock = GetTextBlock(children, "NightType");
            textBlock.Text = forecast.nightType;

            textBlock = GetTextBlock(children, "NightWindDirection");
            textBlock.Text = forecast.nightWindDirection;

            textBlock = GetTextBlock(children, "NightWindPower");
            textBlock.Text = forecast.nightWindPower;
        }

        private List<TextBlock> GetAllChildren(DependencyObject parent)
        {
            var _List = new List<TextBlock>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is TextBlock)
                {
                    _List.Add(_Child as TextBlock);
                }
                _List.AddRange(GetAllChildren(_Child));
            }
            return _List;
        }

        private TextBlock GetTextBlock(List<TextBlock> children, string name)
        {
            return children.OfType<TextBlock>().First(x => x.Name.Equals(name));
        }
    }
}
