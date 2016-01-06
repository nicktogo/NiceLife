using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Schedule.db
{
    class Call
    {
        public static String TABLE_NAME = "Call";
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public long State { get; set; }//1--undo,2---doing,3---done;
        public long PlanId { get; set; }

    }
}
