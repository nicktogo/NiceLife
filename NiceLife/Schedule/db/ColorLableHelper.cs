﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
namespace NiceLife.Schedule.db
{
    class ColorLableHelper : DbHelper<ColorLable>
    {
        private ColorLableHelper() : base() { }

        private static ColorLableHelper helper;

        public static ColorLableHelper GetHelper()
        {
            lock (typeof(ColorLableHelper))
            {
                if (helper == null)
                {
                    helper = new ColorLableHelper();
                }
            }
            return helper;
        }

        public override void InsertItems(List<ColorLable> items)
        {
            foreach (ColorLable color in items)
            {
                InsertSingleItem(color);
            }

        }

        public override long InsertSingleItem(ColorLable item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                
                statement.Bind("@Color", item.Color);
                statement.Bind("@Mean", item.Mean);


                statement.Step();
               // conn.Prepare("select last_insert_rowid()");
                return 0;
            }
        }

        protected override String GetInsertSQL()
        {
            return "INSERT INTO ColorLable (Color,Mean) VALUES (@Color,@Mean)";
        }

        public override void DeleteSingleItemById(long id)
        {
            using (var statement = conn.Prepare(GetDeleteSingleSQL()))
            {
                statement.Bind("@Id", id);
                statement.Step();
            }
        }

        public override void UpdateItems(List<ColorLable> items)
        {

        }

        public override void UpdateSingleItem(ColorLable item)
        {
            using (var statement = conn.Prepare(GetUpdateSingleSQL()))
            {
                statement.Bind("@Color", item.Color);
                statement.Bind("@Mean", item.Mean);
                statement.Bind("@Id", item.Id);
                statement.Step();
            }
        }

        public override List<ColorLable> SelectGroupItems(long Id)
        {
            List<ColorLable> items = new List<ColorLable>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                statement.Bind("@Id", Id);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    ColorLable c = CreateItem(statement);
                    items.Add(c);
                }
            }
            return items;
        }

        public  List<ColorLable> SelectlistItems()
        {
            List<ColorLable> items = new List<ColorLable>();
            using (var statement = conn.Prepare(GetSelectlistSQL()))
            {
                
                while (statement.Step() == SQLiteResult.ROW)
                {
                    ColorLable c = CreateItem(statement);
                    items.Add(c);
                }
            }
            return items;
        }
        protected string GetDeleteSingleSQL()
        {
            return "DELETE FROM ColorLable WHERE Id=@Id";
        }
        protected string GetUpdateSingleSQL()
        {
            return "UPDATE ColorLable SET Color =@Color,Mean =@Mean WHERE Id = @Id";
        }
        protected override String GetSelectAllSQL()
        {
            return "SELECT * FROM ColorLable WHERE Id = @Id";
        }
        protected  String GetSelectlistSQL()
        {
            return "SELECT * FROM ColorLable";
        }

        public override ColorLable SelectSingleItemById(long id)
        {
            ColorLable c = null;
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
            return "SELECT * FROM ColorLable WHERE Id = @Id";
        }

        public override ColorLable CreateItem(ISQLiteStatement statement)
        {
            ColorLable c = new ColorLable();

            c.Id = (long)statement[0];
            c.Color = (String)statement[1];
            c.Mean = (String)statement[2];
           
            return c;
        }
    }
}
