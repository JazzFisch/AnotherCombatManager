using System;

namespace DnD4e.LibraryHelper.Common {
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

    public enum Alignment {
        Invalid,
        LawfulGood,
        Good,
        Neutral,
        Unaligned,
        Evil,
        ChaoticEvil
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

    public enum RenderType {
        Invalid,
        Character,
        Monster,
        Encounter,
        CharacterPower,
        MonsterPower
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
}
