using System;
using System.Diagnostics;
using System.Linq;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using MediaTime.Core.Model;
using MediaTime.Core.Services;
using SQLite.Net;
using SQLite.Net.Async;

namespace MediaTime.Core.Repositories
{
    public class FavoriteRepository : IDisposable, IFavoriteRepository
    {
        private const string Filename = "favorite.sql";
        private readonly DataService<FavoriteMedia> _dataService;
        public FavoriteRepository(ISQLiteConnectionFactory factory)
        {
            var connection = factory.Create(Filename);
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
            var favoriteMedia = new FavoriteMedia
            {
                Image = media.Image,
                SubTitle = media.SubTitle,
                Title = media.Title,
                Url = media.Url
            };
            //favoriteMedia.Id = Count + 1;
            return _dataService.Insert(favoriteMedia);
        }

        public int Delete(Media media)
        {
            var mediaToDelete = _dataService.Get(favoriteMedia => Media.Equals(favoriteMedia, media));
            return _dataService.Delete(mediaToDelete);
        }

        public IQueryable<Media> GetAllItems()
        {
            return _dataService.GetAllItems();
        }

        public int Count
        {
            get { return _dataService.Count; }
        }
    }
}
