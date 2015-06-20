using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository
{
    public interface IFilmsRepository : ISubCategoryRepository
    {
        Task<string[]> GetCustomLanguagesAsync();
        Task<string[]> GetCustomTranslateAsync();
        Task<Media[]> GetMediaAsync(View view, FilmsFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(FilmsFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(FilmsFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}