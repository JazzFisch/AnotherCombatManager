using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD4e.LibraryHelper.Character {
    public class StatBlock : KeyedCollection<string, Stat> {
        protected override string GetKeyForItem (Stat item) {
            return item.Aliases[0].Name;
        }
    }
}
