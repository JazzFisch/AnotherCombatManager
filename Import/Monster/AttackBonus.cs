using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Monster {
    public class AttackBonus {
        [XmlAttribute("FinalValue")]
        public int Bonus { get; set; }

        [XmlElement]
        public DefenseReference Defense { get; set; }

        public override string ToString () {
            return String.Format("{0:+#;-#;0} vs. {1}", this.Bonus, this.Defense.Defense);
        }
    }
}
