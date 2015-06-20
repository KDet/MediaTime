using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository
{
    public sealed class TvShowRepository : BaseMultimediaRepository, ITvShowRepository
    {
        public TvShowRepository(IHtmlPageLoaderService htmlPageLoaderService)
            : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/video/tvshow/", BaseUrl);
        }

        public async Task<string[]> GetCustomLanguagesAsync()
        {
            if (CurrentHtmlDocument == null)
                CurrentHtmlDocument = await HtmlPageLoaderService.LoadPageAsync(Url);

            return GetCustomValues(CurrentHtmlDocument, CustomFilter.Language).ToArray();
        }

        public async Task<Media[]> GetMediaAsync(View view, TvShowFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            return view == View.Detailed
                       ? (Media[])(await GetDetailedMediaAsync(filters, sort, page))
                       : await GetListedMediaAsync(filters, sort, page);
        }
        public async Task<MediaDetailed[]> GetDetailedMediaAsync(TvShowFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.Detailed, filters, sort, page));
            return ProcessDetailedMedia(doc).ToArray();
        }
        public async Task<MediaListed[]> GetListedMediaAsync(TvShowFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.List, filters, sort, page));
            return ProcessListedMedia(doc).ToArray();
        }
    }
}