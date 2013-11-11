using System;
using System.Collections.Generic;
using System.Linq;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.ExtensionMethods;
using ExportCharacter = DnD4e.LibraryHelper.Character.Character;
using ImportCharacter = DnD4e.LibraryHelper.Import.Character.Character;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
    internal static class CharacterMethods {
        public static ExportCharacter ToCharacter (this ImportCharacter import) {
            System.Diagnostics.Debugger.Break();

            var export = new ExportCharacter() {
                AbilityScores = import.AbilityScores.ToDictionary(),
                ActionPoints = import.Sheet.Stats["_BaseActionPoints"],
                Alignment = import.SafeGetRuleNameByType("Alignment").ToAlignment(),
                Class = import.SafeGetRuleNameByType("Class"),
                Defenses = import.Defenses.ToDictionary(),
                Experience = import.Sheet.Details.Experience.SafeToInt(),
                Handle = import.ToHandle(),
                HealingSurges = import.HealingSurges,
                HitPoints = import.HitPoints,
                Initiative = import.Initiative,
                Languages = import.ToRuleNamesList("Language"),
                Level = import.Level,
                Name = import.Name,
                Race = import.SafeGetRuleNameByType("Race"),
                Role = "Hero", //import.SafeGetRuleNameByType("Role"),
                Skills = import.Skills.ToDictionary(),
                Size = import.SafeGetRuleNameByType("Size"),
                Speed = import.Sheet.Stats["Speed"].Value,
                Vision = import.SafeGetRuleNameByType("Vision")

                //Items = import.Items.Select(i => new KeyValuePair<string, int>(i.Item.Name, i.Quantity)).ToList(),
                //Powers = import.Powers.Select(p => p.ToExportPower()).ToList(),
                //Traits = import.Traits.ToExportTraitList(),
            };

            return export;
        }

        private static Dictionary<AbilityScore, int> ToDictionary (this Character.AbilityScores abilityScores) {
            var output = new Dictionary<AbilityScore, int>();
            for (int i = (int)1; i <= 6; ++i) {
                output[(AbilityScore)i] = abilityScores[(AbilityScore)i];
            }
            return output;
        }

        private static Dictionary<Defense, int> ToDictionary (this Character.Defenses defenses) {
            var output = new Dictionary<Defense, int>();
            for (int i = 1; i <= 4; ++i) {
                output[(Defense)i] = defenses[(Defense)i];
            }
            return output;
        }

        private static Dictionary<Skill, int> ToDictionary (this Character.Skills skills) {
            var output = new Dictionary<Skill, int>();
            for (int i = 1; i <= 17; ++i) {
                output[(Skill)i] = skills[(Skill)i];
            }
            return output;
        }

        public static string ToHandle (this ImportCharacter import) {
            return String.Format("* {0} ({1}{2})", import.Name, import.SafeGetRuleNameByType("Class"), import.Level);
        }

        private static IEnumerable<string> ToRuleNamesList (this ImportCharacter import, string ruleType) {
            Dictionary<string, List<Character.Rule>> rules;
            if (import.Sheet.Rules.ByType().TryGetValue(ruleType, out rules) && rules.Keys.Count > 0) {
                return rules.Keys;
            }
            return new string[0];
        }

        private static string SafeGetRuleNameByType (this ImportCharacter import, string ruleType) {
            Dictionary<string, List<Character.Rule>> rules;
            var output = String.Empty;

            if (import.Sheet.Rules.ByType().TryGetValue(ruleType, out rules) && rules.Count == 1) {
                output = rules.Keys.First();
            }
            return output;
        }
    }
}
