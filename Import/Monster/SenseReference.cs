using System;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Monster {
    internal class SenseReference : ReferencedObjectWrapper {
        [XmlElement]
        public int Range { get; set; }
    }
}
