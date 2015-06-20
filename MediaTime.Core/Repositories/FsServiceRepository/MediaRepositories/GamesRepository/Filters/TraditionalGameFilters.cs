using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository.Filters
{
    public class TraditionalGameFilters : IFilters
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

        public GameLanguage Language
        {
            get { return (GameLanguage)Filters["Language"]; }
            set { Filters["Language"] = value; }
        }

        public string LanguageCustom
        {
            get { return (string)Filters["LanguageCustom"]; }
            set { Filters["LanguageCustom"] = value; }
        }

        public TraditionalGameFilters()
        {
            Language = GameLanguage.None;
            Platform = Platform.None;
            GameKind = GameKind.None;
            LanguageCustom = string.Empty;
        }

        public bool HasFilter()
        {
            return Language != GameLanguage.None || Platform != Platform.None || GameKind != GameKind.None || !string.IsNullOrWhiteSpace(LanguageCustom);
        }
    }
}