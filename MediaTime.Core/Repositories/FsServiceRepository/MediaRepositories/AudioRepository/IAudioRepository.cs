namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository
{
    public interface IAudioRepository : ICategoryRepository
    {
        MusicAlbumsRepository MusicAlbums { get; }
        SinglesRepository Singles { get; }
        MusicCollectionsRepository MusicCollections { get; }
        SoundtracksRepository Soundtracks { get; }
    }
}