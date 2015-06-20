using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;
using MediaTime.Core.Entities;
using MediaTime.Core.Repositories.FsServiceRepository;
using MediaTime.Core.Services;
using MediaTime.Core.ViewModels;
using MediaTime.Localization;

namespace MediaTime.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
               .EndingWith("Repository")
               .AsInterfaces()
               .RegisterAsLazySingleton();
            
            //якщо не зареєструвати окремо то баг!
            Mvx.RegisterSingleton<IHtmlPageLoaderService>(new HtmlPageLoaderService());
            Mvx.LazyConstructAndRegisterSingleton<IMvxJsonConverter, MvxJsonConverter>();

            InitializeText();
            RegisterAppStart<HomeViewModel>();
        }

        private static void InitializeText()
        {
            //var builder = new TextProviderBuilder();
            //Mvx.RegisterSingleton<IMvxTextProviderBuilder>(builder);
            //Mvx.RegisterSingleton<IMvxTextProvider>(builder.TextProvider);
            var provider = new ResxTextProvider(Strings.ResourceManager);
            Mvx.RegisterSingleton<IMvxTextProvider>(provider);
            Mvx.RegisterSingleton<IMvxTextProviderBuilder>(new ResTextProviderBuilder(provider));
        }
    }
}