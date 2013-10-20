using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Monster {
    public class Damage {
        [XmlElement("DamageConstant")]
        public DamageConstant DamageConstant { get; set; }

        [XmlElement("DiceQuantity")]
        public int Dice { get; set; }

        [XmlElement]
        public int DiceSides { get; set; }

        [XmlElement]
        public string Expression { get; set; }

        public int Bonus {
            get {
                return (int)Math.Round(this.DamageConstant.Bonus.Value);
            }
        }

        public override string ToString () {
            return this.Expression;
        }
    }
}
