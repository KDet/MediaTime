using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository.Filters;
using MediaTime.Core.Services;

namespace MediaTime.Core.ViewModels
{
    public abstract class SubCategoryViewModel : CategoryViewModel
    {
        public class FilterViewModel : BindableBase
        {
            //ДЛЯ ЛОГІКИ
            //для назви перечислення
            private string _key;
            public string Key
            {
                get { return _key; }
                set { SetProperty(ref _key, value); }
            }

            //для всіх значень перечислення
            private object[] _value;
            public object[] Value
            {
                get { return _value; }
                set { SetProperty(ref _value, value); }
            }

            private object _selectedParameter;
            public object SelectedParameter
            {
                get { return _selectedParameter; }
                set
                {
                    SetProperty(ref _selectedParameter, value);
                    var oldValue = _filtersSource.Filters[Key];
                    _filtersSource.Filters[Key] = value;
                    OnFilterChangedValue(Key, oldValue, value);
                }
            }

            //private int _selectedIndex;
            //public int SelectedIndex
            //{
            //    get { return _selectedIndex; }
            //    set { SetProperty(ref _selectedIndex, value); }
            //}

            //ДЛЯ ВІДОБРАЖЕННЯ
            //тут логіка перекладу!
            public string Title { get { return Translator.Current.TranslateQuick(Key); } }

            //тут логіка заміни значення текстом

            public string[] Options { get { return Translator.Current.TranslateRangeQuick(Value.ToStringList()); } }

            private readonly IFilters _filtersSource;

            public static event EventHandler<FilterEventArgs> FilterChangedValue;
            protected virtual void OnFilterChangedValue(string key, object oldValue, object newValue)
            {
                EventHandler<FilterEventArgs> handler = FilterChangedValue;
                if (handler != null) handler(this, new FilterEventArgs(this, key, oldValue, newValue));
            }

            public class FilterEventArgs : EventArgs
            {
                public FilterViewModel OriginalSource { get; private set; }
                public string Key { get; private set; }
                public object OldValue { get; private set; }
                public object NewValue { get; private set; }

                public FilterEventArgs(FilterViewModel source, string key, object oldValue, object newValue)
                {
                    OriginalSource = source;
                    Key = key;
                    OldValue = oldValue;
                    NewValue = newValue;
                }
            }

            public FilterViewModel(IFilters filtersSource)
            {
                _filtersSource = filtersSource;
            }

            public static ObservableCollection<FilterViewModel> Factory(IFilters filtersSource, string[] customLanguages, string[] customTranslations)
            {
                var filters = filtersSource.Filters.GetFilters();
                var filtersViewModels = new ObservableCollection<FilterViewModel>();
                foreach (var filter in filters.Where(pair => pair.Key != "TranslateCustom" && pair.Key != "LanguageCustom"))
                {
                    filtersViewModels.Add(new FilterViewModel(filtersSource)
                    {
                        Key = filter.Key,
                        Value = filter.Value
                    });
                }
                if (filters.ContainsKey("TranslateCustom") && customTranslations != null)
                {
                    filtersViewModels.Add(new FilterViewModel(filtersSource)
                    {
                        Key = "TranslateCustom",
                        Value = customTranslations.ToArrayOf<object>()
                    });
                }
                if (filters.ContainsKey("LanguageCustom") && customLanguages != null)
                {
                    filtersViewModels.Add(new FilterViewModel(filtersSource)
                    {
                        Key = "LanguageCustom",
                        Value = customLanguages.ToArrayOf<object>()
                    });
                }
                return filtersViewModels;
            }
        }

        private readonly IFilters _filtersSource;

        protected SubCategoryViewModel(ISubCategoryRepository subCategoryRepository, IFilters filtersSource, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(subCategoryRepository,favoriteRepository, textProviderBuilder)
        {
            _filtersSource = filtersSource;
            FilterViewModel.FilterChangedValue += (sender, args) =>
            {
                if (Equals(args.OldValue, args.NewValue)) return;
                RefreshPageCommand.Execute();
            };
            //SortParameterChanged += (sender, args) =>
            //{
            //    
            //};
        }

        private Sort _sortParameter = Sort.Default;
        public Sort SortParameter
        {
            get { return _sortParameter; }
            set
            {
                if(_sortParameter == value) return;
                _sortParameter = value;
                RaisePropertyChanged(() => SortParameter);
                RefreshPageCommand.Execute();
            }
        }
        //#region Подія - змінено параметр сортування

        //private event EventHandler<SortEventArgs> SortParameterChanged;

        //private void OnSortParameterChanged(ref Sort oldValue, ref Sort newValue)
        //{
        //    EventHandler<SortEventArgs> handler = SortParameterChanged;
        //    if (handler != null) handler(this, new SortEventArgs(ref oldValue, ref newValue));
        //}

        //public class SortEventArgs : EventArgs
        //{
        //    public Sort OldValue { get; private set; }
        //    public Sort NewValue { get; private set; }

        //    public SortEventArgs(ref Sort oldValue, ref Sort newValue)
        //    {
        //        OldValue = oldValue;
        //        NewValue = newValue;
        //    }
        //}

        //#endregion

        private ObservableCollection<FilterViewModel> _filtersViewModels;
        public ObservableCollection<FilterViewModel> FiltersViewModels
        {
            get { return _filtersViewModels; }
            set
            {
                if (_filtersViewModels == value) return;
                _filtersViewModels = value;
                RaisePropertyChanged(() => FiltersViewModels);
                RefreshPageCommand.Execute();
            }
        }
        public override void Init()
        {
            FiltersViewModels = FilterViewModel.Factory(_filtersSource, null, null);
            base.Init();
        }
        public override void Start()
        {
            base.Start();
          //  LifecycleState = Lifecycle.Start;
            
            
            ////дофетчити вкінці фільтри
            //if (_filtersSource.Filters.ContainsKey("LanguageCustom"))
            //{
            //    var filter = new FilterViewModel(_filtersSource);
            //    filter.Key = "LanguageCustom";
            //    filter.Value = await GetCustomFilters(CustomFilter.Language);
            //    FiltersViewModels.Add(filter);
            //}
            //if (_filtersSource.Filters.ContainsKey("TranslateCustom"))
            //{
            //    var filter = new FilterViewModel(_filtersSource);
            //    filter.Key = "TranslateCustom";
            //    filter.Value = await GetCustomFilters(CustomFilter.Translation);
            //    FiltersViewModels.Add(filter);
            //}
           // LifecycleState = Lifecycle.Run;
        }

        protected virtual Task<string[]> GetCustomFilters(CustomFilter customFilter)
        {
            return null;
        }
    }

    public class FilmsViewModel : SubCategoryViewModel
    {
        private readonly IFilmsRepository _filmsRepository;
        private readonly FilmsFilters _filmsFilters;

        public FilmsViewModel(IFilmsRepository filmsRepository,IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder builder)
            : this(filmsRepository, new FilmsFilters(),favoriteRepository, builder) { }
        public FilmsViewModel(IFilmsRepository filmsRepository, FilmsFilters filmFilters, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(filmsRepository, filmFilters,favoriteRepository, textProviderBuilder)
        {
            _filmsRepository = filmsRepository;
            _filmsFilters = filmFilters;
        }
        protected override Task<MediaDetailed[]> GetDetailedMediaAsync()
        {
            return _filmsRepository.GetDetailedMediaAsync(_filmsFilters, SortParameter);
        }

        protected override Task<MediaListed[]> GetListedMediaAsync()
        {
            return _filmsRepository.GetListedMediaAsync(_filmsFilters, SortParameter);
        }

        protected override Task<string[]> GetCustomFilters(CustomFilter customFilter)
        {
            switch (customFilter)
            {
                case CustomFilter.Language:
                    return _filmsRepository.GetCustomLanguagesAsync();
                case CustomFilter.Translation:
                    return _filmsRepository.GetCustomTranslateAsync();
                default:
                    return null;
            }
        }
    }

    public class SerialsViewModel : SubCategoryViewModel
    {
        private readonly ISerialsRepository _serialsRepository;
        private readonly SerialsFilters _serialsFilters;

        public SerialsViewModel(ISerialsRepository serialsRepository,IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder builder)
            : this(serialsRepository, new SerialsFilters(),favoriteRepository, builder) { }
        public SerialsViewModel(ISerialsRepository serialsRepository, SerialsFilters serialsFilters, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(serialsRepository, serialsFilters,favoriteRepository, textProviderBuilder)
        {
            _serialsRepository = serialsRepository;
            _serialsFilters = serialsFilters;
        }
    }

    public class CartoonsViewModel : SubCategoryViewModel
    {
        private readonly ICartoonsRepository _cartoonsRepository;
        private readonly CartoonsFilters _cartoonsFilters;

        public CartoonsViewModel(ICartoonsRepository cartoonsRepository,IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder builder)
            : this(cartoonsRepository, new CartoonsFilters(),favoriteRepository, builder) { }
        public CartoonsViewModel(ICartoonsRepository cartoonsRepository, CartoonsFilters cartoonsFilters, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(cartoonsRepository, cartoonsFilters,favoriteRepository, textProviderBuilder)
        {
            _cartoonsRepository = cartoonsRepository;
            _cartoonsFilters = cartoonsFilters;
        }
    }

    public class CartoonSerialsViewModel : SubCategoryViewModel
    {
        private readonly ICartoonSerialsRepository _cartoonSerialsRepository;
        private readonly CartoonSerialsFilters _cartoonSerialsFilters;

        public CartoonSerialsViewModel(ICartoonSerialsRepository cartoonSerialsRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder builder)
            : this(cartoonSerialsRepository, new CartoonSerialsFilters(),favoriteRepository, builder) { }
        public CartoonSerialsViewModel(ICartoonSerialsRepository cartoonSerialsRepository, CartoonSerialsFilters cartoonSerialsesFilters, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(cartoonSerialsRepository, cartoonSerialsesFilters,favoriteRepository, textProviderBuilder)
        {
            _cartoonSerialsRepository = cartoonSerialsRepository;
            _cartoonSerialsFilters = cartoonSerialsesFilters;
        }
    }

    public class TvShowViewModel : SubCategoryViewModel
    {
        private readonly ITvShowRepository _tvShowRepository;
        private readonly TvShowFilters _tvShowFilters;

        public TvShowViewModel(ITvShowRepository tvShowRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder builder)
            : this(tvShowRepository, new TvShowFilters(),favoriteRepository, builder) { }
        public TvShowViewModel(ITvShowRepository tvShowRepository, TvShowFilters tvShowFilters, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(tvShowRepository, tvShowFilters,favoriteRepository, textProviderBuilder)
        {
            _tvShowRepository = tvShowRepository;
            _tvShowFilters = tvShowFilters;
        }
    }

    public class ClipsViewModel : SubCategoryViewModel
    {
        private readonly IClipsRepository _clipsRepository;
        private readonly ClipsFilters _clipsFilters;

        public ClipsViewModel(IClipsRepository clipsRepository,IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder builder)
            : this(clipsRepository, new ClipsFilters(),favoriteRepository, builder) { }
        public ClipsViewModel(IClipsRepository clipsRepository, ClipsFilters clipsFilters, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(clipsRepository, clipsFilters,favoriteRepository, textProviderBuilder)
        {
            _clipsRepository = clipsRepository;
            _clipsFilters = clipsFilters;
        }
    }

    public class ConcertsViewModel : SubCategoryViewModel
    {
        private readonly IConcertsRepository _concertsRepository;
        private readonly ConcertsFilters _concertsFilters;

        public ConcertsViewModel(IConcertsRepository concertsRepository, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder builder)
            : this(concertsRepository, new ConcertsFilters(),favoriteRepository, builder) { }
        public ConcertsViewModel(IConcertsRepository concertsRepository, ConcertsFilters concertsFilters, IFavoriteRepository favoriteRepository, IMvxTextProviderBuilder textProviderBuilder)
            : base(concertsRepository, concertsFilters,favoriteRepository, textProviderBuilder)
        {
            _concertsRepository = concertsRepository;
            _concertsFilters = concertsFilters;
        }
    }
}