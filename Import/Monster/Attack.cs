using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Import.Common;
using DnD4e.LibraryHelper.Import.ExtensionMethods;

namespace DnD4e.LibraryHelper.Import.Monster {
    public class Attack : NamedValueElement {
        [XmlArray]
        [XmlArrayItem("MonsterPowerAttackNumber")]
        public List<AttackBonus> AttackBonuses { get; set; }

        [XmlElement]
        public AttackType Effect { get; set; }

        [XmlElement]
        public AttackType Hit { get; set; }

        [XmlElement]
        public AttackType Miss { get; set; }

        [XmlElement("Range")]
        public string RangeString { get; set; }

        [XmlElement]
        public string Targets { get; set; }

        [XmlIgnore]
        public Range Range { get { return this.RangeString.ToRange(); } }

        public override string ToString () {
            if (String.IsNullOrWhiteSpace(this.RangeString) || String.IsNullOrWhiteSpace(this.Targets)) {
                return this.Name;
            }
            else {
                return String.Format("{0} ({1})", this.RangeString, this.Targets);
            }
        }
    }
}
