using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Monster {
    public class Powers {
        [XmlElement("MonsterPower")]
        public List<MonsterPower> MonsterPowers { get; set; }

        [XmlElement("MonsterTrait")]
        public List<MonsterTrait> MonsterTraits { get; set; }
    }
}
