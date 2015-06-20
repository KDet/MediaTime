using System;
using System.Globalization;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using Cirrious.MvvmCross.ViewModels;

namespace MediaTime.Core.ViewModels
{
    public enum MediaMenuOptions
    {
        Video,
        Audio,
        Texts,
        Games,
    }

    public enum VideoMenuOptions
    {
        Films,
        Serials,
        Cartoons,
        CartoonSerials,
        TvShow,
        Clips,
        Concerts,
    }

    public enum AudioMenuOptions
    {
        MusicAlbums,
        Singles,
        MusicCollections,
        Soundtracks,
    }

    public enum TextsMenuOptions
    {
        FictionLiterature,
        AppliedLiterature,
        Journals,
        Comix
    }

    public enum GamesMenuOptions
    {
        TraditionalGames,
        OnlineGames,
        CasualGames
    }

    public class MenuViewModel : BaseViewModel
    {
        private readonly string DefaultLang = CultureInfo.CurrentUICulture.Name;

        private readonly IMvxTextProviderBuilder _textProviderBuilder;
        private MvxCommand<string> _pickLangCommand;
        private MvxCommand<MediaMenuOptions> _goToCategoryCommand;
        private MvxCommand<VideoMenuOptions> _goToVideoCategoryCommand;
        private MvxCommand<AudioMenuOptions> _goToAudioCategoryCommand;
        private MvxCommand<TextsMenuOptions> _goToTextsCategoryCommand;
        private MvxCommand<GamesMenuOptions> _goToGamesCommand;
        private MvxCommand _goBackCommand;

        public MenuViewModel(IMvxTextProviderBuilder textProviderBuilder)
        {
            _textProviderBuilder = textProviderBuilder;
        }
        public override void Init()
        {
            _textProviderBuilder.LoadResources(DefaultLang);
            base.Init();
        }
        public MvxCommand<string> PickLangCommand
        {
            get
            {
                return _pickLangCommand ??
                       (_pickLangCommand = new MvxCommand<string>(culture =>
                       {
                           _textProviderBuilder.LoadResources(culture);
                           RaisePropertyChanged(() => TextSource);
                       }));
            }
        }
        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder(string.Empty, GetType().Name); }
        }

        /// <summary>
        /// Команда для переходу на сторінку категорії медіа
        /// </summary>
        public MvxCommand<MediaMenuOptions> GoToCategoryCommand
        {
            get
            {
                return _goToCategoryCommand ?? (_goToCategoryCommand = new MvxCommand<MediaMenuOptions>(param =>
                {
                    switch (param)
                    {
                        case MediaMenuOptions.Video:
                            ShowViewModel<VideoViewModel>();
                            break;
                        case MediaMenuOptions.Audio:
                            ShowViewModel<AudioViewModel>();
                            break;
                        case MediaMenuOptions.Texts:
                            ShowViewModel<TextsViewModel>();
                            break;
                        case MediaMenuOptions.Games:
                            ShowViewModel<GamesViewModel>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("param");
                    }
                }));
            }
        }
        /// <summary>
        /// Команда для переходу на сторінку підкатегорії відео медіа
        /// </summary>
        public MvxCommand<VideoMenuOptions> GoToVideoCategoryCommand
        {
            get
            {
                return _goToVideoCategoryCommand ?? (_goToVideoCategoryCommand = new MvxCommand<VideoMenuOptions>(param =>
                {
                    switch (param)
                    {
                        case VideoMenuOptions.Films:
                            ShowViewModel<FilmsViewModel>();
                            break;
                        case VideoMenuOptions.Serials:
                            ShowViewModel<SerialsViewModel>();
                            break;
                        case VideoMenuOptions.Cartoons:
                            ShowViewModel<CartoonsViewModel>();
                            break;
                        case VideoMenuOptions.CartoonSerials:
                            ShowViewModel<CartoonSerialsViewModel>();
                            break;
                        case VideoMenuOptions.TvShow:
                            ShowViewModel<TvShowViewModel>();
                            break;
                        case VideoMenuOptions.Clips:
                            ShowViewModel<ClipsViewModel>();
                            break;
                        case VideoMenuOptions.Concerts:
                            ShowViewModel<ConcertsViewModel>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("param");
                    }
                }));
            }
        }
        /// <summary>
        /// Команда для переходу на сторінку аудіо- категорії медіа
        /// </summary>
        public MvxCommand<AudioMenuOptions> GoToAudioCategoryCommand
        {
            get
            {
                return _goToAudioCategoryCommand ??
                       (_goToAudioCategoryCommand =
                           new MvxCommand<AudioMenuOptions>(param => ShowViewModel<AudioViewModel>()));
            }
        }
        /// <summary>
        /// Команда для переходу на сторінку текст-категорії медіа
        /// </summary>
        public MvxCommand<TextsMenuOptions> GoToTextsCategoryCommand
        {
            get
            {
                return _goToTextsCategoryCommand ??
                       (_goToTextsCategoryCommand =
                           new MvxCommand<TextsMenuOptions>(param => ShowViewModel<TextsViewModel>()));
            }
        }
        /// <summary>
        /// Команда для переходу на сторінку ігри-категорії медіа
        /// </summary>
        public MvxCommand<GamesMenuOptions> GoToGamesCommand
        {
            get
            {
                return _goToGamesCommand ??
                       (_goToGamesCommand = new MvxCommand<GamesMenuOptions>(param => ShowViewModel<GamesViewModel>()));
            }
        }

        /// <summary>
        /// Команда переходу на попередню сторінку стеку навігації
        /// </summary>
        public MvxCommand GoBackCommand
        {
            get { return _goBackCommand ?? (_goBackCommand =  new MvxCommand(() => Close(this))); }
        }
    }
}