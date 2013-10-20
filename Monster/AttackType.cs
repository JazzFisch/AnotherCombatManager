using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Monster {
    public class AttackType : NamedValueElement {
        [XmlElement]
        public string Action { get; set; }

        [XmlArray("Aftereffects")]
        [XmlArrayItem("MonsterAttackEntry")]
        public List<AttackType> AfterEffects { get; set; }

        [XmlArray]
        [XmlArrayItem("MonsterAttack")]
        public List<Attack> Attacks { get; set; }

        [XmlElement]
        public Damage Damage { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlArray]
        [XmlArrayItem("MonsterAttackEntry")]
        public List<AttackType> FailedSavingThrows { get; set; }

        [XmlArray]
        [XmlArrayItem("MonsterSustainEffect")]
        public List<AttackType> Sustains { get; set; }

        public override string ToString () {
            return String.Format("{0} {1}", this.Damage, this.Description);
        }
    }
}
