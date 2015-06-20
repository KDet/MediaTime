using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using Cirrious.MvvmCross.Plugins.WebBrowser;
using Cirrious.MvvmCross.ViewModels;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories;

namespace MediaTime.Core.ViewModels
{
    public class HomeViewModel : MenuViewModel
    {
        public class HomeSavedState : ISavedState
        {
            public string FavoriteMediaCache { get; set; }

            public void ClearCash()
            {
                FavoriteMediaCache = null;
            }
        }

        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMvxWebBrowserTask _webBrowser;
        private ObservableCollection<Media> _favoriteMedia;
        private MvxCommand<Media> _itemSelectedCommand;
        private MvxCommand<Media> _removeFromFavoriteCommand;

        private readonly IMvxJsonConverter _jsonConverter;

        #region CIRS Lifecycle
        public HomeViewModel(IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder, IMvxWebBrowserTask webBrowser)
            : base(textProviderBuilder)
        {
            _favoriteRepository = favoriteRepository;
            _webBrowser = webBrowser;
            _jsonConverter = GetInstance<IMvxJsonConverter>();
           // LifecycleState = Lifecycle.Run;
        }
        //public void Init()
        //{
        //    base.Init();
        //}
        public void ReloadState(HomeSavedState categorySavedState)
        {
            if (categorySavedState == null)
            {
                LifecycleState = Lifecycle.ReloadFail;
                return;
            }
           // bool isStateReloaded;
            FavoriteMedia = _jsonConverter.DeserializeObject<ObservableCollection<Media>>(categorySavedState.FavoriteMediaCache);
           // isStateReloaded = FavoriteMedia != null;
            LifecycleState =/* !isStateReloaded ? Lifecycle.ReloadFail :*/ Lifecycle.Reload;
        }
        public async override void Start()
        {
            var mediaTask = FavoriteMedia == null || LifecycleState != Lifecycle.Reload
                ? GetFavoriteMediaAsync().ContinueWith(task =>
                {
                    FavoriteMedia = new ObservableCollection<Media>(task.Result);
                })
                : null;
            LifecycleState = Lifecycle.Start;
            try
            {
                await Task.WhenAll(mediaTask);
            }
            catch //логіка перевірки
            {
                LifecycleState = Lifecycle.StartFail;
                return;
            }
            LifecycleState = Lifecycle.Run;
        }
        public override ISavedState SaveState()
        {
            base.SaveState();
            var favoriteMediaCash = _jsonConverter.SerializeObject(FavoriteMedia);
            LifecycleState = string.IsNullOrWhiteSpace(favoriteMediaCash) ? Lifecycle.SaveFail : Lifecycle.Save;
            return new HomeSavedState { FavoriteMediaCache = favoriteMediaCash };
        }
        #endregion

        private Task<Media[]> GetFavoriteMediaAsync()
        {
            return Task.Run(() => _favoriteRepository.GetAllItems().ToArray());
        }
        private Task RemoveFromFavorite(Media media)
        {
            FavoriteMedia.Remove(media);
            return Task.Run(() => _favoriteRepository.Delete(media));
        }

        public ObservableCollection<Media> FavoriteMedia
        {
            get { return _favoriteMedia; }
            set
            {
                _favoriteMedia = value;
                RaisePropertyChanged(() => FavoriteMedia);
            }
        }
        
        public MvxCommand<Media> RemoveFromeFavoriteCommand
        {
            get
            {
                return _removeFromFavoriteCommand ??
                       (_removeFromFavoriteCommand =
                           new MvxCommand<Media>(async media => await RemoveFromFavorite(media)));
            }
        }
        public MvxCommand<Media> ItemSelectedCommand
        {
            get
            {
                return _itemSelectedCommand ??
                       (_itemSelectedCommand =
                           new MvxCommand<Media>(
                               media =>
                                   ShowViewModel<MediaItemViewModel>(
                                       new MediaItemViewModel.MediaNavigatedObject(media.Url, media.Title,
                                           media.SubTitle, media.Image))));
            }
        }
        /// <summary>
        /// Команда для переходу на інтернет ресурс за веб посиланням
        /// Парметр команди - веб адрес
        /// </summary>
        public MvxCommand GoToWebsiteCommand
        {
            get
            {
                return new MvxCommand(() => _webBrowser.ShowWebPage("http://brb.to/"));
            }
        }
    }
}