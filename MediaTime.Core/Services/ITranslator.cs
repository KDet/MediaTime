namespace MediaTime.Core.Services
{
    public interface ITranslator
    {
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.IDictionary<object, object>> Dictionaries { get; }
        string CurrentLanguage { set; get; }
        string TranslateQuick(string keyWord, TranslateMode mode = TranslateMode.Safe);
        string TranslateAutoTo(string word, string language, TranslateMode mode = TranslateMode.Safe);
        string TranslateAuto(string word, TranslateMode mode = TranslateMode.Safe);
    }
}