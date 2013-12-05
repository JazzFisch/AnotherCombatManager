using System;
using System.Xml.Serialization;
using AnotherCM.Library.Import.Common;

namespace AnotherCM.Library.Import.Monster {
    public class MonsterTrait : PowerBase {
        [XmlElement]
        public string Details { get; set; }

        [XmlElement]
        public SimpleValue Range { get; set; }
    }
}
