using System;
using System.Collections.Generic;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Trap {
    public class Trap : Combatant {
        public string CompendiumUrl { get; set; }

        public Dictionary<Skill, int> Detect { get; set; }

        public List<string> Immunities { get; set; }

        public string Type { get; set; }

        public Trap () {
            // TODO: construct all collections
            this.Detect = new Dictionary<Skill, int>();
            this.Immunities = new List<string>();
        }
    }
}
