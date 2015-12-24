using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLitePCL;

namespace NiceLife.Weather.Database
{
    public class ProvinceHelper : DbHelper<Province>
    {

        protected ProvinceHelper() : base() { }

        protected static ProvinceHelper helper;

        public static ProvinceHelper GetHelper()
        {
            lock (typeof(ProvinceHelper))
            {
                if (helper == null)
                {
                    helper = new ProvinceHelper();
                }
            }
            return helper;
        }

        public override void DeleteSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void InsertItems(List<Province> items)
        {
            foreach (Province item in items)
            {
                InsertSingleItem(item);
            }
        }

        public override void InsertSingleItem(Province item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                statement.Bind("@Name", item.Name);
                statement.Bind("@Code", item.Code);
                statement.Step();
            }
        }

        protected override string GetInsertSQL()
        {
            return "INSERT INTO Province (Name, Code) VALUES (@Name, @Code)";
        }

        public override List<Province> SelectGroupItems(long foreignId)
        {
            List<Province> items = new List<Province>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    var item = CreateItem(statement);
                    items.Add(item);
                }
            }
            return items;
        }

        protected override string GetSelectAllSQL()
        {
            return "SELECT * FROM Province";
        }

        public override Province SelectSingleItemById(long id)
        {
            Province country = null;
            using (var statement = conn.Prepare(GetSelectSQL()))
            {
                statement.Bind("@Id", id);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    country = CreateItem(statement);
                }
            }
            return country;
        }

        protected override string GetSelectSQL()
        {
            return "SELECT Id, Name, Code FROM Province WHERE Id = @Id";
        }

        public override void UpdateItems(List<Province> items)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSingleItem(Province item)
        {
            throw new NotImplementedException();
        }

        public override Province CreateItem(ISQLiteStatement statement)
        {
            Province country = new Province();
            country.Id = (long)statement[0];
            country.Name = (String)statement[1];
            country.Code = (String)statement[2];
            return country;
        }
    }
}
