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

        public abstract void DeleteSingleItemById(long id);

        public abstract void UpdateItems(List<T> items);

        public abstract void UpdateSingleItem(T item);

        public abstract List<T> SelectGroupItemsByDate(DateTime date);

        protected abstract String GetSelectAllSQL();

        public abstract T SelectSingleItemById(long id);

        protected abstract String GetSelectSQL();

        public abstract T CreateItem(ISQLiteStatement statement);
    }
}
