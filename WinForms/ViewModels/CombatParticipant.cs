using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnotherCM.Library.Common;

namespace AnotherCM.WinForms.ViewModels {
    public class CombatParticipant {

        public CombatParticipant (Combatant combatant) {
            this.Combatant = combatant;
            this.Effects = new ObservableKeyedCollection<int, Effect>(e => e.Key);
        }

        public Combatant Combatant { get; private set; }

        public int CurrentHitPoints { get; set; }

        public ObservableKeyedCollection<int, Effect> Effects { get; private set; }

        public int HitPoints { get { return this.Combatant.HitPoints; } }

        public bool InReserve { get; set; }

        public string Name { get { return this.Combatant.Name; } }

        public int Round { get; set; }

        public RenderType Type { get { return this.Combatant.RenderType; } }
    }

    public class Effect {
        public int Key { get; private set; }

        // Expires (EoNT, SoNT, EoC) ? composite class ?
        // Began (round num) ?
        // Begins (future) ?
    }
}
