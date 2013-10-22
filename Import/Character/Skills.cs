using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Character
{
    public class Skills
    {
        private Stats stats;

        public Skills (Stats stats)
        {
            this.stats = stats;
        }

        public int Acrobatics { get { return this.stats["Acrobatics"]; } }

        public int Arcana { get { return this.stats["Arcana"]; } }

        public int Athletics { get { return this.stats["Athletics"]; } }

        public int Bluff { get { return this.stats["Bluff"]; } }

        public int Diplomacy { get { return this.stats["Diplomacy"]; } }

        public int Dungeoneering { get { return this.stats["Dungeoneering"]; } }

        public int Endurance { get { return this.stats["Endurance"]; } }

        public int Heal { get { return this.stats["Heal"]; } }

        public int History { get { return this.stats["History"]; } }

        public int Insight { get { return this.stats["Insight"]; } }

        public int Intimidate { get { return this.stats["Intimidate"]; } }

        public int Nature { get { return this.stats["Nature"]; } }

        public int Perception { get { return this.stats["Perception"]; } }

        public int Religion { get { return this.stats["Religion"]; } }

        public int Stealth { get { return this.stats["Stealth"]; } }

        public int Streetwise { get { return this.stats["Streetwise"]; } }

        public int Thievery { get { return this.stats["Thievery"]; } }

        public int this[Skill skill]
        {
            get
            {
                return this.stats[skill.ToString()];
            }
        }
    }
}
