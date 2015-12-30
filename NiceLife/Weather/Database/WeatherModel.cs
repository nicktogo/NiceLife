using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class WeatherModel
    {
        public RealtimeDetail realtimeDetail { get; set; }
        public ObservableCollection<Forecast> forecasts { get; set; }

        public WeatherModel()
        {
            forecasts = new ObservableCollection<Forecast>();
        }
    }
}
