using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Weather.Database
{
    public class CountyHelper : DbHelper<County>
    {
        private CountyHelper() : base() { }

        private static CountyHelper helper;

        public static CountyHelper GetHelper()
        {
            lock (typeof(CountyHelper))
            {
                if (helper == null)
                {
                    helper = new CountyHelper();
                }
            }
            return helper;
        }

        public override County CreateItem(ISQLiteStatement statement)
        {
            County county = new County();
            county.Id = (long)statement[0];
            county.Name = (String)statement[1];
            county.Code = (String)statement[2];
            county.CityId = (long)statement[3];
            county.CountySelect = (long)statement[4];

            return county;
        }

        public override void DeleteSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void InsertItems(List<County> items)
        {
            foreach (County county in items)
            {
                InsertSingleItem(county);
            }
        }

        public override void InsertSingleItem(County item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                statement.Bind("@Name", item.Name);
                statement.Bind("@Code", item.Code);
                statement.Bind("@CityId", item.CityId);
                statement.Bind("@CountySelect", item.CountySelect);

                statement.Step();
            }
        }

        public override List<County> SelectAllItems()
        {
            List<County> items = new List<County>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    County county = CreateItem(statement);
                    items.Add(county);
                }
            }
            return items;
        }

        public override County SelectSingleItemById(long id)
        {
            County county = null;
            using (var statement = conn.Prepare(GetSelectSQL()))
            {
                statement.Bind("@Id", id);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    county = CreateItem(statement);
                }
            }
            return county;
        }

        public override void UpdateItems(List<County> items)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSingleItem(County item)
        {
            using (var statement = conn.Prepare(GetUpdateCountySelectSQL()))
            {
                statement.Bind("@CountySelect", item.CountySelect);
                statement.Bind("@Id", item.Id);
                statement.Step();
            }
        }

        protected override string GetInsertSQL()
        {
            return "INSERT INTO County (Name, Code, CityId) VALUES (@Name, @Code, @CityId)";
        }

        protected override string GetSelectAllSQL()
        {
            return "SELECT * FROM County";
        }

        protected override string GetSelectSQL()
        {
            return "SELECT * FROM County WHERE Id = @Id";
        }

        protected string GetUpdateCountySelectSQL()
        {
            return "UPDATE County SET CountySelect = @CountySelect WHERE Id = @Id";
        }
    }
}
