using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AnotherCM.Library.Common;
using AnotherCM.Library.Import.Common;
using ImportAbilityScoreNumber = AnotherCM.Library.Import.Common.AbilityScoreNumber;

namespace AnotherCM.Library.Import.Monster {
    [XmlRoot]
    public class Monster : NamedValueElement {
        [XmlElement]
        public ValueEnumCollection<AbilityScore, ImportAbilityScoreNumber> AbilityScores { get; set; }

        [XmlElement]
        public SimpleValue ActionPoints { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper Alignment { get; set; }

        [XmlElement]
        public ValueList<CalculatedNumber> AttackBonuses { get; set; }

        [XmlElement]
        public string CompendiumUrl { get; set; }

        [XmlElement]
        public ValueEnumCollection<Defense, SimpleAdjustableNumber> Defenses { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement]
        public SimpleValue Experience { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper GroupRole { get; set; }

        [XmlElement]
        public SimpleValue HitPoints { get; set; }

        [XmlElement]
        public SimpleValue Initiative { get; set; }

        [XmlElement]
        public bool IsLeader { get; set; }

        [XmlArray]
        public List<ItemAndQuantity> Items { get; set; }

        [XmlArray]
        [XmlArrayItem("ObjectReference")]
        public List<ReferencedObjectWrapper> Keywords { get; set; }

        [XmlArray]
        [XmlArrayItem("ObjectReference")]
        public List<ReferencedObjectWrapper> Immunities { get; set; }

        [XmlElement]
        public Speed LandSpeed { get; set; }

        [XmlArray]
        [XmlArrayItem("ObjectReference")]
        public List<ReferencedObjectWrapper> Languages { get; set; }

        [XmlElement]
        public int Level { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper Origin { get; set; }

        [XmlElement]
        public bool Phasing { get; set; }

        [XmlElement("Powers")]
        public Powers PowerReferences { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper Race { get; set; }

        [XmlElement]
        public SimpleValue Regeneration { get; set; }

        [XmlArray]
        [XmlArrayItem("CreatureSusceptibility")]
        public List<Susceptibility> Resistances { get; set; }

        [XmlElement]
        public ObjectReference Role { get; set; }

        [XmlArray]
        public List<MonsterSavingThrow> SavingThrows { get; set; }

        [XmlArray]
        public List<SenseReference> Senses { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper Size { get; set; }

        [XmlElement]
        public ValueEnumCollection<Skill, SkillNumber> Skills { get; set; }

        [XmlArray]
        [XmlArrayItem("CreatureSpeed")]
        public List<Speed> Speeds { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper SourceBook { get; set; }

        [XmlArray]
        [XmlArrayItem("ObjectReference")]
        public List<ReferencedObjectWrapper> SourceBooks { get; set; }

        [XmlElement]
        public string Tactics { get; set; }

        [XmlElement]
        public ReferencedObjectWrapper Type { get; set; }

        [XmlArray]
        [XmlArrayItem("CreatureSusceptibility")]
        public List<Susceptibility> Weaknesses { get; set; }

        public Monster () {
            // TODO: construct all wrapped objects
            this.AbilityScores = new ValueEnumCollection<AbilityScore, ImportAbilityScoreNumber>();
            this.AttackBonuses = new ValueList<CalculatedNumber>();
            this.Defenses = new ValueEnumCollection<Defense, SimpleAdjustableNumber>();
            this.Skills = new ValueEnumCollection<Skill, SkillNumber>();
        }

        [XmlIgnore]
        public List<MonsterPower> Powers { get { return this.PowerReferences.MonsterPowers; } }

        [XmlIgnore]
        public List<MonsterTrait> Traits { get { return this.PowerReferences.MonsterTraits; } }

        public override string ToString () {
            return this.Name;
        }

        public static bool TryCreateFromFile (string path, out Monster monster) {
            using (var fs = new FileStream(path, FileMode.Open)) {
                using (var xml = new XmlTextReader(fs)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(Monster));
                    if (serializer.CanDeserialize(xml)) {
                        monster = serializer.Deserialize(xml) as Monster;
                        return true;
                    }
                    else {
                        monster = null;
                        return false;
                    }
                }
            }
        }
    }
}
