using System.Globalization;
using Cirrious.MvvmCross.Localization;

namespace MediaTime.Core.Services
{
    public interface IResxTextProvider : IMvxTextProvider
    {
        CultureInfo CurrentLanguage { get; set; }
    }
}