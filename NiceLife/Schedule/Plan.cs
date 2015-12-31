using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Schedule
{
    class Plan
    {
        public long Id { get; set; }
        public String Title { get; set; }
        public long colorId { get; set; }
        public long callId { get; set; }
        public string Text { get; set; }
        public string beginTime { get; set; }
        public string endTime { get; set; }
        public int call_YorN { get; set; }
    }
}
