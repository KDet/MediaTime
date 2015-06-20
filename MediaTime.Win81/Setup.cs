using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsStore.Platform;
using MediaTime.Core.Services;
using MediaTime.Core.ViewModels;
using MediaTime.Win81.Views;

namespace MediaTime.Win81
{
    public class Setup : MvxStoreSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
       
        protected override void InitializeViewLookup()
        {
            var viewModelViewLookup = new Dictionary<Type, Type>
            {
                {typeof (HomeViewModel), typeof (HomeView)},

                {typeof (VideoViewModel), typeof (CategoryView)},
                {typeof (AudioViewModel), typeof (CategoryView)},
                {typeof (TextsViewModel), typeof (CategoryView)},
                {typeof (GamesViewModel), typeof (CategoryView)},

                {typeof (MediaItemViewModel), typeof (MediaItemView)},
            };
            var container = Mvx.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelViewLookup);
            //base.InitializeViewLookup();
        }
        protected override void InitializeLastChance()
        {
           // Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            //Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
           base.InitializeLastChance();
        }
    }
}