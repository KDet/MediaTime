using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository
{
    public interface IComixRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, ComixFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(ComixFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(ComixFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<string[]> GetCustomLanguagesAsync();
    }
}