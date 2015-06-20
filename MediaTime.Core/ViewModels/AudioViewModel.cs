using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using MediaTime.Core.Repositories;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository;

namespace MediaTime.Core.ViewModels
{
    public class AudioViewModel : CategoryViewModel
    {
        public AudioViewModel(IAudioRepository audioRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder) : base(audioRepository,favoriteRepository, textProviderBuilder) { }
    }
}