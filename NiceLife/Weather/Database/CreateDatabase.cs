using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLitePCL;

namespace NiceLife.Weather.Database
{
    public class CreateDatabase
    {
        // create province
        private static String CREATE_PROVINCE = @"CREATE TABLE IF NOT EXISTS Province (
            Id integer primary key autoincrement, 
            Name text, 
            Code text
            );";

        public static SQLiteResult LoadDatabase()
        {
            
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_NAME))
            {
                var statement = conn.Prepare(CREATE_PROVINCE);
                return statement.Step();
            }
        }

    }
}
