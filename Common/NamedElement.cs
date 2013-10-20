using System;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Common {
    public interface INamedObject {
        string Name { get; set; }
    }

    public class NamedAttributeElement : INamedObject {
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

    public class NamedValueElement : INamedObject {
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
