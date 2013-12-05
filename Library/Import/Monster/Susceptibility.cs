using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using AnotherCM.Library.Import.Common;

namespace AnotherCM.Library.Import.Monster {
    public class Susceptibility : ReferencedObjectWrapper {
        [XmlElement]
        public SimpleValue Amount { get; set; }

        [XmlElement]
        public string Details { get; set; }

        public override string ToString () {
            List<string> strings = new List<string>();
            if (this.Amount.Value > 0) {
                strings.Add(this.Amount.Value.ToString());
            }
            if (!String.IsNullOrWhiteSpace(this.Name)) {
                strings.Add(this.Name);
            }
            if (!String.IsNullOrWhiteSpace(this.Details)) {
                strings.Add(this.Details);
            }
            return String.Join(" ", strings);
        }
    }
}
