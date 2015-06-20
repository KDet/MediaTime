//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using VideoPocket.Core.Model;

//namespace VideoPocket.Core.ViewModels
//{
//    public class MainPageViewModel : BaseViewModel
//    {
//        private ObservableCollection<Media> _mostViewedMedia;
//        public ObservableCollection<Media> RelatedMedia
//        {
//            get { return _mostViewedMedia; }
//            set { SetProperty(ref _mostViewedMedia, value); }
//        }

//        //TODO зробити прив'язку для знайденого медіа
//        private ObservableCollection<SearchMedia> _foundMedia;
//        public ObservableCollection<SearchMedia> FoundMedia
//        {
//            get { return _foundMedia; }
//            set { SetProperty(ref _foundMedia, value); }
//        }

//        public RelayCommand<object> RefreshPageCommand { get; private set; }
//        public RelayCommand<Category> ItemSelectedCommand { get; private set; }
//        public RelayCommand<string> SearchCommand { get; set; }
//        public RelayCommand<Media> RetrieveMediaCommand { get; set; }

//        private string _categoryName;
//        public string CategoryName
//        {
//            get { return _categoryName; }
//            set { SetProperty(ref _categoryName, value); }
//        }

//        public MainPageViewModel()
//        {
//            ItemSelectedCommand = new RelayCommand<Category>(OnCategoryClicked);
//            RefreshPageCommand = new RelayCommand<object>(async o => await OnRefreshPage(o));
//            SearchCommand = new RelayCommand<string>(async s => await OnSearch(s));
//            RetrieveMediaCommand = new RelayCommand<Media>(OnRetriveMedia);
//        }

//        private void OnRetriveMedia(Media media)
//        {
//           NavigationService.Navigate("RetrievedMediaPage", media.Url);
//        }

//        private void OnCategoryClicked(Category category)
//        {
//            var name = category.HasSubCategories ? "Category" : category.Name;
//            if (ViewModelLocator.SetViewModelByName(string.Format("{0}PageViewModel", name)))
//                NavigationService.Navigate(name == "Category" ? "CategoryPage" : "SubCategoryPage", category.Name);
//        }

//        protected virtual async Task OnRefreshPage(object param)
//        {
//            var mediaTop = await GetRelatedMediaAsync(true);
//            RelatedMedia = new ObservableCollection<Media>(mediaTop.Take(5));
//        }

//        protected virtual async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await FsRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected int FoundPageNum = 0;
//        protected virtual async Task OnSearch(string query)
//        {
//            //TODO зробити навігацію на сторінку пошуку
//            //FoundMedia = new ObservableCollection<SearchMedia>();
//            //do
//            //{
//            //    var searchResalts = await FsRepository.SearchAsync(query, FoundPageNum);
//            //    if(searchResalts == null || searchResalts.Length == 0) return;

//            //    foreach (var media in searchResalts)
//            //        FoundMedia.Add(media);
//            //} while (true);
             
//        }

//        public override async void LoadState(object navigationParameter, Dictionary<string, object> pageState)
//        {
//            //TODO синхронізувати потік тут!!!!!!!
//            // Разрешение сохраненному состоянию страницы переопределять первоначально отображаемый элемент
//            //if (pageState != null && pageState.ContainsKey("RelatedMedia"))
//               // navigationParameter = pageState["RelatedMedia"];
//            CategoryName = Translator.Current.TranslateQuick("Media");
//            var mediaTop = await FsRepository.GetRelatedMediaAsync(true);
//            RelatedMedia = new ObservableCollection<Media>(mediaTop.Take(5));
//        }

//        //public override void SaveState(Dictionary<string, object> pageState)
//        //{
//        //    //тут можливо прийдеться зберігати серіалізований список знайдених файлів
//        //}
//    }
//}
