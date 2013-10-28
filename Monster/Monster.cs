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
        public string CompendiumUrl { get; set; }

        public string Description { get; set; }

        public string GroupRole { get; set; }

        public bool IsLeader { get; set; }

        public List<KeyValuePair<string, int>> Items { get; set; }

        public List<string> Keywords { get; set; }

        public List<string> Immunities { get; set; }

        public string Origin { get; set; }

        public List<string> OtherSpeeds { get; set; }

        public bool Phasing { get; set; }

        public List<Power> Powers { get; set; }

        public int Regeneration { get; set; }

        public List<string> Resistances { get; set; }

        public string Role { get; set; }

        public int SavingThrows { get; set; }

        public List<KeyValuePair<string, int>> Senses { get; set; }

        public string SourceBook { get; set; }

        public List<string> SourceBooks { get; set; }

        public string Tactics { get; set; }

        public List<Trait> Traits { get; set; }

        public string Type { get; set; }

        public List<string> Weaknesses { get; set; }

        public Monster () {
            // TODO: construct all collections
            this.Items = new List<KeyValuePair<string, int>>();
            this.Keywords = new List<string>();
            this.Immunities = new List<string>();
            this.OtherSpeeds = new List<string>();
            this.Powers = new List<Power>();
            this.Resistances = new List<string>();
            this.Senses = new List<KeyValuePair<string, int>>();
            this.Traits = new List<Trait>();
            this.Weaknesses = new List<string>();
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

        public static bool TryCreateFromJson (string json, out Monster monster) {
            monster = null;
            try {
                monster = JsonConvert.DeserializeObject<Monster>(json);
                return true;
            }
            catch (System.Exception ex) {
                Trace.WriteLine(ex);
            }

            return false;
        }
    }
}
