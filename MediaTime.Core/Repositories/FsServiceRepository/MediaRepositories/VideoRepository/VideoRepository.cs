namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository
{
    public class VideoRepository : BaseRepository, IVideoRepository
    {
        private static FilmsRepository _films;
        private static SerialsRepository _serials;
        private static CartoonsRepository _cartoons;
        private static CartoonSerialsRepository _cartoonSerials;
        private static TvShowRepository _tvShow;
        private static ClipsRepository _clips;
        private static ConcertsRepository _concerts;

        public VideoRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/video/", BaseUrl);
        }

        public FilmsRepository Films
        {
            get { return _films ?? (_films = new FilmsRepository(HtmlPageLoaderService)); }
        }
        public SerialsRepository Serials
        {
            get { return _serials ?? (_serials = new SerialsRepository(HtmlPageLoaderService)); }
        }
        public CartoonsRepository Cartoons
        {
            get { return _cartoons ?? (_cartoons = new CartoonsRepository(HtmlPageLoaderService)); }
        }
        public CartoonSerialsRepository CartoonSerials
        {
            get { return _cartoonSerials ?? (_cartoonSerials = new CartoonSerialsRepository(HtmlPageLoaderService)); }
        }
        public TvShowRepository TvShow
        {
            get { return _tvShow ?? (_tvShow = new TvShowRepository(HtmlPageLoaderService)); }
        }
        public ClipsRepository Clips
        {
            get { return _clips ?? (_clips = new ClipsRepository(HtmlPageLoaderService)); }
        }
        public ConcertsRepository Concerts
        {
            get { return _concerts ?? (_concerts = new ConcertsRepository(HtmlPageLoaderService)); }
        }

    }
}