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

        // create city
        private static String CREATE_CITY = @"CREATE TABLE IF NOT EXISTS City (
            Id integer primary key autoincrement, 
            Name text, 
            Code text, 
            ProvinceId integer
            );";

        // create county
        private static String CREATE_COUNTY = @"CREATE TABLE IF NOT EXISTS  County( 
            Id integer primary key autoincrement, 
            Name text, 
            Code text, 
            CityId integer, 
            CountySelect integer default 0
            );";

        public static SQLiteResult LoadDatabase()
        {
            
            using (SQLiteConnection conn = new SQLiteConnection(App.DB_NAME))
            {
                var statement = conn.Prepare(CREATE_PROVINCE);
                statement.Step();
                statement = conn.Prepare(CREATE_CITY);
                statement.Step();
                statement = conn.Prepare(CREATE_COUNTY);
                statement.Step();
                return statement.Step();
            }
        }

    }
}
