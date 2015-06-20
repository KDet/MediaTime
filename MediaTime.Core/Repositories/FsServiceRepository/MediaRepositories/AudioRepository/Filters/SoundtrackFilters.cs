using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository.Filters
{
    public class SoundtrackFilters : IFilters
    {
        private readonly FiltersDictionary _filters = new FiltersDictionary();
        public FiltersDictionary Filters
        {
            get { return _filters; }
        }
        public  Production Production
        {
            get { return ( Production)Filters["Production"]; }
            set { Filters["Production"] = value; }
        }
        public SoundtrackFilters()
        {
            Production =  Production.None;
        }
        public bool HasFilter()
        {
            return Production !=  Production.None;
        }
    }
}