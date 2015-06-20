using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using MediaTime.Core.Model;

namespace MediaTime.Core.Repositories
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

    public class FavoriteRepository :  IDisposable
    {
        private readonly DataService<FavoriteMedia> _dataService;
        public FavoriteRepository(ISQLiteConnectionFactory factory)
        {
            var connection = factory.Create("favorite.sql");
            _dataService = new DataService<FavoriteMedia>(connection);
        }
        
        public void Dispose()
        {
            if (_dataService != null)
                _dataService.Dispose();
        }

        public IQueryable<Media> Find(string nameFilter)
        {
            return _dataService.FindSorted(x => x.Title.Contains(nameFilter), x => x.Title);
        }

        

        public int Insert(Media media)
        {
            
           //FavoriteMedia favoriteMedia = 
           return _connection.Insert(media);
        }

        public int Update(Media media)
        {
            return _connection.Update(media);
        }

        public int Delete(Media media)
        {
            _connection
            return _connection.Delete(media);
        }

        public IQueryable<Media> GetAllItems()
        {
            return _connection.Table<FavoriteMedia>().AsQueryable();
        }

        public int Count
        {
            get
            {
                return _connection.Table<FavoriteMedia>().Count();
            }
        }
    }
}
