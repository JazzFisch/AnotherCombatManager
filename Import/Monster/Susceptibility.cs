using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DnD4e.LibraryHelper.Import.Common;

namespace DnD4e.LibraryHelper.Import.Monster {
    internal class Susceptibility : ReferencedObjectWrapper {
        [XmlElement]
        public SimpleValue Amount { get; set; }

        [XmlElement]
        public string Details { get; set; }

        public override string ToString () {
            var sb = new StringBuilder();
            if (this.Amount.Value > 0) {
                sb.AppendFormat("{0} ", this.Amount.Value);
            }

            if (String.IsNullOrWhiteSpace(this.Details)) {
                sb.Append(this.Name);
            }
            else {
                sb.AppendFormat("{0} {1}", this.Name, this.Details);
            }
            return sb.ToString();
        }
    }
}
