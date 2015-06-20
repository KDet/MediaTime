//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using MediaTime.Core.Model;
//using MediaTime.Core.Properties;
//using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository;
//using MediaTime.Core.Services;

//namespace MediaTime.Core.ViewModels
//{
//    public class FirstViewModel
//        : BaseViewModel
//    {
//        private readonly VideoRepository _videoRepository;
//        private readonly HtmlPageLoaderService _htmlPageLoaderService;
//        /// <summary>
//        /// Backing field for my _hello.
//        /// </summary>
//        private string _hello;
//        /// <summary>
//        ///  Backing field for my command.
//        /// </summary>
//        private Cirrious.MvvmCross.ViewModels.MvxCommand _myCommand;
//        private ObservableCollection<Media> _mostViewedMedia;
//        private ICommand _refreshPageCommand;
//        private ICommand _itemSelectedCommand;
//        private ICommand _searchCommand;

//        /// <summary>
//        /// Gets My Command.
//        /// <para>
//        /// An example of a command and how to navigate to another view model
//        /// Note the ViewModel inside of ShowViewModel needs to change!
//        /// </para>
//        /// </summary>
//        public ICommand MyCommand
//        {
//            get
//            {
//                return _myCommand ??
//                       (_myCommand = new Cirrious.MvvmCross.ViewModels.MvxCommand(() => ShowViewModel<FirstViewModel>()));
//            }
//        }
       
//        public ICommand ItemSelectedCommand
//        {
//            get
//            {
//                return _itemSelectedCommand ??
//                       (_itemSelectedCommand =
//                           new Cirrious.MvvmCross.ViewModels.MvxCommand(() => ShowViewModel<FirstViewModel>()));
//            }

//        }
//        public ICommand SearchCommand
//        {
//            get
//            {
//                return _searchCommand ??
//                       (_searchCommand =
//                           new Cirrious.MvvmCross.ViewModels.MvxCommand(() => ShowViewModel<FirstViewModel>()));
//            }

//        }
//        public string Hello
//        {
//            get { return _hello; }
//            set
//            {
//                _hello = value;
//                RaisePropertyChanged(() => Hello);
//            }
//        }
//        public ObservableCollection<Media> MostViewedMedia
//        {
//            get { return _mostViewedMedia; }
//            set
//            {
//                _mostViewedMedia = value;
//                RaisePropertyChanged(() => MostViewedMedia);
//            }
//        }
        

//        public FirstViewModel() : base()
//        {
//            _htmlPageLoaderService = new HtmlPageLoaderService();
//            _videoRepository = new VideoRepository(_htmlPageLoaderService);

//            #region 

////ObservableListedMedia[] listedMedia =
//            //{
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"ms-appx:///Assets/testTitle.png", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäí´ ´ãðè / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"/Assets/DarkGray.png", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"D:\Work\Source control\VideoPocket\VideoPocket\VideoPocket.WindowsStore\Assets\testTitle.png", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"D:\Work\Source control\VideoPocket\VideoPocket\VideoPocket.WindowsStore\Assets\testTitle.png", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"http://s2.dotua.org/fsua_items/cover/00/29/42/9/00294291.jpg", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"http://s2.dotua.org/fsua_items/cover/00/29/42/9/00294291.jpg", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"http://s2.dotua.org/fsua_items/cover/00/29/42/9/00294291.jpg", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"http://s2.dotua.org/fsua_items/cover/00/29/42/9/00294291.jpg", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"http://s2.dotua.org/fsua_items/cover/00/29/42/9/00294291.jpg", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"},
//            //    new ObservableListedMedia(){Date = DateTime.Now.ToString(), Url = @"http://brb.to/video/films/i25jLwxqAcajTcmaCGHxfAA-pyataya-vlast.html", Image = @"http://s2.dotua.org/fsua_items/cover/00/29/42/9/00294291.jpg", Qualities = new List<string>{"HD","HQ"}, Status = "2 ñåð´ß 3 ñåçîí", SubTitle = "Subtitle", Title = "ƒîëîäíûå èãðû: ˆ âñïûõíåò ïëàìß / The Hunger Games: Catching Fire"}
//            //};

//            #endregion

//           // var media = _videoRepository.GetRelatedMediaAsync();
//           // RelatedMedia = new ObservableCollection<Media>(media);
//           //ListedMediaCollection = new ObservableCollection<Media>(listedMedia);
//        }
//        private ObservableCollection<MediaListed> _listedMediaCollection;
//        public ObservableCollection<MediaListed> ListedMediaCollection
//        {
//            get { return _listedMediaCollection; }
//            set
//            {
//                _listedMediaCollection = value;
//                RaisePropertyChanged(() => ListedMediaCollection);
//            }
//        }
//        private ObservableCollection<MediaDetailed> _detailedMediaCollection;
//        public ObservableCollection<MediaDetailed> DetailedMediaCollection
//        {
//            get { return _detailedMediaCollection; }
//            set
//            {
//                _detailedMediaCollection = value;
//                RaisePropertyChanged(() => DetailedMediaCollection);
//            }
//        }

//        //public override void Start()
//        //{
//        //    base.Start();
//        //    //TODO ця логіка має бути в htmlPageLoaderService, але поки немає синхронного чи asyc-await перезавантаженої функції для роботи з http реалізована логіка через калбеки
//        //    var request = new MvxRestRequest(_videoRepository.Url);
//        //    var client = new MvxRestClient();
//        //    //var client = Mvx.Resolve<IMvxRestClient>();
//        //    client.MakeRequest(request,
//        //        async response =>
//        //        {
//        //            using (var reader = new StreamReader(response.Stream))
//        //                _htmlPageLoaderService.HtmlContent = reader.ReadToEnd();
//        //            await LoadContent();
//        //        },
//        //        error =>
//        //        {
//        //            // do something with the error
//        //        });
//        //}
//        private async Task LoadContent()
//        {
//            var media = await _videoRepository.GetRelatedMediaAsync();
//            MostViewedMedia = new ObservableCollection<Media>(media);
//            var retrive = await _videoRepository.RetrieveMediaAsync(media.First());
//            var detailedMedia = await _videoRepository.GetDetailedMediaAsync(2);
//            DetailedMediaCollection = new ObservableCollection<MediaDetailed>(detailedMedia);
//            var listedMedia = await _videoRepository.GetListedMediaAsync();
//            ListedMediaCollection = new ObservableCollection<MediaListed>(listedMedia);
//        }
//    }

//    public class ObservableMedia : Media, INotifyPropertyChanged
//    {
//        private bool _isSelected;
//        public event PropertyChangedEventHandler PropertyChanged;

//        [NotifyPropertyChangedInvocator]
//        protected void OnPropertyChanged(string propertyName)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
//        }

//        public bool IsSelected
//        {
//            get { return _isSelected; }
//            set
//            {
//                _isSelected = value;
//                OnPropertyChanged("IsSelected");
//            }
//        }
//    }

//    public class ObservableListedMedia : MediaListed
//    {
//        private bool _isSelected;
//        public event PropertyChangedEventHandler PropertyChanged;

//        [NotifyPropertyChangedInvocator]
//        protected void OnPropertyChanged(string propertyName)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
//        }

//        public bool IsSelected
//        {
//            get { return _isSelected; }
//            set
//            {
//                _isSelected = value;
//                OnPropertyChanged("IsSelected");
//            }
//        }
//    }
//}