using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository
{
    public interface ICasualGamesRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, CasualGameFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(CasualGameFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(CasualGameFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}