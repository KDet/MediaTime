using System.Linq;

namespace MediaTime.Core.Services
{
    public class Translator : ITranslator
    {
        private static Translator _instance;

        private static readonly System.Collections.Generic.Dictionary<string, System.Collections.Generic.IDictionary<object, object>> DictionariesInstance = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IDictionary<object, object>>();

        private string _currentLanguage = "Українська";

        public static Translator Current
        {
            get { return _instance ?? (_instance = new Translator()); }
        }

        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.IDictionary<object, object>> Dictionaries
        {
            get { return DictionariesInstance; }           
        }

        public string CurrentLanguage
        {
            set
            {
                //якщо словник не мітить вказаної мови - помилка
                if (!DictionariesInstance.ContainsKey(value))
                    throw new System.Collections.Generic.KeyNotFoundException();
                _currentLanguage = value;
            }
            get
            {
                //То саме що й перевірити чи словник не пустий (на початку Українська мова може бути не інстальована, або будуть інші мови але ще не змінено значення поточної)
                return DictionariesInstance.ContainsKey(_currentLanguage) ? _currentLanguage : null;
            }
        }

        protected System.Collections.Generic.IDictionary<object, object> CurrentDictionary
        {
            get
            {
                if (DictionariesInstance.Count == 0 || !DictionariesInstance.ContainsKey(_currentLanguage))
                    return null;
                return DictionariesInstance[_currentLanguage];
            }
        }

        public string TranslateQuick(string keyWord, TranslateMode mode = TranslateMode.Safe)
        {
            if (CurrentDictionary == null || string.IsNullOrEmpty(keyWord))
                return mode == TranslateMode.Safe ? keyWord : null;

            object value;
            return CurrentDictionary.TryGetValue(keyWord, out value)
                ? value.ToString()
                : (mode == TranslateMode.Safe ? keyWord : null);
        }

        public string TranslateAutoTo(string word, string language, TranslateMode mode = TranslateMode.Safe)
        {
            if(!Dictionaries.ContainsKey(language))
                throw new System.Collections.Generic.KeyNotFoundException();

            //перевірити в точному режимі чи слово не є ключем
            var translated = TranslateQuick(word, TranslateMode.Exactly);
            if (!string.IsNullOrEmpty(translated))
                return translated;

            //пошук слова по всьому словнику
            var entry = 
                Dictionaries.SelectMany(
                    dictionary => dictionary.Value.Where(translation => Equals(translation.Value, word))).FirstOrDefault();

            var key = Equals(entry, default(System.Collections.Generic.KeyValuePair<object, object>)) ? null : entry.Key;
            return key != null ? Dictionaries[language][key].ToString() : (mode == TranslateMode.Safe ? word : null); 
        }

        public string TranslateAuto(string word, TranslateMode mode = TranslateMode.Safe)
        {
            return TranslateAutoTo(word, CurrentLanguage, mode);
        }

        public static bool LoadDictionaryResource(System.Collections.Generic.IDictionary<object, object> dictionaryResource)
        {
            return dictionaryResource != null &&
                   dictionaryResource.All(
                       dictionary =>
                           LoadDictinary(dictionary.Key.ToString(), dictionary.Value as System.Collections.Generic.IDictionary<object, object>));
        }

        public static bool LoadDictinary(string language, System.Collections.Generic.IDictionary<object, object> dictionary)
        {
            if (dictionary== null || DictionariesInstance.ContainsKey(language)) return false;
            
            //перекладу будуть підлягати й цілі об'єкти
            DictionariesInstance.Add(language, dictionary);
            return true;
        }

        //тут використовуються стрічки а не перечислення (більш зручно), оскільки можуть завантажуватись словники для різних мов.
        //Мова по замовчуванні українська

        public string[] TranslateRangeQuick(System.Collections.Generic.IEnumerable<string> keyWords, TranslateMode mode = TranslateMode.Safe)
        {
            return (from keyWord in keyWords
                select TranslateQuick(keyWord, mode)).ToArray();
        }


        public string[] TranslateRangeAuto(System.Collections.Generic.IEnumerable<string> words, TranslateMode mode = TranslateMode.Safe)
        {
            return TranslateRangeAutoTo(words, CurrentLanguage, mode);
        }

        public string[] TranslateRangeAutoTo(System.Collections.Generic.IEnumerable<string> words, string language, TranslateMode mode = TranslateMode.Safe)
        {
            return (from word in words
                select TranslateAutoTo(word,language, mode)).ToArray();
        }
    }

    public enum TranslateMode
    {
        Exactly,
        Safe
    }
}
