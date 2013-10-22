using System;
using System.Text;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Import.Character {
    internal class Modifier {
        [XmlAttribute("abilmod")]
        public bool AbilityModifier { get; set; }

        [XmlAttribute("charelem")]
        public string CharElem { get; set; }

        [XmlAttribute("Level")]
        public int Level { get; set; }

        [XmlAttribute("not-wearing")]
        public string NotWearing { get; set; }

        [XmlAttribute("requires")]
        public string Requires { get; set; }

        [XmlAttribute("statlink")]
        public string StatLink { get; set; }

        [XmlAttribute("String")]
        public string String { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("wearing")]
        public string Wearing { get; set; }

        [XmlIgnore]
        public Stat Link { get; set; }

        public override string ToString () {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(this.StatLink)) {
                sb.Append(this.StatLink);
            }
            else {
                sb.Append(this.Type);
            }
            sb.Append(" : ");
            sb.Append(this.Value);
            return sb.ToString();
        }
    }
}
