using System;
using System.Collections.ObjectModel;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using MediaTime.Core.Common;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository;

namespace MediaTime.Core.ViewModels
{
    public class MediaItemViewModel : MenuViewModel
    {
        public class MediaSavedState : Media, ISavedState
        {
            public string Description { get; set; }
            public int Likes { get; set; }
            public int Dislikes { get; set; }

            public string InfoTableCashe { get; set; }
            public string ScreenshotsCashe { get; set; }
            public string FileListCase { get; set; }
            public string ReviewsCashe { get; set; }
            public string SimilarCashe { get; set; }
            public void ClearCash()
            {
                throw new System.NotImplementedException();
            }
        }
        
        public class MediaNavigatedObject : Media, INavigatedObject
        {
            public bool IsObjectValid()
            {
                return Image != null || SubTitle != null || Url != null || Title != null;
            }

            public MediaNavigatedObject() { }
            public MediaNavigatedObject(string url, string title, string subTitle, string image) : base(url, title, subTitle, image) { }
        }

        private string _image;
        private string _title;
        private string _subTitle;
        private string _description;
        private readonly IFsRepository _repository;
        private int _likes;
        private int _dislikes;
        private ObservableKeyValueList<string, string> _infoTable;
        private ObservableCollection<Review> _reviews;
        private ObservableCollection<string> _screenshots;
        private ObservableCollection<Media> _similar;
        private ObservableCollection<Storage> _fileList;

        private MediaNavigatedObject _navigationMedia;
        private readonly IMvxJsonConverter _jsonConverter;

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                RaisePropertyChanged(() => Image);
            }
        }
        public string Tittle
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Tittle);
            }
        }
        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                RaisePropertyChanged(() => SubTitle);
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }
        public int Likes
        {
            get { return _likes; }
            set
            {
                _likes = value;
                RaisePropertyChanged(() => Likes);
            }
        }
        public int Dislikes
        {
            get { return _dislikes; }
            set
            {
                _dislikes = value;
                RaisePropertyChanged(() => Dislikes);
            }
        }
        public ObservableKeyValueList<string, string> InfoTable
        {
            get { return _infoTable; }
            set
            {
                _infoTable = value;
                RaisePropertyChanged(() => InfoTable);
            }
        }
        public ObservableCollection<Review> Reviews
        {
            get { return _reviews; }
            set
            {
                _reviews = value;
                RaisePropertyChanged(() => Reviews);
            }
        }
        public ObservableCollection<string> Screenshots
        {
            get { return _screenshots; }
            set
            {
                _screenshots = value;
                RaisePropertyChanged(() => Screenshots);
            }
        }
        public ObservableCollection<Media> Similar
        {
            get { return _similar; }
            set
            {
                _similar = value;
                RaisePropertyChanged(() => Similar);
            }
        }
        public ObservableCollection<Storage> FileList
        {
            get { return _fileList; }
            set
            {
                _fileList = value;
                RaisePropertyChanged(() => FileList);
            }
        }

        #region CIRS Lifecycle
        public MediaItemViewModel(IFsRepository repository, IMvxTextProviderBuilder textProviderBuilder) : base(textProviderBuilder)
        {
            _repository = repository;
            _jsonConverter = GetInstance<IMvxJsonConverter>();
        }
        public void Init(MediaNavigatedObject navigationObject)
        {
           _navigationMedia = navigationObject;
           LifecycleState = Lifecycle.Init;
        }
        public override async void Start()
        {
            if (_navigationMedia == null || !_navigationMedia.IsObjectValid())
            {
                LifecycleState = Lifecycle.StartFail;
                Close(this);
                return;
            }
            if (LifecycleState != Lifecycle.Reload)
            {
                LifecycleState = Lifecycle.Start;
                RetrievedMedia retrievedMedia = null;
                try
                {
                   retrievedMedia = await _repository.RetrieveMediaAsync(_navigationMedia);
                }
                catch (Exception)
                {
                    LifecycleState = Lifecycle.SaveFail;
                    throw;
                }
                Image = retrievedMedia.Image;
                SubTitle = retrievedMedia.SubTitle;
                Tittle = retrievedMedia.Title;
                Description = retrievedMedia.Description;
                Likes = retrievedMedia.Likes;
                Dislikes = retrievedMedia.Dislikes;
                if (retrievedMedia.InfoTable != null)
                    InfoTable = new ObservableKeyValueList<string, string>(retrievedMedia.InfoTable);
                Reviews = new ObservableCollection<Review>(retrievedMedia.Reviews);
                Screenshots = new ObservableCollection<string>(retrievedMedia.Screenshots);
                Similar = new ObservableCollection<Media>(retrievedMedia.Similar);
                FileList = new ObservableCollection<Storage>(retrievedMedia.FileList);
            }
            LifecycleState = Lifecycle.Run;
        }
        public void ReloadState(MediaSavedState categorySavedState)
        {
           // base.ReloadState(categorySavedState);
            if (categorySavedState == null)
            {
                LifecycleState = Lifecycle.ReloadFail;
                return;
            }
            // bool isSateReloaded = true;
            //if (string.IsNullOrEmpty(Description))
            Description = categorySavedState.Description;
            //if (Dislikes == 0)
            Dislikes = categorySavedState.Dislikes;
            //if (string.IsNullOrEmpty(SubTitle))
            SubTitle = categorySavedState.SubTitle;
            //if (string.IsNullOrEmpty(Tittle))
            Tittle = categorySavedState.Title;
            //if (string.IsNullOrEmpty(Image))
            Image = categorySavedState.Image;
            // if (Likes == 0)
            Likes = categorySavedState.Likes;
            //if(FileList == null)
            // FileList = JsonConverter.DeserializeObject<ObservableCollection<Storage>>(categorySavedState.FileListCase);
            try
            {
                //if (InfoTable == null)
                InfoTable =
                _jsonConverter.DeserializeObject<ObservableKeyValueList<string, string>>(
                    categorySavedState.InfoTableCashe);
            }
            catch (Exception)
            {
                LifecycleState = Lifecycle.ReloadFail;
                //throw;
            }
            try
            {
                //if (Reviews == null)
                Reviews = _jsonConverter.DeserializeObject<ObservableCollection<Review>>(categorySavedState.ReviewsCashe);
            }
            catch (Exception)
            {
                LifecycleState = Lifecycle.ReloadFail;
                //throw;
            }
            try
            {
                // if (Screenshots == null)
                Screenshots =
                    _jsonConverter.DeserializeObject<ObservableCollection<string>>(categorySavedState.ScreenshotsCashe);
            }
            catch (Exception)
            {
                LifecycleState = Lifecycle.ReloadFail;
                //throw;
            }
            try
            {
                //if (Similar == null)
                Similar = _jsonConverter.DeserializeObject<ObservableCollection<Media>>(categorySavedState.SimilarCashe);
            }
            catch (Exception)
            {
                LifecycleState = Lifecycle.ReloadFail;
                //throw;
            }
            LifecycleState = Lifecycle.Reload;
        }
        public  ISavedState SaveState()
        {
            base.SaveState(null);
            var obj = new MediaSavedState();
            obj.Description = Description;
            obj.Dislikes = Dislikes;
            obj.FileListCase = _jsonConverter.SerializeObject(FileList);
            obj.Image = Image;
            obj.InfoTableCashe = _jsonConverter.SerializeObject(InfoTable);
            obj.Likes = Likes;
            obj.ReviewsCashe = _jsonConverter.SerializeObject(Reviews);
            obj.ScreenshotsCashe = _jsonConverter.SerializeObject(Screenshots);
            obj.SimilarCashe = _jsonConverter.SerializeObject(Similar);
            obj.SubTitle = SubTitle;
            obj.Title = Tittle;
            return obj;
        }
        #endregion
    }
}