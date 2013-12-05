using System;
using System.Collections.ObjectModel;

namespace AnotherCM.Library.Import.Character {
    public class StatBlock : KeyedCollection<string, Stat> {
        protected override string GetKeyForItem (Stat item) {
            return item.Aliases[0].Name;
        }
    }
}
