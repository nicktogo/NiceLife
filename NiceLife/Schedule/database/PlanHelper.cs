using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Schedule.database
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

        public override void InsertSingleItem(Plan item)
        {
            using (var statement = conn.Prepare(GetInsertSQL()))
            {
                statement.Bind("@Title", item.Title);
                statement.Bind("@Description", item.Description);
                
                statement.Bind("@BeginDate", item.BeginDate);
                statement.Bind("@EndDate", item.EndDate);
                statement.Bind("@ColorId", item.ColorId);
                statement.Bind("@RemindId", item.RemindId);
                statement.Bind("@IsRemind", item.IsRemind);
                statement.Bind("@Last", item.Last);
                statement.Bind("@Loop", item.Loop);

                statement.Step();
            }
        }

        protected override String GetInsertSQL()
        {
            return "INSERT INTO Plan (Title,ColorId,RemindId,BeginDate,EndDate,IsRemind,Last,Loop,Description) VALUES (@Title,@ColorId,@RemindId,@BeginDate,@EndDate,@IsRemind,@Last,@Loop,@Description)";
        }

        public override void DeleteSingleItemById(long id)
        {
            throw new NotImplementedException();
        }

        public override void UpdateItems(List<Plan> items)
        {
           
        }

        public override void UpdateSingleItem(Plan item)
        {
            throw new NotImplementedException();
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
            plan.RemindId = (long)statement[3];
            plan.IsRemind = (int)statement[4];
            plan.Description = (String)statement[5];
            plan.BeginDate = (DateTime)statement[6];
            plan.EndDate = (DateTime)statement[7];
            plan.Last = (long)statement[8];
            plan.Loop = (int)statement[9];
            return plan;
        }
    }
}