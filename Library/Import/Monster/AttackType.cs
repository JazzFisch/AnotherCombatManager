using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using AnotherCM.Library.Common;
using AnotherCM.Library.Import.Common;
using AnotherCM.Library.Import.ExtensionMethods;

namespace AnotherCM.Library.Import.Monster {
    public class AttackType : NamedValueElement {
        [XmlElement]
        public string Action { get; set; }

        [XmlIgnore]
        public ActionType ActionType {
            get { return this.Action.ToActionType(); }
        }

        [XmlArray("Aftereffects")]
        [XmlArrayItem("MonsterAttackEntry")]
        public List<AttackType> AfterEffects { get; set; }

        [XmlArray]
        [XmlArrayItem("MonsterAttack")]
        public List<Attack> Attacks { get; set; }

        [XmlElement]
        public Damage Damage { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlArray]
        [XmlArrayItem("MonsterAttackEntry")]
        public List<AttackType> FailedSavingThrows { get; set; }

        [XmlArray]
        [XmlArrayItem("MonsterSustainEffect")]
        public List<AttackType> Sustains { get; set; }

        [XmlIgnore]
        public bool IsEmpty {
            get {
                return String.IsNullOrWhiteSpace(this.Action) &&
                       this.AfterEffects.Count == 0 &&
                       this.Attacks.Count == 0 &&
                       this.Damage.IsEmpty && 
                       String.IsNullOrWhiteSpace(this.Description) &&
                       this.FailedSavingThrows.Count == 0 &&
                       this.Sustains.Count == 0;
            }
        }

        public override string ToString () {
            return String.Format("{0} {1}", this.Damage, this.Description);
        }
    }
}
