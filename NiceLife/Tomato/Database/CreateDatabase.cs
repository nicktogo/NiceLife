using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Tomato.Database
{
    class CreateDatabase
    {
        // create task
        private static String CREATE_TASK = @"CREATE TABLE IF NOT EXISTS Task (
            Id integer primary key autoincrement, 
            Title text, 
            Description text,
            Date datetime,
            Type text,
            Status text，
            Total_Tomato integer,
            Done_Tomato integer
            );";

        public static void LoadDatabase()
        {
            SQLiteConnection conn = new SQLiteConnection(App.DB_NAME);
            using (var statement = conn.Prepare(CREATE_TASK))
            {
                statement.Step();
            }
        }
    }
}
