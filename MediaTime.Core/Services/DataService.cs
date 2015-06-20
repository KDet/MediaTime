using System;
using System.Linq;
using System.Linq.Expressions;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MediaTime.Core.Services
{
    public class DataService<T>: IDisposable where T : new()
    {
        private readonly ISQLiteConnection _connection;

        public DataService(ISQLiteConnectionFactory factory, string address)
            : this(factory.Create(address)) { }
        public DataService(ISQLiteConnection sqLiteConnection)
        {
            _connection = sqLiteConnection;
            _connection.CreateTable<T>();
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Close();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _connection.Table<T>()
                .Where(predicate).AsQueryable();
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _connection.Get(predicate);
        }
        public IQueryable<T> FindSorted<TKey>(Expression<Func<T, bool>> toFind, Expression<Func<T, TKey>> toSort)
        {
            return _connection.Table<T>()
                .OrderBy(toSort)
                .Where(toFind).AsQueryable();
        }
        public int Insert(T media)
        {
            return _connection.Insert(media);
        }
        public int Update(T media)
        {
            return _connection.Update(media);
        }
        public int Delete(T media)
        {
            return _connection.Delete(media);
        }
        public IQueryable<T> GetAllItems()
        {
            return _connection.Table<T>().AsQueryable();
        }
        public int Count
        {
            get
            {
                return _connection.Table<T>().Count();
            }
        }
    }
}