using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Weather.Database
{
    public class RealTimeDetailHelper : DbHelper<RealtimeDetail>
    {
        private RealTimeDetailHelper() : base() { }

        private static RealTimeDetailHelper helper;

        public static RealTimeDetailHelper GetHelper()
        {
            lock (typeof(RealTimeDetailHelper))
            {
                if (helper == null)
                {
                    helper = new RealTimeDetailHelper();
                }
            }
            return helper;
        }

        public override RealtimeDetail CreateItem(ISQLiteStatement statement)
        {
            RealtimeDetail d = new RealtimeDetail();
            d.Id = (long)statement[0];
            d.CountyId = (long)statement[1];
            d.CountyName = (string)statement[2];
            d.UpdateTime = (string)statement[3];
            d.RealtimeTemp = (string)statement[4];
            d.Humidity = (string)statement[5];
            d.RealtimeWindDirection = (string)statement[6];
            d.RealtimeWindPower = (string)statement[7];
            d.Sunrise = (string)statement[8];
            d.Sunset = (string)statement[9];
            return d;
        }

        public override void DeleteSingleItemById(long id)
        {
            using (var statement = conn.Prepare(GetDeleteSQL()))
            {
                statement.Bind("@CountyId", id);
                statement.Step();
            }
        }

        public override void InsertItems(List<RealtimeDetail> items)
        {
            throw new NotImplementedException();
        }

        public override void InsertSingleItem(RealtimeDetail item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                statement.Bind("@CountyId", item.CountyId);
                statement.Bind("@CountyName", item.CountyName);
                statement.Bind("@UpdateTime", item.UpdateTime);
                statement.Bind("@RealtimeTemp", item.RealtimeTemp);
                statement.Bind("@Humidity", item.Humidity);
                statement.Bind("@RealtimeWindDirection", item.RealtimeWindDirection);
                statement.Bind("@RealtimeWindPower", item.RealtimeWindPower);
                statement.Bind("@Sunrise", item.Sunrise);
                statement.Bind("@Sunset", item.Sunset);
                statement.Step();
            }
        }

        public override List<RealtimeDetail> SelectGroupItems(long foreignId)
        {
            throw new NotImplementedException();
        }

        public override RealtimeDetail SelectSingleItemById(long id)
        {
            RealtimeDetail d = null;
            using (var statement = conn.Prepare(GetSelectSQL()))
            {
                statement.Bind("@CountyId", id);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    d = CreateItem(statement);
                }
            }
            return d;
        }

        public override void UpdateItems(List<RealtimeDetail> items)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSingleItem(RealtimeDetail item)
        {
            throw new NotImplementedException();
        }

        protected override string GetInsertSQL()
        {
            return @"INSERT INTO RealTimeDetail 
                (CountyId, CountyName, UpdateTime, RealtimeTemp, Humidity, RealtimeWindDirection, RealtimeWindPower, Sunrise, Sunset) 
                VALUES(@CountyId, @CountyName, @UpdateTime, @RealtimeTemp, @Humidity, @RealtimeWindDirection, @RealtimeWindPower, @Sunrise, @Sunset)";
        }

        protected override string GetSelectAllSQL()
        {
            throw new NotImplementedException();
        }

        protected override string GetSelectSQL()
        {
            return "SELECT * FROM RealTimeDetail WHERE CountyId = @CountyId";
        }

        protected string GetDeleteSQL()
        {
            return "DELETE FROM RealTimeDetail WHERE CountyId = @CountyId";
        }
    }
}
