﻿using SQLitePCL;
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

            task.Id = (long)statement[0];
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
                statement.Bind("@Date", item.Date.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                statement.Bind("@Type", item.Type);
                statement.Bind("@Status", item.Status);
                statement.Bind("@TotalTomato", item.TotalTomato);
                statement.Bind("@DoneTomato", item.DoneTomato);

                statement.Step();
            }
        }

        public override List<Task> SelectGroupItemsByDate(DateTime date)
        {
            List<Task> items = new List<Task>();
            using (var statement = conn.Prepare(GetSelectTaskByDateSQL()))
            {
                statement.Bind("@Date", date.Date.ToString("yyyy-MM-dd HH:mm:ss.fff")); 
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Task task = CreateItem(statement);
                    items.Add(task);
                }
            }
            return items;
        }

        public override List<DateTime> SelectDate()
        {
            List<DateTime> items = new List<DateTime>();
            using (var statement = conn.Prepare(GetSelectDateSQL()))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    DateTime date;
                    DateTime.TryParse((String)statement[0], out date);
                    items.Add(date);
                }
            }
            return items;
        }

        public override List<DateTime> SelectAllDate()
        {
            List<DateTime> items = new List<DateTime>();
            using (var statement = conn.Prepare(GetSelectAllDateSQL()))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    DateTime date;
                    DateTime.TryParse((String)statement[0], out date);
                    items.Add(date);
                }
            }
            return items;
        }

        public override List<Task> SelectAllItems()
        {
            List<Task> items = new List<Task>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Task task = CreateItem(statement);
                    items.Add(task);
                }
            }
            return items;
        }

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

        protected override string GetSelectTaskByDateSQL()
        {
            return "SELECT * FROM Task WHERE Date Between date(@Date, 'start of day') and datetime(@Date, '+1 day', 'start of day')";
        }

        protected override string GetSelectDateSQL()
        {
            return "select DISTINCT strftime('%Y-%m-%d', Date) from Task where strftime('%Y-%m-%d', Date) >= strftime('%Y-%m-%d', 'now') order by Date ASC";
        }

        protected string GetDeleteAllSQL()
        {
            return "DELETE FROM Task";
        }

        protected override string GetSelectAllSQL()
        {
            return "SELECT * FROM Task order by Date ASC";
        }

        protected override string GetSelectAllDateSQL()
        {
            return "select DISTINCT strftime('%Y-%m-%d', Date) from Task order by Date ASC";
        }
    }
}
