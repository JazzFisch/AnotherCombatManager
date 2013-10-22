using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    internal class TextString : NamedAttributeElement {
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
