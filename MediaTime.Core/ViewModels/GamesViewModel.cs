using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using MediaTime.Core.Repositories;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository;

namespace MediaTime.Core.ViewModels
{
    public class GamesViewModel : CategoryViewModel
    {
        public GamesViewModel(IGamesRepository gamesRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder) : base(gamesRepository,favoriteRepository, textProviderBuilder) { }
    }
}