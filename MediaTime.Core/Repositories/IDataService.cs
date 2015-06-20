using System.Collections.Generic;
using MediaTime.Core.Model;

namespace MediaTime.Core.Repositories
{
    public interface IDataService
    {
        List<Media> MediaMatching(string nameFilter);
        int Insert(Media media);
        int Update(Media media);
        int Delete(Media media);
        List<Media> GetAllItems();
        int Count { get; }
    }
}