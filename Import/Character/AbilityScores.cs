using System;
using DnD4e.LibraryHelper.Common;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class AbilityScores {
        private Stats stats;

        public AbilityScores (Stats stats) {
            if (stats == null) {
                throw new ArgumentNullException("stats");
            }
            this.stats = stats;
        }

        public int Strength { get { return this.stats["Strength"]; } }

        public int Constitution { get { return this.stats["Constitution"]; } }

        public int Dexterity { get { return this.stats["Dexterity"]; } }

        public int Intelligence { get { return this.stats["Intelligence"]; } }

        public int Wisdom { get { return this.stats["Wisdom"]; } }

        public int Charisma { get { return this.stats["Charisma"]; } }

        public int this[AbilityScore score] {
            get {
                return this.stats[score.ToString()];
            }
        }
    }
}
