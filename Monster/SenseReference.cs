using System;
using System.Xml.Serialization;

namespace DnD4e.LibraryHelper.Monster {
    public class SenseReference : ReferencedObjectWrapper {
        [XmlElement]
        public int Range { get; set; }
    }
}
