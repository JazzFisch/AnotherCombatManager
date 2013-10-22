using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    internal class Details : NamedAttributeElement {
        [XmlElement]
        public int Level { get; set; }

        [XmlElement]
        public string Player { get; set; }

        [XmlElement]
        public string Height { get; set; }

        [XmlElement]
        public string Weight { get; set; }

        [XmlElement]
        public string Gender { get; set; }

        [XmlElement]
        public string Age { get; set; }

        [XmlElement]
        public string Alignment { get; set; }

        [XmlElement]
        public string Company { get; set; }

        [XmlElement]
        public string Experience { get; set; }

        [XmlElement]
        public string CarriedMoney { get; set; }

        [XmlElement]
        public string StoredMoney { get; set; }

        [XmlElement]
        public string Appearance { get; set; }

        [XmlElement]
        public string Notes { get; set; }
    }
}
