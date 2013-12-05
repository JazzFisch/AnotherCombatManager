using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Monster {
    public class MonsterTrait : PowerBase {
        [XmlElement]
        public string Details { get; set; }

        [XmlElement]
        public SimpleValue Range { get; set; }
    }
}
