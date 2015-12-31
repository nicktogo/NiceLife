using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Schedule.database
{
    class ColorLable
    {
        public static String TABLE_NAME = "ColorLable";

        public long Id { get; set; }
        public string Color { get; set; }
        public string Mean { get; set; }
    }
}
