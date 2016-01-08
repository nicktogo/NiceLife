using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace NiceLife.Tomato
{
    public abstract class DBHelper<T>
    {
        protected SQLiteConnection conn;

        protected DBHelper()
        {
            conn = new SQLiteConnection(App.DB_NAME);
        }

        public abstract void InsertItems(List<T> items);

        public abstract void InsertSingleItem(T item);

        protected abstract String GetInsertSQL();

        public abstract void DeleteSingleItem(T item);

        public abstract void UpdateDoneTomato(T item);

        public abstract void UpdateStatus(T item);

        public abstract List<T> SelectGroupItemsByDate(DateTime date);

        public abstract List<DateTime> SelectDate();

        public abstract List<DateTime> SelectAllDate();

        public abstract List<T> SelectAllItems();

        protected abstract String GetSelectTaskByDateSQL();

        protected abstract String GetSelectDateSQL();

        protected abstract String GetSelectAllDateSQL();

        protected abstract String GetSelectAllSQL();

        public abstract T SelectSingleItemById(long id);

        public abstract T CreateItem(ISQLiteStatement statement);





        public abstract int numOfAllTask();

        public abstract int numOfDoneTask();

        public abstract int numOfAllTaskByDate(DateTime date);

        public abstract int numOfDoneTaskByDate(DateTime date);


        public abstract int numOfAllTomato();

        public abstract int numOfDoneTomato();

        public abstract int numOfAllTomatoByDate(DateTime date);

        public abstract int numOfDoneTomatoByDate(DateTime date);

    }
}
