using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Import.ExtensionMethods;
using Newtonsoft.Json;
using ImportMonster = DnD4e.LibraryHelper.Import.Monster.Monster;

namespace DnD4e.LibraryHelper.Monster {
    [JsonObject]
    [DefaultProperty("Name")]
    public class Monster : Combatant {
        [Browsable(false)]
        public override CombatantType CombatantType { get { return Common.CombatantType.Monster; } }

        public string CompendiumUrl { get; set; }

        public string Description { get; set; }

        [Category("General")]
        public string GroupRole { get; set; }

        [Category("General")]
        public bool IsLeader { get; set; }

        public List<KeyValuePair<string, int>> Items { get; set; }

        [Category("General")]
        public List<string> Keywords { get; set; }

        public List<string> Immunities { get; set; }

        [Category("General")]
        public string Origin { get; set; }

        public List<string> OtherSpeeds { get; set; }

        public bool Phasing { get; set; }

        public List<Power> Powers { get; set; }

        public Regeration Regeneration { get; set; }

        public List<string> Resistances { get; set; }

        public int SavingThrows { get; set; }

        public List<KeyValuePair<string, int>> Senses { get; set; }

        public Dictionary<Skill, int> Skills { get; set; }

        public string SourceBook { get; set; }

        public List<string> SourceBooks { get; set; }

        public string Tactics { get; set; }

        public List<Trait> Traits { get; set; }

        [Category("General")]
        public string Type { get; set; }

        public List<string> Weaknesses { get; set; }

        public Monster () {
            // TODO: construct all collections
            //this.Items = new List<KeyValuePair<string, int>>();
            //this.Keywords = new List<string>();
            //this.Immunities = new List<string>();
            //this.OtherSpeeds = new List<string>();
            //this.Powers = new List<Power>();
            //this.Resistances = new List<string>();
            //this.Senses = new List<KeyValuePair<string, int>>();
            //this.Skills = new Dictionary<Skill, int>();
            //this.Traits = new List<Trait>();
            //this.Weaknesses = new List<string>();
        }

        public static async Task<Monster> LoadFromFileAsync (string filename) {
            Monster monster = null;
            Stopwatch timer = Stopwatch.StartNew();
            try {
                string xmlString;
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, useAsync: true)) {
                    using (var reader = new StreamReader(file)) {
                        xmlString = await reader.ReadToEndAsync();
                    }
                }

                ImportMonster import = await xmlString.DeserializeXmlAsync<ImportMonster>();
                monster = import.ToMonster();
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
            finally {
                timer.Stop();
                Trace.TraceInformation("Deserializing Monster [{0}] from AT took {1}ms", Path.GetFileName(filename), timer.ElapsedMilliseconds);
            }

            return monster;
        }
    }
}
