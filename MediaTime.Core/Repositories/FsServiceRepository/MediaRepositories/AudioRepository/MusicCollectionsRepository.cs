using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository
{
    public sealed class MusicCollectionsRepository : BaseMultimediaRepository, IMusicCollectionsRepository
    {
        public MusicCollectionsRepository(IHtmlPageLoaderService htmlPageLoaderService)
            : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/audio/collections/", BaseUrl);
        }

        public async Task<Media[]> GetMediaAsync(View view, CollectionFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            return view == View.Detailed
                       ? (Media[])(await GetDetailedMediaAsync(filters, sort, page))
                       : await GetListedMediaAsync(filters, sort, page);
        }
        public async Task<MediaDetailed[]> GetDetailedMediaAsync(CollectionFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.Detailed, filters, sort, page));
            return ProcessDetailedMedia(doc).ToArray();
        }
        public async Task<MediaListed[]> GetListedMediaAsync(CollectionFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.List, filters, sort, page));
            return ProcessListedMedia(doc).ToArray();
        }
    }
}