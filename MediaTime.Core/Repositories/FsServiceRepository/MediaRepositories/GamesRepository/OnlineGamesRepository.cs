using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository
{
    public sealed class OnlineGamesRepository : BaseMultimediaRepository, IOnlineGamesRepository
    {
        public OnlineGamesRepository(IHtmlPageLoaderService htmlPageLoaderService)
            : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/games/online/", BaseUrl);
        }

        public async Task<Media[]> GetMediaAsync(View view, OnlineGameFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            return view == View.Detailed
                       ? (Media[])(await GetDetailedMediaAsync(filters, sort, page))
                       : await GetListedMediaAsync(filters, sort, page);
        }
        public async Task<MediaDetailed[]> GetDetailedMediaAsync(OnlineGameFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.Detailed, filters, sort, page));
            return ProcessDetailedMedia(doc).ToArray();
        }
        public async Task<MediaListed[]> GetListedMediaAsync(OnlineGameFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.List, filters, sort, page));
            return ProcessListedMedia(doc).ToArray();
        }
    }
}