using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AnotherCM.Library.Common {
    public abstract class Combatant : Renderable {
        public Dictionary<AbilityScore, int> AbilityScores { get; set; }

        public int ActionPoints { get; set; }

        public Alignment Alignment { get; set; }

        public Dictionary<Defense, int> Defenses { get; set; }

        public int Experience { get; set; }

        public int HitPoints { get; set; }

        public int Initiative { get; set; }

        public int Speed { get; set; }

        public List<string> Languages { get; set; }

        [Category("General")]
        public int Level { get; set; }

        [Category("General")]
        public string Race { get; set; }

        [Category("General")]
        public string Role { get; set; }

        [Category("General")]
        public string Size { get; set; }

        public Combatant () {
        }
    }
}
