﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Weather.Database
{
    public class ForecastHelper : DbHelper<Forecast>
    {
        private ForecastHelper() : base() { }

        private static ForecastHelper helper;

        public static ForecastHelper GetHelper()
        {
            lock (typeof(ForecastHelper))
            {
                if (helper == null)
                {
                    helper = new ForecastHelper();
                }
            }
            return helper;
        }

        public override Forecast CreateItem(ISQLiteStatement statement)
        {
            Forecast forecast = new Forecast();

            forecast.id = (long)statement[0];
            forecast.countyId = (long)statement[1];

            forecast.date = (DateTime)statement[2];
            forecast.hight = (String)statement[3];
            forecast.low = (String)statement[4];

            forecast.dayType = (String)statement[5];
            forecast.dayWindDirection = (String)statement[6];
            forecast.dayWindPower = (String)statement[7];

            forecast.nightType = (String)statement[8];
            forecast.nightWindDirection = (String)statement[9];
            forecast.nightWindPower = (String)statement[10];

            return forecast;
        }

        public override void DeleteSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void InsertItems(List<Forecast> items)
        {
            foreach (Forecast f in items)
            {
                InsertSingleItem(f);
            }
        }

        public override void InsertSingleItem(Forecast item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                statement.Bind("@Id", item.id);
                statement.Bind("@CountyId", item.countyId);

                statement.Bind("@Date", DateTimeSQLite(item.date));
                statement.Bind("@Hight", item.hight);
                statement.Bind("@Low", item.low);

                statement.Bind("@DayType", item.dayType);
                statement.Bind("@DayWindDirection", item.dayWindDirection);
                statement.Bind("@DayWindPower", item.dayWindPower);

                statement.Bind("@NightType", item.nightType);
                statement.Bind("@NightWindDirection", item.nightWindDirection);
                statement.Bind("@NightWindPower", item.nightWindPower);

                statement.Step();
            }
        }

        public List<Forecast> SelectGroupItems(DateTime beginDateTime)
        {
            List<Forecast> items = new List<Forecast>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                statement.Bind("@BeginDateTime", DateTimeSQLite(beginDateTime));
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Forecast f = CreateItem(statement);
                    items.Add(f);
                }
            }
            return items;
        }

        public override List<Forecast> SelectGroupItems(long foreignId)
        {
            throw new NotImplementedException();
        }

        public override Forecast SelectSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void UpdateItems(List<Forecast> items)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSingleItem(Forecast item)
        {
            throw new NotImplementedException();
        }

        protected override string GetInsertSQL()
        {
            return @"INSERT INTO Forecast 
                (Id, CountyId, Date, Hight, Low, DayType, DayWindDirection, DayWindPower, NightType, NightWindDirection, NightWindPower) 
                VALUES(@Id, @CountyId, @Date, @Hight, @Low, @DayType, @DayWindDirection, @DayWindPower, @NightType, @NightWindDirection, @NightWindPower)";
        }

        protected override string GetSelectAllSQL()
        {
            return "SELECT * FROM Forecast WHERE Date >= @BeginDateTime";
        }

        protected override string GetSelectSQL()
        {
            throw new NotImplementedException();
        }

        private String DateTimeSQLite(DateTime dateTime)
        {
            String dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        }
    }
}
