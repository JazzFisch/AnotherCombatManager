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
            foreach (var attack in power.Attacks) {
                sb.Append(attack.AttackBonuses.Count == 0 ? "Effect" : "Attack");
                switch (power.ActionType) {
                    case ActionTypeEnum.Interrupt:
                    case ActionTypeEnum.Reaction:
                        sb.AppendFormat("({0})", power.Action);
                        break;

                    default:
                        break;
                }
                sb.Append(": ");

                if (attack.AttackBonuses.Count == 0) {
                    sb.AppendFormatIfNotEmpty("{0}; ", attack.Effect.Description);
                    //Dim dam As Damage = cDamages("cEffect")
                    //output.Append(dam.Text_Out(pow))
                    //sb.Append(attack.Effect.ToText(delimeter));
                    sb.Append(delimeter);
                }
                else {
                    //If sRange <> "" Then output.Append(sRange)
                    //If sRange <> "" And sTarget <> "" Then output.Append(" ")
                    //If sTarget <> "" Then output.Append("(" & sTarget & ")")
                    //If sRange <> "" Or sTarget <> "" Then output.Append("; ")
                    //sb.Append(attack.Range);
                    //sb.AppendIfNotEmpty(attack.Range, "; ");
                    sb.AppendFormatIfNotEmpty("{0}; ", attack.ToString());
                    sb.Append(String.Join(", ", attack.AttackBonuses));
                    //sb.AppendFormatIfNotEmpty("{0}; ", );
                    sb.Append(delimeter);

                    
                }
            }

            return sb.ToString();

            //Else
            //    If sDesc <> "" Then output.Append("; " & sDesc)
            //    output.Append(delim)
            //    Dim types As New ArrayList
            //    types.Add("cHit")
            //    types.Add("cMiss")
            //    types.Add("cEffect")
            //    For Each damkey As String In types
            //        Dim dam As Damage = cDamages(damkey)
            //        If dam.bIsValid Then
            //            output.Append(damkey.Substring(1) & ": ")
            //            output.Append(dam.Text_Out(pow))
            //            output.Append(delim)
            //        End If
            //    Next
            //End If
        }
    }
}
