using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Common;

namespace DnD4e.LibraryHelper.Monster {
    public class CreatureSusceptibility : ReferencedObjectWrapper {
        [XmlElement]
        public SimpleValue Amount { get; set; }

        public override string ToString () {
            return String.Format("{0} {1}", Amount.Value, Name);
        }
    }
}
