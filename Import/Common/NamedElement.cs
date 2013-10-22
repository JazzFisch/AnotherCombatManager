using System;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Import.Common {
    internal interface INamedObject {
        string Name { get; set; }
    }

    internal class NamedAttributeElement : INamedObject {
        private string name;

        [XmlAttribute("name")]
        public string Name {
            get { return this.name; }
            set { this.name = value != null ? value.Trim() : null; }
        }

        public override string ToString () {
            return this.Name;
        }
    }

    internal class NamedValueElement : INamedObject {
        private string name;

        [XmlElement]
        public string Name {
            get { return this.name; }
            set { this.name = value != null ? value.Trim() : null; }
        }

        public override string ToString () {
            return this.Name;
        }
    }
}
