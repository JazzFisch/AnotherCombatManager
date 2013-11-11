using System;
using System.Collections.Generic;
using System.Linq;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Rules : DiscoverableKeyedCollection<string, Rule> {
        private Dictionary<string, Dictionary<string, List<Rule>>> byType;

        protected override string GetKeyForItem (Rule rule) {
            if (!String.IsNullOrWhiteSpace(rule.InternalId)) {
                return rule.InternalId;
            }
            else {
                return rule.Name;
            }
        }

        public Dictionary<string, Dictionary<string, List<Rule>>> ByType () {
            this.byType = new Dictionary<string, Dictionary<string, List<Rule>>>();
            foreach (var rule in this.Items) {
                Dictionary<string, List<Rule>> rules;
                List<Rule> matches;
                string type = rule.Type;

                if (!this.byType.TryGetValue(type, out rules)) {
                    rules = new Dictionary<string, List<Rule>>();
                    rules.Add(rule.Name, new List<Rule>() { rule });
                    this.byType[type] = rules;
                }
                else if (!rules.TryGetValue(rule.Name, out matches)) {
                    rules.Add(rule.Name, new List<Rule>() { rule });
                }
                else {
                    rules[rule.Name].Add(rule);
                }
            }
            return this.byType;
        }

        public bool TryGetFirstByTypeAndName (string type, string name, out Rule rule) {
            Dictionary<string, List<Rule>> rules;
            List<Rule> matches;
            rule = null;

            if (this.ByType().TryGetValue(type, out rules) && rules.TryGetValue(name, out matches)) {
                rule = matches.First();
                return true;
            }
            return false;
        }
    }

    public class Specifics : DiscoverableKeyedCollection<string, Specific> {
        protected override string GetKeyForItem (Specific specific) {
            return specific.Name;
        }

        protected override void InsertItem (int index, Specific item) {
            Specific existing;
            if (base.TryGetValue(item.Name, out existing)) {
                existing.Value = String.Concat(existing.Value, ", ", item.Value);
                return;
            }
            base.InsertItem(index, item);
        }
    }

    public class Stats : DiscoverableKeyedCollection<string, Stat> {
        protected override string GetKeyForItem (Stat stat) {
            return stat.Aliases[0].Name;
        }
    }
}
