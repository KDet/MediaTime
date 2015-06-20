using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters
{
    public class ClipsFilters : IFilters
    {
        private readonly FiltersDictionary _filters = new FiltersDictionary();

        public FiltersDictionary Filters
        {
            get { return _filters; }
        }

        public Production Production
        {
            get { return (Production)Filters["Production"]; }
            set { Filters["Production"] = value; }
        }

        public Quality Quality
        {
            get { return (Quality)Filters["Quality"]; }
            set { Filters["Quality"] = value; }
        }

        public MusicKind Kind
        {
            get { return (MusicKind)Filters["Kind"]; }
            set { Filters["Kind"] = value; }
        }

        public ClipsFilters()
        {
            Quality = Quality.None;
            Kind = MusicKind.None;
            Production = Production.None;
        }

        public bool HasFilter()
        {
            return Quality != Quality.None || Kind != MusicKind.None || Production != Production.None;
        }
    }
}