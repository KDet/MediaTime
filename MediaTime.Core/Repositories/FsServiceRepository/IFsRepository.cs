using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository;

namespace MediaTime.Core.Repositories.FsServiceRepository
{
    public interface IFsRepository : IPlaceholder
    {
        /// <summary>
        /// Video repository
        /// </summary>
        VideoRepository Video { get; }

        /// <summary>
        /// Audio repository
        /// </summary>
        AudioRepository Audio { get; }

        /// <summary>
        /// Literature repository
        /// </summary>
        TextsRepository Texts { get; }

        /// <summary>
        /// Game repository
        /// </summary>
        GamesRepository Games { get; }

        /// <summary>
        /// Get fs.to updates list
        /// </summary>
        /// <param name="page">Current page number</param>
        /// <returns>List of new files fs.to service</returns>
        Task<UpdatedMedia[]> GetUpdatesAsync(int page = 0);
    }
}