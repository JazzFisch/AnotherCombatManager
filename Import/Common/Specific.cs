using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Character {
    public class Specific : NamedAttributeElement {
        private string value;

        [XmlText]
        public string Value {
            get { return this.value; }
            set { this.value = value != null ? value.Trim() : null; }
        }

        public override string ToString () {
            return String.Format("{0} : {1}", this.Name, this.value);
        }
    }
}
