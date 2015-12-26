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

        // create forecast
        private static String CREATE_FORECAST = @"CREATE TABLE IF NOT EXISTS Forecast(
            Id integer primary key autoincrement, 
            CountyId integer, 
            Date datetime, 
            Hight text, 
            Low text, 
            DayType text, 
            DayWindDirection text, 
            DayWindPower text, 
            NightType text, 
            NightWindDirection text, 
            NightWindPower text
            );";

        // create RealtimeDetail
        private static string CREATE_REALTIMEDETAIL = @"CREATE TABLE IF NOT EXISTS RealTimeDetail(
            Id integer primary key autoincrement, 
            CountyId integer, 
            CountyName text, 
            UpdateTime text, 
            RealtimeTemp text, 
            Humidity text, 
            RealtimeWindDirection text, 
            RealtimeWindPower text, 
            Sunrise text, 
            Sunset text
            );";

        public static void LoadDatabase()
        {
            SQLiteConnection conn = new SQLiteConnection(App.DB_NAME);
            using (var statement = conn.Prepare(CREATE_PROVINCE))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(CREATE_CITY))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(CREATE_COUNTY))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(CREATE_FORECAST))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(CREATE_REALTIMEDETAIL))
            {
                statement.Step();
            }
        }

    }
}
