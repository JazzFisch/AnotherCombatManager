using System;

namespace AnotherCM.Library.Import.Common {
    public sealed class EnumCollection<TKey, TItem> : DiscoverableKeyedCollection<TKey, TItem>
        where TKey : struct
        where TItem : INamedObject 
    {
        protected override TKey GetKeyForItem (TItem item) {
            TKey key;
            Enum.TryParse<TKey>(item.Name, true, out key);
            return key;
        }
    }
}
