using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Character {
    public class TextString : NamedAttributeElement {
        [XmlText]
        public string Value { get; set; }

        public override string ToString () {
            if (String.IsNullOrWhiteSpace(this.Value)) {
                return this.Name.Trim();
            }
            else {
                return this.Value.Trim();
            }
        }
    }
}
