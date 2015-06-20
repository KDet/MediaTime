namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository
{
    public class AudioRepository : BaseRepository, IAudioRepository
    {
        private static MusicAlbumsRepository _musicAlbums;
        private static SinglesRepository _singles;
        private static MusicCollectionsRepository _musicCollections;
        private static SoundtracksRepository _soundtracks;

        public AudioRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService)
        {
            Url = string.Format("{0}/audio/", BaseUrl);
        }

        public MusicAlbumsRepository MusicAlbums
        {
            get { return _musicAlbums ?? (_musicAlbums = new MusicAlbumsRepository(HtmlPageLoaderService)); }
        }
        public SinglesRepository Singles
        {
            get { return _singles ?? (_singles = new SinglesRepository(HtmlPageLoaderService)); }
        }
        public MusicCollectionsRepository MusicCollections
        {
            get { return _musicCollections ?? (_musicCollections = new MusicCollectionsRepository(HtmlPageLoaderService)); }
        }
        public SoundtracksRepository Soundtracks
        {
            get { return _soundtracks ?? (_soundtracks = new SoundtracksRepository(HtmlPageLoaderService)); }
        }
    }
}