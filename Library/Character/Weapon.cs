using System;
using System.Collections.Generic;
using AnotherCM.Library.Common;

namespace AnotherCM.Library.Character {
    public class Weapon {
        public string Name { get; set; }

        public int AttackBonus { get; set; }

        public AbilityScore AttackStat { get; set; }

        public List<string> Conditions { get; set; }

        public string Damage { get; set; }

        public List<string> DamageComponents { get; set; }

        public string DamageType { get; set; }

        public Defense Defense { get; set; }

        public List<string> HitComponents { get; set; }

        public Weapon () {
        }

        public override string ToString () {
            return this.Name;
        }
    }
}
