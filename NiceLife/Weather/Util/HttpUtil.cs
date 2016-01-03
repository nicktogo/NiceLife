using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;

namespace NiceLife.Weather.Util
{
    public class HttpUtil
    {
        public const string FORECAST_SOURCE = "http://wthrcdn.etouch.cn/WeatherApi?city={0}";
        public const string PROVINCE_SOURCE = "http://www.weather.com.cn/data/list3/city.xml";
        public const string CITY_COUNTY_SOURCE = "http://www.weather.com.cn/data/list3/city{0}.xml";
        public const string IMAGE_SOURCE = "http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1";
        public const string IMAGE_PREFIX = "http://s.cn.bing.net{0}";

        public static async void SendHttpRequest(string address, HttpCallbackListener listener)
        {
            await Task.Run(async () =>
            {
                HttpClient httpClient = new HttpClient();
                Uri uri = new Uri(address);
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                string response = "";
                try
                {
                    httpResponse = await httpClient.GetAsync(uri);
                    httpResponse.EnsureSuccessStatusCode();
                    var contentType = httpResponse.Content.Headers.ContentType;
                    if (contentType == null)
                    {
                        httpResponse.Content.Headers.Add("Content-Type", "text/xml;CharSet=utf-8");
                    }
                    else if (String.IsNullOrEmpty(contentType.CharSet))
                    {
                        contentType.CharSet = "utf-8";
                    }
                    response = await httpResponse.Content.ReadAsStringAsync();

                    listener.OnFinished(response);
                }
                catch (Exception e)
                {
                    listener.OnError(e);
                }
            });
        }

        public static async void SendGetImageResquest(string address, HttpCallbackListener listener)
        {
            await Task.Run(async () =>
            {
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                Uri uri = new Uri(address);
                var imageBytes = await httpClient.GetByteArrayAsync(uri);
                try
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile imageFile = await localFolder.CreateFileAsync(WeatherFlipView.BACKGROUND_IMAGE_NAME, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBytesAsync(imageFile, imageBytes);
                }
                catch (Exception e)
                {

                    listener.OnError(e);
                }
            });
        }
    }
}
