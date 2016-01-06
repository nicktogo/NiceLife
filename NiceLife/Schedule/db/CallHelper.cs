using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
namespace NiceLife.Schedule.db
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

        public override long InsertSingleItem(Call item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                
                statement.Bind("@Date", DateTimeSQLite(item.Date));
                statement.Bind("@Type", item.Type);
                statement.Bind("@State", item.State);
                statement.Bind("@PlanId", item.PlanId);


                statement.Step();
                return 0;
            }
        }
       

        protected override String GetInsertSQL()
        {
            return "INSERT INTO Call (Date,Type,State,PlanId) VALUES (@Date,@Type,@State,@PlanId)";
        }

        public override void DeleteSingleItemById(long id)
        {
            using (var statement = conn.Prepare(GetDeleteSingleSQL()))
            {
                statement.Bind("@Id", id);
                statement.Step();
            }
        }

        public override void UpdateItems(List<Call> items)
        {

        }

        public override void UpdateSingleItem(Call item)
        {
            using (var statement = conn.Prepare(GetUpdateSingleSQL()))
            {

                statement.Bind("@Date", DateTimeSQLite(item.Date));
                statement.Bind("@Type", item.Type);
                statement.Bind("@State", item.State);
                statement.Bind("@Id", item.Id);

                statement.Step();
              
                
            }
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
        public  List<Call> SelectlistItems(DateTime date)
        {
            List<Call> items = new List<Call>();
            using (var statement = conn.Prepare(GetSelectlistSQL()))
            {
             
                while (statement.Step() == SQLiteResult.ROW)
                {
                    DateTime d;
                    DateTime.TryParse((String)statement[1], out d);
                    if (d.Date == date.Date)
                    {
                        Call c = CreateItem(statement);
                        items.Add(c);
                    }
                }
            }
            return items;
        }
        protected string GetUpdateSingleSQL()
        {
            return "UPDATE  Call SET Date=@Date,Type=@Type,State = @State WHERE Id=@Id";
        }
        protected string GetDeleteSingleSQL()
        {
            return "DELETE FROM Call WHERE Id=@Id";
        }
        protected  String GetSelectlistSQL()
        {
            return "SELECT * FROM Call ";
        }
        protected override String GetSelectAllSQL()
        {
            return "SELECT * FROM Call WHERE Id = @Id";
        }

        public override Call SelectSingleItemById(long Pid)
        {
            Call c = null;
            using (var statement = conn.Prepare(GetSelectSQL()))
            {
                statement.Bind("@PlanId", Pid);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    c = CreateItem(statement);
                }
            }
            return c;
        }

        protected override String GetSelectSQL()
        {
            return "SELECT * FROM Call WHERE PlanId = @PlanId";
        }

        public override Call CreateItem(ISQLiteStatement statement)
        {
            Call c = new Call();

            c.Id = (long)statement[0];
            DateTime date;
            DateTime.TryParse((String)statement[1], out date);
            c.Date =date;
            c.Type = (String)statement[2];
            c.State = (long)statement[3];
            c.PlanId=(long)statement[4];
            return c;
        }
        private String DateTimeSQLite(DateTime dateTime)
        {
            String dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        }
    }
}
