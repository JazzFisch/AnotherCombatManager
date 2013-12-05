using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;
using DnD4e.LibraryHelper.Import.ExtensionMethods;
using ActionTypeEnum = DnD4e.LibraryHelper.Common.ActionType;
using PowerUsageEnum = DnD4e.LibraryHelper.Common.PowerUsage;

namespace DnD4e.LibraryHelper.Import.Monster {
    public class MonsterPower : PowerBase {
        private ActionTypeEnum actionType;
        private PowerUsageEnum powerUsage;
        private string action;
        private string usage;

        [XmlElement]
        public string Action {
            get { return this.action; }
            set { this.action = value; this.actionType = value.ToActionType(); }
        }

        [XmlArray]
        [XmlArrayItem("MonsterAttack")]
        public List<Attack> Attacks { get; set; }

        [XmlElement]
        public string Flavor { get; set; }

        [XmlElement]
        public string Requirements { get; set; }

        [XmlElement]
        public string Trigger { get; set; }

        [XmlElement]
        public string Usage {
            get { return this.usage; }
            set { this.usage = value; this.powerUsage = value.ToPowerUsage(); }
        }

        [XmlElement]
        public string UsageDetails { get; set; }

        [XmlIgnore]
        public ActionTypeEnum ActionType { get { return this.actionType; } }

        [XmlIgnore]
        public PowerUsageEnum PowerUsage { get { return this.powerUsage; } }
    }
}
