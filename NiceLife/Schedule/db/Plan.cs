using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Schedule.db
{
    class Plan
    {
        public static String TABLE_NAME = "Plan";
        public long Id { get; set; }
        public String Title { get; set; }
        public long ColorId { get; set; }
        public long RemindId { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IsRemind { get; set; }
        public long Last { get; set; }
        public int Loop { get; set; }
        
    }
}
