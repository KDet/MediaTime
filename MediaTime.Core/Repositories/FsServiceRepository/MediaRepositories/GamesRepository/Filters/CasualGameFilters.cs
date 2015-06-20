using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository.Filters
{
    public class CasualGameFilters : IFilters
    {
        private readonly FiltersDictionary _filters = new FiltersDictionary();

        public FiltersDictionary Filters
        {
            get { return _filters; }
        }

        public Platform Platform
        {
            get { return (Platform)Filters["Platform"]; }
            set { Filters["Platform"] = value; }
        }

        public GameKind GameKind
        {
            get { return (GameKind)Filters["GameKind"]; }
            set { Filters["GameKind"] = value; }
        }

        public CasualGameFilters()
        {
            Platform = Platform.None;
            GameKind = GameKind.None;
        }

        public bool HasFilter()
        {
            return Platform != Platform.None || GameKind != GameKind.None;
        }
    }
}
