using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Stat {
        [XmlElement("alias")]
        public List<TextString> Aliases { get; set; }

        [XmlElement("statadd")]
        public List<Modifier> Modifiers { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

        public Stat () {
            this.Aliases = new List<TextString>();
        }

        [XmlIgnore]
        public string String {
            get {
                if (this.Modifiers.Count > 0) {
                    string mod = this.Modifiers[0].String;
                    if (!String.IsNullOrWhiteSpace(mod)) {
                        return mod;
                    }
                }
                return null;
            }
        }

        public static implicit operator int (Stat stat)
        {
            return stat.Value;
        }

        public override string ToString () {
            StringBuilder sb = new StringBuilder();
            if (this.Aliases.Count > 0)
            {
                sb.Append(this.Aliases[0]);
                sb.Append(" : ");
            }

            if (!String.IsNullOrWhiteSpace(this.String))
            {
                sb.Append(this.String);
            }
            else
            {
                sb.Append(this.Value);
            }
            return sb.ToString();
        }
    }
}
