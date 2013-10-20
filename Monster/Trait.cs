using System;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Monster {
    public class MonsterTrait : PowerBase {
        [XmlElement]
        public string Details { get; set; }

        [XmlElement]
        public SimpleValue Range { get; set; }
    }
}
