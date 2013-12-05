using System;
using System.Text;
using AnotherCM.Library.Common;

namespace AnotherCM.Library.ExtensionMethods {
    internal static class CombatantMethods {
        public static string ToHandle (this Combatant combatant) {
            var handle = new StringBuilder();
            handle.AppendFormat(
                "{0} ({1}{2}",
                combatant.Name,
                combatant.Role.Substring(0, 2),
                combatant.Level
            );
            if (combatant is Monster.Monster) {
                var monster = combatant as Monster.Monster;
                if (!String.IsNullOrWhiteSpace(monster.GroupRole)) {
                    handle.AppendFormat("{0}", monster.GroupRole.Substring(0, 1));
                }
            }
            handle.Append(")");

            return handle.ToString();
        }
    }
}
