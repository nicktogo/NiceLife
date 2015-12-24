using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class City
    {
        public static String TABLE_NAME = "City";

        public long Id { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public long ProvinceId { get; set; }
    }
}
