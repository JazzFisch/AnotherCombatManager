using System;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    internal class Rules : DiscoverableKeyedCollection<string, Rule> {
        protected override string GetKeyForItem (Rule rule) {
            if (!String.IsNullOrWhiteSpace(rule.InternalId)) {
                return rule.InternalId;
            }
            else {
                return rule.Name;
            }
        }
    }

    internal class Specifics : DiscoverableKeyedCollection<string, Specific> {
        protected override string GetKeyForItem (Specific specific) {
            return specific.Name;
        }
    }

    internal class Stats : DiscoverableKeyedCollection<string, Stat> {
        protected override string GetKeyForItem (Stat stat) {
            return stat.Aliases[0].Name;
        }
    }
}
