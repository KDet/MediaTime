using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository
{
    public interface ITvShowRepository : ISubCategoryRepository
    {
        Task<string[]> GetCustomLanguagesAsync();
        Task<Media[]> GetMediaAsync(View view, TvShowFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(TvShowFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(TvShowFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}