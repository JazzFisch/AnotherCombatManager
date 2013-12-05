using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Sheet {
        [XmlElement]
        public Details Details { get; set; }

        [XmlArray("StatBlock")]
        public Stats Stats { get; set; }

        [XmlArray("LootTally")]
        [XmlArrayItem("loot")]
        public List<Item> Items { get; set; }

        [XmlArray("PowerStats")]
        [XmlArrayItem("Power")]
        public List<Power> Powers { get; set; }

        [XmlArray("RulesElementTally")]
        [XmlArrayItem("RulesElement")]
        public Rules Rules { get; set; }

        public Sheet () {
            this.Stats = new Stats();
        }
    }
}
