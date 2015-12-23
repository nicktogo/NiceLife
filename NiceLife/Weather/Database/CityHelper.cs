using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Weather.Database
{
    public class CityHelper : DbHelper<City>
    {
        protected CityHelper() : base() { }

        protected static CityHelper helper;

        public CityHelper GetHelper()
        {
            lock (typeof(CityHelper))
            {
                if (helper == null)
                {
                    helper = new CityHelper();
                }
            }
            return helper;
        }

        public override City CreateItem(ISQLiteStatement statement)
        {
            City city = new City();
            city.Id = (long)statement[0];
            city.Name = (String)statement[1];
            city.Code = (String)statement[2];
            city.ProvinceId = (long)statement[3];

            return city;
        }

        public override void DeleteSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void InsertItems(List<City> items)
        {
            foreach (City city in items)
            {
                InsertSingleItem(city);
            }
        }

        public override void InsertSingleItem(City item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                statement.Bind("@Name", item.Name);
                statement.Bind("@Code", item.Code);
                statement.Bind("@ProvinceId", item.ProvinceId);

                statement.Step();
            }
        }

        public override List<City> SelectAllItems()
        {
            List<City> items = new List<City>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                while(statement.Step() == SQLiteResult.ROW)
                {
                    City city = CreateItem(statement);
                    items.Add(city);
                }
            }
            return items;
        }

        public override City SelectSingleItemById(long id)
        {
            City city = null;
            using (var statement = conn.Prepare(GetSelectSQL()))
            {
                statement.Bind("@Id", id);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    city = CreateItem(statement);
                }
            }
            return city;
        }

        public override void UpdateItems(List<City> items)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSingleItem(City item)
        {
            throw new NotImplementedException();
        }

        protected override string GetInsertSQL()
        {
            return "INSERT INTO City (Name, Code, ProvinceId) VALUES(@Name, @Code, @ProvinceId)";
        }

        protected override string GetSelectAllSQL()
        {
            return "SELECT * FROM City";
        }

        protected override string GetSelectSQL()
        {
            return "SELECT * FROM City WHERE Id = @Id";
        }
    }
}
