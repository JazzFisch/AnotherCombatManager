using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Import.Common;
using DnD4e.LibraryHelper.Import.ExtensionMethods;
using ImportCharacter = DnD4e.LibraryHelper.Import.Character.Character;

namespace DnD4e.LibraryHelper.Character {
    public class Character : Combatant {
        public string Class { get; set; }

        public List<Feat> ClassFeatures { get; set; }

        public override CombatantType CombatantType { get { return Common.CombatantType.Character; } }

        public List<Feat> Feats { get; set; }

        public string Gender { get; set; }

        public int HealingSurges { get; set; }

        public List<Item> Items { get; set; }

        public int PassiveInsight { get; set; }

        public int PassivePerception { get; set; }

        public List<Power> Powers { get; set; }

        public string PowerSource { get; set; }

        public List<Feat> RacialTraits { get; set; }

        public Dictionary<Skill, SkillValue> Skills { get; set; }

        public string Vision { get; set; }

        public Character () {
            // TODO: construct all collections
            this.ClassFeatures = new List<Feat>();
            this.Feats = new List<Feat>();
            this.Powers = new List<Power>();
            this.RacialTraits = new List<Feat>();
            this.Skills = new Dictionary<Skill, SkillValue>();
        }

        internal static async Task<Character> LoadFromFileAsync (string filename, Rules rules) {
            Character character = null;
            Stopwatch timer = Stopwatch.StartNew();
            try {
                string xmlString;
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, useAsync: true)) {
                    using (var reader = new StreamReader(file)) {
                        xmlString = await reader.ReadToEndAsync();
                    }
                }

                // cleanup campaign settings
                // CBuilder seems to insert invalid XML values?
                var endStr = "</D20CampaignSetting>";
                int start = xmlString.IndexOf("<D20CampaignSetting");
                int end = xmlString.IndexOf(endStr) + endStr.Length;
                if (start != -1 && end != -1 && start < end) {
                    xmlString = xmlString.Remove(start, end - start + 1);
                }

                ImportCharacter import = await xmlString.DeserializeXmlAsync<ImportCharacter>();
                character = import.ToCharacter(rules);
            }
            catch (System.Exception ex) {
                Trace.TraceError(ex.ToString());
                System.Diagnostics.Debugger.Break();
            }
            finally {
                timer.Stop();
                Trace.TraceInformation("Deserializing Character [{0}] from CB took {1}ms", Path.GetFileName(filename), timer.ElapsedMilliseconds);
            }

            return character;
        }
    }
}
