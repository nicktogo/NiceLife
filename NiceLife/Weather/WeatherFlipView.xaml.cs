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
        private DependencyObject currentFlipViewContainer;
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

        private T FindElementByName<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            T element = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                FrameworkElement tempElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (tempElement == null)
                {
                    continue;
                }
                if (tempElement is T && tempElement.Name.Equals(name))
                {
                    return (T)tempElement;
                }
                element = FindElementByName<T>(tempElement, name);
                if (element != null)
                {
                    break;
                }
            }
            return element;
        }

        private void ForecastGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Forecast forecast in e.AddedItems)
            {
                if (currentFlipViewContainer != null)
                {
                    var DayType = FindElementByName<TextBlock>(currentFlipViewContainer, "DayType");
                    DayType.Text = forecast.dayType;
                    var DayWindDirection = FindElementByName<TextBlock>(currentFlipViewContainer, "DayWindDirection");
                    DayWindDirection.Text = forecast.dayWindDirection;
                    var DayWindPower = FindElementByName<TextBlock>(currentFlipViewContainer, "DayWindPower");
                    DayWindPower.Text = forecast.dayWindPower;
                    var NightType = FindElementByName<TextBlock>(currentFlipViewContainer, "NightType");
                    NightType.Text = forecast.nightType;
                    var NightWindDirection = FindElementByName<TextBlock>(currentFlipViewContainer, "NightWindDirection");
                    NightWindDirection.Text = forecast.nightWindDirection;
                    var NightWindPower = FindElementByName<TextBlock>(currentFlipViewContainer, "NightWindPower");
                    NightWindPower.Text = forecast.nightWindPower;
                }
                break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            currentFlipViewContainer = WeatherFlip.ItemContainerGenerator.ContainerFromItem(WeatherFlip.SelectedItem);
            if (currentFlipViewContainer != null)
            {
                var ForecastGrid = FindElementByName<GridView>(currentFlipViewContainer, "ForecastGrid");
                ForecastGrid.SelectedIndex = 0;
            }
        }
    }
}
