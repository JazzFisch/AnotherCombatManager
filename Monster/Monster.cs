using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Import.ExtensionMethods;
using Newtonsoft.Json;
using ImportMonster = DnD4e.LibraryHelper.Import.Monster.Monster;

namespace DnD4e.LibraryHelper.Monster {
    [JsonObject]
    public class Monster : Combatant {
        //public ValueEnumCollection<AbilityScore, AbilityScoreNumber> AbilityScores { get; set; }

        //public ValueList<CalculatedNumber> AttackBonuses { get; set; }

        public string CompendiumUrl { get; set; }

        //public ValueEnumCollection<Defense, SimpleAdjustableNumber> Defenses { get; set; }

        public string Description { get; set; }

        public string GroupRole { get; set; }

        public bool IsLeader { get; set; }

        //public List<ItemAndQuantity> Items { get; set; }

        public List<string> Keywords { get; set; }

        public List<string> Immunities { get; set; }

        public string Origin { get; set; }

        public bool Phasing { get; set; }

        //public Powers PowerReferences { get; set; }

        public int Regeneration { get; set; }

        //public List<Susceptibility> Resistances { get; set; }

        public string Role { get; set; }

        //public List<MonsterSavingThrow> SavingThrows { get; set; }

        //public List<SenseReference> Senses { get; set; }

        //public ValueEnumCollection<Skill, SkillNumber> Skills { get; set; }

        //public List<Speed> Speeds { get; set; }

        public string SourceBook { get; set; }

        public List<string> SourceBooks { get; set; }

        public string Tactics { get; set; }

        public string Type { get; set; }

        //public List<Susceptibility> Weaknesses { get; set; }

        public Monster () {
            // TODO: construct all wrapped objects
        }

        public static bool TryCreateFromFile (string filename, out Monster monster) {
            monster = null;
            try {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
                    using (var xml = new XmlTextReader(fs)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(ImportMonster));
                        if (serializer.CanDeserialize(xml)) {
                            var import = serializer.Deserialize(xml) as ImportMonster;
                            monster = import.ToMonster();
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Trace.WriteLine(ex);
            }

            return false;
        }
    }
}
