using System.Globalization;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;

namespace MediaTime.Core.Services
{
    public class ResTextProviderBuilder : IMvxTextProviderBuilder
    {
        private readonly IResxTextProvider _resxTextProvider;
        public ResTextProviderBuilder(IResxTextProvider aResxTextProvider)
        {
            _resxTextProvider = aResxTextProvider;
        }
        
        public void LoadResources(string localisationName)
        {
            _resxTextProvider.CurrentLanguage = new CultureInfo(localisationName);
        }

        public void LoadResources(CultureInfo cultureInfo)
        {
            _resxTextProvider.CurrentLanguage = cultureInfo;
        }

        public IMvxTextProvider TextProvider { get { return _resxTextProvider; } }
    }
}