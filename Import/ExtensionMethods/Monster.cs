using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.ExtensionMethods;
using ExportMonster = DnD4e.LibraryHelper.Monster.Monster;
using ImportMonster = DnD4e.LibraryHelper.Import.Monster.Monster;

namespace DnD4e.LibraryHelper.Import.ExtensionMethods {
    internal static class MonsterMethods {
        public static ExportMonster ToMonster (this ImportMonster import) {
            var export = new ExportMonster() {
                ActionPoints = import.ActionPoints.Value,
                Alignment = import.Alignment.Name.ToAlignment(),
                CompendiumUrl = import.CompendiumUrl,
                Description = import.Description,
                Experience = import.Experience.Value,
                GroupRole = import.GroupRole.Name,
                Handle = import.ToHandle(),
                HitPoints = import.HitPoints.Value,
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
                Role = import.Role.Name,
                Size = import.Size.Name,
                SourceBook = import.SourceBook.Name,
                Tactics = import.Tactics,
                Type = import.Type.Name
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
    }
}
