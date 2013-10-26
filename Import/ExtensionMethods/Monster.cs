using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DnD4e.LibraryHelper.Import.Common;
using DnD4e.LibraryHelper.Import.Monster;
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
                Items = import.Items.Select(i => new KeyValuePair<string, int>(i.Item.Name, i.Quantity)).ToList(),
                Keywords = import.Keywords.Select(r => r.Name).ToList(),
                Speed = import.LandSpeed.Value,
                OtherSpeeds = import.Speeds.Select(s => s.ToString()).ToList(),
                Languages = import.Languages.Select(r => r.Name).ToList(),
                Level = import.Level,
                Name = import.Name,
                Origin = import.Origin.Name,
                Phasing = import.Phasing,
                Race = import.Race.Name,
                Resistances = import.Resistances.Select(r => r.ToString()).ToList(),
                Role = import.Role.Name,
                SavingThrows = import.SavingThrows.Count > 0 ? import.SavingThrows[0].Value : 0,
                Senses = import.Senses.Select(s => new KeyValuePair<string, int>(s.Name, s.Range)).ToList(),
                Skills = import.Skills.ToDictionary(),
                Size = import.Size.Name,
                SourceBook = import.SourceBook.Name,
                //Tactics = import.Tactics,
                Type = import.Type.Name,
                Weaknesses = import.Weaknesses.Select(r => r.ToString()).ToList(),
            };

            return export;
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
    }
}
