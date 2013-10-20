using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnD4e.LibraryHelper.Monster;
using ActionTypeEnum = DnD4e.LibraryHelper.Common.ActionType;
using PowerUsageEnum = DnD4e.LibraryHelper.Common.PowerUsage;

namespace DnD4e.LibraryHelper.ExtensionMethods {
    public static class MonsterPowerMethods {
        public static string ToText (this MonsterPower power, string delimeter) {
            //attack.text_out(tempPow, "###")
            var sb = new StringBuilder();
            sb.AppendFormatIfNotEmpty("Requirements: {0}{1}", power.Requirements, delimeter);
            sb.AppendFormatIfNotEmpty("Trigger: {0}{1}", power.Trigger, delimeter);

            foreach (var attack in power.Attacks) {
                sb.Append(attack.AttackBonuses.Count == 0 ? "Effect" : "Attack");
                switch (power.ActionType) {
                    case ActionTypeEnum.Interrupt:
                    case ActionTypeEnum.Reaction:
                        sb.AppendFormat(" ({0})", power.Action);
                        break;

                    default:
                        break;
                }
                sb.Append(": ");

                if (attack.AttackBonuses.Count == 0) {
                    sb.Append(attack.Effect.ToText(delimeter));
                    sb.Append(delimeter);
                }
                else {
                    sb.AppendFormatIfNotEmpty("{0}; ", attack.ToString());
                    sb.Append(String.Join(", ", attack.AttackBonuses));
                    sb.Append(delimeter);
                    sb.AppendFormatIfNotEmpty("Hit: {0}{1}", attack.Hit.ToText(delimeter), delimeter);
                    sb.AppendFormatIfNotEmpty("Miss: {0}{1}", attack.Miss.ToText(delimeter), delimeter);
                    sb.AppendFormatIfNotEmpty("Effect: {0}{1}", attack.Effect.ToText(delimeter), delimeter);
                }
            }

            return sb.ToString();
        }
    }
}
