using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository
{
    public interface IFictionLiteratureRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, FictionLiteratureFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(FictionLiteratureFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(FictionLiteratureFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<string[]> GetCustomLanguagesAsync();
    }
}