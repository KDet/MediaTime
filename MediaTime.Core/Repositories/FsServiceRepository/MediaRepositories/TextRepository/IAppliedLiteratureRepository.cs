using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository
{
    public interface IAppliedLiteratureRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, AppliedLiteratureFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(AppliedLiteratureFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(AppliedLiteratureFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<string[]> GetCustomLanguagesAsync();
    }
}