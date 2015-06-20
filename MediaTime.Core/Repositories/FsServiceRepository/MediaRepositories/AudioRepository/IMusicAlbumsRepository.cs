using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository
{
    public interface IMusicAlbumsRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, AlbumFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(AlbumFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(AlbumFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}