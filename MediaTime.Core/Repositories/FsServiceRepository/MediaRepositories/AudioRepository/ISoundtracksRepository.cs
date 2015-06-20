using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository
{
    public interface ISoundtracksRepository : ISubCategoryRepository
    {
        Task<Media[]> GetMediaAsync(View view, SoundtrackFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaDetailed[]> GetDetailedMediaAsync(SoundtrackFilters filters, Sort sort = Sort.Default, int page = 0);
        Task<MediaListed[]> GetListedMediaAsync(SoundtrackFilters filters, Sort sort = Sort.Default, int page = 0);
    }
}