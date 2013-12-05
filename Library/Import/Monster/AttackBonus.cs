using System;
using System.Xml.Serialization;
using AnotherCM.Library.Import.Common;

namespace AnotherCM.Library.Import.Monster {
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
