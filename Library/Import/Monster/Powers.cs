using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using AnotherCM.Library.Import.Common;

namespace AnotherCM.Library.Import.Monster {
    public class Powers {
        [XmlElement("MonsterPower")]
        public List<MonsterPower> MonsterPowers { get; set; }

        [XmlElement("MonsterTrait")]
        public List<MonsterTrait> MonsterTraits { get; set; }
    }
}
