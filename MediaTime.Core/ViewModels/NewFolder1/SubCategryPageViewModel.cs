//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Threading.Tasks;
//using MediaTime.Core.Model;
//using MediaTime.Core.Repositories.FsServiceRepository;
//using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories;
//using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
//using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository.Filters;
//using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository;
//using MediaTime.Core.Services;

//namespace MediaTime.Core.ViewModels
//{
//    public class FilterViewModel : BindableBase
//    {
//        //ДЛЯ ЛОГІКИ
//        //для назви перечислення
//        private string _key;
//        public string Key
//        {
//            get { return _key; }
//            set { SetProperty(ref _key, value); }
//        }

//        //для всіх значень перечислення
//        private Array _value;
//        public Array Value
//        {
//            get { return _value; }
//            set { SetProperty(ref _value, value); }
//        }

//        private object _selectedParameter;
//        public object SelectedParameter
//        {
//            get { return _selectedParameter; }
//            set
//            {
//                SetProperty(ref _selectedParameter, value);
//                var oldValue = _filtersSource.Filters[Key];
//                _filtersSource.Filters[Key] = value;
//                OnFilterChangedValue(Key, oldValue, value);
//            }
//        }

//        private int _selectedIndex;
//        public int SelectedIndex
//        {
//            get { return _selectedIndex; }
//            set { SetProperty(ref _selectedIndex, value); }
//        }

//        //ДЛЯ ВІДОБРАЖЕННЯ
//        //тут логіка перекладу!
//        public string Title { get { return Translator.Current.TranslateQuick(Key); } }

//        //тут логіка заміни значення текстом

//        public string[] Options { get { return Translator.Current.TranslateRangeQuick(Value.ToStringList()); } }

//        private readonly IFilters _filtersSource;

//        public static event EventHandler<FilterEventArgs> FilterChangedValue;
//        protected virtual void OnFilterChangedValue(string key, object oldValue, object newValue)
//        {
//            EventHandler<FilterEventArgs> handler = FilterChangedValue;
//            if (handler != null) handler(this, new FilterEventArgs(this, key, oldValue, newValue));
//        }

//        public class FilterEventArgs : EventArgs
//        {
//            public FilterViewModel OriginalSource { get; private set; }
//            public string Key { get; private set; }
//            public object OldValue { get; private set; }
//            public object NewValue { get; private set; }

//            public FilterEventArgs(FilterViewModel source, string key, object oldValue, object newValue)
//            {
//                OriginalSource = source;
//                Key = key;
//                OldValue = oldValue;
//                NewValue = newValue;
//            }
//        }

//        public FilterViewModel(IFilters filtersSource)
//        {
//            _filtersSource = filtersSource;
//        }
//    }

//    //Тут перенесено всі заморочки з фільтруванням і сортуванням
//    public abstract class BaseSubCategoryPageViewModel : CategoryViewModel
//    {
//        private ObservableCollection<FilterViewModel> _filters;
//        public ObservableCollection<FilterViewModel> Filters
//        {
//            get { return _filters; }
//            set { SetProperty(ref _filters, value); }
//        }

//        public string[] SortOptions { get; set; }

//        private Sort _sortParameter = Sort.Default;
//        public Sort SortParameter
//        {
//            get { return _sortParameter; }
//            set
//            {
//                var oldValue = _sortParameter;
//                SetProperty(ref _sortParameter, value);
//                OnSortParameterChanged(ref oldValue, ref value);
//            }
//        }

//        #region Подія - змінено параметр сортування

//        private event EventHandler<SortEventArgs> SortParameterChanged;

//        private void OnSortParameterChanged(ref Sort oldValue, ref Sort newValue)
//        {
//            EventHandler<SortEventArgs> handler = SortParameterChanged;
//            if (handler != null) handler(this, new SortEventArgs(ref oldValue, ref newValue));
//        }

//        private class SortEventArgs : EventArgs
//        {
//            public Sort OldValue { get; private set; }
//            public Sort NewValue { get; private set; }

//            public SortEventArgs(ref Sort oldValue, ref Sort newValue)
//            {
//                OldValue = oldValue;
//                NewValue = newValue;
//            }
//        }

//        #endregion

//        private readonly IFilters _subCategoryFilters;

//        protected SubCategoryPageViewModel(IFilters subCategoryFilters)
//        {
//            //SortParameter = Sort.OnRating;
//            _subCategoryFilters = subCategoryFilters;
//            SortParameterChanged += async (sender, args) =>
//                {
//                    if (Equals(args.OldValue, args.NewValue)) return;
//                    await OnRefreshPage("True");
//                };

//            //Distinct для того щоб видалити defaul знаення, яке буде завжди одним з існуючихваріантів
//            SortOptions =

//                    Translator.Current.TranslateRangeQuick(Enum.GetValues(typeof(Sort)).ArrayToStringList().Distinct());
//        }

//        public override async void LoadState(object navigationParameter, Dictionary<string, object> pageState)
//        {
//            var category = Categories.FindByName(navigationParameter as string);
//            if (category == null) return;

//            CategoryColor = category.Color;
//            CategoryName = category.Title;

//            //обнклення всіх параметрів, на випадок, якщо функція буде викликана з іншої функції
//            CurentDetailedPage = CurentListedPage = 0;
//            NoMoreDetailedPages = NoMoreListedPages = false;

//            Filters = new ObservableCollection<FilterViewModel>();
//            var retrievedFilters = _subCategoryFilters.Filters.GetFiltersList();
//            foreach (var retrieveFilter in retrivedFilters.Where(retrieveFilter => !retrieveFilter.Key.Contains("TranslateCustom") && !retrieveFilter.Key.Contains("LanguageCustom")))
//            {
//                Filters.Add(new FilterViewModel(_subCategoryFilters)
//                    {
//                        Key = retrieveFilter.Key,
//                        Value = retrieveFilter.Value,
//                    });
//            }
//            FilterViewModel.FilterChangedValue += OnFilterChangedValue;
//            await OnNextPage();           //завантажити першу сторінку поточного відображення даних (деталізованого чи списком)
//            //завантажити популярне
//            var mediaTop = await GetRelatedMediaAsync();
//            RelatedMedia = new ObservableCollection<Media>(mediaTop.Take(5));

//            //це для того щоб авторські параметри були завжди вкінці і щоб вони правильно відображалися
//            foreach (var retrieveFilter in retrivedFilters)
//            {
//                if (retrieveFilter.Key.Contains("TranslateCustom"))
//                {
//                    Filters.Add(new FilterViewModel(_subCategoryFilters)
//                        {
//                            Key = retrieveFilter.Key,
//                            Value = new[] { "None" }.Union(await GetCustomTranslateAsync()).ToArray()
//                        });
//                }
//                else if (retrieveFilter.Key.Contains("LanguageCustom"))
//                {
//                    Filters.Add(new FilterViewModel(_subCategoryFilters)
//                        {
//                            Key = retrieveFilter.Key,
//                            Value = new[] { "None" }.Union(await GetCustomLanguagesAsync()).ToArray()
//                        });
//                }
//            }
//        }



//        protected virtual Task<string[]> GetCustomTranslateAsync()
//        {
//            return null;
//        }

//        protected virtual Task<string[]> GetCustomLanguagesAsync()
//        {
//            return null;
//        }

//        protected virtual async void OnFilterChangedValue(object sender, FilterViewModel.FilterEventArgs args)
//        {
//            //Якщо попереднє і наступне значення однакові - не фільтрувати
//            if (Equals(args.OldValue, args.NewValue)) return;
//            //Якщо параметрами є стрічки, то перевірити чи подію не спричинила ініціалізація, якщо так пропустити
//            if (Equals(args.OldValue as string, string.Empty) && Equals(args.NewValue as string, "None")) return;
//            await OnRefreshPage("True");
//        }
//    }

//    public class FilmsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly FilmsRepository _filmsRepository;
//        private readonly FilmFilters _filmFilters;

//        public FilmsPageViewModel(FilmFilters filmFilters)
//            : base(filmFilters)
//        {
//            _filmFilters = filmFilters;
//            _filmsRepository = FsRepository.Video.Films;
//        }

//        public FilmsPageViewModel()
//            : this(new FilmFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }


//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _filmsRepository.GetDetailedMediaAsync(_filmFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _filmsRepository.GetListedMediaAsync(_filmFilters, SortParameter, page);
//        }
//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _filmsRepository.GetCustomLanguagesAsync();
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _filmsRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomTranslateAsync()
//        {
//            return await _filmsRepository.GetCustomTranslateAsync();
//        }
//    }

//    public class SerialsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly SerialsRepository _curentRepository;
//        private readonly SerialFilters _curentFilters;

//        public SerialsPageViewModel(SerialFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Video.Serials;
//        }

//        public SerialsPageViewModel()
//            : this(new SerialFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }


//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomTranslateAsync()
//        {
//            return await _curentRepository.GetCustomTranslateAsync();
//        }
//    }

//    public class CartoonsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly CartoonsRepository _curentRepository;
//        private readonly CartoonFilters _curentFilters;

//        public CartoonsPageViewModel(CartoonFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Video.Cartoons;
//        }

//        public CartoonsPageViewModel()
//            : this(new CartoonFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomTranslateAsync()
//        {
//            return await _curentRepository.GetCustomTranslateAsync();
//        }
//    }

//    public class CartoonSerialsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly CartoonSerialsRepository _curentRepository;
//        private readonly CartoonSerialFilters _curentFilters;

//        public CartoonSerialsPageViewModel(CartoonSerialFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Video.CartoonSerials;
//        }

//        public CartoonSerialsPageViewModel()
//            : this(new CartoonSerialFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomTranslateAsync()
//        {
//            return await _curentRepository.GetCustomTranslateAsync();
//        }
//    }

//    public class TVShowsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly TVShowRepository _curentRepository;
//        private readonly TVShowFilters _curentFilters;

//        public TVShowsPageViewModel(TVShowFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Video.TvShow;
//        }

//        public TVShowsPageViewModel()
//            : this(new TVShowFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//    }

//    public class ConcertsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly ConcertsRepository _curentRepository;
//        private readonly ConcertsFilters _curentFilters;

//        public ConcertsPageViewModel(ConcertsFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Video.Concerts;
//        }

//        public ConcertsPageViewModel()
//            : this(new ConcertsFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }
//    }

//    public class ClipsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly ClipsRepository _curentRepository;
//        private readonly ClipsFilters _curentFilters;

//        public ClipsPageViewModel(ClipsFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Video.Clips;
//        }

//        public ClipsPageViewModel()
//            : this(new ClipsFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }
//    }

//    //Аудіо
//    public class AlbumsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly MusicAlbumsRepository _curentRepository;
//        private readonly AlbumFilters _curentFilters;

//        public AlbumsPageViewModel(AlbumFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Audio.MusicAlbums;
//        }

//        public AlbumsPageViewModel()
//            : this(new AlbumFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }
//    }

//    public class SinglesPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly SinglesRepository _curentRepository;
//        private readonly SingleFilters _curentFilters;

//        public SinglesPageViewModel(SingleFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Audio.Singles;
//        }

//        public SinglesPageViewModel()
//            : this(new SingleFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//    }

//    public class MusicCollectionsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly MusicCollectionsRepository _curentRepository;
//        private readonly CollectionFilters _curentFilters;

//        public MusicCollectionsPageViewModel(CollectionFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Audio.MusicCollections;
//        }

//        public MusicCollectionsPageViewModel()
//            : this(new CollectionFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }
//    }

//    public class SoundtracksPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly SoundtracksRepository _curentRepository;
//        private readonly SoundtrackFilters _curentFilters;

//        public SoundtracksPageViewModel(SoundtrackFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Audio.Soundtracks;
//        }

//        public SoundtracksPageViewModel()
//            : this(new SoundtrackFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }
//    }

//    //Література
//    public class FictionLiteraturePageViewModel : SubCategoryPageViewModel
//    {
//        private readonly FictionLiteratureRepository _curentRepository;
//        private readonly TextFilters _curentFilters;

//        public FictionLiteraturePageViewModel(TextFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Texts.FictionLiterature;
//        }

//        public FictionLiteraturePageViewModel()
//            : this(new TextFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }
//    }

//    public class AppliedLiteraturePageViewModel : SubCategoryPageViewModel
//    {
//        private readonly AppliedLiteratureRepository _curentRepository;
//        private readonly TextFilters _curentFilters;

//        public AppliedLiteraturePageViewModel(TextFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Texts.AppliedLiterature;
//        }

//        public AppliedLiteraturePageViewModel()
//            : this(new TextFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }
//    }

//    public class JournalsPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly JournalsRepository _curentRepository;
//        private readonly TextFilters _curentFilters;

//        public JournalsPageViewModel(TextFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Texts.Journals;
//        }

//        public JournalsPageViewModel()
//            : this(new TextFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }
//    }

//    public class ComixPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly ComixRepository _curentRepository;
//        private readonly TextFilters _curentFilters;

//        public ComixPageViewModel(TextFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Texts.Comix;
//        }

//        public ComixPageViewModel()
//            : this(new TextFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }
//    }

//    //Ігри
//    public class TraditionalGamesPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly TraditionalGamesRepository _curentRepository;
//        private readonly TraditionalGameFilters _curentFilters;

//        public TraditionalGamesPageViewModel(TraditionalGameFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Games.TraditionalGames;

//        }

//        public TraditionalGamesPageViewModel()
//            : this(new TraditionalGameFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }

//        protected override async Task<string[]> GetCustomLanguagesAsync()
//        {
//            return await _curentRepository.GetCustomLanguagesAsync();
//        }
//    }

//    public class OnlineGamesPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly OnlineGamesRepository _curentRepository;
//        private readonly OnlineGameFilters _curentFilters;

//        public OnlineGamesPageViewModel(OnlineGameFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Games.OnlineGames;

//        }

//        public OnlineGamesPageViewModel()
//            : this(new OnlineGameFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }
//    }

//    public class CasualGamesPageViewModel : SubCategoryPageViewModel
//    {
//        private readonly CasualGamesRepository _curentRepository;
//        private readonly CasualGameFilters _curentFilters;

//        public CasualGamesPageViewModel(CasualGameFilters filmFilters)
//            : base(filmFilters)
//        {
//            _curentFilters = filmFilters;
//            _curentRepository = FsRepository.Games.CasualGames;

//        }

//        public CasualGamesPageViewModel()
//            : this(new CasualGameFilters())
//        {
//        }

//        protected override async Task OnSearch(string query)
//        {
//            //base.OnSearch(query);
//        }

//        protected override async Task<MediaDetailed[]> GetDetailedMediaAsync(int page)
//        {
//            return await _curentRepository.GetDetailedMediaAsync(_curentFilters, SortParameter, page);
//        }
//        protected override async Task<MediaListed[]> GetListedMediaAsync(int page)
//        {
//            return await _curentRepository.GetListedMediaAsync(_curentFilters, SortParameter, page);
//        }

//        protected override async Task<Media[]> GetRelatedMediaAsync(bool refreshMode = false)
//        {
//            return await _curentRepository.GetRelatedMediaAsync(refreshMode);
//        }
//    }
//}
