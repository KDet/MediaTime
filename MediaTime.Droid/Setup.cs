using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using MediaTime.Core.ViewModels;
using MediaTime.Droid.Views;

namespace MediaTime.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

        //protected override System.Collections.Generic.List<System.Reflection.Assembly> ValueConverterAssemblies
        //{
        //    get
        //    {
        //        var toReturn = base.ValueConverterAssemblies;
        //        toReturn.Add(typeof(MvxLanguageConverter).Assembly);
        //        return toReturn;
        //    }
        //}

        protected override void InitializeViewLookup()
        {
            var viewModelViewLookup = new Dictionary<Type, Type>
            {
                {typeof (HomeViewModel), typeof (HomeView)},

                //{typeof (VideoViewModel), typeof (CategoryView)},
                //{typeof (AudioViewModel), typeof (CategoryView)},
                //{typeof (TextsViewModel), typeof (CategoryView)},
                //{typeof (GamesViewModel), typeof (CategoryView)},

                //{typeof (MediaItemViewModel), typeof (MediaItemView)},
            };
            var container = Mvx.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelViewLookup);
            //base.InitializeViewLookup();
        }
    }
}