using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.Common;
using DnD4e.LibraryHelper.Import.Monster;
using ExportMonster = DnD4e.LibraryHelper.Monster.Monster;
using ImportMonster = DnD4e.LibraryHelper.Import.Monster.Monster;
using ExportAttack = DnD4e.LibraryHelper.Monster.Attack;
using ExportAttackType = DnD4e.LibraryHelper.Monster.AttackType;
using ExportPower = DnD4e.LibraryHelper.Monster.Power;
using ExportRegeneration = DnD4e.LibraryHelper.Monster.Regeration;
using ExportTrait = DnD4e.LibraryHelper.Monster.Trait;
using System.Globalization;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
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
                Immunities = import.Immunities.Select(r => r.Name).ToList(),
                Initiative = import.Initiative.Value,
                IsLeader = import.IsLeader,
                Items = import.Items.Select(i => new KeyValuePair<string, int>(i.Item.Name, i.Quantity)).ToList(),
                Keywords = import.Keywords.Select(r => r.Name).ToList(),
                OtherSpeeds = import.Speeds.Select(s => (s.ToString() ?? String.Empty).Trim()).ToList(),
                Languages = import.Languages.Select(r => r.Name).ToList(),
                Level = import.Level,
                Name = import.Name,
                Origin = import.Origin.Name,
                Phasing = import.Phasing,
                Powers = import.Powers.Select(p => p.ToExportPower()).ToList(),
                Race = import.Race.Name,
                Regeneration = new ExportRegeneration() { Value = import.Regeneration.Value, Details = import.Regeneration.Details },
                Resistances = import.Resistances.Select(r => r.ToString()).ToList(),
                Role = import.Role.Name,
                SavingThrows = import.SavingThrows.Count > 0 ? import.SavingThrows[0].Value : 0,
                Senses = import.Senses.Select(s => new KeyValuePair<string, int>(s.Name, s.Range)).ToList(),
                Skills = import.Skills.ToDictionary(),
                Size = import.Size.Name,
                SourceBook = import.SourceBook.Name,
                Speed = import.LandSpeed.Value,
                Tactics = (import.Tactics ?? String.Empty).FixWhitespace(),
                Traits = import.Traits.ToExportTraitList(),
                Type = import.Type.Name,
                Weaknesses = import.Weaknesses.Select(r => r.ToString()).ToList(),
            };

            return export;
        }

        private static Dictionary<TEnum, int> ToDictionary<TEnum, TValue> (this ValueEnumCollection<TEnum, TValue> collection)
            where TEnum : struct
            where TValue : SimpleValue {
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
                Attacks = monsterPower.Attacks.Select(a => a.ToExportAttack()).ToList(),
                Flavor = monsterPower.Flavor.FixWhitespace(),
                IsBasic = monsterPower.IsBasic,
                Keywords = monsterPower.Keywords.Select(k => k.Name).ToList(),
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
            return new ExportAttackType() {
                Action = monsterAttackType.Action.FixWhitespace(),
                AfterEffects = monsterAttackType.AfterEffects.Select(ae => ae.ToExportAttackType()).ToList(),
                Attacks = monsterAttackType.Attacks.Select(a => a.ToExportAttack()).ToList(),
                Damage = monsterAttackType.Damage.Expression.FixWhitespace(),
                Description = monsterAttackType.Description.FixWhitespace(),
                FailedSavingThrows = monsterAttackType.FailedSavingThrows.Select(fst => fst.ToExportAttackType()).ToList(),
                IsEmpty = monsterAttackType.IsEmpty,
                Name = monsterAttackType.Name,
                Sustains = monsterAttackType.Sustains.Select(s => s.ToExportAttackType()).ToList()
            };
        }

        public static List<ExportTrait> ToExportTraitList (this List<MonsterTrait> import) {
            return import.Select(t => new ExportTrait() { 
                Name = t.Name, 
                Details = t.Details.FixWhitespace(),
                Keywords = t.Keywords.Select(k => k.Name).ToList(),
                Range = t.Range.Value 
            }).ToList();
        }
    }
}
