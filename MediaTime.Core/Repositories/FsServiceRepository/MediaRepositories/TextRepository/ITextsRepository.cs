namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository
{
    public interface ITextsRepository : ICategoryRepository
    {
        FictionLiteratureRepository FictionLiterature { get; }
        AppliedLiteratureRepository AppliedLiterature { get; }
        JournalsRepository Journals { get; }
        ComixRepository Comix { get; }
    }
}