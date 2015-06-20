using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using MediaTime.Core.Repositories;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository;

namespace MediaTime.Core.ViewModels
{
    public class VideoViewModel : CategoryViewModel
    {
        public VideoViewModel(IVideoRepository videoRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(videoRepository,favoriteRepository, textProviderBuilder)
        {
        }
    }
}