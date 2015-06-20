using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository
{
    public interface IMusicCollectionsRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, CollectionFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(CollectionFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(CollectionFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}