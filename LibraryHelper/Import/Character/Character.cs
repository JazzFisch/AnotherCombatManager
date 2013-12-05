using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    [XmlRoot("D20Character")]
    public class Character {
        // private so we can track when the serialize changes it
        private Sheet sheet;

        //[XmlAttribute("legality")]
        //public string Legality { get; set; }

        //[XmlElement("Level")]
        //public List<Level> ReferenceInfo { get; set; }

        [XmlElement("CharacterSheet")]
        public Sheet Sheet {
            get { return this.sheet; }
            set { UpdateCharacter(value); }
        }

        public Character () {
            //this.ReferenceInfo = new List<Level>();
            this.sheet = new Sheet();
            this.AbilityScores = new AbilityScores(this.Sheet.Stats);
            this.Defenses = new Defenses(this.Sheet.Stats);
            this.Skills = new Skills(this.Sheet.Stats);
        }

        [XmlIgnore]
        public AbilityScores AbilityScores { get; private set; }

        [XmlIgnore]
        public Defenses Defenses { get; private set; }

        [XmlIgnore]
        public Skills Skills { get; private set; }

        /////////////////////////////////////////////////////////////////////
        // character details
        public int HealingSurges { get { return this.sheet.Stats["Healing Surges"]; } }

        public int HitPoints { get { return this.sheet.Stats["Hit Points"]; } }

        public List<Item> Items { get { return this.sheet.Items; } }

        public int Initiative { get { return this.sheet.Stats["Initiative"]; } }

        public int Level { get { return this.sheet.Stats["Level"]; } }

        public string Name { get { return this.sheet.Details.Name.FixWhitespace(); } }

        public int PassiveInsight { get { return this.sheet.Stats["Passive Insight"]; } }

        public int PassivePerception { get { return this.sheet.Stats["Passive Perception"]; } }

        public List<Power> Powers { get { return this.sheet.Powers; } }

        public string Size { get { return this.sheet.Stats["Size"].String; } }

        public int Speed { get { return this.sheet.Stats["Speed"]; } }

        private void UpdateCharacter (Sheet sheet) {
            this.sheet = sheet;
            if (sheet == null) {
                return;
            }

            this.AbilityScores = new AbilityScores(this.sheet.Stats);
            this.Defenses = new Defenses(this.sheet.Stats);
            this.Skills = new Skills(this.Sheet.Stats);

            // fixup links
            foreach (var stat in this.sheet.Stats) {
                foreach (var mod in stat.Modifiers) {
                    if (String.IsNullOrWhiteSpace(mod.StatLink)) {
                        continue;
                    }

                    mod.Link = this.sheet.Stats[mod.StatLink];
                }
            }

            // fixup rules
            // first, add all the item rules
            var weaponRules = (from i in this.sheet.Items
                               from r in i.Rules
                               select r).Distinct();
            var union = this.sheet.Rules.Union(weaponRules);
            Rules rules = new Rules();
            foreach (var rule in union) {
                rules.Add(rule);
            }
            this.sheet.Rules = rules;

            // next, expand all the weapon rules on powers
            foreach (var power in this.sheet.Powers) {
                foreach (var weapon in power.Weapons) {
                    var ids = (from r in weapon.Rules
                               select r.InternalId).ToArray();
                    weapon.Rules.Clear();
                    foreach (var id in ids) {
                        weapon.Rules.Add(this.sheet.Rules[id]);
                    }
                }
            }
        }
    }
}
