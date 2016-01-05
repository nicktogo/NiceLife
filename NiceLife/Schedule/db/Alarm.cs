using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Schedule.db
{
   public class Alarm
    {
        public int Num { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public Alarm() { }
        public Alarm(int num,string title, string description, DateTime beginTime,DateTime endTime)
        {
            Num = num;
            Title = title;
            Description = description;
            BeginTime = beginTime;
            EndTime = endTime;
        }
        public override string ToString()
        {
            return "Num:  "+Num+"\n"+"Title: " + Title + "\n" + "BeginTime: " + BeginTime.ToString("d") + "\n" +"EndTime: "+EndTime.ToString("d")+"\n"+ "Description: " + Description;
        }
    }
}
