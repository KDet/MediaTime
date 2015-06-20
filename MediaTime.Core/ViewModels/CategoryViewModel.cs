using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using Cirrious.MvvmCross.ViewModels;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories;

namespace MediaTime.Core.ViewModels
{
    public abstract class CategoryViewModel : MenuViewModel
    {
        public class CategorySavedState : ISavedState
        {
            public string ListedMediaCache { get; set; }
            public string DetailedMediaCache { get; set; }
            public string RelatedMediaCache { get; set; }

            public void ClearCash()
            {
                ListedMediaCache = null;
                DetailedMediaCache = null;
                RelatedMediaCache = null;
            }
        }

        private ObservableCollection<Media> _relatedMedia;
        private ObservableCollection<MediaListed> _listedMediaCollection;
        private ObservableCollection<MediaDetailed> _detailedMediaCollection;
        private View _viewMode = View.List;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMvxJsonConverter _jsonConverter;
        private MvxCommand<Media> _removeFromFavoriteCommand;
        private readonly IFavoriteRepository _favoriteRepository;
        private MvxCommand<Media> _addToFavoriteCommand;
        private Media _selectedItem;

        private Task RemoveFromFavorite(Media media)
        {
           return Task.Run(() => _favoriteRepository.Delete(media));
        }
        private Task AddToFavorite(Media media)
        {
            return Task.Run(() => _favoriteRepository.Insert(media));
        }

        public MvxCommand RefreshPageCommand
        {
            get { return new MvxCommand(Start); }
        }
        public MvxCommand<Media> ItemSelectedCommand
        {
            get { return new MvxCommand<Media>(media => ShowViewModel<MediaItemViewModel>(new MediaItemViewModel.MediaNavigatedObject(media.Url,media.Title,media.SubTitle, media.Image))); }
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
        public MvxCommand<Media> AddToFavoriteCommand
        {
            get
            {
                return _addToFavoriteCommand ??
                       (_addToFavoriteCommand =
                           new MvxCommand<Media>(async media => await AddToFavorite(media)));
            }
        }

       
        public ObservableCollection<Media> RelatedMedia
        {
            get { return _relatedMedia; }
            set
            {
                _relatedMedia = value;
                RaisePropertyChanged(() => RelatedMedia);
            }
        }
        public ObservableCollection<MediaListed> ListedMediaCollection
        {
            get { return _listedMediaCollection; }
            set
            {
                _listedMediaCollection = value;
                RaisePropertyChanged(() => ListedMediaCollection);
            }
        }
        public ObservableCollection<MediaDetailed> DetailedMediaCollection
        {
            get { return _detailedMediaCollection; }
            set
            {
                _detailedMediaCollection = value;
                RaisePropertyChanged(() => DetailedMediaCollection);
            }
        }
        public View ViewMode
        {
            get { return _viewMode; }
            set
            {
                if (_viewMode == value) return;
                _viewMode = value;
                RaisePropertyChanged(() => ViewMode);
                Start();
            }
        }

        #region CIRS Lifecycle
        protected CategoryViewModel(ICategoryRepository categoryRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder) : base(textProviderBuilder)
        {
            _categoryRepository = categoryRepository;
            _favoriteRepository = favoriteRepository;
            _jsonConverter = GetInstance<IMvxJsonConverter>();
           // LifecycleState = Lifecycle.Run;
        }

        //public override void Init( /*NavigationObject navigationObject*/)
        //{
        //    base.Init(); 
        //}
        public void ReloadState(CategorySavedState categorySavedState)
        {
            //base.ReloadState(categorySavedState);
            if (categorySavedState == null)
            {
                LifecycleState = Lifecycle.ReloadFail;
                return;
            }
          //  bool isStateReloaded;
            switch (_viewMode)
            {
                case View.List:
                    ListedMediaCollection =
                        _jsonConverter.DeserializeObject<ObservableCollection<MediaListed>>(
                            categorySavedState.ListedMediaCache);
                   // isStateReloaded = ListedMediaCollection != null;
                    break;
                case View.Detailed:
                    DetailedMediaCollection =
                        _jsonConverter.DeserializeObject<ObservableCollection<MediaDetailed>>(
                            categorySavedState.DetailedMediaCache);
                  //  isStateReloaded = DetailedMediaCollection != null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            LifecycleState = /*!isStateReloaded ? Lifecycle.ReloadFail :*/ Lifecycle.Reload;
        }
        public async override void Start()
        {
           var mediaTask = RelatedMedia == null || LifecycleState != Lifecycle.Reload
                ? GetRelatedMediaAsync().ContinueWith(task =>
                {
                    RelatedMedia = new ObservableCollection<Media>(task.Result);
                })
                : null;
            var listedMediaTask = ViewMode == View.List && (ListedMediaCollection == null || LifecycleState != Lifecycle.Reload)
                ? GetListedMediaAsync().ContinueWith(task =>
                {
                    ListedMediaCollection = new ObservableCollection<MediaListed>(task.Result);
                })
                : null;
            var detailedMediaTask = ViewMode == View.Detailed && (DetailedMediaCollection == null || LifecycleState != Lifecycle.Reload)
                ? GetDetailedMediaAsync().ContinueWith(task =>
                {
                    DetailedMediaCollection = new ObservableCollection<MediaDetailed>(task.Result);
                })
                : null;
            LifecycleState = Lifecycle.Start;
            var tasks = new[] { mediaTask, listedMediaTask, detailedMediaTask };
            try
            {
                await Task.WhenAll(tasks.Where(task => task != null).ToArray());
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
            try
            {
                switch (_viewMode)
                {
                    case View.List:
                        var listedMediaCash = _jsonConverter.SerializeObject(ListedMediaCollection);
                          LifecycleState = /*string.IsNullOrWhiteSpace(listedMediaCash)? Lifecycle.SaveFail :*/ Lifecycle.Save;
                        return new CategorySavedState { ListedMediaCache = listedMediaCash };
                    case View.Detailed:
                        var detailedMediaCash = _jsonConverter.SerializeObject(DetailedMediaCollection);
                        LifecycleState = /*string.IsNullOrWhiteSpace(detailedMediaCash) ? Lifecycle.SaveFail :*/ Lifecycle.Save;
                        return new CategorySavedState { DetailedMediaCache = detailedMediaCash };
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                LifecycleState = Lifecycle.SaveFail;
                throw;
            }
        }
        #endregion

        protected virtual Task<Media[]> GetRelatedMediaAsync()
        {
            return _categoryRepository.GetRelatedMediaAsync(true);
        }
        protected virtual Task<MediaListed[]> GetListedMediaAsync()
        {
            return _categoryRepository.GetListedMediaAsync();
        }
        protected virtual Task<MediaDetailed[]> GetDetailedMediaAsync()
        {
            return _categoryRepository.GetDetailedMediaAsync();
        }
    }
}