using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Tomato.Database
{
    public class TaskHelper : DBHelper<Task>
    {
        private TaskHelper() : base() { }

        private static TaskHelper helper;

        public static TaskHelper GetHelper()
        {
            lock (typeof(TaskHelper))
            {
                if (helper == null)
                {
                    helper = new TaskHelper();
                }
            }
            return helper;
        }

        public override Task CreateItem(ISQLiteStatement statement)
        {
            Task task = new Task();

            task.Title = (String)statement[1];
            task.Description = (String)statement[2];

            DateTime Date;
            DateTime.TryParse((String)statement[3], out Date);
            task.Date = Date;

            task.Type = (String)statement[4];
            task.Status = (String)statement[5];
            task.TotalTomato = (long)statement[6];
            task.DoneTomato = (long)statement[7];

            return task;
        }

        public bool DeleteAllItems()
        {
            using (var statement = conn.Prepare(GetDeleteAllSQL()))
            {
                return statement.Step() == SQLiteResult.DONE ? true : false;
            }
        }

        public override void DeleteSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void InsertItems(List<Task> items)
        {
            foreach (Task f in items)
            {
                InsertSingleItem(f);
            }
        }

        public override void InsertSingleItem(Task item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                statement.Bind("@Title", item.Title);
                statement.Bind("@Description", item.Description);
                statement.Bind("@Date", DateTimeSQLite(item.Date));
                statement.Bind("@Type", item.Type);
                statement.Bind("@Status", item.Status);
                statement.Bind("@TotalTomato", item.TotalTomato);
                statement.Bind("@DoneTomato", item.DoneTomato);

                statement.Step();
            }
        }

        public override List<Task> SelectGroupItems(long foreignId)
        {
            throw new NotImplementedException();
        }
        //public override List<Task> SelectGroupItems(long foreignId)
        //{
        //    List<Task> items = new List<Task>();
        //    using (var statement = conn.Prepare(GetSelectAllSQL()))
        //    {
        //        statement.Bind("@CountyId", foreignId);
        //        while (statement.Step() == SQLiteResult.ROW)
        //        {
        //            Task f = CreateItem(statement);
        //            items.Add(f);
        //        }
        //    }
        //    return items;
        //}

        public override Task SelectSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void UpdateItems(List<Task> items)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSingleItem(Task item)
        {
            throw new NotImplementedException();
        }

        protected override string GetInsertSQL()
        {
            return @"INSERT INTO Task 
                (Title, Description, Date, Type, Status, TotalTomato, DoneTomato) 
                VALUES(@Title, @Description, @Date, @Type, @Status, @TotalTomato, @DoneTomato)";
        }

        protected override string GetSelectAllSQL()
        {
            return "SELECT * FROM Task WHERE Date = @Date";
        }

        protected override string GetSelectSQL()
        {
            throw new NotImplementedException();
        }

        protected string GetDeleteAllSQL()
        {
            return "DELETE FROM Task";
        }

        private String DateTimeSQLite(DateTime dateTime)
        {
            String dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        }

    }
}
