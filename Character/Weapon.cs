using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Character {
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
            // TODO: construct all collections
            //this.Conditions = new List<string>();
            //this.DamageComponents = new List<string>();
            //this.HitComponents = new List<string>();
        }

        public override string ToString () {
            return this.Name;
        }
    }
}
