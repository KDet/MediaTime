using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using MediaTime.Core.Repositories;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository;

namespace MediaTime.Core.ViewModels
{
    public class TextsViewModel : CategoryViewModel
    {
        public TextsViewModel(ITextsRepository textsRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder) : base(textsRepository,favoriteRepository, textProviderBuilder) { }
    }
}