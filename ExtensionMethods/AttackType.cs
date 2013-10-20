using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.Monster;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    public static class AttackTypeMethods {
        public static string ToText (this AttackType attack, string delimiter) {
            if (attack.IsEmpty) {
                return String.Empty;
            }
            else if (attack.Damage.IsEmpty) {
                return attack.Description;
            }

            var sb = new StringBuilder();
            sb.AppendIfNotEmpty(attack.ToString());
            for (int i = 0; i < attack.FailedSavingThrows.Count; ++i) {
                var failed = attack.FailedSavingThrows[i];
                sb.AppendFormat("    {0} Failed Saving Throw: {1}{2}", (i + 1).ToIteration(), failed.Description, delimiter); // failed.ToHtml(power));
            }
            foreach (var after in attack.AfterEffects) {
                sb.AppendFormat("    After Effect: {0}{1}", after.Description, delimiter); //after.ToHtml(power));
            }
            foreach (var sustain in attack.Sustains) {
                sb.AppendFormat("    Sustain: {0}{1}", sustain.Description, delimiter); //sustain.ToHtml(power));
            }

            return sb.ToString();
        }
    }
}
