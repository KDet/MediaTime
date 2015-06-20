using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository
{
    public interface IJournalsRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, JournalsFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(JournalsFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(JournalsFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<string[]> GetCustomLanguagesAsync();
    }
}