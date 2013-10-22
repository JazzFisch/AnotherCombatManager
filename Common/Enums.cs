using System;

namespace DnD4e.LibraryHelper.Import.Common {
    public enum AbilityScore {
        Invalid,
        Strength,
        Constitution,
        Dexterity,
        Intelligence,
        Wisdom,
        Charisma
    }

    public enum ActionType {
        Invalid,
        Free,
        Minor,
        Move,
        Standard,
        Opportunity,
        Reaction,
        Interrupt
    }

    public enum AttackType {
        Invalid,
        Personal,
        Melee,
        MeleeWeapon,
        MeleeTouch,
        Ranged,
        RangedWeapon,
        AreaBlast,
        AreaBurst,
        AreaWall,
        CloseBlast,
        CloseBurst,
        Trait
    }

    public enum Defense {
        Invalid,
        AC,
        Fortitude,
        Reflex,
        Will
    }

    public enum PowerUsage {
        Invalid,
        AtWill,
        Encounter,
        Daily,
        Recharge
    }

    public enum Skill {
        Invalid,
        Acrobatics,
        Arcana,
        Athletics,
        Bluff,
        Diplomacy,
        Dungeoneering,
        Endurance,
        Heal,
        History,
        Insight,
        Intimidate,
        Nature,
        Perception,
        Religion,
        Stealth,
        Streetwise,
        Thievery
    }

    public static class EnumExtensionMethods {
        public static ActionType ToActionType (this string actionTypeString) {
            if (String.IsNullOrWhiteSpace(actionTypeString)) {
                return ActionType.Invalid;
            }
            string[] parts = actionTypeString.Split();
            ActionType type;
            if (!Enum.TryParse(parts[0], true, out type) && parts.Length >= 2) {
                Enum.TryParse(parts[1], true, out type);
            }
            return type;
        }

        public static AttackType ToAttackType (this string attackTypeString) {
            if (String.IsNullOrWhiteSpace(attackTypeString)) {
                return AttackType.Invalid;
            }
            string[] parts = attackTypeString.Split();
            AttackType type;
            if (!Enum.TryParse(parts[0], true, out type) && (parts.Length > 1)) {
                if (!Enum.TryParse(String.Concat(parts[0], parts[1]), true, out type)) {
                    return AttackType.Invalid;
                }
            }
            return type;
        }

        public static PowerUsage ToPowerUsage (this string powerUsageString) {
            if (String.IsNullOrWhiteSpace(powerUsageString)) {
                return PowerUsage.Invalid;
            }

            string[] parts = powerUsageString.Split();
            PowerUsage type;
            if (!Enum.TryParse(parts[0].Replace("-", String.Empty), true, out type) && (parts.Length > 1)) {
                Enum.TryParse(parts[1], true, out type);
            }
            return type;
        }

        public static Range ToRange (this string rangeString) {
            if (String.IsNullOrWhiteSpace(rangeString)) {
                return Range.Invalid;
            }

            Range range = new Range() { Distance = 1.0f };
            string[] parts = rangeString.Split();

            AttackType type;
            string distanceStr = parts[parts.Length - 1];
            string typeStr = parts.Length > 1 ? String.Concat(parts[0], parts[1]) : parts[0];
            if (!Enum.TryParse(typeStr, true, out type)) {
                if (!Enum.TryParse(parts[0], true, out type)) {
                    return Range.Invalid;
                }
                else if (parts.Length >= 2) {
                    distanceStr = parts[1];
                }
            }
            else if (parts.Length >= 3) {
                distanceStr = parts[2];
            }

            float distance;
            if (Single.TryParse(distanceStr, out distance)) {
                range.Distance = distance;
            }

            // final fixup
            range.AttackType = type;
            if (range.AttackType == AttackType.Personal) {
                range.Distance = 0.0f;
            }

            return range;
        }
    }
}
