using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Monster {
    public class Damage {
        [XmlElement("DamageConstant")]
        public DamageConstant DamageConstant { get; set; }

        [XmlElement("DiceQuantity")]
        public int Dice { get; set; }

        [XmlElement]
        public int DiceSides { get; set; }

        [XmlElement]
        public string Expression { get; set; }

        [XmlIgnore]
        public int Bonus {
            get {
                return (int)Math.Round(this.DamageConstant.Bonus.Value);
            }
        }

        [XmlIgnore]
        public bool IsEmpty {
            get {
                return String.IsNullOrWhiteSpace(this.Expression);
            }
        }

        public override string ToString () {
            return this.Expression;
        }
    }
}
