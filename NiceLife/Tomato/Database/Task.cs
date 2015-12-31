using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Tomato.Database
{
    public class Task
    {
        public static String TABLE_NAME = "Task";

        public long Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public String Type { get; set; }
        public String Status { get; set; }
        public long TotalTomato { get; set; }
        public long DoneTomato { get; set; }
    }
}
