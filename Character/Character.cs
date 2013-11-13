using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public List<Feat> Feats { get; set; }

        public int HealingSurges { get; set; }

        //public List<Item> Items { get { return this.sheet.Items; } }

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

        internal static bool TryCreateFromFile (string filename, D20Rules rules, out Character character) {
            character = null;
            try {
                string xmlString;
                using (var text = File.OpenText(filename)) {
                    xmlString = text.ReadToEnd();
                }

                // cleanup campaign settings
                // CBuilder seems to insert invalid XML values?
                var endStr = "</D20CampaignSetting>";
                int start = xmlString.IndexOf("<D20CampaignSetting");
                int end = xmlString.IndexOf(endStr) + endStr.Length;
                if (start != -1 && end != -1 && start < end) {
                    xmlString = xmlString.Remove(start, end - start + 1);
                }

                using (var reader = new StringReader(xmlString)) {
                    using (var xml = new XmlTextReader(reader)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(ImportCharacter));
                        if (serializer.CanDeserialize(xml)) {
                            var import = serializer.Deserialize(xml) as ImportCharacter;
                            character = import.ToCharacter(rules);
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception ex) {
                Trace.WriteLine(ex);
                System.Diagnostics.Debugger.Break();
            }

            return false;
        }
    }
}
