using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Import.Common {
    internal class ReferencedObject : NamedValueElement {
        [XmlElement]
        public string Description { get; set; }

        // TODO: doesn't really belong this deep
        [XmlElement]
        public string DefenseName { get; set; }

        [XmlElement]
        public string ID { get; set; }

        // TODO: doesn't really belong this deep
        [XmlElement("URL")]
        public string Url { get; set; }
    }

    internal class ReferencedObjectWrapper {
        [XmlElement]
        public ReferencedObject ReferencedObject { get; set; }

        public string Description { get { return this.ReferencedObject.Description; } }

        public string Name { get { return this.ReferencedObject.Name; } }

        public ReferencedObjectWrapper () {
            this.ReferencedObject = new ReferencedObject();
        }

        public override string ToString () {
            return this.ReferencedObject.ToString();
        }
    }

    // to be used when the element has xsi:type="ObjectReference"
    internal class ObjectReference : ReferencedObjectWrapper { }

    internal class DefenseReference : ReferencedObjectWrapper {
        public Defense Defense {
            get {
                Defense def;
                Enum.TryParse(this.ReferencedObject.DefenseName, true, out def);
                return def;
            }
        }
    }
}
