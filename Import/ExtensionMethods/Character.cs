using System;
using System.Collections.Generic;
using System.Linq;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.Common;
using ExportCharacter = DnD4e.LibraryHelper.Character.Character;
using ExportPower = DnD4e.LibraryHelper.Character.Power;
using ExportWeapon = DnD4e.LibraryHelper.Character.Weapon;
using ImportCharacter = DnD4e.LibraryHelper.Import.Character.Character;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
    internal static class CharacterMethods {
        public static ExportCharacter ToCharacter (this ImportCharacter import, D20Rules rules) {
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
                Powers = import.Powers.ToPowers(import.Sheet.Rules, rules.Rules),                
                Race = import.SafeGetRuleNameByType("Race"),
                Role = "Hero", //import.SafeGetRuleNameByType("Role"),
                Skills = import.Skills.ToDictionary(),
                Size = import.SafeGetRuleNameByType("Size"),
                Speed = import.Sheet.Stats["Speed"].Value,
                Vision = import.SafeGetRuleNameByType("Vision")

                //Items = import.Items.Select(i => new KeyValuePair<string, int>(i.Item.Name, i.Quantity)).ToList(),
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

        private static List<ExportPower> ToPowers (this List<Character.Power> importPowers, Rules charRules, Rules d20Rules) {
            List<ExportPower> powers = new List<ExportPower>();
            foreach (var importPower in importPowers) {
                Rule charRule;
                if (!charRules.TryGetFirstByTypeAndName("Power", importPower.Name, out charRule)) {
                    continue;
                }

                // use the D20 rules instead of the character file's rules
                Rule d20Rule = d20Rules != null ? d20Rules[charRule.InternalId] : null;
                if (d20Rule != null && d20Rule.Specifics.Count > charRule.Specifics.Count) {
                    charRule = d20Rule;
                }

                importPower.Specifics = charRule.Specifics;

                var power = new ExportPower() {
                    ActionType = importPower.SafeGet("Action Type"),
                    Attack = importPower.Attack,
                    AttackType = importPower.SafeGet("Attack Type"),
                    Display = importPower.Display.FixWhitespace(),
                    Effect = importPower.Effect.FixWhitespace(),
                    Flavor = importPower.Flavor.FixWhitespace(),
                    Hit = importPower.Hit.FixWhitespace(),
                    Keywords = importPower.Keywords.ToList(),
                    Name = importPower.Name,
                    PowerType = importPower.SafeGet("Power Type"),
                    Target = importPower.Target.FixWhitespace(),
                    Text = charRule.Text.FixWhitespace(),
                    Usage = importPower.SafeGet("Power Usage"),
                    Weapons = importPower.Weapons.Select(w => new ExportWeapon() {
                        AttackBonus = w.AttackBonus,
                        AttackStat = w.AttackStat.ToAbilityScore(),
                        Conditions = w.Conditions.ToStringList(),
                        Damage = w.Damage.Expression,
                        DamageComponents = w.DamageComponents.ToStringList(),
                        DamageType = w.DamageType.FixWhitespace(),
                        Defense = w.Defense.ToDefense(),
                        HitComponents = w.HitComponents.ToStringList(),
                        Name = w.Name,
                    }).ToList()
                };
                powers.Add(power);
            }

            return powers;
        }

        public static string ToHandle (this ImportCharacter import) {
            return String.Format("* {0} ({1}{2})", import.Name, import.SafeGetRuleNameByType("Class"), import.Level);
        }

        private static IEnumerable<string> ToRuleNamesList (this ImportCharacter import, string ruleType) {
            Dictionary<string, List<Rule>> rules;
            if (import.Sheet.Rules.ByType().TryGetValue(ruleType, out rules) && rules.Keys.Count > 0) {
                return rules.Keys;
            }
            return new string[0];
        }

        public static List<string> ToStringList (this string listString) {
            if (String.IsNullOrWhiteSpace(listString)) {
                return new List<string>();
            }

            var parts = listString.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<string>();
            foreach (var part in parts) {
                var str = part.Trim();
                if (!String.IsNullOrEmpty(str)) {
                    list.Add(str);
                }
            }
            return list;
        }

        private static string SafeGetRuleNameByType (this ImportCharacter import, string ruleType) {
            Dictionary<string, List<Rule>> rules;
            var output = String.Empty;

            if (import.Sheet.Rules.ByType().TryGetValue(ruleType, out rules) && rules.Count == 1) {
                output = rules.Keys.First();
            }
            return output;
        }
    }
}
