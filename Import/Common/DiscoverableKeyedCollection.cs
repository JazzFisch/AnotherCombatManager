using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DnD4e.LibraryHelper.Import.Common {
    internal abstract class DiscoverableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem> {
        public ICollection<TKey> Keys {
            get {
                if (base.Dictionary != null) {
                    return base.Dictionary.Keys;
                }
                else {
                    var items = from item in base.Items
                                select this.GetKeyForItem(item);
                    return items.ToArray();
                }
            }
        }

        public bool TryGetValue (TKey key, out TItem value) {
            if (base.Dictionary != null) {
                return this.Dictionary.TryGetValue(key, out value);
            }
            else {
                var items = from item in base.Items
                            where this.GetKeyForItem(item).Equals(key)
                            select item;
                if (items.Any()) {
                    value = items.First();
                    return true;
                }
                else {
                    value = default(TItem);
                    return false;
                }
            }
        }
    }
}
