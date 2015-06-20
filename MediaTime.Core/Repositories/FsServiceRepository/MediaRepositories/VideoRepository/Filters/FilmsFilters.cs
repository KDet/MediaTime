using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters
{
    public class FilmsFilters : IFilters
    {
        private readonly FiltersDictionary _filters = new FiltersDictionary();

        public FiltersDictionary Filters
        {
            get { return _filters; }
        }

        public Quality Quality
        {
            get { return (Quality)Filters["Quality"]; }
            set { Filters["Quality"] = value; }
        }

        public Language Language
        {
            get { return (Language)Filters["Language"]; }
            set { Filters["Language"] = value; }
        }

        public string LanguageCustom
        {
            get { return (string)Filters["LanguageCustom"]; }
            set { Filters["LanguageCustom"] = value; }
        }

        public Production Production
        {
            get { return (Production)Filters["Production"]; }
            set { Filters["Production"] = value; }
        }

        public Mood Mood
        {
            get { return (Mood)Filters["Mood"]; }
            set { Filters["Mood"] = value; }
        }

        public Translation Translation
        {
            get { return (Translation)Filters["Translation"]; }
            set { Filters["Translation"] = value; }
        }

        public string TranslateCustom
        {
            get { return (string)Filters["TranslateCustom"]; }
            set { Filters["TranslateCustom"] = value; }
        }

        public FilmsFilters()
        {
            Mood = Mood.None;
            Translation = Translation.None;
            TranslateCustom = string.Empty;
            Production = Production.None;
            Quality = Quality.None;
            Language = Language.None;
            LanguageCustom = string.Empty;
        }

        public bool HasFilter()
        {
            return Mood != Mood.None || Translation != Translation.None || Quality != Quality.None || Language != Language.None || Production != Production.None || !string.IsNullOrWhiteSpace(LanguageCustom) || !string.IsNullOrWhiteSpace(TranslateCustom);
        }
    }
}