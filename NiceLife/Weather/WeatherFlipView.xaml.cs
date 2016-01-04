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
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NiceLife.Weather
{
    /// <summary>                                              
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherFlipView : Page
    {
        public const string BACKGROUND_IMAGE_NAME = "background.jpg";
        public const int NAVIGATED_FROM_MAINPAGE = 0x00;
        public const int NAVIGATED_FROM_WEATHERPAGE = 0x01;

        private ObservableCollection<WeatherModel> weathers = new ObservableCollection<WeatherModel>();
        private DependencyObject currentFlipViewContainer;
        private BitmapImage backgroudSource = new BitmapImage();

        private bool isFromMainPage = false;
        public WeatherFlipView()
        {
            this.InitializeComponent();

            // set background image
            GetBackgroundFromFile();
        }

        private async void GetBackgroundFromFile()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile backgroundFile = await localFolder.GetFileAsync(BACKGROUND_IMAGE_NAME);
                using (IRandomAccessStream fileStream = await backgroundFile.OpenAsync(FileAccessMode.Read))
                {
                    await backgroudSource.SetSourceAsync(fileStream);
                }
            }
            catch (FileNotFoundException)
            {
                Uri uri = new Uri("ms-appx:///Assets/DefaultBackground.jpg");
                backgroudSource.UriSource = uri;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            isFromMainPage = (int)e.Parameter == NAVIGATED_FROM_MAINPAGE ? true : false;
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

        private void GetFromServer()
        {
            List<County> counties = CountyHelper.GetHelper().GetSelectedItems();
            foreach (County county in counties)
            {
                string address = String.Format(HttpUtil.FORECAST_SOURCE, county.Name);
                HttpUtil.SendHttpRequest(address, new Listener(this, county.Id));
            }
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
            if (isFromMainPage)
            {
                //update all forecast
                GetFromServer();
            }
            else
            {
                GetWeathers();
            }
            currentFlipViewContainer = WeatherFlip.ItemContainerGenerator.ContainerFromItem(WeatherFlip.SelectedItem);
            if (currentFlipViewContainer != null)
            {
                var ForecastGrid = FindElementByName<GridView>(currentFlipViewContainer, "ForecastGrid");
                ForecastGrid.SelectedIndex = 0;
            }
        }

        private void SearchArea_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var counties = CountyHelper.GetHelper().SelectGroupItems(CountyHelper.ALL_COUNTIES);
                sender.ItemsSource = counties;
            }
        }

        private void SearchArea_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var county = args.SelectedItem as County;
            sender.Text = county == null ? "" : county.Name;
        }

        private void SearchArea_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // user selected a county from dropdown list
                AddWeather(args.ChosenSuggestion as County);
            }
            else
            {
                // user submit directly
            }
        }

        private void AddWeather(County county)
        {
            if (county == null)
            {
                return;
            }
            CountyHelper countyHelper = CountyHelper.GetHelper();
            county.CountySelect = County.COUNT_SELECTED;
            countyHelper.UpdateSingleItem(county);

            string address = String.Format(HttpUtil.FORECAST_SOURCE, county.Name);
            HttpUtil.SendHttpRequest(address, new ForecastListener(this, county));
        }

        private class ForecastListener : HttpCallbackListener
        {
            private WeatherFlipView page;
            private County county;
            public ForecastListener(WeatherFlipView page, County county)
            {
                this.page = page;
                this.county = county;
            }
            public void OnError(Exception e)
            {
                throw e;
            }

            public async void OnFinished(string response)
            {
                bool result = false;
                result = DataUtility.handleForecastResponse(response, county.Id);
                if (result)
                {
                    WeatherModel weatherModel = new WeatherModel();
                    weatherModel.realtimeDetail = RealTimeDetailHelper.GetHelper().SelectSingleItemById(county.Id);
                    List<Forecast> forecasts = ForecastHelper.GetHelper().SelectGroupItems(county.Id);
                    foreach (Forecast f in forecasts)
                    {
                        weatherModel.forecasts.Add(f);
                    }
                    await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        page.weathers.Add(weatherModel);
                        page.WeatherFlip.SelectedItem = weatherModel;
                    });
                }
            }
        }

        private void WeatherFlip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentFlipViewContainer = WeatherFlip.ItemContainerGenerator.ContainerFromItem(WeatherFlip.SelectedItem);
            if (currentFlipViewContainer != null)
            {
                var ForecastGrid = FindElementByName<GridView>(currentFlipViewContainer, "ForecastGrid");
                ForecastGrid.SelectedIndex = 0;
            }
        }

        private void RefreshWeather_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
