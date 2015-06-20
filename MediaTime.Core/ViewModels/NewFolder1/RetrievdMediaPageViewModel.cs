//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using VideoPocket.Core.Model;
//using VideoPocket.Core.Repositories;

//namespace VideoPocket.Core.ViewModels
//{
//    public class RetrievedMediaPageViewModel : MainPageViewModel
//    {
//        public class ObservableKeyValuePair<TK,TV> : BindableBase
//        {
//            private TV _value;
//            private TK _key;

//            public TV Value
//            {
//                get { return _value; }
//                set { SetProperty(ref _value , value); }
//            }

//            public TK Key
//            {
//                get { return _key; }
//                set { SetProperty(ref _key , value); }
//            }
//        }

//        private ObservableCollection<ObservableKeyValuePair<string, string>> _infoTable = new ObservableCollection<ObservableKeyValuePair<string, string>>();

//        public ObservableCollection<ObservableKeyValuePair<string, string>> InfoTable
//        {
//            get { return _infoTable; }
//            set { SetProperty(ref _infoTable, value); }
//        }

//        private string _image;
//        public string Image
//        {
//            get { return _image; }
//            set { SetProperty(ref _image , value); }
//        }

//        private string _title;
//        public string Title
//        {
//            get { return _title; }
//            set { SetProperty(ref _title, value); }
//        }

//        private string _description;
//        public string Description
//        {
//            get { return _description; }
//            set { SetProperty(ref _description, value); }
//        }

//        private ObservableCollection<string> _screenshots;
//        public ObservableCollection<string> Screenshots
//        {
//            get { return _screenshots; }
//            set { SetProperty(ref _screenshots , value); }
//        }

//        private FileListControlViewModel _fileSystem;
//        public FileListControlViewModel FileSystem
//        {
//            get { return _fileSystem; }
//            private set { SetProperty(ref _fileSystem ,value); }
//        }

//        public override async void LoadState(object navigationParameter, Dictionary<string, object> pageState)
//        {
//            var retrievedMedia = await FsRepository.RetrieveMediaUrlAsync(navigationParameter as string);

//            CategoryName = retrievedMedia.Title.Split('/').LastOrDefault();
//            Title = retrievedMedia.Title;
//            Image = retrievedMedia.Image;
//            RelatedMedia = new ObservableCollection<Media>(retrievedMedia.Similar.Take(5));
           
//            foreach (var keyValuePair in retrievedMedia.InfoTable)
//                InfoTable.Add(new ObservableKeyValuePair<string, string>
//                    {
//                        Key = keyValuePair.Key,
//                        Value = keyValuePair.Value
//                    });
//            Description = retrievedMedia.Description;
//            Screenshots = new ObservableCollection<string>(retrievedMedia.Screenshots);
//            FileSystem = new FileListControlViewModel(retrievedMedia.FileList);
//        }
//    }
//}
