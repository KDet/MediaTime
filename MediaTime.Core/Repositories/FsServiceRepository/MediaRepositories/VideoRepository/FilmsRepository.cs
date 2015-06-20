using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository
{
    public sealed class FilmsRepository : BaseMultimediaRepository, IFilmsRepository
    {
        public FilmsRepository(IHtmlPageLoaderService htmlPageLoaderService)
            : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/video/films/", BaseUrl);
        }

        public async Task<string[]> GetCustomLanguagesAsync()
        {
            if (CurrentHtmlDocument == null)
                CurrentHtmlDocument = await HtmlPageLoaderService.LoadPageAsync(Url);

            return GetCustomValues(CurrentHtmlDocument, CustomFilter.Language).ToArray();
        }
        public async Task<string[]> GetCustomTranslateAsync()
        {
            if (CurrentHtmlDocument == null)
                CurrentHtmlDocument = await HtmlPageLoaderService.LoadPageAsync(Url);

            return GetCustomValues(CurrentHtmlDocument, CustomFilter.Translation).ToArray();
        }
        public async Task<Media[]> GetMediaAsync(View view, FilmsFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            return view == View.Detailed
                ? (Media[])(await GetDetailedMediaAsync(filters, sort, page))
                : await GetListedMediaAsync(filters, sort, page);
        }

#if NextVersionStaff
        public IEnumerable<Media> GroupMediaBy(View view, FilmGroup group, object value, Sort sort = Sort.Default, int page = 0)
        {
            return view == View.Detailed
                       ? (IEnumerable<Media>)GroupListedMediaBy(group, value, sort, page)
                       : GroupDetailedMediaBy(group, value, sort, page);
        }

        public async Task<Media[]> GroupMediaByAsync(View view, FilmGroup group, object value, Sort sort = Sort.Default, int page = 0)
        {
            return await Task.Run(() => GroupMediaBy(view, group, value, sort, page).ToArray());
        }

#endif
        public async Task<MediaDetailed[]> GetDetailedMediaAsync(FilmsFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.Detailed, filters, sort, page));
            return ProcessDetailedMedia(doc).ToArray();
        }

#if NextVersionStaff
        public IEnumerable<MediaDetailed> GroupDetailedMediaBy(FilmGroup group, object value, Sort sort = Sort.Default, int page = 0)
        {
            var query = String.Format("{0}{3}/{4}/?sort={1}&view=detailed&page={2}", Url,
                                      ((Sort)((int)sort)).ToString().ToLower(), page, group.ToString().ToLower(),
                                      value);
            return ProcessDetailedMedia(query);
        }


        public async Task<MediaDetailed[]> GroupDetailedMediaByAsync(FilmGroup group, object value, Sort sort = Sort.Default, int page = 0)
        {
            return await Task.Run(() => GroupDetailedMediaBy(group, value, sort, page).ToArray());
        }
#endif
        public async Task<MediaListed[]> GetListedMediaAsync(FilmsFilters filters, Sort sort = Sort.Default, int page = 0)
        {
            var doc = await HtmlPageLoaderService.LoadPageAsync(HelpComputeQuery(View.List, filters, sort, page));
            return ProcessListedMedia(doc).ToArray();
        }

#if NextVersionStaff
        public IEnumerable<MediaListed> GroupListedMediaBy(FilmGroup group, object value, Sort sort = Sort.Default, int page = 0)
        {
            var query = String.Format("{0}{3}/{4}/?sort={1}&view=list&page={2}", Url,
                                      ((Sort)((int)sort)).ToString().ToLower(), page, group.ToString().ToLower(),
                                      value);
            return ProcessListedMedia(query);
        }

        public async Task<MediaListed[]> GroupListedMediaByAsync(FilmGroup group, object value, Sort sort = Sort.Default, int page = 0)
        {
            return await Task.Run(() => GroupListedMediaBy(group, value, sort, page).ToArray());
        }
#endif
    }
}
