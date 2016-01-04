using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Schedule.db
{
   public class Alarm
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public Alarm() { }
        public Alarm(string title, string description, DateTime beginTime,DateTime endTime)
        {
            Title = title;
            Description = description;
            BeginTime = beginTime;
            EndTime = endTime;
        }
        public override string ToString()
        {
            return "Title: " + Title + "\n" + "BeginTime: " + BeginTime.ToString("d") + "\n" +"EndTime: "+EndTime.ToString("d")+"\n"+ "Description: " + Description;
        }
    }
}
