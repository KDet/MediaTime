using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository
{
    public class TextsRepository : BaseRepository, ITextsRepository
    {
        private static FictionLiteratureRepository _fictionLiterature;
        private static AppliedLiteratureRepository _appliedLiterature;
        private static JournalsRepository _journals;
        private static ComixRepository _comix;

        public TextsRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/texts/", BaseUrl);
        }

        public FictionLiteratureRepository FictionLiterature
        {
            get { return _fictionLiterature ?? (_fictionLiterature = new FictionLiteratureRepository(HtmlPageLoaderService)); }
        }
        public AppliedLiteratureRepository AppliedLiterature
        {
            get { return _appliedLiterature ?? (_appliedLiterature = new AppliedLiteratureRepository(HtmlPageLoaderService)); }
        }
        public JournalsRepository Journals
        {
            get { return _journals ?? (_journals = new JournalsRepository(HtmlPageLoaderService)); }
        }
        public ComixRepository Comix
        {
            get { return _comix ?? (_comix = new ComixRepository(HtmlPageLoaderService)); }
        }
    }

    public abstract class LiteratureFilterableRepository : BaseMultimediaRepository
    {
        protected LiteratureFilterableRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService) { }

        public async Task<string[]> GetCustomLanguagesAsync()
        {
            if (CurrentHtmlDocument == null)
                CurrentHtmlDocument = await HtmlPageLoaderService.LoadPageAsync(Url);

            return GetCustomValues(CurrentHtmlDocument, CustomFilter.Language).ToArray();
        }
    }
}
