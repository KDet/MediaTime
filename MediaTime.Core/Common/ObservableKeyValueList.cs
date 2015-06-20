using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MediaTime.Core.ViewModels;

namespace MediaTime.Core.Common
{
    public class ObservableKeyValueList<TKey, TValue> : ObservableCollection<ObservableKeyValuePair<TKey, TValue>>
    {
        public ObservableKeyValueList() { }
        public ObservableKeyValueList(IEnumerable<ObservableKeyValuePair<TKey, TValue>> keyValuePairs) :base(keyValuePairs) { }

        public ObservableKeyValueList(IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
            : base(keyValuePairs.Select(
                keyValuePair => new ObservableKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value))) { }
    }

    public class ObservableKeyValuePair<TKey, TValue> : BindableBase
    {
        private TValue _value;
        private TKey _key;

        public TValue Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value, "Value"); }
        }
        public TKey Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value, "Key"); }
        }

        public ObservableKeyValuePair() { }
        public ObservableKeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }
    }
}