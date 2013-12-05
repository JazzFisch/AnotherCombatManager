using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using AnotherCM.Library.Import.Common;
using AnotherCM.Library.Import.ExtensionMethods;
using AttackTypeEnum = AnotherCM.Library.Common.AttackType;

namespace AnotherCM.Library.Import.Monster {
    public class PowerBase : NamedValueElement {
        private AttackTypeEnum attackType;
        private string type;

        [XmlElement]
        public bool IsBasic { get; set; }

        [XmlArray]
        [XmlArrayItem("ObjectReference")]
        public List<ReferencedObjectWrapper> Keywords { get; set; }

        [XmlElement]
        public int Tier { get; set; }

        [XmlElement]
        public string Type {
            get { return this.type; }
            set { this.type = value; this.attackType = value.ToAttackType(); }
        }

        [XmlIgnore]
        public AttackTypeEnum AttackType { get { return this.attackType; } }

        public override string ToString () {
            return this.Name;
        }
    }
}
