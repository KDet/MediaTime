namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters
{
    public interface IFilters
    {
        bool HasFilter();
        FiltersDictionary Filters { get; }
    }
}