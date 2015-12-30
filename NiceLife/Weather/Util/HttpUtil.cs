﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace NiceLife.Weather.Util
{
    public class HttpUtil
    {
        public const string FORECAST_SOURCE = "http://wthrcdn.etouch.cn/WeatherApi?city={0}";

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
    }
}
