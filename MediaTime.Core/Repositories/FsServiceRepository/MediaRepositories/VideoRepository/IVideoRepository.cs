namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository
{
    public interface IVideoRepository : ICategoryRepository
    {
        FilmsRepository Films { get; }
        SerialsRepository Serials { get; }
        CartoonsRepository Cartoons { get; }
        CartoonSerialsRepository CartoonSerials { get; }
        TvShowRepository TvShow { get; }
        ClipsRepository Clips { get; }
        ConcertsRepository Concerts { get; }
    }
}