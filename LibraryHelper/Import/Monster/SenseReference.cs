using System;
using System.Text;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.ExtensionMethods;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Monster {
    public class SenseReference : ReferencedObjectWrapper {
        [XmlElement]
        public int Range { get; set; }

        public override string ToString () {
            var sb = new StringBuilder();
            sb.Append(this.Name);
            if (this.Range > 0) {
                sb.AppendFormat(" {0}", this.Range);
            }
            return sb.ToString();
        }
    }
}
