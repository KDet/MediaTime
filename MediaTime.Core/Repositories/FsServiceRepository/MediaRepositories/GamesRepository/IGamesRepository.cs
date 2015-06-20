namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository
{
    public interface IGamesRepository : ICategoryRepository
    {
        TraditionalGamesRepository TraditionalGames { get; }
        OnlineGamesRepository OnlineGames { get; }
        CasualGamesRepository CasualGames { get; }
    }
}