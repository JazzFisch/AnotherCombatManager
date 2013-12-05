using System;
using Newtonsoft.Json.Converters;

namespace AnotherCM.Library.Common {
    internal class ObservableKeyedCollectionConverter<TKey, TItem> : CustomCreationConverter<ObservableKeyedCollection<TKey, TItem>> {
        private Func<TItem, TKey> keyFunction;

        public ObservableKeyedCollectionConverter (Func<TItem, TKey> keyFunction) {
            this.keyFunction = keyFunction;
        }

        public override ObservableKeyedCollection<TKey, TItem> Create (Type objectType) {
            return new ObservableKeyedCollection<TKey, TItem>(this.keyFunction);
        }
    }
}
