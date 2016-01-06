using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Schedule.db
{
    class PlanHelper : DbHelper<Plan>
    {
        private PlanHelper() : base() { }

        private static PlanHelper helper;

        public static PlanHelper GetHelper()
        {
            lock (typeof(PlanHelper))
            {
                if (helper == null)
                {
                    helper = new PlanHelper();
                }
            }
            return helper;
        }

        public override void InsertItems(List<Plan> items)
        {
            foreach (Plan plan in items)
            {
                InsertSingleItem(plan);
            }

        }

        public override long InsertSingleItem(Plan item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
         
                statement.Bind("@Title", item.Title);
                statement.Bind("@ColorId", item.ColorId);
               
                statement.Bind("@Description", item.Description);
                
                statement.Bind("@BeginDate", DateTimeSQLite(item.BeginDate));
                statement.Bind("@EndDate", DateTimeSQLite(item.EndDate));
                
                
                
                statement.Bind("@Last", item.Last);
                statement.Bind("@Loop", item.Loop);
                statement.Bind("@IsRemind", item.IsRemind);
                statement.Bind("@RemindId", item.RemindId);
                statement.Step();
               
                return 0;

            }
            
        }

        protected override String GetInsertSQL()
        {
            return "INSERT INTO Plan (Title,ColorId,Description,BeginDate,EndDate,Last,Loop,IsRemind,RemindId) VALUES (@Title,@ColorId,@Description,@BeginDate,@EndDate,@Last,@Loop,@IsRemind,@RemindId)";
        }

        public override void DeleteSingleItemById(long id)
        {
            using (var statement = conn.Prepare(GetDeleteSingleSQL()))
            {
                statement.Bind("@Id", id);
                statement.Step();
            }
        }

        public override void UpdateItems(List<Plan> items)
        {
           
        }

        public override void UpdateSingleItem(Plan item)
        {
            using (var statement = conn.Prepare(GetUpdateSingleSQL()))
            {

                statement.Bind("@Title", item.Title);
                statement.Bind("@ColorId", item.ColorId);

                statement.Bind("@Description", item.Description);

                statement.Bind("@BeginDate", DateTimeSQLite(item.BeginDate));
                statement.Bind("@EndDate", DateTimeSQLite(item.EndDate));



                statement.Bind("@Last", item.Last);
                statement.Bind("@Loop", item.Loop);
                statement.Bind("@IsRemind", item.IsRemind);
                statement.Bind("@RemindId", item.RemindId);
                statement.Bind("@Id", item.Id);
                statement.Step();

                

            }
        }

        public override List<Plan> SelectGroupItems(long foreignId)
        {
            List<Plan> items = new List<Plan>();
            using (var statement = conn.Prepare(GetSelectAllSQL()))
            {
                statement.Bind("@RemindId", foreignId);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Plan plan = CreateItem(statement);
                    items.Add(plan);
                }
            }
            return items;
        }
        public  List<Plan> SelectByDate(DateTime date)
        {
            List<Plan> items = new List<Plan>();
            using (var statement = conn.Prepare(GetSelectByDate()))
            {
               // statement.Bind("@BeginDate", DateTimeSelect(date));
                while (statement.Step() == SQLiteResult.ROW)
                {
                    DateTime d;
                    DateTime.TryParse((String)statement[4], out d);
                  
                    if (d.Date == date.Date)
                    {
                        Plan plan = CreateItem(statement);
                        items.Add(plan);
                    }
                }
            }
            return items;
        }
        public List<Plan> SelectByDoing(DateTime date)
        {
            List<Plan> items = new List<Plan>();
            using (var statement = conn.Prepare(GetSelectByDate()))
            {
                // statement.Bind("@BeginDate", DateTimeSelect(date));
                while (statement.Step() == SQLiteResult.ROW)
                {
                    DateTime d, d2;
                    DateTime.TryParse((String)statement[4], out d);
                    DateTime.TryParse((String)statement[5], out d2);
                    if (d.Date == date.Date && d <= date && d2 >= date)
                    {
                        Plan plan = CreateItem(statement);
                        items.Add(plan);
                    }
                }
            }
            return items;
        }
        public List<Plan> SelectByFurture(DateTime date)
        {
            List<Plan> items = new List<Plan>();
            using (var statement = conn.Prepare(GetSelectByDate()))
            {
                // statement.Bind("@BeginDate", DateTimeSelect(date));
                while (statement.Step() == SQLiteResult.ROW)
                {
                    DateTime d;
                    DateTime.TryParse((String)statement[4], out d);
                    
                    if ( d>date)
                    {
                        Plan plan = CreateItem(statement);
                        items.Add(plan);
                    }
                }
            }
            return items;
        }
        public List<Plan> SelectByAgo(DateTime date)
        {
            List<Plan> items = new List<Plan>();
            using (var statement = conn.Prepare(GetSelectByDate()))
            {
                // statement.Bind("@BeginDate", DateTimeSelect(date));
                while (statement.Step() == SQLiteResult.ROW)
                {
                    DateTime d;
                    DateTime.TryParse((String)statement[5], out d);
                    if (d < date)
                    {
                        Plan plan = CreateItem(statement);
                        items.Add(plan);
                    }
                }
            }
            return items;
        }
        protected String GetUpdateSingleSQL()
        {
            return "UPDATE Plan SET Title = @Title,ColorId = @ColorId,Description = @Description,BeginDate = @BeginDate,EndDate = @EndDate,Last = @Last,Loop = @Loop,IsRemind = @IsRemind,RemindId = @RemindId WHERE Id = @Id";
        }
        protected String GetDeleteSingleSQL()
        {
            return "DELETE  FROM Plan WHERE Id=@Id";
        }
        protected  String GetSelectByDate()
        {
            return "SELECT * FROM Plan";
        }
        protected override String GetSelectAllSQL()
        {
            return "SELECT * FROM Plan WHERE RemindId = @RemindId";
        }

        public override Plan SelectSingleItemById(long id)
        {
            Plan plan = null;
            using (var statement = conn.Prepare(GetSelectSQL()))
            {
                statement.Bind("@Id", id);
                if (statement.Step() == SQLiteResult.ROW)
                {
                    plan = CreateItem(statement);
                }
            }
            return plan;
        }

        protected override String GetSelectSQL()
        {
            return "SELECT * FROM Plan WHERE Id = @Id";
        }

        public override Plan CreateItem(ISQLiteStatement statement)
        {
            Plan plan = new Plan();
           
          
            plan.Id = (long)statement[0];
            plan.Title = (String)statement[1];
            plan.ColorId = (long)statement[2];
            
           
            plan.Description = (String)statement[3];
            DateTime date;
            DateTime.TryParse((String)statement[4], out date);
            plan.BeginDate = date;
            DateTime.TryParse((String)statement[5], out date);
            plan.EndDate = date;
            plan.Last = (long)statement[6];
            plan.Loop = Convert.ToInt16( statement[7]);
            plan.IsRemind = Convert.ToInt16(statement[8]);
            plan.RemindId = (long)statement[9];
            return plan;
        }
        private String DateTimeSQLite(DateTime dateTime)
        {
            String dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        }
        
    }
}