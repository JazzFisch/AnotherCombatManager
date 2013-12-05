using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace DnD4e.LibraryHelper.Common {
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
