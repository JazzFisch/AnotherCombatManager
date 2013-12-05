using System;
using System.Collections.Generic;
using System.Linq;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.Common;
using ExportCharacter = DnD4e.LibraryHelper.Character.Character;
using ExportClassFeat = DnD4e.LibraryHelper.Character.ClassFeat;
using ExportFeat = DnD4e.LibraryHelper.Character.Feat;
using ExportItem = DnD4e.LibraryHelper.Character.Item;
using ExportPower = DnD4e.LibraryHelper.Character.Power;
using ExportSkillValue = DnD4e.LibraryHelper.Character.SkillValue;
using ExportWeapon = DnD4e.LibraryHelper.Character.Weapon;
using ImportCharacter = DnD4e.LibraryHelper.Import.Character.Character;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
    internal static class CharacterMethods {
        public static ExportCharacter ToCharacter (this ImportCharacter import, Rules d20Rules) {
            Rules rules = d20Rules ?? import.Sheet.Rules;

            var export = new ExportCharacter();
            export.AbilityScores = import.AbilityScores.ToDictionary();
            export.ActionPoints = import.Sheet.Stats["_BaseActionPoints"];
            export.Alignment = import.SafeGetRuleNameByType("Alignment").ToAlignment();
            export.Class = import.SafeGetRuleNameByType("Class");
            export.ClassFeatures = import.Sheet.Rules.ByType()["Class Feature"].ToFeats(rules);
            export.Defenses = import.Defenses.ToDictionary();
            export.Experience = import.Sheet.Details.Experience.SafeToInt();
            export.Feats = import.Sheet.Rules.ByType()["Feat"].ToFeats(rules);
            export.Gender = import.SafeGetRuleNameByType("Gender");
            export.Handle = import.ToHandle();
            export.HealingSurges = import.HealingSurges;
            export.HitPoints = import.HitPoints;
            export.Initiative = import.Initiative;
            export.Items = import.Items.ToItems(rules);
            export.Languages = import.ToRuleNamesList("Language");
            export.Level = import.Level;
            export.Name = import.Name;
            export.PassiveInsight = import.PassiveInsight;
            export.PassivePerception = import.PassivePerception;
            export.Powers = import.Powers.ToPowers(import.Sheet.Rules, rules);
            export.PowerSource = import.SafeGetRuleNameByType("Power Source");
            export.Race = import.SafeGetRuleNameByType("Race");
            export.RacialTraits = import.Sheet.Rules.ByType()["Racial Trait"].ToFeats(rules);
            export.Role = import.SafeGetRuleNameByType("Role");
            export.Skills = import.ToSkills();
            export.Size = import.SafeGetRuleNameByType("Size");
            export.Speed = import.Sheet.Stats["Speed"].Value;
            export.Vision = import.SafeGetRuleNameByType("Vision");

            return export;
        }

        private static Rule CompleteRule (Rule characterRule, Rules d20Rules) {
            if (d20Rules == null) {
                return characterRule;
            }

            Rule d20Rule;
            if (!d20Rules.TryGetValue(characterRule.InternalId, out d20Rule)) {
                return characterRule;
            }

            return d20Rule.Specifics.Count > characterRule.Specifics.Count ? d20Rule : characterRule;
        }

        private static string SafeGetRuleNameByType (this ImportCharacter import, string ruleType) {
            Dictionary<string, List<Rule>> rules;
            var output = String.Empty;

            if (import.Sheet.Rules.ByType().TryGetValue(ruleType, out rules) && rules.Count == 1) {
                output = rules.Keys.First();
            }
            return output;
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

        private static List<ExportFeat> ToFeats (this Dictionary<string, List<Rule>> feats, Rules d20Rules) {
            var output = new List<ExportFeat>();
            foreach (var kvp in feats) {
                var rule = CompleteRule(kvp.Value[0], d20Rules);
                var feat = new ExportFeat() {
                    Name = kvp.Key,
                    ShortDescription = rule.Specifics.SafeGetValue("Short Description").FixWhitespace(),
                    Text = rule.Text.FixWhitespace()
                };
                output.Add(feat);
            }

            return output;
        }

        public static string ToHandle (this ImportCharacter import) {
            return String.Format("* {0} ({1}{2})", import.Name, import.SafeGetRuleNameByType("Class"), import.Level);
        }

        public static List<ExportItem> ToItems (this List<Character.Item> import, Rules d20Rules) {
            List<ExportItem> output = new List<ExportItem>();
            foreach (var item in import) {
                Rule baseRule = CompleteRule(item.Rules[0], d20Rules);
                Rule magicRule = item.Rules.Count > 1 ? CompleteRule(item.Rules[1], d20Rules) : null;
                var export = new ExportItem() {
                    Name = item.Name,
                    Count = item.Count,
                    EquippedCount = item.EquippedCount,
                    ArmorCategory = baseRule.Specifics.SafeGetValue("Armor Category"),
                    ArmorType = baseRule.Specifics.SafeGetValue("Armor Type"),
                    Category = baseRule.Specifics.SafeGetValue("Category"),
                    Flavor = baseRule.Flavor.FixWhitespace(),
                    ItemSlot = baseRule.Specifics.SafeGetValue("Item Slot"),
                    Text = baseRule.Text.FixWhitespace(),
                    Type = baseRule.Type,
                    Weight = baseRule.Specifics.SafeGetValue("Weight"),
                };
                output.Add(export);
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
                Rule rule = CompleteRule(charRule, d20Rules);
                importPower.Specifics = rule.Specifics;

                var power = new ExportPower();
                power.ActionType = importPower.SafeGet("Action Type");
                power.Attack = importPower.Attack;
                power.AttackType = importPower.SafeGet("Attack Type");
                power.Display = importPower.Display.FixWhitespace();
                power.Effect = importPower.Effect.FixWhitespace();
                power.Flavor = (!String.IsNullOrWhiteSpace(importPower.Flavor) ? importPower.Flavor : rule.Flavor).FixWhitespace();
                power.Hit = importPower.Hit.FixWhitespace();
                power.Keywords = importPower.Keywords.ToList();
                power.Name = importPower.Name;
                power.PowerType = importPower.SafeGet("Power Type");
                power.Range = importPower.Range;
                power.Source = rule.Source.FixWhitespace();
                power.Target = importPower.Target.FixWhitespace();
                power.Text = rule.Text.FixWhitespace();
                power.Usage = importPower.SafeGet("Power Usage");
                power.Weapons = importPower.Weapons.Select(w => new ExportWeapon() {
                    AttackBonus = w.AttackBonus,
                    AttackStat = w.AttackStat.ToAbilityScore(),
                    Conditions = w.Conditions.ToStringList(),
                    Damage = w.Damage.Expression,
                    DamageComponents = w.DamageComponents.ToStringList(),
                    DamageType = w.DamageType.FixWhitespace(),
                    Defense = w.Defense.ToDefense(),
                    HitComponents = w.HitComponents.ToStringList(),
                    Name = w.Name,
                }).ToList();
                powers.Add(power);
            }

            return powers;
        }

        private static List<string> ToRuleNamesList (this ImportCharacter import, string ruleType) {
            Dictionary<string, List<Rule>> rules;
            if (import.Sheet.Rules.ByType().TryGetValue(ruleType, out rules) && rules.Keys.Count > 0) {
                return rules.Keys.ToList();
            }
            return new List<string>();
        }

        private static Dictionary<Skill, ExportSkillValue> ToSkills (this ImportCharacter character) {
            var output = new Dictionary<Skill, ExportSkillValue>();
            var skills = character.Skills;
            for (int i = 1; i <= 17; ++i) {
                var skill = (Skill)i;
                var trained = skill.ToString() + " Trained";
                var value = new ExportSkillValue() {
                    Value = skills[(Skill)i],
                    IsTrained = character.Sheet.Stats[trained] != 0
                };
                output[(Skill)i] = value;
            }
            return output;
        }

        public static List<string> ToStringList (this string listString) {
            if (String.IsNullOrWhiteSpace(listString)) {
                return null;
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
    }
}
