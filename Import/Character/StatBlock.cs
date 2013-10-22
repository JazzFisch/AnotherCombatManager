using System;
using System.Collections.ObjectModel;

namespace DnD4e.LibraryHelper.Import.Character {
    internal class StatBlock : KeyedCollection<string, Stat> {
        protected override string GetKeyForItem (Stat item) {
            return item.Aliases[0].Name;
        }
    }
}
