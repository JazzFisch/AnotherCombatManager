using System;
using System.Collections.Generic;
using AnotherCM.Library.Common;

namespace AnotherCM.Library.Monster {
    public class Attack {
        public Dictionary<Defense, int> AttackBonuses { get; set; }

        public AttackType Effect { get; set; }

        public AttackType Hit { get; set; }

        public AttackType Miss { get; set; }

        public string Name { get; set; }

        public string Range { get; set; }

        public string Targets { get; set; }
    }
}
