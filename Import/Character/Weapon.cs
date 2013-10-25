using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Weapon : NamedAttributeElement {
        [XmlElement("RulesElement")]
        public Rules Rules { get; set; }

        [XmlElement]
        public int AttackBonus { get; set; }

        [XmlElement]
        public string AttackState { get; set; }

        [XmlElement]
        public string Defense { get; set; }

        [XmlElement]
        public Damage Damage { get; set; }

        [XmlElement]
        public string CritRange { get; set; }

        [XmlElement]
        public Damage CritDamage { get; set; }

        [XmlIgnore]
        public Range Range { get { return CreateRange(); } }

        private Range CreateRange () {
            Specific specific;
            Range range = new Range { Distance = 1.0f };
            if (!this.Rules[0].Specifics.TryGetValue("Range", out specific)) {
                return range;
            }

            string weaponRange = specific.Value;
            string[] parts = weaponRange.Trim().Split('/');

            float distance, max = 0;
            Single.TryParse(parts[0], out distance);
            if (parts.Length > 1) {
                Single.TryParse(parts[1], out max);
            }
            range.Distance = distance;
            range.MaxDistance = max;

            return range;
        }
    }
}
