using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiceLife.Weather.Database;
using System.Xml;

namespace NiceLife.Weather.Util
{
    public class DataUtility
    {
        private const string TAG_DATE = "date";
        private const string TAG_HIGH = "high";
        private const string TAG_LOW = "low";
        private const string TAG_TYPE = "type";
        private const string TAG_WIND_DIRECTION = "fengxiang";
        private const string TAG_WIND_POWER = "fengli";

        public static bool handleProvincesResponse(string response)
        {
            if (!String.IsNullOrEmpty(response))
            {
                string[] provinces = response.Split(',');
                if (provinces.Count() > 0)
                {
                    List<Province> lprovinces = new List<Province>();
                    foreach (string p in provinces)
                    {
                        string[] element = p.Split('|');
                        Province province = new Province();
                        province.Code = element[0];
                        province.Name = element[1];
                        lprovinces.Add(province);
                    }
                    ProvinceHelper helper = ProvinceHelper.GetHelper();
                    helper.InsertItems(lprovinces);
                }
                return true;
            }
            return false;
        }

        public static bool handleCitiesResponse(string response, long ProvinceId)
        {
            if (!String.IsNullOrEmpty(response))
            {
                string[] cities = response.Split(',');
                if (cities.Count() > 0)
                {
                    List<City> lcities = new List<City>();
                    foreach (string c in cities)
                    {
                        string[] element = c.Split('|');
                        City city = new City();
                        city.Code = element[0];
                        city.Name = element[1];
                        city.ProvinceId = ProvinceId;
                        lcities.Add(city);
                    }
                    CityHelper helper = CityHelper.GetHelper();
                    helper.InsertItems(lcities);
                }
                return true;
            }
            return false;
        }

        public static bool handleCountiesResponse(string response, long CityId)
        {
            if (!String.IsNullOrEmpty(response))
            {
                string[] counties = response.Split(',');
                if (counties.Count() > 0)
                {
                    List<County> lcounties = new List<County>();
                    foreach (string c in counties)
                    {
                        string[] element = c.Split('|');
                        County county = new County();
                        county.Code = element[0];
                        county.Name = element[1];
                        county.CityId = CityId;
                        lcounties.Add(county);
                    }
                    CountyHelper helper = CountyHelper.GetHelper();
                    helper.InsertItems(lcounties);
                }
                return true;
            }
            return false;
        }

        public static bool handleWeatherResponse(string response, long CountyId)
        {
            if (!String.IsNullOrEmpty(response))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNodeList elements = doc.GetElementsByTagName("weather");

                List<Forecast> forecasts = new List<Forecast>();
                DateTime now = DateTime.Now;
                string yesterday = doc.GetElementsByTagName("date_1").Item(0).InnerText;
                string actualYesterday = now.AddDays(-1).Day.ToString();
                DateTime firstDate = yesterday.Contains(actualYesterday) ? now : now.AddDays(-1);

                foreach (XmlElement element in elements)
                {
                    Forecast forecast = new Forecast();
                    forecast.countyId = CountyId;
                    forecast.date = firstDate;
                    firstDate = firstDate.AddDays(1);

                    XmlNodeList tempList;
                    tempList = element.GetElementsByTagName(TAG_HIGH);
                    forecast.hight = tempList.Item(0).InnerText.Split(' ')[1];

                    tempList = element.GetElementsByTagName(TAG_LOW);
                    forecast.low = tempList.Item(0).InnerText.Split(' ')[1];

                    tempList = element.GetElementsByTagName(TAG_TYPE);
                    forecast.dayType = tempList.Item(0).InnerText;
                    forecast.nightType = tempList.Item(1).InnerText;

                    tempList = element.GetElementsByTagName(TAG_WIND_DIRECTION);
                    forecast.dayWindDirection = tempList.Item(0).InnerText;
                    forecast.nightWindDirection = tempList.Item(1).InnerText;

                    tempList = element.GetElementsByTagName(TAG_WIND_POWER);
                    forecast.dayWindPower = tempList.Item(0).InnerText;
                    forecast.nightWindPower = tempList.Item(1).InnerText;

                    forecasts.Add(forecast);
                }
                ForecastHelper helper = ForecastHelper.GetHelper();
                helper.DeleteAllItems();
                helper.InsertItems(forecasts);
                return true;
            }
            return false;
        }
    }
}
