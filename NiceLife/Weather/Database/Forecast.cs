using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class Forecast
    {
        public long id { get; set; }
        public long countyId { get; set; }

        public DateTime date { get; set; }
        public String hight { get; set; }
        public String low { get; set; }

        public String dayType { get; set; }
        public String dayWindDirection { get; set; }
        public String dayWindPower { get; set; }

        public String nightType { get; set; }
        public String nightWindDirection { get; set; }
        public String nightWindPower { get; set; }
    }
}
