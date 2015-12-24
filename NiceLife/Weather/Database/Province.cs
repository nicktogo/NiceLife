using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class Province
    {
        public static String TABLE_NAME = "Province";

        public long Id { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
    }
}
