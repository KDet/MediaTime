namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository
{
    public class GamesRepository : BaseRepository, IGamesRepository
    {
        private static TraditionalGamesRepository _traditionalGames;
        private static OnlineGamesRepository _onlineGames;
        private static CasualGamesRepository _casualGames;

        public GamesRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/games/", BaseUrl);
        }

        public TraditionalGamesRepository TraditionalGames
        {
            get { return _traditionalGames ?? (_traditionalGames = new TraditionalGamesRepository(HtmlPageLoaderService)); }
        }
        public OnlineGamesRepository OnlineGames
        {
            get { return _onlineGames ?? (_onlineGames = new OnlineGamesRepository(HtmlPageLoaderService)); }
        }
        public CasualGamesRepository CasualGames
        {
            get { return _casualGames ?? (_casualGames = new CasualGamesRepository(HtmlPageLoaderService)); }
        }
    }
}
