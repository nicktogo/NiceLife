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
                    if (String.IsNullOrEmpty(contentType.CharSet))
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