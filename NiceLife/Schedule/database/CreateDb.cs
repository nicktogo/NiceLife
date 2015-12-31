using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Schedule.database
{
    class CreateDb
    {
        private static String CREATE_Plan = @"CREATE TABLE IF NOT EXISTS Plan (
            Id integer primary key autoincrement, 
            Title text, 
            ColorId integer,
            Description text,
            BeginDate datetime,
            EndDate datetime,
            Last integer,
            Loop integer,
            IsRemind integer,
            RemindId integer
            
            );";
        private static String CREATE_ColorLable = @"CREATE TABLE IF NOT EXISTS ColorLable (
            Id integer primary key autoincrement,  
            Color text,
            Mean text
            );";
        private static String CREATE_Call = @"CREATE TABLE IF NOT EXISTS Call (
            Id integer primary key autoincrement,  
            Date datetime,
            Type integer,
            State integer
            );";

        public static void LoadDatabase()
        {
            SQLiteConnection conn = new SQLiteConnection(App.DB_NAME);
            using (var statement = conn.Prepare(CREATE_Plan))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(CREATE_Call))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(CREATE_ColorLable))
            {
                statement.Step();
            }
        }
    }
}
