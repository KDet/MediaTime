using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository.Filters
{
    public class AlbumFilters : IFilters
    {
        private readonly FiltersDictionary _filters = new FiltersDictionary();
        public FiltersDictionary Filters
        {
            get { return _filters; }
        }
        public MusicKind Kind
        {
            get { return (MusicKind)Filters["Kind"]; }
            set { Filters["Kind"] = value; }
        }
        public  Production Production
        {
            get { return ( Production)Filters["Production"]; }
            set { Filters["Production"] = value; }
        }
        public AlbumFilters()
        {
            Production =  Production.None;
            Kind =  MusicKind.None;
        }
        public bool HasFilter()
        {
            return Kind !=  MusicKind.None || Production !=  Production.None;
        }
    }
}