using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Character
{
    public class AbilityScores
    {
        private Stats stats;

        public AbilityScores (Stats stats)
        {
            if (stats == null)
            {
                throw new ArgumentNullException("stats");
            }
            this.stats = stats;
        }

        public int Strength { get { return this.stats["Strength"]; } }
        public int StrengthModifier { get { return this.stats["Strength modifier"]; } }

        public int Constitution { get { return this.stats["Constitution"]; } }
        public int ConstitutionModifier { get { return this.stats["Constitution modifier"]; } }

        public int Dexterity { get { return this.stats["Dexterity"]; } }
        public int DexterityModifier { get { return this.stats["Dexterity modifier"]; } }

        public int Intelligence { get { return this.stats["Intelligence"]; } }
        public int IntelligenceModifier { get { return this.stats["Intelligence modifier"]; } }

        public int Wisdom { get { return this.stats["Wisdom"]; } }
        public int WisdomModifier { get { return this.stats["Wisdom modifier"]; } }

        public int Charisma { get { return this.stats["Charisma"]; } }
        public int CharismaModifier { get { return this.stats["Charisma modifier"]; } }

        public int this[AbilityScore score]
        {
            get
            {
                return this.stats[score.ToString()];
            }
        }
    }
}
