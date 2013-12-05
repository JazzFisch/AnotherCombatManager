using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AnotherCM.Library.ExtensionMethods;
using AnotherCM.Library.Import.Common;
using AnotherCM.Library.Import.Monster;
using ExportAttack = AnotherCM.Library.Monster.Attack;
using ExportAttackType = AnotherCM.Library.Monster.AttackType;
using ExportMonster = AnotherCM.Library.Monster.Monster;
using ExportPower = AnotherCM.Library.Monster.Power;
using ExportRegeneration = AnotherCM.Library.Monster.Regeration;
using ExportTrait = AnotherCM.Library.Monster.Trait;
using ImportMonster = AnotherCM.Library.Import.Monster.Monster;

namespace AnotherCM.Library.Import.ExtensionMethods {
    internal static class MonsterMethods {
        private static readonly CultureInfo Culture = new CultureInfo("en-US");

        public static ExportMonster ToMonster (this ImportMonster import) {
            var export = new ExportMonster() {
                AbilityScores = import.AbilityScores.ToDictionary(),
                ActionPoints = import.ActionPoints.Value,
                Alignment = import.Alignment.Name.ToAlignment(),
                CompendiumUrl = import.CompendiumUrl,
                Defenses = import.Defenses.ToDictionary(),
                Description = import.Description,
                Experience = import.Experience.Value,
                GroupRole = import.GroupRole.Name,
                Handle = import.ToHandle(),
                HitPoints = import.HitPoints.Value,
                Immunities = import.Immunities.Count != 0 ? import.Immunities.Select(r => r.Name).ToList() : null,
                Initiative = import.Initiative.Value,
                IsLeader = import.IsLeader,
                Items = import.Items.Count != 0 ? import.Items.Select(i => new KeyValuePair<string, int>(i.Item.Name, i.Quantity)).ToList() : null,
                Keywords = import.Keywords.Count != 0 ? import.Keywords.Select(r => r.Name).ToList() : null,
                OtherSpeeds = import.Speeds.Count != 0 ? import.Speeds.Select(s => (s.ToString() ?? String.Empty).Trim()).ToList() : null,
                Languages = import.Languages.Count != 0 ? import.Languages.Select(r => r.Name).ToList() : null,
                Level = import.Level,
                Name = import.Name,
                Origin = import.Origin.Name,
                Phasing = import.Phasing,
                Powers = import.Powers.Select(p => p.ToExportPower()).ToList(),
                Race = import.Race.Name,
                Regeneration = new ExportRegeneration() { Value = import.Regeneration.Value, Details = import.Regeneration.Details },
                Resistances = import.Resistances.Count != 0 ? import.Resistances.Select(r => r.ToString()).ToList() : null,
                Role = import.Role.Name,
                SavingThrows = import.SavingThrows.Count > 0 ? import.SavingThrows[0].Value : 0,
                Senses = import.Senses.Count != 0 ? import.Senses.Select(s => new KeyValuePair<string, int>(s.Name, s.Range)).ToList() : null,
                Skills = import.Skills.ToDictionary(),
                Size = import.Size.Name,
                SourceBook = import.SourceBook.Name,
                Speed = import.LandSpeed.Value,
                Tactics = (import.Tactics ?? String.Empty).FixWhitespace(),
                Traits = import.Traits.ToExportTraitList(),
                Type = import.Type.Name,
                Weaknesses = import.Weaknesses.Count != 0 ? import.Weaknesses.Select(r => r.ToString()).ToList() : null,
            };

            return export;
        }

        private static Dictionary<TEnum, int> ToDictionary<TEnum, TValue> (this ValueEnumCollection<TEnum, TValue> collection)
            where TEnum : struct
            where TValue : SimpleValue 
        {
            if (collection == null || collection.Count == 0) {
                return null;
            }
            var select = collection.Values.Keys.Select(
                key => new KeyValuePair<TEnum, int>(
                    key,
                    collection[key].Value
                )
            );
            return select.ToDictionary(s => s.Key, s => s.Value);
        }

        public static string ToHandle (this ImportMonster import) {
            var handle = new StringBuilder();
            handle.AppendFormat(
                "{0} ({1}{2}",
                import.Name,
                import.Role.Name.Substring(0, 2),
                import.Level
            );
            if (!String.IsNullOrWhiteSpace(import.GroupRole.Name)) {
                handle.AppendFormat("{0}", import.GroupRole.Name.Substring(0, 1));
            }
            handle.Append(")");

            return handle.ToString();
        }

        public static ExportPower ToExportPower (this MonsterPower monsterPower) {
            return new ExportPower() {
                Action = Culture.TextInfo.ToTitleCase(monsterPower.Action ?? String.Empty),
                Attacks = monsterPower.Attacks.Count != 0 ? monsterPower.Attacks.Select(a => a.ToExportAttack()).ToList() : null,
                Flavor = monsterPower.Flavor.FixWhitespace(),
                IsBasic = monsterPower.IsBasic,
                Keywords = monsterPower.Keywords.Count != 0 ? monsterPower.Keywords.Select(k => k.Name).ToList() : null,
                Name = monsterPower.Name.FixWhitespace(),
                Requirements = monsterPower.Requirements.FixWhitespace(),
                Trigger = monsterPower.Trigger.FixWhitespace(),
                Type = Culture.TextInfo.ToTitleCase(monsterPower.Type ?? String.Empty).FixWhitespace(),
                Usage = Culture.TextInfo.ToTitleCase(monsterPower.Usage ?? String.Empty).FixWhitespace(),
                UsageDetails = monsterPower.UsageDetails.FixWhitespace()
            };
        }

        public static ExportAttack ToExportAttack (this Attack monsterAttack) {
            return new ExportAttack() {
                AttackBonuses = monsterAttack.AttackBonuses.ToDictionary(ab => ab.Defense.Defense, ab => ab.Bonus),
                Effect = monsterAttack.Effect.ToExportAttackType(),
                Hit = monsterAttack.Hit.ToExportAttackType(),
                Miss = monsterAttack.Miss.ToExportAttackType(),
                Name = monsterAttack.Name.FixWhitespace(),
                Range = (monsterAttack.RangeString ?? String.Empty).Trim(),
                Targets = monsterAttack.Targets.FixWhitespace()
            };
        }

        public static ExportAttackType ToExportAttackType (this AttackType monsterAttackType) {
            if (monsterAttackType == null || monsterAttackType.IsEmpty) {
                return null;
            }

            return new ExportAttackType() {
                Action = monsterAttackType.Action.FixWhitespace(),
                AfterEffects = monsterAttackType.AfterEffects.Count != 0 ? monsterAttackType.AfterEffects.Select(ae => ae.ToExportAttackType()).ToList() : null,
                Attacks = monsterAttackType.Attacks.Count != 0 ? monsterAttackType.Attacks.Select(a => a.ToExportAttack()).ToList() : null,
                Damage = !String.IsNullOrWhiteSpace(monsterAttackType.Damage.Expression) ? new Library.Common.Damage() {
                    Bonus = monsterAttackType.Damage.Bonus,
                    Dice = monsterAttackType.Damage.Dice,
                    DieSides = monsterAttackType.Damage.DiceSides,
                    Expression = monsterAttackType.Damage.Expression.FixWhitespace()
                } : null,
                Description = monsterAttackType.Description.FixWhitespace(),
                FailedSavingThrows = monsterAttackType.FailedSavingThrows.Count != 0 ? monsterAttackType.FailedSavingThrows.Select(fst => fst.ToExportAttackType()).ToList() : null,
                Name = monsterAttackType.Name,
                Sustains = monsterAttackType.Sustains.Count != 0 ? monsterAttackType.Sustains.Select(s => s.ToExportAttackType()).ToList() : null
            };
        }

        public static List<ExportTrait> ToExportTraitList (this List<MonsterTrait> import) {
            if (import == null || import.Count == 0) {
                return null;
            }
            return import.Select(t => new ExportTrait() { 
                Name = t.Name, 
                Details = t.Details.FixWhitespace(),
                Keywords = t.Keywords.Count != 0 ? t.Keywords.Select(k => k.Name).ToList() : null,
                Range = t.Range.Value 
            }).ToList();
        }
    }
}
