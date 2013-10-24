using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DnD4e.LibraryHelper.Import.Common;
using ExportMonster = DnD4e.LibraryHelper.Monster.Monster;
using ImportMonster = DnD4e.LibraryHelper.Import.Monster.Monster;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
    internal static class MonsterMethods {
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
                Keywords = import.Keywords.Select(r => r.Name).ToList(),
                LandSpeed = import.LandSpeed.Value,
                Languages = import.Languages.Select(r => r.Name).ToList(),
                Level = import.Level,
                Name = import.Name,
                Origin = import.Origin.Name,
                Phasing = import.Phasing,
                Race = import.Race.Name,
                Resistances = import.Resistances.Select(w => new DnD4e.LibraryHelper.Monster.Susceptibility() { 
                    Name = w.Name, Details = w.Details, Value = w.Amount != null ? w.Amount.Value : 0}
                ).ToList(),
                Role = import.Role.Name,
                SavingThrows = import.SavingThrows.Count > 0 ? import.SavingThrows[0].Value : 0,
                Skills = import.Skills.ToDictionary(),
                Size = import.Size.Name,
                SourceBook = import.SourceBook.Name,
                Tactics = import.Tactics,
                Type = import.Type.Name,
                Weaknesses = import.Weaknesses.Select(w => new DnD4e.LibraryHelper.Monster.Susceptibility() { 
                    Name = w.Name, Details = w.Details, Value = w.Amount != null ? w.Amount.Value : 0}
                ).ToList()
            };

            return export;
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

        private static Dictionary<TEnum, int> ToDictionary<TEnum, TValue> (this ValueEnumCollection<TEnum, TValue> collection) 
            where TEnum : struct 
            where TValue : SimpleValue 
        {
            var select = collection.Values.Keys.Select(
                key => new KeyValuePair<TEnum, int>(
                    key,
                    collection[key].Value
                )
            );
            return select.ToDictionary(s => s.Key, s => s.Value);
        }
    }
}
