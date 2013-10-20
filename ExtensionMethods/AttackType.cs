using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.Monster;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    public static class AttackTypeMethods {
        public static string ToText (this AttackType attack, string delimiter) {
            var sb = new StringBuilder();
            if (attack.IsEmpty) {
                return String.Empty;
            }
            else if (attack.Damage.IsEmpty) {
                sb.Append(attack.Description);
            }
            else {
                sb.AppendIfNotEmpty(attack.ToString());
            }

            for (int i = 0; i < attack.FailedSavingThrows.Count; ++i) {
                var failed = attack.FailedSavingThrows[i];
                sb.AppendFormat("{0}    {1} Failed Saving Throw: {2}", delimiter, (i + 1).ToIteration(), failed); // failed.ToHtml(power));
            }
            foreach (var sustain in attack.Sustains) {
                sb.AppendFormat("{0}    Sustain", delimiter);
                sb.AppendFormatIfNotEmpty(" {0}", sustain.Action);
                sb.AppendFormat(": {0}", sustain.Description); //sustain.ToHtml(power));
            }

            foreach (var attackInner in attack.Attacks) {
                sb.AppendFormat("{0}    {1}: {2}", delimiter, attackInner.Name, attackInner.Effect);
            }

            if (!attack.Damage.IsEmpty) {
                foreach (var after in attack.AfterEffects) {
                    sb.AppendFormat("{0}    Aftereffect: {1}", delimiter, after); //after.ToHtml(power));
                }
            }

            return sb.ToString();
        }
    }
}
