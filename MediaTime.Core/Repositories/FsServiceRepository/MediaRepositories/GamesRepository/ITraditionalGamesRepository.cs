using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository
{
    public interface ITraditionalGamesRepository : ISubCategoryRepository
    {
        Task<string[]> GetCustomLanguagesAsync();
        Task<Media[]> GetMediaAsync(View view, TraditionalGameFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(TraditionalGameFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(TraditionalGameFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}