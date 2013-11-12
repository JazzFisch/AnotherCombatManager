using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Import.Common;
using DnD4e.LibraryHelper.Import.ExtensionMethods;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Power : NamedAttributeElement {
        [XmlElement("specific")]
        public Specifics Specifics { get; set; }

        [XmlElement("Weapon")]
        public List<Weapon> Weapons { get; set; }

        public override string ToString () {
            return this.Name;
        }

        /////////////////////////////////////////////////////////////////////
        // access helpers
        public ActionType ActionType { get { return SafeGet("Action Type").ToActionType(); } }
        public string Attack { get { return SafeGet("Attack"); } }
        public string Display { get { return SafeGet("Display"); } }
        public string Effect { get { return SafeGet("Effect"); } }
        public string Flavor { get { return SafeGet("Flavor"); } }
        public string Hit { get { return SafeGet("Hit"); } }
        public string[] Keywords { get { return SafeGet("Keywords").Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries); } }
        public PowerUsage PowerUsage { get { return SafeGet("Power Usage").ToPowerUsage(); } }
        public Range Range { get { return CreateRange(); } }
        public string Target { get { return SafeGet("Target"); } }

        public string SafeGet (string key) {
            Specific specific;
            if (!this.Specifics.TryGetValue(key, out specific) || (specific == null) || (String.IsNullOrWhiteSpace(specific.Value))) {
                return String.Empty;
            }

            return specific.Value.Trim();
        }

        private Range CreateRange () {
            Range range = SafeGet("Attack Type").ToRange();
            if (range.AttackType == AttackType.RangedWeapon) {
                Range weaponRange = this.Weapons[0].Range;
                range.Distance = weaponRange.Distance;
                range.MaxDistance = weaponRange.MaxDistance;
            }

            return range;
        }
    }
}
