using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class County
    {
        public static String TABLE_NAME = "County";

        public long Id { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public long CityId { get; set; }
        public long CountySelect { get; set; }
    }
}
