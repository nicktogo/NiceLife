using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
namespace NiceLife.Schedule.database
{
    class CallHelper : DbHelper<Call>
    {
        private CallHelper() : base() { }

        private static CallHelper helper;

        public static CallHelper GetHelper()
        {
            lock (typeof(CallHelper))
            {
                if (helper == null)
                {
                    helper = new CallHelper();
                }
            }
            return helper;
        }

        public override void InsertItems(List<Call> items)
        {
            foreach (Call call in items)
            {
                InsertSingleItem(call);
            }

        }

        public override void InsertSingleItem(Call item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                
                statement.Bind("@Date", item.Date);
                statement.Bind("@Type", item.Type);
                statement.Bind("@State", item.State);


                statement.Step();
            }
        }
       

        protected override String GetInsertSQL()
        {
            return "INSERT INTO Call (Date,Type,State) VALUES (@Date,@Type,@State)";
        }

        public override void DeleteSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void UpdateItems(List<Call> items)
        {

        }

        public override void UpdateSingleItem(Call item)
        {
            throw new NotImplementedException();
        }

        public override List<Call> SelectGroupItems(long Id)
        {
            List<Call> items = new List<Call>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                statement.Bind("@Id", Id);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Call c = CreateItem(statement);
                    items.Add(c);
                }
            }
            return items;
        }

        protected override String GetSelectAllSQL()
        {
            return "SELECT * FROM Call WHERE Id = @Id";
        }

        public override Call SelectSingleItemById(long id)
        {
            Call c = null;
            using (var statement = conn.Prepare(GetSelectSQL()))
            {
                statement.Bind("@Id", id);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    c = CreateItem(statement);
                }
            }
            return c;
        }

        protected override String GetSelectSQL()
        {
            return "SELECT * FROM Call WHERE Id = @Id";
        }

        public override Call CreateItem(ISQLiteStatement statement)
        {
            Call c = new Call();

            c.Id = (long)statement[0];
            c.Date = (DateTime)statement[1];
            c.Type = (String)statement[2];
            c.State = (int)statement[3];
            return c;
        }
    }
}
