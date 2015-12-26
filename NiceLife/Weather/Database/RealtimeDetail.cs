using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class RealtimeDetail
    {
        public long Id { get; set; }
        public long CountyId { get; set; }
        public string CountyName { get; set; }
        public string UpdateTime { get; set; }
        public string RealtimeTemp { get; set; }
        public string RealtimeWindDirection { get; set; }
        public string RealtimeWindPower { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
    }
}
