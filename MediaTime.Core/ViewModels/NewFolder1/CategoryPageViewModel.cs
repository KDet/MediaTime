//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using MediaOnTouch.ViewModel;
//using VideoPocket.Core.Model;

//namespace VideoPocket.Core.ViewModels
//{
//    public class CategoryPageViewModel : MainPageViewModel
//    {
//        public CategoryPageViewModel()
//        {
//            NextPageCommand = new RelayCommand(async () => await OnNextPage());
//        }

//        private Color _categoryColor;
//        public Color CategoryColor
//        {
//            get { return _categoryColor; }
//            set { SetProperty(ref _categoryColor,value); }
//        }

//        private bool _isDetailedView;
//        public bool IsDetailedView
//        {
//            get { return _isDetailedView; }
//            set { SetProperty(ref _isDetailedView, value); }
//        }

//        private ObservableCollection<MediaListed> _listedMediaCollection = new ObservableCollection<MediaListed>();
//        public ObservableCollection<MediaListed> ListedMediaCollection
//        {
//            get { return _listedMediaCollection; }
//            set
//            {
//                SetProperty(ref _listedMediaCollection, value);
//            }
//        }

//        private ObservableCollection<MediaDetailed> _detailedMediaCollection = new ObservableCollection<MediaDetailed>();
//        public ObservableCollection<MediaDetailed> DetailedMediaCollection
//        {
//            get { return _detailedMediaCollection; }
//            set { SetProperty(ref _detailedMediaCollection, value); }
//        }

//        private BaseRepository _repository;
//        protected int CurentListedPage;
//        protected int CurentDetailedPage;
//        protected bool NoMoreDetailedPages;
//        protected bool NoMoreListedPages;
//        //protected string CurrentCategoryName;

//        public override async void LoadState(object navigationParameter, Dictionary<string, object> pageState)
//        {
//            //якщо дівайс був shutdowm чи suspended - відновити роботу
//            //if (pageState != null && pageState.ContainsKey(""))
//            //    navigationParameter = pageState[""];

//            var category = Categories.FindByName(navigationParameter as string);
//            if(category == null) return;
           
//            CategoryColor = category.Color;
//            CategoryName = category.Title;
            
//            //обнклення всіх параметрів, на випадок, якщо функція буде викликана з іншої функції
//            CurentDetailedPage = CurentListedPage = 0;
//            NoMoreDetailedPages = NoMoreListedPages = false;
//            //завантажити основний контент - лідери категорії
//            //Знаходження потрібної репозиторії для одної з 4 можливих категорій медіа
//            Media[] mediaTop;
//            switch (category.Name)
//            {
//                case "Video":
//                    _repository = FsRepository.Video;
//                    mediaTop = await _repository.GetRelatedMediaAsync();
//                    break;

//                case "Audio":
//                    _repository = FsRepository.Audio;
//                    mediaTop = await _repository.GetRelatedMediaAsync();
//                    break;

//                case "Texts":
//                    _repository = FsRepository.Texts;
//                    mediaTop = await _repository.GetRelatedMediaAsync();
//                    break;

//                case "Games":
//                    _repository = FsRepository.Games;
//                    mediaTop = await _repository.GetRelatedMediaAsync();
//                    break;
//                default:
//                    //іншого ніяк не може бути
//                    return;
//            }

//            await OnNextPage();           //завантажити першу сторінку поточного відображення даних (деталізованого чи списком)
//            //завантажити популярне
//            RelatedMedia = new ObservableCollection<Media>(mediaTop.Take(5));
//        }

//        public override void SaveState(Dictionary<string, object> pageState)
//        {
//            //base.SaveState(pageState);
//        }

//        public RelayCommand NextPageCommand { get; private set; }
//        protected async Task OnNextPage()
//        {
//            //ВАЖЛИВО при виклику команди спочатку інкрементувати номер поточної сторінки, а потім вже її завантажувати, щоб у випадку коли ще не закінчилось завантаження попередньої сторінки в одному потоці, в іншому потоці йшло паралельно завантаження іншої сторінки.
//            // можна обійтися без синхронізації потоків бо CurentDetailedPage сруктура, а структура копіюється в кожному потоці?!
//            if (IsDetailedView)
//                CurentDetailedPage++;
//            else
//                CurentListedPage++;

//           // bool stayOnPage = !string.IsNullOrEmpty(param);
//            var latestPage = Math.Max(CurentListedPage, CurentDetailedPage);
//           // if (stayOnPage) --latestPage;

//            if (IsDetailedView)
//            {
//                if (NoMoreDetailedPages) return;
//                for (int i = CurentDetailedPage-1; i < latestPage; i++)
//                {
//                    var detailedMedia = await GetDetailedMediaAsync(i);
//                    if (detailedMedia == null || detailedMedia.Length == 0)
//                    {
//                        NoMoreDetailedPages = true;
//                        return;
//                    }

//                    foreach (var media in detailedMedia)
//                        DetailedMediaCollection.Add(media);
//                }
//                CurentDetailedPage = latestPage;
//            }
//            else
//            {
//                if (NoMoreListedPages) return;
//                for (int i = CurentListedPage-1; i < latestPage; i++)
//                {
//                    var listedMedia = await GetListedMediaAsync(i);
//                    if (listedMedia == null || listedMedia.Length == 0)
//                    {
//                        NoMoreListedPages = true;
//                        return;
//                    }

//                    foreach (var media in listedMedia)
//                        ListedMediaCollection.Add(media);
//                }
//                CurentListedPage = latestPage;
//            }
//        }

//        #region Допоміжні для OnNextPage

//        protected async virtual Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _repository.GetListedMediaAsync(page);
//        }

//        protected async virtual Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _repository.GetDetailedMediaAsync(page);
//        }

//        #endregion

//        protected override async Task OnRefreshPage(object param)
//        {
//            if (param == null)
//                await base.OnRefreshPage(null);

//            CurentDetailedPage = CurentListedPage = 0;
//            NoMoreDetailedPages = NoMoreListedPages = false;
//            ListedMediaCollection = new ObservableCollection<MediaListed>();
//            DetailedMediaCollection = new ObservableCollection<MediaDetailed>();
//            await OnNextPage();
//        }


//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _repository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }


//        //public override void SaveState(Dictionary<string, object> pageState)
//        //{
//        //    base.SaveState(pageState);
//        //    //треба?
//        //}
//    }
//}
