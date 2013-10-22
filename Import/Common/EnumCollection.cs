using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.Import.Common {
    internal sealed class EnumCollection<TKey, TItem> : DiscoverableKeyedCollection<TKey, TItem>
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
