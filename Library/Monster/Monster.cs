using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AnotherCM.Library.Common;
using AnotherCM.Library.Import.ExtensionMethods;
using Newtonsoft.Json;
using ImportMonster = AnotherCM.Library.Import.Monster.Monster;

namespace AnotherCM.Library.Monster {
    [JsonObject]
    [DefaultProperty("Name")]
    public class Monster : Combatant {
        [Browsable(false)]
        public override RenderType RenderType { get { return Common.RenderType.Monster; } }

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
            finally {
                timer.Stop();
                Trace.TraceInformation("Deserializing Monster [{0}] from AT took {1}ms", Path.GetFileName(filename), timer.ElapsedMilliseconds);
            }

            return monster;
        }
    }
}
