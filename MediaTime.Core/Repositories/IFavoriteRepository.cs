using System.Linq;
using MediaTime.Core.Model;

namespace MediaTime.Core.Repositories
{
    public interface IFavoriteRepository
    {
        IQueryable<Media> Find(string nameFilter);
        int Insert(Media media);
        int Delete(Media media);
        IQueryable<Media> GetAllItems();
        int Count { get; }
    }
}