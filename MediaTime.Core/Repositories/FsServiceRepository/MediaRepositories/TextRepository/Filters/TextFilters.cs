using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository.Filters
{
    public abstract class TextFilters : IFilters
    {
        private readonly FiltersDictionary _filters = new FiltersDictionary();

        public FiltersDictionary Filters
        {
            get { return _filters; }
        }

        public Device Device
        {
            get { return (Device)Filters["Device"]; }
            set { Filters["Device"] = value; }
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

        protected TextFilters()
        {
            Language = Language.None;
            Device = Device.None;
            LanguageCustom = string.Empty;
        }

        public bool HasFilter()
        {
            return Language != Language.None || Device != Device.None || !string.IsNullOrWhiteSpace(LanguageCustom);
        }
    }
}