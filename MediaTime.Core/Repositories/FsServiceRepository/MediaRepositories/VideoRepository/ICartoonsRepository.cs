using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository
{
    public interface ICartoonsRepository : ISubCategoryRepository
    {
        Task<string[]> GetCustomLanguagesAsync();
        Task<string[]> GetCustomTranslateAsync();
        Task<Media[]> GetMediaAsync(View view, CartoonsFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(CartoonsFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(CartoonsFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}